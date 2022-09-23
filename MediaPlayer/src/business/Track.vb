Imports MediaPlayer.SettingsEnums
Public Class Track

    Shared ReadOnly Property dll As Utils
        Get
            Return formhandle.dll
        End Get
    End Property
    Shared ReadOnly Property root As String
        Get
            Return formhandle.root
        End Get
    End Property

    Shared ReadOnly Property currTrack As Track
        Get
            Return PlayerInterface.currTrack
        End Get
    End Property
    Shared ReadOnly Property playlist As List(Of Track)
        Get
            If PlayerInterface.playlist Is Nothing Then Return Nothing
            Return PlayerInterface.playlist
        End Get
    End Property
    Shared ReadOnly Property l2_2 As ListBox
        Get
            Return formhandle.l2_2
        End Get
    End Property
    Shared ReadOnly Property gldt As List(Of String)
        Get
            Return PlayerInterface.gldt
        End Get
    End Property
    Shared ReadOnly Property glnames As List(Of String)
        Get
            Return PlayerInterface.glnames
        End Get
    End Property

    Public ReadOnly Property isVirtual() As Boolean
        Get
            Return fullPath <> virtualPath
        End Get
    End Property




    Public Shared formhandle As Form1

    Public fullPath As String
    Public path As String
    Public dir As String
    Public name As String
    Public ext As String
    Public genre As Genre
    Public count As Integer = 0
    Public length As Double = 0
    Public added As Date = Nothing
    Public popularity As Integer
    Public partsCount As Integer = 0
    Public parts As New List(Of TrackPart)
    Public currPart As TrackPart
    Public playlistIndex As Integer = -1
    Public virtualPath As String
    Public locations As New List(Of Folder)

    Public Sub New(handle As Form1, ByVal fullPath As String, Optional ByVal light As Boolean = False, Optional virtPath As String = "")
        formhandle = handle
        MyClass.fullPath = fullPath
        virtualPath = IIf(virtPath = "", fullPath, virtPath)
        path = Mid(virtualPath, 1, virtualPath.LastIndexOf("\") + 1)
        genre = Genre.Undefined
        If path = root Then
            dir = root
        ElseIf path.StartsWith(root) Then
            dir = Mid(virtualPath, root.Length + 1, virtualPath.LastIndexOf("\") - root.Length)
        Else
            dir = path
        End If
        If virtualPath.Contains(".") Then
            name = Mid(virtualPath, virtualPath.LastIndexOf("\") + 2, virtualPath.LastIndexOf(".") - virtualPath.LastIndexOf("\") - 1)
            ext = Mid(virtualPath, virtualPath.LastIndexOf(".") + 1)
        Else
            name = virtualPath.Substring(virtualPath.LastIndexOf("\") + 1)
            ext = ""
        End If

        If Not light Then
            updateStats()
        End If
    End Sub
    Sub updateStats(Optional ByVal reloadDate As Boolean = False)
        updateCount()
        updateLength()
        updateDate(reloadDate)
        updateParts()
        updateGenre()
        updatePopularity(False)
    End Sub
    Sub invalidateStats()
        updateCount()
        invalidateLength()
        updateDate(True)
        updateParts()
        updateGenre()
        updatePopularity(False)
    End Sub

    Sub updatePopularity(ByVal subUpdates As Boolean)
        If subUpdates Then
            updateDate()
            updateCount()
            updateLength()
        End If
        Dim diff As Integer = dll.GetDayDiff(IIf(added = Nothing, New Date(2011, 4, 6), added), Date.Today)
        Try
            If diff > 0 Then popularity = (count * length) / diff
        Catch ex As Exception
            popularity = 0
        End Try
    End Sub
    Sub updateCount()
        count = loadRawSetting(SettingsIdentifier.TRACKS_COUNT, name)
    End Sub
    Sub updateLength()
        length = loadRawSetting(SettingsIdentifier.TRACKS_TIME, name)
    End Sub
    Public Sub invalidateLength()
        length = Player.newMedia(fullPath).duration
    End Sub
    Sub updateDate(Optional ByVal reload As Boolean = False)
        Dim dt As String = formhandle.getDate(name, reload)
        If dt = "" Then
            added = Nothing
        Else
            added = CDate(dt)
        End If
    End Sub
    Sub updateParts()
        partsCount = getPartCount()
        Dim p As String = lyrpath & name & ".ini"
        If partsCount > 0 Then
            parts = New List(Of TrackPart)
            For i = 0 To partsCount - 1
                Dim timeData As String = IniService.iniReadValue(i + 1, "time", "0,0", p)
                Dim nameData As String = IniService.iniReadValue(i + 1, "name", "", p)
                parts.Add(New TrackPart(formhandle, Me, i, {timeData, nameData}))
            Next
        End If
    End Sub

    Sub updateGenre()
        Dim temp As Genre = Genre.getGenre(loadRawSetting(SettingsIdentifier.GENRES_MAPPING, name))
        If temp.Equals(Genre.Undefined) Then
            updateLocations()
            For Each folder As Folder In locations
                If Not folder.genre.Equals(Genre.Undefined) Then
                    temp = folder.genre
                    Exit For
                End If
            Next
        End If
        genre = temp
    End Sub

    Public Sub updateLocations()
        locations = getLocations(True)
    End Sub

    Function getLocations(Optional ByVal searchHidden As Boolean = False) As List(Of Folder)
        Dim res As New List(Of Folder)
        For Each fol As Folder In Folder.folders
            If Not fol.isExcluded Or searchHidden Then
                For k = 0 To fol.tracks.Count - 1
                    If fol.tracks(k).name = name Then
                        res.Add(fol)
                        Exit For
                    End If
                Next
            End If
        Next
        Return res
    End Function


    Public Function getFolder() As Folder
        For Each fol As Folder In Folder.folders
            If fol.containsTrack(Me) Then Return fol
        Next
        Return Nothing
    End Function

    Public Sub setDate()
        If Not IO.File.Exists(logpath) Then
            formhandle.showOptions(OptionsForm.optionState.PATHS)
        Else
            updateDate(True)
            Dim creationDate As String = IO.File.GetCreationTime(fullPath).ToShortDateString
            Dim modiDate As String = IO.File.GetLastWriteTime(fullPath).ToShortDateString
            Dim a As String = InputBox("Type in date of track aquirement." & vbNewLine & vbNewLine & "File creation date: " & creationDate &
                                         vbNewLine & "File modifying date: " & modiDate, ,
                                       IIf(added = Nothing, "", added.ToShortDateString()))
            If Not a = "" Then
                Dim dt As Date = Nothing
                If Date.TryParse(a, dt) Then
                    Dim wInd As Integer = getLogDateIndex()
                    IniService.iniWriteValue(IniSection.LOG_DATE, wInd, dt.ToShortDateString & "  -  " & name, logpath)
                    formhandle.labelStatsUpdate()
                Else
                    MsgBox("Invalid date Format. DD.MM.YYYY")
                End If
            Else
                If Not added = Nothing Then
                    If MsgBox("Delete saved date?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        Dim wInd As Integer = getLogDateIndex()
                        Dim vals As List(Of String) = IniService.iniGetAllValues(IniSection.LOG_DATE, logpath)
                        If vals.Count > 0 Then
                            For i = wInd To vals.Count - 1
                                IniService.iniWriteValue(IniSection.LOG_DATE, i, vals(i), logpath)
                            Next
                            IniService.iniDeleteKey(IniSection.LOG_DATE, vals.Count, logpath)
                            formhandle.labelStatsUpdate()
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Function getLogDateIndex() As Integer '1-based index
        If added <> Nothing Then
            If gldt IsNot Nothing Then
                For i = 0 To gldt.Count - 1
                    If gldt.Item(i) = added.ToShortDateString() AndAlso glnames.Item(i) = name Then
                        Return i + 2
                    End If
                Next
            End If
        End If
        Return IIf(gldt Is Nothing, 1, gldt.Count + 2)
    End Function

    Public Function getPartCount() As Integer
        Dim ini As String = lyrpath & name & ".ini"
        If IO.File.Exists(ini) = False Then
            Return 0
            ' ElseIf Not dll.iniIsValidKey("1", "time", ini) Then
            '    Return 0
        Else
            Dim n As Integer = 0
            Do Until Not IniService.iniIsValidKey(n + 1, "time", ini)
                n += 1
            Loop
            Return n
        End If
    End Function

    Function getCurrentPart(Optional ByVal timeint As Integer = -1) As TrackPart
        If timeint = -1 Then timeint = Int(Player.getCurrentPosition())
        Dim curr As Integer = partsCount
        If parts.Count = 0 Then Return Nothing
        For i = parts.Count - 1 To 0 Step -1
            If timeint >= parts(i).fromSec Then
                Return parts(i)
            End If
        Next
        Return parts(0)
    End Function

    Function getTrackPartName(Optional ByVal part As TrackPart = Nothing) As String
        Dim p As TrackPart = IIf(part Is Nothing, getCurrentPart(), part)
        If p IsNot Nothing Then
            Return p.name
        End If
        Return ""
    End Function

    Public Sub nextPart()
        If currPart.id < partsCount - 1 Then
            currPart = parts(currPart.id + 1)
        End If
    End Sub

    Public Sub prevPart()
        If currPart.id > 0 Then
            currPart = parts(currPart.id - 1)
        End If
    End Sub

    Public Sub addToPlaylist(Optional ByVal index As Integer = -1)
        Dim removedIndex As Integer = removeFromPlaylist(name)
        If index = -1 Then
            playlist.Add(Me)
            playlistIndex = PlayerInterface.playlist.Count - 1
            insertToList()
        Else
            If removedIndex > -1 And index > removedIndex Then index -= 1
            For i = index To playlist.Count - 1
                playlist(i).playlistIndex += 1
            Next
            playlist.Insert(index, Me)
            playlistIndex = index
            insertToList(index)
        End If
    End Sub


    Public Sub playNext()
        If Not currTrack = Nothing Then
            currTrack.selectPlaylist()
            addToPlaylist(PlayerInterface.playlistContains(currTrack) + 1)
        Else
            playlist(0).selectPlaylist()
            addToPlaylist(1)
        End If
    End Sub

    Public Sub selectPlaylist()
        If playlist.Contains(Me) Then
            l2_2.SelectedIndex = playlistIndex
        Else
            For i = playlist.Count - 1 To 0 Step -1
                If playlist(i).name.ToLower = name.ToLower Then
                    l2_2.SelectedIndex = i
                    GoTo 1
                End If
            Next
            playlist.Add(Me)
            playlistIndex = playlist.Count - 1
            insertToList()
            l2_2.SelectedIndex = l2_2.Items.Count - 1
        End If
1:      formhandle.l2.SelectedIndex = -1
    End Sub

    Sub insertToList(Optional index As Integer = -1)
        Dim tempState As PlayerEnums.SearchState = Form1.searchState
        Form1.searchState = PlayerEnums.SearchState.NONE
        If index = -1 Then
            l2_2.Items.Add(Me)
        Else
            l2_2.Items.Insert(index, Me)
        End If
        Form1.searchState = tempState
    End Sub

    Public Sub play()
        selectPlaylist()
        l2_2.SelectedItem = Me
        PlayerInterface.launchTrack(Me)
        PlayerInterface.last = Me
    End Sub

    Public Function removeFromPlaylist() As Integer
        If playlistIndex > -1 Then
            For i = playlistIndex + 1 To playlist.Count - 1
                playlist(i).playlistIndex -= 1
            Next
            playlist.RemoveAt(playlistIndex)
            formhandle.l2_2.Items.RemoveAt(playlistIndex)
            Dim ret As Integer = playlistIndex
            playlistIndex = -1
            Return ret
        End If
        Return -1
    End Function

    Public Function removeFromPlaylist(ByVal byName As String) As Integer
        For i = playlist.Count - 1 To 0 Step -1
            If playlist(i).name.ToLower = byName.ToLower Then
                Return playlist(i).removeFromPlaylist()
            End If
        Next
        Return -1
    End Function



    Public Function getPlaylistIndex() As Integer
        For i = 0 To playlist.Count - 1
            If playlist(i).name.ToLower = name.ToLower Then
                Return i
            End If
        Next
        Return -1
    End Function


    Sub copyToVirtualFolder(fol As Folder)
        If Not fol = Nothing Then
            If Not IniService.iniIsValidKey(fol.fullPath, name, playlistPath) OrElse MsgBox("Track already in playlist. Overwrite?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                IniService.iniWriteValue(fol.fullPath, name, fullPath, playlistPath)
                fol.tracks.Add(New Track(formhandle, fullPath, False, fol.fullPath & name & ext))
            End If
        End If
    End Sub
    Sub moveToVirtualFolder(fol As Folder)
        Dim source As Folder = Folder.getFolder(path)
        If Not fol = Nothing AndAlso source IsNot Nothing Then
            If Not IniService.iniIsValidKey(fol.fullPath, name, playlistPath) OrElse MsgBox("Track already in playlist. Overwrite?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                IniService.iniWriteValue(fol.fullPath, name, fullPath, playlistPath)
                fol.tracks.Add(New Track(formhandle, fullPath, False, fol.fullPath & name & ext))
                If source.isVirtual Then
                    IniService.iniDeleteKey(path, name, playlistPath)
                Else
                    Try
                        'IO.File.Delete(fullPath)
                    Catch ex As Exception
                    End Try
                End If
                source.tracks.Remove(Me)
            End If
        End If
    End Sub

    Public Sub virtualDelete()
        Dim fol As Folder = Folder.getFolder(path)
        If Not fol = Nothing Then
            IniService.iniDeleteKey(fol.fullPath, name, playlistPath)
            Form1.listRemove(formhandle.l2, Me)
            If fol = Folder.getSelectedFolder(Form1.tv) Then
                fol.invalidateFolderTracks(False)
                If Player.getPlayState() = WMPLib.WMPPlayState.wmppsPlaying Then
                    If PlayerInterface.currTrack.name = name Then
                        PlayerInterface.playNextTrack()
                        removeFromPlaylist()
                    End If
                End If
            End If
        End If
    End Sub


    Shared Sub invalidateTracks(Optional ByVal light As Boolean = False)
        If IO.Directory.Exists(Folder.top.fullPath) And Folder.folders.Count > 0 Then
            For i = 0 To Folder.folders.Count - 1
                Folder.folders(i).tracks = New List(Of Track)
                Dim files As New List(Of String)
                If Folder.folders(i).isVirtual Then
                    If formhandle Is Nothing Then
                        formhandle = Form1
                    End If
                    files = IniService.iniGetAllValues(Folder.folders(i).fullPath, playlistPath)
                Else
                    If IO.Directory.Exists(Folder.folders(i).fullPath) Then
                        files = OperatingSystem.getAudioFiles(Folder.folders(i).fullPath)
                    End If
                End If
                For k = 0 To files.Count - 1
                    Dim virtPath As String = ""
                    If Folder.folders(i).isVirtual Then
                        virtPath = Folder.folders(i).fullPath & files(k).Substring(files(k).LastIndexOf("\") + 1)
                    End If
                    Dim newTrack As Track = New Track(Form1, files(k), light, virtPath)
                    Folder.folders(i).tracks.Add(newTrack)
                Next

            Next
        End If
        'tracks unique, assign several folders
    End Sub

    Public Shared Function getTrack(ByVal fullPath As String) As Track
        If Folder.folders IsNot Nothing And fullPath <> "" Then
            If fullPath.StartsWith(Folder.top.fullPath) Then
                For i = 0 To Folder.folders.Count - 1
                    If Folder.folders(i).fullPath = Mid(fullPath, 1, fullPath.LastIndexOf("\") + 1) Then
                        If Folder.folders(i).tracks IsNot Nothing Then
                            For k = 0 To Folder.folders(i).tracks.Count - 1
                                If Folder.folders(i).tracks(k).fullPath = fullPath Or Folder.folders(i).tracks(k).virtualPath = fullPath Then Return Folder.folders(i).tracks(k)
                            Next
                        End If
                    End If
                Next
            Else
                Return getFirstTrack(fullPath.Substring(fullPath.LastIndexOf("\") + 1, fullPath.LastIndexOf(".") - fullPath.LastIndexOf("\") - 1), fullPath.Substring(fullPath.LastIndexOf(".")))
            End If
        End If
        Return Nothing
    End Function
    Public Shared Function getFirstTrack(ByVal name As String) As Track
        Return getFirstTrack(name, "")
    End Function
    Public Shared Function getFirstTrack(ByVal name As String, extension As String) As Track
        If Folder.folders IsNot Nothing And name <> "" Then
            For i = 0 To Folder.folders.Count - 1
                If Folder.folders(i).tracks IsNot Nothing Then
                    For k = 0 To Folder.folders(i).tracks.Count - 1
                        If Folder.folders(i).tracks(k).name = name Then
                            If extension = "" Or Folder.folders(i).tracks(k).ext = extension Then
                                Return Folder.folders(i).tracks(k)
                            End If
                        End If
                    Next
                End If
            Next
        End If
        Return Nothing
    End Function



    Function dateString() As String
        If added = Nothing Then Return ""
        Return added.ToShortDateString
    End Function


    Overrides Function ToString() As String
        If Form1.searchState = PlayerEnums.SearchState.SEARCHING Then
            Return "  " & name
        Else
            Return name ' dateString()  '& " - " & name
        End If

    End Function

    Shared Operator =(ByVal a As Track, ByVal b As Track) As Boolean
        If a Is Nothing And b Is Nothing Then Return True
        If a Is Nothing Xor b Is Nothing Then Return False
        If a.Equals(b) Then Return True
        Return False
    End Operator
    Shared Operator <>(ByVal a As Track, ByVal b As Track) As Boolean
        If a Is Nothing And b Is Nothing Then Return False
        If a Is Nothing Xor b Is Nothing Then Return True
        If a.Equals(b) Then Return False
        Return True
    End Operator


End Class
