Imports System.IO
Imports System.Text

Public Class IniService

    Private Declare Ansi Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (
        ByVal lpApplicationName As String, ByVal lpSchlüsselName As String, ByVal lpDefault As String,
        ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer

    Private Declare Ansi Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (
        ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String,
        ByVal lpFileName As String) As Integer

    Private Declare Ansi Function DeletePrivateProfileSection Lib "kernel32" Alias "WritePrivateProfileStringA" (
        ByVal Section As String, ByVal NoKey As Integer, ByVal NoSetting As Integer,
        ByVal FileName As String) As Integer

    Public Shared inipath As String
    Private Const SECTION_START = "["
    Private Const SECTION_END = "]"
    Private Const KEY_VALUE_SEPARATOR = "="

    Public Shared Sub init(defaultIniPath As String)
        inipath = defaultIniPath
    End Sub

    Private Shared Function getIniFile(path As String) As String
        Dim result As String = inipath
        If Not path = "" Then result = path
        If result = "" Then
            ExceptionService.handleException("No iniPath specified.")
        End If
        If Not IO.File.Exists(result) Then
            ExceptionService.handleException("File '" + inipath + "' does not exist.")
        End If
        Return result
    End Function

    Public Shared Function iniIsValidSection(section As String, Optional path As String = "") As Boolean
        Dim iniFile As String = getIniFile(path)
        Dim secs As List(Of String) = iniGetAllSections(inipath)
        If secs Is Nothing Then Return False
        For Each sec As String In secs
            If sec.ToLower() = section.ToLower() Then Return True
        Next
        Return False
    End Function

    Public Shared Function iniIsValidKey(section As String, key As String, Optional path As String = "") As Boolean
        Dim iniFile As String = getIniFile(path)
        If Not iniIsValidSection(section, iniFile) Then Return False
        Dim keys As List(Of String) = iniGetAllKeys(section, iniFile)
        For Each k As String In keys
            If k.ToLower() = key.ToLower() Then Return True
        Next
        Return False
    End Function
    Public Shared Function iniGetAllLines(section As String, Optional path As String = "") As List(Of String)
        Dim iniFile As String = getIniFile(path)
        Dim res As New List(Of String)
        Dim sr As New StreamReader(iniFile, Encoding.Default)
        Do Until sr.Peek = -1
            If sr.ReadLine().ToLower() = SECTION_START & section.ToLower() & SECTION_END Then
                Do Until sr.Peek = -1 OrElse Chr(sr.Peek) = SECTION_START
                    Dim line As String = sr.ReadLine
                    If line <> "" Then res.Add(line)
                    If sr.EndOfStream Then Exit Do
                Loop
            End If
        Loop
        sr.Close()
        Return res
    End Function

    Public Shared Function iniGetAllKeys(section As String, Optional path As String = "") As List(Of String)
        Dim iniFile As String = getIniFile(path)
        Dim res As New List(Of String)
        Dim sr As StreamReader = Nothing
        Try
            sr = New StreamReader(iniFile, Encoding.Default)
            Do Until sr.Peek = -1
                If sr.ReadLine().ToLower() = SECTION_START & section.ToLower() & SECTION_END Then
                    Do Until sr.Peek = -1 OrElse Chr(sr.Peek) = SECTION_START
                        Dim line As String = sr.ReadLine
                        If Not line = "" Then
                            If line.Contains(KEY_VALUE_SEPARATOR) Then
                                res.Add(line.Substring(0, line.IndexOf(KEY_VALUE_SEPARATOR)))
                            Else
                                res.Add(line)
                            End If
                        End If
                        If sr.EndOfStream Then Exit Do
                    Loop
                End If
            Loop
        Catch ex As Exception
            ExceptionService.handleException("Error fetching all ini keys.", ex)
        Finally
            If sr IsNot Nothing Then sr.Close()
        End Try
        Return res
    End Function

    Public Shared Function iniGetAllValues(section As String, Optional path As String = "") As List(Of String)
        Dim iniFile As String = getIniFile(path)
        Dim res As New List(Of String)
        Dim sr As New StreamReader(iniFile, Encoding.Default)
        Do Until sr.Peek = -1
            If sr.ReadLine().ToLower() = SECTION_START & section.ToLower() & SECTION_END Then
                Do Until sr.Peek = -1 OrElse Chr(sr.Peek) = SECTION_START
                    Dim line As String = sr.ReadLine
                    If Not line = "" Then
                        If line.Contains(KEY_VALUE_SEPARATOR) Then
                            res.Add(line.Substring(line.IndexOf(KEY_VALUE_SEPARATOR) + 1))
                        Else
                            res.Add(line)
                        End If
                    End If
                    If sr.EndOfStream Then Exit Do
                Loop
            End If
        Loop
        sr.Close()
        Return res
    End Function

    Public Shared Function iniGetAllSections(Optional path As String = "") As List(Of String)
        Dim iniFile As String = getIniFile(path)
        Dim res As New List(Of String)
        Try
            Dim sr As New StreamReader(iniFile, Encoding.Default)
            Do Until sr.Peek = -1
                If Chr(sr.Peek) = SECTION_START Then
                    Dim sectionLine As String = sr.ReadLine
                    res.Add(sectionLine.Substring(1, sectionLine.Length - 2))
                Else
                    sr.ReadLine()
                End If
            Loop
            sr.Close()
        Catch ex As Exception
            ExceptionService.handleException("Failed to load all ini sections.", ex)
        End Try
        Return res
    End Function

    Public Shared Function iniGetAllPairs(section As String, Optional path As String = "") As List(Of KeyValuePair(Of String, String))
        Dim iniFile As String = getIniFile(path)
        Dim keys As List(Of String) = iniGetAllKeys(section, iniFile)
        Dim vals As List(Of String) = iniGetAllValues(section, iniFile)

        Dim res As New List(Of KeyValuePair(Of String, String))
        If keys IsNot Nothing Then
            For i = 0 To keys.Count - 1
                res.Add(New KeyValuePair(Of String, String)(keys(i), vals(i)))
            Next
        End If
        Return res
    End Function


    Public Shared Function iniReadValue(section As String, key As String, Optional defaultvalue As String = "", Optional path As String = "", Optional bufferSize As Integer = 1024) As String
        Dim iniFile As String = getIniFile(path)
        Dim sTemp As String = Space(bufferSize)
        Dim Length As Integer = GetPrivateProfileString(section, key, defaultvalue, sTemp, bufferSize, iniFile)
        Return Left(sTemp, Length)
    End Function


    Public Shared Sub iniWriteValue(section As String, key As String, value As String, Optional path As String = "")
        Dim iniFile As String = getIniFile(path)
        WritePrivateProfileString(section, key, value, iniFile)
    End Sub

    Public Shared Sub iniRenameKey(section As String, key As String, value As String, Optional path As String = "")
        Dim iniFile As String = getIniFile(path)
        Dim keys As List(Of String) = iniGetAllKeys(section, iniFile)
        Dim nk(keys.Count - 1) As String
        keys.CopyTo(nk)
        Dim newKeys As List(Of String) = nk.ToList
        Dim vals As List(Of String) = iniGetAllValues(section, iniFile)
        For i = 0 To keys.Count - 1
            If keys(i).ToLower() = key.ToLower() Then
                newKeys(i) = value
                Exit For
            End If
        Next
        For i = 0 To keys.Count - 1
            iniDeleteKey(section, keys(i), iniFile)
        Next
        For i = 0 To newKeys.Count - 1
            iniWriteValue(section, newKeys(i), vals(i), iniFile)
        Next
    End Sub

    Public Shared Sub iniDeleteKey(section As String, key As String, Optional path As String = "")
        Dim iniFile As String = getIniFile(path)
        WritePrivateProfileString(section, key, Nothing, iniFile)
    End Sub

    Public Shared Sub iniDeleteSection(section As String, Optional path As String = "")
        Dim iniFile As String = getIniFile(path)
        DeletePrivateProfileSection(section, 0, 0, iniFile)
    End Sub

    Public Shared Function iniAppendRaw(value As String, Optional path As String = "") As Boolean
        Dim iniFile As String = getIniFile(path)
        Dim sw As StreamWriter = Nothing
        Try
            sw = New StreamWriter(iniFile, True, Encoding.Default)
            sw.Write(value)
        Catch ex As Exception
            Return False
        Finally
            If sw IsNot Nothing Then sw.Close()
        End Try
        Return True
    End Function

    <Obsolete("NotImplemented")>
    Public Shared Function iniInsertRaw(section As String, value As String, Optional path As String = "") As Boolean
        Dim iniFile As String = getIniFile(path)
        Dim s As String
        Try
            Dim sr As New StreamReader(iniFile, Encoding.Default)
            s = sr.ReadToEnd()
            sr.Close()
        Catch ex As Exception
            Return False
        End Try
        Dim pos As Integer = s.IndexOf(section)
        If pos = -1 Then
            Return iniAppendRaw(value, iniFile)
        Else
            ' TODO implement me
        End If
        Return True
    End Function
End Class
