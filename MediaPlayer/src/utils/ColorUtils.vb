Imports MediaPlayer.PlayerEnums

Public Class ColorUtils

    Public Structure ColorStruct
        Public backColor As Color
        Public foreColor As Color
    End Structure

    Public Shared Function getControlColor(control As Control, parentForm As Form) As ColorStruct
        Dim form1Lists() As Control = {Form1.tv, Form1.l2, Form1.l2_2, Form1.labelL2_2Count, Form1.labelL2Count}
        If form1Lists.Contains(control) Then
            Return New ColorStruct With {.backColor = getLightColor(parentForm), .foreColor = getInvLightColor(parentForm)}
        End If

        If control.Equals(Form1.tSearch) Then
            Return New ColorStruct With {.backColor = getLightColor(parentForm), .foreColor = IIf(Form1.searchState = SearchState.NONE, Color.DimGray, IIf(darkTheme, Color.White, Color.Black))}
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
            Return New ColorStruct With {.backColor = getLightColor(parentForm), .foreColor = getInvLightColor(parentForm)}

        Else
            Return New ColorStruct With {.backColor = getDarkColor(parentForm), .foreColor = getInvDarkColor(parentForm)}
        End If
    End Function

    Public Shared ReadOnly Property getLightColor(currForm As Form) As Color
        Get
            Return IIf(formLocked And currForm.Equals(Form1), Color.DimGray, IIf(darkTheme, Color.FromArgb(35, 35, 35), Color.White))
        End Get
    End Property
    Public Shared ReadOnly Property getDarkColor(currForm As Form) As Color
        Get
            Return IIf(formLocked And currForm.Equals(Form1), Color.DimGray, IIf(darkTheme, Color.FromArgb(20, 20, 20), Color.FromArgb(255, 240, 240, 240)))
        End Get
    End Property
    Public Shared ReadOnly Property getInvLightColor(currForm As Form) As Color
        Get
            Return IIf(formLocked And currForm.Equals(Form1), Color.Black, IIf(Not darkTheme, Color.Black, Color.White))
        End Get
    End Property
    Public Shared ReadOnly Property getInvDarkColor(currForm As Form) As Color
        Get
            Return IIf(formLocked And currForm.Equals(Form1), Color.Black, IIf(Not darkTheme, Color.Black, Color.FromArgb(255, 240, 240, 240)))
        End Get
    End Property
End Class
