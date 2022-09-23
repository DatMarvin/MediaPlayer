Imports System.IO
Imports MediaPlayer.PlayerEnums
Imports WMPLib

Public Class PlayerInterface

    Private Shared formHandle As Form1

    Shared firstPlayStart As FirstStartState

    Public Shared currTrack As Track
    Public Shared last As Track
    Public Shared trackLoop As LoopMode
    Public Shared loopVals(2) As Double
    Public Shared gldt As New List(Of String)
    Public Shared glnames As New List(Of String)

    Shared dll As New Utils
    Public Shared ReadOnly Property currTrackPart As TrackPart
        Get
            Return currTrack.currPart
        End Get
    End Property


    Public Shared playlist As New List(Of Track)

    Public Shared currLyrTrack As Track

    Public Shared Sub initPlayer(formHandle As Form1)
        PlayerInterface.formHandle = formHandle
        Player.initPlayer(formHandle)
    End Sub




    Public Shared Sub launchTrack(track As Track)
        If Not radioEnabled And Not track = Nothing Then
            If File.Exists(track.fullPath) Then
                updatePlayRate()
                Player.setUrl(track.fullPath)
                last = track
                track.updateParts()
                firstStart()
            End If
            resetLoop()
        End If
    End Sub
    Public Shared Sub launchRadio(rad As Radio)
        If radioEnabled And Form1.searchState = SearchState.NONE And rad IsNot Nothing Then
            Player.setUrl(rad.url)
            firstStart()
        End If
    End Sub
    Public Shared Sub launchURL(url As String)
        If Form1.searchState = SearchState.NONE Then
            resetLoop()
            updatePlayRate()
            Player.setUrl(url)
            firstStart()
        End If
    End Sub

    Shared Sub firstStart()
        If firstPlayStart = FirstStartState.INIT Then
            firstPlayStart = FirstStartState.STARTING
            If SettingsService.loadSetting(SettingsIdentifier.FTP_AUTO_UPDATE) Then
                dll.checkPlayerUpdate(dll.ftpCred, True)
            End If
        End If
    End Sub

    Public Shared Sub updatePlayRate()
        updatePlayRate(playRate)
    End Sub

    Public Shared Sub updatePlayRate(val As Double)
        Try
            Player.setPlayRate(val)
        Catch ex As Exception
            val = 1.0
        End Try
        If optionsMode Then OptionsForm.labelPlayRate.Text = "Play Rate: " & val
        saveSetting(SettingsIdentifier.PLAY_RATE, val)
    End Sub

    Public Shared Sub setBalance(val As Integer)
        Try
            Player.setBalance(val)
        Catch ex As Exception
            val = 0
        End Try
        If optionsMode Then OptionsForm.labelBalance.Text = "Balance: " & val
        saveSetting(SettingsIdentifier.BALANCE, val)
    End Sub




    Public Shared Sub switchSourceMode()
        If radioEnabled Then
            changeSourceMode(MusicSource.LOCAL)
        Else
            changeSourceMode(MusicSource.RADIO)
        End If
    End Sub
    Public Shared Sub changeSourceMode(mode As MusicSource)
        If Not Form1.searchState = SearchState.NONE Then Form1.cancelSearch(False)
        If mode = 0 Then
            Form1.tv.Enabled = True
            Form1.l2_2.Enabled = True
            Form1.tSearch.Enabled = True
            Form1.menuSortBy.Enabled = True
            Form1.menuLyrics.Enabled = True
            Form1.menuStatistics.Enabled = True
            If radioEnabled Then
                Form1.saveRadioTime()
                setSetting(SettingsIdentifier.MUSIC_SOURCE, MusicSource.LOCAL)
                Form1.l2.Items.Clear()
                Form1.localfill(False)
                Form1.sortListAuto()
                Form1.setlistselected()
                Player.unmute()
                Player.pause()
            Else

                If Form1.l2.SelectedIndex > -1 Then
                    launchRadio(Form1.l2.SelectedItem)
                    Dim tr As Track = Form1.l2.SelectedItem
                    tr.play()
                ElseIf Form1.l2_2.SelectedIndex > -1 Then
                    launchRadio(Form1.l2_2.SelectedItem)
                ElseIf Not currTrack = Nothing Then
                    launchTrack(currTrack)
                End If
            End If
        Else
            If radioEnabled Then
                Form1.saveRadioTime()
            End If
            resetLoop()
            Form1.tv.Nodes.Clear()
            Form1.radfill()
            clearPlaylist()
            Form1.tv.Enabled = False
            Form1.l2_2.Enabled = False
            Form1.tSearch.Enabled = False
            Form1.menuSortBy.Enabled = False
            Form1.menuLyrics.Enabled = False
            Form1.menuStatistics.Enabled = False
            If Form1.l2.Items.Count > 0 And Player.getPlayState() = WMPPlayState.wmppsPlaying Then
                launchRadio(Form1.l2.SelectedItem)
            End If
        End If
        saveSetting(SettingsIdentifier.MUSIC_SOURCE, mode)
        HotkeyService.startHotkeyDelay()

    End Sub


    Public Shared Sub resetLoop()
        trackLoop = LoopMode.NO
        loopVals(1) = 0
        loopVals(2) = 0
        Form1.labelLoop.Cursor = Cursors.Default
    End Sub

    Public Shared Sub switchpart(switchdir As Integer) 'dir 2-forward,1-back
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
                Player.setCurrentPosition(loopVals(1))
                Form1.labelStatsUpdate()
                Form1.labelLoop.Cursor = Cursors.Hand
            End If
        End If
    End Sub


    Public Shared Sub selectPlaylist(index As Integer)
        If playlist.Count > index Then
            Form1.l2_2.SelectedIndex = index
        End If
    End Sub

    Public Shared Function playlistContains(track As Track) As Integer
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

    Public Shared Function getNextRandomTrack() As Track
        Dim tracks As New List(Of Track)
        For Each item In Form1.l2.Items
            If TypeOf item Is Track Then
                tracks.Add(item)
            End If
        Next
        If tracks.Count = 0 Then Return Nothing
        Return tracks(Utils.getNextRandom(0, tracks.Count))
    End Function

    Public Shared Sub playNextTrack()
        If currTrack Is Nothing Or currTrack IsNot Nothing AndAlso currTrack.getPlaylistIndex() = playlist.Count - 1 Then
            If Form1.l2.Items.Count = 0 Then Form1.refill(Not playMode = PlayMode.REPEAT)
            If Form1.l2.Items.Count > 0 Then

                Dim nextTrack As Track
                If randomNextTrack Then
                    nextTrack = getNextRandomTrack()
                Else
                    Form1.sortListAuto()
                    nextTrack = Form1.l2.Items(0)
                End If

                If nextTrack = Nothing Then
                    playlist(0).play()
                Else
                    If removeNextTrack Then
                        Form1.l2.Items.Remove(nextTrack)
                    End If
                    nextTrack.addToPlaylist()
                    nextTrack.selectPlaylist()

                    If Player.getPlayState() = WMPLib.WMPPlayState.wmppsPlaying Or Player.getPlayState() = WMPPlayState.wmppsUndefined Or Player.getPlayState() = WMPPlayState.wmppsStopped Then nextTrack.play()
                End If
            Else
                playlist(0).play()
            End If
        Else
            playlist(currTrack.getPlaylistIndex() + 1).play()
        End If
    End Sub

    Public Shared Sub playPrevTrack()
        If currTrack = Nothing Then
            If Form1.l2_2.SelectedIndex > 0 Then Form1.l2_2.SelectedIndex -= 1
        Else
            If currTrack.getPlaylistIndex() > 0 Then playlist(currTrack.getPlaylistIndex() - 1).play()
        End If
    End Sub

    Public Shared Sub clearPlaylist()
        For i = playlist.Count - 1 To 0 Step -1
            playlist(i).removeFromPlaylist()
        Next
    End Sub
    Public Shared Sub playStateHandler()
        'playstate handler
        If Player.getPlayState() = WMPLib.WMPPlayState.wmppsPlaying Then
            If trackLoop = LoopMode.YES Then
                If Player.getCurrentPosition() > loopVals(2) And Player.getCurrentPosition() < loopVals(1) Then
                    Player.setCurrentPosition(loopVals(1))
                End If
                If loopVals(1) < loopVals(2) Then
                    If Player.getCurrentPosition() < loopVals(1) Then
                        Player.setCurrentPosition(loopVals(1))
                    ElseIf Player.getCurrentPosition() > loopVals(2) Then
                        Player.setCurrentPosition(loopVals(1))
                    End If
                    If currTrack IsNot Nothing AndAlso loopVals(2) > currTrack.length AndAlso currTrack.length > 0 Then
                        If Math.Abs(Player.getCurrentPosition() - currTrack.length) <= 0.25 Then
                            Player.setCurrentPosition(loopVals(1))
                        End If
                    End If
                End If
            End If

            currTrack = Track.getTrack(Player.getUrl())

            If currTrack IsNot Nothing Then
                currTrack.currPart = currTrack.getCurrentPart(Player.getCurrentPosition())
            End If

            If firstPlayStart = FirstStartState.STARTING And Not radioEnabled Then
                If Not last = Nothing Then
                    Dim l As ListBox = Form1.getSelectedList()
                    If l.SelectedItem = last Then
                        Dim timeTemp As Double = SettingsService.loadSetting(SettingsIdentifier.LAST_TRACK_APPLY_TIME)

                        If timeTemp > 2 * 60 Or timeTemp > l.SelectedItem.length / 2 Then
                            Player.setCurrentPosition(timeTemp)
                        End If
                        SettingsService.saveSetting(SettingsIdentifier.LAST_TRACK_APPLY_TIME, 0.0)
                        SettingsService.saveSetting(SettingsIdentifier.LAST_TRACK_RECORDED_TIME, 0.0)
                    End If
                End If
                firstPlayStart = FirstStartState.STARTED
            End If

        ElseIf Player.getPlayState() = WMPPlayState.wmppsReady And Not radioEnabled Then
            If Not Player.isUrlEmpty() Then
                Player.resetUrl()
                Dim ind As Integer = Form1.getSelectedList().SelectedItem.removeFromPlayList()
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
        ElseIf Player.getPlayState() = WMPLib.WMPPlayState.wmppsStopped Then

            resetLoop()
            If radioEnabled Then
                If Form1.l2.Items.Count > 0 Then
                    Form1.saveRadioTime()
                    launchRadio(Form1.l2.SelectedItem.name)
                End If
            ElseIf Not currTrack = Nothing Then 'e.g. local source

                Form1.labelStatsUpdate()
                If Not IsNothing(Form1.tv.SelectedNode) Then
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
                            Form1.l2.SelectedIndex += 1
                            launchRadio(Form1.l2.SelectedItem)
                            last = Form1.l2.SelectedItem
                        Case PlayMode.REPEAT
                            Form1.labelStatsUpdate()
                            currTrack.play()
                        Case PlayMode.RANDOM
                            playNextTrack()
                        Case Else
                    End Select
                Else
                    If playMode = PlayMode.REPEAT Then
                        launchURL(Player.getUrl())
                    ElseIf playMode = PlayMode.RANDOM Then
                        playNextTrack()
                    End If

                End If
            Else
                If playMode = PlayMode.REPEAT Then
                    launchURL(Player.getUrl())
                ElseIf playMode = PlayMode.RANDOM Then
                    playNextTrack()
                End If

            End If
        End If

    End Sub

End Class
