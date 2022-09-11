Imports WMPLib

Public Class TcpRemoteControl

#Region "Tcghp"

    Public Shared remoteTcp As Tcp
    Shared lastTcpCommand As String
    Public Shared Sub resetConnection(ByVal port As Integer, Optional ByVal errorMsg As Boolean = True)
        If remoteTcp IsNot Nothing Then
            remoteTcp.stopListener()
            remoteTcp.stopAllConnections()
        End If
        remoteTcp = New Tcp()
        startListener(port, errorMsg)
    End Sub

    Public Shared Sub startListener(ByVal port As Integer, Optional ByVal errorMsg As Boolean = True)
        If remoteTcp.startListener(port) Then
            OptionsForm.labelPort.Text = remoteTcp.port
            Form1.setRemoteImage()
            OptionsForm.setListenerStatus()

            doListen()
        Else
            OptionsForm.labelPort.Text = port
            remoteTcp.port = port
            Form1.setRemoteImage()
            remoteTcp.stopListener()
            OptionsForm.setListenerStatus()
            If errorMsg Then MsgBox("Port is not open", MsgBoxStyle.Exclamation)
        End If
    End Sub

    Public Shared Sub stopConnection(ip As String, ByVal reason As String, Optional ByVal showErr As Boolean = True)
        Try
            remoteTcp.send(ip, "disconnect")
        Catch ex As Exception
            If showErr Then MsgBox(reason & ": dc send failed")
        End Try
        Dim res As Integer = remoteTcp.stopConnection(ip)
        stopConnectionCallbackHandler(res, reason, showErr)
    End Sub
    Private Shared Sub stopConnection(connection As Tcp.ClientConnection, ByVal reason As String, Optional ByVal showErr As Boolean = True)
        Try
            connection.send("disconnect")
        Catch ex As Exception
            If showErr Then MsgBox(reason & ": dc send failed")
        End Try
        Dim res As Integer = connection.closeConnection()
        stopConnectionCallbackHandler(res, reason, showErr)
    End Sub

    Public Shared Sub stopAllConnections(ByVal reason As String, Optional ByVal showErr As Boolean = True)
        Try
            remoteTcp.sendAll("disconnect")
        Catch ex As Exception
            If showErr Then MsgBox(reason & ": dc send failed")
        End Try
        Dim res As Integer = remoteTcp.stopAllConnections()
        stopConnectionCallbackHandler(res, reason, showErr)
    End Sub

    Private Shared Sub stopConnectionCallbackHandler(resultCode As Integer, ByVal reason As String, Optional ByVal showErr As Boolean = True)
        If resultCode = 1 Then
            If showErr Then MsgBox(reason & ": thread close failed")
        ElseIf resultCode = 2 Then
            If showErr Then MsgBox(reason & ": client close failed")
        End If
        OptionsForm.refreshRemoteUI()
        Form1.setRemoteImage()
    End Sub

    Private Shared Async Sub doListen()
        Do
            Dim res As Tcp.ListenResult = Await remoteTcp.listen(0)

            If res.resultCode = 1 Then
                If Not SettingsService.getSetting(SettingsIdentifier.REMOTE_BLOCK_EXT_IPS) Then
                    OptionsForm.SendToBack()
                    If MsgBox("External device [" & remoteTcp.getIp(res.client) & "] requesting remote access." & vbNewLine & "Accept connection?", MsgBoxStyle.YesNo + MsgBoxStyle.Information) = MsgBoxResult.No Then
                        remoteTcp.stopConnection(remoteTcp.getIp(res.client))
                    Else
                        If remoteTcp.establishConnection(res.client).resultCode = 2 Then
                            Form1.setRemoteImage()
                            OptionsForm.refreshRemoteUI()

                        End If
                    End If
                Else
                    remoteTcp.stopConnection(remoteTcp.getIp(res.client))
                End If
            ElseIf res.resultCode = 2 Then
                Form1.setRemoteImage()
                OptionsForm.refreshRemoteUI()
            ElseIf res.resultCode = 3 Then
                Exit Do
            End If
        Loop
    End Sub

    Public Shared Sub dispatchTcpMessages()
        Dim connections = remoteTcp.getRandomConnectionOrder()
        connections.ForEach(Sub(c) handleTcpMessages(c))
    End Sub

    Private Shared Sub handleTcpMessages(connection As Tcp.ClientConnection)
        Dim fullComm As String = connection.tcpMsg

        If fullComm Is Nothing Then
            stopConnection(connection, "abort")
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
                        stopConnection(connection, "abort")
                    Case "req"
                        connection.send("ack")
                    Case "play_pause"
                        Key.keyList(Key.keyName.Play_Pause).execute()
                    Case "headset"
                        AudioService.setSoundDevice("Headset")
                    Case "speakers"
                        AudioService.setSoundDevice("Speaker")
                    Case "bluetooth"
                        AudioService.setSoundDevice("Speakers")
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
                        AudioService.system_volume_down()
                        If lastTcpCommand = comm Then AudioService.system_volume_down()
                    Case "sysvol_up"
                        AudioService.system_volume_up()
                        If lastTcpCommand = comm Then AudioService.system_volume_up()
                    Case "sysvol_mute"
                        AudioService.system_volume_mute()
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
                                If Not OperatingSystem.isProcessAlive("magnify") Then Process.Start("magnify") 'Shell("magnify")
                            Else
                                Utils.killProc("magnify")
                            End If
                            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\Microsoft\ScreenMagnifier", "Magnification", zoom)

                        ElseIf comm.StartsWith("position_") Then
                            Dim m As Integer = comm.Substring(9, comm.LastIndexOf("_") - 9)
                            Dim s As Integer = comm.Substring(comm.LastIndexOf("_") + 1)
                            Form1.wmp.Ctlcontrols.currentPosition = m * 60 + s
                        ElseIf comm.StartsWith("sysvol_") Then
                            Try
                                Dim v As Integer = comm.Substring(7)
                                AudioService.SetVolume(v)
                            Catch ex As Exception
                            End Try
                        ElseIf comm.StartsWith("volume_") Then
                            Try
                                Dim v As Integer = comm.Substring(7)
                                Form1.wmp.settings.volume = v
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
                                OperatingSystem.SetMonitorState(0, Process.GetCurrentProcess.MainWindowHandle)
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
                                    Dim conNode() As TreeNode = Form1.tv.Nodes.Find(Folder.top.name & "\" & comm.Substring(5), True)
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
                                    For i = 0 To Form1.l2.Items.Count - 1
                                        sendS &= Form1.l2.Items(i).name & IIf(i = Form1.l2.Items.Count - 1, "", vbLf)
                                    Next
                                    connection.send("ansl2" & sendS & "*")
                                End If
                            ElseIf comm.StartsWith("reql3") Then
                                If Form1.l2_2.Items.Count = 0 Or radioEnabled Then
                                    connection.send("ansl3*")
                                Else
                                    Dim sendS As String = ""
                                    Dim currBufferCount As Integer = 0
                                    For i = 0 To Form1.l2_2.Items.Count - 2
                                        sendS &= Form1.l2_2.Items(i).ToString & vbLf
                                        currBufferCount += Form1.l2_2.Items(i).ToString.Length + 1
                                    Next
                                    sendS &= Form1.l2_2.Items(Form1.l2_2.Items.Count - 1).ToString
                                    currBufferCount += Form1.l2_2.Items(Form1.l2_2.Items.Count - 1).ToString.Length
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
                                    If Genre.contains(Folder.getSelectedFolder(Form1.tv).name) Then
                                        sendS &= vbLf & Folder.getSelectedFolder(Form1.tv).name 'Array.IndexOf(genres, tv.SelectedNode.Name)
                                    Else : sendS &= vbLf & "Everything" '"0"
                                    End If
                                    connection.send("anstv" & sendS)
                                Else
                                    connection.send("anstvrad")
                                End If
                            ElseIf comm.StartsWith("reqlb") Then
                                Dim sendS As String = ""
                                If Form1.currTrack IsNot Nothing Then
                                    Form1.currTrack.invalidateStats()
                                    sendS &= Form1.currTrack.name & vbLf & Form1.currTrack.count & vbLf &
                                                                 CInt(Form1.currTrack.length) & vbLf & Form1.currTrack.added.ToShortDateString
                                Else
                                    If radioEnabled And Form1.wmp.playState = WMPPlayState.wmppsPlaying Then
                                        sendS &= Form1.l2.SelectedItem.name & vbLf & "0" & vbLf & "0" & vbLf & "0"
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
                                For i = 0 To Form1.l2.Items.Count - 1
                                    If Form1.l2.Items(i).name = trName Then
                                        Form1.l2.Items(i).play()
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
                lastTcpCommand = comm
            Next

        End If
    End Sub

    Private Shared Function getMouseScroll(ByVal raw As String) As List(Of Integer)
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
    Private Shared Function getMouseMove(ByVal raw As String) As List(Of Integer())
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


End Class
