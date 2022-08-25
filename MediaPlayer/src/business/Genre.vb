Imports MediaPlayer.SettingsEnums
Public Class Genre

    Shared formHandle As Form1

    Public Shared ReadOnly Property dll() As Utils
        Get
            Return formHandle.dll
        End Get
    End Property

    Public Shared genres As List(Of Genre)
    Public Shared Undefined = New Genre("Undefined")

    Public name As String
    Public tracks As List(Of Track)
    Public folders As List(Of Folder)

    Public Sub New(name As String)
        Me.name = name
        tracks = New List(Of Track)
        folders = New List(Of Folder)
    End Sub




    Public Sub addFolder(folder As Folder)
        If Not containsFolder(folder) Then
            folders.Add(folder)
        End If
    End Sub

    Public Function containsTrack(track As Track) As Boolean
        For Each t As Track In tracks
            If t.name.ToLower = track.name.ToLower Then Return True
        Next
        Return False
    End Function
    Public Function containsFolder(folder As Folder) As Boolean
        For Each f As Folder In folders
            If f.fullPath.ToLower = folder.fullPath.ToLower Then Return True
        Next
        Return False
    End Function
    Public Function containsTrack(track As String) As Boolean
        For Each t As Track In tracks
            If t.name.ToLower = track.ToLower Then Return True
        Next
        Return False
    End Function
    Public Function containsFolder(folder As String) As Boolean
        For Each f As Folder In folders
            If f.fullPath.ToLower = folder.ToLower Then Return True
        Next
        Return False
    End Function

    Public Function folderAssociationExists(folder As Folder) As Genre
        Dim currVal As String = loadRawSetting(SettingsIdentifier.GENRES_MAPPING, folder.fullPath)
        If Not currVal = "" Then
            For Each g As Genre In genres
                If Not g.Equals(Me) Then
                    If currVal.ToLower = g.name.ToLower Then Return g
                End If
            Next
        End If
        Return Genre.Undefined
    End Function

    Public Shared Sub initGenres(handle As Form1, genreStr() As String)
        formHandle = handle
        genres = New List(Of Genre)
        If genreStr IsNot Nothing Then
            For Each s As String In genreStr
                genres.Add(New Genre(s))
            Next
        End If
    End Sub

    Public Shared Sub updateTrackAssociations()
        For Each g As Genre In genres
            g.tracks = New List(Of Track)
        Next
        Dim pairs As List(Of KeyValuePair(Of String, String)) = dll.iniGetAllPairs(IniSection.GENRES, inipath)
        For Each p As KeyValuePair(Of String, String) In pairs
            If Not p.Key.Contains("\") Then
                Dim currGenre As Genre = getGenre(p.Value)
                If currGenre IsNot Nothing Then
                    currGenre.tracks.Add(Track.getFirstTrack(p.Key))
                End If
            End If
        Next
    End Sub
    Public Shared Sub updateFolderAssociations()
        For Each g As Genre In genres
            g.folders = New List(Of Folder)
        Next
        Dim pairs As List(Of KeyValuePair(Of String, String)) = dll.iniGetAllPairs(IniSection.GENRES, inipath)
        For Each p As KeyValuePair(Of String, String) In pairs
            If p.Key.Contains("\") Then
                Dim currGenre As Genre = getGenre(p.Value)
                If currGenre IsNot Nothing Then
                    Dim addFolder As Folder = Folder.getFolder(p.Key)
                    If addFolder IsNot Nothing Then
                        currGenre.folders.Add(addFolder)
                    End If
                End If
            End If
        Next
    End Sub

    Public Shared Function getGenre(name As String) As Genre
        If genres Is Nothing Then Return Undefined
        For Each g As Genre In genres
            If g.name.ToLower = name.ToLower Then Return g
        Next
        Return Undefined
    End Function

    Public Shared Sub writeGenres()
        Dim res As String = ""
        For Each g As Genre In genres
            res &= g.name & ";"
        Next
        If res.EndsWith(";") Then res = res.Substring(0, res.Length - 1)
        SettingsService.saveSetting(SettingsIdentifier.GENRES, res)
    End Sub


    Public Shared Function contains(value As String) As Boolean
        For Each g As Genre In genres
            If g.name.ToLower = value.ToLower Then Return True
        Next
        Return False
    End Function

    Overrides Function ToString() As String
        Return name
    End Function
End Class
