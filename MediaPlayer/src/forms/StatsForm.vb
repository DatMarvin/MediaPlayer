'20.08.2019

Imports MediaPlayer.SettingsEnums
Public Class StatsForm

    Property overlayMode() As Form1.eOverlayMode
        Get
            Return Form1.overlayMode
        End Get
        Set(value As Form1.eOverlayMode)
            Form1.overlayMode = value
        End Set
    End Property

    ReadOnly Property dll() As Utils
        Get
            Return Form1.dll
        End Get
    End Property

    Private Sub StatsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Location = New Point(Form1.Left + Form1.Width / 2 - Me.Width / 2, Form1.Top + Form1.Height / 2 - Me.Height / 2 + 15)
        FormUtils.colorForm(Me)
    End Sub


    Sub updateMode(toMode As Form1.eOverlayMode)
        overlayMode -= getCurrentStatsMode()
        overlayMode += toMode
        updateUI(True)
        Select Case getCurrentStatsMode()
            Case Form1.eOverlayMode.STATS_TRACKS
                radTracks.Checked = True
                invalidateTv()
                folderChange()
            Case Form1.eOverlayMode.STATS_RADIO
                radRadio.Checked = True
                tvSelection.Nodes.Clear()
                tvSelection.Nodes.Add("Radio Stations")
                loadRadioStats()
                labelCountUpdate()
            Case Form1.eOverlayMode.STATS_FOLDERS
                radFolders.Checked = True
                invalidateTv()
                folderChange()
        End Select

    End Sub

    Sub updateUI(Optional adjustColumns As Boolean = False)

        tvSelection.Font = Form1.tv.Font
        listTrackStats.Font = New Font("Microsoft Sans Serif", Int(8))
        listRadioStats.Font = listTrackStats.Font
        listFolderStats.Font = listTrackStats.Font

        Dim sender As ListView = Nothing
        If getCurrentStatsMode() = Form1.eOverlayMode.STATS_TRACKS Then sender = listTrackStats
        If getCurrentStatsMode() = Form1.eOverlayMode.STATS_RADIO Then sender = listRadioStats
        If getCurrentStatsMode() = Form1.eOverlayMode.STATS_FOLDERS Then sender = listFolderStats

        tvSelection.Enabled = Not sender.Equals(listRadioStats)
        tvSelection.CheckBoxes = sender.Equals(listFolderStats)
        avCheck.Visible = sender.Equals(listFolderStats)
        checkAll.Visible = sender.Equals(listFolderStats)
        buttonApply.Visible = sender.Equals(listFolderStats)

        listTrackStats.Visible = False
        listRadioStats.Visible = False
        listFolderStats.Visible = False

        Dim tvRatio As Double = 220 / (900)
        Dim listRatio As Double = 680 / (900)

        tvSelection.Size = New Size(Width * tvRatio - 10, Height - 30 - 120)
        tvSelection.Location = New Point(5, Height - tvSelection.Height - 50)
        sender.Size = New Size(Width * listRatio - 20, tvSelection.Height + tvSelection.Top - 30)
        sender.Location = New Point(tvSelection.Right + 5, tvSelection.Bottom - sender.Height)

        sender.Visible = True
        sender.Sorting = SortOrder.None
        sender.BringToFront()

        If adjustColumns Then
            Dim w As Integer = listRatio * 900 - 20
            listTrackStats.Columns(0).Width = CDbl(178 / w) * listTrackStats.Width : listTrackStats.Columns(1).Width = CDbl(40 / w) * listTrackStats.Width
            listTrackStats.Columns(2).Width = CDbl(90 / w) * listTrackStats.Width : listTrackStats.Columns(3).Width = CDbl(75 / w) * listTrackStats.Width
            listTrackStats.Columns(4).Width = CDbl(75 / w) * listTrackStats.Width : listTrackStats.Columns(5).Width = CDbl(70 / w) * listTrackStats.Width
            listTrackStats.Columns(6).Width = CDbl(30 / w) * listTrackStats.Width : listTrackStats.Columns(7).Width = CDbl(80 / w) * listTrackStats.Width

            listFolderStats.Columns(0).Width = CDbl(187 / w) * listFolderStats.Width : listFolderStats.Columns(1).Width = CDbl(50 / w) * listFolderStats.Width
            listFolderStats.Columns(2).Width = CDbl(60 / w) * listFolderStats.Width : listFolderStats.Columns(3).Width = CDbl(120 / w) * listFolderStats.Width
            listFolderStats.Columns(4).Width = CDbl(75 / w) * listFolderStats.Width : listFolderStats.Columns(5).Width = CDbl(77 / w) * listFolderStats.Width
            listFolderStats.Columns(6).Width = CDbl(70 / w) * listFolderStats.Width
        End If

        groupMode.Size = New Size(tvSelection.Width, 45)
        groupMode.Location = New Point(tvSelection.Left, sender.Top)

        checkAll.Location = New Point(tvSelection.Left, tvSelection.Top - checkAll.Height)
        avCheck.Location = New Point(checkAll.Right + 5, checkAll.Top)
        buttonApply.Location = New Point(tvSelection.Right - buttonApply.Width, checkAll.Top - 6)
        buttonTotal.Location = New Point(tvSelection.Right - buttonTotal.Width, sender.Top - buttonTotal.Height)
        labelCountUpdate()
    End Sub

    Sub invalidateTv()
        Dim selNodeString As String = Nothing
        If tvSelection.SelectedNode IsNot Nothing Then
            selNodeString = tvSelection.SelectedNode.FullPath
        Else
            If Form1.tv.SelectedNode IsNot Nothing Then selNodeString = Form1.tv.SelectedNode.FullPath
        End If
        tvSelection.Nodes.Clear()
        insTv(tvSelection.Nodes.Add(Folder.top.name, Folder.top.name), Folder.top)
        tvSelection.Nodes(Folder.top.name).Expand()
        If selNodeString IsNot Nothing AndAlso tvSelection.Nodes.Find(selNodeString, True).Length > 0 Then
            tvSelection.SelectedNode = tvSelection.Nodes.Find(selNodeString, True)(0)
        Else
            tvSelection.SelectedNode = tvSelection.TopNode
        End If
    End Sub

    Private Sub insTv(ByVal currNode As TreeNode, ByVal currFolder As Folder)
        If Not currFolder.isExcluded Then
            currNode.Checked = True
        End If
        If currFolder.children IsNot Nothing Then
            currFolder.children.Sort(Function(x, y) x.name.CompareTo(y.name))
            For i = 0 To currFolder.children.Count - 1
                insTv(currNode.Nodes.Add(currFolder.children(i).nodePath, currFolder.children(i).name), currFolder.children(i))
            Next
        End If
    End Sub

    Async Function loadTrackStats() As Threading.Tasks.Task
        Dim fol As Folder = Folder.getSelectedFolder(tvSelection)
        Form1.loaddates()

        listTrackStats.BeginUpdate()
        listTrackStats.Items.Clear()
        Dim prevSort = listTrackStats.Sorting
        Dim prevSorter = listTrackStats.ListViewItemSorter
        listTrackStats.Sorting = SortOrder.None

        Dim topCollection As New ListView.ListViewItemCollection(listTrackStats)
        For i = 0 To fol.tracks.Count - 1
            topCollection.Add(New ListViewItem(fol.tracks(i).name))
        Next

        Dim subCollection = Await AsyncTask.executeTask(Me, AsyncTask.getStatsList(fol.tracks))

        For i = 0 To topCollection.Count - 1
            topCollection(i).SubItems.AddRange(subCollection(i))
        Next
        listTrackStats.Sorting = prevSort
        listTrackStats.ListViewItemSorter = prevSorter
        listTrackStats.Sort()
        listTrackStats.EndUpdate()

    End Function

    Async Function loadFolderStats() As Threading.Tasks.Task
        listFolderStats.BeginUpdate()
        listFolderStats.Items.Clear()
        listFolderStats.Sorting = SortOrder.None
        If tvSelection.Nodes.Count = 0 Then Return

        Dim folders As New List(Of Folder) '= Folder.getFolders(, False)
        Dim checkedNodes As List(Of TreeNode) = getCheckedNodes(tvSelection.Nodes)
        For Each n As TreeNode In checkedNodes
            folders.Add(Folder.getFolder(Folder.root.fullPath & n.FullPath))
        Next

        Dim topCollection As New ListView.ListViewItemCollection(listFolderStats)
        For i = 0 To folders.Count - 1
            topCollection.Add(New ListViewItem(folders(i).nodePath))
        Next
        Dim subCollection As List(Of Integer())
        subCollection = Await AsyncTask.executeTask(Me, AsyncTask.getFolderStatsList(folders, dateLogStart))
        For i = 0 To topCollection.Count - 1
            Dim n As Integer = subCollection(i)(0)
            If n = 0 Then n = 1
            topCollection(i).SubItems.Add(n) 'number
            topCollection(i).SubItems.Add(IIf(avCheck.Checked, Int(subCollection(i)(3) / n), subCollection(i)(3))) 'count
            topCollection(i).SubItems.Add(dll.SecondsToydhmsString(IIf(avCheck.Checked, subCollection(i)(2) / n, subCollection(i)(2)))) 'time
            topCollection(i).SubItems.Add(dll.SecondsTohmsString(IIf(avCheck.Checked, subCollection(i)(5) / n, subCollection(i)(5)))) 'pop
            topCollection(i).SubItems.Add(dll.SecondsTohmsString(IIf(avCheck.Checked, subCollection(i)(4) / n, subCollection(i)(4)))) 'len
            topCollection(i).SubItems.Add(Now.Date.Subtract(New TimeSpan(subCollection(i)(6) / n, 0, 0, 0))) 'age
        Next
        listFolderStats.EndUpdate()
    End Function

    Sub loadRadioStats()
        Dim names() As String = dll.iniGetAllKeys(IniSection.RADIO_TIME, inipath)
        Dim vals() As String = dll.iniGetAllValues(IniSection.RADIO_TIME, inipath)
        If names IsNot Nothing And vals IsNot Nothing Then
            For i = 0 To names.Length - 1
                listRadioStats.Items.Add(names(i))
                listRadioStats.Items.Item(i).SubItems.Add(dll.SecondsTodhmsString(vals(i)))
            Next
        End If
    End Sub

    Function getAllNodes(currNodes As TreeNodeCollection, Optional currRes As List(Of TreeNode) = Nothing) As List(Of TreeNode)
        If currRes Is Nothing Then currRes = New List(Of TreeNode)
        For Each n As TreeNode In currNodes
            currRes.Add(n)
            getAllNodes(n.Nodes, currRes)
        Next
        Return currRes
    End Function

    Function getCheckedNodes(currNodes As TreeNodeCollection, Optional currRes As List(Of TreeNode) = Nothing) As List(Of TreeNode)
        If currRes Is Nothing Then currRes = New List(Of TreeNode)
        For Each n As TreeNode In currNodes
            If n.Checked Then currRes.Add(n)
            getCheckedNodes(n.Nodes, currRes)
        Next
        Return currRes
    End Function


    Private Sub tvSelection_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles tvSelection.AfterSelect
        If e.Action > 0 And getCurrentStatsMode() = Form1.eOverlayMode.STATS_TRACKS Then
            folderChange()
        End If
    End Sub


    Private Sub buttonApply_Click(sender As Object, e As EventArgs) Handles buttonApply.Click
        If getCurrentStatsMode() = Form1.eOverlayMode.STATS_FOLDERS Then folderChange()
    End Sub

    Async Sub folderChange()
        Select Case getCurrentStatsMode()
            Case Form1.eOverlayMode.STATS_TRACKS
                Try
                    Await loadTrackStats()
                Catch opCancel As ObjectDisposedException
                End Try
            Case Form1.eOverlayMode.STATS_RADIO
               'sub updateMode()
            Case Form1.eOverlayMode.STATS_FOLDERS
                Try
                    Await loadFolderStats()
                Catch opCancel As ObjectDisposedException
                End Try
        End Select
        labelCountUpdate()
    End Sub

    Private Sub radTracks_CheckedChanged(sender As Object, e As EventArgs) Handles radTracks.Click, radRadio.Click, radFolders.Click
        If radTracks.Checked Then
            updateMode(Form1.eOverlayMode.STATS_TRACKS)
        ElseIf radRadio.Checked Then
            updateMode(Form1.eOverlayMode.STATS_RADIO)
        ElseIf radFolders.Checked Then
            updateMode(Form1.eOverlayMode.STATS_FOLDERS)
        End If
    End Sub

    Sub labelCountUpdate()
        Dim currList As ListView = Nothing
        Select Case getCurrentStatsMode()
            Case Form1.eOverlayMode.STATS_TRACKS : currList = listTrackStats
            Case Form1.eOverlayMode.STATS_RADIO : currList = listRadioStats
            Case Form1.eOverlayMode.STATS_FOLDERS : currList = listFolderStats
        End Select
        Dim ind As String = ""
        If currList.SelectedItems.Count > 0 Then ind = currList.SelectedItems(0).Index + 1 & " - "
        labelCount.Text = "Items: " & ind & currList.Items.Count
        labelCount.Location = New Point(listTrackStats.Left, currList.Top - labelCount.Height - 4)
    End Sub

    Private Sub listTrackStats_SelectedIndexChanged(sender As Object, e As EventArgs) Handles listTrackStats.SelectedIndexChanged
        labelCountUpdate()
    End Sub

    Private Sub listTrackStats_ItemActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles listTrackStats.ItemActivate
        If Not IsNothing(tvSelection.SelectedNode) Then
            If Not radioEnabled Then
                Form1.last = Track.getFirstTrack(listTrackStats.SelectedItems(0).Text)
                Form1.wmpstart(Form1.last)
            End If
        End If
    End Sub

    Private Sub checkAll_Click(sender As Object, e As EventArgs) Handles checkAll.Click
        Dim nodes As List(Of TreeNode) = getAllNodes(tvSelection.Nodes)
        For Each n As TreeNode In nodes
            n.Checked = checkAll.Checked
        Next
    End Sub


    Dim oldState As FormWindowState
    Private Sub StatsForm_ResizeBegin(sender As Object, e As EventArgs) Handles Me.ResizeBegin
        oldState = WindowState
    End Sub

    Private Sub StatsForm_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        updateUI(WindowState <> oldState)
        oldState = WindowState
    End Sub

    Private Sub StatsForm_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        updateUI(True)
    End Sub


    Private Sub list1_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles listTrackStats.ColumnClick, listFolderStats.ColumnClick
        Dim l As ListView = IIf(sender.Equals(listTrackStats), listTrackStats, listFolderStats)
        l.BeginUpdate()
        l.Sorting = IIf(l.Sorting = SortOrder.Descending, SortOrder.Ascending, SortOrder.Descending)
        l.ListViewItemSorter = New ListViewItemComparer(e.Column, l, getCurrentStatsMode())
        l.Sort()
        l.EndUpdate()
    End Sub

    Function getCurrentStatsMode() As Form1.eOverlayMode
        If Form1.overlayModeContains(Form1.eOverlayMode.STATS_TRACKS) Then : Return Form1.eOverlayMode.STATS_TRACKS
        ElseIf Form1.overlayModeContains(Form1.eOverlayMode.STATS_RADIO) Then : Return Form1.eOverlayMode.STATS_RADIO
        ElseIf Form1.overlayModeContains(Form1.eOverlayMode.STATS_FOLDERS) Then : Return Form1.eOverlayMode.STATS_FOLDERS
        Else : Return 0
        End If
    End Function

    Class ListViewItemComparer
        Implements IComparer

        Private col As Integer = 0
        Private order As SortOrder
        Private statsMode As Form1.eOverlayMode
        Private Shared dll As New Utils

        Public Sub New(ByVal col As Integer, ByVal list As ListView, ByVal mode As Form1.eOverlayMode)
            MyClass.col = col
            MyClass.statsMode = mode
            MyClass.order = list.Sorting
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Dim res As Integer = 0
            If statsMode = Form1.eOverlayMode.STATS_TRACKS Then
                Select Case col 'list1
                    Case 0, 7 : res = [String].Compare(CType(x, ListViewItem).SubItems(col).Text, CType(y, ListViewItem).SubItems(col).Text)
                    Case 1, 6 : res = CInt(CType(x, ListViewItem).SubItems(col).Text).CompareTo(CInt(CType(y, ListViewItem).SubItems(col).Text))
                    Case 2 : res = CInt(dll.dhmsStringToSeconds(CType(x, ListViewItem).SubItems(col).Text)).CompareTo(CInt(dll.dhmsStringToSeconds(CType(y, ListViewItem).SubItems(col).Text)))
                    Case 3, 4 : res = CInt(dll.hmsStringToSeconds(CType(x, ListViewItem).SubItems(col).Text)).CompareTo(CInt(dll.hmsStringToSeconds(CType(y, ListViewItem).SubItems(col).Text)))
                    Case 5
                        res = dateCompare(x, y, col)
                End Select
            ElseIf statsMode = Form1.eOverlayMode.STATS_FOLDERS Then
                Select Case col 'list3
                    Case 0 : res = [String].Compare(CType(x, ListViewItem).SubItems(col).Text, CType(y, ListViewItem).SubItems(col).Text)
                    Case 1, 2 : res = CInt(CType(x, ListViewItem).SubItems(col).Text).CompareTo(CInt(CType(y, ListViewItem).SubItems(col).Text))
                    Case 3 : res = CInt(dll.ydhmsStringToSeconds(CType(x, ListViewItem).SubItems(col).Text)).CompareTo(CInt(dll.ydhmsStringToSeconds(CType(y, ListViewItem).SubItems(col).Text)))
                    Case 4, 5 : res = CInt(dll.hmsStringToSeconds(CType(x, ListViewItem).SubItems(col).Text)).CompareTo(CInt(dll.hmsStringToSeconds(CType(y, ListViewItem).SubItems(col).Text)))
                    Case 6
                        res = dateCompare(x, y, col)
                End Select
            End If

            Return res * IIf(order = SortOrder.Descending, -1, 1)
        End Function
        Public Function dateCompare(ByVal x As Object, ByVal y As Object, ByVal col As Integer) As Integer
            Dim s1 As String = CType(x, ListViewItem).SubItems(col).Text
            If s1 = "" Then s1 = dateLogStart
            Dim s2 As String = CType(y, ListViewItem).SubItems(col).Text
            If s2 = "" Then s2 = dateLogStart
            Return CDate(s1).CompareTo(CDate(s2))
        End Function

    End Class

    Private Sub buttonTotal_Click(sender As Object, e As EventArgs) Handles buttonTotal.Click
        totaltime()
    End Sub

    Public Sub totaltime()
        Dim radTime As Integer = 0
        Dim radnumber As Integer = 0
        Dim vals() As String = dll.iniGetAllValues(IniSection.RADIO_TIME, inipath)
        If vals IsNot Nothing Then
            For i = 0 To vals.Length - 1
                radTime += vals(i)
                radnumber += 1
            Next
        End If
        If radnumber = 0 Then radnumber = 1

        Dim alltim As Integer = 0
        Dim allnumber As Integer = 0
        Dim allLen As Double = 0
        Dim allCount As Integer = 0


        Dim diff As Integer = Now.Subtract(IIf(dateLogStart.ToShortDateString() = "06.04.2011", CDate("11.09.2012"), dateLogStart)).TotalDays

        Dim strTracks() As String = dll.iniGetAllLines(IniSection.TRACKS, inipath)
        If Not IsNothing(strTracks) Then
            For i = 0 To strTracks.Length - 1
                Dim name As String = Mid(strTracks(i), 1, strTracks(i).LastIndexOf("="))
                Dim count As Integer = Mid(strTracks(i), strTracks(i).LastIndexOf("=") + 2)
                If count > 0 Then
                    allnumber += 1
                    allCount += count
                    Dim currLen As Double = loadRawSetting(SettingsIdentifier.TRACKS_TIME, name)
                    alltim += count * currLen
                    allLen += currLen
                End If
            Next
        End If
        If allnumber = 0 Then allnumber = 1


        '  allLen = Await AsyncTask.executeTask(Me, AsyncTask.getTotaltime())

        MsgBox("Radio Time:" & vbNewLine & "‾‾‾‾‾‾‾‾‾‾‾‾" & vbNewLine & dll.SecondsTodhmsString(radTime) & vbNewLine & "Ø " & dll.SecondsTodhmsString(Int(radTime / radnumber)) & vbNewLine & "»  " & dll.SecondsTodhmsString(Int(radTime / diff)) & vbNewLine & vbNewLine &
                   "Stats File:" & vbNewLine & "‾‾‾‾‾‾‾‾‾‾" & vbNewLine &
                   "Total Number:" & vbNewLine & allnumber - radnumber & vbNewLine & vbNewLine &
                   "Total Time: " & vbNewLine & dll.SecondsTodhmsString(alltim) & vbNewLine & "Ø " & dll.SecondsTodhmsString(Int(alltim / allnumber)) & vbNewLine & "»  " & dll.SecondsTodhmsString(Int(alltim / diff)) & vbNewLine & vbNewLine &
                   "Total Count: " & vbNewLine & allCount & vbNewLine & "Ø " & Int(allCount / allnumber) & vbNewLine & "»  " & Int(allCount / diff) & vbNewLine & vbNewLine &
                   "Total Length: " & vbNewLine & dll.SecondsTodhmsString(Int(allLen)) & vbNewLine & "Ø " & dll.SecondsTodhmsString(Int(allLen / allnumber)) & vbNewLine & vbNewLine &
                   "Alltime:" & vbNewLine & "‾‾‾‾‾‾‾‾" & vbNewLine &
                   "Total Time: " & vbNewLine & dll.SecondsTodhmsString((alltim + radTime)) & vbNewLine & "Ø " & dll.SecondsTodhmsString(Int((alltim + radTime) / (allnumber + radnumber))) & vbNewLine & "»  " & dll.SecondsTodhmsString(Int((alltim + radTime) / diff)))
    End Sub

End Class