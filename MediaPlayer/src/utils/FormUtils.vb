Imports MediaPlayer.PlayerEnums

Public Class FormUtils

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
            Form1.setMenuIcons()
        End If

    End Sub

End Class
