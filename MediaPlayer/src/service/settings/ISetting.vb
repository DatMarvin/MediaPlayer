Public Interface ISetting

    Function loadSetting() As String

    ' 
    Function loadRawSetting(parameter As String) As String

    Function getSetting() As String

    Function saveSetting(newValue As String) As Boolean

    Function saveRawSetting(parameter As String, newValue As String) As Boolean

    Sub setSetting(newValue As String)

    Function exists() As Boolean

    Sub initSetting()

End Interface
