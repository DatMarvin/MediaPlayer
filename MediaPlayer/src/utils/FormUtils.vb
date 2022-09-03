Imports MediaPlayer.PlayerEnums

Public Class FormUtils

    Private Shared currForm As Form

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
        currForm = form
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
            Dim colorDef As ColorStruct = getControlColor(c)
            c.BackColor = colorDef.backColor
            c.ForeColor = colorDef.foreColor
            If TypeOf c Is Button Then
                CType(c, Button).FlatStyle = FlatStyle.System
            End If

        Next

        If form.Equals(Form1) Then
            setMenuIcons()
        End If

        currForm = Nothing

    End Sub

    Public Structure ColorStruct
        Public backColor As Color
        Public foreColor As Color
    End Structure

    Public Shared Function getControlColor(ByVal control As Control) As ColorStruct
        Dim form1Lists() As Control = {Form1.tv, Form1.l2, Form1.l2_2, Form1.labelL2_2Count, Form1.labelL2Count}
        If form1Lists.Contains(control) Then
            Return New ColorStruct With {.backColor = getLightColor(), .foreColor = getInvLightColor()}
        End If

        If control.Equals(Form1.tSearch) Then
            Return New ColorStruct With {.backColor = getLightColor(), .foreColor = IIf(Form1.searchState = SearchState.NONE, Color.DimGray, IIf(darkTheme, Color.White, Color.Black))}
        End If

        Dim playerOverlayLabels() As Control = {Form1.labelPrevTrack, Form1.labelNextTrack}
        If playerOverlayLabels.Contains(control) Then
            Return New ColorStruct With {.backColor = Color.White, .foreColor = SystemColors.HotTrack}
        End If
        Dim playerOverlayLabels2() As Control = {Form1.labelVolume, Form1.picRepeat, Form1.picRandom}
        If playerOverlayLabels2.Contains(control) Then
            Return New ColorStruct With {.backColor = Color.FromArgb(240, 240, 240), .foreColor = SystemColors.HotTrack}
        End If

        If TypeOf control Is ListBox Or TypeOf control Is Button Then
            Return New ColorStruct With {.backColor = getLightColor(), .foreColor = getInvLightColor()}

        Else
            Return New ColorStruct With {.backColor = getDarkColor(), .foreColor = getInvDarkColor()}
        End If
    End Function

    Public Shared ReadOnly Property getLightColor() As Color
        Get
            Return IIf(formLocked And currForm.Equals(Form1), Color.DimGray, IIf(darkTheme, Color.FromArgb(35, 35, 35), Color.White))
        End Get
    End Property
    Public Shared ReadOnly Property getDarkColor() As Color
        Get
            Return IIf(formLocked And currForm.Equals(Form1), Color.DimGray, IIf(darkTheme, Color.FromArgb(20, 20, 20), Color.FromArgb(255, 240, 240, 240)))
        End Get
    End Property
    Public Shared ReadOnly Property getInvLightColor() As Color
        Get
            Return IIf(formLocked And currForm.Equals(Form1), Color.Black, IIf(Not darkTheme, Color.Black, Color.White))
        End Get
    End Property
    Public Shared ReadOnly Property getInvDarkColor() As Color
        Get
            Return IIf(formLocked And currForm.Equals(Form1), Color.Black, IIf(Not darkTheme, Color.Black, Color.FromArgb(255, 240, 240, 240)))
        End Get
    End Property
End Class
