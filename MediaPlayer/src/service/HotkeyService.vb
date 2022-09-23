Imports MediaPlayer.PlayerEnums

Public Class HotkeyService

    ' If hotkey for locking the form is pressed during global timer polling, the event should not be raised in the dedicated hotkey timer polling
    Public Shared lockChange As Boolean = False
    Public Shared Sub keyPressHandler()
        If formLocked Or (keylogger And Not KeyloggerModule.allowHotkeys) Then
            Exit Sub
        End If

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
    Public Shared Sub keyExecute(kNum As Key.keyName)
        keyExecute(Key.keyList(kNum))
    End Sub

    Public Shared Sub globalKeyPressHandler()
        If Not keylogger Or KeyloggerModule.allowHotkeys Then
            If Key.keyList(Key.keyName.Hotkey_Toggle).pressed Then
                If Not optionsMode Then
                    Form1.lockFormSwitch()
                    startHotkeyDelay(200)
                End If
            End If
        End If
    End Sub

    Public Shared Sub startHotkeyDelay(Optional ms As Integer = 0)
        Form1.keydelayt.Interval = IIf(ms = 0, hotkeyDelayMs, ms)
        Form1.keydelayt.Start()
        Form1.keyt.Stop()
    End Sub

    Public Shared Sub stopHotkeyDelayTimer()
        Form1.keydelayt.Stop()
        Form1.keydelayt.Interval = hotkeyDelayMs
        If Not formLocked Then Form1.keyt.Enabled = True
    End Sub

    Public Shared Sub keyExecute(ByVal k As Key)
        Select Case k.name
            Case Key.keyName.Play_Pause
                If Player.getPlayState() = WMPLib.WMPPlayState.wmppsPlaying Then
                    If radioEnabled Then
                        Player.setMute(Not Player.isMute())
                    Else
                        Player.pause()
                    End If
                ElseIf Player.getPlayState() = WMPLib.WMPPlayState.wmppsPaused Then
                    If Not radioEnabled Then
                        Player.play()
                    Else
                        Form1.saveRadioTime()
                        Form1.tv.Enabled = False
                        Form1.l2_2.Enabled = False
                        PlayerInterface.launchRadio(Form1.l2.SelectedItem)
                    End If
                ElseIf Player.getPlayState() = WMPLib.WMPPlayState.wmppsUndefined Then
                    If Not radioEnabled Then
                        If Form1.l2.SelectedIndex = -1 Then
                            Form1.setlistselected()
                            PlayerInterface.playlist(Form1.l2_2.SelectedIndex).play()
                        Else
                            Form1.l2.SelectedItem.play()
                        End If
                    Else
                        Form1.saveRadioTime()
                        PlayerInterface.launchRadio(Form1.l2.SelectedItem)
                    End If
                Else

                End If
                startHotkeyDelay()

            Case Key.keyName.Next_Track
2:              If Not radioEnabled Then
                    PlayerInterface.playNextTrack()
                    startHotkeyDelay()
                Else
                    If Not Form1.l2.SelectedIndex = Form1.l2.Items.Count - 1 Then
                        Form1.saveRadioTime()
                        Form1.l2.SelectedIndex += 1
                        PlayerInterface.launchRadio(Form1.l2.SelectedItem)
                        startHotkeyDelay()
                    End If
                End If
                Exit Sub
            Case Key.keyName.Previous_Track
3:              If Not radioEnabled Then
                    PlayerInterface.playPrevTrack()
                    startHotkeyDelay()
                Else
                    If Not Form1.l2.SelectedIndex = 0 Then
                        Form1.saveRadioTime()
                        Form1.l2.SelectedIndex -= 1
                        PlayerInterface.launchRadio(Form1.l2.SelectedItem)
                        startHotkeyDelay()
                    End If
                End If
            Case Key.keyName.Volume_Mute
                Player.setMute(Not Player.isMute())
                startHotkeyDelay()
            Case Key.keyName.Volume_Min
                Player.setVolume(1)
                startHotkeyDelay(100)
            Case Key.keyName.Volume_Half
                Player.setVolume(1)
                startHotkeyDelay(50)
            Case Key.keyName.Volume_Max
                Player.setVolume(100)
                startHotkeyDelay(100)
            Case Key.keyName.Volume_Down
                If Player.getVolume() > 0 Then
                    If Player.getVolume() < 51 Then
                        Player.decreaseVolume(Int(Player.getVolume() / 5) + 1)
                    ElseIf Player.getVolume() < 76 Then
                        Player.setVolume(50)
                    Else
                        Player.setVolume(75)
                    End If
                    startHotkeyDelay(50)
                End If
            Case Key.keyName.Volume_Up
                If Player.getVolume() < 100 Then
                    If Player.getVolume() < 50 Then
                        Player.increaseVolume(Int(Player.getVolume() / 5) + 1)
                    ElseIf Player.getVolume() < 75 Then
                        Player.setVolume(75)
                    Else
                        Player.setVolume(100)

                    End If
                    startHotkeyDelay(50)
                End If
            Case Key.keyName.Fast_Forward
                If Player.getPlayState() = WMPLib.WMPPlayState.wmppsPlaying Then
                    Try
                        Player.increaseCurrentPosition(5)
                    Catch ex As Exception
                    End Try
                End If
            Case Key.keyName.Slow_Forward
                If Player.getPlayState() = WMPLib.WMPPlayState.wmppsPlaying Then
                    Try
                        Player.increaseCurrentPosition(1)
                    Catch ex As Exception
                    End Try
                End If
            Case Key.keyName.Fast_Rewind
                If Player.getPlayState() = WMPLib.WMPPlayState.wmppsPlaying Then
                    Try
                        Player.decreaseCurrentPosition(5)
                    Catch ex As Exception
                    End Try
                End If
            Case Key.keyName.Slow_Rewind
                If Player.getPlayState() = WMPLib.WMPPlayState.wmppsPlaying Then
                    Try
                        Player.decreaseCurrentPosition(1)
                    Catch ex As Exception
                    End Try
                End If
            Case Key.keyName.Repeat_Mode
                Form1.changePlayMode(PlayMode.REPEAT)
            Case Key.keyName.Random_Mode
                Form1.changePlayMode(PlayMode.RANDOM)
            Case Key.keyName.Source_Local
                PlayerInterface.changeSourceMode(MusicSource.LOCAL)
            Case Key.keyName.Source_Radio
                PlayerInterface.changeSourceMode(MusicSource.RADIO)
            Case Key.keyName.Tree_Up
                If Not radioEnabled Then
                    If Not IsNothing(Form1.tv.SelectedNode.PrevNode) Or Not IsNothing(Form1.tv.SelectedNode.Parent) Then
                        If Not IsNothing(Form1.tv.SelectedNode.PrevNode) Then
                            Form1.tv.SelectedNode = Form1.tv.SelectedNode.PrevNode
                        ElseIf Not IsNothing(Form1.tv.SelectedNode.Parent) Then
                            Form1.tv.SelectedNode = Form1.tv.SelectedNode.Parent
                        End If
                        Form1.tv_AfterSelectSUB()
                        startHotkeyDelay()
                    End If
                End If
            Case Key.keyName.Tree_Down
                If Not radioEnabled Then
                    If Not IsNothing(Form1.tv.SelectedNode.NextNode) Then
                        Form1.tv.SelectedNode = Form1.tv.SelectedNode.NextNode
                    Else
                        If Form1.tv.SelectedNode.Nodes.Count > 0 Then
                            Form1.tv.SelectedNode = Form1.tv.SelectedNode.Nodes(0)
                        End If
                    End If
                    Form1.tv_AfterSelectSUB()
                    startHotkeyDelay()
                End If
            Case Key.keyName.Track_ToQueue
                'File.Copy(l2_2.SelectedItem.fullpath, "C:\users\marvin\music\Chillen\" & l2_2.SelectedItem.name & ".mp3")
                'l2_2.SelectedIndex += 1
                'launchTrack(l2_2.Items(l2_2.SelectedIndex))
                'startHotkeyDelay()
                'Exit Sub
                Form1.TrackToQueue()
            Case Key.keyName.Track_PlayNext
                Dim l As ListBox = Form1.getSelectedList()
                Dim selTrack As Track = l.SelectedItem
                selTrack.playNext()
                If l Is Form1.l2 Then
                    If removeNextTrack Then
                        Form1.l2.Items.Remove(selTrack)
                        Form1.l2.SelectedIndex = -1
                    End If
                End If
                startHotkeyDelay()
            Case Key.keyName.Track_Remove
                If Form1.removeItem(True) Then
                    startHotkeyDelay()
                End If
            Case Key.keyName.Track_Delete
                If Form1.deleteTrack(Form1.getSelectedTrack(), False) Then
                    startHotkeyDelay()
                End If
            Case Key.keyName.Track_Loop
                If Not radioEnabled Then
                    If Player.getCurrentPosition() >= 0 Then
                        If PlayerInterface.trackLoop = LoopMode.NO Then
                            PlayerInterface.trackLoop = LoopMode.INTERMEDIATE
                            Form1.labelLoop.Cursor = Cursors.Hand
                            PlayerInterface.loopVals(1) = Player.getCurrentPosition()
                            Form1.labelStatsUpdate()
                            startHotkeyDelay(200)
                        ElseIf PlayerInterface.trackLoop = LoopMode.INTERMEDIATE Then
                            PlayerInterface.trackLoop = LoopMode.YES
                            Form1.labelLoop.Cursor = Cursors.Hand
                            PlayerInterface.loopVals(2) = Player.getCurrentPosition()
                            startHotkeyDelay()
                            startHotkeyDelay(150)
                        ElseIf PlayerInterface.trackLoop = LoopMode.YES Then
                            PlayerInterface.resetLoop()
                            startHotkeyDelay(100)
                        End If
                    End If
                End If
            Case Key.keyName.Search
                If Not radioEnabled Then
                    If Not Form1.ContainsFocus Then
                        Key.keyList.Item(Key.keyName.Restore_Window).execute()
                    End If
                    Form1.tSearch.Focus()
                    Form1.initSearch()
                    startHotkeyDelay()
                End If
            Case Key.keyName.Next_Part
                PlayerInterface.switchpart(2)
                startHotkeyDelay()
            Case Key.keyName.Previous_Part
                PlayerInterface.switchpart(1)
                startHotkeyDelay()
            Case Key.keyName.Count_Sub
                Dim l As ListBox = Form1.getSelectedList()
                If l IsNot Nothing Then
                    saveRawSetting(SettingsIdentifier.TRACKS_COUNT, l.SelectedItem, l.SelectedItem.count - 1)
                    Form1.labelStatsUpdate(l)
                End If
                startHotkeyDelay(150)
            Case Key.keyName.Count_Add
                '    wmp.Ctlcontrols.pause() : Dim SAPI : SAPI = CreateObject("SAPI.spvoice") 
                ': SAPI.speak(l2.SelectedItem) : wmp.Ctlcontrols.play()
                Dim l As ListBox = Form1.getSelectedList()
                If l IsNot Nothing Then
                    Dim c As Integer = l.SelectedItem.count
                    If c = 0 Then
                        saveRawSetting(SettingsIdentifier.TRACKS_COUNT, l.SelectedItem, loadRawSetting(SettingsIdentifier.TRACKS_COUNT, l.SelectedItem.name) + 1)
                    Else
                        saveRawSetting(SettingsIdentifier.TRACKS_COUNT, l.SelectedItem, l.SelectedItem.count + 1)
                        Form1.labelStatsUpdate(l)
                    End If
                End If
                startHotkeyDelay(150)
            Case Key.keyName.Clicker_On
                If autoClicker Then Form1.clickerTimer.Start()
            Case Key.keyName.Clicker_Off
                'handled in alltime
            Case Key.keyName.Cursor_Up
                If cursorMover Then
                    Cursor.Position = New Point(Cursor.Position.X, Cursor.Position.Y - cursorMoverIncr)
                    startHotkeyDelay(cursorMoverDelay)
                End If
            Case Key.keyName.Cursor_Right
                If cursorMover Then
                    Cursor.Position = New Point(Cursor.Position.X + cursorMoverIncr, Cursor.Position.Y)
                    startHotkeyDelay(cursorMoverDelay)
                End If

            Case Key.keyName.Cursor_Down
                If cursorMover Then
                    Cursor.Position = New Point(Cursor.Position.X, Cursor.Position.Y + cursorMoverIncr)
                    startHotkeyDelay(cursorMoverDelay)
                End If
            Case Key.keyName.Cursor_Left
                If cursorMover Then
                    Cursor.Position = New Point(Cursor.Position.X - cursorMoverIncr, Cursor.Position.Y)
                    startHotkeyDelay(cursorMoverDelay)
                End If
            Case Key.keyName.Restore_Window
                Utils.SwitchTo(Process.GetCurrentProcess.MainWindowHandle)
        End Select
    End Sub

End Class
