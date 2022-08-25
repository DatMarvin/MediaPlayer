
Imports MediaPlayer.SettingsEnums
Imports System.Reflection
Imports System.Net
Imports System.IO
Public Class OptionsForm


    Public Shared ReadOnly Property dll As Utils
        Get
            Return Form1.dll
        End Get
    End Property


    ReadOnly Property selectedModifier As Key.modifier
        Get
            Return DirectCast([Enum].Parse(GetType(Key.modifier), getSelectedRadio.Text), Key.modifier)
        End Get
    End Property
    ReadOnly Property genres As List(Of Genre)
        Get
            Return Genre.genres
        End Get
    End Property
    ReadOnly Property tv As TreeView
        Get
            Return Form1.tv
        End Get
    End Property

    ReadOnly Property ftpCred As Utils.credentials
        Get
            Return dll.ftpCred
        End Get
    End Property
    ReadOnly Property remoteTcp() As Tcp
        Get
            Return Form1.remotetcp
        End Get
    End Property

    ReadOnly Property args As List(Of String)
        Get
            If arguments Is Nothing Then Return New List(Of String)
            Return arguments.ToList()
        End Get
    End Property

    Public waitingInput As Boolean = False
    Public state As optionState
    Public updateTvAfter As Boolean = False
    Dim nodeSelection As Boolean = False
    Dim trackSelection As Boolean = False
    Public selNodes() As String
    Public selTracks() As String
    Public arguments() As String

    Public Enum optionState As Integer
        NONE
        KEYSET
        'FOLDERS deprecated 11.07.19
        GENRES
        GADGETS
        RADIO
        REMOTE
        UPDATE
        PATHS '13.10.2017
        PLAYLISTS '30.04.2019
        PLAYER '12.08.2019
    End Enum



    Private Sub OptionsForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Size = New Size(325 + listMenu.Width, 300)
        Me.Location = New Point(Form1.Left + Form1.Width / 2 - Me.Width / 2, Form1.Top + Form1.Height / 2 - Me.Height / 2)
        labelMenu.Text = ""
        colorForm()
        Form1.cancelSearch()
        If state = optionState.NONE Then
            If Form1.lastOptionsState = optionState.NONE Then
                init(0)
            Else
                init(Form1.lastOptionsState)
            End If
        Else

            init(state)
        End If

    End Sub

    Public Sub addArguments(arguments() As String)
        If arguments IsNot Nothing Then
            For Each s As String In arguments
                dll.ExtendArray(Me.arguments, s)
            Next
        End If
    End Sub

    Sub init(ByVal state As optionState)
        Cursor = Cursors.WaitCursor
        Me.state = state
        labelMenu.Text = ""
        For Each c As Control In Controls
            If TypeOf c Is GroupBox Then
                c.Visible = False
            End If
        Next
        selectIndex(state)

        If state > 0 Then
            ' listMenu.Location = New Point(1, Me.DisplayRectangle.Height / 2 - listMenu.Height / 2)
            listMenu.Location = New Point(1, 10)
            labelMenu.Location = New Point(1, listMenu.Bottom + 1)
            labelMenu.MaximumSize = New Size(listMenu.Width, 0)
            If Controls.ContainsKey("g" & stateToIndex(state) + 1) Then
                Controls("g" & stateToIndex(state) + 1).Location = New Point(listMenu.Right + 1, 1)
                Controls("g" & stateToIndex(state) + 1).Visible = True
            End If
        End If

        Select Case state
            Case optionState.KEYSET
                labelMenu.Text = "Assign hotkeys to control the player." & vbNewLine & "Modifier must be the same for all combinations." & vbNewLine & "Timer delay specifies the time between key strokes."
                commandBox.Items.AddRange(Key.keyList.ToArray)
                commandBox.SelectedIndex = 0
                If args.Count > 0 Then
                    For i = 0 To commandBox.Items.Count - 1
                        Dim key As Key = commandBox.Items(i)
                        If key.name = args(0) Then
                            commandBox.SelectedIndex = i
                            Exit For
                        End If
                    Next
                End If
                keyButton.TabStop = False
                commandBox.Select()
                numDelay.Value = Form1.keydelayt.Interval

            Case optionState.GENRES
                labelMenu.Text = "Folders and individual tracks can be associated to a genre." & vbNewLine & "The genre of a track overrides potential conflicts with folder associations."
                listGenres.Items.Clear()
                listAssociations.Items.Clear()
                listAssociations.Items.Add("Select Genre...")
                For Each g As Genre In Genre.genres
                    listGenres.Items.Add(g)
                Next
                Dim initGenre As Genre = Genre.Undefined
                If args.Count > 0 Then
                    For Each g As Genre In Genre.genres

                        If g.name = args(0) Then initGenre = g
                    Next
                End If
                listGenres.SelectedItem = initGenre

            Case optionState.GADGETS

            Case optionState.RADIO
                labelMenu.Text = "Manages radio stations that can be streamed."
                listStations.Items.Clear()
                sortingBox.Items.Clear()
                sortingBox.Items.Add("Default")
                sortingBox.Items.Add("Time Listened")
                sortingBox.SelectedIndex = SettingsService.getSetting(SettingsIdentifier.RADIO_SORT)
                Dim rads As List(Of Radio) = Radio.getStations()
                If sortingBox.SelectedIndex = 1 Then
                    rads.Sort(Function(x, y) y.time.CompareTo(x.time))
                End If
                listStations.Items.AddRange(rads.ToArray)
                listStations.SelectedIndex = rads.Count - 1

            Case optionState.REMOTE
                labelMenu.Text = "The player can be controlled by a TCP/IP connection with the player acting as server. A connection via external IP is permitted."
                Dim ownIp As String = remoteTcp.getIPAddress(Dns.GetHostName)
                labelOwnIp.Text = IIf(ownIp = "", "Offline", ownIp)
                setExternalIP()
                checkEnableStartup.Checked = SettingsService.getSetting(SettingsIdentifier.REMOTE)
                checkBlockExtIps.Checked = SettingsService.getSetting(SettingsIdentifier.REMOTE_BLOCK_EXT_IPS)
                checkBlockMessages.Checked = SettingsService.getSetting(SettingsIdentifier.REMOTE_BLOCK_MESSAGES)
                refreshRemoteUI()
                tPort.Text = ""
                labelPort.Text = remoteTcp.port

            Case optionState.UPDATE
                labelMenu.Text = "Files are downloaded via file transfer protocol." & vbNewLine & "Own version can be published into local webshare repository."
                Try
                    Dim sr2 As New StreamReader(My.Application.Info.DirectoryPath & "\version")
                    labelCurrVersion.Text = sr2.ReadToEnd
                    sr2.Close()
                Catch ex As Exception
                    Try
                        Dim dt As Date = IO.File.GetLastWriteTime(My.Application.Info.DirectoryPath & "\mp3player.exe")
                        labelCurrVersion.Text = "~ " & dll.ReverseDateString(dt.ToShortDateString) & "_" & IIf(dt.Hour < 10, "0", "") & dt.Hour & "." & IIf(dt.Minute < 10, "0", "") & dt.Minute & "." & IIf(dt.Second < 10, "0", "") & dt.Second
                    Catch eex As Exception
                        labelCurrVersion.Text = "Unknown"
                    End Try
                End Try
                checkAutoUpdate.Checked = SettingsService.getSetting(SettingsIdentifier.FTP_AUTO_UPDATE)

                tftpIp.Text = SettingsService.getSetting(SettingsIdentifier.FTP_IP)
                tftpUser.Text = SettingsService.getSetting(SettingsIdentifier.FTP_USER)
                tftpPw.Text = SettingsService.getSetting(SettingsIdentifier.FTP_PW)
                pBar.Value = 0
                pBar2.Value = 0
                labelftpCurrProg.Text = "0 / 0"
                labelftpTotalProg.Text = "0 / 0"
                labelPublishedVersion.Text = ""
                addCoreFiles()
                Dim publ() As String = SettingsService.loadSetting(SettingsIdentifier.FTP_AUTO_UPDATE).Split(";")
                If publ IsNot Nothing Then
                    For i = 0 To publ.Length - 1
                        If Not publ(i) = "" Then
                            listPublish.Items.Add(publ(i))
                            dll.publishFileList.Add(publ(i))
                        End If
                    Next
                End If

            Case optionState.PATHS
                labelMenu.Text = "First two paths must be valid to use the player, other invalid paths may lead to malfunctions."
                tStatsFile.Text = inipath
                tMusicDir.Text = path
                tPlaylistFile.Text = playlistPath
                tDatesFile.Text = logpath
                tLyricsDir.Text = lyrpath
                tFtpDir.Text = ftpPath
                If darkTheme Then
                    logPathKeyPic.BackgroundImage = My.Resources.unlock_inv
                    logPathReloadPic.BackgroundImage = My.Resources.rel_inv
                Else
                    logPathKeyPic.BackgroundImage = My.Resources.unlock
                    logPathReloadPic.BackgroundImage = My.Resources.rel
                End If

                checkValidity(False, False)

            Case optionState.PLAYLISTS
                labelMenu.Text = "Manage Playlists here."
                radioAll.Checked = True
                If args.Contains("folders") Then
                    radioFolders.Checked = True
                Else
                    radioPlaylists.Checked = True
                End If
                Dim sel As Folder = Nothing
                For Each s As String In args
                    sel = Folder.getFolder(s)
                Next

                refillPlaylists(sel)

            Case optionState.PLAYER
                checkDarkTheme.Checked = darkTheme
                checkSavePos.Checked = saveWinPosSize
                trackbarBalance.Value = balance
                trackbarPlayRate.Value = scaleToNum(playRate)
                labelBalance.Text = "Balance: " & trackbarBalance.Value
                labelPlayRate.Text = "Play Rate: " & numToScale(trackbarPlayRate.Value)
                checkRandomNextTrack.Checked = randomNextTrack
                checkPlaylistHistory.Checked = getSetting(SettingsIdentifier.PLAYLIST_SAVE_HISTORY)
                checkRemoveTrackFromList.Checked = removeNextTrack

                Form1.saveWinPos()
                Form1.saveWinSize()

            Case optionState.NONE
                MsgBox("No option mode selected")
        End Select
        Cursor = Cursors.Default
    End Sub

    Function saveChanges() As Boolean 'true if form must not be closed
        dll.inipath = inipath
        Select Case state
            Case optionState.KEYSET
                If waitingInput Then switchKeyInputState()
                ' Form1.delayMs = numDelay.Value
                saveSetting(SettingsIdentifier.DELAY_MS, numDelay.Value)
                Form1.keydelayt.Interval = numDelay.Value

            Case optionState.GENRES
                Dim diff As Boolean = False
                For i = 0 To listGenres.Items.Count - 1
                    If genres IsNot Nothing AndAlso Not genres.Contains(listGenres.Items(i)) Then
                        diff = True
                        Exit For
                    End If
                Next
                If genres Is Nothing OrElse genres.Count <> listGenres.Items.Count Or diff Then
                    ' ReDim Form1.genres(listGenres.Items.Count - 1)
                    Dim gString As String = ""
                    For i = 0 To listGenres.Items.Count - 1
                        ' Form1.genres(i) = listGenres.Items(i)
                        gString &= listGenres.Items(i) & IIf(i = listGenres.Items.Count - 1, "", ";")
                    Next
                    SettingsService.saveSetting(SettingsIdentifier.GENRES, gString)
                End If
                Form1.labelStatsUpdate()


            Case optionState.GADGETS

            Case optionState.RADIO
                If sortingBox.SelectedIndex = 0 Then
                    For i = 0 To listStations.Items.Count - 1
                        dll.iniDeleteKey(IniSection.RADIO, listStations.Items(i).name, inipath)
                    Next
                End If
                For i = 0 To listStations.Items.Count - 1
                    saveRawSetting(SettingsIdentifier.RADIO_STATIONS, listStations.Items(i).name, listStations.Items(i).url)
                Next
                If radioEnabled Then
                    Form1.changeSourceMode(1)
                End If

            Case optionState.UPDATE
                If dll.ftpThread IsNot Nothing AndAlso dll.ftpThread.IsAlive Then
                    If MsgBox("Abort Update Search?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        dll.ftpThread.Abort()
                    Else
                        Return True
                    End If
                End If
                credentialsUpdate()
                abortDownloadGC()

            Case optionState.PATHS
                If checkValidity(True, True) Then
                    SettingsService.setSetting(SettingsIdentifier.INIPATH, tStatsFile.Text)
                    If Not inipath = tStatsFile.Text Then
                        'Form1.inipath = tStatsFile.Text
                        If dll.iniIsValidSection(IniSection.TRACKS, inipath) Then
                            If MsgBox("You chose a new settings file. Override other paths from that file?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                                setPath(0, SettingsService.loadSetting(SettingsIdentifier.PATH))
                                setPath(1, SettingsService.loadSetting(SettingsIdentifier.LOGPATH))
                                setPath(2, SettingsService.loadSetting(SettingsIdentifier.LYRPATH))
                                setPath(3, SettingsService.loadSetting(SettingsIdentifier.FTPPATH))
                                setPath(4, "")
                                setPath(5, SettingsService.loadSetting(SettingsIdentifier.PLAYLISTPATH))
                                Form1.savePaths()
                                MsgBox("Restart the program to apply changes")
                                Return False
                            End If
                        End If
                    Else
                        ' Form1.inipath = tStatsFile.Text
                    End If
                    setPath(0)
                    Me.Update()
                    setPath(1)
                    setPath(2)
                    setPath(3)
                    setPath(4)
                    setPath(5)
                    Form1.savePaths()
                Else
                    Return True
                End If

            Case optionState.PLAYLISTS
                If updateTvAfter Then
                    If Not Form1.l2.Items.Count = 0 And Form1.l2_2.Items.Count = 0 Then Form1.saveLastTrack(CType(Form1.getSelectedList.SelectedItem, Track).virtualPath)
                    Form1.localfill()
                End If

        End Select
        Return False
    End Function
    Private Sub OptionsForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If saveChanges() Then
            e.Cancel = True
            Return
        End If
        Form1.lastOptionsState = state
        Form1.lockFormSwitch()
    End Sub


#Region "General"

    Sub colorForm() '06.08.19
        If inipath = "" Then Return
        Dim inverted As Boolean = SettingsService.getSetting(SettingsIdentifier.DARK_THEME)
        Dim lightCol As Color = IIf(inverted, Color.FromArgb(50, 50, 50), Color.White)
        Dim darkCol As Color = IIf(inverted, Color.FromArgb(20, 20, 20), Color.FromArgb(255, 240, 240, 240))

        Dim invLightCol As Color = IIf(Not inverted, Color.Black, Color.White)
        Dim invDarkCol As Color = IIf(Not inverted, Color.Black, Color.FromArgb(255, 240, 240, 240))

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
            ElseIf TypeOf c Is Button Then
                CType(c, Button).FlatStyle = FlatStyle.System
                c.BackColor = lightCol
                c.ForeColor = invLightCol
            Else
                c.BackColor = darkCol
                c.ForeColor = invDarkCol
            End If

        Next
    End Sub

    Private Sub listMenu_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles listMenu.MouseClick
        Dim it As Integer = sender.IndexFromPoint(New Point(Cursor.Position.X - sender.PointToScreen(New Point(sender.Left, sender.Top)).X + sender.Left, Cursor.Position.Y - sender.PointToScreen(New Point(sender.Left, sender.Top)).Y + sender.top))
        If it > -1 Then
            If Not state = indexToState(listMenu.SelectedIndex) Then
                If Not saveChanges() Then
                    Text = ""
                    init(listMenu.SelectedIndex)
                Else
                    selectIndex(state)
                End If
            End If
        End If
    End Sub

    Private Sub listMenu_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles listMenu.MouseDown
        Dim it As Integer = listMenu.SelectedIndex
    End Sub
    Private Sub listMenu_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles listMenu.SelectedIndexChanged

    End Sub
    Function indexToState(ByVal index As Integer) As optionState
        Select Case index
            Case 0 : Return optionState.PLAYER
            Case 1 : Return optionState.PATHS
            Case 2 : Return optionState.KEYSET
            Case 3 : Return optionState.RADIO
            Case 4 : Return optionState.GENRES
            Case 5 : Return optionState.PLAYLISTS
            Case 6 : Return optionState.GADGETS
            Case 7 : Return optionState.REMOTE
            Case 8 : Return optionState.UPDATE
            Case Else : Return optionState.NONE
        End Select
    End Function
    Sub init(ByVal listIndex As Integer)
        init(indexToState(listIndex))
        listMenu.SelectedIndex = listIndex
    End Sub
    Function stateToIndex(ByVal state As optionState) As Integer
        Select Case state
            Case optionState.PLAYER : Return 0
            Case optionState.PATHS : Return 1
            Case optionState.KEYSET : Return 2
            Case optionState.RADIO : Return 3
            Case optionState.GENRES : Return 4
            Case optionState.PLAYLISTS : Return 5
            Case optionState.GADGETS : Return 6
            Case optionState.REMOTE : Return 7
            Case optionState.UPDATE : Return 8
            Case Else : Return -1
        End Select
    End Function
    Sub selectIndex(ByVal state As optionState)
        listMenu.SelectedIndex = stateToIndex(state)
    End Sub
    Sub setPath(ByVal index As Integer, Optional ByVal refString As String = "")
        Select Case index
            Case 0
                If refString = "" Then
                    setSetting(SettingsIdentifier.PATH, tMusicDir.Text)
                    If Not path = tMusicDir.Text Then
                        ' Form1.path = tMusicDir.Text
                        Folder.setTopFolder(path)
                        Form1.localfill()
                    Else
                        'Form1.path = tMusicDir.Text
                    End If
                Else
                    setSetting(SettingsIdentifier.PATH, refString)
                    '  Form1.path = refString
                End If
            Case 1
                If refString = "" Then
                    setSetting(SettingsIdentifier.LOGPATH, tDatesFile.Text) '  Form1.logpath = tDatesFile.Text
                Else : setSetting(SettingsIdentifier.LOGPATH, refString)
                End If
            Case 2
                If refString = "" Then
                    setSetting(SettingsIdentifier.LYRPATH, tLyricsDir.Text) ' Form1.lyrpath = tLyricsDir.Text
                Else : setSetting(SettingsIdentifier.LYRPATH, refString)
                End If
            Case 3
                If refString = "" Then
                    setSetting(SettingsIdentifier.FTPPATH, tFtpDir.Text) ' Form1.ftpPath = tFtpDir.Text
                Else : setSetting(SettingsIdentifier.FTPPATH, refString)
                End If
            Case 4
                ' commPath - deprecated
            Case 5
                If refString = "" Then
                    setSetting(SettingsIdentifier.PLAYLISTPATH, tPlaylistFile.Text) ' Form1.playlistPath = tPlaylistFile.Text
                Else : setSetting(SettingsIdentifier.PLAYLISTPATH, refString)
                End If
        End Select
    End Sub

#End Region 'General


#Region "Key Set"
    Private Sub commandBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles commandBox.SelectedIndexChanged
        reloadKeySet()
    End Sub


    Sub reloadKeySet()
        listSet.Items.Clear()
        Dim currKey As Key = Key.getKey(commandBox.Text)
        If currKey IsNot Nothing Then
            listSet.Items.AddRange(currKey.keyCombiSet.ToArray)
        End If
    End Sub

    Private Sub defaultclick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles defaultButton.Click
        If MsgBox("Are you sure to delete all custom key bindings?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            For Each k As Key In Key.keyList
                dll.iniDeleteKey(IniSection.HOTKEYS, k.ToString, inipath)
            Next
            Key.initKeys()
            reloadKeySet()
        End If
    End Sub

    Function getSelectedRadio() As RadioButton
        If r0.Checked Then : Return r0
        ElseIf r1.Checked Then : Return r1
        ElseIf r2.Checked Then : Return r2
        Else : Return r3
        End If
    End Function

    Private Sub keyButton_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles keyButton.KeyDown
        e.SuppressKeyPress = True
        If waitingInput Then
            If Not e.KeyCode = Keys.Space And Not e.KeyCode = Keys.Enter And Not e.KeyCode = Keys.LWin And Not e.KeyCode = Keys.RWin And Not e.KeyCode = Keys.CapsLock _
             And Not e.KeyCode = Keys.Control And Not e.KeyCode = Keys.ControlKey And Not e.KeyCode = Keys.LControlKey And Not e.KeyCode = Keys.RControlKey _
               And Not e.KeyCode = Keys.Alt And Not e.KeyCode = Keys.RMenu And Not e.KeyCode = Keys.Shift And Not e.KeyCode = Keys.LShiftKey And Not e.KeyCode = Keys.RShiftKey And Not e.KeyCode = Keys.ShiftKey Then
                keyInput(e.KeyCode)
            Else
                If MsgBox("Key not allowed. Proceed anyways?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    keyInput(e.KeyCode)
                End If
            End If
        End If

    End Sub
    Private Sub keyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles keyButton.Click

    End Sub

    Sub keyInput(ByVal inputKey As Keys)
        Dim currKey As Key = Key.getKey(commandBox.Text)
        Dim exist As Key = Key.keyCombinationExists(inputKey, selectedModifier)
        If exist IsNot Nothing Then
            If currKey.Equals(exist) OrElse MsgBox("Already in use in command '" & exist.ToString & "'. Proceed anyways?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                Exit Sub
            End If
        End If
        waitingInput = False
        listSet.Items.Add(New Key.KeyCombi(inputKey, getSelectedModifier()))
        commandBox.Enabled = True
        remButton.Enabled = True
        listSet.Enabled = True
        keyButton.Text = "Assign"
        saveCurrentSet(currKey)
        setCurrentSet(currKey)
    End Sub

    Private Function getSelectedModifier() As Key.modifier
        Dim curr As RadioButton = getSelectedRadio()
        If curr.Equals(r1) Then Return Key.modifier.Ctrl
        If curr.Equals(r2) Then Return Key.modifier.AltGr
        If curr.Equals(r3) Then Return Key.modifier.Shift
        Return Key.modifier.None
    End Function

    Private Sub keyButton_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles keyButton.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            switchKeyInputState()
        ElseIf e.Button = MouseButtons.XButton1 Or e.Button = Windows.Forms.MouseButtons.XButton2 Then
            If waitingInput Then
                keyInput(IIf(e.Button = Windows.Forms.MouseButtons.XButton1, Keys.XButton1, Keys.XButton2))
            End If
        Else
            If MsgBox("Key not allowed. Proceed anyways?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                keyInput(e.Button)
            End If
        End If
    End Sub
    Sub switchKeyInputState()
        waitingInput = Not waitingInput
        commandBox.Enabled = Not waitingInput
        remButton.Enabled = Not waitingInput
        listSet.Enabled = Not waitingInput
        If waitingInput Then
            keyButton.Text = "Push Key"
        Else
            keyButton.Text = "Assign"
        End If
    End Sub

    Sub setCurrentSet(ByVal currKey As Key)
        currKey.keyCombiSet.Clear()
        For i = 0 To listSet.Items.Count - 1
            currKey.keyCombiSet.Add(listSet.Items(i))
        Next
    End Sub

    Sub saveCurrentSet(ByVal currKey As Key)
        If listSet.Items.Count > 0 Then
            Dim combiString As String = ""
            For i = 0 To listSet.Items.Count - 1
                combiString &= listSet.Items(i).ToString().Replace(" ", "") & IIf(i < listSet.Items.Count - 1, ";", "")
            Next
            saveRawSetting(SettingsIdentifier.HOTKEY_MAPPING, currKey.ToString(), combiString)
        Else
            dll.iniDeleteKey(IniSection.HOTKEYS, currKey.ToString, inipath)
        End If
    End Sub

    Private Sub remButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles remButton.Click
        If listSet.SelectedIndex > -1 Then
            Dim currKey As Key = Key.getKey(commandBox.Text)
            For Each combi As Key.KeyCombi In currKey.keyCombiSet
                If combi.ToString = DirectCast(listSet.SelectedItem, Key.KeyCombi).ToString Then
                    currKey.keyCombiSet.Remove(combi)
                    Exit For
                End If
            Next
            listSet.Items.Remove(listSet.SelectedItem)
            saveCurrentSet(currKey)
            setCurrentSet(currKey)
        End If
    End Sub
#End Region 'Key Set

#Region "Genres"



    Private Sub newGenreButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewGenreButton.Click
        Me.TopMost = False
        Dim a As String = InputBox("Type in new genre")
        If Not a = "" And Not a.Contains(";") And Not a.ToLower = Genre.Undefined.ToString().ToLower Then
            For Each g As Genre In listGenres.Items
                If g.name.ToLower = a.ToLower Then
                    MsgBox("Genre already exists", MsgBoxStyle.Exclamation)
                    Return
                End If
            Next
            Dim newGenre As New Genre(a)
            listGenres.Items.Add(newGenre)
            Genre.genres.Add(newGenre)
            Genre.writeGenres()
            listGenres.SelectedIndex = listGenres.Items.Count - 1
        End If
        Me.TopMost = True
    End Sub

    Private Sub remButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles remButton2.Click
        If Not listGenres.SelectedIndex = -1 Then
            MsgBox("Folders and tracks associated with this genre are not deleted, but may get overwritten.", MsgBoxStyle.Information)
            Dim selGenre As Genre = listGenres.SelectedItem
            Genre.genres.Remove(selGenre)
            listGenres.Items.RemoveAt(listGenres.SelectedIndex)
            Genre.writeGenres()
        End If
    End Sub

    Private Sub listGenres_SelectedIndexChanged(sender As Object, e As EventArgs) Handles listGenres.SelectedIndexChanged
        If listGenres.SelectedIndex > -1 Then
            refillAssociations(listGenres.SelectedItem)
        End If
    End Sub

    Sub refillAssociations(genre As Genre)

        listAssociations.Items.Clear()

        Genre.updateTrackAssociations()
        Genre.updateFolderAssociations()

        Dim tempList As New List(Of Object)
        If genre.Equals(Genre.Undefined) Then
            tempList.Add("Select Genre...")
        Else
            For i = 0 To genre.folders.Count - 1
                If i = 0 Then tempList.Add("--------------[Folders]--------------")
                tempList.Add(genre.folders(i))
            Next

            For i = 0 To genre.tracks.Count - 1
                If i = 0 Then
                    If tempList.Count > 0 Then tempList.Add("")
                    tempList.Add("--------------[Tracks]--------------")
                End If
                tempList.Add(genre.tracks(i))
            Next
        End If
        listAssociations.Items.AddRange(tempList.ToArray)
    End Sub

    Private Sub remGenreDepButton_Click(sender As Object, e As EventArgs) Handles remGenreDepButton.Click
        If listGenres.SelectedIndex > -1 Then
            If listAssociations.SelectedIndex > -1 Then
                If TypeOf listAssociations.SelectedItem Is Track Then
                    Dim tr As Track = listAssociations.SelectedItem
                    dll.iniDeleteKey(IniSection.GENRES, tr.name, inipath)
                    listGenres.SelectedItem.tracks.remove(tr)
                    listAssociations.Items.RemoveAt(listAssociations.SelectedIndex)
                ElseIf TypeOf listAssociations.SelectedItem Is Folder Then
                    Dim fol As Folder = listAssociations.SelectedItem
                    dll.iniDeleteKey(IniSection.GENRES, fol.fullPath, inipath)
                    listGenres.SelectedItem.folders.remove(fol)
                    fol.genre = Genre.Undefined
                    listAssociations.Items.RemoveAt(listAssociations.SelectedIndex)
                End If
            End If
        End If
    End Sub

    Private Sub clearGenreDepButton_Click(sender As Object, e As EventArgs) Handles clearGenreDepButton.Click
        If listGenres.SelectedIndex > -1 Then
            For Each it As Object In listAssociations.Items
                If TypeOf it Is Track Then
                    Dim tr As Track = it
                    dll.iniDeleteKey(IniSection.GENRES, tr.name, inipath)
                    listGenres.SelectedItem.tracks.remove(tr)
                ElseIf TypeOf it Is Folder Then
                    Dim fol As Folder = it
                    dll.iniDeleteKey(IniSection.GENRES, fol.fullPath, inipath)
                    listGenres.SelectedItem.folders.remove(fol)
                    fol.genre = Genre.Undefined
                End If
            Next
            listAssociations.Items.Clear()
        End If
    End Sub


    '#description to button
    Private Sub addTrackButton_Click(sender As Object, e As EventArgs) Handles addTrackButton.Click
        If listGenres.SelectedIndex > -1 Then
            TopMost = False
            Dim def As String = ""
            If Form1.getSelectedList().SelectedItem IsNot Nothing Then def = Form1.getSelectedList().SelectedItem.name
            Dim a As String = InputBox("Type in track name", , def)
            If Not a = "" Then
                Dim tr As Track = Track.getFirstTrack(a)
                If tr Is Nothing Then
                    MsgBox("Track not found", MsgBoxStyle.Exclamation)
                    Return
                Else
                    Dim currGenre As Genre = listGenres.SelectedItem
                    If Not dll.iniIsValidKey(IniSection.GENRES, tr.name, inipath) OrElse
                        MsgBox("Track association already exists for genre " & loadRawSetting(SettingsIdentifier.GENRES_MAPPING, tr.name) & ". Overwrite?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        saveRawSetting(SettingsIdentifier.GENRES_MAPPING, tr.name, currGenre.name)
                        refillAssociations(currGenre)
                    End If
                End If
            End If
            TopMost = True
        End If
    End Sub

    Private Sub addFolderButton_Click(sender As Object, e As EventArgs) Handles addFolderButton.Click
        If listGenres.SelectedIndex > -1 Then
            NodeSelectionForm.selectNodes()
            For i = 0 To NodeSelectionForm.selNodes.Count - 1
                Dim currFol As Folder = NodeSelectionForm.selNodes(i)
                If currFol IsNot Nothing Then
                    If Not listAssociations.Items.Contains(currFol) Then
                        Dim currGenre As Genre = listGenres.SelectedItem
                        Dim containedIn As Genre = currGenre.folderAssociationExists(currFol)
                        If containedIn.Equals(Genre.Undefined) _
                            OrElse MsgBox("Folder '" & currFol.nodePath & "'" & " already associated to Genre '" & containedIn.name & "'" & vbNewLine & "Overwrite association?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                            saveRawSetting(SettingsIdentifier.GENRES_MAPPING, currFol.fullPath, currGenre.name)
                            refillAssociations(currGenre)
                        End If
                    End If
                End If
            Next
        End If
    End Sub

#End Region 'Genres

#Region "Nodes"


    'Sub selectNode(multiSelect As Boolean)
    '    nodeSelection = True
    '    If Not multiSelect Then dll.ExtendArray(arguments, "single")
    '    TopMost = False
    '    NodeSelectionForm.TopMost = True
    '    NodeSelectionForm.tvSelection.Nodes.Clear()
    '    NodeSelectionForm.tvSelection.SelectedNode = Nothing
    '    NodeSelectionForm.ShowDialog()
    'End Sub



    Private Sub okButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If state = optionState.GENRES Then

        ElseIf state = optionState.PLAYLISTS Then

        Else
            'ReDim selNodes(listNodes.Items.Count - 1)
            'For i = 0 To listNodes.Items.Count - 1
            '    selNodes(i) = listNodes.Items(i).nodePath
            'Next
            'endNodeSelection()
            'Me.Close()
        End If
    End Sub



#End Region 'Nodes


#Region "Radio"
    Private Sub listStations_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles listStations.SelectedIndexChanged
        If listStations.SelectedIndex > -1 Then
            labelUrl.Text = "URL: " & listStations.SelectedItem.url
        End If
    End Sub

    Private Sub sortingBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sortingBox.SelectedIndexChanged
        If listStations.Items.Count > 0 Then
            saveSetting(SettingsIdentifier.RADIO_SORT, sender.selectedIndex)

            Dim rad As Radio = Nothing
            If listStations.SelectedItem IsNot Nothing Then
                rad = listStations.SelectedItem
            End If

            listStations.Items.Clear()
            Dim rads As List(Of Radio) = Radio.getStations()
            If sortingBox.SelectedIndex = 1 Then
                rads.Sort(Function(x, y) y.time.CompareTo(x.time))
            End If
            listStations.Items.AddRange(rads.ToArray)
            If rads.Count > 0 Then
                For i = 0 To listStations.Items.Count - 1
                    If listStations.Items(i).name = rad.name Then
                        listStations.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub moveUpButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles moveUpButton.Click
        If listStations.SelectedIndex > 0 Then
            listStations.Items.Insert(listStations.SelectedIndex - 1, listStations.SelectedItem)
            Dim ind As Integer = listStations.SelectedIndex
            listStations.Items.RemoveAt(listStations.SelectedIndex)
            listStations.SelectedIndex = ind - 2
        End If
    End Sub
    Private Sub moveDownButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles moveDownButton.Click
        If listStations.SelectedIndex > -1 And listStations.SelectedIndex < listStations.Items.Count - 1 Then
            listStations.Items.Insert(listStations.SelectedIndex + 2, listStations.SelectedItem)
            Dim ind As Integer = listStations.SelectedIndex
            listStations.Items.RemoveAt(listStations.SelectedIndex)
            listStations.SelectedIndex = ind + 1
        End If
    End Sub
    Private Sub changeUrlButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles changeUrlButton.Click
        If listStations.SelectedItem IsNot Nothing Then
            TopMost = False
            Dim a As String = InputBox("Paste new URL to overwrite existing URL", , listStations.SelectedItem.url)
            If Not a = "" Then
                saveRawSetting(SettingsIdentifier.RADIO_STATIONS, listStations.SelectedItem.name, a)
                listStations.SelectedItem.url = a
                labelUrl.Text = "URL: " & listStations.SelectedItem.url
            End If
            TopMost = True
        End If

    End Sub
    Private Sub changeNameButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles changeNameButton.Click
        If listStations.SelectedItem IsNot Nothing Then
            TopMost = False
            Dim a As String = InputBox("Type in new name", , listStations.SelectedItem.name)
            If Not a = "" And Not a = listStations.SelectedItem.name Then
                dll.iniRenameKey(IniSection.RADIO, listStations.SelectedItem.name, a, inipath)
                dll.iniRenameKey(IniSection.RADIO_TIME, listStations.SelectedItem.name, a, inipath)
            End If
            TopMost = True
        End If
    End Sub
    Private Sub remButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles remButton4.Click
        If Not listStations.SelectedItem Is Nothing Then
            dll.iniDeleteKey(IniSection.RADIO, listStations.SelectedItem.name)
            Dim ind As Integer = listStations.SelectedIndex
            listStations.Items.Remove(listStations.SelectedItem)
            If listStations.Items.Count > 0 Then listStations.SelectedIndex = IIf(ind < listStations.Items.Count, ind, listStations.Items.Count - 1)
        End If
    End Sub

    Private Sub addButton_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles addButton.Click
        TopMost = False
        Dim a As String = InputBox("Type in name of radio station")
        If Not a = "" Then
            Dim b As String = InputBox("Paste radio URL")
            If Not b = "" Then
                Dim rad As New Radio(a, b)
                listStations.Items.Add(rad)
                saveRawSetting(SettingsIdentifier.RADIO_STATIONS, rad.name, rad.url)
            End If
        End If
        TopMost = True
    End Sub
#End Region 'Radio

#Region "Remote"
    Private Sub checkEnableStartup_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles checkEnableStartup.CheckedChanged
        SettingsService.saveSetting(SettingsIdentifier.REMOTE, sender.checked)
    End Sub

    Private Sub checkBlockExtIps_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles checkBlockExtIps.CheckedChanged
        SettingsService.saveSetting(SettingsIdentifier.REMOTE_BLOCK_EXT_IPS, sender.checked)
    End Sub

    Private Sub checkBlockMessages_CheckedChanged(sender As Object, e As EventArgs) Handles checkBlockMessages.CheckedChanged
        SettingsService.saveSetting(SettingsIdentifier.REMOTE_BLOCK_MESSAGES, sender.checked)
    End Sub
    Private Sub listPairedIps_SelectedIndexChanged(sender As Object, e As EventArgs) Handles listPairedIps.SelectedIndexChanged
        stopConnectionButton.Enabled = (listPairedIps.SelectedIndex > -1)
    End Sub

    Private Sub stopConnectionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopConnectionButton.Click
        If listPairedIps.SelectedIndex > -1 Then
            Form1.TcpStopConnection(CStr(listPairedIps.SelectedItem), "force")
        End If
    End Sub
    Private Sub stopAllConnectionsButton_Click(sender As Object, e As EventArgs) Handles stopAllConnectionsButton.Click
        Form1.TcpStopAllConnections("force")
    End Sub

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click
        Form1.TcpStartListener(remoteTcp.port)
    End Sub



    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click
        If Not remoteTcp.stopListener() Then MsgBox("Failure")
        setListenerStatus()
        Form1.setRemoteImage()
    End Sub

    Private Sub resetButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles resetButton2.Click
        If Not tPort.Text = "" AndAlso CInt(tPort.Text) < 65536 Then
            Form1.TcpStart(CInt(tPort.Text))
        End If
        tPort.Text = ""
    End Sub

    Private Sub tPort_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tPort.KeyPress
        Dim t As TextBox = sender
        If Not Char.IsDigit(e.KeyChar) And Not e.KeyChar = ChrW(Keys.Back) And t.TextLength < 9 Then
            e.Handled = True
        Else
            If e.KeyChar = ChrW(Keys.Back) Then
                t.Text = ""
            ElseIf Not Char.IsDigit(e.KeyChar) Then
                e.Handled = True
            End If
        End If
    End Sub

    Sub setExternalIP()
        Try
            Dim wc As New WebClient
            AddHandler wc.DownloadStringCompleted, Sub(sender As Object, e As DownloadStringCompletedEventArgs)
                                                       Try
                                                           Dim res As String = e.Result
                                                           res = res.Substring(res.IndexOf(":") + 2)
                                                           labelExtIp.Text = res.Substring(0, res.IndexOf("<"))
                                                       Catch ex As Exception
                                                           labelExtIp.Text = "N/A"
                                                       Finally
                                                           Cursor = Cursors.Default
                                                       End Try
                                                   End Sub
            Cursor = Cursors.AppStarting
            wc.DownloadStringAsync(New Uri("http://checkip.dyndns.org/"))
        Catch ex As Exception
            labelExtIp.Text = ""
        End Try
    End Sub

    Sub setListenerStatus()
        labelStatus.Text = IIf(remoteTcp.isListenerActive, "Listening", "Blocked")
        startButton.Enabled = Not remoteTcp.isListenerActive
        stopButton.Enabled = remoteTcp.isListenerActive
        resetButton2.Enabled = Not remoteTcp.isEstablished
    End Sub


    Private Sub remoteSendButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles remoteSendButton.Click
        TopMost = False
        Try
            remoteTcp.send(InputBox("IP", , "127.0.0.1"), InputBox("Port", , "55555"), InputBox("Message", , "disconnect"))
        Catch ex As Exception
            MsgBox("No message sent")
        End Try
        TopMost = True
    End Sub

    Public Sub refreshRemoteUI()
        refreshPairedIpsList()
        Dim connectionsAvailable As Boolean = (listPairedIps.Items.Count > 0)
        Dim connectionSelected As Boolean = (listPairedIps.SelectedIndex > -1)
        stopConnectionButton.Enabled = connectionSelected
        stopAllConnectionsButton.Enabled = connectionsAvailable
        setListenerStatus()
    End Sub

    Public Sub refreshPairedIpsList()
        listPairedIps.Items.Clear()
        If remoteTcp IsNot Nothing AndAlso remoteTcp.isEstablished Then
            listPairedIps.Items.AddRange(remoteTcp.getAllIps().ToArray())
        End If
    End Sub
#End Region 'Remote

#Region "Update"
    Private Sub checkAutoUpdate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles checkAutoUpdate.CheckedChanged
        SettingsService.saveSetting(SettingsIdentifier.FTP_AUTO_UPDATE, sender.checked)
    End Sub

    Private Sub publishRemButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles publishRemButton.Click
        Dim selIndex As Integer = listPublish.SelectedIndex
        If selIndex > -1 Then
            dll.publishFileList.Remove(listPublish.SelectedItem)
            listPublish.Items.RemoveAt(selIndex)
            listPublish.SelectedIndex = IIf(selIndex < listPublish.Items.Count, selIndex, IIf(listPublish.Items.Count > 0, 0, -1))
            SettingsService.saveSetting(SettingsIdentifier.FTP_PUBLISH, getPublishFiles(True))
        End If
    End Sub
    Private Sub publishAddButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles publishAddButton.Click
        Dim res() As String = Form1.getFilesDialog(My.Application.Info.DirectoryPath & "\")
        If res IsNot Nothing Then
            Dim err As String = ""
            'publishfilelist extra files not working
            For Each s As String In res
                Try
                    Dim name As String = s.Substring(s.LastIndexOf("\") + 1)
                    If Not coreFiles.Contains(name) Then
                        If Not s = My.Application.Info.DirectoryPath & s.Substring(s.LastIndexOf("\")) Then
                            IO.File.Copy(s, My.Application.Info.DirectoryPath & s.Substring(s.LastIndexOf("\")), True)
                        End If
                        If Not listPublish.Items.Contains(name) Then
                            listPublish.Items.Add(name)
                        End If
                        If Not dll.publishFileList.Contains(name) Then
                            dll.publishFileList.Add(name)
                        End If
                    End If

                Catch ex As Exception
                    err &= s & vbNewLine
                End Try
            Next
            If Not err = "" Then
                MsgBox(err = "Failed to add following files to publishing list:" & vbNewLine & vbNewLine & err)
            Else
                SettingsService.saveSetting(SettingsIdentifier.FTP_PUBLISH, getPublishFiles(True))
            End If
        End If
    End Sub

    Function getPublishFiles(ByVal excludeCore As Boolean) As String
        Dim res As String = ""
        For i = 0 To dll.publishFileList.Count - 1
            If Not excludeCore Then

            ElseIf Not coreFiles.Contains(dll.publishFileList(i)) Then
                res &= dll.publishFileList(i) & IIf(i = dll.publishFileList.Count - 1, "", ";")
            End If
        Next
        Return res
    End Function

    Public coreFiles As New List(Of String) From {"Interop.WMPLib.dll", "mp3player.exe", "dll.dll", "AXInterop.WMPLib.dll"}

    Private Sub publishButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles publishButton.Click
        If dll.ftpThread IsNot Nothing AndAlso dll.ftpThread.IsAlive Then dll.ftpThread.Abort()
        If Not dll.req.IsBusy Then
            If Form1.isValidDirectoryPath(ftpPath) Then
                addCoreFiles()
                For Each pubItem As String In listPublish.Items
                    dll.publishFileList.Add(pubItem)
                Next
                Dim pub As String = dll.publishPlayer(ftpPath)
                If Char.IsDigit(pub(0)) Then
                    labelPublishedVersion.Text = pub
                Else
                    MsgBox("Publishing failed. Error list:" & vbNewLine & pub)
                End If
            Else
                MsgBox("Enter valid FTP sharing directory.")
                init(optionState.PATHS)
            End If
        End If
    End Sub
    Private Sub publishPathButton_Click(sender As Object, e As EventArgs) Handles publishPathButton.Click
        init(optionState.PATHS)
    End Sub

    Sub addCoreFiles()
        If dll.publishFileList Is Nothing Then
            dll.publishFileList = New List(Of String)
        End If
        dll.publishFileList.Clear()
        For Each s As String In coreFiles
            dll.publishFileList.Add(s)
        Next
    End Sub

    Sub credentialsUpdate()
        SettingsService.saveSetting(SettingsIdentifier.FTP_IP, tftpIp.Text)
        SettingsService.saveSetting(SettingsIdentifier.FTP_USER, tftpUser.Text)
        SettingsService.saveSetting(SettingsIdentifier.FTP_PW, tftpPw.Text)

        dll.ftpCred.ip = tftpIp.Text
        dll.ftpCred.user = tftpUser.Text
        dll.ftpCred.pw = tftpPw.Text
    End Sub

    Private Sub DownloadLatestButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DownloadLatestButton.Click
        If dll.ftpThread IsNot Nothing AndAlso dll.ftpThread.IsAlive Then dll.ftpThread.Abort()
        If DownloadLatestButton.Text = "Download" Then

            If Not dll.req.IsBusy Then
                credentialsUpdate()
                Cursor = Cursors.WaitCursor
                If dll.ftpCheckStatus(ftpCred) Then
                    Cursor = Cursors.Default
                    DownloadLatestButton.Text = "Cancel"
                    dll.updatePlayerAsync(dll.ftpCred)
                Else
                    abortDownloadGC("Server offline")
                End If
            End If
        Else
            abortDownloadGC()
        End If
    End Sub

    Sub abortDownloadGC(Optional ByVal msg As String = "")
        DownloadLatestButton.Text = "Download"
        dll.updateIndex = 0
        pBar.Value = 0
        pBar2.Value = 0
        labelftpCurrProg.Text = "0 / 0"
        labelftpTotalProg.Text = "0 / 0"
        Cursor = Cursors.Default
        If dll.req.IsBusy Then dll.req.CancelAsync()
        If dll.updateFiles IsNot Nothing Then
            For Each fil As String In dll.updateFiles
                Try
                    If File.Exists(My.Application.Info.DirectoryPath & "\Releases\Release" & dll.updateVersionPath & "\" & fil) Then File.Delete(My.Application.Info.DirectoryPath & "\Releases\Release" & dll.updateVersionPath & "\" & fil)
                Catch ex As Exception
                End Try
            Next
        End If
        Try
            If File.Exists(My.Application.Info.DirectoryPath & "\Releases\Release" & dll.updateVersionPath & "\mp3player2.exe") Then File.Delete(My.Application.Info.DirectoryPath & "\Releases\Release" & dll.updateVersionPath & "\mp3player2.exe")
        Catch ex As Exception
        End Try
        If Not msg = "" Then
            MsgBox(msg)
            If Not msg = "Server offline" Then
                If MsgBox("Want to retry with stable download method?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    dll.updatePlayer(dll.ftpCred)
                End If

            End If
        End If

    End Sub


    Private Sub checkUpdatesButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles checkUpdatesButton.Click
        If Not dll.req.IsBusy Then
            TopMost = False
            credentialsUpdate()
            dll.checkPlayerUpdate(dll.ftpCred, False)
        End If
    End Sub

    Private Sub SearchHomeIP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles searchHomeIpButton.Click
        setHomeIP()
    End Sub

    Sub setHomeIP()
        Try
            Dim wc As New WebClient
            AddHandler wc.DownloadStringCompleted, Sub(sender As Object, e As DownloadStringCompletedEventArgs)
                                                       Try
                                                           'error: .net 4.0 only tls 1.0 -> mirgration to .net 4.x
                                                           Dim res As String = e.Result
                                                           IPAddress.Parse(res)
                                                           tftpIp.Text = res
                                                       Catch ex As Exception
                                                           tftpIp.Text = "N/A"
                                                       Finally
                                                           Cursor = Cursors.Default
                                                       End Try
                                                   End Sub
            Cursor = Cursors.AppStarting
            wc.DownloadStringAsync(New Uri("https://datmarvin.github.io/homeip/"))
        Catch ex As Exception
            tftpIp.Text = ""
        End Try
    End Sub


#End Region 'Update

#Region "Paths"
    Private Sub bStatsFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bStatsFile.Click
        Dim s As String = Form1.getFileDialog(IIf(checkFileValidity(tStatsFile) >= 0, tStatsFile.Text, My.Application.Info.DirectoryPath & "\"))
        If Not s = "" Then tStatsFile.Text = s
    End Sub
    Private Sub bMusicDir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bMusicDir.Click
        Dim s As String = Form1.getDirectoryDialog(tMusicDir.Text)
        If Not s = "" Then tMusicDir.Text = s
    End Sub
    Private Sub bPlaylistFile_Click(sender As Object, e As EventArgs) Handles bPlaylistFile.Click
        Dim s As String = Form1.getFileDialog(tPlaylistFile.Text)
        If Not s = "" Then tPlaylistFile.Text = s
    End Sub
    Private Sub bDatesFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bDatesFile.Click
        Dim s As String = Form1.getFileDialog(tDatesFile.Text)
        If Not s = "" Then tDatesFile.Text = s
    End Sub

    Private Sub logPathReloadButton_Click(sender As Object, e As EventArgs) Handles logPathReloadPic.Click
        Form1.datesInitiallyLoaded = False
        Dim res = Form1.loaddates()
        MsgBox("Reloading result: " & res)
    End Sub

    Private Sub logPathKeyButton_Click(sender As Object, e As EventArgs) Handles logPathKeyPic.Click
        Dim key As String = InputBox("Type in decryption key", , logPathKey)
        If key <> logPathKey Then
            ' Interaction.SaveSetting("mp3player", "Config", "logPathKey", key)
            '   Form1.logPathKey = key
            saveSetting(SettingsIdentifier.LOG_PATH_KEY, key)
        End If
    End Sub

    Private Sub bLyricsDir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bLyricsDir.Click
        Dim s As String = Form1.getDirectoryDialog(tLyricsDir.Text)
        If Not s = "" Then tLyricsDir.Text = s
    End Sub
    Private Sub bFtpDir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bFtpDir.Click
        Dim s As String = Form1.getDirectoryDialog(tFtpDir.Text)
        If Not s = "" Then tFtpDir.Text = s
    End Sub


    Function checkValidity(ByVal resolveError As Boolean, ByVal message As Boolean) As Boolean
        Dim s As String = ""
        If checkFileValidity(tStatsFile, resolveError) = -1 Then
            s &= "Invalid settings/statistics file." & vbNewLine
        End If
        If checkDirectoryValidity(tMusicDir, resolveError) = -1 Then
            s &= "Invalid Music root folder." & vbNewLine
        End If
        If Not s = "" Then
            If message Then
                Dim res As MsgBoxResult = MsgBox(s & vbNewLine & "Critical errors must be resolved to continue." & vbNewLine & "Try automatic fix?", MsgBoxStyle.YesNoCancel)
                If res = MsgBoxResult.Yes Then
                    If checkFileValidity(tStatsFile, False) = -1 Then
                        tStatsFile.Text = My.Application.Info.DirectoryPath & "\mp3player.ini"
                    End If
                    If checkDirectoryValidity(tMusicDir, False) = -1 Then
                        tMusicDir.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) & "\"
                    End If
                    If checkFileValidity(tStatsFile, True) >= 0 And checkDirectoryValidity(tMusicDir, True) >= 0 Then
                        Return True
                    End If
                ElseIf res = MsgBoxResult.Cancel Then
                    Environment.Exit(0)
                End If
            End If

            Return False
        End If
        Dim s2 As String = ""
        If checkFileValidity(tPlaylistFile, resolveError) = -1 Then
            s2 &= "Invalid playlist file." & vbNewLine
        End If
        If checkFileValidity(tDatesFile, resolveError) = -1 Then
            s2 &= "Invalid date log file." & vbNewLine
        End If
        If checkDirectoryValidity(tLyricsDir, resolveError) = -1 Then
            s2 &= "Invalid lyrics folder." & vbNewLine
        End If
        If checkDirectoryValidity(tFtpDir, resolveError) = -1 Then
            s2 &= "Invalid ftp sharing folder." & vbNewLine
        End If
        If Not s2 = "" Then
            If message Then MsgBox(s2 & vbNewLine & "Not resolving errors may result in misbehavior of certain features.", MsgBoxStyle.Exclamation)
        End If
        Return True
    End Function

    Function checkFileValidity(ByVal t As TextBox, Optional ByVal resolveError As Boolean = False) As Integer
        If Form1.isValidFilePath(t.Text) Then
            If File.Exists(t.Text) Then
                t.BackColor = Color.Green
                Return 1
            Else
                t.BackColor = Color.Orange 'yellow
                If resolveError Then
                    Try
                        File.Create(t.Text).Close()
                        t.BackColor = Color.Green
                    Catch ex As Exception
                        t.BackColor = Color.Red
                        Return -1
                    End Try
                End If
                Return 0
            End If
        Else

            t.BackColor = Color.Red

            Return -1
        End If
    End Function

    Function checkDirectoryValidity(ByVal t As TextBox, Optional ByVal resolveError As Boolean = False) As Integer
        If Form1.isValidDirectoryPath(t.Text) Then
            If Directory.Exists(t.Text) Then
                t.BackColor = Color.Green
                Return 1
            Else
                t.BackColor = Color.Orange
                If resolveError Then
                    Try
                        Directory.CreateDirectory(t.Text)
                        t.BackColor = Color.Green
                    Catch ex As Exception
                        t.BackColor = Color.Red
                        Return -1
                    End Try
                End If
                Return 0
            End If
        Else
            t.BackColor = Color.Red
            Return -1
        End If
    End Function


    Private Sub tStatsFile_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStatsFile.TextChanged, tDatesFile.TextChanged, tPlaylistFile.TextChanged
        checkFileValidity(sender)
    End Sub

    Private Sub tMusicDir_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tMusicDir.TextChanged, tLyricsDir.TextChanged, tFtpDir.TextChanged
        checkDirectoryValidity(sender)
    End Sub







#End Region 'Paths


#Region "Playlists"

    Private Sub listPlaylists_SelectedIndexChanged(sender As Object, e As EventArgs) Handles listPlaylists.SelectedIndexChanged
        If listPlaylists.SelectedIndex > -1 Then
            Dim folder As Folder = listPlaylists.SelectedItem
            checkHiddenPlaylist.Checked = Not folder.isExcluded
            checkIgnoreErrors.Checked = folder.ignoresErrors()
        End If
        refreshPlaylistsUI()
    End Sub

    Sub refillPlaylists(selected As Folder)
        listPlaylists.Items.Clear()
        Dim fols As List(Of Folder) = Nothing
        If radioPlaylists.Checked Then
            fols = Folder.getVirtualFolders()
        Else
            fols = Folder.getFolders(False, True)
        End If
        For Each f As Folder In fols
            If radioAll.Checked OrElse f.isExcluded Then listPlaylists.Items.Add(f)
        Next
        If selected IsNot Nothing AndAlso listPlaylists.Items.Contains(selected) Then listPlaylists.SelectedItem = selected
        refreshPlaylistsUI()
    End Sub

    Sub refillPlaylists()
        listPlaylists.Items.Clear()
        Dim fols As List(Of Folder) = Nothing
        If radioPlaylists.Checked Then
            fols = Folder.getVirtualFolders()
        Else
            fols = Folder.getFolders(False, True)
        End If
        For Each f As Folder In fols
            If radioAll.Checked OrElse f.isExcluded Then listPlaylists.Items.Add(f)
        Next
        refreshPlaylistsUI()
    End Sub


    Private Sub managePlaylistButton_Click(sender As Object, e As EventArgs) Handles managePlaylistButton.Click
        If listPlaylists.SelectedIndex > -1 Then
            Dim fol As Folder = listPlaylists.SelectedItem
            TrackSelectionForm.selectTracks(fol, TrackSelectionForm.eTrackSelectionMode.MANAGE, "Manage tracks [" & fol.nodePath & "]", IIf(radioPlaylists.Checked, Nothing, {"folder"}))
        End If
    End Sub

    Private Sub newPlaylistButton_Click(sender As Object, e As EventArgs) Handles newPlaylistButton.Click
        Dim parent As Folder = NodeSelectionForm.selectNode("Choose parent node...", IIf(radioPlaylists.Checked, Nothing, {"virtual"}))
        If parent IsNot Nothing Then
            Dim child As Folder = Nothing
            If radioPlaylists.Checked Then
                child = parent.createSubPlaylist()
            Else
                child = parent.createSubFolder()
            End If
            If child IsNot Nothing Then
                Folder.folders.Add(child)
                parent.children.Add(child)
                refillPlaylists()
                listPlaylists.SelectedItem = Folder.getFolder(child.fullPath)
                updateTvAfter = True
                TrackSelectionForm.selectTracks(child, TrackSelectionForm.eTrackSelectionMode.MANAGE, "Add Tracks to playlist...")
            End If
        End If
    End Sub

    Private Sub deletePlaylistButton_Click(sender As Object, e As EventArgs) Handles deletePlaylistButton.Click
        If listPlaylists.SelectedIndex > -1 Then
            Dim fol As Folder = listPlaylists.SelectedItem
            If fol.delete(True) Then
                listPlaylists.Items.Remove(listPlaylists.SelectedItem)
                updateTvAfter = True
            End If
        End If
    End Sub


    Private Sub checkHiddenPlaylist_Click(sender As Object, e As EventArgs) Handles checkHiddenPlaylist.Click
        If listPlaylists.SelectedIndex > -1 Then
            Dim fol As Folder = listPlaylists.SelectedItem
            fol.isExcluded = Not checkHiddenPlaylist.Checked
            Folder.writeExcludedFolders()
            refillPlaylists(fol)
            updateTvAfter = True
        End If
    End Sub

    Private Sub checkIgnoreErrors_Click(sender As Object, e As EventArgs) Handles checkIgnoreErrors.Click
        If listPlaylists.SelectedIndex > -1 Then
            Dim fol As Folder = listPlaylists.SelectedItem
            fol.writeIgnoreError(checkIgnoreErrors.Checked)
        End If
    End Sub

    Private Sub convertButton_Click(sender As Object, e As EventArgs) Handles convertButton.Click
        If listPlaylists.SelectedIndex > -1 Then
            Dim fol As Folder = listPlaylists.SelectedItem
            If radioPlaylists.Checked Then
                fol.convertToFolder()
            Else
                fol.convertToPlaylist()
            End If
            refillPlaylists()
        End If
    End Sub

    Sub refreshPlaylistsUI()
        Dim c() As Control = {managePlaylistButton, deletePlaylistButton, convertButton}
        Array.ForEach(c, Sub(x)
                             x.Enabled = listPlaylists.SelectedIndex > -1
                         End Sub)
        If radioPlaylists.Checked Then
            groupPlaylists.Text = "Current Playlists"
            newPlaylistButton.Text = "New Playlist"
            convertButton.Text = "Convert to Folder"
            checkHiddenPlaylist.Text = "Visible Playlist"
        Else
            groupPlaylists.Text = "Current Folders"
            newPlaylistButton.Text = "New Folder"
            convertButton.Text = "Convert to Playlist"
            checkHiddenPlaylist.Text = "Visible Folder"
        End If
    End Sub

    Private Sub radioHidden_CheckedChanged(sender As Object, e As EventArgs) Handles radioHidden.CheckedChanged
        refillPlaylists()
    End Sub

    Private Sub checkHiddenPlaylist_CheckedChanged(sender As Object, e As EventArgs) Handles checkHiddenPlaylist.CheckedChanged

    End Sub

    Private Sub listExclude_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub radioPlaylists_CheckedChanged(sender As Object, e As EventArgs) Handles radioPlaylists.CheckedChanged

    End Sub

    Private Sub radioFolders_CheckedChanged(sender As Object, e As EventArgs) Handles radioFolders.CheckedChanged

    End Sub

    Private Sub radioPlaylists_Click(sender As Object, e As EventArgs) Handles radioPlaylists.Click
        refillPlaylists()
    End Sub

    Private Sub radioFolders_Click(sender As Object, e As EventArgs) Handles radioFolders.Click
        refillPlaylists()
    End Sub


#End Region 'Playlists

#Region "Gadgets"
    Private Sub gadgetFormButton_Click(sender As Object, e As EventArgs) Handles gadgetFormButton.Click
        Form1.showGadgetForm(Form1.lastGadgetsState)
    End Sub
#End Region 'Gadgets


#Region "Player Settings"
    Function getFontDialogResult(Optional prevFont As Font = Nothing) As Font
        Dim fd As New FontDialog
        fd.AllowScriptChange = False
        fd.ShowEffects = False
        If prevFont IsNot Nothing Then fd.Font = prevFont
        fd.ShowDialog()
        Return fd.Font
    End Function

    Private Sub buttonFolderFont_Click(sender As Object, e As EventArgs) Handles buttonFolderFont.Click
        saveFont(Form1.tv, SettingsIdentifier.FONT_FOLDERS)
    End Sub

    Private Sub buttonLyricsFont_Click(sender As Object, e As EventArgs) Handles buttonLyricsFont.Click
        saveFont(LyricsForm.tLyrics, SettingsIdentifier.FONT_LYRICS)
    End Sub

    Private Sub buttonTrackFont_Click(sender As Object, e As EventArgs) Handles buttonTrackFont.Click
        Dim f As Font = saveFont(Form1.l2, SettingsIdentifier.FONT_TRACKS)
        Form1.l2.Font = f
        Form1.l2_2.Font = f
    End Sub

    Function saveFont(control As Control, iniKey As SettingsIdentifier) As Font
        Return saveFont(control, getFontDialogResult(control.Font), iniKey)
    End Function
    Function saveFont(control As Control, font As Font, iniKey As SettingsIdentifier) As Font
        Try
            control.Font = font
        Catch ex As Exception
        End Try
        Dim fontValue As String = font.FontFamily.Name & ";" & CInt(font.Style) & ";" & CInt(font.Size)
        SettingsService.saveSetting(iniKey, fontValue)
        Return font
    End Function

    Private Sub checkDarkTheme_Click(sender As Object, e As EventArgs) Handles checkDarkTheme.Click
        Form1.colorForm(formLocked, sender.checked)
        colorForm()
    End Sub

    Private Sub checkSavePos_Click(sender As Object, e As EventArgs) Handles checkSavePos.Click
        saveSetting(SettingsIdentifier.SAVE_WIN_POS_SIZE, sender.checked)
    End Sub

    Private Sub buttonResetWinPos_Click(sender As Object, e As EventArgs) Handles buttonResetWinPos.Click
        Form1.WindowState = FormWindowState.Normal
        Form1.Size = New Size(Form1.minWidth, Form1.minHeight)
        Form1.Location = New Point(My.Computer.Screen.WorkingArea.Width / 2 - Form1.Width / 2, My.Computer.Screen.WorkingArea.Height / 2 - Form1.Height / 2)

    End Sub

    Private Sub trackbarBalance_Scroll(sender As Object, e As EventArgs) Handles trackbarBalance.Scroll
        Form1.setBalance(trackbarBalance.Value)
    End Sub

    Private Sub trackbarPlayRate_Scroll(sender As Object, e As EventArgs) Handles trackbarPlayRate.Scroll
        Form1.setPlayRate(numToScale(trackbarPlayRate.Value))
    End Sub

    Function numToScale(n As Integer) As Double
        Select Case n
            Case 0 : Return 0.5
            Case 1 : Return 0.75
           ' Case 2 : Return 1.0
            Case 3 : Return 1.1
            Case 4 : Return 1.2
            Case 5 : Return 1.3
            Case 6 : Return 1.4
            Case 7 : Return 1.5
            Case 8 : Return 1.75
            Case 9 : Return 2.0
            Case Else : Return 1.0
        End Select
    End Function

    Function scaleToNum(n As Double) As Integer
        Select Case n
            Case 0.5 : Return 0
            Case 0.75 : Return 1
           ' Case 2 : Return 1.0
            Case 1.1 : Return 3
            Case 1.2 : Return 4
            Case 1.3 : Return 5
            Case 1.4 : Return 6
            Case 1.5 : Return 7
            Case 1.75 : Return 8
            Case 2.0 : Return 9
            Case Else : Return 2
        End Select
    End Function

    Private Sub buttonProperties_Click(sender As Object, e As EventArgs) Handles buttonProperties.Click
        Form1.wmp.ShowPropertyPages()
    End Sub
    Private Sub checkRandomNextTrack_Click(sender As Object, e As EventArgs) Handles checkRandomNextTrack.Click
        saveSetting(SettingsIdentifier.RANDOM_NEXT_TRACK, sender.checked)
    End Sub

    Private Sub checkRemoveTrackFromList_Click(sender As Object, e As EventArgs) Handles checkRemoveTrackFromList.Click
        saveSetting(SettingsIdentifier.REMOVE_NEXT_TRACK, sender.checked)
    End Sub

    Private Sub checkPlaylistHistory_Click(sender As Object, e As EventArgs) Handles checkPlaylistHistory.Click
        saveSetting(SettingsIdentifier.PLAYLIST_SAVE_HISTORY, sender.checked)
    End Sub

    Private Sub checkRandomNextTrack_CheckedChanged(sender As Object, e As EventArgs) Handles checkRandomNextTrack.CheckedChanged

    End Sub

    Private Sub checkSavePos_CheckedChanged(sender As Object, e As EventArgs) Handles checkSavePos.CheckedChanged

    End Sub





#End Region 'Player Settings

End Class