Imports MediaPlayer.SettingsEnums
Public Class Radio

    Public Shared dll As New Utils
    Public url As String
    Public name As String
    Public time As Integer
    Public timeTemp As Integer

    Public Sub New(ByVal name As String, ByVal url As String)
        MyClass.name = name
        MyClass.url = url

    End Sub
    Sub update()
        time = loadRawSetting(SettingsIdentifier.RADIO_TIME, name)
    End Sub
    Overrides Function ToString() As String
        Return name
    End Function

    Public Shared Function getStations() As List(Of Radio)
        Dim rads As New List(Of Radio)
        Dim names() As String = dll.iniGetAllKeys(IniSection.RADIO, inipath)
        Dim urls() As String = dll.iniGetAllValues(IniSection.RADIO, inipath)
        If names IsNot Nothing And urls IsNot Nothing Then
            For i = 0 To names.Length - 1
                rads.Add(New Radio(names(i), urls(i)))
                rads(rads.Count - 1).update()
            Next
        End If
        Return rads
    End Function

End Class
