Public Class Radio

    Public Shared dll As New Class1
    Public url As String
    Public name As String
    Public time As Integer
    Public timeTemp As Integer

    Public Sub New(ByVal name As String, ByVal url As String)
        MyClass.name = name
        MyClass.url = url

    End Sub
    Sub update()
        time = dll.iniReadValue("RadioTime", name, 0, Form1.inipath)
    End Sub
    Overrides Function ToString() As String
        Return name
    End Function

    Public Shared Function getStations() As List(Of Radio)
        Dim rads As New List(Of Radio)
        Dim names() As String = dll.iniGetAllKeys("Radio", Form1.inipath)
        Dim urls() As String = dll.iniGetAllValues("Radio", Form1.inipath)
        If names IsNot Nothing And urls IsNot Nothing Then
            For i = 0 To names.Length - 1
                rads.Add(New Radio(names(i), urls(i)))
                rads(rads.Count - 1).update()
            Next
        End If
        Return rads
    End Function

End Class
