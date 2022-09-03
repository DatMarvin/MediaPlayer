Public Class IniSetting(Of T)
    Implements ISetting

    Dim iniPath As String
    Dim bufferSize As Integer

    Dim defaultValue As String
    Dim value As String

    Dim iniSection As String
    Dim iniKey As String

    Public Sub New(ByVal iniSection As String, ByVal iniKey As String, defaultValue As String, iniPath As String, bufferSize As Integer, dlg As ActionDelegate)
        Me.iniSection = iniSection
        Me.iniKey = iniKey
        Me.defaultValue = defaultValue
        Me.iniPath = iniPath
        Me.bufferSize = bufferSize

        initSetting()

        If dlg IsNot Nothing Then
            dlg.Invoke(value)
        End If
    End Sub


    Public Sub New(ByVal iniSection As String, ByVal iniKey As String, defaultValue As String, iniPath As String, dlg As ActionDelegate)
        MyClass.New(iniSection, iniKey, defaultValue, iniPath, 1024, dlg)
    End Sub

    Public Sub New(ByVal iniSection As String, ByVal iniKey As String, defaultValue As String, bufferSize As Integer, dlg As ActionDelegate)
        MyClass.New(iniSection, iniKey, defaultValue, SettingsService.loadSetting(SettingsIdentifier.INIPATH), bufferSize, dlg)
    End Sub

    Public Sub New(ByVal iniSection As String, ByVal iniKey As String, defaultValue As String, iniPath As String)
        MyClass.New(iniSection, iniKey, defaultValue, iniPath, 1024, Nothing)
    End Sub

    Public Sub New(ByVal iniSection As String, ByVal iniKey As String, defaultValue As String, bufferSize As Integer)
        MyClass.New(iniSection, iniKey, defaultValue, SettingsService.loadSetting(SettingsIdentifier.INIPATH), bufferSize, Nothing)
    End Sub

    Public Sub New(ByVal iniSection As String, ByVal iniKey As String, defaultValue As String, dlg As ActionDelegate)
        MyClass.New(iniSection, iniKey, defaultValue, SettingsService.loadSetting(SettingsIdentifier.INIPATH), 1024, dlg)
    End Sub
    Public Sub New(ByVal iniSection As String, ByVal iniKey As String, defaultValue As String)
        MyClass.New(iniSection, iniKey, defaultValue, SettingsService.loadSetting(SettingsIdentifier.INIPATH), 1024, Nothing)
    End Sub

    Public Sub initSetting() Implements ISetting.initSetting
        If Not isRawSetting() Then
            loadSetting()
        End If
    End Sub

    Public Function loadSetting() As String Implements ISetting.loadSetting
        If isRawSetting() Then
            Throw New InvalidOperationException()
        End If
        value = loadFromPersistence()
        Return value
    End Function

    Public Function getSetting() As String Implements ISetting.getSetting
        Return value
    End Function

    Public Function saveSetting(newValue As String) As Boolean Implements ISetting.saveSetting
        If isRawSetting() Then
            Throw New InvalidOperationException()
        End If
        Return saveToPersistence(newValue)
    End Function

    Public Sub setSetting(newValue As String) Implements ISetting.setSetting
        value = newValue
    End Sub

    Public Function exists() As Boolean Implements ISetting.exists
        Return Form1.dll.iniIsValidKey(iniSection, iniKey, iniPath)
    End Function

    Public Function isRawSetting() As Boolean
        Return iniKey Is Nothing
    End Function

    Private Function loadFromPersistence() As String
        Return loadFromPersistence(iniKey)
    End Function
    Private Function loadFromPersistence(iniKey As String) As String
        Return Form1.dll.iniReadValue(iniSection, iniKey, defaultValue, iniPath, bufferSize)
    End Function

    Private Function saveToPersistence(newValue As String) As Boolean
        Return saveToPersistence(iniKey, newValue)
    End Function
    Private Function saveToPersistence(iniKey As String, newValue As String) As Boolean
        Dim old As String = value
        Form1.dll.iniWriteValue(iniSection, iniKey, newValue, iniPath)
        setSetting(newValue)
        Return old <> value
    End Function

    Public Function loadRawSetting(parameter As String) As String Implements ISetting.loadRawSetting
        If Not isRawSetting() Then
            Throw New InvalidOperationException()
        End If
        Return loadFromPersistence(parameter)
    End Function

    Public Function saveRawSetting(parameter As String, newValue As String) As Boolean Implements ISetting.saveRawSetting
        If Not isRawSetting() Then
            Throw New InvalidOperationException()
        End If
        Return saveToPersistence(parameter, newValue)
    End Function
End Class
