'04.04.2017
Public Class TrackPart


    Public ReadOnly Property dll As Utils
        Get
            Return formHandle.dll
        End Get
    End Property

    Public track As Track
    Public id As Integer
    Public name As String
    Public fromSec As Integer
    Public toSec As Integer
    Public ReadOnly Property fromFormat As String
        Get
            Return dll.secondsTo_ms_Format(fromSec)
        End Get
    End Property
    Public ReadOnly Property toFormat As String
        Get
            Return dll.secondsTo_ms_Format(toSec)
        End Get
    End Property
    Public ReadOnly Property format As String
        Get
            Return fromFormat & " - " & toFormat
        End Get
    End Property

    Public formHandle As Form1

    Public Sub New(handle As Form1, ByVal track As Track, ByVal id As Integer, ByVal data() As String)
        formHandle = handle
        If Not isValidInput(data) Then
            MsgBox("Invalid Track Parts init call. Corrupt data.")
            Exit Sub
        End If
        Me.track = track
        Me.id = id
        Try
            Dim times() As Integer = minFormatsToSec(Utils.ParseMinuteSecondString(data(0)))
            Me.fromSec = times(0)
            Me.toSec = times(1)
        Catch ex As Exception

        End Try

        If data.Length > 1 Then
            name = data(1)
        Else
            name = ""
        End If
    End Sub



    Function isValidInput(ByVal data() As String) As Boolean
        Dim ind As Integer = 0
        If data Is Nothing Then
            Return False
        ElseIf data.Length = 1 And Not data(0).Contains(",") Then
            Return False
        ElseIf data.Length = 2 AndAlso Not (data(0).Contains(",") Or data(1).Contains(",")) Then
            Return False

        Else
            If Not data(0).Contains(",") Then ind = 1

            Dim formats() As String = data(ind).Split(",")
            If Not formats.Length = 2 OrElse formats(0) = "" OrElse formats(1) = "" Then
                Return False
            Else
                For m = 0 To 1
                    If formats(m).Contains(":") Then
                        If Not Integer.TryParse(formats(m).Split(":")(0), New Integer) OrElse Not Integer.TryParse(formats(m).Split(":")(1), New Integer) Then
                            Return False
                        End If
                    Else
                        If Not Integer.TryParse(formats(m), New Integer) Then Return False
                    End If
                Next
            End If
        End If
        Return True
    End Function


    Function minFormatsToSec(ByVal s() As String) As Integer()
        Dim res(1) As Integer
        Dim vals() As String
        For i = 0 To 1
            vals = s(i).Split(":")
            If vals IsNot Nothing Then
                If vals.Length = 2 Then
                    res(i) = vals(0) * 60 + vals(1)
                ElseIf vals.Length = 1 Then 'second format
                    res(i) = vals(0)
                End If
            End If
        Next
        Return res
    End Function

    Shared Sub sortTrackParts(ByRef parts As List(Of TrackPart))
        parts.Sort(New Comparison(Of TrackPart)(Function(x, y)
                                                    Return x.fromSec.CompareTo(y.fromSec)
                                                End Function))
    End Sub

    Overrides Function ToString() As String
        Dim display As String = name
        If display = "" Then display = "Unknown Title"
        Dim treeChar As Char = "|"
        Dim padSpaces As String = "  " & IIf(Form1.searchState > PlayerEnums.SearchState.NONE, "  ", "")
        Return padSpaces & treeChar & " " & format & " → " & display
    End Function
End Class
