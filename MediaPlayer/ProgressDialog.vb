Public Class ProgressDialog


    Private Sub ProgressDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Location = New Point(AsyncTask.getFormHandle().Left + AsyncTask.getFormHandle().Width / 2 - Me.Width / 2, AsyncTask.getFormHandle().Top + AsyncTask.getFormHandle().Height / 2 - Me.Height / 2)
    End Sub


    Private Sub ProgressDialog_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        AsyncTask.cancelTask()
    End Sub

    Sub updateProgress(prog As IProgress(Of Integer), curr As Integer, max As Integer)
        If AsyncTask.progressDialogHandle IsNot Nothing Then
            prog.Report(((curr) / max) * 100)
            progLabel.Invoke(Sub() progLabel.Text = curr & " / " & max)
        End If
    End Sub

    Sub progressBarUpdate(value As Integer)
        pb.Value = value
    End Sub


    Private Sub cancel_click(sender As Object, e As EventArgs) Handles cancelButton.Click
        AsyncTask.cancelTask()
    End Sub

End Class