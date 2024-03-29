﻿
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


    Dim dItem As Track
    Dim dragList As ListBox


    Public root As String 'TODO needed?


    Public searchState As SearchState


    Public overlayMode As eOverlayMode
    Public lastOptionsState As OptionsForm.optionState


    Public lastGadgetsState As GadgetsForm.GadgetState

    Public WithEvents dragDropNextField As Button
    Public WithEvents dragDropQueueField As Button

    Public datesInitiallyLoaded As Boolean = False

#End Region


#Region "Form1"

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        SettingsService.saveSetting(SettingsIdentifier.VOLUME, Player.getVolume())
        SettingsService.saveSetting(SettingsIdentifier.PLAY_MODE, playMode)
        SettingsService.saveSetting(SettingsIdentifier.MUSIC_SOURCE, CInt(radioEnabled))
        SettingsService.saveSetting(SettingsIdentifier.WIN_MAX, WindowState = FormWindowState.Maximized)

        If keylogger Then
            KeyloggerModule.keyloggerDestroy()
        End If

        If l2.SelectedIndex > -1 And tv.SelectedNode IsNot Nothing And Not radioEnabled And Not PlayerInterface.last = Nothing Then
            saveLastTrack()
        End If

        If SettingsService.getSetting(SettingsIdentifier.PLAYLIST_SAVE_HISTORY) Then
            saveCurrPlaylistHistory()
        End If

        If radioEnabled Then saveRadioTime()

        TcpRemoteControl.stopAllConnections("close", False)
    End Sub

    Private Sub Form1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F8 Then e.Handled = True
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        VersionUpdateService.checkForVersionUpdate()

        SettingsService.initSystemSettings()

        Folder.setTopFolder(getSetting(SettingsIdentifier.PATH))
        Folder.invalidateFolders(Folder.top)
        Track.invalidateTracks(True)

        PlayerInterface.initPlayer(Me)
        SettingsService.initPlayerSettings()


        Key.initKeys()

        FileSystemWatcher.fswInit()


        loadWinPosSize()
        FormUtils.colorForm(Me)


        AutoStarts.executeAutoStarts()
        KeyloggerModule.keyloggerInit()
        GadgetsForm.initMacrosTable()

        updatePlayMode()

        startTimers()

        If My.Application.CommandLineArgs.Count = 0 Then

            If radioEnabled Then
                PlayerInterface.changeSourceMode(MusicSource.RADIO)
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
                Dim prioTrack As Track = IIf(PlayerInterface.currTrack = Nothing, PlayerInterface.last, PlayerInterface.currTrack)
                If Not prioTrack = Nothing Then
                    prioTrack.selectPlaylist()
                End If
                IniService.iniDeleteSection(IniSection.HISTORY)
            End If
            If Not PlayerInterface.last = Nothing Then
                Dim l As ListBox = getSelectedList()
                If l.SelectedItem IsNot Nothing AndAlso l.SelectedItem.name = PlayerInterface.last.name Then
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
                PlayerInterface.playlist(0).play()
            Else
                setSetting(SettingsIdentifier.MUSIC_SOURCE, MusicSource.LOCAL)
                localfill()
                initSearch()
                tSearch.Text = para
            End If
        End If

    End Sub


    Sub startTimers()
        alltime.Start()
        keyt.Start()
        clickcountt.Start()
        radiotimer.Start()
        iniValT.Start()
    End Sub

    Sub loadWinPosSize()
        MinimumSize = New Size(minWidth, minHeight)

        If Not saveWinPosSize Then
            formResize()
            Return
        End If

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
        HotkeyService.keyPressHandler()
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
                                HotkeyService.startHotkeyDelay()
                            End If
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub keydelayt_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles keydelayt.Tick
        HotkeyService.stopHotkeyDelayTimer()
    End Sub

    Private Sub iniValT_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles iniValT.Tick
        If Player.getPlayState() = WMPLib.WMPPlayState.wmppsPlaying Then
            If Not radioEnabled Then
                If PlayerInterface.currTrack IsNot Nothing Then PlayerInterface.currTrack.currPart = PlayerInterface.currTrack.getCurrentPart(Player.getCurrentPosition())
                If Not PlayerInterface.last = Nothing Then saveLastTrack()

                If Not PlayerInterface.currTrack = Nothing Then
                    SettingsService.saveSetting(SettingsIdentifier.LAST_TRACK_RECORDED_TIME, Player.getCurrentPosition())
                    If Player.getCurrentMedia() IsNot Nothing Then
                        Try
                            If Player.getCurrentMedia().duration > 0 Then
                                saveRawSetting(SettingsIdentifier.TRACKS_TIME, PlayerInterface.currTrack.name, Player.getCurrentMedia().duration)
                            End If
                            labelStatsUpdate()
                        Catch ex As Exception

                        End Try

                    End If
                End If
            End If
            SettingsService.saveSetting(SettingsIdentifier.VOLUME, Player.getVolume())
            SettingsService.saveSetting(SettingsIdentifier.PLAY_MODE, playMode)
        End If
    End Sub

    Private Sub alltime_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles alltime.Tick
        updateUI()

        TcpRemoteControl.dispatchTcpMessages()

        HotkeyService.globalKeyPressHandler()

        macroKeyPressHandler()

        ClickGadget.clickGadgetHandler()

        PlayerInterface.playStateHandler()

    End Sub

    Private Sub radiotimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radiotimer.Tick
        If radioEnabled AndAlso Player.getPlayState() = WMPLib.WMPPlayState.wmppsPlaying Then
            If Not Player.getPlayState() = WMPLib.WMPPlayState.wmppsTransitioning Then
                If l2.SelectedItem IsNot Nothing Then l2.SelectedItem.timetemp = Player.getCurrentPosition()
            End If
        End If
    End Sub


    Private Sub clicker_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clickerTimer.Tick
        ClickGadget.performAutoClick()
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


#Region "Form1"
    Public Sub setLockImage()
        If formLocked Then
            menuLock.Image = IIf(darkTheme, My.Resources.unlock_inv, My.Resources.unlock)
            menuLock.ToolTipText = "Unlock Hotkeys"
        Else
            menuLock.Image = IIf(darkTheme, My.Resources.lock_inv, My.Resources.lock)
            menuLock.ToolTipText = "Lock Hotkeys"
        End If
    End Sub
    Public Sub setRemoteImage()
        If TcpRemoteControl.remoteTcp.isEstablished Then
            menuRemote.Image = IIf(darkTheme, My.Resources.online_inv, My.Resources.online)
            menuRemote.ToolTipText = "Status: Connected"
        Else
            If TcpRemoteControl.remoteTcp.isListenerActive Then
                menuRemote.Image = IIf(darkTheme, My.Resources.offline_inv, My.Resources.offline)
                menuRemote.ToolTipText = "Status: Ready"
            Else
                menuRemote.Image = IIf(darkTheme, My.Resources.blocked_inv, My.Resources.blocked)
                menuRemote.ToolTipText = "Status: Blocked"
            End If

        End If
    End Sub
    Public Sub setLyricsImage()
        Dim l As ListBox = getSelectedList()
        If radioEnabled Or l Is Nothing OrElse l.SelectedIndex = -1 OrElse TypeOf l.SelectedItem IsNot Track Then
            menuLyrics.Image = IIf(darkTheme, My.Resources.cross_inv, My.Resources.cross)
        Else
            Dim track As Track = l.SelectedItem
            If LyricsForm.hasLyrics(track) Then
                menuLyrics.Image = IIf(darkTheme, My.Resources.tick_inv, My.Resources.tick)
            Else
                menuLyrics.Image = IIf(darkTheme, My.Resources.cross_inv, My.Resources.cross)
            End If
        End If
    End Sub
    Public Sub setSettingsImage()
        menuSettings.Image = IIf(darkTheme, My.Resources.settings_inv, My.Resources.settings)
    End Sub

    Public Sub setGadgetsImage()
        menuGadgets.Image = IIf(darkTheme, My.Resources.gadgets_inv, My.Resources.gadgets)
    End Sub

    Public Sub setMenuIcons()
        setLockImage()
        setRemoteImage()
        setSettingsImage()
        setLyricsImage()
        setGadgetsImage()
    End Sub
#End Region



#Region "Local Source/Playlist"
    Private Sub ImportPlaylistToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuSourceExternalMedia.Click
        If Not radioEnabled Then
            If Not searchState = SearchState.NONE Then cancelSearch()
            Dim selectedFiles() As String = OperatingSystem.getFilesDialog()
            If selectedFiles IsNot Nothing AndAlso selectedFiles.Length > 0 Then
                Dim addFolder As Folder = New Folder(selectedFiles(0).Substring(0, selectedFiles(0).LastIndexOf("\") + 1))
                addFolder.addFolder(selectedFiles)
                l2.SelectedIndex = -1
            Else
                Dim a As String = InputBox("Type in Folder to import", , My.Computer.FileSystem.SpecialDirectories.MyMusic & "\")
                If Not a = "" Then
                    If Directory.Exists(a) Then
                        Dim n As Integer = 0
                        For Each fil As String In My.Computer.FileSystem.GetFiles(a)
                            If Utils.hasAudioExt(fil) Then
                                Track.getTrack(fil).addToPlaylist()
                                n += 1
                            End If
                        Next
                        If n = 0 Then
                            MsgBox("No Audio files found", MsgBoxStyle.Exclamation)
                        Else
                            If Not PlayerInterface.currTrack = Nothing Then
                                PlayerInterface.currTrack.play()
                            Else
                                PlayerInterface.playlist(0).play()
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
        PlayerInterface.switchSourceMode()
    End Sub
#End Region

#Region "Settings"
    Sub lockFormSwitch()
        setSetting(SettingsIdentifier.FORM_LOCKED, Not getSetting(SettingsIdentifier.FORM_LOCKED))
        FormUtils.colorForm(Me)
        setLockImage()
        If formLocked Then
            keyt.Stop()
            HotkeyService.lockChange = True
        Else
            HotkeyService.startHotkeyDelay()
        End If
    End Sub



    Private Sub menuIcons_MouseHover(sender As Object, e As EventArgs) Handles menuLock.MouseHover, menuRemote.MouseHover, menuSettings.MouseHover, menuGadgets.MouseHover
        ttShow(sender.ToolTipText, menuStrip, Cursor.Position.X + 10 - menuStrip.Left - Left, Cursor.Position.Y - 28 - menuStrip.Top - Top, 1000)
    End Sub


    Private Sub menuLock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuLock.Click
        If Not optionsMode Then lockFormSwitch()
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
                If Not PlayerInterface.last = Nothing AndAlso listContains(l2, PlayerInterface.last) >= 0 Then
                    l2.SelectedIndex = listContains(l2, PlayerInterface.last)
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
            Dim prioTrack As Track = IIf(PlayerInterface.currTrack = Nothing, PlayerInterface.last, PlayerInterface.currTrack)
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

                If PlayerInterface.last IsNot Nothing Then saveLastTrack()
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
                    If Player.getUrl() = dItem.fullPath Then Player.resetUrl()
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
                If Not PlayerInterface.currTrack = Nothing Then
                    PlayerInterface.currTrack.selectPlaylist()
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
        setLyricsImage()
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
                        PlayerInterface.trackLoop = LoopMode.YES
                        PlayerInterface.loopVals(1) = part.fromSec
                        PlayerInterface.loopVals(2) = part.toSec
                    End If
                Else
                    l2.SelectedItem.addToPlaylist()
                    l2.SelectedItem.play()
                End If
            Else
                saveRadioTime()
                PlayerInterface.launchRadio(l2.SelectedItem)
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
            PlayerInterface.last = l2.SelectedItem
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

                If Not l2.SelectedIndex = -1 And Not Player.getPlayState() = WMPLib.WMPPlayState.wmppsPlaying And PlayerInterface.last = Nothing Then
                    PlayerInterface.last = l2.SelectedItem
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

        PlayerInterface.last = Track.getTrack(SettingsService.loadSetting(SettingsIdentifier.LAST_TRACK_FILE))

        If tv.Nodes(0).Nodes.Count > 0 Then
            Dim prioTrack As Track = IIf(PlayerInterface.currTrack = Nothing, PlayerInterface.last, PlayerInterface.currTrack)
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
                Dim prioTrack As Track = IIf(PlayerInterface.currTrack = Nothing, PlayerInterface.last, PlayerInterface.currTrack)
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
        Dim rads As List(Of Radio) = Radio.getStations()
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
                If PlayerInterface.currTrack Is Nothing OrElse Not t.name.ToLower = PlayerInterface.currTrack.name.ToLower Then t.addToPlaylist()
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
                            If PlayerInterface.currTrack IsNot Nothing AndAlso selTrack.name.ToLower = PlayerInterface.currTrack.name.ToLower Then
                                Player.resetUrl()
                                Player.pause()
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
                If Not PlayerInterface.last = Nothing Then
                    If listContains(ol, PlayerInterface.last) >= 0 Then
                        ol.SelectedIndex = listContains(ol, PlayerInterface.last)
                    Else
                        If ol.Items.Count > 0 Then
                            ol.SelectedIndex = rnd.Next(0, ol.Items.Count)
                        End If
                    End If
                End If
            Else
                PlayerInterface.clearPlaylist()
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
                        If Player.getUrl() = track.fullPath Then
                            Player.resetUrl()
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
                            If Player.getUrl().ToLower = CStr(locPath & str.name & str.ext).ToLower And (Player.getPlayState() = WMPLib.WMPPlayState.wmppsPlaying Or Player.getPlayState() = WMPLib.WMPPlayState.wmppsPaused) Then
                                PlayerInterface.launchURL(root & locs(i).nodePath & newName & str.ext)
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

    Public Sub TrackToQueue() 'handles toqueue command
        If Not radioEnabled Then
            Dim l As ListBox = getSelectedList()
            If Not TypeOf l.SelectedItem Is Track Then Return
            Dim selTrack As Track = l.SelectedItem
            If Not PlayerInterface.currTrack = Nothing Then
                PlayerInterface.currTrack.selectPlaylist()
            Else
                PlayerInterface.playlist(0).selectPlaylist()
            End If
            selTrack.addToPlaylist()
            If l Is l2 Then
                If removeNextTrack Then
                    l2.Items.Remove(selTrack)
                    l2.SelectedIndex = -1
                End If
            End If
            HotkeyService.startHotkeyDelay()
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

        If PlayerInterface.currTrack = Nothing AndAlso l2.SelectedIndex = -1 AndAlso l2_2.SelectedIndex = -1 And dragList Is Nothing Then
            If Track.playlist IsNot Nothing Then
                If Track.playlist.Count > 0 Then
                    Track.playlist(0).selectPlaylist()
                End If
            End If
        End If
    End Sub

    Sub labelUIUpdate()
        labelVolume.Text = Player.getVolume()
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
                If Not radioEnabled And Not Player.isUrlEmpty() Then setlistselected()
            End If
        End If
    End Sub


    Sub windowTextUpdate()
        If Not radioEnabled And Not PlayerInterface.currTrack Is Nothing Then
            Dim t As String = PlayerInterface.currTrack.name
            If PlayerInterface.currTrack.partsCount > 1 Then
                If PlayerInterface.currTrackPart IsNot Nothing Then
                    Dim pString As String = PlayerInterface.currTrackPart.name
                    t &= " | " & IIf(pString = "", "", pString & " | ") & PlayerInterface.currTrackPart.id + 1 & "(" & PlayerInterface.currTrack.partsCount & ")"
                End If
            End If
            If Player.getPlayState() = WMPPlayState.wmppsPlaying Then
                t = "♫ " & t '23.02.18 →
            ElseIf Player.getPlayState() = WMPPlayState.wmppsPaused Or Player.getPlayState() = WMPPlayState.wmppsStopped Then
                t = "■ ⁯⁯⁯⁯⁯⁯" & t
            End If
            Me.Text = t
        ElseIf radioEnabled Then
            If l2.SelectedItem IsNot Nothing Then Me.Text = IIf(Player.getPlayState() = WMPPlayState.wmppsPlaying, "♫ ", "■ ") & l2.SelectedItem.name
        ElseIf Player.isUrlEmpty() Then
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
            If Not PlayerInterface.currTrack = Nothing AndAlso PlayerInterface.currTrack.name = selList.SelectedItem.name Then
                If PlayerInterface.currTrack.partsCount = 0 Then
                    PlayerInterface.currTrack.updateParts()
                End If
1:              If PlayerInterface.currTrack.partsCount > 0 AndAlso PlayerInterface.currTrackPart IsNot Nothing Then labelPartsCount.Text = PlayerInterface.currTrackPart.id + 1 & " (" & PlayerInterface.currTrack.partsCount & ")"
                Try
                    If PlayerInterface.currTrack.partsCount > 0 Then labelPartName.Text = PlayerInterface.currTrackPart.name
                    If PlayerInterface.trackLoop = LoopMode.NO Then
                        If PlayerInterface.currTrack.partsCount > 0 Then
                            labelLoop.Text = PlayerInterface.currTrackPart.format
                        Else
                            labelLoop.Text = ""
                        End If
                    ElseIf PlayerInterface.trackLoop = LoopMode.INTERMEDIATE Then
                        labelLoop.Text = "[" & dll.secondsTo_ms_Format(Int(PlayerInterface.loopVals(1))) & " -"
                    Else
                        labelLoop.Text = "[" & dll.secondsTo_ms_Format(Int(PlayerInterface.loopVals(1))) & " - " & dll.secondsTo_ms_Format(Int(PlayerInterface.loopVals(2))) & "]"
                    End If
                Catch ex As Exception
                    labelLoop.Text = "error"
                End Try
            ElseIf selList.SelectedItem.partsCount > 0 Then
                If PlayerInterface.currTrack <> Nothing And selList.SelectedItem IsNot Nothing Then
                    If PlayerInterface.currTrack.name = selList.SelectedItem.name Then
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
                If l2.Items(i).name = PlayerInterface.last.name Then
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
                If l2_2.Items(i).name = PlayerInterface.last.name Then
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
        If PlayerInterface.trackLoop = LoopMode.INTERMEDIATE Then
            val = InputBox("From:", , dll.secondsTo_ms_Format(Int(PlayerInterface.loopVals(1))))
            If val = "" Then
                PlayerInterface.resetLoop()
            Else : PlayerInterface.loopVals(1) = dll.minFormatToSec(val)
            End If
        ElseIf PlayerInterface.trackLoop = LoopMode.YES Then
            val = InputBox("From:", , dll.secondsTo_ms_Format(Int(PlayerInterface.loopVals(1))))
            If val = "" Then
                PlayerInterface.resetLoop() : Exit Sub
            Else : PlayerInterface.loopVals(1) = dll.minFormatToSec(val)
            End If
            val = InputBox("To:", , dll.secondsTo_ms_Format(Int(PlayerInterface.loopVals(2))))
            If val = "" Then
                PlayerInterface.resetLoop()
            Else : PlayerInterface.loopVals(2) = dll.minFormatToSec(val)
            End If
        End If
    End Sub

    Public Sub setLoop(fromTime As Integer, toTime As Integer)
        PlayerInterface.loopVals(1) = fromTime
        PlayerInterface.loopVals(2) = toTime
        PlayerInterface.trackLoop = LoopMode.YES
    End Sub

    Private Sub Label16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles labelPartName.Click
        If Not labelPartName.Text = "" Then My.Computer.Clipboard.SetText(labelPartName.Text)
    End Sub

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
        labelPrevTrack.Location = New Point(wmp.Left + 7, wmp.Bottom - 44)
        picRepeat.Location = New Point(wmp.Left + 34, wmp.Bottom - 29)
        picRandom.Location = New Point(wmp.Left + 75, wmp.Bottom - 31)
        labelNextTrack.Location = New Point(wmp.Right - 28, wmp.Bottom - 44)
        labelVolume.Location = New Point(wmp.Left + 200, wmp.Bottom - 13)
        labelPrevTrack.BringToFront()
        picRepeat.BringToFront()
        picRandom.BringToFront()
        labelNextTrack.BringToFront()
        labelVolume.BringToFront()

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

        If Me.WindowState = FormWindowState.Minimized Then
            If getSetting(SettingsIdentifier.WIN_MINIMIZE_TO_ICON_TRAY) Then
                iconTray.Visible = True
                Me.Hide()
            End If
        Else
            iconTray.Visible = False
        End If
    End Sub
    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        formResize()
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
                If Player.getPlayState() = WMPLib.WMPPlayState.wmppsPlaying Then
                    PlayerInterface.last = l2.SelectedItem
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
        SettingsService.saveSetting(SettingsIdentifier.LAST_TRACK_FILE, IIf(val = "", PlayerInterface.last.virtualPath, val))
    End Sub

    Public Sub saveCurrPlaylistHistory()
        For Each t As Track In PlayerInterface.playlist
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
1:          PlayerInterface.gldt.Clear()
            PlayerInterface.glnames.Clear()
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
                            PlayerInterface.gldt.Add(s.Substring(s.IndexOf("=") + 1, 10))
                            PlayerInterface.glnames.Add(s.Substring(s.IndexOf("=") + 16))
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
        If reloadDates Or PlayerInterface.gldt.Count = 0 Or PlayerInterface.glnames.Count = 0 Then
            If Not loaddates() Then
                'loading dates failed
            End If
        End If
        Dim res As String = PlayerInterface.glnames.IndexOf(track)
        If res = -1 Then
            Return ""
        Else
            Return PlayerInterface.gldt(res)
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


#Region "System Functions"







    Protected Overrides Sub WndProc(ByRef m As Message)
        If m.Msg = &H4A Then 'H400 user
            Dim cds As OperatingSystem.COPYDATASTRUCT = Marshal.PtrToStructure(m.LParam, GetType(OperatingSystem.COPYDATASTRUCT))
            Dim comm As String = cds.lpData.Substring(0, 2)
            Dim data As String = cds.lpData.Substring(2)
            If comm = "ms" Then
                If radioEnabled Then PlayerInterface.changeSourceMode(MusicSource.LOCAL)
                initSearch()
                HotkeyService.keyExecute(Key.keyName.Restore_Window)
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



#End Region


    Private Sub iconTray_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles iconTray.MouseDoubleClick
        Me.Show()
        ShowInTaskbar = True
        Me.WindowState = FormWindowState.Normal
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        '   MsgBox(IsValidFileNameOrPath("C:\oer\asder"))
    End Sub

End Class

