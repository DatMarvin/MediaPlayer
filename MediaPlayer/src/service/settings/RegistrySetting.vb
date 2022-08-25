Public Class RegistrySetting
    Implements ISetting

    Private Const EMPTY_SETTING = "EMPTY_SETTING"

    Dim section As String
    Dim key As String
    Dim defaultValue As String
    Dim value As String

    Public Sub New(ByVal section As String, ByVal key As String, defaultValue As String)
        MyClass.New(section, key, defaultValue, Nothing)
    End Sub
    Public Sub New(ByVal section As String, ByVal key As String, defaultValue As String, dlg As ActionDelegate)
        Me.section = section
        Me.key = key
        Me.defaultValue = defaultValue

        initSetting()

        If dlg IsNot Nothing Then
            dlg.Invoke(value)
        End If
    End Sub

    Public Sub initSetting() Implements ISetting.initSetting
        value = loadFromPersistence()
    End Sub

    Public Function loadSetting() As String Implements ISetting.loadSetting
        value = loadFromPersistence()
        Return value
    End Function


    Public Function getSetting() As String Implements ISetting.getSetting
        Return value
    End Function


    Public Function saveSetting(newValue As String) As Boolean Implements ISetting.saveSetting
        Return saveToPersistence(newValue)
    End Function

    Public Sub setSetting(newValue As String) Implements ISetting.setSetting
        value = newValue
    End Sub

    Public Function exists() As Boolean Implements ISetting.exists
        Return Interaction.GetSetting(My.Application.Info.ProductName, section, key, EMPTY_SETTING) <> EMPTY_SETTING
    End Function



    Private Function loadFromPersistence() As String
        Return Interaction.GetSetting(My.Application.Info.ProductName, section, key, defaultValue)
    End Function

    Private Function saveToPersistence(newValue As String) As Boolean
        Dim old As String = value
        Interaction.SaveSetting(My.Application.Info.ProductName, section, key, newValue)
        setSetting(newValue)
        Return old <> value
    End Function

    Public Function loadRawSetting(parameter As String) As String Implements ISetting.loadRawSetting
        Throw New NotImplementedException()
    End Function

    Public Function saveRawSetting(parameter As String, newValue As String) As Boolean Implements ISetting.saveRawSetting
        Throw New NotImplementedException()
    End Function
End Class
