Imports MediaPlayer.PlayerEnums

Public Class FormUtils

#Region "Form1"
    Public Shared Sub setLockImage()
        If formLocked Then
            Form1.menuLock.Image = IIf(darkTheme, My.Resources.unlock_inv, My.Resources.unlock)
            Form1.menuLock.ToolTipText = "Unlock Hotkeys"
        Else
            Form1.menuLock.Image = IIf(darkTheme, My.Resources.lock_inv, My.Resources.lock)
            Form1.menuLock.ToolTipText = "Lock Hotkeys"
        End If
    End Sub
    Public Shared Sub setRemoteImage()
        If Form1.remoteTcp.isEstablished Then
            Form1.menuRemote.Image = IIf(darkTheme, My.Resources.online_inv, My.Resources.online)
            Form1.menuRemote.ToolTipText = "Status: Connected"
        Else
            If Form1.remoteTcp.isListenerActive Then
                Form1.menuRemote.Image = IIf(darkTheme, My.Resources.offline_inv, My.Resources.offline)
                Form1.menuRemote.ToolTipText = "Status: Ready"
            Else
                Form1.menuRemote.Image = IIf(darkTheme, My.Resources.blocked_inv, My.Resources.blocked)
                Form1.menuRemote.ToolTipText = "Status: Blocked"
            End If

        End If
    End Sub
    Public Shared Sub setLyricsImage()
        Dim l As ListBox = Form1.getSelectedList()
        If radioEnabled Or l Is Nothing OrElse l.SelectedIndex = -1 OrElse TypeOf l.SelectedItem IsNot Track Then
            Form1.menuLyrics.Image = IIf(darkTheme, My.Resources.cross_inv, My.Resources.cross)
        Else
            Dim track As Track = l.SelectedItem
            If LyricsForm.hasLyrics(track) Then
                Form1.menuLyrics.Image = IIf(darkTheme, My.Resources.tick_inv, My.Resources.tick)
            Else
                Form1.menuLyrics.Image = IIf(darkTheme, My.Resources.cross_inv, My.Resources.cross)
            End If
        End If
    End Sub
    Public Shared Sub setSettingsImage()
        Form1.menuSettings.Image = IIf(darkTheme, My.Resources.settings_inv, My.Resources.settings)
    End Sub

    Public Shared Sub setGadgetsImage()
        Form1.menuGadgets.Image = IIf(darkTheme, My.Resources.gadgets_inv, My.Resources.gadgets)
    End Sub

    Private Shared Sub setMenuIcons()
        setLockImage()
        setRemoteImage()
        setSettingsImage()
        setLyricsImage()
        setGadgetsImage()
    End Sub
#End Region


    Public Shared Sub colorForm(ByVal form As Form)
        Dim elements As New List(Of Control) From {form}
        For Each c As Control In form.Controls
            elements.Add(c)
            For Each subControl As Control In c.Controls
                elements.Add(subControl)
                For Each subSubControl As Control In subControl.Controls
                    elements.Add(subSubControl)
                    For Each subSubSubControl As Control In subSubControl.Controls
                        elements.Add(subSubSubControl)
                    Next
                Next
            Next
        Next

        For Each c As Control In elements
            Dim colorDef As ColorUtils.ColorStruct = ColorUtils.getControlColor(c, form)
            c.BackColor = colorDef.backColor
            c.ForeColor = colorDef.foreColor
            If TypeOf c Is Button Then
                CType(c, Button).FlatStyle = FlatStyle.System
            End If

        Next

        If form.Equals(Form1) Then
            setMenuIcons()
        End If

    End Sub

End Class
