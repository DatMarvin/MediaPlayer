'05.06.2015 click count
Imports MediaPlayer.SettingsEnums
Module ClickGadget

    Public Declare Function GetAsyncKeyState Lib "user32" (ByVal nVirtKey As Integer) As Short

    Public Const CLICKS_BUTTON_LEFT = "Left"
    Public Const CLICKS_BUTTON_RIGHT = "Right"
    Public Const CLICKS_BUTTON_MIDDLE = "Total"
    Public Const CLICKS_BUTTON_TOTAL = "Total"

    Dim cll As Long = 0
    Dim clr As Long = 0
    Dim clm As Long = 0
    Dim downl As Boolean = False
    Dim downr As Boolean = False
    Dim downm As Boolean = False

    Sub clickGadgetHandler()

        If autoClicker Then
            If Key.keyList(Key.keyName.Clicker_Off).pressed Then
                Form1.clickerTimer.Stop()
            End If
        End If

        If clickCounter Then
            If Not GetAsyncKeyState(1) = 0 AndAlso Not Form1.keydelayt.Enabled Then
                If downl = False Then
                    downl = True
                    cll += 1
                End If
            Else : downl = False
            End If
            If Not GetAsyncKeyState(2) = 0 AndAlso Not Form1.keydelayt.Enabled Then
                If downr = False Then
                    downr = True
                    clr += 1
                End If
            Else : downr = False
            End If
            If Not GetAsyncKeyState(4) = 0 AndAlso Not Form1.keydelayt.Enabled Then
                If downm = False Then
                    downm = True
                    clm += 1
                End If
            Else : downm = False
            End If
        End If
    End Sub

    Public Sub performAutoClick()
        For i = 1 To autoClickerRep
            Utils.lMouseClick()
        Next
    End Sub

    Sub flushClickCounter()
        Dim totcurr As Long = cll + clr + clm
        saveRawSetting(SettingsIdentifier.CLICK_COUNTER, CLICKS_BUTTON_LEFT, CLng(loadRawSetting(SettingsIdentifier.CLICK_COUNTER, CLICKS_BUTTON_LEFT)) + cll)
        saveRawSetting(SettingsIdentifier.CLICK_COUNTER, CLICKS_BUTTON_RIGHT, CLng(loadRawSetting(SettingsIdentifier.CLICK_COUNTER, CLICKS_BUTTON_RIGHT)) + clr)
        saveRawSetting(SettingsIdentifier.CLICK_COUNTER, CLICKS_BUTTON_MIDDLE, CLng(loadRawSetting(SettingsIdentifier.CLICK_COUNTER, CLICKS_BUTTON_MIDDLE)) + clm)
        saveRawSetting(SettingsIdentifier.CLICK_COUNTER, CLICKS_BUTTON_TOTAL, CLng(loadRawSetting(SettingsIdentifier.CLICK_COUNTER, CLICKS_BUTTON_TOTAL)) + totcurr)
        cll = 0
        clr = 0
        clm = 0
    End Sub


End Module
