Imports System.IO
Imports MediaPlayer.SettingsEnums
Public Class Folder

    Public Shared root As Folder
    Public Shared top As Folder
    Public Shared folders As List(Of Folder)

    Public fullPath As String
    Public path As String
    Public name As String
    Public nodePath As String
    Public genre As Genre
    Public tracks As New List(Of Track)
    Public children As New List(Of Folder)
    Public isVirtual As Boolean
    Public isExcluded As Boolean

    Shared ReadOnly Property dll As Utils
        Get
            Return Form1.dll
        End Get
    End Property

    Public Sub New(ByVal fullPath As String)
        MyClass.fullPath = fullPath
        Dim last As Integer = Mid(fullPath, 1, fullPath.Length - 1).LastIndexOf("\")
        path = Mid(fullPath, 1, last + 1)
        name = Mid(fullPath, last + 2, fullPath.Length - last - 2)
        If Not root = Nothing Then
            If Not root.fullPath = fullPath Then
                nodePath = Mid(fullPath.Replace(root.fullPath, ""), 1, fullPath.Replace(root.fullPath, "").Length - IIf(fullPath(fullPath.Length - 1) = "\", 1, 0))
            End If
        End If
        updateGenre()
        isVirtual = Not IO.Directory.Exists(fullPath)
    End Sub

    Public Sub addFolder(ByVal fillTracks() As String)
        Folder.folders.Add(Me)
        Folder.folders(folders.Count - 1).tracks = New List(Of Track)
        For i = 0 To fillTracks.Count - 1
            Dim tr As Track = New Track(Form1, fillTracks(i), True)
            folders(folders.Count - 1).tracks.Add(tr)
            tr.addToPlaylist()
        Next
    End Sub

    Shared dialogMode As Boolean = False
    Public Sub invalidateFolderTracks(Optional ByVal subFolders As Boolean = True, Optional ByVal light As Boolean = False)
        If dialogMode Then Return
        tracks = New List(Of Track)
        Dim files As List(Of String)
        If isVirtual Then
            files = IniService.iniGetAllValues(fullPath, playlistPath)
        Else
            files = OperatingSystem.getAudioFiles(fullPath)
        End If
        If files IsNot Nothing Then
            Dim loadFails As New List(Of Track)
            For k = 0 To files.Count - 1
                If IO.File.Exists(files(k)) Then
                    Dim virtPath As String = ""
                    If isVirtual Then virtPath = fullPath & files(k).Substring(files(k).LastIndexOf("\") + 1)
                    Dim newTrack As New Track(Form1, files(k), light, virtPath)
                    If Not Me.containsTrack(newTrack) Then
                        tracks.Add(newTrack)
                    Else
                        loadFails.Add(newTrack)
                    End If

                Else
                    loadFails.Add(New Track(Form1, files(k), light, fullPath & files(k).Substring(files(k).LastIndexOf("\") + 1)))
                End If
            Next
            If loadFails.Count > 0 Then
                Dim ignore As Boolean = ignoresErrors()
                If Not ignore Then
                    dialogMode = True
                    If MsgBox(loadFails.Count & " out of " & files.Count & " tracks have an invalid source file." & vbNewLine & "Ignore this error?", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation) = MsgBoxResult.Yes Then
                        writeIgnoreError(True)
                    Else
                        TrackSelectionForm.selectTracks(Me, loadFails, TrackSelectionForm.eTrackSelectionMode.MANAGE, path, {"error"})
                    End If
                End If
                dialogMode = False
            End If
        End If

        If subFolders Then
            If children IsNot Nothing Then
                For i = 0 To children.Count - 1
                    If children(i) IsNot Nothing Then children(i).invalidateFolderTracks(, light)
                Next
            End If
        End If
    End Sub

    Function containsTrack(track As Track) As Boolean
        Return Not tracks.TrueForAll(Function(c)
                                         Return track.name.ToLower <> c.name.ToLower
                                     End Function)
    End Function

    Public Sub updateGenre()
        genre = Genre.getGenre(SettingsService.loadRawSetting(SettingsIdentifier.GENRES_MAPPING, fullPath))
        genre.addFolder(Me)
    End Sub

    Function folderStatsTotal() As Integer()
        Dim use As List(Of Track) = tracks
        If use.Count = 0 Then Return Nothing
        For i = 0 To use.Count - 1
            use(i).updatePopularity(True)
        Next
        Dim tot As Integer = 0
        Dim len As Integer = 0
        Dim pop As Integer = 0
        Dim c As Integer = 0
        Dim diff As Integer = Now.Subtract(dateLogStart).TotalDays
        Dim age As Integer = 0
        For i = 0 To use.Count - 1
            age += IIf(use(i).added = Nothing, Now.Subtract(New Date(2011, 4, 6)).TotalDays - 1, Now.Subtract(use(i).added).TotalDays)
            len += use(i).length
            c += use(i).count
            pop += use(i).popularity
            tot += use(i).count * use(i).length
        Next
        Return {use.Count, diff, tot, c, len, pop, age}
    End Function

    Function createSubPlaylist() As Folder
1:      Dim a As String = InputBox("Type in playlist name", , "My Playlist")
        If Not a = "" Then
            Dim exists As Integer = directoryOrVirtualExists(nodePath, a)
            If exists = 0 Then
                Dim newListPath As String = fullPath & a & "\"
                IniService.iniAppendRaw(vbNewLine & "[" & newListPath & "]", playlistPath)
                Dim newFolder As New Folder(newListPath)
                children.Add(newFolder)
                Return newFolder
            Else
                MsgBox(IIf(exists = 1, "Folder ", "Playlist ") & "with that name already exists." & vbNewLine & "Please choose another name...", MsgBoxStyle.Information)
                GoTo 1
            End If
        End If
        Return Nothing
    End Function

    Function createSubFolder() As Folder
        Dim parentExist As Integer = directoryOrVirtualExists(nodePath)
        If parentExist = 0 Then
            MsgBox("Parent folder [" & fullPath & "] does not exist.", MsgBoxStyle.Exclamation)
        ElseIf parentExist = 2 Then
            MsgBox("Creating a subfolder in a playlist is not possible. Try creating a playlist instead.", MsgBoxStyle.Exclamation)
        Else
1:          Dim a As String = InputBox("Type in folder name", , "New Folder")
            If Not a = "" Then
                Dim exists As Integer = directoryOrVirtualExists(nodePath, a)
                If exists = 0 Then
                    Dim newListPath As String = fullPath & a & "\"
                    ' dll.iniWriteValue(newListPath, "Create section", Now.ToShortDateString, playlistPath)
                    '  dll.iniDeleteKey(newListPath, "Create section", playlistPath)
                    Try
                        IO.Directory.CreateDirectory(newListPath)
                    Catch ex As Exception
                        MsgBox("Failed to create subfolder." & vbNewLine & ex.Message)
                        Return Nothing
                    End Try

                    Dim newFolder As New Folder(newListPath)
                    children.Add(newFolder)
                    Return newFolder
                Else
                    MsgBox(IIf(exists = 1, "Folder ", "Playlist ") & "with that name already exists." & vbNewLine & "Please choose another name...", MsgBoxStyle.Information)
                    GoTo 1
                End If
            End If
        End If

        Return Nothing
    End Function
    Public Function delete(Optional promptConfirm As Boolean = True) As Boolean
        If Me.Equals(top) Then
            MsgBox("No permission to delete the top level directory.", MsgBoxStyle.Exclamation)
            Return False
        End If
        If isVirtual Then
            If promptConfirm AndAlso MsgBox("Are you sure to remove this playlist folder with all its contents?" & vbNewLine & "Source files will not be deleted.", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation) = MsgBoxResult.Yes Then
                Try
                    Dim folders As List(Of Folder) = Folder.getVirtualFolders()
                    For Each f As Folder In folders
                        If f.fullPath.StartsWith(fullPath) Then
                            IniService.iniDeleteSection(f.fullPath, playlistPath)
                            Folder.invalidateFolders(Folder.top)
                        End If
                    Next
                    Return True
                Catch ex As Exception
                    MsgBox(ex.Message, MsgBoxStyle.Exclamation)
                End Try
            End If
        Else
            If promptConfirm AndAlso MsgBox("Are you sure to permanently delete this folder and all subfolders with all their contents?", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation) = MsgBoxResult.Yes Then
                Try
                    Dim folders As List(Of Folder) = Folder.getVirtualFolders()
                    For Each f As Folder In folders
                        If f.fullPath.StartsWith(fullPath) Then
                            IniService.iniDeleteSection(f.fullPath, playlistPath)
                        End If
                    Next
                    Try
                        IO.Directory.Delete(fullPath, True)
                        Folder.folders.Remove(Me)
                    Catch ex As Exception
                        Folder.invalidateFolders(Folder.top)
                        MsgBox("Failed to delete folder. Please try again or delete the folder manually.")
                        Return False
                    End Try
                    Folder.invalidateFolders(Folder.top)
                    Return True
                Catch ex As Exception
                    MsgBox(ex.Message, MsgBoxStyle.Exclamation)
                End Try
            End If
        End If
        Return False
    End Function


    Public Function hide() As Boolean
        isExcluded = True
        writeExcludedFolders()
        Form1.localfill()
        Return True
    End Function

    Sub convertToFolder()
        If Not IO.Directory.Exists(New IO.DirectoryInfo(fullPath).Parent.FullName) Then
            MsgBox("Playlist cannot be converted." & vbNewLine & "Reason: Parent folder is a playlist", MsgBoxStyle.Exclamation)
            Return
        End If
        Form1.fsw.EnableRaisingEvents = False
        If IO.Directory.Exists(fullPath) Then
            If MsgBox("Folder already exists. Files may get overwritten. Continue?", MsgBoxStyle.OkCancel + MsgBoxStyle.Information) = MsgBoxResult.Cancel Then Return
        Else
            IO.Directory.CreateDirectory(fullPath)
        End If
1:      Try
            Form1.Cursor = Cursors.WaitCursor
            For Each t As Track In tracks
                IO.File.Copy(t.fullPath, fullPath & t.name & t.ext, True)
            Next
        Catch ex As Exception
            If MsgBox("Failed to copy source files. Try again?", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation) = MsgBoxResult.Yes Then
                GoTo 1
            Else
                Form1.fsw.EnableRaisingEvents = True
                Return
            End If
        Finally
            Form1.Cursor = Cursors.Default
        End Try

        IniService.iniDeleteSection(fullPath, playlistPath)

        Folder.invalidateFolders(Folder.top)
        Form1.tv_refill()

        Form1.fsw.EnableRaisingEvents = True
    End Sub

    Function convertToPlaylist() As Boolean
        Dim subs() As String = dll.GetAllDirectories(fullPath, True)
        If subs IsNot Nothing Then 'no non-virtual subfolders 
            MsgBox("Folder cannot be converted to playlist." & vbNewLine & "Reason: Folder contains subfolders", MsgBoxStyle.Exclamation)
            Return False
        End If
        If IniService.iniIsValidSection(fullPath, playlistPath) Then
            MsgBox("Playlist already exists (currently shadowed by folder)", MsgBoxStyle.Exclamation)
            Return False
        End If

        Dim extDir As String = ""

        'transfer source tracks to other folder 
1:      If MsgBox("Do you want the relocation of source tracks to be handled automatically?", MsgBoxStyle.YesNo + MsgBoxStyle.Information) = MsgBoxResult.Yes Then
            extDir = Folder.top.fullPath & "External Source\"
        Else
            Dim selNode As Folder = NodeSelectionForm.selectNode("Transfer source tracks to...", {"virtual"})
            If selNode Is Nothing OrElse selNode.nodePath = nodePath Then Return False
            extDir = selNode.fullPath
        End If
        Form1.fsw.EnableRaisingEvents = False
        If Not Directory.Exists(extDir) Then Directory.CreateDirectory(extDir)
        Try
            Dim overwriteFlag As Boolean = (MsgBox("Overwrite existing files in target directory?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes)
            For Each t As Track In tracks
                Dim targetFullpath As String = extDir & t.name & t.ext
                If overwriteFlag Or Not File.Exists(targetFullpath) Then File.Copy(t.fullPath, targetFullpath, True)
            Next
        Catch ex As Exception
            If MsgBox("Failed to copy source tracks." & vbNewLine & "Try again?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                GoTo 1
            Else
                Form1.fsw.EnableRaisingEvents = True
                Return False
            End If
        End Try

        'create playlist 
2:      Dim playlistCreateFail As Boolean = False
        Dim writeContent As String = ""
        Dim sw As StreamWriter = Nothing
        Try
            writeContent = vbNewLine & "[" & fullPath & "]"
            For Each t As Track In tracks
                writeContent &= vbNewLine & t.name & "=" & extDir & t.name & t.ext
            Next
            sw = New StreamWriter(playlistPath, True, System.Text.Encoding.Default)
            sw.Write(writeContent)

        Catch ex As Exception
            playlistCreateFail = True
            If MsgBox("An I/O exception occured. Failed to create playlist. Try again?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                If sw IsNot Nothing Then sw.Close()
                GoTo 2
            Else
                Form1.fsw.EnableRaisingEvents = True
                Return False
            End If
        Finally
            If sw IsNot Nothing Then sw.Close()
        End Try


        'delete folder
        Dim deleteFail As Boolean = False
        Try
            Form1.Cursor = Cursors.WaitCursor
            If PlayerInterface.currTrack IsNot Nothing Then
                If PlayerInterface.currTrack.fullPath.StartsWith(fullPath) Then Player.resetUrl()
            End If
            IO.Directory.Delete(fullPath, True)
            For i = PlayerInterface.playlist.Count - 1 To 0 Step -1
                If Not IO.File.Exists(PlayerInterface.playlist(i).fullPath) Then
                    PlayerInterface.playlist(i).removeFromPlaylist()
                End If
            Next
            Form1.Cursor = Cursors.Default
        Catch ex As Exception
            deleteFail = True
            Form1.Cursor = Cursors.Default
            MsgBox("Failed to delete directory." & vbNewLine & "A file may be used by another process.", MsgBoxStyle.Critical)
        End Try



        If deleteFail Then
            MsgBox("Playlist created. Delete the folder manually to complete the conversion", MsgBoxStyle.Exclamation)
        Else
            Folder.invalidateFolders(Folder.top)
            Form1.tv_refill()
        End If

        Form1.fsw.EnableRaisingEvents = True
        Return True
    End Function

    Public Function ignoresErrors() As Boolean
        Dim ignoreRaw As String = SettingsService.loadSetting(SettingsIdentifier.IGNORE_ERRORS)
        Dim vals() = ignoreRaw.Split(";")
        If vals IsNot Nothing Then
            For Each s As String In vals
                If s.ToLower = fullPath.ToLower Then Return True
            Next
        End If
        Return False
    End Function

    Public Sub writeIgnoreError(ignoreState As Boolean)
        Dim curr As String = SettingsService.getSetting(SettingsIdentifier.IGNORE_ERRORS)
        Dim entries() As String = curr.Split(";")
        If entries IsNot Nothing AndAlso entries.Count > 0 AndAlso entries(0) <> "" Then
            If Not entries.Contains(fullPath) Then
                If ignoreState = True Then curr &= ";" & fullPath
            Else
                If Not ignoreState Then
                    curr = ""
                    entries(Array.IndexOf(entries, fullPath)) = ""
                    For i = 0 To entries.Length - 1
                        curr &= entries(i) & IIf(i < entries.Length - 1 And entries(i) <> "", ";", "")
                    Next
                End If
            End If
        Else
            curr = fullPath
        End If
        SettingsService.saveSetting(SettingsIdentifier.IGNORE_ERRORS, curr)
    End Sub

    '#############SHARED SECTION######################

    Public Shared Sub invalidateFolders(ByVal path As Folder, Optional includeVirtual As Boolean = True)
        If IO.Directory.Exists(path.fullPath) Then
            folders = New List(Of Folder)
            Folder.top = New Folder(SettingsService.path)
            Form1.Cursor = Cursors.WaitCursor
            ' folders = Await AsyncTask.executeTask(Form1, AsyncTask.getAllFolders(path, excludeFolders, includeExcluded))
            folders = getAllFolders(Folder.top, includeVirtual)
            Form1.Cursor = Cursors.Default
        End If
    End Sub

    Public Shared Sub setTopFolder(ByVal fullPath As String)
        Dim s As String = fullPath.Substring(0, fullPath.Length - 1)
        Dim l As Integer = fullPath.LastIndexOf("\")
        Dim f As Integer = fullPath.IndexOf("\")
        If l = f Then
            root = New Folder(fullPath)
        Else
            root = New Folder(s.Substring(0, s.LastIndexOf("\") + 1))
        End If
        Form1.root = root.fullPath
        top = New Folder(fullPath)
        setSetting(SettingsIdentifier.PATH, fullPath)
        'Form1.path = fullPath
        If Not IO.Directory.Exists(fullPath) Then IO.Directory.CreateDirectory(fullPath)
    End Sub

    Shared Function getAllFolders(ByVal folder As Folder, Optional includeVirtual As Boolean = False) As List(Of Folder)
        Dim res As New List(Of Folder) From {folder}

        If includeVirtual Then
            Dim lists As List(Of String) = IniService.iniGetAllSections(playlistPath)
            For i = 0 To lists.Count - 1
                If New Folder(lists(i)).path.ToLower = folder.fullPath.ToLower Then
                    If Not IO.Directory.Exists(lists(i)) Then 'prefer real folder over virtual
                        getAllFoldersHelper(folder, lists(i))
                    End If
                End If
            Next
        End If

        If Not folder.isVirtual Then
            If IO.Directory.Exists(folder.fullPath) Then
                For Each dir As String In My.Computer.FileSystem.GetDirectories(folder.fullPath)
                    Dim dirInfo As New IO.DirectoryInfo(dir)
                    If Not dir.Substring(dir.LastIndexOf("\") + 1).StartsWith(".") Then
                        Dim attr As List(Of Integer) = dll.getBinaryComponents(dirInfo.Attributes)
                        If Not attr.Contains(2) And Not attr.Contains(4) And Not attr.Contains(1024) Then
                            getAllFoldersHelper(folder, dir)
                        End If
                    End If
                Next
            End If

        End If

        If Not folder.isExcluded Then
            If folder.children IsNot Nothing Then
                For Each c As Folder In folder.children
                    res.AddRange(getAllFolders(c, includeVirtual))
                Next
            End If
        End If

        Return res
    End Function

    Private Shared Sub getAllFoldersHelper(ByRef folder As Folder, dir As String)
        If Not dir.EndsWith("\") Then dir &= "\"

        Dim fol As Folder = getFolder(dir)
        If fol Is Nothing Then fol = New Folder(dir)

        Dim exclRaw As String = SettingsService.getSetting(SettingsIdentifier.EXCLUDED_FOLDERS)
        Dim exclStr() As String = exclRaw.Split(";")
        If exclStr IsNot Nothing AndAlso exclStr.Contains(dir) Then
            fol.isExcluded = True
        End If

        folder.children.Add(fol)
    End Sub

    Shared Function getSelectedFolder(tv As TreeView) As Folder
        Dim rootFill As String = IIf(Folder.top.path = "" And Folder.top.fullPath = Folder.top.fullPath, "", Folder.root.fullPath)
        If tv.SelectedNode Is Nothing Then Return Nothing
        For i = 0 To folders.Count - 1
            If folders(i).fullPath = rootFill & tv.SelectedNode.FullPath & "\" Then Return folders(i)
        Next
        Return Nothing
    End Function

    Public Shared Function getFolder(ByVal path As String, Optional ByVal newIfNull As Boolean = False) As Folder
        If Not path.EndsWith("\") Then path &= "\"
        For i = 0 To folders.Count - 1
            If folders(i).fullPath.ToLower = path.ToLower Or folders(i).nodePath.ToLower & "\" = path.ToLower Then Return folders(i)
        Next
        If newIfNull Then Return New Folder(path)
        Return Nothing
    End Function

    Public Shared Sub writeExcludedFolders()
        Dim res As String = ""
        For Each f As Folder In getExcludedFolders()
            res &= f.fullPath & ";"
        Next
        If res.EndsWith(";") Then res = res.Substring(0, res.Length - 1)
        SettingsService.saveSetting(SettingsIdentifier.EXCLUDED_FOLDERS, res)
    End Sub



    Public Shared Function getVirtualFolders() As List(Of Folder)
        Dim res As New List(Of Folder)
        For Each f As Folder In folders
            If f.isVirtual Then res.Add(f)
        Next
        Return res
    End Function

    Public Shared Function getExcludedFolders() As List(Of Folder)
        Dim res As New List(Of Folder)
        For Each f As Folder In folders
            If f.isExcluded Then res.Add(f)
        Next
        Return res
    End Function

    Public Shared Function getFolders(Optional virtual As Boolean = True, Optional excluded As Boolean = True) As List(Of Folder)
        Dim res As New List(Of Folder)
        If Not radioEnabled Then
            For Each f As Folder In folders
                If (Not f.isExcluded Or excluded) And (Not f.isVirtual Or virtual) Then
                    res.Add(f)
                End If
            Next
        End If
        Return res
    End Function

    Public Shared Function directoryOrVirtualExists(baseNodePath As String, subDir As String) As Integer
        If Not baseNodePath.EndsWith("\") Then baseNodePath &= "\"
        If Not IO.Directory.Exists(root.fullPath & baseNodePath & subDir) Then
            Dim virts As List(Of Folder) = getVirtualFolders()
            If virts.TrueForAll(Function(c) As Boolean
                                    Return baseNodePath & subDir <> c.nodePath
                                End Function) Then
                Return 0 'neither
            End If
        Else
            Return 1 'folder
        End If
        Return 2 'playlist
    End Function

    Public Shared Function directoryOrVirtualExists(nodePath As String) As Integer
        If Not nodePath.EndsWith("\") Then nodePath &= "\"
        If Not IO.Directory.Exists(root.fullPath & nodePath) Then
            Dim virts As List(Of Folder) = getVirtualFolders()
            If virts.TrueForAll(Function(c) As Boolean
                                    Return nodePath <> c.nodePath & "\"
                                End Function) Then
                Return 0 'neither
            End If
        Else
            Return 1 'folder
        End If
        Return 2 'playlist
    End Function

    Overrides Function ToString() As String
        If Form1.searchState > PlayerEnums.SearchState.NONE Then
            Return name & " - [" & nodePath & "]"
        ElseIf optionsMode Or NodeSelectionForm.Visible Then
            Return nodePath
        Else
            Return name
        End If
    End Function

    Shared Operator =(ByVal a As Folder, ByVal b As Folder) As Boolean
        If a Is Nothing And b Is Nothing Then Return True
        If a Is Nothing Xor b Is Nothing Then Return False
        If a.Equals(b) Then Return True
        Return False
    End Operator
    Shared Operator <>(ByVal a As Folder, ByVal b As Folder) As Boolean
        If a Is Nothing And b Is Nothing Then Return False
        If a Is Nothing Xor b Is Nothing Then Return True
        If a.Equals(b) Then Return False
        Return True
    End Operator

End Class
