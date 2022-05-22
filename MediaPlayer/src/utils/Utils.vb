Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports System.IO.Compression
Imports CoreAudioApi 'reference
Imports System.Runtime.InteropServices

Public Class Utils

    Public Declare Function getForegroundWindow Lib "user32" Alias "GetForegroundWindow" () As IntPtr

    Public Declare Function GetParent Lib "user32" (ByVal hwnd As IntPtr) As IntPtr
    Public Declare Function GetWindowText Lib "user32.dll" Alias "GetWindowTextA" (ByVal hWnd As IntPtr, ByVal lpString As String, ByVal cch As Integer) As Integer
    Public Declare Function GetWindowTextLength Lib "user32.dll" Alias "GetWindowTextLengthA" (ByVal hWnd As IntPtr) As Integer

    Public Declare Function GetWindowThreadProcessId Lib "user32.dll" (ByVal hWnd As IntPtr, ByRef lpdwProcessId As Integer) As Integer

    <DllImport("user32.dll", EntryPoint:="SendMessageA")>
    Public Shared Sub SendMessage(
      ByVal hWnd As IntPtr,
      ByVal uMsg As Int32,
      ByVal wParam As Int32,
      ByVal lParam As Int32)
    End Sub
    Public Declare Function BlockInput Lib "user32.dll" (ByVal fBlock As Boolean) As Boolean
    Function isWindowInForeground(ptr As IntPtr) As Boolean
        Return getForegroundWindow() = ptr
    End Function
    Function isWindowInForeground() As Boolean
        Return getForegroundWindow() = Process.GetCurrentProcess().MainWindowHandle
    End Function

    Public Shared Function AlphToInt(ByVal abc As Char) As Integer
        abc = CStr(abc).ToLower
        Select Case abc
            Case "a" : Return 1 : Case "b" : Return 2 : Case "c" : Return 3 : Case "d" : Return 4 : Case "e" : Return 5 : Case "f" : Return 6 : Case "g" : Return 7 : Case "h" : Return 8
            Case "i" : Return 9 : Case "j" : Return 10 : Case "k" : Return 11 : Case "l" : Return 12 : Case "m" : Return 13 : Case "n" : Return 14 : Case "o" : Return 15
            Case "p" : Return 16 : Case "q" : Return 17 : Case "r" : Return 18 : Case "s" : Return 19 : Case "t" : Return 20 : Case "u" : Return 21 : Case "v" : Return 22
            Case "w" : Return 23 : Case "x" : Return 24 : Case "y" : Return 25 : Case "z" : Return 26
            Case Else : Return 0
        End Select
    End Function

    Function IntToAlph(ByVal int As Integer, Optional ByVal upper As Boolean = False) As Char
        Dim abc As Char
        Select Case int
            Case 1 : abc = "a" : Case 2 : abc = "b" : Case 3 : abc = "c" : Case 4 : abc = "d" : Case 5 : abc = "e" : Case 6 : abc = "f" : Case 7 : abc = "g" : Case 8 : abc = "h"
            Case 9 : abc = "i" : Case 10 : abc = "j" : Case 11 : abc = "k" : Case 12 : abc = "l" : Case 13 : abc = "m" : Case 14 : abc = "n" : Case 15 : abc = "o" : Case 16 : abc = "p"
            Case 17 : abc = "q" : Case 18 : abc = "r" : Case 19 : abc = "s" : Case 20 : abc = "t" : Case 21 : abc = "u" : Case 22 : abc = "v" : Case 23 : abc = "w"
            Case 24 : abc = "x" : Case 25 : abc = "y" : Case 26 : abc = "z"
            Case Else : abc = ""
        End Select
        If upper = True Then abc = CStr(abc).ToUpper
        Return abc
    End Function

    Function ReverseDateString(ByVal str As String) As String
        Return Mid(str, 7, 4) & "." & Mid(str, 4, 2) & "." & Mid(str, 1, 2)
    End Function

    Function hasAudioExt(ByVal filStr As String) As Boolean
        Dim str As String = Mid(filStr, filStr.LastIndexOf(".") + 2).ToLower
        Dim audioExt() As String = {"mp3", "wav", "wma", "m4a", "flac", "aac"}
        Return audioExt.Contains(str)
    End Function


    Function GetDayDiff(ByVal dt1 As Date, ByVal dt2 As Date) As Double
        Return dt2.Subtract(dt1).TotalDays
    End Function
    Function GetDayDiff(ByVal dt11 As String, ByVal dt12 As String, ByVal dt13 As String, ByVal dt21 As String, ByVal dt22 As String, ByVal dt23 As String) As Double
        Dim d1 As Date = Date.Parse(dt11 & "." & dt12 & "." & dt13)
        Dim d2 As Date = Date.Parse(dt21 & "." & dt22 & "." & dt23)
        Return d2.Subtract(d1).TotalDays
    End Function
    Function GetDayDiff(ByVal dt1 As String, ByVal dt2 As String) As Double
        Dim d1 As Date = Date.Parse(dt1)
        Dim d2 As Date = Date.Parse(dt2)
        Return d2.Subtract(d1).TotalDays
    End Function

    Sub Encode(ByVal path As String)
        Dim str As String = ""
        Try
            Dim sr As New StreamReader(path, System.Text.Encoding.Default)
            str = sr.ReadToEnd
            sr.Close()
        Catch ex As Exception
            MsgBox("Encoding failed for " & path, MsgBoxStyle.Critical)
        End Try

        Dim rd As New RijndaelManaged

        Dim md5 As New MD5CryptoServiceProvider

        Dim key() As Byte = md5.ComputeHash(Encoding.Default.GetBytes(""))

        md5.Clear()
        rd.Key = key
        rd.GenerateIV()

        Dim iv() As Byte = rd.IV
        Dim ms As New MemoryStream

        ms.Write(iv, 0, iv.Length)

        Dim cs As New CryptoStream(ms, rd.CreateEncryptor, CryptoStreamMode.Write)
        Dim data() As Byte = System.Text.Encoding.Default.GetBytes(str)

        cs.Write(data, 0, data.Length)
        cs.FlushFinalBlock()

        Dim encdata() As Byte = ms.ToArray()

        Dim sw As New StreamWriter(path, False, System.Text.Encoding.Default)
        sw.Write(Convert.ToBase64String(encdata))
        sw.Close()
        cs.Close()
        rd.Clear()
        str = ""

    End Sub

    Function Decode(ByVal path As String) As Boolean
        Dim str As String = ""
        Try
            Dim sr As New StreamReader(path, System.Text.Encoding.Default)
            str = sr.ReadToEnd
            sr.Close()
        Catch ex As Exception
            MsgBox("Decoding failed for " & path, MsgBoxStyle.Critical)
            Return False
        End Try

        If Not str.Contains("  -  ") And str.Length > 15 Then

            Dim rd As New RijndaelManaged
            Dim rijndaelIvLength As Integer = 16
            Dim md5 As New MD5CryptoServiceProvider
            Dim key() As Byte = md5.ComputeHash(Encoding.Default.GetBytes(""))

            md5.Clear()

            Dim encdata() As Byte = Nothing
            Try
                encdata = Convert.FromBase64String(str)
            Catch ex As Exception
                Return False
            End Try

            Dim ms As New MemoryStream(encdata)
            Dim iv(15) As Byte

            ms.Read(iv, 0, rijndaelIvLength)
            rd.IV = iv
            rd.Key = key

            Dim cs As New CryptoStream(ms, rd.CreateDecryptor, CryptoStreamMode.Read)

            Dim data(ms.Length - rijndaelIvLength) As Byte

            Dim i As Integer = cs.Read(data, 0, data.Length)
            Dim sw As New StreamWriter(path, False, System.Text.Encoding.Default)
            sw.Write(System.Text.Encoding.Default.GetString(data, 0, i))
            sw.Close()
            cs.Close()
            rd.Clear()


        Else
            str = ""
        End If
        str = ""
        Return True
    End Function

    Function encrypt(ByRef buffer As String, ByVal str As String, Optional ByVal keyString As String = "") As Boolean
        Try
            If str = "" Then
                buffer = ""
                Return True
            End If

            Dim rd As New RijndaelManaged
            Dim hashBytes() As Byte = Text.Encoding.Default.GetBytes(keyString)
            ' Dim md5 As New System.Security.Cryptography.MD5CryptoServiceProvider
            '  rd.Key  = md5.ComputeHash(hashBytes)
            Dim sha256 As New SHA256CryptoServiceProvider
            rd.Key = sha256.ComputeHash(hashBytes)
            rd.GenerateIV()
            Dim iv() As Byte = rd.IV
            Dim ms As New MemoryStream()
            ms.Write(iv, 0, iv.Length)

            Dim encryptor = rd.CreateEncryptor()
            Dim cs As New CryptoStream(ms, encryptor, CryptoStreamMode.Write)
            rd.Clear()
            Dim data() As Byte = System.Text.Encoding.Default.GetBytes(str)

            cs.Write(data, 0, data.Length)
            cs.FlushFinalBlock()
            cs.Close()

            Dim encdata() As Byte = ms.ToArray()
            ms.Close()
            buffer = Convert.ToBase64String(encdata)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Function decrypt(ByRef buffer As String, ByVal str As String, ByVal keyString As String) As Integer
        If str = "" Then
            buffer = ""
            Return 0
        End If
        Dim rd As New RijndaelManaged
        Dim hashBytes() As Byte = Text.Encoding.Default.GetBytes(keyString)
        'Dim md5 As New System.Security.Cryptography.MD5CryptoServiceProvider
        ' rd.Key = md5.ComputeHash(hashBytes)
        Dim sha256 As New SHA256CryptoServiceProvider
        rd.Key = sha256.ComputeHash(hashBytes)

        Dim encdata() As Byte
        Try
            encdata = Convert.FromBase64String(str)
        Catch ex As Exception
            Return 2
        End Try
        Dim ms As New MemoryStream(encdata)
        Dim rijndaelIvLength As Integer = 16
        Dim iv(rijndaelIvLength - 1) As Byte
        ms.Read(iv, 0, rijndaelIvLength)
        rd.IV = iv

        Dim cs As New CryptoStream(ms, rd.CreateDecryptor, CryptoStreamMode.Read)
        Dim data(ms.Length - rijndaelIvLength) As Byte
        Dim i As Integer
        Try
            i = cs.Read(data, 0, data.Length)
        Catch ex As CryptographicException
            Return 1
        End Try

        cs.Close()
        rd.Clear()
        ms.Close()
        buffer = Text.Encoding.Default.GetString(data, 0, i)
        Return 0
    End Function
    Function minFormatToSec(ByVal s As String) As Integer
        If Not s = "" Then
            Dim vals() As String = s.Split(":")
            If vals IsNot Nothing Then
                If vals.Length = 2 Then
                    Return vals(0) * 60 + IIf(vals(1) = "", 0, vals(1))
                ElseIf vals.Length = 1 Then 'second format
                    Return vals(0)
                End If
            End If
        End If
        Return 0
    End Function
    Function SecondsTodhmsString(ByVal s As Integer) As String
        Return CStr(Int(Int(Int(s / 60) / 60) / 24)) & "d " &
                CStr(Int(Int(s / 60) / 60) Mod 24).PadLeft(2, "0") & "h " &
                 CStr(Int(s / 60) Mod 60).PadLeft(2, "0") & "m " &
                 CStr(Int((s Mod 3600) Mod 60)).PadLeft(2, "0") & "s"
    End Function
    Function SecondsToydhmsString(ByVal s As Integer) As String
        Return CStr(Int(Int(Int(Int(s / 60) / 60) / 24) / 365)) & "y " &
                CStr(Int(Int(Int(s / 60) / 60) / 24) Mod 365).PadLeft(3, "0") & "d " &
                CStr(Int(Int(s / 60) / 60) Mod 24).PadLeft(2, "0") & "h " &
                 CStr(Int(s / 60) Mod 60).PadLeft(2, "0") & "m " &
                 CStr(Int((s Mod 3600) Mod 60)).PadLeft(2, "0") & "s"
    End Function
    Function SecondsTohmsString(ByVal s As Integer) As String
        Return CStr(Int(Int(s / 60) / 60)).PadLeft(2, "0") & "h " &
            CStr(Int(s / 60) Mod 60).PadLeft(2, "0") & "m " &
            CStr(Int((s Mod 3600) Mod 60)).PadLeft(2, "0") & "s"
    End Function
    Function secondsTo_ms_Format(ByVal secString As String) As String
        If secString.Contains(":") Then Return secString
        Return CStr(Int(CInt(secString) / 60)).PadLeft(2, "0") & ":" & CStr(CInt(secString) Mod 60).PadLeft(2, "0")
    End Function

    Function ydhmsStringToSeconds(ByVal str As String) As Integer
        If str.Contains("s") And str.Contains("m") And str.Contains("h") And str.Contains("d") And str.Contains("y") Then
            Dim ints() As Integer = ParserInt(str, False)
            Return ints(0) * 365 * 24 * 3600 + ints(1) * 24 * 3600 + ints(2) * 3600 + ints(3) * 60 + ints(4)
        Else
            Return -1
        End If
    End Function
    Function dhmsStringToSeconds(ByVal str As String) As Integer
        If str.Contains("s") And str.Contains("m") And str.Contains("h") And str.Contains("d") Then
            Dim ints() As Integer = ParserInt(str, False)
            Return ints(0) * 24 * 3600 + ints(1) * 3600 + ints(2) * 60 + ints(3)
        Else
            Return -1
        End If
    End Function
    Function hmsStringToSeconds(ByVal str As String) As Integer
        If str.Contains("s") And str.Contains("m") And str.Contains("h") Then
            Dim ints() As Integer = ParserInt(str, False)
            Return ints(0) * 3600 + ints(1) * 60 + ints(2)
        Else
            Return -1
        End If
    End Function

    Function ParserInt(ByVal str As String, Optional ByVal signed As Boolean = True) As Integer()
        Dim reints() As Integer = Nothing
        If Not str = "" Then
            For i = 0 To str.Length - 1
                Dim ch As String = str(i)
                Dim currint As String = ""
                Dim n As Integer = 0
                Dim sign As Integer = 1
                If Char.IsDigit(ch) Then
                    If Not i = 0 Then
                        If str(i - 1) = "-" And signed = True Then sign = -1
                    End If
                    Do While i + n < str.Length
                        If Char.IsDigit(str(i + n)) Then
                            currint &= str(i + n) : n += 1
                        Else : Exit Do
                        End If
                    Loop
                    ExtendArray(reints, currint * sign)
                    i += n
                End If
            Next
        End If
        Return reints
    End Function

    Sub ExtendArray(ByRef ds() As Double, Optional ByVal value As Double = 0)
        If IsNothing(ds) Then
            ReDim ds(0)
            ds(0) = value
        Else
            ReDim Preserve ds(ds.Length)
            ds(ds.Length - 1) = value
        End If
    End Sub
    Sub ExtendArray(ByRef ints() As Integer, Optional ByVal value As Integer = 0)
        If IsNothing(ints) Then
            ReDim ints(0)
            ints(0) = value
        Else
            ReDim Preserve ints(ints.Length)
            ints(ints.Length - 1) = value
        End If
    End Sub
    Sub ExtendArray(ByRef strs() As String, Optional ByVal value As String = "")
        If IsNothing(strs) Then
            ReDim strs(0)
            strs(0) = value
        Else
            ReDim Preserve strs(strs.Length)
            strs(strs.Length - 1) = value
        End If
    End Sub

    Function GetAllDirectories(ByVal path As String, Optional ByVal subfolders As Boolean = False, Optional ByVal matchstr As String = "") As String()
        Dim all() As String = Nothing
        Dim paths() As String
        Dim tarpath() As String = {path}
        Dim newtar() As String = Nothing
        Dim flag As Boolean = False
        Do
            flag = True
            If Not IsNothing(newtar) Then : If newtar.Length > 0 Then : tarpath = newtar : ReDim newtar(0) : End If : End If
            For Each t As String In tarpath
                Dim str() As String = Nothing
                Try
                    For Each dir As String In My.Computer.FileSystem.GetDirectories(t)
                        If dir.ToLower.Contains(matchstr) Then
                            ExtendArray(str, Mid(dir, dir.LastIndexOf("\") + 2))
                        End If
                    Next
                Catch ex As Exception
                    Console.WriteLine(ex.ToString)
                End Try
                If subfolders = False Then
                    Return str
                    Exit Function
                End If
                If Not IsNothing(str) Then
                    paths = str.Clone
                    For Each a As String In paths
                        ExtendArray(all, t & a & "\")
                        ExtendArray(newtar, t & a & "\")
                        flag = False
                    Next
                End If
            Next
        Loop Until flag
        Return all
    End Function

    Function GetAllFiles(ByVal path As String, Optional ByVal getExt As Boolean = False, Optional ByVal matchstr As String = "", Optional ByVal matchext As String = "") As String()
        Dim str() As String = Nothing
        For Each fil As String In My.Computer.FileSystem.GetFiles(path)
            If fil.ToLower.Contains(matchstr) And fil.ToLower.EndsWith(matchext) Then
                If getExt = False Then
                    ExtendArray(str, Mid(fil, fil.LastIndexOf("\") + 2, fil.LastIndexOf(".") - fil.LastIndexOf("\") - 1))
                Else
                    ExtendArray(str, Mid(fil, fil.LastIndexOf("\") + 2))
                End If
            End If
        Next
        Return str
    End Function
    Function getBinaryComponents(ByVal number As Integer) As List(Of Integer)
        Dim res As New List(Of Integer)
        If number <= 0 Then Return New List(Of Integer)
        Dim highestPot As Integer = Int(Math.Log(number, 2))
        For i = highestPot To 0 Step -1
            If 2 ^ i <= number Then
                res.Add(2 ^ i)
                number -= 2 ^ i
            End If
        Next
        Return res
    End Function
    Public Declare Function SetWindowPos Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal hWndInsertAfter As IntPtr, ByVal x As Integer, ByVal y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As UInteger) As Boolean
    Public Declare Function ShowWindow Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal nCmdShow As Integer) As Boolean
    Public Const HWND_TOP = 0 ' Move to top of z-order
    Public Const SWP_NOSIZE = &H1 ' Do not re-size window
    Public Const SWP_NOMOVE = &H2 ' Do not reposition window
    Public Const SWP_SHOWWINDOW = &H40 ' Make window visible/active

    Public Shared Sub SwitchTo(ByVal hWnd As IntPtr)
        ShowWindow(hWnd, 6)
        ShowWindow(hWnd, 9)
        SetWindowPos(hWnd, HWND_TOP, 0, 0, 0, 0, SWP_NOMOVE Or SWP_NOSIZE Or SWP_SHOWWINDOW)
    End Sub

    Public Sub SetVolume(ByVal vol As Integer)
        If vol > 100 Then vol = 100
        If vol < 0 Then vol = 0
        Dim DevEnum As New MMDeviceEnumerator
        Dim device As MMDevice = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia)
        device.AudioEndpointVolume.MasterVolumeLevelScalar = vol / 100.0F
    End Sub
    Private Enum Params As Int32
        SC_MONITORPOWER = &HF170    ' wParam
        WM_SYSCOMMAND = &H112       ' uMsg
        TURN_MONITOR_OFF = 2        ' Monitor ausschalten
        TURN_MONITOR_ON = -1        ' Monitor einschalten
    End Enum
    Public Sub SetMonitorState(ByVal Index As Integer, ByVal Handle As IntPtr)
        Select Case Index
            Case 0
                SendMessage(Handle, Params.WM_SYSCOMMAND, Params.SC_MONITORPOWER,
                  Params.TURN_MONITOR_OFF)
            Case 1
                SendMessage(Handle, Params.WM_SYSCOMMAND, Params.SC_MONITORPOWER,
                  Params.TURN_MONITOR_ON)
        End Select
    End Sub

#Region "ini.vb"


    Private Declare Ansi Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (
        ByVal lpApplicationName As String, ByVal lpSchlüsselName As String, ByVal lpDefault As String,
        ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer

    Private Declare Ansi Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (
        ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String,
        ByVal lpFileName As String) As Integer

    Private Declare Ansi Function DeletePrivateProfileSection Lib "kernel32" Alias "WritePrivateProfileStringA" (
        ByVal Section As String, ByVal NoKey As Integer, ByVal NoSetting As Integer,
        ByVal FileName As String) As Integer

    Public inipath As String

    Function iniIsValidSection(ByVal Section As String, Optional ByVal path As String = "") As Boolean
        If Not path = "" Then inipath = path
        If inipath = "" Then Return False
        If Not IO.File.Exists(inipath) Then Return False

        Dim secs() As String = iniGetAllSections(inipath)
        If secs Is Nothing Then Return False
        For Each sec As String In secs
            If sec.ToLower = Section.ToLower Then Return True
        Next
        Return False
    End Function

    Function iniIsValidKey(ByVal Section As String, ByVal key As String, Optional ByVal path As String = "") As Boolean
        If Not path = "" Then inipath = path
        If inipath = "" Then Return False
        If Not IO.File.Exists(inipath) Then Return False

        If Not iniIsValidSection(Section, inipath) Then Return False
        Dim keys() As String = iniGetAllKeys(Section, inipath)
        If keys Is Nothing Then Return False
        For Each k As String In keys
            If k.ToLower = key.ToLower Then Return True
        Next
        Return False
    End Function
    Function iniGetAllLines(ByVal Section As String, Optional ByVal path As String = "") As String()
        If Not path = "" Then inipath = path
        If inipath = "" Or IO.File.Exists(inipath) = False Then
            Return Nothing
        End If
        Dim res() As String = Nothing
        Dim sr As New StreamReader(inipath, Encoding.Default)
        Do Until sr.Peek = -1
            If sr.ReadLine.ToLower = "[" & Section.ToLower & "]" Then
                Do Until sr.Peek = -1 OrElse Chr(sr.Peek) = "["
                    Dim c As String = sr.ReadLine
                    If Not c = "" Then ExtendArray(res, c)
                    If sr.EndOfStream Then Exit Do
                Loop
            End If
        Loop
        sr.Close()
        Return res
    End Function

    Function iniGetAllKeys(ByVal Section As String, Optional ByVal path As String = "") As String()
        If Not path = "" Then inipath = path
        If inipath = "" Or IO.File.Exists(inipath) = False Then
            Return Nothing
        End If
        Dim res() As String = Nothing
        Dim sr As StreamReader = Nothing
        Try
            sr = New StreamReader(inipath, Encoding.Default)
            Do Until sr.Peek = -1
                If sr.ReadLine.ToLower = "[" & Section.ToLower & "]" Then
                    Do Until sr.Peek = -1 OrElse Chr(sr.Peek) = "["
                        Dim c As String = sr.ReadLine
                        If Not c = "" Then
                            If c.Contains("=") Then
                                ExtendArray(res, Mid(c, 1, c.IndexOf("=")))
                            Else
                                ExtendArray(res, c)
                            End If
                        End If
                        If sr.EndOfStream Then Exit Do
                    Loop
                End If
            Loop
        Catch ex As Exception
            Throw ex
        Finally
            If sr IsNot Nothing Then sr.Close()
        End Try
        Return res
    End Function

    Function iniGetAllValues(ByVal Section As String, Optional ByVal path As String = "") As String()
        If Not path = "" Then inipath = path
        If inipath = "" Or IO.File.Exists(inipath) = False Then
            Return Nothing
        End If
        Dim res() As String = Nothing
        Dim sr As New StreamReader(inipath, Encoding.Default)
        Do Until sr.Peek = -1
            If sr.ReadLine.ToLower = "[" & Section.ToLower & "]" Then
                Do Until sr.Peek = -1 OrElse Chr(sr.Peek) = "["
                    Dim c As String = sr.ReadLine
                    If Not c = "" Then
                        If c.Contains("=") Then
                            ExtendArray(res, Mid(c, c.IndexOf("=") + 2))
                        Else
                            ExtendArray(res, c)
                        End If
                    End If
                    If sr.EndOfStream Then Exit Do
                Loop
            End If
        Loop
        sr.Close()
        Return res
    End Function

    Function iniGetAllSections(Optional ByVal path As String = "") As String()
        If Not path = "" Then inipath = path
        If inipath = "" Or IO.File.Exists(inipath) = False Then
            Return Nothing
        End If
        Dim res() As String = Nothing
        Try
            Dim sr As New StreamReader(inipath, Encoding.Default)
            Do Until sr.Peek = -1
                If Chr(sr.Peek) = "[" Then
                    Dim c As String = sr.ReadLine
                    ExtendArray(res, Mid(c, 2, c.Length - 2))
                Else
                    sr.ReadLine()
                End If
            Loop
            sr.Close()
        Catch ex As Exception
            Return Nothing
        End Try
        Return res
    End Function

    Function iniGetAllPairs(ByVal Section As String, Optional ByVal path As String = "") As List(Of KeyValuePair(Of String, String))
        If Not path = "" Then inipath = path
        If inipath = "" Or IO.File.Exists(inipath) = False Then
            Return Nothing
        End If
        Dim keys() As String = iniGetAllKeys(Section, path)
        Dim vals() As String = iniGetAllValues(Section, path)

        Dim res As New List(Of KeyValuePair(Of String, String))
        If keys IsNot Nothing Then
            For i = 0 To keys.Length - 1
                res.Add(New KeyValuePair(Of String, String)(keys(i), vals(i)))
            Next
        End If
        Return res
    End Function


    Public Function iniReadValue(ByVal Section As String, ByVal Key As String, Optional ByVal Defaultvalue As String = "", Optional ByVal path As String = "", Optional ByVal BufferSize As Integer = 1024) As String
        If Not path = "" Then inipath = path
        If inipath = "" Or IO.File.Exists(inipath) = False Then
            Return ""
        End If
        Dim sTemp As String = Space(BufferSize)
        Dim Length As Integer = GetPrivateProfileString(Section, Key, Defaultvalue, sTemp, BufferSize, inipath)
        Return Left(sTemp, Length)
    End Function


    Public Sub iniWriteValue(ByVal Section As String, ByVal Key As String, ByVal Value As String, Optional ByVal path As String = "")
        If Not path = "" Then inipath = path
        If inipath = "" Or IO.Directory.Exists(IO.Path.GetDirectoryName(inipath)) = False Then
            Exit Sub
        End If
        WritePrivateProfileString(Section, Key, Value, inipath)
    End Sub

    Public Sub iniRenameKey(ByVal section As String, ByVal key As String, ByVal value As String, Optional ByVal path As String = "")
        If Not path = "" Then inipath = path
        If inipath = "" Or IO.Directory.Exists(IO.Path.GetDirectoryName(inipath)) = False Then
            Exit Sub
        End If
        Dim keys() As String = iniGetAllKeys(section, inipath)
        Dim newKeys() As String = keys.Clone
        Dim vals() As String = iniGetAllValues(section, inipath)
        If keys IsNot Nothing Then
            For i = 0 To keys.Length - 1
                If keys(i).ToLower = key.ToLower Then
                    newKeys(i) = value
                    Exit For
                End If
            Next
            For i = 0 To keys.Length - 1
                iniDeleteKey(section, keys(i), inipath)
            Next
            For i = 0 To newKeys.Length - 1
                iniWriteValue(section, newKeys(i), vals(i), inipath)
            Next
        End If
    End Sub

    Function iniGetAllKeysList(ByVal Section As String, Optional ByVal path As String = "") As List(Of String)
        If Not path = "" Then inipath = path
        If inipath = "" Or IO.File.Exists(inipath) = False Then
            Return Nothing
        End If
        Dim res As New List(Of String)
        Dim sr As New StreamReader(inipath, Encoding.Default)
        Do Until sr.Peek = -1
            If sr.ReadLine.ToLower = "[" & Section.ToLower & "]" Then
                Do Until Chr(sr.Peek) = "["
                    Dim c As String = sr.ReadLine
                    If Not c = "" Then res.Add(c.Substring(0, c.IndexOf("=")))
                    If sr.EndOfStream Then Exit Do
                Loop
            End If
        Loop
        sr.Close()
        Return res
    End Function

    Function iniGetAllValuesList(ByVal Section As String, Optional ByVal path As String = "") As List(Of String)
        If Not path = "" Then inipath = path
        If inipath = "" Or IO.File.Exists(inipath) = False Then
            Return Nothing
        End If
        Dim res As New List(Of String)
        Dim sr As New StreamReader(inipath, Encoding.Default)
        Do Until sr.Peek = -1
            If sr.ReadLine.ToLower = "[" & Section.ToLower & "]" Then
                Do Until Chr(sr.Peek) = "["
                    Dim c As String = sr.ReadLine
                    If Not c = "" Then res.Add(c.Substring(c.IndexOf("=") + 1))
                    If sr.EndOfStream Then Exit Do
                Loop
            End If
        Loop
        sr.Close()
        Return res
    End Function



    Public Sub iniDeleteKey(ByVal Section As String, ByVal Key As String, Optional ByVal path As String = "")
        If Not path = "" Then inipath = path
        If inipath = "" Then
            Exit Sub
        End If

        Dim File As String
        File = IO.Path.GetDirectoryName(inipath)
        If IO.Directory.Exists(File) = False Then

            Exit Sub
        End If

        WritePrivateProfileString(Section, Key, Nothing, inipath)
    End Sub

    Public Sub iniDeleteSection(ByVal Section As String, Optional ByVal path As String = "")
        If Not path = "" Then inipath = path
        If inipath = "" Then
            Exit Sub
        End If

        If IO.File.Exists(inipath) = False Then
            Exit Sub
        End If

        DeletePrivateProfileSection(Section, 0, 0, inipath)
    End Sub

    Public Function iniAppendRaw(value As String, Optional path As String = "") As Boolean
        If Not path = "" Then inipath = path
        If inipath = "" Or IO.File.Exists(inipath) = False Then
            Return False
        End If
        Try
            Dim sw As New StreamWriter(path, True, Text.Encoding.Default)
            sw.Write(value)
            sw.Close()
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Public Function iniInsertRaw(section As String, value As String, Optional path As String = "") As Boolean
        If Not path = "" Then inipath = path
        If inipath = "" Or IO.File.Exists(inipath) = False Then
            Return False
        End If
        Dim s As String = ""
        Try
            Dim sr As New StreamReader(path, Text.Encoding.Default)
            s = sr.ReadToEnd()
            sr.Close()
        Catch ex As Exception
            Return False
        End Try
        Dim pos As Integer = s.IndexOf(section)
        If pos = -1 Then
            Return iniAppendRaw(value, path)
        Else

        End If
        Try
            Dim sw As New StreamWriter(path, True, Text.Encoding.Default)
            sw.Write(value)
            sw.Close()
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

#End Region

#Region "FTP"

    Public Structure Credentials
        Dim ip As String
        Dim user As String
        Dim pw As String
    End Structure

    Function ftpUpload(ByVal ip As String, ByVal sourceFile As String, ByVal destFile As String, ByVal user As String, ByVal pw As String) As String
        Dim req As System.Net.FtpWebRequest = DirectCast(System.Net.WebRequest.Create("ftp://" & ip & destFile), System.Net.FtpWebRequest)
        req.Credentials = New Net.NetworkCredential(user, pw)
        req.Method = Net.WebRequestMethods.Ftp.UploadFile
        Dim bytes() As Byte = IO.File.ReadAllBytes(sourceFile)
        req.Timeout = 90
        Dim stre As Stream
        Try
            stre = req.GetRequestStream
            stre.Write(bytes, 0, bytes.Length)
            stre.Close()
            stre.Dispose()
            Return "Success"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function


    Public req As New Net.WebClient()
    Public ftpCred As New Credentials
    Public updateVersionPath As String = ""
    Public updateIndex As Integer = 0
    Public publishFileList As List(Of String)
    Public updateFiles() As String


    Function ftpDownload(ByVal creds As Credentials, ByVal sourceFile As String, ByVal destFile As String) As Boolean
        Dim req As New Net.WebClient()
        req.Credentials = New Net.NetworkCredential(creds.user, creds.pw)
        Dim bytes() As Byte = Nothing
        Try
            bytes = req.DownloadData("ftp://" & creds.ip & sourceFile)
        Catch ex As Exception
            Return False
        End Try
        Dim stre As FileStream = IO.File.Create(destFile)
        Try
            stre.Write(bytes, 0, bytes.Length)
            stre.Close()
            stre.Dispose()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Sub ftpDownloadAsync(ByVal creds As Credentials, ByVal sourceFile As String, ByVal destFile As String)
        req = New WebClient()
        req.Credentials = New Net.NetworkCredential(creds.user, creds.pw)
        Dim request = DirectCast(WebRequest.Create("ftp://" & creds.ip & sourceFile), FtpWebRequest)
        request.Credentials = New NetworkCredential(creds.user, creds.pw)
        request.Method = WebRequestMethods.Ftp.GetFileSize
        Dim len As Long
        Try
            len = Math.Max(1, DirectCast(request.GetResponse(), FtpWebResponse).ContentLength)
        Catch ex As Exception
            len = -1
        End Try

        AddHandler req.DownloadProgressChanged, Sub(sender As Object, e As Net.DownloadProgressChangedEventArgs)
                                                    Dim perc As Integer = CInt((e.BytesReceived / len) * 100)
                                                    OptionsForm.pBar2.Value = Math.Min(100, perc)
                                                    Dim fillStr As String = "NaN"
                                                    If updateFiles IsNot Nothing Then fillStr = updateFiles.Length + 1
                                                    OptionsForm.labelftpTotalProg.Text = updateIndex & " / " & fillStr
                                                    Dim fillNum As Integer = "1"
                                                    If updateFiles IsNot Nothing Then fillNum = updateFiles.Length + 1
                                                    OptionsForm.pBar.Value = (100 / fillNum) * (updateIndex)
                                                    OptionsForm.labelftpCurrProg.Text = e.BytesReceived & " / " & len & " - " & perc & " %"
                                                End Sub
        AddHandler req.DownloadDataCompleted, Sub(sender As Object, e As System.Net.DownloadDataCompletedEventArgs)

                                                  Dim stre As FileStream = IO.File.Create(destFile)
                                                  Try
                                                      If e.Result IsNot Nothing Then
                                                          stre.Write(e.Result, 0, e.Result.Length)
                                                          stre.Close()
                                                          stre.Dispose()
                                                          downloadCallback(creds, sourceFile, destFile)
                                                      End If
                                                  Catch ex As Exception
                                                      OptionsForm.abortDownloadGC("Error writing byte array" & vbNewLine & ex.Message & vbNewLine & vbNewLine & ex.InnerException.Message)
                                                  End Try
                                              End Sub

        req.DownloadDataAsync(New Uri("ftp://" & creds.ip & sourceFile), Nothing)
    End Sub

    Sub downloadCallback(ByVal creds As Credentials, ByVal sourceFile As String, ByVal destFile As String)
        If sourceFile = "/mp3player/releases" Then
            Try
                Dim sr As New StreamReader(My.Application.Info.DirectoryPath & "\Releases\releases")
                Dim fils() As String = sr.ReadToEnd().Split(";")
                sr.Close()
                updateVersionPath = fils(0)
                For i = 0 To fils.Length - 1
                    fils(i) = fils(i).Replace(";", "")
                Next
                ReDim updateFiles(fils.Length - 2)
                For i = 1 To fils.Length - 1
                    updateFiles(i - 1) = fils(i)
                Next
            Catch ex As Exception
                OptionsForm.abortDownloadGC("Failed to read release file")
            End Try
        End If
        updateIndex += 1
        updatePlayerAsync(creds, updateVersionPath)
    End Sub


    Function ftpCheckStatus(ByVal creds As Credentials) As Boolean
        Dim request = DirectCast(WebRequest.Create("ftp://" & creds.ip), FtpWebRequest)
        request.Credentials = New NetworkCredential(creds.user, creds.pw)
        request.Method = WebRequestMethods.Ftp.ListDirectory
        Try
            Using response As FtpWebResponse = DirectCast(request.GetResponse(), FtpWebResponse)
                Return True
            End Using
        Catch ex As WebException
        End Try
        Return False
    End Function

    Function updatePlayer(ByVal creds As Credentials) As Integer
        Dim versionPath As String = ""
        Dim basePath As String = My.Application.Info.DirectoryPath  '.Substring(0, My.Application.Info.DirectoryPath.LastIndexOf("\"))
        If Not Directory.Exists(basePath & "\Releases") Then Directory.CreateDirectory(basePath & "\Releases")
        If Not ftpDownload(creds, "/mp3player/releases", basePath & "\Releases\releases") Then Return -1
        Try
            Dim sr As New StreamReader(basePath & "\Releases\releases")
            versionPath = sr.ReadToEnd
            sr.Close()
        Catch ex As Exception
            Return -2
        End Try

        If Not Directory.Exists(basePath & "\Releases\Release" & versionPath) Then Directory.CreateDirectory(basePath & "\Releases\Release" & versionPath)
        For Each fil As String In updateFiles
            ftpDownload(creds, "/mp3player/release" & versionPath & "/" & fil, basePath & "\Releases\Release" & versionPath & "\" & fil)
        Next
        If File.Exists(basePath & "\Releases\Release" & versionPath & "\mp3player2.exe") Then File.Delete(basePath & "\Releases\Release" & versionPath & "\mp3player2.exe")
        File.Copy(basePath & "\Releases\Release" & versionPath & "\mp3player.exe", basePath & "\Releases\Release" & versionPath & "\mp3player2.exe")
        Process.Start(basePath & "\Releases\Release" & versionPath & "\mp3player2.exe", "up " & basePath)
        Return 1
    End Function


    Sub updatePlayerAsync(ByVal creds As Credentials, Optional ByVal versionPath As String = "")
        Dim basePath As String = My.Application.Info.DirectoryPath
        Select Case updateIndex
            Case 0
                If Not Directory.Exists(basePath & "\Releases") Then Directory.CreateDirectory(basePath & "\Releases")
                ftpDownloadAsync(creds, "/mp3player/releases", basePath & "\Releases\releases")
            Case Is <= updateFiles.Count
                If updateIndex = 1 And Not Directory.Exists(basePath & "\Releases\Release" & versionPath) Then Directory.CreateDirectory(basePath & "\Releases\Release" & versionPath)
                '  MsgBox("updateindex " & updateIndex & " inc " & vbNewLine & "'" & "/mp3player/release" & versionPath & "/" & updateFiles(updateIndex - 1) & "'" & vbNewLine & _
                '    "'" & basePath & "\Releases\Release" & versionPath & "\" & updateFiles(updateIndex - 1) & "'")
                ftpDownloadAsync(creds, "/mp3player/release" & versionPath & "\" & updateFiles(updateIndex - 1), basePath & "\Releases\Release" & versionPath & "\" & updateFiles(updateIndex - 1))
            Case Is > updateFiles.Count
                updateIndex = 0
                updateFiles = Nothing

                Try
                    Dim archiveEntries As List(Of ZipArchiveEntry) = getArchiveEntries(basePath & "\Releases\Release" & versionPath & "\mp3player.zip")
                    For Each entry As ZipArchiveEntry In archiveEntries
                        Dim name As String = basePath & "\Releases\Release" & versionPath & "\" & entry.FullName
                        If IO.File.Exists(name) Then IO.File.Delete(name)
                    Next
                Catch ex As Exception
                    MsgBox("Failed to delete existing file for archive extraction")
                End Try

                Try
                    extractArchive(basePath & "\Releases\Release" & versionPath & "\mp3player.zip", basePath & "\Releases\Release" & versionPath & "\")
                Catch ex As Exception
                    MsgBox("Failed to extract files from archive " & vbNewLine & basePath & "\Releases\Release" & versionPath & "\mp3player.zip" & vbNewLine & "to destination" & vbNewLine & basePath & "\Releases\Release" & versionPath & "\")
                End Try
                ' If File.Exists(basePath & "\Releases\Release" & versionPath & "\mp3player2.exe") Then File.Delete(basePath & "\Releases\Release" & versionPath & "\mp3player2.exe")
                '  File.Copy(basePath & "\Releases\Release" & versionPath & "\mp3player.exe", basePath & "\Releases\Release" & versionPath & "\mp3player2.exe")
                Process.Start(basePath & "\Releases\Release" & versionPath & "\mp3player.exe", "up " & basePath)

                '  If File.Exists(basePath & "\Releases\Release" & versionPath & "\mp3player2.exe") Then File.Delete(basePath & "\Releases\Release" & versionPath & "\mp3player2.exe")
                ' File.Copy(basePath & "\Releases\Release" & versionPath & "\mp3player.exe", basePath & "\Releases\Release" & versionPath & "\mp3player2.exe")
                ' Process.Start(basePath & "\Releases\Release" & versionPath & "\mp3player2.exe", "up " & basePath)
        End Select
    End Sub

    Public ftpThread As Thread
    Sub checkPlayerUpdate(ByVal creds As Credentials, ByVal silent As Boolean)
        ftpThread = New Thread(Sub()
                                   If ftpCheckStatus(creds) Then
                                       Dim basePath As String = My.Application.Info.DirectoryPath
                                       If Not Directory.Exists(basePath & "\Releases") Then Directory.CreateDirectory(basePath & "\Releases")
                                       If Not ftpDownload(creds, "/mp3player/releases", basePath & "\Releases\releases") Then
                                           If Not silent Then MsgBox("Failed to download release file")
                                           Threading.Thread.CurrentThread.Abort()
                                       End If

                                       Dim sr As New StreamReader(basePath & "\Releases\releases")
                                       updateVersionPath = sr.ReadToEnd.Split(";")(0)
                                       sr.Close()

                                       Dim currVersion As String = ""
                                       Try
                                           Dim sr2 As New StreamReader(My.Application.Info.DirectoryPath & "\version")
                                           currVersion = sr2.ReadToEnd
                                           sr2.Close()
                                       Catch ex As Exception
                                           If Not silent Then MsgBox("Failed to read current version")
                                           Threading.Thread.CurrentThread.Abort()
                                       End Try
                                       Dim res As Integer = String.Compare(updateVersionPath, currVersion)
                                       Select Case res
                                           Case -1
                                               If Not silent Then MsgBox("Invalid version")
                                           Case 0
                                               If Not silent Then MsgBox("Player is up to date")
                                           Case 1
                                               '     Form1.Button2.Invoke(Sub() Form1.Button2.Text = "new")

                                               MsgBox("New version available!" & vbNewLine & vbNewLine &
                                                      "Old version:" & vbNewLine & currVersion & vbNewLine & vbNewLine &
                                                      "New version:" & vbNewLine & updateVersionPath)
                                       End Select
                                   Else
                                       If Not silent Then MsgBox("Server offline")
                                   End If
                                   Threading.Thread.CurrentThread.Abort()
                               End Sub)
        ftpThread.IsBackground = True
        ftpThread.Start()
    End Sub

    Function createArchive(destination As String, sourceDirectory As String) As Boolean
        If sourceDirectory = "" OrElse Not IO.Directory.Exists(sourceDirectory) Then
            IO.File.Create(destination).Close()
        Else
            ZipFile.CreateFromDirectory(sourceDirectory, destination)
        End If
        Return True
    End Function

    Function addToArchive(archivePath As String, filePath As String, Optional mode As CompressionLevel = CompressionLevel.Fastest) As Boolean
        Try
            Using archive As ZipArchive = ZipFile.Open(archivePath, ZipArchiveMode.Update)
                archive.CreateEntryFromFile(filePath, filePath.Substring(filePath.LastIndexOf("\") + 1), mode)
            End Using
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Function extractArchive(archivePath As String, destination As String)
        ZipFile.ExtractToDirectory(archivePath, destination)
        Return True
    End Function

    Function getArchiveEntries(archivePath As String) As List(Of ZipArchiveEntry)
        Dim archive As ZipArchive = ZipFile.Open(archivePath, ZipArchiveMode.Read)
        Dim res As New List(Of ZipArchiveEntry)
        For Each entry As ZipArchiveEntry In archive.Entries
            res.Add(entry)
        Next
        Return res
    End Function

    Function publishPlayer(ByVal ftpPath As String) As String
        Dim err As String = ""
        Dim myDir As String = My.Application.Info.DirectoryPath & "\"
        Dim ftpP As String = ftpPath & "mp3player\Release" & ReverseDateString(Now.ToShortDateString) & "_" & Now.Hour.ToString.PadLeft(2, "0") & "." & Now.Minute.ToString.PadLeft(2, "0") & "." & Now.Second.ToString.PadLeft(2, "0") & "\" ' & "." & IIf(Now.Hour < 10, "0", "") & Now.Hour & "." & IIf(Now.Minute < 10, "0", "") & Now.Minute & "." & IIf(Now.Second < 10, "0", "") & Now.Second & "\"
        Directory.CreateDirectory(ftpP)
        For Each fil As String In publishFileList
            Try
                addToArchive(ftpP & "mp3player.zip", myDir & fil)
                'File.Copy(myDir & fil, ftpP & fil)
            Catch ex As Exception
                err &= "Failed to add " & myDir & fil & " to archive " & ftpP & fil & vbNewLine
            End Try
        Next
        If err = "" Then
            Dim wr As StreamWriter = Nothing
            Try
                wr = New StreamWriter(ftpPath & "mp3player\releases", False)
                wr.Write(ftpP.Substring(ftpP.IndexOf("mp3player\Release") + 17, ftpP.LastIndexOf("\") - 17 - ftpP.IndexOf("mp3player\Release")))
                For i = 0 To publishFileList.Count - 1
                    'wr.Write(";" & publishFileList(i))
                Next
                wr.Write(";mp3player.zip")
            Catch ex As Exception
                err &= "Failed write to file " & ftpPath & "mp3player\releases"
            Finally
                wr.Close()
            End Try
        End If
        If err = "" Then err = ftpP.Substring(ftpP.IndexOf("mp3player\Release") + 17, ftpP.LastIndexOf("\") - 17 - ftpP.IndexOf("mp3player\Release"))
        Return err
    End Function



#End Region

End Class