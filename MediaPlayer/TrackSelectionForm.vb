'09.07.19
Imports System.IO
Public Class TrackSelectionForm

    Shared dll As New Class1

    Public selTracks As List(Of Track)
    Public multiSelect As Boolean

    Public currFolder As Folder
    Dim currTracks As List(Of Track)

    Dim currLevel As Integer = 0
    Public Shared instances As List(Of TrackSelectionForm)

    Dim arguments() As String
    Dim mode As eTrackSelectionMode
    Dim changeOccured As Boolean = False
    Dim searchState As eSearchState
    Dim searchSource As Boolean
    Dim searchChecked As New List(Of Track)
    Dim playOnClick As Boolean
    Dim sortMode As Form1.sortMode
    Dim reverseSort As Boolean

    ReadOnly Property args As List(Of String)
        Get
            If arguments Is Nothing Then Return New List(Of String)
            Return arguments.ToList()
        End Get
    End Property
    ReadOnly Property playlistPath() As String
        Get
            Return Form1.playlistPath
        End Get
    End Property

    Enum eTrackSelectionMode
        SELECTION
        MANAGE
    End Enum

    Private Sub TrackSelectionForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Size = New Size(567, 440)
        Me.Location = New Point(Form1.Left + Form1.Width / 2 - Me.Width / 2, Form1.Top + Form1.Height / 2 - Me.Height / 2)
        colorMe()
        selTracks = New List(Of Track)
        If instances Is Nothing Then
            instances = New List(Of TrackSelectionForm)
        End If
        instances.Add(Me)
        currLevel = instances.Count
        searchSource = dll.iniReadValue("Config", "searchSource", 0, Form1.inipath)
        playOnClick = dll.iniReadValue("Config", "playOnClick", 0, Form1.inipath)
        checkPlayOnClick.Checked = playOnClick
        checkPlayOnClick.BringToFront()
        sortCombo.SelectedIndex = sortMode / 2
        sortRevButton.Text = IIf(sortMode Mod 2 = 0, "↑", "↓")
    End Sub


    Sub colorMe() '06.08.19
        Dim inverted As Boolean = dll.iniReadValue("Config", "invColors", Form1.inipath)
        Dim lightCol As Color = IIf(inverted, Color.FromArgb(50, 50, 50), Color.White)
        Dim darkCol As Color = IIf(inverted, Color.FromArgb(20, 20, 20), Color.FromArgb(255, 240, 240, 240))

        Dim invLightCol As Color = IIf(Not inverted, Color.Black, Color.White)
        Dim invDarkCol As Color = IIf(Not inverted, Color.Black, Color.FromArgb(255, 240, 240, 240))
        tSearch.ForeColor = IIf(tSearch.Text = "Search...", Color.DimGray, IIf(inverted, Color.White, Color.Black))
        Dim elements As New List(Of Control)
        elements.Add(Me)
        For Each c As Control In Me.Controls
            elements.Add(c)
            For Each subControl As Control In c.Controls
                elements.Add(subControl)
                For Each subSubControl As Control In subControl.Controls
                    elements.Add(subSubControl)
                    For Each subSubSubControl As Control In subSubControl.Controls
                        elements.Add(subSubSubControl)
                    Next
                Next
            Next
        Next

        For Each c As Control In elements
            If TypeOf c Is ListBox Then
                c.BackColor = lightCol
                c.ForeColor = invLightCol
            ElseIf TypeOf c Is Button Or TypeOf c Is textbox Then
                c.BackColor = lightCol
                c.ForeColor = invLightCol
            Else
                c.BackColor = darkCol
                c.ForeColor = invDarkCol
            End If

        Next
    End Sub

    Public Shared Function selectTracks(folder As Folder, mode As eTrackSelectionMode, Optional title As String = "", Optional args() As String = Nothing) As List(Of Track)
        Return selectTracks(folder, folder.tracks, mode, title, args)
    End Function

    Public Shared Function selectTracks(folder As Folder, tracks As List(Of Track), mode As eTrackSelectionMode, Optional title As String = "", Optional args() As String = Nothing) As List(Of Track)
        Dim newForm As New TrackSelectionForm

        OptionsForm.TopMost = False
        If instances IsNot Nothing Then
            For Each i As TrackSelectionForm In instances
                i.TopMost = False
            Next
        End If

        newForm.currFolder = folder
        newForm.arguments = args
        newForm.mode = mode
        newForm.Text = title
        newForm.currTracks = tracks
        newForm.sortMode = Form1.trackSort
        newForm.reverseSort = newForm.dll.getBinaryComponents(Form1.trackSort).Contains(Form1.sortMode.REVERSE)

        newForm.refreshList()
        newForm.refreshButtons()
        newForm.ShowDialog()
        Return newForm.selTracks
    End Function


    Private Sub okButton2_Click(sender As Object, e As EventArgs) Handles okButton2.Click
        If searchState > eSearchState.NONE Then
            cancelSearch()
        Else
            selTracks = getCheckedTracks()
            Close()
        End If
    End Sub
    Private Sub cancelButton2_Click(sender As Object, e As EventArgs) Handles cancelButton2.Click
        If searchState > eSearchState.NONE Then
            cancelSearch()
        Else
            selTracks = New List(Of Track)
            Close()
        End If
    End Sub

    Private Sub TrackSelectionForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        instances.Remove(Me)
        If instances.Count = 0 And changeOccured Then
            OptionsForm.updateTvAfter = True
            If Not args.Contains("errors") Then 'loop in invalidateFolderTracks()
                Form1.localfill()
            End If
        End If
    End Sub

    Private Sub addButton_Click(sender As Object, e As EventArgs) Handles addButton.Click
        Dim selFolder As Folder = NodeSelectionForm.selectNode("Source Playlist")
        If selFolder IsNot Nothing Then
            Dim tracks As List(Of Track) = selectTracks(selFolder, eTrackSelectionMode.SELECTION, "Select Tracks to add...", arguments)
            Dim trackPaths() As String = Nothing
            For i = 0 To tracks.Count - 1
                dll.ExtendArray(trackPaths, tracks(i).fullPath)
            Next
            addTracks(trackPaths)
        End If
    End Sub

    Private Sub addExternalButton_Click(sender As Object, e As EventArgs) Handles addExternalButton.Click
        addTracks(Form1.getAudioFilesDialog(Folder.top.fullPath))
    End Sub

    Sub addTracks(sourceFiles() As String) 'target folder is var currFolder
        If sourceFiles IsNot Nothing Then
            Dim fails As Integer = 0
            refreshDisable = True
            Cursor = Cursors.WaitCursor
            Dim newTrack As Track = Nothing
            If currFolder.isVirtual Then
                For Each s As String In sourceFiles
                    newTrack = New Track(Form1, s, True, currFolder.fullPath & s.Substring(s.LastIndexOf("\") + 1))
                    If Not currFolder.containsTrack(newTrack) Then
                        dll.iniWriteValue(currFolder.fullPath, newTrack.name, newTrack.fullPath, playlistPath)
                        currFolder.tracks.Add(newTrack)
                    End If
                Next
            Else
                For Each s As String In sourceFiles
                    Try
                        Dim newPath As String = currFolder.fullPath & s.Substring(s.LastIndexOf("\") + 1)
                        If Not IO.File.Exists(newPath) Then
                            newTrack = New Track(Form1, newPath, False)
                            IO.File.Copy(s, newPath)
                            currFolder.tracks.Add(newTrack)
                        End If
                    Catch ex As Exception
                        fails += 1
                    End Try

                Next
            End If

            refreshDisable = False
            Cursor = Cursors.Default
            refreshList()
            If fails > 0 Then
                MsgBox("Failed to copy " & fails & " out of " & sourceFiles.Count & " tracks.", MsgBoxStyle.Exclamation)
            End If
            refreshButtons()
        End If
    End Sub

    Private Sub removeButton_Click(sender As Object, e As EventArgs) Handles removeButton.Click
        If searchState > eSearchState.NONE Then
            cancelSearch()
        Else
            Dim checked As List(Of ListViewItem) = getCheckedItems()
            If checked.Count >= 1 Then
                Dim tracks As List(Of Track) = getCheckedTracks()
                Dim fails As Integer = 0
                If Not args.Contains("folder") OrElse tracks.Count > 0 AndAlso MsgBox("Are you sure to delete the " & tracks.Count & " selected files permanently?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    For i = 0 To tracks.Count - 1
                        Dim curr As Track = tracks(i)
                        If args.Contains("folder") Then
                            Try
                                IO.File.Delete(curr.fullPath)
                                listTrackSelection.Items.Remove(checked(i))
                            Catch ex As Exception
                                fails += 1
                            End Try
                        Else
                            currFolder.tracks.Remove(curr)
                            dll.iniDeleteKey(curr.path, curr.name, playlistPath)
                            listTrackSelection.Items.Remove(checked(i))
                        End If
                        changeOccured = True
                    Next
                    labelItemCount.Text = listTrackSelection.Items.Count
                End If
                refreshButtons()
                If fails > 0 Then
                    MsgBox("Failed to delete " & fails & " out of " & tracks.Count & " tracks.", MsgBoxStyle.Exclamation)
                End If
            End If
        End If
    End Sub

    Private Sub changeSourceButton_Click(sender As Object, e As EventArgs) Handles changeSourceButton.Click
        If searchState > eSearchState.NONE Then
            cancelSearch()
        Else
            Dim checked As List(Of ListViewItem) = getCheckedItems()
            Dim checkedTracks As List(Of Track) = getCheckedTracks()
            If checked.Count = 1 Then
                Dim currTrack As Track = checkedTracks(0)
                Dim newSource As String = Form1.getExactFileDialog(currTrack.name, currTrack.ext, currTrack.path)
                If IO.File.Exists(newSource) And dll.hasAudioExt(newSource) Then
                    dll.iniWriteValue(currTrack.path, currTrack.name, newSource, playlistPath)
                    currTrack.fullPath = newSource
                    currFolder = currFolder
                    checked(0).SubItems(2).Text = newSource
                    changeOccured = True
                    refreshList()
                Else

                End If
            ElseIf checked.Count > 1 Then
                Dim newSource As String = Form1.getDirectoryDialog()
                If IO.Directory.Exists(newSource) Then
                    For i = 0 To checkedTracks.Count - 1
                        Dim curr As Track = checkedTracks(i)
                        Dim newSourcePath As String = newSource & curr.name & curr.ext
                        dll.iniWriteValue(curr.path, curr.name, newSourcePath, playlistPath)
                        curr.fullPath = newSourcePath
                        checked(i).SubItems(2).Text = newSourcePath
                        If IO.File.Exists(newSourcePath) Then


                        Else

                        End If
                        changeOccured = True
                    Next
                    refreshList()
                End If
            End If
            refreshButtons()
        End If
    End Sub

    Sub refreshList(Optional rememberChecked As Boolean = False)
        Cursor = Cursors.WaitCursor
        If rememberChecked Then refreshCheckedTracks()
        listTrackSelection.Items.Clear()
        listTrackSelection.BeginUpdate()
        refreshDisable = True

        currTracks = Form1.sortTracks(currTracks, sortMode)
        For Each t As Track In currTracks
            Dim item As ListViewItem = addListRow(t)

            If args.Contains("error") OrElse Not IO.File.Exists(t.fullPath) Then
                checkAll.Checked = args.Contains("error")
                item.BackColor = Color.Red
                item.Checked = True
            End If
            If rememberChecked Then
                If Not Array.TrueForAll(searchChecked.ToArray, Function(x As Track)
                                                                   Return x.name <> t.name
                                                               End Function) Then
                    item.Checked = True
                End If
            End If

        Next
        refreshDisable = False
        listTrackSelection.EndUpdate()
        Cursor = Cursors.Default
        labelItemCount.Text = listTrackSelection.Items.Count
    End Sub

    Function addListRow(t As Track) As ListViewItem
        Dim item As ListViewItem = listTrackSelection.Items.Add(t.name)  'name
        Select Case sortMode + CInt(reverseSort)
            Case Form1.sortMode.DATE_ADDED
                listTrackSelection.Items.Item(listTrackSelection.Items.Count - 1).SubItems.Add(t.dateString)
            Case Form1.sortMode.TIME_LISTENED
                listTrackSelection.Items.Item(listTrackSelection.Items.Count - 1).SubItems.Add(dll.SecondsTodhmsString(t.count * t.length))
            Case Form1.sortMode.COUNT
                listTrackSelection.Items.Item(listTrackSelection.Items.Count - 1).SubItems.Add(t.count)
            Case Form1.sortMode.LENGTH
                listTrackSelection.Items.Item(listTrackSelection.Items.Count - 1).SubItems.Add(dll.secondsTo_ms_Format(t.length))
            Case Form1.sortMode.POPULARITY
                listTrackSelection.Items.Item(listTrackSelection.Items.Count - 1).SubItems.Add(dll.SecondsTohmsString(t.popularity))
            Case Else
                listTrackSelection.Items.Item(listTrackSelection.Items.Count - 1).SubItems.Add("")
        End Select
        listTrackSelection.Items.Item(listTrackSelection.Items.Count - 1).SubItems.Add(t.fullPath) 'source
        Return item
    End Function

    Function getCheckedItems() As List(Of ListViewItem)
        Dim res As New List(Of ListViewItem)
        For i = 0 To listTrackSelection.Items.Count - 1
            Dim item As ListViewItem = listTrackSelection.Items(i)
            If item IsNot Nothing Then
                If item.Checked Then
                    res.Add(item)
                End If
            End If
        Next
        Return res
    End Function

    Function getCheckedTracks() As List(Of Track)
        Dim res As New List(Of Track)
        Dim checked As List(Of ListViewItem) = getCheckedItems()
        For i = 0 To checked.Count - 1
            For j = 0 To currTracks.Count - 1
                If checked(i).Text.ToLower = currTracks(j).name.ToLower Then
                    res.Add(currTracks(j))
                    Exit For
                End If
            Next
        Next
        Return res
    End Function

    Sub refreshButtons()
        If refreshDisable Then Return
        infoButton.Enabled = False
        okButton2.Enabled = False
        cancelButton2.Enabled = False
        changeSourceButton.Enabled = False
        addButton.Enabled = False
        removeButton.Enabled = False
        addExternalButton.Enabled = False
        playButton.Enabled = False
        Dim checkedCount As Integer = 0
        checkedCount = getCheckedItems().Count
        infoButton.Enabled = checkedCount = 1
        playButton.Enabled = checkedCount = 1
        If mode = eTrackSelectionMode.SELECTION Then
            okButton2.Enabled = True
            cancelButton2.Enabled = True
        Else
            okButton2.Enabled = True
            cancelButton2.Enabled = True
            addButton.Enabled = Not args.Contains("error")
            addExternalButton.Enabled = Not args.Contains("error")
            removeButton.Enabled = checkedCount > 0
            changeSourceButton.Enabled = checkedCount > 0 And Not args.Contains("folder")

        End If
    End Sub

    Private Sub listTrackSelection_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles listTrackSelection.ItemChecked
        refreshButtons()
    End Sub

    Private Sub checkAll_CheckedChanged(sender As Object, e As EventArgs) Handles checkAll.CheckedChanged
        refreshButtons()
    End Sub

    Dim refreshDisable As Boolean = False
    Private Sub checkAll_Click(sender As Object, e As EventArgs) Handles checkAll.Click
        refreshDisable = True
        For Each it As ListViewItem In listTrackSelection.Items
            it.Checked = checkAll.Checked
        Next
        refreshDisable = False
        refreshButtons()
    End Sub

    Private Sub infoButton_Click(sender As Object, e As EventArgs) Handles infoButton.Click
        If searchState > eSearchState.NONE Then
            cancelSearch()
        Else
            Dim checked As List(Of Track) = getCheckedTracks()
            If checked Is Nothing Then Return
            Dim t As Track = checked(0)
            Dim locs As String = ""
            For Each f As Folder In t.getLocations(True)
                locs &= f.fullPath & vbNewLine
            Next
            t.invalidateStats()
            MsgBox("Source file: " & IIf(IO.File.Exists(t.fullPath), "Valid", "Invalid") & vbNewLine & t.fullPath & vbNewLine & vbNewLine &
                   "Virtual Location: " & t.isVirtual.ToString & vbNewLine & vbNewLine &
                   "Locations:" & vbNewLine & locs & vbNewLine &
                   "Added: " & vbNewLine & t.dateString() & vbNewLine & vbNewLine &
                   "Count:" & vbNewLine & t.count & vbNewLine & vbNewLine &
                   "Length:" & vbNewLine & dll.secondsTo_ms_Format(t.length) & vbNewLine & vbNewLine &
                   "Time Listened:" & vbNewLine & dll.SecondsTodhmsString(t.count * t.length) & vbNewLine & vbNewLine &
                   "Popularity:" & vbNewLine & dll.SecondsTohmsString(t.popularity) & vbNewLine & vbNewLine &
                   "Track Parts:" & vbNewLine & t.partsCount & vbNewLine & vbNewLine &
                   "Genre:" & vbNewLine & t.genre.name,, t.name)
        End If
    End Sub

    Private Sub playButton_Click(sender As Object, e As EventArgs) Handles playButton.Click
        If searchState > eSearchState.NONE Then
            cancelSearch()
        Else
            Dim checked As List(Of Track) = getCheckedTracks()
            If checked Is Nothing Then Return
            Dim t As Track = checked(0)
            If Form1.wmp.URL = t.fullPath And Form1.wmp.playState = WMPLib.WMPPlayState.wmppsPlaying Then
                Form1.wmp.Ctlcontrols.pause()
            Else
                t.play()
            End If
        End If
    End Sub

    Private Sub sortCombo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles sortCombo.SelectedIndexChanged
        If searchState > eSearchState.NONE Then
            cancelSearch()
        End If
        sortMode = sortCombo.SelectedIndex * 2
        If reverseSort Then sortMode += 1
        refreshList(True)
    End Sub

    Private Sub sortRevButton_Click(sender As Object, e As EventArgs) Handles sortRevButton.Click
        If searchState > eSearchState.NONE Then
            cancelSearch()
        End If
        If dll.getBinaryComponents(sortMode).Contains(Form1.sortMode.REVERSE) Then
            sortMode -= 1
            reverseSort = False
            sortRevButton.Text = "↑"
        Else
            sortMode += 1
            reverseSort = True
            sortRevButton.Text = "↓"
        End If
        refreshList(True)
    End Sub


#Region "Search"
    Enum eSearchState
        NONE
        INIT
        EMPTY
        SEARCHING
    End Enum

    Private Sub t2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tSearch.Click
        If searchState = eSearchState.NONE Then
            initSearch()
        ElseIf searchState = eSearchState.INIT Then
            tSearch.Text = ""
        End If
    End Sub
    Private Sub t2_GotFocus(sender As Object, e As EventArgs) Handles tSearch.GotFocus

    End Sub

    Sub initSearch()
        tSearch.Text = ""
        checkSearchSource.Visible = True : checkSearchSource.Text = "Search Source"
        picCancel.Visible = True
        searchState = eSearchState.INIT
        refreshList(True)
    End Sub

    Sub cancelSearch(Optional refillList As Boolean = True)
        If Not searchState = eSearchState.NONE Then
            tSearch.Text = "Search..."
            checkSearchSource.Visible = False
            picCancel.Visible = False
            searchState = eSearchState.NONE
            If refillList Then
                refreshList(True)
            End If
            okButton2.Select()
            refreshButtons()
        End If
    End Sub

    Private Sub cancelLabel_Click(sender As Object, e As EventArgs) Handles picCancel.Click
        cancelSearch()
    End Sub

    Private Sub t2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tSearch.KeyDown
        For Each k As Key In Key.keyList
            For Each keyCombi As Key.KeyCombi In k.keyCombiSet
                If e.KeyCode = keyCombi.key Then
                    If Not Key.anyModKey And keyCombi.modf = Key.modifier.None Or Key.altGrKey And keyCombi.modf = Key.modifier.AltGr Or keyCombi.modf = Key.modifier.Ctrl And Key.ctrlKey Or keyCombi.modf = Key.modifier.Shift And Key.shiftKey Then
                        e.SuppressKeyPress = True
                        Return
                    End If
                End If
            Next
        Next


        If tSearch.Text = "Search..." Then tSearch.Text = ""

        'delete whole words
        If e.KeyCode = Keys.Back And e.Control Then
            e.SuppressKeyPress = True
            Dim selStart As Integer = tSearch.SelectionStart
            If selStart > 0 And tSearch.SelectionLength = 0 Then
                Dim opText As String = tSearch.Text.Substring(0, selStart)
                Dim delInd As Integer = Math.Max(Math.Max(opText.LastIndexOf(" "), opText.LastIndexOf(",")), -1)
                Dim delLength As Integer = selStart
                If delInd > -1 Then delLength = selStart - delInd
                tSearch.Text = tSearch.Text.Remove(selStart - delLength, delLength)
                tSearch.SelectionStart = selStart - delLength
            End If

        ElseIf e.KeyCode = Keys.Enter Then

            If searchState = eSearchState.SEARCHING Then
                For i = 0 To listTrackSelection.Items.Count - 1
                    listTrackSelection.Items(i).Checked = True
                Next
            End If
            cancelSearch()
        End If
    End Sub

    Private Sub t2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tSearch.TextChanged
        Dim inverted As Boolean = dll.iniReadValue("Config", "invColors", "False", Form1.inipath)
        tSearch.ForeColor = IIf(tSearch.Text = "Search...", Color.DimGray, IIf(inverted, Color.White, Color.Black))
        ' tSearch.ForeColor = Color.Black
        If tSearch.Text = "" Then
            If Not searchState = eSearchState.INIT Then
                searchState = eSearchState.EMPTY
                refreshCheckedTracks()
                refreshList(True)
            End If

        ElseIf Not tSearch.Text = "Search..." Then
            searchState = eSearchState.SEARCHING
            tSearch.Text = tSearch.Text.ToLower
            searchFill()
        End If
    End Sub

    Sub refreshCheckedTracks()
        For i = 0 To listTrackSelection.Items.Count - 1
            Dim currName As String = listTrackSelection.Items(i).Text.ToLower
            If listTrackSelection.Items(i).Checked AndAlso Array.TrueForAll(searchChecked.ToArray, Function(x)
                                                                                                       Return x.name.ToLower <> currName
                                                                                                   End Function) Then
                searchChecked.Add(currTracks.Find(Function(x)
                                                      Return x.name.ToLower = currName
                                                  End Function))
            ElseIf Not listTrackSelection.Items(i).Checked AndAlso Not Array.TrueForAll(searchChecked.ToArray, Function(x)
                                                                                                                   Return x.name.ToLower <> currName
                                                                                                               End Function) Then
                searchChecked.Remove(currTracks.Find(Function(x)
                                                         Return x.name.ToLower = currName
                                                     End Function))
            End If
        Next
    End Sub


    Sub searchFill()
        Cursor = Cursors.WaitCursor

        refreshCheckedTracks()

        Dim list As New List(Of Track)
        For Each t As Track In currTracks

            Dim parsed() As String = tSearch.Text.Split(New Char() {","})

            Dim trackAdded As Boolean = False
            If Array.TrueForAll(parsed, Function(p) t.name.ToLower.Contains(p)) Then
                list.Add(t)
                trackAdded = True
            End If

            If searchSource Then
                If Array.TrueForAll(parsed, Function(p) t.fullPath.ToLower.Contains(p)) Then
                    If Not trackAdded Then
                        trackAdded = True
                        list.Add(t)
                    End If
                End If
            End If

        Next

        addTracks(list)
        Cursor = Cursors.Default

    End Sub

    Sub addTracks(tracks As List(Of Track))
        listTrackSelection.Items.Clear()
        Cursor = Cursors.WaitCursor
        listTrackSelection.Items.Clear()
        listTrackSelection.BeginUpdate()
        refreshDisable = True
        For Each t As Track In tracks
            Dim item As ListViewItem = addListRow(t)

            If Not Array.TrueForAll(searchChecked.ToArray, Function(x As Track)
                                                               Return x.name <> t.name
                                                           End Function) Then
                item.Checked = True
            End If
        Next
        refreshDisable = False
        listTrackSelection.EndUpdate()
        Cursor = Cursors.Default
        labelItemCount.Text = listTrackSelection.Items.Count
    End Sub


    Private Sub checkSearchSource_CheckedChanged(sender As Object, e As EventArgs) Handles checkSearchSource.CheckedChanged
        searchSource = checkSearchSource.Checked
        dll.iniWriteValue("Config", "searchSource", Convert.ToInt32(searchSource), Form1.inipath)
        If searchState = eSearchState.SEARCHING Then searchFill()
        tSearch.Focus()
    End Sub
    Private Sub checkSearchSource_VisibleChanged(sender As Object, e As EventArgs) Handles checkSearchSource.VisibleChanged
        checkSearchSource.Checked = searchSource
    End Sub

    Private Sub checkPlayOnClick_CheckedChanged(sender As Object, e As EventArgs) Handles checkPlayOnClick.CheckedChanged
        playOnClick = checkPlayOnClick.Checked
        dll.iniWriteValue("Config", "playOnClick", Convert.ToInt32(playOnClick), Form1.inipath)
        If Not playOnClick Then Form1.wmp.Ctlcontrols.pause()
    End Sub


    Private Sub listTrackSelection_ItemSelectionChanged(sender As Object, e As ListViewItemSelectionChangedEventArgs) Handles listTrackSelection.ItemSelectionChanged
        If listTrackSelection.SelectedIndices.Count > 0 Then
            If playOnClick Then
                Dim tr As Track = Track.getTrack(listTrackSelection.SelectedItems(0).SubItems(2).Text)
                If tr IsNot Nothing Then
                    tr.play()
                End If
            End If
        End If
    End Sub

    Private Sub listTrackSelection_SelectedIndexChanged(sender As Object, e As EventArgs) Handles listTrackSelection.SelectedIndexChanged

    End Sub



#End Region 'search

End Class