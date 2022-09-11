Imports System.Runtime.InteropServices
Imports System.Text

Public Class OperatingSystem

    <StructLayout(LayoutKind.Sequential)>
    Public Structure COPYDATASTRUCT
        Public dwData As IntPtr
        Public cbData As Integer
        Public lpData As String
    End Structure

    Public Declare Function GetWindow Lib "user32.dll" (hWnd As IntPtr, uCmd As Integer) As IntPtr
    Public Declare Function FindWindow Lib "user32.dll" Alias "FindWindowA" (lpClassName As String, lpWindowName As String) As IntPtr
    Public Declare Function SendMessageHM Lib "user32.dll" Alias "SendMessageA" (hWnd As IntPtr, wMsg As Int32, wParam As Int32, lParam As StringBuilder) As Int32

#Region "Dialogs"
    Public Shared Function getExactFileDialog(name As String, ext As String, Optional initDir As String = "") As String
        Return getFileDialog(initDir, name & ext)
    End Function
    Public Shared Function getFileDialog(Optional initDir As String = "", Optional extFilter As String = "") As String
        Dim op As OpenFileDialog = getCommonFileDialog(initDir, extFilter, False)
        If op.ShowDialog() = DialogResult.OK AndAlso Not op.FileName = "" Then
            Return op.FileName
        End If
        Return ""
    End Function
    Public Shared Function getFilesDialog(Optional initDir As String = "", Optional extFilter As String = "") As String()
        Dim op As OpenFileDialog = getCommonFileDialog(initDir, extFilter, True)
        If op.ShowDialog() = DialogResult.Cancel Then Return Nothing
        Return op.FileNames
    End Function
    Public Shared Function getAudioFilesDialog(Optional initDir As String = "") As String()
        Return getFilesDialog(initDir, "Audio files|*.mp3;*.wav;*.m4a;*.flac;*.mp3;*.aac")
    End Function

    Private Shared Function getCommonFileDialog(initDir As String, filter As String, multiSelect As Boolean) As OpenFileDialog
        Dim op As New OpenFileDialog
        op.Multiselect = multiSelect
        op.CheckFileExists = True
        If Not initDir = "" Then
            Try
                Do
                    initDir = initDir.Substring(0, initDir.LastIndexOf("\"))
                Loop Until initDir.Count(Function(c) c = "\") <= 1 Or IO.Directory.Exists(initDir)
                op.InitialDirectory = initDir
            Catch ex As Exception
            End Try
        End If
        If filter <> "" Then op.Filter = "(" & filter & ")|" & filter
        Return op
    End Function

    Public Shared Function getDirectoryDialog(Optional initDir As String = "") As String
        Dim op As New FolderBrowserDialog
        op.ShowNewFolderButton = True
        If Not initDir = "" Then
            Try
                op.SelectedPath = initDir
            Catch ex As Exception
            End Try
        End If
        If op.ShowDialog() = DialogResult.OK AndAlso Not op.SelectedPath = "" Then
            Return op.SelectedPath & IIf(op.SelectedPath.EndsWith("\"), "", "\")
        End If
        Return ""
    End Function
#End Region 'DIALOGS

    Public Shared Function getAudioFiles(path As String) As List(Of String)
        Dim res As New List(Of String)
        If IO.Directory.Exists(path) Then
            For Each fil As String In My.Computer.FileSystem.GetFiles(path)
                If Utils.hasAudioExt(fil) Then
                    res.Add(fil)
                End If
            Next
        End If
        Return res
    End Function



    Public Shared Function isValidDirectoryPath(ByVal s As String) As Boolean
        Return isValidFileOrDirectoryPath(s) AndAlso s.EndsWith("\")
    End Function
    Public Shared Function isValidFilePath(s As String) As Boolean
        Return isValidFileOrDirectoryPath(s) AndAlso Not s.EndsWith("\") AndAlso Not s.EndsWith(".")
    End Function
    Private Shared Function isValidFileOrDirectoryPath(s As String) As Boolean
        For Each badChar As Char In System.IO.Path.GetInvalidPathChars
            If InStr(s, badChar) > 0 Then
                Return False
            End If
        Next
        Return s.Length > 3 AndAlso Not s.Contains("\\") AndAlso Not s.EndsWith(" ") AndAlso Not s.StartsWith(" ") AndAlso s.Substring(1, 2) = ":\"
    End Function


    Public Shared Function isProcessAlive(name As String) As Boolean
        For Each p As Process In Process.GetProcessesByName(name)
            Return True
        Next
        Return False
    End Function
End Class
