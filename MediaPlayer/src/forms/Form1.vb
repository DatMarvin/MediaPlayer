
#Region "Imports"
Imports System.IO
Imports WMPLib
Imports System.Runtime.InteropServices
Imports System.IO.Compression
Imports System.Text
Imports MediaPlayer.PlayerEnums
Imports MediaPlayer.SettingsService
Imports MediaPlayer.SettingsEnums
#End Region

Public Class Form1

#Region "Variables"
    Public Const minWidth As Integer = 908
    Public Const minHeight As Integer = 577

    Public dll As New Utils
    ReadOnly rnd As New Random


    Dim lockChange As Boolean = False

    Public currTrack As Track
    Public last As Track
    Public trackLoop As LoopMode
    Public loopVals(2) As Double
    Public gldt As New List(Of String)
    Public glnames As New List(Of String)

    ReadOnly Property currTrackPart As TrackPart
        Get
            Return currTrack.currPart
        End Get
    End Property

    Dim dItem As Track
    Dim dragList As ListBox

    Dim firstPlayStart As FirstStartState

    Public root As String

    Dim lastTCPCommand As String
    Public remoteTcp As New Tcp()

    Public playlist As New List(Of Track)

    Public lastOptionsState As OptionsForm.optionState


    Dim currLyrTrack As Track
    Public overlayMode As eOverlayMode

    Public searchState As SearchState

    '   Public savePlaylistHistory As Boolean

    Public lastGadgetsState As GadgetsForm.GadgetState

    Public WithEvents dragDropNextField As Button
    Public WithEvents dragDropQueueField As Button

    Public datesInitiallyLoaded As Boolean = False

#End Region


#Region "Form1"

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        SettingsService.saveSetting(SettingsIdentifier.VOLUME, wmp.settings.volume)
        SettingsService.saveSetting(SettingsIdentifier.PLAY_MODE, playMode)
        SettingsService.saveSetting(SettingsIdentifier.MUSIC_SOURCE, CInt(radioEnabled))
        SettingsService.saveSetting(SettingsIdentifier.WIN_MAX, WindowState = FormWindowState.Maximized)

        If keylogger Then
            KeyloggerModule.keyloggerDestroy()
        End If

        If l2.SelectedIndex > -1 And tv.SelectedNode IsNot Nothing And Not radioEnabled And Not last = Nothing Then
            saveLastTrack()
        End If

        If SettingsService.getSetting(SettingsIdentifier.PLAYLIST_SAVE_HISTORY) Then
            saveCurrPlaylistHistory()
        End If

        If radioEnabled Then saveRadioTime()

        TcpStopAllConnections("close", False)
    End Sub

    Private Sub Form1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F8 Then e.Handled = True
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        VersionUpdateService.checkForVersionUpdate()

        MinimumSize = New Size(minWidth, minHeight)

        SettingsService.initSystemSettings()

        Folder.setTopFolder(getSetting(SettingsIdentifier.PATH))
        Folder.invalidateFolders(Folder.top)
        Track.invalidateTracks(True)

        initPlayerSettings()


        Key.initKeys()

        FileSystemWatcher.fswInit()


        currTrack = Nothing
        wmp.settings.balance = balance
        wmp.settings.rate = playRate

        formResize()
        FormUtils.colorForm(Me)

        AutoStarts.executeAutoStarts()


        KeyloggerModule.keyloggerInit()

        GadgetsForm.initMacrosTable()

        updatePlayMode()

        alltime.Start()
        keyt.Start()
        clickcountt.Start()
        radiotimer.Start()
        iniValT.Start()

        If My.Application.CommandLineArgs.Count = 0 Then

            If radioEnabled Then
                changeSourceMode(1)
            Else
                localfill()
            End If

            If getSetting(SettingsIdentifier.PLAYLIST_SAVE_HISTORY) Then
                Dim pairs As List(Of KeyValuePair(Of String, String)) = IniService.iniGetAllPairs(IniSection.HISTORY)
                If pairs IsNot Nothing Then
                    For Each p As KeyValuePair(Of String, String) In pairs
                        Dim addTrack As Track = Nothing
                        If Folder.getSelectedFolder(tv).tracks.TrueForAll(Function(x)
                                                                              Return x.fullPath <> p.Value
                                                                          End Function) Then
                            addTrack = New Track(Me, p.Value)
                        Else
                            addTrack = Track.getTrack(p.Value)
                        End If
                        addTrack.addToPlaylist()
                    Next
                End If
                Dim prioTrack As Track = IIf(currTrack = Nothing, last, currTrack)
                If Not prioTrack = Nothing Then
                    prioTrack.selectPlaylist()
                End If
                IniService.iniDeleteSection(IniSection.HISTORY)
            End If
            If Not last = Nothing Then
                Dim l As ListBox = getSelectedList()
                If l.SelectedItem IsNot Nothing AndAlso l.SelectedItem.name = last.name Then
                    If SettingsService.loadSetting(SettingsIdentifier.LAST_TRACK_RECORDED_TIME) > 0.0 Then
                        SettingsService.saveSetting(SettingsIdentifier.LAST_TRACK_APPLY_TIME, SettingsService.getSetting(SettingsIdentifier.LAST_TRACK_RECORDED_TIME))
                        SettingsService.saveSetting(SettingsIdentifier.LAST_TRACK_RECORDED_TIME, 0.0)
                    End If
                End If
            ElseIf Not radioEnabled Then
                If l2.Items.Count > 0 Then l2.SelectedIndex = rnd.Next(0, l2.Items.Count)
            End If

            '   savePaths()

        ElseIf My.Application.CommandLineArgs.Count > 0 Then
            Dim para As String = My.Application.CommandLineArgs(0)
            If File.Exists(para) Then
                setSetting(SettingsIdentifier.MUSIC_SOURCE, MusicSource.LOCAL)
                If Not para.ToLower.StartsWith(Folder.top.fullPath.ToLower) Then
                    Folder.setTopFolder(Mid(para, 1, para.LastIndexOf("\") + 1))
                End If

                localfill()

                Dim argTracks As New List(Of Track)
                For i = 0 To My.Application.CommandLineArgs.Count - 1
                    Dim curr As String = My.Application.CommandLineArgs(i)
                    If Not Folder.folders.Contains(Folder.getFolder(curr.Substring(0, curr.LastIndexOf("\") + 1))) Then
                        Folder.folders.Add(New Folder(curr.Substring(0, curr.LastIndexOf("\") + 1)))
                        Folder.folders(Folder.folders.Count - 1).tracks = New List(Of Track)
                    End If
                    argTracks.Add(New Track(Me, My.Application.CommandLineArgs(i)))
                    Dim currFolder As Folder = Folder.getFolder(curr.Substring(0, curr.LastIndexOf("\") + 1))
                    currFolder.tracks.Add(argTracks(argTracks.Count - 1))
                Next

                Dim conNode() As TreeNode = tv.Nodes.Find(Track.getTrack(para).dir, True)
                If conNode.Length > 0 Then
                    tv.SelectedNode = conNode(0)
                End If

                For Each t As Track In argTracks
                    t.addToPlaylist()
                Next
                playlist(0).play()
            Else
                setSetting(SettingsIdentifier.MUSIC_SOURCE, MusicSource.LOCAL)
                localfill()
                initSearch()
                tSearch.Text = para
            End If
        End If

    End Sub

    Sub loadWinPosSize()
        Dim x, y, w, h As Integer

        Dim siz As String = SettingsService.getSetting(SettingsIdentifier.WIN_SIZE)
        Try
            w = siz.Split(";")(0)
            h = siz.Split(";")(1)
            If w < minWidth Then w = minWidth
            If h < minHeight Then h = minHeight
        Catch ex As Exception
            w = minWidth : h = minHeight
        End Try
        Dim pos As String = SettingsService.getSetting(SettingsIdentifier.WIN_POS)
        Try
            x = pos.Split(";")(0)
            y = pos.Split(";")(1)
        Catch ex As Exception
            x = My.Computer.Screen.WorkingArea.Width / 2 - Width / 2
            y = My.Computer.Screen.WorkingArea.Height / 2 - Height / 2
        End Try
        Size = New Size(w, h)
        Location = New Point(x, y)
        If SettingsService.getSetting(SettingsIdentifier.WIN_MAX) Then WindowState = FormWindowState.Maximized
        formResize()
    End Sub

#End Region

#Region "Timer"


    Private Sub keys_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles keyt.Tick
        If Not formLocked And (Not keylogger Or KeyloggerModule.allowHotkeys) Then keyPressHandler()
    End Sub

    Private Sub globalKeyPressHandler()
        If Not keylogger Or KeyloggerModule.allowHotkeys Then
            If Key.keyList(Key.keyName.Hotkey_Toggle).pressed Then
                If Not optionsMode Then
                    lockFormSwitch()
                    keydelay(200)
                End If
            End If
        End If
    End Sub

    Sub macroKeyPressHandler()

        If macrosEnabled Then
            For i = 0 To GadgetsForm.MACROS_COUNT - 1
                Dim macro As GadgetsForm.Macro = GadgetsForm.macros(i)
                If macro.active Then
                    If Not formLocked OrElse macro.hotkeyOverride Then
                        If Not String.IsNullOrEmpty(macro.path) Then
                            If Key.keyList(37 + i).pressed Then
                                Try
                                    Process.Start(macro.path, macro.args)
                                Catch ex As Exception
                                    Throw ex '  MsgBox(ex)
                                End Try
                                keydelay()
                            End If
                        End If
                    End If
                End If
            Next

            '#########Screenshot Legacy 19.06.2020
            'If My.Computer.Clipboard.ContainsImage Then
            '    Dim img As Drawing.Image = My.Computer.Clipboard.GetImage()
            '    img.Save(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\sc.png")
            '    Process.Start("mspaint", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\sc.png")
            '    keydelay()
            'End If
        End If
    End Sub

    Private Sub keyPressHandler()
        If lockChange Then
            lockChange = False
            Exit Sub
        End If

        Dim pressed As New List(Of Key)
        For Each k As Key In Key.keyList
            If k.pressed() Then
                pressed.Add(k)
            End If
        Next
        If pressed.Count > 0 Then
            For Each k As Key In pressed
                k.execute()
            Next
        End If

    End Sub
    Public Sub keyExecute(ByVal kNum As Key.keyName)
        keyExecute(Key.keyList(kNum))
    End Sub
    Public Sub keyExecute(ByVal k As Key)
        Select Case k.name
            Case Key.keyName.Play_Pause
                If wmp.playState = WMPLib.WMPPlayState.wmppsPlaying Then
                    If radioEnabled Then
                        wmp.settings.mute = Not wmp.settings.mute
                    Else
                        wmp.Ctlcontrols.pause()
                    End If
                ElseIf wmp.playState = WMPLib.WMPPlayState.wmppsPaused Then
                    If Not radioEnabled Then
                        wmp.Ctlcontrols.play()
                    Else
                        saveRadioTime()
                        tv.Enabled = False
                        l2_2.Enabled = False
                        wmpstart(l2.SelectedItem)
                    End If
                ElseIf wmp.playState = WMPLib.WMPPlayState.wmppsUndefined Then
                    If Not radioEnabled Then
                        If l2.SelectedIndex = -1 Then
                            setlistselected()
                            playlist(l2_2.SelectedIndex).play()
                        Else
                            l2.SelectedItem.play()
                        End If
                    Else
                        saveRadioTime()
                        wmpstart(l2.SelectedItem)
                    End If
                Else

                End If
                keydelay()

            Case Key.keyName.Next_Track
2:              If Not radioEnabled Then
                    playNextTrack()
                    keydelay()
                Else
                    If Not l2.SelectedIndex = l2.Items.Count - 1 Then
                        saveRadioTime()
                        l2.SelectedIndex += 1
                        wmpstart(l2.SelectedItem)
                        keydelay()
                    End If
                End If
                Exit Sub
            Case Key.keyName.Previous_Track
3:              If Not radioEnabled Then
                    playPrevTrack()
                    keydelay()
                Else
                    If Not l2.SelectedIndex = 0 Then
                        saveRadioTime()
                        l2.SelectedIndex -= 1
                        wmpstart(l2.SelectedItem)
                        keydelay()
                    End If
                End If
            Case Key.keyName.Volume_Mute
                wmp.settings.mute = Not wmp.settings.mute
                keydelay()
            Case Key.keyName.Volume_Min
                wmp.settings.volume = 1
                keydelay(100)
            Case Key.keyName.Volume_Half
                wmp.settings.volume = 50
                keydelay()
            Case Key.keyName.Volume_Max
                wmp.settings.volume = 100
                keydelay(100)
            Case Key.keyName.Volume_Down
                If wmp.settings.volume > 0 Then
                    If wmp.settings.volume < 51 Then
                        wmp.settings.volume -= Int(wmp.settings.volume / 5) + 1
                    ElseIf wmp.settings.volume < 76 Then
                        wmp.settings.volume = 50
                    Else
                        wmp.settings.volume = 75
                    End If
                    keydelay(50)
                End If
            Case Key.keyName.Volume_Up
                If wmp.settings.volume < 100 Then
                    If wmp.settings.volume < 50 Then
                        wmp.settings.volume += Int(wmp.settings.volume / 5) + 1
                    ElseIf wmp.settings.volume < 75 Then
                        wmp.settings.volume = 75
                    Else
                        wmp.settings.volume = 100
                    End If
                    keydelay(50)
                End If
            Case Key.keyName.Fast_Forward
                If wmp.playState = WMPLib.WMPPlayState.wmppsPlaying Then
                    Try
                        wmp.Ctlcontrols.currentPosition += 5
                    Catch ex As Exception
                    End Try
                End If
            Case Key.keyName.Slow_Forward
                If wmp.playState = WMPLib.WMPPlayState.wmppsPlaying Then
                    Try
                        wmp.Ctlcontrols.currentPosition += 1
                    Catch ex As Exception
                    End Try
                End If
            Case Key.keyName.Fast_Rewind
                If wmp.playState = WMPLib.WMPPlayState.wmppsPlaying Then
                    Try
                        wmp.Ctlcontrols.currentPosition -= 5
                    Catch ex As Exception
                    End Try
                End If
            Case Key.keyName.Slow_Rewind
                If wmp.playState = WMPLib.WMPPlayState.wmppsPlaying Then
                    Try
                        wmp.Ctlcontrols.currentPosition -= 1
                    Catch ex As Exception
                    End Try
                End If
            Case Key.keyName.Repeat_Mode
                changePlayMode(PlayMode.REPEAT)
            Case Key.keyName.Random_Mode
                changePlayMode(PlayMode.RANDOM)
            Case Key.keyName.Source_Local
                changeSourceMode(0)
            Case Key.keyName.Source_Radio
                changeSourceMode(1)
            Case Key.keyName.Tree_Up
                If Not radioEnabled Then
                    If Not IsNothing(tv.SelectedNode.PrevNode) Or Not IsNothing(tv.SelectedNode.Parent) Then
                        If Not IsNothing(tv.SelectedNode.PrevNode) Then
                            tv.SelectedNode = tv.SelectedNode.PrevNode
                        ElseIf Not IsNothing(tv.SelectedNode.Parent) Then
                            tv.SelectedNode = tv.SelectedNode.Parent
                        End If
                        tv_AfterSelectSUB()
                        keydelay()
                    End If
                End If
            Case Key.keyName.Tree_Down
                If Not radioEnabled Then
                    If Not IsNothing(tv.SelectedNode.NextNode) Then
                        tv.SelectedNode = tv.SelectedNode.NextNode
                    Else
                        If tv.SelectedNode.Nodes.Count > 0 Then
                            tv.SelectedNode = tv.SelectedNode.Nodes(0)
                        End If
                    End If
                    tv_AfterSelectSUB()
                    keydelay()
                End If
            Case Key.keyName.Track_ToQueue
                'File.Copy(l2_2.SelectedItem.fullpath, "C:\users\marvin\music\Chillen\" & l2_2.SelectedItem.name & ".mp3")
                'l2_2.SelectedIndex += 1
                'wmpstart(l2_2.Items(l2_2.SelectedIndex))
                'keydelay()
                'Exit Sub
                TrackToQueue()
            Case Key.keyName.Track_PlayNext
                Dim l As ListBox = getSelectedList()
                Dim selTrack As Track = l.SelectedItem
                selTrack.playNext()
                If l Is l2 Then
                    If removeNextTrack Then
                        l2.Items.Remove(selTrack)
                        l2.SelectedIndex = -1
                    End If
                End If
                keydelay()
            Case Key.keyName.Track_Remove
                If removeItem(True) Then
                    keydelay()
                End If
            Case Key.keyName.Track_Delete
                If deleteTrack(getSelectedTrack(), False) Then
                    keydelay()
                End If
            Case Key.keyName.Track_Loop
                If Not radioEnabled Then
                    If wmp.Ctlcontrols.currentPosition >= 0 Then
                        If trackLoop = LoopMode.NO Then
                            trackLoop = LoopMode.INTERMEDIATE
                            labelLoop.Cursor = Cursors.Hand
                            loopVals(1) = wmp.Ctlcontrols.currentPosition
                            labelStatsUpdate()
                            keydelay(200)
                        ElseIf trackLoop = LoopMode.INTERMEDIATE Then
                            trackLoop = LoopMode.YES
                            labelLoop.Cursor = Cursors.Hand
                            loopVals(2) = wmp.Ctlcontrols.currentPosition
                            keydelay()
                            keydelay(150)
                        ElseIf trackLoop = LoopMode.YES Then
                            resetLoop()
                            keydelay(100)
                        End If
                    End If
                End If
            Case Key.keyName.Search
                If Not radioEnabled Then
                    If Not ContainsFocus Then
                        Key.keyList.Item(Key.keyName.Restore_Window).execute()
                    End If
                    tSearch.Focus()
                    initSearch()
                    keydelay()
                End If
            Case Key.keyName.Next_Part
                switchpart(2)
                keydelay()
            Case Key.keyName.Previous_Part
                switchpart(1)
                keydelay()
            Case Key.keyName.Count_Sub
                Dim l As ListBox = getSelectedList()
                If l IsNot Nothing Then
                    saveRawSetting(SettingsIdentifier.TRACKS_COUNT, l.SelectedItem, l.SelectedItem.count - 1)
                    labelStatsUpdate(l)
                End If
                keydelay(150)
            Case Key.keyName.Count_Add
                '    wmp.Ctlcontrols.pause() : Dim SAPI : SAPI = CreateObject("SAPI.spvoice") 
                ': SAPI.speak(l2.SelectedItem) : wmp.Ctlcontrols.play()
                Dim l As ListBox = getSelectedList()
                If l IsNot Nothing Then
                    Dim c As Integer = l.SelectedItem.count
                    If c = 0 Then
                        saveRawSetting(SettingsIdentifier.TRACKS_COUNT, l.SelectedItem, loadRawSetting(SettingsIdentifier.TRACKS_COUNT, l.SelectedItem.name) + 1)
                    Else
                        saveRawSetting(SettingsIdentifier.TRACKS_COUNT, l.SelectedItem, l.SelectedItem.count + 1)
                        labelStatsUpdate(l)
                    End If
                End If
                keydelay(150)
            Case Key.keyName.Clicker_On
                If autoClicker Then clickerTimer.Start()
            Case Key.keyName.Clicker_Off
                'handled in alltime
            Case Key.keyName.Cursor_Up
                If cursorMover Then
                    Cursor.Position = New Point(Cursor.Position.X, Cursor.Position.Y - cursorMoverIncr)
                    keydelay(cursorMoverDelay)
                End If
            Case Key.keyName.Cursor_Right
                If cursorMover Then
                    Cursor.Position = New Point(Cursor.Position.X + cursorMoverIncr, Cursor.Position.Y)
                    keydelay(cursorMoverDelay)
                End If

            Case Key.keyName.Cursor_Down
                If cursorMover Then
                    Cursor.Position = New Point(Cursor.Position.X, Cursor.Position.Y + cursorMoverIncr)
                    keydelay(cursorMoverDelay)
                End If
            Case Key.keyName.Cursor_Left
                If cursorMover Then
                    Cursor.Position = New Point(Cursor.Position.X - cursorMoverIncr, Cursor.Position.Y)
                    keydelay(cursorMoverDelay)
                End If
            Case Key.keyName.Restore_Window
                Utils.SwitchTo(Process.GetCurrentProcess.MainWindowHandle)
        End Select
    End Sub

    Sub keydelay(Optional ByVal ms As Integer = 0)
        keydelayt.Interval = IIf(ms = 0, delayMs, ms)
        keydelayt.Start()
        keyt.Stop()
    End Sub
    Private Sub keydelayt_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles keydelayt.Tick
        keydelayt.Stop()
        keydelayt.Interval = delayMs
        If Not formLocked Then keyt.Enabled = True
    End Sub

    Private Sub iniValT_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles iniValT.Tick
        If wmp.playState = WMPLib.WMPPlayState.wmppsPlaying Then
            If Not radioEnabled Then
                If currTrack IsNot Nothing Then currTrack.currPart = currTrack.getCurrentPart(wmp.Ctlcontrols.currentPosition)
                If Not last = Nothing Then saveLastTrack()

                If Not currTrack = Nothing Then
                    SettingsService.saveSetting(SettingsIdentifier.LAST_TRACK_RECORDED_TIME, wmp.Ctlcontrols.currentPosition)
                    If wmp.currentMedia IsNot Nothing Then
                        Try
                            If wmp.currentMedia.duration > 0 Then
                                saveRawSetting(SettingsIdentifier.TRACKS_TIME, currTrack.name, wmp.currentMedia.duration)
                            End If
                            labelStatsUpdate()
                        Catch ex As Exception

                        End Try

                    End If
                End If
            End If
            SettingsService.saveSetting(SettingsIdentifier.VOLUME, wmp.settings.volume)
            SettingsService.saveSetting(SettingsIdentifier.PLAY_MODE, playMode)
        End If
    End Sub

    Private Sub alltime_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles alltime.Tick
        updateUI()

        TCPHandler()

        globalKeyPressHandler()

        macroKeyPressHandler()

        clickGadgetHandler()

        playStateHandler()

    End Sub

    Private Sub radiotimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radiotimer.Tick
        If radioEnabled AndAlso wmp.playState = WMPLib.WMPPlayState.wmppsPlaying Then
            If Not wmp.playState = WMPLib.WMPPlayState.wmppsTransitioning Then
                If l2.SelectedItem IsNot Nothing Then l2.SelectedItem.timetemp = wmp.Ctlcontrols.currentPosition
            End If
        End If
    End Sub


    Private Sub clicker_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clickerTimer.Tick
        For i = 1 To autoClickerRep
            Utils.lMouseClick()
        Next
    End Sub

    Private Sub clickcountt_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clickcountt.Tick
        ClickGadget.flushClickCounter()
    End Sub

    Private Sub keyloggerTimer_Tick(sender As Object, e As EventArgs) Handles keyloggerTimer.Tick
        KeyloggerModule.handleTimerTick()
    End Sub



#Region "File System Watcher"

    Private Sub fsw_Changed(sender As Object, e As FileSystemEventArgs) Handles fsw.Changed, fsw.Created, fsw.Deleted
        FileSystemWatcher.handleCreatedChangedDeleted(e)
    End Sub

    Private Sub fsw_Renamed(sender As Object, e As RenamedEventArgs) Handles fsw.Renamed
        FileSystemWatcher.handleRenamed(e)
    End Sub

    Private Sub fswSleep_Tick(sender As Object, e As EventArgs) Handles fswSleep.Tick
        FileSystemWatcher.handleFswSleepTimer()
    End Sub
#End Region



#End Region

#Region "Menu Strip"

#Region "Track Stats/Total Time"

    Sub openOverlay(mode As eOverlayMode)
        Select Case mode
            Case eOverlayMode.LYRICS
                overlayMode += eOverlayMode.LYRICS
                If LyricsForm.Visible Then
                    LyricsForm.BringToFront()
                Else
                    LyricsForm.Show()
                End If

            Case eOverlayMode.PARTS
                overlayMode += eOverlayMode.PARTS
                If PartsForm.Visible Then
                    PartsForm.BringToFront()
                Else
                    PartsForm.Show()
                End If

            Case >= eOverlayMode.STATS_TRACKS
                overlayMode += mode
                If StatsForm.Visible Then
                    StatsForm.BringToFront()
                Else
                    StatsForm.Show()
                End If
        End Select
    End Sub

    Sub closeOverlay(mode As eOverlayMode)
        Select Case mode
            Case eOverlayMode.LYRICS
                overlayMode -= eOverlayMode.LYRICS
                LyricsForm.Close()

            Case eOverlayMode.PARTS
                overlayMode -= eOverlayMode.PARTS
                PartsForm.Close()

            Case >= eOverlayMode.STATS_TRACKS
                overlayMode -= mode
                StatsForm.Close()
        End Select
    End Sub

    Function overlayModeContains(mode As eOverlayMode) As Boolean
        Dim comp As List(Of Integer) = dll.getBinaryComponents(overlayMode)
        For i = 0 To comp.Count - 1
            If comp(i) = mode Then
                Return True
            End If
        Next
        Return False
    End Function
#End Region

#Region "Local Source/Playlist"
    Private Sub ImportPlaylistToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuSourceExternalMedia.Click
        If Not radioEnabled Then
            If Not searchState = SearchState.NONE Then cancelSearch()
            Dim ao As New OpenFileDialog
            ao.Multiselect = True
            ao.ShowDialog()
            If Not ao.FileNames.Count = 0 Then
                Dim addFolder As Folder = New Folder(ao.FileNames(0).Substring(0, ao.FileNames(0).LastIndexOf("\") + 1))
                addFolder.addFolder(ao.FileNames())
                l2.SelectedIndex = -1
            Else
                Dim a As String = InputBox("Type in Folder to import", , My.Computer.FileSystem.SpecialDirectories.MyMusic & "\")
                If Not a = "" Then
                    If Directory.Exists(a) Then
                        Dim n As Integer = 0
                        For Each fil As String In My.Computer.FileSystem.GetFiles(a)
                            If dll.hasAudioExt(fil) Then
                                Track.getTrack(fil).addToPlaylist()
                                n += 1
                            End If
                        Next
                        If n = 0 Then
                            MsgBox("No Audio files found", MsgBoxStyle.Exclamation)
                        Else
                            If Not currTrack = Nothing Then
                                currTrack.play()
                            Else
                                playlist(0).play()
                            End If
                        End If

                    End If
                End If
            End If
        End If

    End Sub
    Private Sub LocalSourceToolStripMenuItem_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles MenuSource.DropDownOpening
        MenuSourceLocalRadio.Text = IIf(radioEnabled, "Local", "Radio")
    End Sub
    Private Sub RadioToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuSourceLocalRadio.Click
        switchSourceMode()
    End Sub
#End Region

#Region "Settings"
    Sub lockFormSwitch()
        setSetting(SettingsIdentifier.FORM_LOCKED, Not getSetting(SettingsIdentifier.FORM_LOCKED))
        FormUtils.colorForm(Me)
        FormUtils.setLockImage()
        If formLocked Then
            keyt.Stop()
            lockChange = True
        Else
            keyt.Start()
            keydelayt.Interval = delayMs
            keydelayt.Start()
        End If
    End Sub



    Private Sub menuIcons_MouseHover(sender As Object, e As EventArgs) Handles menuLock.MouseHover, menuRemote.MouseHover, menuSettings.MouseHover, menuGadgets.MouseHover
        ttShow(sender.ToolTipText, menuStrip, Cursor.Position.X + 10 - menuStrip.Left - Left, Cursor.Position.Y - 28 - menuStrip.Top - Top, 1000)
    End Sub


    Private Sub menuLock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuLock.Click
        If Not optionsMode Then lockFormSwitch()
    End Sub

    Public Sub setPlayRate()
        setPlayRate(playRate)
    End Sub

    Public Sub setPlayRate(val As Double)
        Try
            wmp.settings.rate = val
        Catch ex As Exception
            val = 1.0
        End Try
        '  playRate = val
        If optionsMode Then OptionsForm.labelPlayRate.Text = "Play Rate: " & val
        saveSetting(SettingsIdentifier.PLAY_RATE, val)
    End Sub

    Public Sub setBalance(val As Integer)
        Try
            wmp.settings.balance = val
        Catch ex As Exception
            val = 0
        End Try
        ' balance = val
        If optionsMode Then OptionsForm.labelBalance.Text = "Balance: " & val
        saveSetting(SettingsIdentifier.BALANCE, val)
    End Sub



    Private Sub menuSettings_Click(sender As Object, e As EventArgs) Handles menuSettings.Click
        showOptions(OptionsForm.optionState.NONE)
    End Sub
    Private Sub RemoteMenuitem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuRemote.Click
        showOptions(OptionsForm.optionState.REMOTE)
    End Sub

    Public Sub showOptions(ByVal state As OptionsForm.optionState, Optional ByVal asDialog As Boolean = False, Optional title As String = "", Optional args() As String = Nothing)
        If Not optionsMode Then
            If SettingsService.settingsInitialized AndAlso Not formLocked Then lockFormSwitch()
            OptionsForm.Text = title
                OptionsForm.state = state
                OptionsForm.arguments = args
                OptionsForm.TopMost = asDialog
                If asDialog Then
                    OptionsForm.ShowDialog()
                Else
                    OptionsForm.Show()
                End If
            End If
    End Sub

    Private Sub menuGadgets_Click(sender As Object, e As EventArgs) Handles menuGadgets.Click
        showGadgetForm(GadgetsForm.GadgetState.NONE)
    End Sub
    Public Sub showGadgetForm(ByVal state As GadgetsForm.GadgetState)
        GadgetsForm.state = state
        GadgetsForm.Show()
    End Sub

    Function diminishArray(ByRef str() As String, ByVal value As String) As String()
        If IsNothing(str) Then
            Return str
        Else
            Dim res() As String = Nothing
            For Each s As String In str
                If Not s = value Then
                    dll.ExtendArray(res, s)
                End If
            Next
            str = res
            Return str
        End If
    End Function
#End Region

#Region "Sort"
    Sub sortCheckedUpdate(checkedSender As ToolStripMenuItem)
        menuSortByName.Checked = False
        menuSortByDateAdded.Checked = False : menuSortByTimeListened.Checked = False : menuSortByCount.Checked = False
        menuSortByLength.Checked = False : menuSortByPopularity.Checked = False
        If checkedSender IsNot Nothing Then
            checkedSender.Checked = True
        Else
            Dim dr As ToolStripMenuItem = menuSortBy.DropDownItems(CInt(Int(trackSort / 2)))
            dr.Checked = True
            If trackSort Mod 2 = 1 Then menuSortByReverse.Checked = True
        End If
    End Sub

    Function toList(ByVal items As ListBox.ObjectCollection) As List(Of Track)
        Dim arr(items.Count - 1) As Track
        items.CopyTo(arr, 0)
        Return arr.ToList

    End Function

    Sub sortList(ByVal sortMode As sortMode, ByVal sender As Object)
        If radioEnabled Then sortCheckedUpdate(Nothing)
        If l2.Items.Count = 0 Or radioEnabled Then Exit Sub
        Dim tr As New List(Of Track)
        tr.AddRange(toList(l2.Items))

        tr = sortTracks(tr, sortMode)

        sortEnd(tr)
        sortCheckedUpdate(sender)
    End Sub

    Public Function sortTracks(ByVal tr As List(Of Track), sortMode As sortMode) As List(Of Track)
        Dim reverse As Boolean = False
        If dll.getBinaryComponents(sortMode).Contains(sortMode.REVERSE) Then
            sortMode -= 1
            reverse = True
        End If

        Select Case sortMode
            Case sortMode.NAME
                Return IIf(reverse, tr.OrderByDescending(Function(x) x.name).ToList, tr.OrderBy(Function(x) x.name).ToList)

            Case sortMode.DATE_ADDED
                For i = 0 To tr.Count - 1
                    tr(i).updateDate()
                Next
                Return IIf(reverse, tr.OrderBy(Function(x) x.added).ToList, tr.OrderByDescending(Function(x) x.added).ToList)

            Case sortMode.TIME_LISTENED
                For i = 0 To tr.Count - 1
                    tr(i).updateCount()
                    tr(i).updateLength()
                Next
                Return IIf(reverse, tr.OrderBy(Function(x) x.count * x.length).ToList, tr.OrderByDescending(Function(x) x.count * x.length).ToList)

            Case sortMode.COUNT
                For i = 0 To tr.Count - 1
                    tr(i).updateCount()
                Next
                Return IIf(reverse, tr.OrderBy(Function(x) x.count).ToList, tr.OrderByDescending(Function(x) x.count).ToList)

            Case sortMode.LENGTH
                For i = 0 To tr.Count - 1
                    tr(i).updateLength()
                Next
                Return IIf(reverse, tr.OrderBy(Function(x) x.length).ToList, tr.OrderByDescending(Function(x) x.length).ToList)

            Case sortMode.POPULARITY
                For i = 0 To tr.Count - 1
                    tr(i).updatePopularity(True)
                Next
                Return IIf(reverse, tr.OrderBy(Function(x) x.popularity).ToList, tr.OrderByDescending(Function(x) x.popularity).ToList)
        End Select
        Return Nothing
    End Function
    Public Sub sortListAuto()
        Cursor = Cursors.WaitCursor
        For i = 0 To 5
            Dim dr As ToolStripMenuItem = menuSortBy.DropDownItems(i)
            If trackSort = i * 2 Then
                dr.Checked = True
                sortList(trackSort, menuSortBy.DropDownItems(i))
            ElseIf trackSort = i * 2 + 1 Then
                dr.Checked = True
                menuSortByReverse.Checked = True
                sortList(trackSort, menuSortBy.DropDownItems(i))
            End If
        Next
        indicateSortMode()
        Cursor = Cursors.Default
    End Sub

    Private Sub AbcToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSortByName.Click
    End Sub
    Private Sub sortToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSortByName.Click, menuSortByDateAdded.Click, menuSortByTimeListened.Click, menuSortByCount.Click, menuSortByLength.Click, menuSortByPopularity.Click
        If searchState = SearchState.NONE Then
            If sender.Equals(menuSortByName) Then : setSetting(SettingsIdentifier.TRACK_SORT, sortMode.NAME + trackSort Mod 2)
            ElseIf sender.Equals(menuSortByDateAdded) Then : setSetting(SettingsIdentifier.TRACK_SORT, sortMode.DATE_ADDED + trackSort Mod 2)
            ElseIf sender.Equals(menuSortByTimeListened) Then : setSetting(SettingsIdentifier.TRACK_SORT, sortMode.TIME_LISTENED + trackSort Mod 2)
            ElseIf sender.Equals(menuSortByCount) Then : setSetting(SettingsIdentifier.TRACK_SORT, sortMode.COUNT + trackSort Mod 2)
            ElseIf sender.Equals(menuSortByLength) Then : setSetting(SettingsIdentifier.TRACK_SORT, sortMode.LENGTH + trackSort Mod 2)
            ElseIf sender.Equals(menuSortByPopularity) Then : setSetting(SettingsIdentifier.TRACK_SORT, sortMode.POPULARITY + trackSort Mod 2)
            End If
            sortListAuto()
        End If
    End Sub

    Private Sub ReverseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuSortByReverse.Click
        If l2.Items.Count > 0 And Not radioEnabled And searchState = SearchState.NONE Then
            sender.checked = Not sender.checked
            If sender.checked Then
                setSetting(SettingsIdentifier.TRACK_SORT, getSetting(SettingsIdentifier.TRACK_SORT) + 1)
            Else
                setSetting(SettingsIdentifier.TRACK_SORT, getSetting(SettingsIdentifier.TRACK_SORT) - 1)
            End If
            sortListAuto()
        End If
    End Sub

    Sub indicateSortMode()
        labelDateAdded2.Font = New Font(labelDateAdded2.Font, (trackSort = sortMode.DATE_ADDED Or trackSort = sortMode.DATE_ADDED + sortMode.REVERSE) * -4)
        labelTimeListened2.Font = New Font(labelTimeListened2.Font, (trackSort = sortMode.TIME_LISTENED Or trackSort = sortMode.TIME_LISTENED + sortMode.REVERSE) * -4)
        labelCount2.Font = New Font(labelCount2.Font, (trackSort = sortMode.COUNT Or trackSort = sortMode.COUNT + sortMode.REVERSE) * -4)
        labelLength2.Font = New Font(labelLength2.Font, (trackSort = sortMode.LENGTH Or trackSort = sortMode.LENGTH + sortMode.REVERSE) * -4)
        labelPopularity2.Font = New Font(labelPopularity2.Font, (trackSort = sortMode.POPULARITY Or trackSort = sortMode.POPULARITY + sortMode.REVERSE) * -4)
    End Sub


    Sub sortEnd(ByVal tr As List(Of Track))
        l2.BeginUpdate()
        l2.Sorted = False
        l2.Items.Clear()
        l2.Items.AddRange(tr.ToArray)
        l2.EndUpdate()
        SettingsService.saveSetting(SettingsIdentifier.TRACK_SORT, trackSort)
        Dim sel As Track = l2.SelectedItem
        If Not radioEnabled Then
            If l2_2.SelectedIndex = -1 Then
                If Not last = Nothing AndAlso listContains(l2, last) >= 0 Then
                    l2.SelectedIndex = listContains(l2, last)
                Else
                    l2.SelectedIndex = rnd.Next(0, l2.Items.Count)
                End If
            End If
        Else
            If listContains(l2, sel) >= 0 Then
                l2.SelectedIndex = listContains(l2, sel)
            Else : l2.SelectedIndex = rnd.Next(0, l2.Items.Count)
            End If
        End If
    End Sub



#End Region

#End Region

#Region "Lists + Drag/Drop"

    Sub setlistselected(Optional ByVal l As ListBox = Nothing)
        If l Is Nothing Then l = getSelectedList()
        If searchState = SearchState.NONE Then
            l.SelectedIndex = -1
            Dim prioTrack As Track = IIf(currTrack = Nothing, last, currTrack)
            If Not prioTrack = Nothing Then
                prioTrack.selectPlaylist()
            Else
                If l.Items.Count > 0 Then l.SelectedIndex = rnd.Next(0, l.Items.Count)
                getOtherList(l).SelectedIndex = -1
            End If
        End If
    End Sub

    Function getSelectedList() As ListBox
        If l2.SelectedIndex > -1 Then
            Return l2
        ElseIf l2_2.SelectedIndex > -1 Then
            Return l2_2
        Else
            Return l2_2
        End If
    End Function
    Function getSelectedTrack() As Track
        Dim l As ListBox = getSelectedList()
        If l IsNot Nothing Then
            If l.SelectedIndex > -1 Then
                If TypeOf l.SelectedItem Is Track Then
                    Return l.SelectedItem
                End If
            End If
        End If
        Return Nothing
    End Function
    Function getOtherList(ByVal l As ListBox) As ListBox
        If l.Equals(l2) Then
            Return l2_2
        Else
            Return l2
        End If
    End Function

    Function listContains(ByVal l As ListBox, ByVal track As Track) As Integer
        If l IsNot Nothing And track IsNot Nothing AndAlso l.Items.Count > 0 Then
            For i = 0 To l.Items.Count - 1
                If l.Items(i).name = track.name Then
                    Return i
                End If
            Next
        End If
        Return -1
    End Function

    Private Sub list_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles l2.MouseEnter, l2_2.MouseEnter, tv.MouseEnter
        If Not formLocked And searchState = SearchState.NONE And Not tSearch.Focused Then
            If Not searchState = SearchState.NONE Or Not sender.Equals(tv) And Me.Focused Then sender.Focus()
        End If
    End Sub

    Private Sub t1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        For Each k As Key In Key.keyList
            For Each keyCombi As Key.KeyCombi In k.keyCombiSet
                If e.KeyCode = keyCombi.key Then
                    e.SuppressKeyPress = True
                    Return
                End If
            Next
        Next
    End Sub

    Sub loadFont(control As Control, key As SettingsIdentifier, Optional defaultFont As Font = Nothing)
        Dim raw As String = SettingsService.loadSetting(key)
        setFont(control, raw, defaultFont)
    End Sub

    Sub setFont(control As Control, fontString As String, Optional defaultFont As Font = Nothing)
        Dim vals() As String = fontString.Split(";")
        Try
            Dim f As New Font(vals(0), CSng(vals(2)), DirectCast(CInt(vals(1)), FontStyle))
            control.Font = f
        Catch ex As Exception
            If defaultFont IsNot Nothing Then
                control.Font = defaultFont
            Else
                control.Font = New Font("Microsoft Sans Serif", 9.75, FontStyle.Regular)
            End If
        End Try
    End Sub


    Private Sub lists_mouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles l2.MouseWheel, l2_2.MouseWheel, tv.MouseWheel
        Dim senderControl As Control = CType(sender, Control)
        If Key.ctrlKey Then
            Dim iniKey As SettingsIdentifier = SettingsIdentifier.FONT_FOLDERS
            If sender.Equals(l2) Or sender.Equals(l2_2) Then iniKey = SettingsIdentifier.FONT_FOLDERS
            If senderControl.Font.Size >= 1 And senderControl.Font.Size <= 70 Then
                If e.Delta > 0 Then
                    OptionsForm.saveFont(senderControl, New Font(senderControl.Font.FontFamily.Name, senderControl.Font.Size + IIf(senderControl.Font.Size < 70, 1, 0), senderControl.Font.Style), iniKey)
                Else
                    OptionsForm.saveFont(senderControl, New Font(senderControl.Font.FontFamily.Name, senderControl.Font.Size + IIf(senderControl.Font.Size > 1, -1, 0), senderControl.Font.Style), iniKey)
                End If

                If senderControl Is l2 Then
                    l2_2.Font = l2.Font
                ElseIf senderControl Is l2_2 Then
                    l2.Font = l2_2.Font
                End If
            End If
            Dim x As Integer = senderControl.Width - 85
            Dim y As Integer = Cursor.Position.Y - senderControl.Top - Me.Top - 40
            ttShow(senderControl.Font.Size, senderControl, x, y, 1500)
        End If
    End Sub



    Private Sub lists_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles l2.KeyDown, l2_2.KeyDown, tv.KeyDown
        If Not e.KeyCode = Keys.Up And Not e.KeyCode = Keys.Down And Not e.KeyCode = Keys.Home And Not e.KeyCode = Keys.End Then
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub dd_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles l2.MouseDown, l2_2.MouseDown
        If Not radioEnabled Then
            Dim cursorPoint = New Point(Cursor.Position.X - sender.PointToScreen(New Point(sender.Left, sender.Top)).X + sender.Left, Cursor.Position.Y - sender.PointToScreen(New Point(sender.Left, sender.Top)).Y + sender.top)
            Dim it As Integer = sender.IndexFromPoint(cursorPoint)
            dItem = Nothing
            If Not it = -1 Then
                If TypeOf sender.items.item(it) Is Track Then
                    dItem = sender.Items.Item(it)

                    If e.Button = Windows.Forms.MouseButtons.Left Then
                        labelStatsUpdate(sender)
                        Cursor = Cursors.Hand
                        dragList = sender
                    End If
                End If

                If e.Button = MouseButtons.Right Or e.Button = MouseButtons.Left Then
                    sender.selectedindex = it
                    getOtherList(sender).SelectedIndex = -1
                End If

                dragDelayT.Start()
            End If
        End If
    End Sub

    Private Sub dragDelayT_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dragDelayT.Tick
        If dragList Is Nothing Then
            Cursor = Cursors.No
            Return
        End If
        Dim cursorPoint = New Point(Cursor.Position.X - dragList.PointToScreen(New Point(dragList.Left, dragList.Top)).X + dragList.Left, Cursor.Position.Y - dragList.PointToScreen(New Point(dragList.Left, dragList.Top)).Y + dragList.Top)
        Dim it As Integer = dragList.IndexFromPoint(cursorPoint)
        If Not it = -1 Then
            If dragList.Items.Item(it).Equals(dItem) Then
                dragDelayT.Stop()

                dragDropNextField = New Button With {.Size = New Drawing.Size(dragList.Width / 2 - 10, 40),
                    .Location = New Point(dragList.Left + 5, cursorPoint.Y + dragList.Top + 10),
                    .AllowDrop = True, .Font = New Font("Microsoft Sans Serif", 15), .FlatStyle = FlatStyle.Popup,
                    .Text = "Play Next"}

                dragDropQueueField = New Button With {.Size = New Drawing.Size(dragList.Width / 2 - 10, 40),
                    .Location = New Point(dragList.Left + dragList.Width / 2 + 5, cursorPoint.Y + dragList.Top + 10),
                    .AllowDrop = True, .Font = New Font("Microsoft Sans Serif", 15), .FlatStyle = FlatStyle.Popup,
                    .Text = IIf(dragList.Equals(l2), "Add to Queue", "Enqueue")}

                If cursorPoint.Y + 10 + dragDropNextField.Height > dragList.Height Then
                    dragDropNextField.Top -= 15 + dragDropNextField.Height
                    dragDropQueueField.Top -= 15 + dragDropQueueField.Height
                End If

                Controls.Add(dragDropNextField)
                Controls.Add(dragDropQueueField)

                dragDropNextField.BringToFront()
                dragDropQueueField.BringToFront()

                If last IsNot Nothing Then saveLastTrack()
                If dragList.DoDragDrop(dItem, DragDropEffects.Move Or DragDropEffects.Copy) = DragDropEffects.Move Then
                    If dragList Is l2 Then
                        dragList.Items.Remove(dItem)
                    End If
                End If
                dragList = Nothing
                dItem = Nothing
                Cursor = Cursors.Default
                Controls.Remove(dragDropNextField)
                Controls.Remove(dragDropQueueField)
            End If
        End If
    End Sub

    Private Sub dragFields_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles dragDropNextField.DragDrop, dragDropQueueField.DragDrop
        Dim dropItem As Track = e.Data.GetData(GetType(Track))
        If sender.Equals(dragDropNextField) Then
            dropItem.playNext()
        Else
            dropItem.addToPlaylist()
        End If
    End Sub

    Private Sub listDragDropHandler(drop As ListBox, dropItem As Track)
        If dragList Is l2_2 And Not dragList Is drop Then
            dItem.removeFromPlaylist()
        End If
        If drop.SelectedIndex = -1 Then
            If drop Is l2_2 Then
                Dim tr As Track = dropItem
                tr.addToPlaylist()
                tr.selectPlaylist()
            Else
                drop.Items.Add(dropItem)
                drop.SelectedIndex = drop.Items.Count - 1
            End If
        Else
            Dim dropTrack As New Track(Me, dropItem.fullPath, True, dropItem.virtualPath)
            If drop Is l2_2 Then
                dropTrack.addToPlaylist(drop.SelectedIndex)
                dropTrack.selectPlaylist()
            Else

                If Not drop.Items(drop.SelectedIndex).name = dropTrack.name Then
                    drop.Items.Insert(drop.SelectedIndex, dropTrack)
                    drop.SelectedIndex -= 1
                End If
            End If
        End If

        If dragList IsNot drop Then dragList.SelectedIndex = -1
    End Sub
    Private Sub list_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles l2.DragDrop, l2_2.DragDrop
        listDragDropHandler(sender, e.Data.GetData(GetType(Track)))
    End Sub
    Private Sub tv_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles tv.DragDrop
        Dim target As Folder = Folder.getFolder(root & tv.SelectedNode.FullPath & "\")
        If target.isVirtual Then
            If e.Effect = DragDropEffects.Move Then
                dItem.moveToVirtualFolder(target)
            ElseIf e.Effect = DragDropEffects.Copy Then
                dItem.copyToVirtualFolder(target)
            End If
        Else
            Dim tarPath As String = root & tv.SelectedNode.FullPath & "\" & dItem.name & dItem.ext
            If File.Exists(tarPath) Then
                If MsgBox("Overwrite?" & vbNewLine & vbNewLine & dItem.name & vbNewLine & "-> " & tv.SelectedNode.FullPath, MsgBoxStyle.YesNo + MsgBoxStyle.Information) = MsgBoxResult.Yes Then
                    If wmp.URL = dItem.fullPath Then wmp.URL = ""
                    File.Delete(tarPath)
                    If e.Effect = DragDropEffects.Move Then : IO.File.Move(dItem.fullPath, tarPath) : Else : IO.File.Copy(dItem.fullPath, tarPath) : End If
                End If
            Else
                If e.Effect = DragDropEffects.Move Then : IO.File.Move(dItem.fullPath, tarPath) : Else : IO.File.Copy(dItem.fullPath, tarPath) : End If
            End If
        End If
    End Sub

    Private Sub drag_Over(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles tv.DragOver, l2.DragOver, l2_2.DragOver, dragDropNextField.DragOver, dragDropQueueField.DragOver
        If e.Data.GetDataPresent(GetType(Track)) Then
            Dim cx = Cursor.Position.X - Me.Left
            Dim cy = Cursor.Position.Y - Me.Top - 23
            If TypeOf sender Is ListBox Then
                Dim other As ListBox = sender 'getOtherList(dragList)
                If New Rectangle(New Point(cx, cy), New Size(1, 1)).IntersectsWith(New Rectangle(other.Left, other.Top, other.Width, other.Height)) Then
                    Dim it2 As Integer = other.IndexFromPoint(New Point(Cursor.Position.X - other.PointToScreen(New Point(other.Left, other.Top)).X + other.Left, Cursor.Position.Y - other.PointToScreen(New Point(other.Left, other.Top)).Y + other.Top))
                    If it2 > -1 Then
                        other.SelectedIndex = it2
                    Else
                        other.SelectedIndex = -1
                    End If
                End If
            ElseIf TypeOf sender Is TreeView Then
                If New Rectangle(New Point(cx, cy), New Size(1, 1)).IntersectsWith(New Rectangle(tv.Left, tv.Top, tv.Width, tv.Height)) Then
                    Dim node As TreeNode = tv.GetNodeAt(New Point(Cursor.Position.X - tv.PointToScreen(New Point(tv.Left, tv.Top)).X + tv.Left, Cursor.Position.Y - tv.PointToScreen(New Point(tv.Left, tv.Top)).Y + tv.Top))
                    If Not node Is Nothing Then
                        tv.SelectedNode = node
                    End If
                End If
            ElseIf TypeOf sender Is Button Then
                sender.BackColor = ColorUtils.getInvLightColor(Me)
                sender.foreColor = ColorUtils.getDarkColor(Me)
            End If

            tt.Hide(Me)

            If e.KeyState = 1 Then
                e.Effect = DragDropEffects.Copy
            ElseIf e.KeyState = 5 Or e.KeyState = 9 Then 'shift and ctrl
                e.Effect = DragDropEffects.Move
            End If
        End If
    End Sub

    Private Sub list_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles l2.DragEnter, l2_2.DragEnter
        getOtherList(sender).SelectedIndex = -1
    End Sub
    Private Sub list_DragLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles l2.DragLeave, l2_2.DragLeave
        If Not sender.Equals(dragList) Then sender.selectedIndex = -1
    End Sub
    Private Sub button_DragLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles dragDropNextField.DragLeave, dragDropQueueField.DragLeave
        sender.BackColor = ColorUtils.getDarkColor(Me)
        sender.foreColor = ColorUtils.getInvDarkColor(Me)
    End Sub

    Private Sub dd_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles l2.MouseUp, l2_2.MouseUp
        dragDelayT.Stop()
        dragList = Nothing
        Cursor = Cursors.Default
    End Sub

#Region "tv"

    Private Sub tv_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tv.AfterSelect
        If dragList Is Nothing Then tv_AfterSelectSUB()
    End Sub
    Sub tv_AfterSelectSUB()
        If Not radioEnabled Then
            If Not IsNothing(tv.SelectedNode) Then
                Dim curr As Folder = Folder.getSelectedFolder(tv)
                If curr IsNot Nothing Then curr.invalidateFolderTracks(False, True)
                l2.Items.Clear()
                refill()
                If Not currTrack = Nothing Then
                    currTrack.selectPlaylist()
                Else
                    If l2_2.SelectedIndex = -1 Then setlistselected()
                End If
                cancelSearch()
            End If
        End If
    End Sub

    Private Sub tv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tv.Click
        Dim node As TreeNode = tv.GetNodeAt(New Point(Cursor.Position.X - sender.PointToScreen(New Point(sender.Left, sender.Top)).X + sender.Left,
                                     Cursor.Position.Y - sender.PointToScreen(New Point(sender.Left, sender.Top)).Y + sender.top))
        If Not IsNothing(node) Then
            tv.SelectedNode = node
        End If

    End Sub
#End Region

#Region "l2"

    Sub ttShow(ByVal text As String, ByVal window As Windows.Forms.IWin32Window, ByVal x As Integer, ByVal y As Integer, ByVal t As Integer)
        tt.Show(text, window, x, y, t)
    End Sub

    Private Sub l_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles l2.SelectedIndexChanged, l2_2.SelectedIndexChanged
        Dim l As ListBox = sender
        If TypeOf l.SelectedItem Is Track Or TypeOf l.SelectedItem Is Radio Then
            If Not l.SelectedIndex = -1 Then
                If Not radioEnabled Then
                    Dim x As Integer = l.Width - 85
                    Dim y As Integer = Cursor.Position.Y - l.Top - Me.Top - 40
                    If l.Focused Then
                        If menuSortByLength.Checked Then
                            l.SelectedItem.InvalidateLength()
                            ttShow(dll.SecondsTohmsString(l.SelectedItem.length), l, x, y, 1500)
                        ElseIf menuSortByPopularity.Checked Then
                            l.SelectedItem.updatePopularity(True)
                            Dim dt As String = l.SelectedItem.dateString
                            If dt = "" Then dt = Format(Now.Date, "dd.MM.yyyy")
                            Dim disttot As Integer = dll.GetDayDiff(Mid(dt, 1, 2), Mid(dt, 4, 2), Mid(dt, 7, 4), Now.Day, Now.Month, Now.Year)
                            If disttot > 0 Then
                                ttShow(dll.SecondsTohmsString(Int((l.SelectedItem.count * l.SelectedItem.length) / disttot)) & " Per Day", l, x, y, 1500)
                            End If
                        ElseIf menuSortByCount.Checked Then
                            l.SelectedItem.updatecount()
                            ttShow(l.SelectedItem.count, l, x, y, 1500)
                        Else
                            l.SelectedItem.updateDate()
                            ttShow(l.SelectedItem.dateString, l, x, y, 1500)
                        End If
                    End If
                    If searchState = SearchState.EMPTY Then cancelSearch(False)
                    If overlayModeContains(eOverlayMode.LYRICS) Then
                        LyricsForm.openLyrics(l.SelectedItem)
                    End If
                    If overlayModeContains(eOverlayMode.PARTS) Then
                        PartsForm.loadParts(l.SelectedItem)
                    End If
                End If
                labelStatsUpdate(l)
            End If
        Else
            'clear stats labels?
        End If
        FormUtils.setLyricsImage()
        If Not l2.SelectedIndex = -1 And sender.Equals(l2) And dragList Is Nothing Then l2_2.SelectedIndex = -1
    End Sub

    Private Sub l2_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles l2.DoubleClick
        If l2.SelectedIndex > -1 Then
            If Not radioEnabled Then
                If searchState > SearchState.NONE Then
                    If TypeOf l2.SelectedItem Is Track Then
                        Dim match As Track = l2.SelectedItem
                        match.addToPlaylist()
                        match.play()
                    ElseIf TypeOf l2.SelectedItem Is TrackPart Then
                        Dim part As TrackPart = l2.SelectedItem
                        part.track.addToPlaylist()
                        part.track.play()
                        trackLoop = LoopMode.YES
                        loopVals(1) = part.fromSec
                        loopVals(2) = part.toSec
                    End If
                Else
                    l2.SelectedItem.addToPlaylist()
                    l2.SelectedItem.play()
                End If
            Else
                saveRadioTime()
                wmpstart(l2.SelectedItem)
            End If
        End If
    End Sub

    Sub listRemove(ByVal l As ListBox, ByVal track As Track)
        For i = l.Items.Count - 1 To 0 Step -1
            If l.Items(i).ToString = track.ToString Then
                l.Items.RemoveAt(i)
            End If
        Next
    End Sub
#End Region

#Region "l2_2"


    Private Sub l2_2_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles l2_2.DoubleClick
        If Not l2.SelectedIndex = -1 Then
            last = l2.SelectedItem
            l2.SelectedIndex = -1
        End If
        If l2_2.SelectedItem IsNot Nothing Then l2_2.SelectedItem.play()
    End Sub
#End Region


#Region "overlayWindows t1;list1/2/3"

    Public Enum eOverlayMode
        NONE = 0
        LYRICS = 1
        PARTS = 2
        STATS_TRACKS = 4
        STATS_RADIO = 8
        STATS_FOLDERS = 16
    End Enum

    Class ListViewItemComparer
        Implements IComparer

        Private col As Integer = 0
        Private order As SortOrder
        Private list As ListView
        Private Shared dll As New Utils

        Public Sub New(ByVal col As Integer, ByVal list As ListView)
            MyClass.col = col
            MyClass.list = list
            MyClass.order = list.Sorting
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Dim res As Integer = 0
            If list.Equals(Form1.listTrackStats) Then
                Select Case col 'list1
                    Case 0, 7 : res = [String].Compare(CType(x, ListViewItem).SubItems(col).Text, CType(y, ListViewItem).SubItems(col).Text)
                    Case 1, 6 : res = CInt(CType(x, ListViewItem).SubItems(col).Text).CompareTo(CInt(CType(y, ListViewItem).SubItems(col).Text))
                    Case 2 : res = CInt(dll.dhmsStringToSeconds(CType(x, ListViewItem).SubItems(col).Text)).CompareTo(CInt(dll.dhmsStringToSeconds(CType(y, ListViewItem).SubItems(col).Text)))
                    Case 3, 4 : res = CInt(dll.hmsStringToSeconds(CType(x, ListViewItem).SubItems(col).Text)).CompareTo(CInt(dll.hmsStringToSeconds(CType(y, ListViewItem).SubItems(col).Text)))
                    Case 5
                        res = dateCompare(x, y, col)
                End Select
            Else
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

    Private Sub list1_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs)
        Dim l As ListView = IIf(sender.Equals(listTrackStats), listTrackStats, listFolderStats)
        l.BeginUpdate()
        l.Sorting = IIf(l.Sorting = SortOrder.Descending, SortOrder.Ascending, SortOrder.Descending)
        l.ListViewItemSorter = New ListViewItemComparer(e.Column, l)
        l.Sort()
        l.EndUpdate()
    End Sub

#End Region

#End Region

#Region "Refill"

    Public Sub refill(Optional sortAfter As Boolean = True)
        If Not IsNothing(tv.SelectedNode) Then
            Dim f As Folder = Folder.getSelectedFolder(tv)
            If f IsNot Nothing AndAlso f.tracks IsNot Nothing Then
                l2.Items.AddRange(f.tracks.ToArray)
                If sortAfter Then sortListAuto()
                If l2_2.SelectedIndex = -1 Then setlistselected()

                If Not l2.SelectedIndex = -1 And Not wmp.playState = WMPLib.WMPPlayState.wmppsPlaying And last = Nothing Then
                    last = l2.SelectedItem
                End If
            End If
        End If
    End Sub


    Private Sub insTv(ByVal currNode As TreeNode, ByVal currFolder As Folder)
        If currFolder.children IsNot Nothing Then
            For Each child As Folder In currFolder.children
                If Not child.isExcluded Then
                    insTv(currNode.Nodes.Add(child.nodePath, child.name), child)
                End If
            Next
        End If
    End Sub

    Public Sub localfill(Optional ByVal invalidate As Boolean = True, Optional selNodeString As String = Nothing)
        If invalidate Or Folder.folders Is Nothing Then
            Folder.invalidateFolders(Folder.top)
            Track.invalidateTracks(True)
        End If

        If selNodeString Is Nothing AndAlso tv.SelectedNode IsNot Nothing Then selNodeString = tv.SelectedNode.Name

        tv.BeginUpdate()

        tv.Nodes.Clear()
        l2.Items.Clear()

        insTv(tv.Nodes.Add(Folder.top.name, Folder.top.name), Folder.top)
        tv.Nodes(Folder.top.name).Expand()
        tv.Sort()

        tv.EndUpdate()

        last = Track.getTrack(SettingsService.loadSetting(SettingsIdentifier.LAST_TRACK_FILE))

        If tv.Nodes(0).Nodes.Count > 0 Then
            Dim prioTrack As Track = IIf(currTrack = Nothing, last, currTrack)
            If Not prioTrack = Nothing And selNodeString Is Nothing Then
                Dim conNode() As TreeNode = tv.Nodes.Find(prioTrack.dir, True)
                If conNode.Length > 0 Then
                    Try
                        tv.SelectedNode = conNode(0)
                    Catch ex As Exception
                        tv.SelectedNode = tv.Nodes(Folder.top.name)
                    End Try
                Else
                    tv.SelectedNode = tv.TopNode
                End If
            Else
                If selNodeString IsNot Nothing AndAlso tv.Nodes.Find(selNodeString, True).Length > 0 Then
                    tv.SelectedNode = tv.Nodes.Find(selNodeString, True)(0)
                Else
                    tv.SelectedNode = tv.TopNode
                End If

            End If
        Else
            tv.SelectedNode = tv.TopNode
        End If
        If Not IsNothing(tv.SelectedNode) Then tv.SelectedNode.EnsureVisible()

        If Not radioEnabled Then
            If l2.Items.Count > 0 Then
                Dim prioTrack As Track = IIf(currTrack = Nothing, last, currTrack)
                If Not prioTrack = Nothing Then
                    prioTrack.selectPlaylist()
                End If
            End If

            'decheck(True)
        Else
            radfill()
        End If

    End Sub

    Public Sub radfill()
        Dim beforeIndex As Integer = l2.SelectedIndex
        l2.Items.Clear()
        l2.Sorted = False
        Dim rads As List(Of Radio) = MediaPlayer.Radio.getStations()
        If radioSort = 1 Then rads.Sort(Function(x, y) y.time.CompareTo(x.time))
        l2.Items.AddRange(rads.ToArray)
        Dim wasRadioBefore As Boolean = radioEnabled And beforeIndex >= 0
        setSetting(SettingsIdentifier.MUSIC_SOURCE, MusicSource.RADIO)
        If rads.Count > 0 Then l2.SelectedIndex = IIf(wasRadioBefore, beforeIndex, 0)
    End Sub

#End Region

#Region "Context Menu"

#Region "con1"

    Private Sub con1_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles con1.Opening
        Dim fol As Folder = Folder.getSelectedFolder(tv)
        ManageToolStripMenuItem.Text = "Manage " & IIf(fol.isVirtual, "Playlist", "Folder")
    End Sub


    Private Sub RefreshTv_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles con1RefreshList.Click
        tv_refill()
    End Sub

    Sub tv_refill(Optional selNodeString As String = Nothing)
        If Not radioEnabled Then
            If Not l2.Items.Count = 0 And l2_2.Items.Count = 0 Then saveLastTrack(CType(getSelectedList.SelectedItem, Track).virtualPath)
            localfill(True, selNodeString)
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles con1NewPlaylist.Click
        If Not IsNothing(tv.SelectedNode) Then
            Dim parent As Folder = Folder.getFolder(Folder.root.fullPath & tv.SelectedNode.FullPath)
            Dim child As Folder = parent.createSubPlaylist()
            If child IsNot Nothing Then
                Folder.folders.Add(child)
                parent.children.Add(child)
                TrackSelectionForm.selectTracks(child, TrackSelectionForm.eTrackSelectionMode.MANAGE, "Add Tracks to playlist...")
                tv_refill(child.nodePath)
            End If
        End If
    End Sub

    Private Sub AddToQueueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles con1QueueAllTracks.Click
        If Not IsNothing(tv.SelectedNode) Then
            For Each t As Track In sortTracks(Folder.getSelectedFolder(tv).tracks, trackSort)
                If currTrack Is Nothing OrElse Not t.name.ToLower = currTrack.name.ToLower Then t.addToPlaylist()
            Next
        End If
    End Sub

    Private Sub ManageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ManageToolStripMenuItem.Click
        Dim fol As Folder = Folder.getSelectedFolder(tv)
        showOptions(OptionsForm.optionState.PLAYLISTS, False, "Manage " & fol.name, IIf(fol.isVirtual, {fol.nodePath}, {"folders", fol.nodePath}))
    End Sub

#End Region

#Region "con2"

#Region "Stats"


    Sub extendArray(ByRef folders() As Folder, Optional ByVal value As Folder = Nothing)
        If IsNothing(folders) Then
            ReDim folders(0)
            folders(0) = value
        Else
            ReDim Preserve folders(folders.Length)
            folders(folders.Length - 1) = value
        End If
    End Sub

    Sub extendArray(ByRef nodes() As TreeNode, Optional ByVal value As TreeNode = Nothing)
        If IsNothing(nodes) Then
            ReDim nodes(0)
            nodes(0) = value
        Else
            ReDim Preserve nodes(nodes.Length)
            nodes(nodes.Length - 1) = value
        End If
    End Sub

    Private Sub GenreDistributionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles con2ListTasksGenreDistributionToolStripMenuItem.Click
        Dim l As ListBox = getSelectedList()
        If l.Items.Count > 0 And Not radioEnabled And searchState = SearchState.NONE Then
            Dim genreRaw As String = SettingsService.getSetting(SettingsIdentifier.GENRES)
            Dim gsUpper() As String = genreRaw.Split(";")
            Dim gs() As String = genreRaw.ToLower.Split(";")
            l.SelectedItem.updateGenre()
            Dim g As Genre = l.SelectedItem.genre
            Dim str As String = "Track genre: " & vbNewLine & g.name & vbNewLine
            If Not gs(0) = "" Then
                Dim n(gs.Length - 1) As Integer
                For i = 0 To l.Items.Count - 1
                    l.Items(i).updateGenre()
                    Me.Text = (i + 1) & " / " & l.Items.Count & " - " & Int(((i + 1) / l.Items.Count) * 100) & "%"
                    Dim ind As Integer = Array.IndexOf(gs, l.Items.Item(i).genre.name.ToLower)
                    If ind > -1 Then n(Array.IndexOf(gs, l.Items.Item(i).genre.name.ToLower)) += 1
                Next
                Me.Text = ""
                str &= vbNewLine & "List genre distribution:" & vbNewLine & vbNewLine
                For k = 0 To gs.Length - 1
                    str &= gsUpper(k) & ": " & n(k) & "  -  " & Int((n(k) / l.Items.Count) * 100) & "%" & vbNewLine
                Next
                MsgBox(str)
            End If
        End If
    End Sub

#End Region



#Region "Item Tasks"
    Function removeItem(hotkeyTrigger As Boolean) As Boolean
        If Not radioEnabled Then
            Dim l As ListBox = getSelectedList()
            If l IsNot Nothing Then
                If Not hotkeyTrigger Or l.Focused Then
                    Dim selTrack As Track = l.SelectedItem
                    If selTrack IsNot Nothing Then
                        If l Is l2 Then
                            If l.Items.Count > 1 Then
                                If l.SelectedIndex < l.Items.Count - 1 Then
                                    l.SelectedIndex += 1
                                    l.Items.RemoveAt(l.SelectedIndex - 1)
                                Else
                                    l.SelectedIndex = l.Items.Count - 2
                                    l.Items.RemoveAt(l.Items.Count - 1)
                                End If
                            Else
                                l.Items.RemoveAt(0)
                            End If
                        Else
                            Dim delIndex As Integer = selTrack.removeFromPlaylist()
                            If delIndex < l.Items.Count + 1 - 1 Then
                                l.SelectedIndex = delIndex
                            Else
                                l.SelectedIndex = l.Items.Count + 1 - 2
                            End If
                            If currTrack IsNot Nothing AndAlso selTrack.name.ToLower = currTrack.name.ToLower Then
                                wmp.URL = ""
                                wmp.Ctlcontrols.pause()
                            End If

                        End If
                        Return True
                    End If
                End If
            End If
        End If
        Return False
    End Function

    Function deleteTrack(track As Track, prompt As Boolean) As Boolean
        Dim l As ListBox = getSelectedList()
        If l.Focused Then
            If track.isVirtual Then
                If Not prompt OrElse MsgBox("Remove track from playlist " & Folder.getFolder(track.path).fullPath & "?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    track.virtualDelete()
                    Return True
                End If
            Else
                Try
                    If Not prompt OrElse MsgBox("Are you sure to delete the file permanently?", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation) = MsgBoxResult.Yes Then
                        IO.File.Delete(track.fullPath)
                        Return True
                    End If
                Catch ex As Exception
                    MsgBox("File is read-only. Please delete manually", MsgBoxStyle.Critical)
                End Try
            End If
        End If
        Return False
    End Function

    Private Sub RemoveFromPlaylistToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveFromPlaylistToolStripMenuItem.Click
        Dim l As ListBox = getSelectedList()
        If Not radioEnabled AndAlso l IsNot Nothing AndAlso l.SelectedIndex > -1 AndAlso TypeOf l.SelectedItem Is Track Then
            Dim track As Track = l.SelectedItem
            Dim fol As Folder = Folder.getFolder(track.path)
            If Not fol = Nothing Then
                If MsgBox("Selected Track:" & vbNewLine & track.name & vbNewLine & vbNewLine & "Remove from following playlist?'" & vbNewLine & fol.fullPath, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    track.virtualDelete()
                    If l Is l2 Then removeItem(False)
                End If
            End If
        End If
    End Sub


    Private Sub con2TrackTasksEditTrackParts_Click(sender As Object, e As EventArgs) Handles con2TrackTasksEditTrackParts.Click
        Dim l As ListBox = getSelectedList()
        If Not radioEnabled AndAlso l IsNot Nothing AndAlso l.SelectedIndex > -1 AndAlso TypeOf l.SelectedItem Is Track Then
            Dim track As Track = l.SelectedItem
            openOverlay(eOverlayMode.PARTS)
            PartsForm.loadParts(track)
        End If
    End Sub

    Private Sub CopyStringToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles con2TrackTasksCopyName.Click
        If Not radioEnabled Then
            Dim l As ListBox = getSelectedList()
            If l.SelectedItem IsNot Nothing Then
                If TypeOf l.SelectedItem Is Track Or TypeOf l.SelectedItem Is TrackPart Then
                    My.Computer.Clipboard.SetText(l.SelectedItem.name)
                End If
            End If
        End If
    End Sub


    Private Sub LocationsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles con2TrackTasksShowLocations.Click
        Dim l As ListBox = getSelectedList()
        Dim selTrack As Track = Nothing
        If TypeOf l.SelectedItem Is TrackPart Then
            selTrack = l.SelectedItem.track
        ElseIf TypeOf l.SelectedItem Is Track Then
            selTrack = l.SelectedItem
        Else
            Return
        End If
        If Not IsNothing(l) And Not radioEnabled Then
            Folder.invalidateFolders(Folder.top)
            Track.invalidateTracks(True)

            Dim locations As List(Of Folder) = selTrack.getLocations(True)
            Dim str As String = "Track source:" & vbNewLine & selTrack.fullPath & vbNewLine & vbNewLine
            If locations.Count > 0 Then
                For Each f As Folder In locations
                    str &= f.nodePath & vbNewLine
                Next
            Else
                str &= "No Location found"
            End If
            MsgBox(str)
        End If
    End Sub


#End Region

#Region "List Tasks"

    Private Sub RefillToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles con2ListTasksRefill.Click
        If l2.SelectedIndex > -1 And getSelectedList() Is l2 Then
            If Not radioEnabled Then
                If searchState = SearchState.NONE Then
                    l2.Items.Clear()
                    refill()
                End If
            Else : radfill() : End If
        End If
    End Sub

    Private Sub ClearToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles con2ListTasksClear.Click
        Dim l As ListBox = getSelectedList()
        Dim ol As ListBox = getOtherList(l)
        If Not radioEnabled Then
            If l Is l2 Then
                If searchState > SearchState.NONE Then cancelSearch(False)

                l.Items.Clear()
                If Not last = Nothing Then
                    If listContains(ol, last) >= 0 Then
                        ol.SelectedIndex = listContains(ol, last)
                    Else
                        If ol.Items.Count > 0 Then
                            ol.SelectedIndex = rnd.Next(0, ol.Items.Count)
                        End If
                    End If
                End If


            Else
                clearPlaylist()
            End If
        End If
    End Sub

    Private Sub QueueInOrderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles con2ListTasksQueueInOrder.Click
        If radioEnabled Or getSelectedList() Is l2_2 Then Exit Sub
        For i = 0 To l2.Items.Count - 1
            If Not TypeOf l2.Items(i) Is Track Then Continue For
            Dim track As Track = l2.Items(i)
            track.addToPlaylist()
        Next
        setlistselected()
    End Sub

    Private Sub con2TracksTasksRemove_Click(sender As Object, e As EventArgs) Handles con2TracksTasksRemove.Click
        removeItem(False)
    End Sub

#End Region

#Region "File Tasks"

    Private Sub DeleteFILEToolStripMenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles con2SourceTasksDelete.Click
        Dim l As ListBox = getSelectedList()
        If Not radioEnabled And l IsNot Nothing And Not radioEnabled Then
            Dim track As Track = CType(l.SelectedItem, Track)
            If track IsNot Nothing Then
                If track.isVirtual Then
                    track.virtualDelete()
                Else
                    If IO.File.Exists(track.fullPath) Then
                        If wmp.URL = track.fullPath Then
                            wmp.URL = ""
                        End If
                        Try
                            If MsgBox("Are you sure to delete the file permanently?", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation) = MsgBoxResult.Yes Then
                                IO.File.Delete(track.fullPath)
                                removeItem(False)
                            End If
                        Catch ex As Exception
                            MsgBox("File is read-only")
                        End Try
                    End If
                End If

            End If
        End If
    End Sub
    Private Sub RenameToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles con2SourceTasksRename.Click
        Dim l As ListBox = getSelectedList()
        If Not radioEnabled And l IsNot Nothing Then
            Dim str As Track = l.SelectedItem
            Folder.invalidateFolders(Folder.top)
            Track.invalidateTracks(True)
            Dim locs As List(Of Folder) = str.getLocations(True)
            locs.Insert(0, Folder.getFolder(str.fullPath.Substring(0, str.fullPath.LastIndexOf("\")), True))
            If locs.Count > 0 Then
                Dim newName As String = InputBox("Type in new Name", , str.name)
                If Not newName = "" And Not newName = str.name Then

                    For i = 0 To locs.Count - 1
                        Dim locPath As String = locs(i).fullPath 'root & locs(i).nodePath & "\"
                        If locs(i).isVirtual Then
                            IniService.iniDeleteKey(locs(i).fullPath, str.name, playlistPath)
                            IniService.iniWriteValue(locs(i).fullPath, newName, str.fullPath.Substring(0, str.fullPath.LastIndexOf("\") + 1) & newName & str.ext, playlistPath)
                        Else
1:                          If Not File.Exists(locPath & newName & str.ext) Then
                                Try
                                    IO.File.Move(locPath & str.name & str.ext, locPath & newName & str.ext)
                                Catch ex As Exception
                                    MsgBox("renaming failed in " & locs(i).nodePath)
                                End Try
                            Else
                                If MsgBox(locPath & newName & str.ext & " already exists. Overwrite that file?", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation) = MsgBoxResult.Yes Then
                                    Try
                                        IO.File.Delete(locPath & newName & str.ext)
                                        IO.File.Move(locPath & str.name & str.ext, locPath & newName & str.ext)
                                    Catch ex As Exception
                                        If MsgBox("Failed to overwrite. Try again?", MsgBoxStyle.YesNo + MsgBoxStyle.Critical) = MsgBoxResult.Yes Then
                                            GoTo 1
                                        End If
                                    End Try
                                End If

                            End If
                            If wmp.URL.ToLower = CStr(locPath & str.name & str.ext).ToLower And (wmp.playState = WMPLib.WMPPlayState.wmppsPlaying Or wmp.playState = WMPLib.WMPPlayState.wmppsPaused) Then
                                wmpstartURL(root & locs(i).nodePath & newName & str.ext)
                            End If
                        End If
                    Next
                    If IniService.iniIsValidKey(IniSection.TRACKS_TIME, str.name) Then
                        saveRawSetting(SettingsIdentifier.TRACKS_TIME, newName, loadRawSetting(SettingsIdentifier.TRACKS_TIME, str.name))
                        IniService.iniDeleteKey(IniSection.TRACKS_TIME, str.name)
                    End If
                    If IniService.iniIsValidKey(IniSection.TRACKS, str.name) Then
                        saveRawSetting(SettingsIdentifier.TRACKS_COUNT, newName, loadRawSetting(SettingsIdentifier.TRACKS_COUNT, str.name))
                        IniService.iniDeleteKey(IniSection.TRACKS, str.name)
                    End If


                    If File.Exists(lyrpath & str.name & ".ini") Then
                        File.Move(lyrpath & str.name & ".ini", lyrpath & newName & ".ini")
                    End If
                    If File.Exists(lyrpath & str.name & ".txt") Then
                        File.Move(lyrpath & str.name & ".txt", lyrpath & newName & ".txt")
                    End If

                    Try
                        Dim args As String = "rep Musik """ & str.name & """  """ & newName & """"
                        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\Visual Studio 2010\Projects\myeventlog\myeventlog\bin\debug\myeventlog.exe", args)
                    Catch ex As Exception
                        MsgBox("Please manually change the date you aquired the track.", MsgBoxStyle.Information)
                    End Try



                    localfill()
                End If
            End If
        End If
    End Sub


    Private Sub ReplaceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles con2SourceTasksReplace.Click
        Dim l As ListBox = getSelectedList()
        If Not radioEnabled And l IsNot Nothing Then
            Dim tr As Track = l.SelectedItem
            Dim locs As List(Of Folder) = tr.getLocations(True)
            If locs.Count > 0 Then
                Dim str As String = ""
                If Not IsNothing(locs) Then
                    For Each f As Folder In locs
                        If Not f.nodePath = tr.dir Then str &= f.nodePath & vbNewLine
                    Next
                    If MsgBox("Replace tracks in following locations with" & vbNewLine & tr.dir & vbNewLine & vbNewLine & str, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        For i = 0 To locs.Count - 1
                            If Not locs(i).nodePath = tr.dir Then
                                IO.File.Copy(tr.fullPath, root & locs(i).nodePath & "\" & tr.name & tr.ext, True)
                            End If
                        Next

                    End If
                Else
                    MsgBox("No Location found", MsgBoxStyle.Information)
                End If

                localfill()
            End If
        End If
    End Sub

#End Region

    Private Sub TrackToQueue() 'handles toqueue command
        If Not radioEnabled Then
            Dim l As ListBox = getSelectedList()
            If Not TypeOf l.SelectedItem Is Track Then Return
            Dim selTrack As Track = l.SelectedItem
            If Not currTrack = Nothing Then
                currTrack.selectPlaylist()
            Else
                playlist(0).selectPlaylist()
            End If
            selTrack.addToPlaylist()
            If l Is l2 Then
                If removeNextTrack Then
                    l2.Items.Remove(selTrack)
                    l2.SelectedIndex = -1
                End If
            End If
            keydelay()
        End If
    End Sub

    Private Sub TrackAddToQueueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles con2AddToQueue.Click
        TrackToQueue()
        setlistselected()
    End Sub


    Private Sub con2_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles con2.Opening
        con2AddToPlaylist.DropDownItems.Clear()
        con2AddToPlaylist.DropDownItems.Add("New Playlist...")
        con2AddToPlaylist.DropDownItems.Add("Existing Playlist...")

        Dim l As ListBox = con2.SourceControl

        Dim Enabled = l Is l2

        Dim itemFromPlaylist As Boolean = False
        If l.SelectedIndex > -1 Then
            Dim item = l.SelectedItem
            If TypeOf item Is Track Then
                If Not DirectCast(item, Track).virtualPath = DirectCast(item, Track).fullPath Then
                    itemFromPlaylist = True
                End If
            End If
        End If

        con2ListTasksQueueInOrder.Enabled = Enabled
        con2ListTasksRefill.Enabled = Enabled
        RemoveFromPlaylistToolStripMenuItem.Enabled = itemFromPlaylist

    End Sub


    Sub con2AddToPlaylist_Clicked(ByVal sender As System.Object, ByVal e As ToolStripItemClickedEventArgs) Handles con2AddToPlaylist.DropDownItemClicked
        con2.Hide()
        If TypeOf getSelectedList().SelectedItem IsNot Track Then Return
        If e.ClickedItem.Text = "New Playlist..." Then
            Dim selNode As Folder = NodeSelectionForm.selectNode("Select playlist parent node")
            If selNode IsNot Nothing Then
1:              Dim a As String = InputBox("Type in playlist name")
                If Not a = "" Then
                    Dim exists As Integer = Folder.directoryOrVirtualExists(selNode.fullPath, a)
                    If exists = 0 Then
                        Dim tr As Track = getSelectedList().SelectedItem
                        IniService.iniWriteValue(selNode.fullPath & a & "\", tr.name, tr.fullPath, playlistPath)
                        tv_refill(selNode.fullPath & a)
                    Else
                        MsgBox(IIf(exists = 1, "Folder ", "Playlist ") & "with that name already exists." & vbNewLine & "Please choose another name.", MsgBoxStyle.Information)
                        GoTo 1
                    End If
                End If
            End If
        Else
            Dim folder As Folder = NodeSelectionForm.selectNode("Add Track to...", {"folder"})
            If folder IsNot Nothing Then
                CType(getSelectedList().SelectedItem, Track).copyToVirtualFolder(folder)
            End If

        End If

    End Sub

#End Region

#End Region

#Region "Labels"

    Sub updateUI()
        windowTextUpdate()

        labelPartsLoopUpdate()

        labelUIUpdate()

        searchStateUIUpdate()

        If currTrack = Nothing AndAlso l2.SelectedIndex = -1 AndAlso l2_2.SelectedIndex = -1 And dragList Is Nothing Then
            If Track.playlist IsNot Nothing Then
                If Track.playlist.Count > 0 Then
                    Track.playlist(0).selectPlaylist()
                End If
            End If
        End If
    End Sub

    Sub labelUIUpdate()
        labelVolume.Text = wmp.settings.volume
        labelL2Count.Text = l2.Items.Count
        labelL2Count.Location = New Point(l2.Right - 19 - labelL2Count.Width, l2.Bottom - labelL2Count.Height - 2)
        labelL2_2Count.Location = New Point(l2_2.Right - 19 - labelL2_2Count.Width, l2_2.Bottom - labelL2_2Count.Height - 2)
        labelL2_2Count.Text = l2_2.Items.Count
    End Sub

    Sub searchStateUIUpdate()
        If searchState = SearchState.NONE Then
            tSearch.Text = "Search..."
            If l2.SelectedIndex = -1 And l2_2.SelectedIndex = -1 And dragList Is Nothing Then
                labelCount.Text = ""
                labelTimeListened.Text = ""
                labelPartsCount.Text = ""
                labelDateAdded.Text = ""
                labelLength.Text = ""
                labelPopularity.Text = ""
                labelGenre.Text = ""
                If Not radioEnabled And Not wmp.URL = "" Then setlistselected()
            End If
        End If
    End Sub


    Sub windowTextUpdate()
        If Not radioEnabled And Not currTrack Is Nothing Then
            Dim t As String = currTrack.name
            If currTrack.partsCount > 1 Then
                If currTrackPart IsNot Nothing Then
                    Dim pString As String = currTrackPart.name
                    t &= " | " & IIf(pString = "", "", pString & " | ") & currTrackPart.id + 1 & "(" & currTrack.partsCount & ")"
                End If
            End If
            If wmp.playState = WMPPlayState.wmppsPlaying Then
                t = "♫ " & t '23.02.18 →
            ElseIf wmp.playState = WMPPlayState.wmppsPaused Or wmp.playState = WMPPlayState.wmppsStopped Then
                t = "■ ⁯⁯⁯⁯⁯⁯" & t
            End If
            Me.Text = t
        ElseIf radioEnabled Then
            If l2.SelectedItem IsNot Nothing Then Me.Text = IIf(wmp.playState = WMPPlayState.wmppsPlaying, "♫ ", "■ ") & l2.SelectedItem.name
        ElseIf wmp.URL = "" Then
            Text = ""
        End If
    End Sub

    Sub labelStatsUpdate(Optional ByVal l As ListBox = Nothing)
        If radioEnabled Then
            If l Is Nothing Then
                l = getSelectedList()
            End If
            If l.SelectedItem IsNot Nothing Then
                l.SelectedItem.update()
                labelTimeListened.Text = dll.SecondsTodhmsString(l.SelectedItem.time)
            Else
                labelTimeListened.Text = ""
            End If
            labelDateAdded.Text = "" : labelPopularity.Text = "" : labelGenre.Text = "" : labelLength.Text = "" : labelCount.Text = "" : labelPartsCount.Text = ""
        Else
            If Folder.folders IsNot Nothing Then
                If l Is Nothing Then
                    l = getSelectedList()
                End If
                If l IsNot Nothing AndAlso l.SelectedItem IsNot Nothing AndAlso TypeOf l.SelectedItem Is Track Then
                    Try
                        l.SelectedItem.invalidateStats()
                    Catch ex As Exception
                        MsgBox("invalidate stats failed")
                    End Try

                    Dim tr As Track = l.SelectedItem
                    labelCount.Text = tr.count

                    Try
                        labelTimeListened.Text = dll.SecondsTodhmsString(l.SelectedItem.count * l.SelectedItem.length)

                    Catch ex As Exception
                        ' MsgBox("labelTimeListened failed")
                    End Try
                    Dim dt As String = ""
                    Try
                        dt = l.SelectedItem.dateString()
                        labelDateAdded.Text = dt
                    Catch ex As Exception
                        ' MsgBox("date string failed")
                    End Try
                    Try
                        labelLength.Text = dll.SecondsTohmsString(l.SelectedItem.length)
                    Catch ex As Exception
                        ' MsgBox("labelLength failed")
                    End Try

                    Try
                        labelGenre.Text = l.SelectedItem.genre.ToString
                    Catch ex As Exception
                        '  MsgBox("labelGenre genres failed")
                    End Try

                    Dim tim As Integer
                    Try
                        tim = l.SelectedItem.count * l.SelectedItem.length
                    Catch ex As Exception
                        ' MsgBox("count * length  failed")
                    End Try

                    Try
                        labelPartsLoopUpdate()
                    Catch ex As Exception
                        '  MsgBox("labelPartsLoopUpdate failed")
                    End Try

                    If dt = "" Then
                        dt = dateLogStart
                    End If

                    Try
                        Dim diff As Integer = Now.Subtract(CDate(dt)).TotalDays
                        If diff > 0 Then tim /= diff
                    Catch ex As Exception
                        MsgBox("diff failed")
                    End Try

                    Dim measurePoint As String = "      "
                    Try
                        If CDate(dt).CompareTo(IIf(dateLogStart.ToShortDateString() = "06.04.2011", CDate("11.09.2012"), dateLogStart)) < 0 Then
                            measurePoint = "  >  "
                        End If
                    Catch ex As Exception
                        MsgBox("measure failed")
                    End Try

                    Try
                        labelPopularity.Text = measurePoint & dll.SecondsTohmsString(tim)
                    Catch ex As Exception
                        MsgBox("labelPopularity failed")
                    End Try

                End If
            End If
        End If

    End Sub

    Sub labelPartsLoopUpdate()
        Dim selList As ListBox = getSelectedList()
        If selList IsNot Nothing And Not selList.SelectedItem Is Nothing And Not radioEnabled And selList.Items.Count > 0 And searchState = SearchState.NONE And dragList Is Nothing Then
            If Not currTrack = Nothing AndAlso currTrack.name = selList.SelectedItem.name Then
                If currTrack.partsCount = 0 Then
                    currTrack.updateParts()
                End If
1:              If currTrack.partsCount > 0 AndAlso currTrackPart IsNot Nothing Then labelPartsCount.Text = currTrackPart.id + 1 & " (" & currTrack.partsCount & ")"
                Try
                    If currTrack.partsCount > 0 Then labelPartName.Text = currTrackPart.name
                    If trackLoop = LoopMode.NO Then
                        If currTrack.partsCount > 0 Then
                            labelLoop.Text = currTrackPart.format
                        Else
                            labelLoop.Text = ""
                        End If
                    ElseIf trackLoop = LoopMode.INTERMEDIATE Then
                        labelLoop.Text = "[" & dll.secondsTo_ms_Format(Int(loopVals(1))) & " -"
                    Else
                        labelLoop.Text = "[" & dll.secondsTo_ms_Format(Int(loopVals(1))) & " - " & dll.secondsTo_ms_Format(Int(loopVals(2))) & "]"
                    End If
                Catch ex As Exception
                    labelLoop.Text = "error"
                End Try
            ElseIf selList.SelectedItem.partsCount > 0 Then
                If currTrack <> Nothing And selList.SelectedItem IsNot Nothing Then
                    If currTrack.name = selList.SelectedItem.name Then
                        GoTo 1
                    End If
                End If
                labelPartsCount.Text = selList.SelectedItem.partsCount
                labelPartName.Text = ""
                labelLoop.Text = ""
            Else
                labelPartsCount.Text = selList.SelectedItem.partsCount
                labelLoop.Text = ""
                labelPartName.Text = ""
            End If
        Else
            labelLoop.Text = ""
            labelPartName.Text = ""
        End If
    End Sub

    Private Sub setDate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles labelDateAdded.Click, labelDateAdded2.Click, con2SourceTasksSetDate.Click
        If Not radioEnabled And searchState = SearchState.NONE Then
            Dim l As ListBox = getSelectedList()
            If l IsNot Nothing AndAlso l.SelectedItem IsNot Nothing Then
                l.SelectedItem.setDate()
            End If
        End If
    End Sub

    Private Sub Label22_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles labelGenre2.Click, labelGenre.Click
        showOptions(OptionsForm.optionState.GENRES,,, {labelGenre.Text})
    End Sub

    Private Sub labelPopularity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles labelPopularity2.Click, labelPopularity.Click
        If Not radioEnabled And searchState = SearchState.NONE Then
            Try
                Dim loggedVal As String = SettingsService.loadSetting(SettingsIdentifier.DATE_LOG_START)
                Dim logStart As Date = CDate(loggedVal)
                Dim dt As Date = InputBox("Default start date for popularity calculation." _
                                            & vbNewLine & "(If no date of a track's aquirement is given)", , logStart.ToShortDateString)
                saveSetting(SettingsIdentifier.DATE_LOG_START, dt)
            Catch ex As Exception
                Return
            End Try
        End If
    End Sub


    Private Sub Label_random(ByVal sender As System.Object, ByVal e As System.EventArgs)
        saveSetting(SettingsIdentifier.PLAY_MODE, PlayMode.RANDOM)
    End Sub
    Private Sub Label_repeat(ByVal sender As System.Object, ByVal e As System.EventArgs)
        saveSetting(SettingsIdentifier.PLAY_MODE, PlayMode.REPEAT)
    End Sub
    Private Sub Label_lastfile()
        Dim flag As Boolean = False
        If l2.Equals(getSelectedList) Then
1:          For i = 0 To l2.Items.Count - 1
                If l2.Items(i).name = last.name Then
                    l2_2.SelectedIndex = -1
                    l2.SelectedIndex = -1
                    l2.SelectedIndex = i
                    GoTo 3
                End If
            Next
            If Not flag Then
                flag = True
                GoTo 2
            End If
        ElseIf l2_2.Equals(getSelectedList) Then
2:          For i = 0 To l2_2.Items.Count - 1
                If l2_2.Items(i).name = last.name Then
                    l2.SelectedIndex = -1
                    l2_2.SelectedIndex = -1
                    l2_2.SelectedIndex = i
                    GoTo 3
                End If
            Next
            If Not flag Then
                flag = True
                GoTo 1
            End If
        Else
            GoTo 2
3:      End If

    End Sub
    Private Sub Label_weiter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles labelNextTrack.Click
        Key.keyList(Key.keyName.Next_Track).execute()
    End Sub
    Private Sub Label_back(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles labelPrevTrack.Click
        Key.keyList(Key.keyName.Previous_Track).execute()
    End Sub
    Private Sub Label18_PARTS(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles labelPartsCount.Click
        Dim l As ListBox = getSelectedList()
        If Not radioEnabled AndAlso l IsNot Nothing AndAlso l.SelectedIndex > -1 AndAlso TypeOf l.SelectedItem Is Track Then
            Dim track As Track = l.SelectedItem
            openOverlay(eOverlayMode.PARTS)
            PartsForm.loadParts(track)
        End If
    End Sub

    Private Sub Label9_COUNT(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles labelCount.Click
        If getSelectedList().SelectedItem IsNot Nothing And searchState = SearchState.NONE Then
            Dim a As String = InputBox("Type in new count", , labelCount.Text)
            If Not a = "" Then
                saveRawSetting(SettingsIdentifier.TRACKS_COUNT, getSelectedList.SelectedItem.name, a)
                labelStatsUpdate(getSelectedList)
            End If
        End If
    End Sub

    Private Sub label15_RADIO_TIME(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles labelTimeListened.Click
        If radioEnabled Then
            If l2.SelectedIndex > -1 Then
                Dim a As String = InputBox("Type in new time", , labelTimeListened.Text)
                If Not a = "" Then
                    If dll.dhmsStringToSeconds(a) > -1 Then
                        saveRawSetting(SettingsIdentifier.RADIO_TIME, getSelectedList.SelectedItem.name, dll.dhmsStringToSeconds(a))
                        l2.SelectedItem.update()
                        labelTimeListened.Text = dll.SecondsTodhmsString(l2.SelectedItem.time)
                    Else
                        MsgBox("Invalid format", MsgBoxStyle.Information)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub Label25_A_B(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles labelLoop.Click
        Dim val As String = ""
        If trackLoop = LoopMode.INTERMEDIATE Then
            val = InputBox("From:", , dll.secondsTo_ms_Format(Int(loopVals(1))))
            If val = "" Then
                resetLoop()
            Else : loopVals(1) = dll.minFormatToSec(val)
            End If
        ElseIf trackLoop = LoopMode.YES Then
            val = InputBox("From:", , dll.secondsTo_ms_Format(Int(loopVals(1))))
            If val = "" Then
                resetLoop() : Exit Sub
            Else : loopVals(1) = dll.minFormatToSec(val)
            End If
            val = InputBox("To:", , dll.secondsTo_ms_Format(Int(loopVals(2))))
            If val = "" Then
                resetLoop()
            Else : loopVals(2) = dll.minFormatToSec(val)
            End If
        End If
    End Sub

    Public Sub setLoop(fromTime As Integer, toTime As Integer)
        loopVals(1) = fromTime
        loopVals(2) = toTime
        trackLoop = LoopMode.YES
    End Sub

    Private Sub Label16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles labelPartName.Click
        If Not labelPartName.Text = "" Then My.Computer.Clipboard.SetText(labelPartName.Text)
    End Sub

    'Private Sub wmp_ClickEvent(ByVal sender As Object, ByVal e As WMPLib._WMPOCXEvents_ClickEvent) Handles wmp.ClickEvent
    '    If Not radio AndAlso currTrack IsNot Nothing Then
    '        If e.fY + wmp.Top < wmp.Height - 42 Then
    '            Label_lastfile()
    '        End If
    '    End If
    'End Sub

    Private Sub picRepeat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picRepeat.Click
        changePlayMode(PlayMode.REPEAT)
    End Sub
    Private Sub picRandom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picRandom.Click
        changePlayMode(PlayMode.RANDOM)
    End Sub

    Sub changePlayMode(toMode As PlayMode)
        saveSetting(SettingsIdentifier.PLAY_MODE, toMode)
        If toMode = PlayMode.REPEAT Then
            picRepeat.BackgroundImage = My.Resources.rep2
            picRandom.BackgroundImage = My.Resources.invshuffle
        ElseIf toMode = PlayMode.RANDOM Then
            picRepeat.BackgroundImage = My.Resources.invrep2
            picRandom.BackgroundImage = My.Resources.shuffle
        End If
    End Sub

    Sub updatePlayMode()
        Dim currMode As PlayMode = getSetting(SettingsIdentifier.PLAY_MODE)
        If currMode = PlayMode.REPEAT Then
            picRepeat.BackgroundImage = My.Resources.rep2
            picRandom.BackgroundImage = My.Resources.invshuffle
        ElseIf currMode = PlayMode.RANDOM Then
            picRepeat.BackgroundImage = My.Resources.invrep2
            picRandom.BackgroundImage = My.Resources.shuffle
        End If
    End Sub
#End Region
    'search label update
#Region "Resizing"
    Sub formResize()

        Dim l1rat As Double = 234 / minWidth   '224
        Dim l2rat As Double = 394 / minWidth  '380
        Dim l2_2rat As Double = 280 / minWidth '270

        tv.Width = Me.Width * l1rat - 10
        l2.Left = tv.Right + 5
        l2.Width = Me.Width * l2rat - 14
        l2_2.Left = l2.Right + 5
        l2_2.Width = Me.Width * l2_2rat - 10
        wmp.Width = tv.Width

        l2.Height = Me.Height - l2.Top - 37

        tv.Height = l2.Height - wmp.Height - 1
        l2_2.Height = l2.Height
        wmp.Top = l2.Bottom - wmp.Height
        labelPrevTrack.Location = New Point(wmp.Left + 7, wmp.Bottom - 44) : picRepeat.Location = New Point(wmp.Left + 34, wmp.Bottom - 29) : picRandom.Location = New Point(wmp.Left + 75, wmp.Bottom - 31) : labelNextTrack.Location = New Point(wmp.Right - 28, wmp.Bottom - 44) : labelVolume.Location = New Point(wmp.Left + 200, wmp.Bottom - 13) : labelPrevTrack.BringToFront() : picRepeat.BringToFront() : picRandom.BringToFront() : labelNextTrack.BringToFront() : labelVolume.BringToFront()

        labelL2Count.Top = l2.Bottom - labelL2Count.Height - 2 'l2count
        labelL2_2Count.Top = l2_2.Bottom - labelL2_2Count.Height - 2 'l2_2count

        checkSeachAllFolders.Left = tSearch.Left
        checkSearchParts.Left = tSearch.Right + 4
        picCancel.Left = tSearch.Right + 2
        cancelLabel.Left = picCancel.Right + 2

        labelPartsCount2.Left = tv.Right - 77 'Parts
        labelPartsCount.Left = labelPartsCount2.Right
        labelLoop.Left = labelPartsCount2.Left 'A-B

        labelDateAdded2.Left = l2.Left + 24 'added
        labelDateAdded.Left = labelDateAdded2.Right
        labelCount2.Left = labelDateAdded2.Left + 3 'count
        labelCount.Left = labelCount2.Right

        labelPopularity2.Left = l2.Right - 177 'pop
        labelPopularity.Left = labelPopularity2.Right
        labelTimeListened2.Left = labelPopularity2.Left + 23 'time
        labelTimeListened.Left = labelTimeListened2.Right + 1

        labelGenre2.Left = l2_2.Left + 65 'genre
        labelGenre.Left = labelGenre2.Right
        labelLength2.Left = labelGenre2.Left - 4 'length
        labelLength.Left = labelLength2.Right

        ' wmp.Location = New Point(0, 50)
        '  wmp.Size = New Size(Me.Width - 15, Me.Height - 80)
        '   wmp.BringToFront()
        If Me.WindowState = FormWindowState.Minimized Then
            'If Not showMinimizedInTaskbar Then
            iconTray.Visible = True
            Me.Hide()
            ' End If
        Else
            iconTray.Visible = False
            '   If firstLoad Then resizeUpdate()
        End If
    End Sub
    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        formResize()
    End Sub

#End Region

#Region "wmp functions"

    Public Sub wmpstart(ByVal track As Track)
        If Not radioEnabled And Not track = Nothing Then
            If File.Exists(track.fullPath) Then
                setPlayRate()
                wmp.URL = track.fullPath
                last = track
                track.updateParts()
                firstStart()
            End If
            resetLoop()
        End If
    End Sub
    Public Sub wmpstart(ByVal rad As Radio)
        If radioEnabled And searchState = SearchState.NONE And rad IsNot Nothing Then
            wmp.URL = rad.url
            firstStart()
        End If
    End Sub
    Private Sub wmpstartURL(ByVal url As String)
        If searchState = SearchState.NONE Then
            resetLoop()
            setPlayRate()
            wmp.URL = url
            firstStart()
        End If
    End Sub

    Sub firstStart()
        If firstPlayStart = FirstStartState.INIT Then
            firstPlayStart = FirstStartState.STARTING
            If SettingsService.loadSetting(SettingsIdentifier.FTP_AUTO_UPDATE) Then
                dll.checkPlayerUpdate(dll.ftpCred, True)
            End If
        End If
    End Sub

    Public Sub switchSourceMode()
        If radioEnabled Then
            changeSourceMode(MusicSource.LOCAL)
        Else
            changeSourceMode(MusicSource.RADIO)
        End If
    End Sub
    Public Sub changeSourceMode(ByVal mode As MusicSource)
        If Not searchState = SearchState.NONE Then cancelSearch(False)
        If mode = 0 Then
            tv.Enabled = True
            l2_2.Enabled = True
            tSearch.Enabled = True
            menuSortBy.Enabled = True
            menuLyrics.Enabled = True
            menuStatistics.Enabled = True
            If radioEnabled Then
                saveRadioTime()
                setSetting(SettingsIdentifier.MUSIC_SOURCE, MusicSource.LOCAL)
                l2.Items.Clear()
                localfill(False)
                sortListAuto()
                setlistselected()
                wmp.settings.mute = False
                wmp.Ctlcontrols.pause()
            Else

                If l2.SelectedIndex > -1 Then
                    wmpstart(l2.SelectedItem)
                    Dim tr As Track = l2.SelectedItem
                    tr.play()
                ElseIf l2_2.SelectedIndex > -1 Then
                    wmpstart(l2_2.SelectedItem)
                ElseIf Not currTrack = Nothing Then
                    wmpstart(currTrack)
                End If
            End If
        Else
            If radioEnabled Then
                saveRadioTime()
            End If
            resetLoop()
            tv.Nodes.Clear()
            radfill()
            clearPlaylist()
            tv.Enabled = False
            l2_2.Enabled = False
            tSearch.Enabled = False
            menuSortBy.Enabled = False
            menuLyrics.Enabled = False
            menuStatistics.Enabled = False
            If l2.Items.Count > 0 And wmp.playState = WMPPlayState.wmppsPlaying Then
                wmpstart(l2.SelectedItem)
            End If
        End If
        saveSetting(SettingsIdentifier.MUSIC_SOURCE, mode)
        keydelay()

    End Sub
#End Region

#Region "Track Parts"
    Sub switchpart(ByVal switchdir As Integer) 'dir 2-forward,1-back
        If Not radioEnabled Then
            currTrack.currPart = currTrack.getCurrentPart()
            If currTrackPart IsNot Nothing Then
                If switchdir = 1 Then
                    If trackLoop = LoopMode.YES Then
                        currTrack.prevPart()
                    End If
                Else
                    currTrack.nextPart()
                End If
                loopVals(1) = currTrackPart.fromSec
                loopVals(2) = currTrackPart.toSec
                trackLoop = LoopMode.YES
                wmp.Ctlcontrols.currentPosition = loopVals(1)
                labelStatsUpdate()
                labelLoop.Cursor = Cursors.Hand
            End If
        End If
    End Sub
    Function ParseMinuteSecondString(ByVal s As String) As String()
        If s Is Nothing Then Return Nothing
        Return s.Split(",")
    End Function


    Sub resetLoop()
        trackLoop = LoopMode.NO
        loopVals(1) = 0
        loopVals(2) = 0
        labelLoop.Cursor = Cursors.Default
    End Sub

#End Region

#Region "Playlist"
    Public Sub selectPlaylist(ByVal index As Integer)
        If playlist.Count > index Then
            l2_2.SelectedIndex = index
        End If
    End Sub

    Public Function playlistContains(ByVal track As Track) As Integer
        If playlist.Contains(track) Then
            Return playlist.IndexOf(track)
        Else
            For i = 0 To playlist.Count - 1
                If playlist(i).name = track.name Then
                    Return i
                End If
            Next
        End If
        Return -1
    End Function

    Function getNextRandomTrack() As Track
        Dim tracks As New List(Of Track)
        For Each item In l2.Items
            If TypeOf item Is Track Then
                tracks.Add(item)
            End If
        Next
        If tracks.Count = 0 Then Return Nothing
        Return tracks(rnd.Next(0, tracks.Count))
    End Function

    Public Sub playNextTrack()
        If currTrack Is Nothing Or currTrack IsNot Nothing AndAlso currTrack.getPlaylistIndex() = playlist.Count - 1 Then
            If l2.Items.Count = 0 Then refill(Not playMode = PlayMode.REPEAT)
            If l2.Items.Count > 0 Then

                Dim nextTrack As Track
                If randomNextTrack Then
                    nextTrack = getNextRandomTrack()
                Else
                    sortListAuto()
                    nextTrack = l2.Items(0)
                End If

                If nextTrack = Nothing Then
                    playlist(0).play()
                Else
                    If removeNextTrack Then
                        l2.Items.Remove(nextTrack)
                    End If
                    nextTrack.addToPlaylist()
                    nextTrack.selectPlaylist()

                    If wmp.playState = WMPLib.WMPPlayState.wmppsPlaying Or wmp.playState = WMPPlayState.wmppsUndefined Or wmp.playState = WMPPlayState.wmppsStopped Then nextTrack.play()
                End If
            Else
                playlist(0).play()
            End If
        Else
            playlist(currTrack.getPlaylistIndex() + 1).play()
        End If
    End Sub

    Public Sub playPrevTrack()
        If currTrack = Nothing Then
            If l2_2.SelectedIndex > 0 Then l2_2.SelectedIndex -= 1
        Else
            If currTrack.getPlaylistIndex() > 0 Then playlist(currTrack.getPlaylistIndex() - 1).play()
        End If
    End Sub

    Sub clearPlaylist()
        For i = playlist.Count - 1 To 0 Step -1
            playlist(i).removeFromPlaylist()
        Next
    End Sub
#End Region

#Region "Search"

    Sub searchFill()
        Cursor = Cursors.WaitCursor

        l2.Items.Clear()
        Dim fols As List(Of Folder) = Nothing
        If searchAllFolders Then
            fols = Folder.getFolders(False, False)
            fols.Remove(Folder.getSelectedFolder(tv))
            fols.Insert(0, Folder.getSelectedFolder(tv))
        Else
            fols = New List(Of Folder) From {Folder.getSelectedFolder(tv)}
        End If
        fols.Remove(Nothing)

        Dim list As New List(Of Object)
        For i = 0 To fols.Count - 1

            Dim tempList As New List(Of Object)
            Dim foundTrack As Boolean = False
            For Each t As Track In fols(i).tracks

                Dim parsed() As String = tSearch.Text.Split(New Char() {","})

                Dim trackAdded As Boolean = False
                If Array.TrueForAll(parsed, Function(p) t.name.ToLower.Contains(p)) Then
                    tempList.Add(t)
                    trackAdded = True
                End If


                If searchParts Then
                    If t.partsCount > 1 Then

                        For Each tp As TrackPart In t.parts
                            If Array.TrueForAll(parsed, Function(p) tp.name.ToLower.Contains(p)) Then
                                If Not trackAdded Then
                                    trackAdded = True
                                    tempList.Add(t)
                                End If
                                tempList.Add(tp)
                            End If
                        Next
                    End If
                End If

                If trackAdded Then foundTrack = True
            Next

            If foundTrack Then
                If list.Count > 0 Then list.Add("")
                list.Add(fols(i))
                list.AddRange(tempList)
            End If

        Next
        l2.Items.AddRange(list.ToArray)
        Cursor = Cursors.Default
    End Sub



    Private Sub t2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tSearch.Click
        If Not optionsMode And Not radioEnabled Then
            If searchState = SearchState.NONE Then
                initSearch()
            ElseIf searchState = SearchState.INIT Then
                tSearch.Text = ""
            End If

            If Not l2.SelectedIndex = -1 Then
                If wmp.playState = WMPLib.WMPPlayState.wmppsPlaying Then
                    last = l2.SelectedItem
                End If
            End If
        End If
    End Sub
    Private Sub t2_GotFocus(sender As Object, e As EventArgs) Handles tSearch.GotFocus

    End Sub

    Sub initSearch()
        tSearch.Text = ""
        checkSeachAllFolders.Visible = True : checkSeachAllFolders.Text = "Search All Folders"
        checkSearchParts.Visible = True : checkSearchParts.Text = "Search Track Parts"
        cancelLabel.Visible = True : picCancel.Visible = True : cancelLabel.Text = "Cancel Search"
        searchState = SearchState.INIT

        If searchParts Then
            For i = 0 To l2.Items.Count - 1
                If TypeOf l2.Items(i) Is Track Then
                    l2.Items(i).updateparts()
                End If
            Next
        End If

        l2.Items.Clear()
        refill() '(false) 08.07.19
    End Sub

    Sub cancelSearch(Optional refillList As Boolean = True)
        If Not searchState = SearchState.NONE Then
            tSearch.Text = "Search..."
            checkSeachAllFolders.Visible = False
            checkSearchParts.Visible = False
            cancelLabel.Visible = False : picCancel.Visible = False
            searchState = SearchState.NONE

            If refillList Then
                l2.Items.Clear()
                refill()
                setlistselected()
            End If
            labelDateAdded.Focus()
        End If
    End Sub

    Private Sub cancelLabel_Click(sender As Object, e As EventArgs) Handles cancelLabel.Click, picCancel.Click
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

            If searchState = SearchState.SEARCHING Then
                Dim match As Track = Nothing
                If l2.Items.Count > 0 Then
                    If TypeOf l2.Items(1) Is Track Then
                        match = l2.Items(1)
                        Dim ind As Integer = listContains(l2_2, match)
                        If ind > -1 And ind < l2_2.Items.Count - 1 Then
                            match.removeFromPlaylist()
                        End If
                        match.selectPlaylist()
                        Dim l = Track.playlist.Count
                    End If
                End If
            End If
            cancelSearch()
        End If
    End Sub

    Private Sub t2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tSearch.TextChanged
        Dim inverted As Boolean = SettingsService.getSetting(SettingsIdentifier.DARK_THEME)
        tSearch.ForeColor = IIf(tSearch.Text = "Search...", Color.DimGray, IIf(inverted, Color.White, Color.Black))

        If tSearch.Text = "" Then
            If Not searchState = SearchState.INIT Then
                searchState = SearchState.EMPTY
                l2.Items.Clear()
                refill()
            End If

        ElseIf Not tSearch.Text = "Search..." Then
            searchState = SearchState.SEARCHING
            tSearch.Text = tSearch.Text.ToLower
            searchFill()
        End If
    End Sub


    Private Sub checkSeachAllFolders_CheckedChanged(sender As Object, e As EventArgs) Handles checkSeachAllFolders.CheckedChanged
        saveSetting(SettingsIdentifier.SEARCH_ALL_FOLDERS, checkSeachAllFolders.Checked)
        If searchState = SearchState.SEARCHING Then searchFill()
        tSearch.Focus()

    End Sub

    Private Sub checkSearchParts_CheckedChanged(sender As Object, e As EventArgs) Handles checkSearchParts.CheckedChanged
        saveSetting(SettingsIdentifier.SEARCH_PARTS, checkSearchParts.Checked)
        If searchParts Then
            For i = 0 To l2.Items.Count - 1
                If TypeOf l2.Items(i) Is Track Then
                    l2.Items(i).updateparts()
                End If
            Next
        End If
        If searchState = SearchState.SEARCHING Then searchFill()
        tSearch.Focus()
    End Sub
    Private Sub checkSearchParts_VisibleChanged(sender As Object, e As EventArgs) Handles checkSearchParts.VisibleChanged, checkSeachAllFolders.VisibleChanged
        checkSearchParts.Checked = searchParts
    End Sub
    Private Sub checkSearchAllFolders_VisibleChanged(sender As Object, e As EventArgs) Handles checkSearchParts.VisibleChanged
        checkSeachAllFolders.Checked = searchAllFolders
    End Sub

#End Region

#Region "Meta Stats"
    Function isEncoded() As Boolean
        If Not IniService.iniIsValidSection("Musik", logpath) Then
            Return True
        End If
        Return False
    End Function

    Sub savePaths()
        saveSetting(SettingsIdentifier.INIPATH, inipath)
        saveSetting(SettingsIdentifier.PATH, path)
        saveSetting(SettingsIdentifier.PLAYLISTPATH, playlistPath)
        saveSetting(SettingsIdentifier.LOGPATH, logpath)
        saveSetting(SettingsIdentifier.LYRPATH, lyrpath)
        saveSetting(SettingsIdentifier.FTPPATH, ftpPath)
    End Sub

    Public Sub saveLastTrack(Optional val As String = "")
        SettingsService.saveSetting(SettingsIdentifier.LAST_TRACK_FILE, IIf(val = "", last.virtualPath, val))
    End Sub

    Public Sub saveCurrPlaylistHistory()
        For Each t As Track In playlist
            SettingsService.saveRawSetting(SettingsIdentifier.PLAYLIST_HISTORY, t.name, t.fullPath)
        Next
    End Sub

    Function loaddates() As Boolean
        If datesInitiallyLoaded Then Return True
        If Not File.Exists(logpath) Then
            If Not SettingsService.exists(SettingsIdentifier.LOGPATH) Then
                SettingsService.saveSetting(SettingsIdentifier.LOGPATH, "")
                If MsgBox("No file for track dates found. Assign now?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    showOptions(OptionsForm.optionState.PATHS, True)
                    GoTo 1
                Else
                    Return finishLoadDates(False)
                End If
            End If
        Else
1:          gldt.Clear()
            glnames.Clear()
            Dim encData As String = ""
            Dim decr As String
            If readContent(logpath, encData) Then
                decr = encData
                If checkFileHeader(encData) OrElse dll.decrypt(decr, encData, logPathKey) = 0 Then
                    Dim split() As String = decr.Split(vbNewLine)
                    Dim activeFlag As Boolean = False
                    For Each s In split
                        If s.Contains("[") And s.Contains("]") And activeFlag Then Exit For
                        If s.Contains("[Musik]") Then activeFlag = True

                        If s.Contains("=") And activeFlag Then
                            gldt.Add(s.Substring(s.IndexOf("=") + 1, 10))
                            glnames.Add(s.Substring(s.IndexOf("=") + 16))
                        End If
                    Next
                    Return finishLoadDates(True)
                End If

            End If
            Return finishLoadDates(False)
        End If
        Return finishLoadDates(True)
    End Function

    Function finishLoadDates(result As Boolean) As Boolean
        datesInitiallyLoaded = True
        Return result
    End Function
    Function checkFileHeader(ByVal str As String) As Boolean
        Return str.Length >= 2 AndAlso str.Substring(0, 2) = "ev"
    End Function
    Function readContent(filePath As String, ByRef buffer As String) As Boolean
        Try
            Dim sr As New StreamReader(filePath, Encoding.GetEncoding(1252))
            buffer = sr.ReadToEnd()
            sr.Close()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function getDate(ByVal track As String, Optional ByVal reloadDates As Boolean = True) As String
        If reloadDates Or gldt.Count = 0 Or glnames.Count = 0 Then
            If Not loaddates() Then
                'loading dates failed
            End If
        End If
        Dim res As String = glnames.IndexOf(track)
        If res = -1 Then
            Return ""
        Else
            Return gldt(res)
        End If
    End Function

    Sub saveRadioTime()
        If l2.Items.Count > 0 And radioEnabled Then
            For i = 0 To l2.Items.Count - 1
                l2.Items(i).update()
                Dim newVal As Integer = l2.Items(i).time + l2.Items(i).timeTemp
                l2.Items(i).time = newVal
                l2.Items(i).timetemp = 0
                saveRawSetting(SettingsIdentifier.RADIO_TIME, l2.Items(i).name, newVal)
            Next
        End If
    End Sub

#End Region

#Region "Tcp"

    Sub TcpStart(ByVal port As Integer, Optional ByVal errorMsg As Boolean = True)
        If remoteTcp IsNot Nothing Then
            remoteTcp.stopListener()
            remoteTcp.stopAllConnections()
        End If
        remoteTcp = New Tcp()
        TcpStartListener(port, errorMsg)

    End Sub

    Sub TcpStartListener(ByVal port As Integer, Optional ByVal errorMsg As Boolean = True)
        If remoteTcp.startListener(port) Then
            OptionsForm.labelPort.Text = remoteTcp.port
            FormUtils.setRemoteImage()
            OptionsForm.setListenerStatus()

            TcpListen()
        Else
            OptionsForm.labelPort.Text = port
            remoteTcp.port = port
            FormUtils.setRemoteImage()
            remoteTcp.stopListener()
            OptionsForm.setListenerStatus()
            If errorMsg Then MsgBox("Port is not open", MsgBoxStyle.Exclamation)
        End If
    End Sub

    Sub TcpStopConnection(ip As String, ByVal reason As String, Optional ByVal showErr As Boolean = True)
        Try
            remoteTcp.send(ip, "disconnect")
        Catch ex As Exception
            If showErr Then MsgBox(reason & ": dc send failed")
        End Try
        Dim res As Integer = remoteTcp.stopConnection(ip)
        TcpStopConnectionHandler(res, reason, showErr)
    End Sub
    Sub TcpStopConnection(connection As Tcp.ClientConnection, ByVal reason As String, Optional ByVal showErr As Boolean = True)
        Try
            connection.send("disconnect")
        Catch ex As Exception
            If showErr Then MsgBox(reason & ": dc send failed")
        End Try
        Dim res As Integer = connection.closeConnection()
        TcpStopConnectionHandler(res, reason, showErr)
    End Sub

    Sub TcpStopAllConnections(ByVal reason As String, Optional ByVal showErr As Boolean = True)
        Try
            remoteTcp.sendAll("disconnect")
        Catch ex As Exception
            If showErr Then MsgBox(reason & ": dc send failed")
        End Try
        Dim res As Integer = remoteTcp.stopAllConnections()
        TcpStopConnectionHandler(res, reason, showErr)
    End Sub

    Sub TcpStopConnectionHandler(resultCode As Integer, ByVal reason As String, Optional ByVal showErr As Boolean = True)
        If resultCode = 1 Then
            If showErr Then MsgBox(reason & ": thread close failed")
        ElseIf resultCode = 2 Then
            If showErr Then MsgBox(reason & ": client close failed")
        End If
        OptionsForm.refreshRemoteUI()
        FormUtils.setRemoteImage()
    End Sub

    Async Sub TcpListen()
        Do
            Dim res As Tcp.ListenResult = Await remoteTcp.listen(0)

            If res.resultCode = 1 Then
                If Not SettingsService.getSetting(SettingsIdentifier.REMOTE_BLOCK_EXT_IPS) Then
                    OptionsForm.SendToBack()
                    If MsgBox("External device [" & remoteTcp.getIp(res.client) & "] requesting remote access." & vbNewLine & "Accept connection?", MsgBoxStyle.YesNo + MsgBoxStyle.Information) = MsgBoxResult.No Then
                        remoteTcp.stopConnection(remoteTcp.getIp(res.client))
                    Else
                        If remoteTcp.establishConnection(res.client).resultCode = 2 Then
                            FormUtils.setRemoteImage()
                            OptionsForm.refreshRemoteUI()

                        End If
                    End If
                Else
                    remoteTcp.stopConnection(remoteTcp.getIp(res.client))
                End If
            ElseIf res.resultCode = 2 Then
                FormUtils.setRemoteImage()
                OptionsForm.refreshRemoteUI()
            ElseIf res.resultCode = 3 Then
                Exit Do
            End If
        Loop
    End Sub

    Sub TCPHandler()
        Dim connections = remoteTcp.getRandomConnectionOrder()
        connections.ForEach(
            Sub(c)
                TcpHandler(c)
            End Sub)
    End Sub

    Sub TcpHandler(connection As Tcp.ClientConnection)
        Dim fullComm As String = connection.tcpMsg

        If fullComm Is Nothing Then
            TcpStopConnection(connection, "abort")
            Return
        End If
        If Not fullComm = "" Then
            connection.tcpMsg = ""
            If fullComm(fullComm.Length - 1) = vbLf Then fullComm = fullComm.Substring(0, fullComm.Length - 1)
            Dim fullSplit() As String = fullComm.Split("|")
            For k = 0 To fullSplit.Length - 1
                Dim comm As String = fullSplit(k)
                Select Case comm
                    Case "disconnect"
                        TcpStopConnection(connection, "abort")
                    Case "req"
                        connection.send("ack")
                    Case "play_pause"
                        Key.keyList(Key.keyName.Play_Pause).execute()
                    Case "headset"
                        setSoundDevice("Headset")
                    Case "speakers"
                        setSoundDevice("Speaker")
                    Case "bluetooth"
                        setSoundDevice("Speakers")
                    Case "volume_min"
                        Key.keyList(Key.keyName.Volume_Min).execute()
                    Case "volume_down"
                        Key.keyList(Key.keyName.Volume_Down).execute()
                    Case "fast_forward"
                        Key.keyList(Key.keyName.Fast_Forward).execute()
                    Case "previous_track"
                        Key.keyList(Key.keyName.Previous_Track).execute()
                    Case "source_local"
                        Key.keyList(Key.keyName.Source_Local).execute()
                    Case "next_track"
                        Key.keyList(Key.keyName.Next_Track).execute()
                    Case "fast_rewind"
                        Key.keyList(Key.keyName.Fast_Rewind).execute()
                    Case "volume_up"
                        Key.keyList(Key.keyName.Volume_Up).execute()
                    Case "volume_max"
                        Key.keyList(Key.keyName.Volume_Max).execute()
                    Case "volume_mute"
                        Key.keyList(Key.keyName.Volume_Mute).execute()
                    Case "slow_rewind"
                        Key.keyList(Key.keyName.Slow_Rewind).execute()
                    Case "random_mode"
                        Key.keyList(Key.keyName.Random_Mode).execute()
                    Case "previous_part"
                        Key.keyList(Key.keyName.Previous_Part).execute()
                    Case "source_radio"
                        Key.keyList(Key.keyName.Source_Radio).execute()
                    Case "next_part"
                        Key.keyList(Key.keyName.Next_Part).execute()
                    Case "repeat_mode"
                        Key.keyList(Key.keyName.Repeat_Mode).execute()
                    Case "slow_forward"
                        Key.keyList(Key.keyName.Slow_Forward).execute()
                    Case "volume_half"
                        Key.keyList(Key.keyName.Volume_Half).execute()
                    Case "tree_up"
                        Key.keyList(Key.keyName.Tree_Up).execute()
                    Case "search"
                        Key.keyList(Key.keyName.Search).execute()
                    Case "search_youtube"
                        Process.Start("https://www.youtube.com/")
                        SendKeys.SendWait("#")
                    Case "search_spotify"
                        Process.Start("https://play.spotify.com/search")
                    Case "tree_down"
                        Key.keyList(Key.keyName.Tree_Down).execute()
                    Case "item_remove"
                        Key.keyList(Key.keyName.Track_Remove).execute()
                    Case "item_next"
                        Key.keyList(Key.keyName.Track_PlayNext).execute()
                    Case "item_queue"
                        Key.keyList(Key.keyName.Track_ToQueue).execute()
                    Case "loop_track"
                        Key.keyList(Key.keyName.Track_Loop).execute()
                    Case "key_space"
                        SendKeys.Send(" ")
                    Case "key_enter" : SendKeys.Send("{ENTER}")
                    Case "key_center" : SendKeys.Send("^{ENTER}")
                    Case "key_aenter" : SendKeys.Send("%{ENTER}")
                    Case "key_senter" : SendKeys.Send("+{ENTER}")
                    Case "key_csenter" : SendKeys.Send("^+{ENTER}")
                    Case "key_left" : SendKeys.Send("{LEFT}")
                    Case "key_cleft" : SendKeys.Send("^{LEFT}")
                    Case "key_aleft" : SendKeys.Send("%{LEFT}")
                    Case "key_sleft" : SendKeys.Send("+{LEFT}")
                    Case "key_csleft" : SendKeys.Send("^+{LEFT}")
                    Case "key_right" : SendKeys.Send("{RIGHT}")
                    Case "key_cright" : SendKeys.Send("^{RIGHT}")
                    Case "key_aright" : SendKeys.Send("%{RIGHT}")
                    Case "key_sright" : SendKeys.Send("+{RIGHT}")
                    Case "key_csright" : SendKeys.Send("^+{RIGHT}")
                    Case "key_up" : SendKeys.Send("{UP}")
                    Case "key_cup" : SendKeys.Send("^{UP}")
                    Case "key_aup" : SendKeys.Send("%{UP}")
                    Case "key_sup" : SendKeys.Send("+{UP}")
                    Case "key_csup" : SendKeys.Send("^+{UP}")
                    Case "key_down" : SendKeys.Send("{DOWN}")
                    Case "key_cdown" : SendKeys.Send("^{DOWN}")
                    Case "key_adown" : SendKeys.Send("%{DOWN}")
                    Case "key_sdown" : SendKeys.Send("+{DOWN}")
                    Case "key_csdown" : SendKeys.Send("^+{DOWN}")
                    Case "key_f8"
                        Key.keyList(Key.keyName.Macro_4).execute()
                    Case "key_tab" : SendKeys.Send("{TAB}")
                    Case "key_ctab" : SendKeys.Send("^{TAB}")
                    Case "key_atab" : SendKeys.Send("%{TAB}")
                    Case "key_stab" : SendKeys.Send("+{TAB}")
                    Case "key_cstab" : SendKeys.Send("^+{TAB}")
                    Case "key_bksp" : SendKeys.Send("{BKSP}")
                    Case "key_cbksp" : SendKeys.Send("^{BKSP}")
                    Case "key_esc" : SendKeys.Send("{ESC}")
                    Case "winswitch"
                        ' Dim shell As New Shell32.Shell
                        '  shell.WindowSwitcher()
                    Case "shut"
                        Shell("shutdown -s -t 0")
                    Case "lmouse"
                        Utils.lMouseClick()
                    Case "lmouse2"
                        Utils.lMouseClick(2)
                    Case "rmouse"
                        Utils.rMouseClick()
                    Case "mmouse"
                        Utils.mMouseClick()
                    Case "sysvol_down"
                        SysVol.system_volume_down()
                        If lastTCPCommand = comm Then SysVol.system_volume_down()
                    Case "sysvol_up"
                        SysVol.system_volume_up()
                        If lastTCPCommand = comm Then SysVol.system_volume_up()
                    Case "sysvol_mute"
                        SysVol.system_volume_mute()
                    Case Else

                        comm = comm.Replace(vbLf, "")
                        Dim maList As List(Of Integer()) = getMouseMove(comm)
                        Dim scList As List(Of Integer) = getMouseScroll(comm)
                        For i = 0 To maList.Count - 1
                            Cursor.Position = New Point(Cursor.Position.X - maList(i)(0), Cursor.Position.Y - maList(i)(1))
                        Next
                        For i = 0 To scList.Count - 1
                            Utils.mouse_event(Utils.MOUSEEVENTF_WHEELROTATE, 0, 0, scList(i), 0)
                        Next

                        If comm.StartsWith("magnify_") Then
                            Dim zoom As Integer = comm.Substring(comm.LastIndexOf("magnify_") + 8)
                            If zoom > 100 Then
                                If Not isProcessAlive("magnify") Then Process.Start("magnify") 'Shell("magnify")
                            Else
                                Utils.killProc("magnify")
                            End If
                            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\Microsoft\ScreenMagnifier", "Magnification", zoom)

                        ElseIf comm.StartsWith("position_") Then
                            Dim m As Integer = comm.Substring(9, comm.LastIndexOf("_") - 9)
                            Dim s As Integer = comm.Substring(comm.LastIndexOf("_") + 1)
                            wmp.Ctlcontrols.currentPosition = m * 60 + s
                        ElseIf comm.StartsWith("sysvol_") Then
                            Try
                                Dim v As Integer = comm.Substring(7)
                                dll.SetVolume(v)
                            Catch ex As Exception
                            End Try
                        ElseIf comm.StartsWith("volume_") Then
                            Try
                                Dim v As Integer = comm.Substring(7)
                                wmp.settings.volume = v
                            Catch ex As Exception
                            End Try
                        ElseIf comm.StartsWith("monitor_") Then
                            Dim base As String = "C:\Windows\WinSxS\amd64_microsoft-windows-displayswitch_31bf3856ad364e35_10.0.18362.1_none_bf1f20f4a1b3e35a\Displayswitch.exe"
                            Dim ext As String = comm.Substring(8)
                            If ext = "internal" Then
                                Shell(base & " /internal")
                            ElseIf ext = "external" Then
                                Shell(base & " /extend")
                            ElseIf ext = "clone" Then
                                Shell(base & " /clone")
                            ElseIf ext = "on" Then
                                SendKeys.Send("{SCROLLLOCK}")
                            ElseIf ext = "off" Then
                                dll.SetMonitorState(0, Process.GetCurrentProcess.MainWindowHandle)
                            End If
                        ElseIf comm.StartsWith("block") Then
                            If comm.Length > 5 Then
                                If comm.Substring(6) = "off" Then
                                    Utils.BlockInput(False)
                                Else
                                    Utils.BlockInput(True)
                                    Dim tim As Integer = comm.Substring(6)
                                    For i = 0 To Int(tim / 1000)
                                        Threading.Thread.Sleep(IIf(i < Int(tim / 1000), 1000, tim Mod 1000))
                                    Next
                                    Utils.BlockInput(False)
                                End If
                            Else
                                Utils.BlockInput(True)
                            End If
                        ElseIf comm.StartsWith("req") Then
                            If comm.StartsWith("reql2") Then
                                If Not radioEnabled Then
                                    Dim currFolder As Folder
                                    Dim conNode() As TreeNode = tv.Nodes.Find(Folder.top.name & "\" & comm.Substring(5), True)
                                    If conNode.Length > 0 Then
                                        currFolder = Folder.getFolder(Folder.top.fullPath & conNode(0).Text & "\")
                                    Else
                                        currFolder = Folder.getFolder(Folder.top.fullPath & "Everything" & "\")
                                    End If
                                    Dim tracks As List(Of Track) = currFolder.tracks
                                    Dim sendS As String = ""
                                    Dim currBufferCount As Integer = 0
                                    For i = 0 To tracks.Count - 2
                                        sendS &= tracks(i).ToString & vbLf
                                        currBufferCount += tracks(i).ToString.Length + 1
                                    Next
                                    sendS &= tracks(tracks.Count - 1).ToString
                                    currBufferCount += tracks(tracks.Count - 1).ToString.Length
                                    Dim chunks As Integer = Int((currBufferCount + 6) / 1024) + 1
                                    If chunks = 1 Then
                                        connection.send("ansl2" & sendS.Substring(0, currBufferCount) & "*")
                                    Else
                                        connection.send("ansl2" & sendS.Substring(0, 1019))
                                        For i = 1 To chunks - 2
                                            connection.send(sendS.Substring(1019 + (i - 1) * 1024, 1024))
                                        Next
                                        connection.send(sendS.Substring(1019 + (chunks - 2) * 1024) & "*")
                                    End If
                                Else
                                    Dim sendS As String = ""
                                    For i = 0 To l2.Items.Count - 1
                                        sendS &= l2.Items(i).name & IIf(i = l2.Items.Count - 1, "", vbLf)
                                    Next
                                    connection.send("ansl2" & sendS & "*")
                                End If
                            ElseIf comm.StartsWith("reql3") Then
                                If l2_2.Items.Count = 0 Or radioEnabled Then
                                    connection.send("ansl3*")
                                Else
                                    Dim sendS As String = ""
                                    Dim currBufferCount As Integer = 0
                                    For i = 0 To l2_2.Items.Count - 2
                                        sendS &= l2_2.Items(i).ToString & vbLf
                                        currBufferCount += l2_2.Items(i).ToString.Length + 1
                                    Next
                                    sendS &= l2_2.Items(l2_2.Items.Count - 1).ToString
                                    currBufferCount += l2_2.Items(l2_2.Items.Count - 1).ToString.Length
                                    Dim chunks As Integer = Int((currBufferCount + 6) / 1024) + 1
                                    If chunks = 1 Then
                                        connection.send("ansl3" & sendS.Substring(0, currBufferCount) & "*")
                                    Else
                                        connection.send("ansl3" & sendS.Substring(0, 1019))
                                        For i = 1 To chunks - 2
                                            connection.send(sendS.Substring(1019 + (i - 1) * 1024, 1024))
                                        Next
                                        connection.send(sendS.Substring(1019 + (chunks - 2) * 1024) & "*")
                                    End If
                                End If
                            ElseIf comm.StartsWith("reqtv") Then
                                If Not radioEnabled Then
                                    Dim sendS As String = "Everything"
                                    Dim sendCurr As Integer = 0
                                    For Each g As Genre In Genre.genres
                                        sendS &= vbLf & g.name
                                    Next
                                    If Genre.contains(Folder.getSelectedFolder(tv).name) Then
                                        sendS &= vbLf & Folder.getSelectedFolder(tv).name 'Array.IndexOf(genres, tv.SelectedNode.Name)
                                    Else : sendS &= vbLf & "Everything" '"0"
                                    End If
                                    connection.send("anstv" & sendS)
                                Else
                                    connection.send("anstvrad")
                                End If
                            ElseIf comm.StartsWith("reqlb") Then
                                Dim sendS As String = ""
                                If currTrack IsNot Nothing Then
                                    currTrack.invalidateStats()
                                    sendS &= currTrack.name & vbLf & currTrack.count & vbLf &
                                                                 CInt(currTrack.length) & vbLf & currTrack.added.ToShortDateString
                                Else
                                    If radioEnabled And wmp.playState = WMPPlayState.wmppsPlaying Then
                                        sendS &= l2.SelectedItem.name & vbLf & "0" & vbLf & "0" & vbLf & "0"
                                    Else
                                        sendS &= "" & vbLf & "0" & vbLf & "0" & vbLf & "0"
                                    End If
                                End If
                                connection.send("anslb" & sendS)
                            End If
                        ElseIf comm.StartsWith("currwindow") Then
                            connection.send("anscurrwindow" & KeyloggerModule.getWindowTitle())
                        ElseIf comm.StartsWith("pll2") Or comm.StartsWith("pll3") Then
                            Dim trName As String = comm.Substring(4)
                            If radioEnabled Then
                                For i = 0 To l2.Items.Count - 1
                                    If l2.Items(i).name = trName Then
                                        l2.Items(i).play()
                                    End If
                                Next
                            Else
                                Dim tr As Track = Track.getFirstTrack(trName)
                                If tr IsNot Nothing Then
                                    tr.play()
                                End If
                            End If
                        ElseIf comm.StartsWith("addd_next") And Not radioEnabled Then
                            Dim trString As String = comm.Substring(9)
                            Dim tr As Track = Track.getFirstTrack(trString)
                            If tr IsNot Nothing Then
                                tr.playNext()
                            End If
                        ElseIf comm.StartsWith("add_queue") And Not radioEnabled Then
                            Dim trString As String = comm.Substring(9)
                            Dim tr As Track = Track.getFirstTrack(trString)
                            If tr IsNot Nothing Then
                                tr.addToPlaylist()
                            End If
                        ElseIf comm.StartsWith("skc_") Then
                            If comm.EndsWith(" ") Then comm = comm.Substring(0, comm.Length - 1)
                            SendKeys.Send("^(" & comm.Substring(4) & ")")
                        ElseIf comm.StartsWith("ska_") Then
                            If comm.EndsWith(" ") Then comm = comm.Substring(0, comm.Length - 1)
                            SendKeys.Send("%(" & comm.Substring(4) & ")")
                        ElseIf comm.StartsWith("sks_") Then
                            If comm.EndsWith(" ") Then comm = comm.Substring(0, comm.Length - 1)
                            SendKeys.Send("+(" & comm.Substring(4) & ")")
                        ElseIf comm.StartsWith("sk_") Then
                            If comm.EndsWith(" ") Then comm = comm.Substring(0, comm.Length - 1)
                            SendKeys.Send(comm.Substring(3))
                        Else
                            If maList.Count = 0 And scList.Count = 0 Then
                                If SettingsService.loadSetting(SettingsIdentifier.REMOTE_BLOCK_MESSAGES) = 0 Then
                                    If MsgBox(remoteTcp.getIp(connection.client) & " at " & Now.ToShortTimeString & " (" & k + 1 & "/" & fullSplit.Length & "):" & vbNewLine & comm, MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then
                                        Exit For
                                    End If
                                End If
                            End If
                        End If
                End Select
                lastTCPCommand = comm
            Next

        End If
    End Sub

    Function getMouseScroll(ByVal raw As String) As List(Of Integer)
        Dim res As New List(Of Integer)
        If raw.Contains(";") And (raw.StartsWith("ma") Or raw.StartsWith("sc")) Then
            Dim n As Integer = raw.Length
            Do Until raw.Length = 0
                If raw(0) = "s" Then
                    res.Add(CInt(raw.Substring(2, raw.IndexOf(";") - 2)))
                    If raw.Length > raw.IndexOf(";") Then
                        raw = raw.Substring(raw.IndexOf(";") + 1)
                    Else
                        raw = ""
                    End If
                Else : raw = raw.Substring(raw.IndexOf(";") + 1)
                End If
                If n < 0 Then Exit Do
            Loop
        End If
        Return res
    End Function
    Function getMouseMove(ByVal raw As String) As List(Of Integer())
        Dim res As New List(Of Integer())
        If raw.Contains(";") And (raw.StartsWith("ma") Or raw.StartsWith("sc")) Then
            Dim n As Integer = raw.Length
            Do Until raw.Length = 0
                If raw(0) = "m" Then
                    res.Add({CInt(raw.Substring(2, raw.IndexOf("_") - 2)), CInt(raw.Substring(raw.IndexOf("_") + 1, raw.IndexOf(";") - raw.IndexOf("_") - 1))})
                    If raw.Length > raw.IndexOf(";") Then
                        raw = raw.Substring(raw.IndexOf(";") + 1)
                    Else
                        raw = ""
                    End If
                Else : raw = raw.Substring(raw.IndexOf(";") + 1)
                End If
                If n < 0 Then Exit Do
            Loop
        End If
        Return res
    End Function
#End Region



#Region "Gadget Helper"


#End Region

#Region "System Functions"
    Function isFileString(ByVal s As String) As Boolean
        If s.Contains(":") And s.Contains("\") And s.Contains(".") Then
            If s.LastIndexOf(".") > s.LastIndexOf("\") Then Return True
        End If
        Return False
    End Function

    Function isValidFilePath(ByVal s As String, Optional ByVal ext As String = "") As Boolean
        Return s.Length > 3 AndAlso Not s.Contains("\\") AndAlso Not s.EndsWith(" ") AndAlso Not s.StartsWith(" ") AndAlso s.Substring(1, 2) = ":\" AndAlso Not s.EndsWith("\") AndAlso s.Contains("\") AndAlso Not s.EndsWith(".") AndAlso IIf(ext = "", True, s.EndsWith("." & ext))
    End Function

    Function isValidDirectoryPath(ByVal s As String) As Boolean
        Return s.Length >= 3 AndAlso Not s.Contains("\\") AndAlso Not s.EndsWith(" ") AndAlso Not s.StartsWith(" ") AndAlso s.Substring(1, 2) = ":\" AndAlso s.EndsWith("\")
    End Function

    Function getValidFilePath(ByVal s As String, Optional ByVal ext As String = "") As String
        If s.Length >= 3 Then
            Dim d As DirectoryInfo
            d = New DirectoryInfo(s.Substring(0, 3))
            If d.Exists Then
                d = New DirectoryInfo(s.Substring(0, s.LastIndexOf("\")))
                If d.Exists Then
                    Dim f As New FileInfo(s)
                    If f.Exists Then
                        If ext = "" OrElse f.Extension = ext Then
                            Return f.FullName
                        End If
                    End If
                End If
            End If
        End If
        Return ""
    End Function


    Function isProcessAlive(name As String) As Boolean
        For Each p As Process In Process.GetProcessesByName(name)
            Return True
        Next
        Return False
    End Function



    Function getAudioFiles(path As String) As List(Of String)
        Dim res As New List(Of String)
        If IO.Directory.Exists(path) Then
            For Each fil As String In My.Computer.FileSystem.GetFiles(path)
                If dll.hasAudioExt(fil) Then
                    res.Add(fil)
                End If
            Next
        End If
        Return res
    End Function

    Sub setSoundDevice(ByVal dev As String)
        Try
            Shell(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\Utils\SoundVolumeView\SoundVolumeView.exe /SwitchDefault " & dev & " 0")
            keydelay(250)
        Catch ex As Exception
            keyt.Stop()
            MsgBox(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\Utils\SoundVolumeView\SoundVolumeView.exe not found." & vbNewLine & "Please install manually to that location.")
            keydelay()
        End Try
    End Sub

#Region "File Dialogs"
    Public Function getExactFileDialog(name As String, ext As String, Optional ByVal initDir As String = "") As String
        Dim op As New OpenFileDialog
        op.Multiselect = False
        If Not initDir = "" Then
            Try
                Do
                    initDir = initDir.Substring(0, initDir.LastIndexOf("\"))
                Loop Until initDir.Count(Function(c) c = "\") <= 1 Or IO.Directory.Exists(initDir)
                op.InitialDirectory = initDir
            Catch ex As Exception
            End Try
        End If
        op.Filter = "(" & name & ext & ")|" & name & ext
        op.ShowDialog()

        If Not op.FileName = "" Then
            Return op.FileName
        End If
        Return ""
    End Function
    Public Function getFileDialog(Optional ByVal initDir As String = "", Optional ByVal ext As String = "") As String
        Dim op As New OpenFileDialog
        op.Multiselect = False
        If Not initDir = "" Then
            Try
                Do
                    initDir = initDir.Substring(0, initDir.LastIndexOf("\"))
                Loop Until initDir.Count(Function(c) c = "\") <= 1 Or IO.Directory.Exists(initDir)
                op.InitialDirectory = initDir
                ' op.FileName = def.Substring(def.LastIndexOf("\") + 1)
            Catch ex As Exception
            End Try
        End If
        If Not ext = "" Then op.Filter = "(*." & ext & ")|*." & ext
        op.ShowDialog()

        If Not op.FileName = "" Then
            Return op.FileName
        End If
        Return ""
    End Function
    Public Function getFilesDialog(Optional ByVal initDir As String = "", Optional ByVal ext As String = "") As String()
        Dim op As New OpenFileDialog
        op.Multiselect = True
        If Not initDir = "" Then
            Try
                op.InitialDirectory = initDir.Substring(0, initDir.LastIndexOf("\"))
            Catch ex As Exception
            End Try
        End If
        If Not ext = "" Then op.Filter = "(*." & ext & ")|*." & ext
        If op.ShowDialog() = DialogResult.Cancel Then Return Nothing
        Return op.FileNames
    End Function
    Public Function getAudioFilesDialog(Optional ByVal initDir As String = "") As String()
        Dim op As New OpenFileDialog
        op.Multiselect = True
        If Not initDir = "" Then
            Try
                op.InitialDirectory = initDir.Substring(0, initDir.LastIndexOf("\"))
            Catch ex As Exception
            End Try
        End If
        op.Filter = "Audio files|*.mp3;*.wav;*.m4a;*.flac;*.mp3;*.aac"

        If op.ShowDialog() = DialogResult.Cancel Then Return Nothing
        Return op.FileNames
    End Function

    Public Function getDirectoryDialog(Optional ByVal def As String = "") As String
        Dim op As New FolderBrowserDialog

        op.ShowNewFolderButton = True

        If Not def = "" Then
            Try
                op.SelectedPath = def
            Catch ex As Exception
            End Try
        End If
        op.ShowDialog()
        If Not op.SelectedPath = "" Then
            Return op.SelectedPath & IIf(op.SelectedPath.EndsWith("\"), "", "\")
        End If
        Return ""
    End Function

    <StructLayout(LayoutKind.Sequential)>
    Private Structure COPYDATASTRUCT
        Public dwData As IntPtr
        Public cbData As Integer
        Public lpData As String
    End Structure

    Declare Function GetWindow Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal uCmd As Integer) As IntPtr
    Declare Function FindWindow Lib "user32.dll" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr 'Int32
    Declare Function SendMessageHM Lib "user32.dll" Alias "SendMessageA" (ByVal hWnd As IntPtr, ByVal wMsg As Int32, ByVal wParam As Int32, ByVal lParam As StringBuilder) As Int32


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        SendMessageHM(Handle, &H401, 0, New StringBuilder("arc"))
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        If m.Msg = &H4A Then 'H400 user
            Dim cds As COPYDATASTRUCT = Marshal.PtrToStructure(m.LParam, GetType(COPYDATASTRUCT))
            Dim comm As String = cds.lpData.Substring(0, 2)
            Dim data As String = cds.lpData.Substring(2)
            If comm = "ms" Then
                If radioEnabled Then changeSourceMode(0)
                initSearch()
                keyExecute(Key.keyName.Restore_Window)
                tSearch.Text = data
            ElseIf comm = "cm" Then
                setSetting(SettingsIdentifier.CURSOR_MOVER_INCR, CInt(data))
                ' cursorMoverIncr = data
            End If
        Else
            MyBase.WndProc(m)
        End If
    End Sub




    Private Sub Form1_LocationChanged(sender As Object, e As EventArgs) Handles Me.LocationChanged
        If SettingsService.settingsInitialized Then
            saveWinPos()
        End If
    End Sub


    Private Sub Form1_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        If SettingsService.settingsInitialized Then
            saveWinSize()
        End If
    End Sub

    Sub saveWinPos()
        If WindowState = FormWindowState.Normal Then
            OptionsForm.labelWinPos.Text = "(" & Left & ", " & Top & ")"
            Dim rawPos As String = IIf(Left < -Width + 5, 0, Left) & ";" & IIf(Top < -20, 0, Top)
            SettingsService.saveSetting(SettingsIdentifier.WIN_POS, rawPos)
        ElseIf WindowState = FormWindowState.Maximized Then
            OptionsForm.labelWinPos.Text = "(0, 0)"
        End If
    End Sub
    Sub saveWinSize()
        If WindowState = FormWindowState.Normal Then
            OptionsForm.labelWinSize.Text = "(" & Width & ", " & Height & ")"
            Dim rawSize As String = IIf(Width < minWidth, minWidth, Width) & ";" & IIf(Height < minHeight, minHeight, Height)
            SettingsService.saveSetting(SettingsIdentifier.WIN_SIZE, rawSize)
        ElseIf WindowState = FormWindowState.Maximized Then
            SettingsService.saveSetting(SettingsIdentifier.WIN_MAX, True)
            OptionsForm.labelWinSize.Text = "(max, max)"
        End If
    End Sub

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        If saveWinPosSize Then
            loadWinPosSize()
        End If
    End Sub

    Private Sub menuStatisticsTracks_Click(sender As Object, e As EventArgs) Handles menuStatisticsTracks.Click
        openOverlay(eOverlayMode.STATS_TRACKS)
        StatsForm.updateMode(eOverlayMode.STATS_TRACKS)
    End Sub

    Private Sub menuStatisticsFolders_Click(sender As Object, e As EventArgs) Handles menuStatisticsFolders.Click
        openOverlay(eOverlayMode.STATS_FOLDERS)
        StatsForm.updateMode(eOverlayMode.STATS_FOLDERS)
    End Sub

    Private Sub menuStatisticsRadio_Click(sender As Object, e As EventArgs) Handles menuStatisticsRadio.Click
        openOverlay(eOverlayMode.STATS_RADIO)
        StatsForm.updateMode(eOverlayMode.STATS_RADIO)
    End Sub

    Private Sub menuLyrics_Click(sender As Object, e As EventArgs) Handles menuLyrics.Click
        Dim l As ListBox = getSelectedList()
        If Not radioEnabled AndAlso l IsNot Nothing AndAlso l.SelectedIndex > -1 AndAlso TypeOf l.SelectedItem Is Track Then
            Dim track As Track = l.SelectedItem
            openOverlay(eOverlayMode.LYRICS)
            LyricsForm.openLyrics(track)
        End If

    End Sub

    Sub playStateHandler()
        'playstate handler
        If wmp.playState = WMPLib.WMPPlayState.wmppsPlaying Then
            If trackLoop = LoopMode.YES Then
                If wmp.Ctlcontrols.currentPosition > loopVals(2) And wmp.Ctlcontrols.currentPosition < loopVals(1) Then
                    wmp.Ctlcontrols.currentPosition = loopVals(1)
                End If
                If loopVals(1) < loopVals(2) Then
                    If wmp.Ctlcontrols.currentPosition < loopVals(1) Then
                        wmp.Ctlcontrols.currentPosition = loopVals(1)
                    ElseIf wmp.Ctlcontrols.currentPosition > loopVals(2) Then
                        wmp.Ctlcontrols.currentPosition = loopVals(1)
                    End If
                    If currTrack IsNot Nothing AndAlso loopVals(2) > currTrack.length AndAlso currTrack.length > 0 Then
                        If Math.Abs(wmp.Ctlcontrols.currentPosition - currTrack.length) <= 0.25 Then
                            wmp.Ctlcontrols.currentPosition = loopVals(1)
                        End If
                    End If
                End If
            End If

            currTrack = Track.getTrack(wmp.URL)

            If currTrack IsNot Nothing Then
                currTrack.currPart = currTrack.getCurrentPart(wmp.Ctlcontrols.currentPosition)
            End If

            If firstPlayStart = FirstStartState.STARTING And Not radioEnabled Then
                If Not last = Nothing Then
                    Dim l As ListBox = getSelectedList()
                    If l.SelectedItem = last Then
                        Dim timeTemp As Double = SettingsService.loadSetting(SettingsIdentifier.LAST_TRACK_APPLY_TIME)

                        If timeTemp > 2 * 60 Or timeTemp > l.SelectedItem.length / 2 Then
                            wmp.Ctlcontrols.currentPosition = timeTemp
                        End If
                        SettingsService.saveSetting(SettingsIdentifier.LAST_TRACK_APPLY_TIME, 0.0)
                        SettingsService.saveSetting(SettingsIdentifier.LAST_TRACK_RECORDED_TIME, 0.0)
                    End If
                End If
                firstPlayStart = FirstStartState.STARTED
            End If

        ElseIf wmp.playState = WMPPlayState.wmppsReady And Not radioEnabled Then
            If Not wmp.URL = "" Then
                wmp.URL = ""
                Dim ind As Integer = getSelectedList().SelectedItem.removeFromPlayList()
                If currTrack IsNot Nothing Then
                    currTrack.selectPlaylist()
                Else
                    If playlist.Count > ind Then
                        playlist(ind).selectPlaylist()
                    End If
                End If
            Else
                currTrack = Nothing
            End If
        ElseIf wmp.playState = WMPLib.WMPPlayState.wmppsStopped Then

            resetLoop()
            If radioEnabled Then
                If l2.Items.Count > 0 Then
                    saveRadioTime()
                    wmpstart(l2.SelectedItem.name)
                End If
            ElseIf Not currTrack = Nothing Then 'e.g. local source

                labelStatsUpdate()
                If Not IsNothing(tv.SelectedNode) Then
                    Dim c As Integer = currTrack.count
                    If c > 0 Then
                        currTrack.count += 1
                        saveRawSetting(SettingsIdentifier.TRACKS_COUNT, currTrack.name, currTrack.count)
                    Else
                        saveRawSetting(SettingsIdentifier.TRACKS_COUNT, currTrack.name, loadRawSetting(SettingsIdentifier.TRACKS_COUNT, currTrack.name) + 1)
                    End If
                    currTrack.selectPlaylist()
                    Select Case playMode
                        Case PlayMode.STRAIGHT
                            l2.SelectedIndex += 1
                            wmpstart(l2.SelectedItem)
                            last = l2.SelectedItem
                        Case PlayMode.REPEAT
                            labelStatsUpdate()
                            currTrack.play()
                        Case PlayMode.RANDOM
                            playNextTrack()
                        Case Else
                    End Select
                Else
                    If playMode = PlayMode.REPEAT Then
                        wmpstartURL(wmp.URL)
                    ElseIf playMode = PlayMode.RANDOM Then
                        playNextTrack()
                    End If

                End If
            Else
                If playMode = PlayMode.REPEAT Then
                    wmpstartURL(wmp.URL)
                ElseIf playMode = PlayMode.RANDOM Then
                    playNextTrack()
                End If

            End If
        End If

    End Sub

#End Region

#End Region

    'Private Sub Button1_Click_3(sender As Object, e As EventArgs)
    '    Dim l As ListBox = getSelectedList()
    '    If Not radio AndAlso l IsNot Nothing AndAlso l.SelectedIndex > -1 AndAlso TypeOf l.SelectedItem Is Track Then
    '        Dim track As Track = Track.getFirstTrack("DefQon 1 - 2009") 'l.SelectedItem
    '        openOverlay(eOverlayMode.PARTS)
    '        PartsForm.loadParts(track)
    '    End If
    '    Return
    '    Dim s As String = ""
    '    For i = 0 To l2.Items.Count - 1
    '        Dim track As Track = l2.Items(i)
    '        s &= track.name & vbNewLine
    '        track.updateParts()
    '        If track.partsCount > 1 Then
    '            For Each p As TrackPart In track.parts
    '                If p.name = "" Then
    '                    s &= " →ID - ID" & vbNewLine
    '                Else
    '                    s &= " →" & p.name & vbNewLine
    '                End If

    '            Next
    '        End If
    '    Next
    '    Dim sw As New StreamWriter("C:\users\marvin\desktop\hardcore.txt")
    '    sw.Write(s)
    '    sw.Close()
    '    Return
    '    For i = 0 To l2.Items.Count - 1
    '        Dim track As Track = l2.Items(i)
    '        track.updateGenre()
    '        Dim fols As List(Of Folder) = track.genre.folders
    '        For Each f As Folder In fols

    '            Dim currFol As Folder = Folder.getFolder(Folder.top.fullPath & f.name)
    '            If currFol IsNot Nothing Then
    '                If Not currFol.containsTrack(track) Then
    '                    MsgBox(currFol.name & vbNewLine & track.name)
    '                End If
    '            End If

    '        Next
    '    Next
    '    Return
    '    Dim fol As Folder = Folder.getSelectedFolder(tv)
    '    showOptions(OptionsForm.optionState.PLAYLISTS, False, "Manage " & fol.name, IIf(fol.isVirtual, {fol.nodePath}, {"folders", fol.nodePath}))
    '    Return

    'End Sub



    Private Sub menuStrip_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles menuStrip.ItemClicked

    End Sub

    Private Sub con2AddToPlaylist_Click(sender As Object, e As EventArgs) Handles con2AddToPlaylist.Click

    End Sub

    Private Sub iconTray_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles iconTray.MouseDoubleClick
        Me.Show()
        ShowInTaskbar = True
        Me.WindowState = FormWindowState.Normal
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        IniService.iniGetAllValues("kk")
    End Sub







    'Private Async Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
    '    Dim conf = SpotifyClientConfig.CreateDefault()
    '    Dim req = New ClientCredentialsRequest("dbf3700dbc0e45d8919ab0b5caa3d283", "a832a46ed505425ebf67e094b98e0722")
    '    Dim res = Await New OAuthClient(conf).RequestToken(req)
    '    Dim sp As New SpotifyClient(conf.WithToken(res.AccessToken))
    '    Dim track = Await sp.Tracks.Get("1UymZaSfQII4vUkz4U5btm")
    '    MsgBox(track.Name)
    '    Dim auth = New TokenAuthenticator(res.AccessToken, "Bearer")
    '    Dim authCred = New CredentialsAuthenticator("dbf3700dbc0e45d8919ab0b5caa3d283", "a832a46ed505425ebf67e094b98e0722")
    '    Dim conn = New SpotifyAPI.Web.Http.APIConnector(New Uri("https://api.spotify.com"), authCred)
    '    Dim pl = New PlayerClient(conn)
    '    Dim b = Await pl.GetAvailableDevices()
    'End Sub


    'Private Sub Button3_Click_2(sender As Object, e As EventArgs)

    '    indicateSortMode()
    '    Return
    '    Dim alls As New List(Of Track)
    '    For Each g As String In genres
    '        Dim f As Folder = Folder.getFolder("C:\users\marvin\music\" & g & "\")
    '        Dim ts As List(Of Track) = f.tracks
    '        For Each t As Track In ts
    '            If alls.Contains(t) Then
    '                MsgBox("alls " & t.name)
    '            End If
    '            alls.Add(t)
    '            For Each g2 As String In genres
    '                If g2 <> g Then
    '                    If IO.File.Exists("C:\users\marvin\music\" & g2 & "\" & t.name & t.ext) Then
    '                        MsgBox("other genre Not " & t.fullPath)
    '                    End If
    '                    If Not IO.File.Exists("C:\users\marvin\music\everything\" & t.name & t.ext) Then
    '                        MsgBox("everything not " & t.fullPath)
    '                    End If
    '                End If
    '            Next

    '        Next
    '    Next
    'End Sub

End Class

