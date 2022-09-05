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
        Dim names As List(Of String) = IniService.iniGetAllKeys(IniSection.RADIO)
        Dim urls As List(Of String) = IniService.iniGetAllValues(IniSection.RADIO)
        If names.Count = urls.Count Then
            For i = 0 To names.Count - 1
                rads.Add(New Radio(names(i), urls(i)))
                rads(rads.Count - 1).update()
            Next
        End If
        Return rads
    End Function

End Class
