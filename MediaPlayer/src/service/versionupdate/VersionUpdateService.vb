Imports System.IO
Imports System.IO.Compression
Public Module VersionUpdateService

    Public Sub checkForVersionUpdate()
        If My.Application.CommandLineArgs.Count > 0 Then
            Dim para As String = My.Application.CommandLineArgs(0)
            If para.StartsWith("up") Then
                install()
            End If
        End If

    End Sub


#Region "Player Update"
    Sub install()
        Utils.killProc(Utils.appName, True)
        Dim currPath As String = My.Application.Info.DirectoryPath
        Dim copyPath As String = ""
        For i = 1 To My.Application.CommandLineArgs.Count - 1
            copyPath &= My.Application.CommandLineArgs(i) & IIf(i = My.Application.CommandLineArgs.Count - 1, "", " ")
        Next
        MsgBox("Starting Installation...")
1:      Dim fils() As String = Nothing
        Try
            Dim sr As New StreamReader(currPath.Substring(0, currPath.LastIndexOf("\")) & "\releases")
            fils = sr.ReadToEnd().Split(";")
            sr.Close()
            For i = 0 To fils.Length - 1
                fils(i) = fils(i).Replace(";", "")
            Next
        Catch ex As Exception
            If MsgBox("Reading release manifest failed." & vbNewLine & vbNewLine &
                      currPath.Substring(0, currPath.LastIndexOf("\")) & "\releases" &
                      vbNewLine & vbNewLine & "Try again?", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation) = MsgBoxResult.Yes Then
                GoTo 1
            Else
                Environment.Exit(0)
            End If
        End Try

        If fils IsNot Nothing Then
            Dim archiveEntries As New List(Of List(Of ZipArchiveEntry))
            For i = 0 To fils.Length - 1
                If CStr(currPath & "\" & fils(i)).EndsWith(".zip") Then
                    archiveEntries.Add(getArchiveEntries(currPath & "\" & fils(i)))
                End If
            Next

            Dim fileList As New List(Of String)
            For Each archive In archiveEntries
                For Each entry In archive
                    fileList.Add(entry.FullName)
                Next
            Next

            For Each fil As String In fileList
                File.Delete(copyPath & "\" & fil)
                File.Copy(currPath & "\" & fil, copyPath & "\" & fil)
            Next

            Try
                Dim wr As New StreamWriter(copyPath & "\version", False)
                wr.Write(currPath.Substring(currPath.LastIndexOf("\") + 8))
                wr.Close()
            Catch ex As Exception
            End Try
            Process.Start(copyPath & "\mp3player.exe")
            Environment.Exit(0)
        Else
            MsgBox("Release manifest is corrupted.")
        End If

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

#End Region

End Module
