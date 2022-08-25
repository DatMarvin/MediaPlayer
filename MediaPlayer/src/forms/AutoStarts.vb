Module AutoStarts


    Public Const AUTOSTARTS_SECTION = "Autostarts"

    Sub executeAutoStarts()
        If SettingsService.getSetting(SettingsIdentifier.AUTOSTARTS) Then
            Dim allKeys() As String = Utils.iniGetAllKeys(AUTOSTARTS_SECTION, SettingsService.getSetting(SettingsIdentifier.INIPATH))
            If allKeys IsNot Nothing Then
                For Each key In allKeys
                    If GadgetsForm.getAutostartActive(key) Then
                        Dim file As String = GadgetsForm.getAutostartPath(key)
                        Dim args As String = GadgetsForm.getAutostartArgs(key)
                        Try
                            Process.Start(file, args)
                        Catch ex As Exception
                            Throw ex '  MsgBox(ex)
                        End Try
                    End If
                Next
            End If
        End If
    End Sub

End Module
