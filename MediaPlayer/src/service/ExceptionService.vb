Imports System.ComponentModel

Public Class ExceptionService

    Private Shared ReadOnly timers As List(Of Timer) = getAllTimers()
    Public Shared Sub handleException(msg As String, Optional cause As Exception = Nothing, Optional severity As MsgBoxStyle = MsgBoxStyle.Critical)
        Dim disabled As List(Of Timer) = disableTimers()
        Dim errorMsg As String = msg
        If cause IsNot Nothing Then
            errorMsg &= vbNewLine & vbNewLine & cause.StackTrace
        End If
        MsgBox(errorMsg, severity)
        enableTimers(disabled)
    End Sub

    Public Shared Sub handleException(msg As String, cause As Exception)
        handleException(msg, cause, MsgBoxStyle.Critical)
    End Sub

    Public Shared Sub handleException(msg As String, severity As MsgBoxStyle)
        handleException(msg, Nothing, severity)
    End Sub

    Private Shared Function disableTimers() As List(Of Timer)
        Dim switched As New List(Of Timer)
        For Each timer As Timer In timers
            If timer.Enabled Then
                timer.Enabled = False
                switched.Add(timer)
            End If
        Next
        Return switched
    End Function

    Private Shared Sub enableTimers(timersToEnable As List(Of Timer))
        For Each timer As Timer In timersToEnable
            timer.Enabled = True
        Next
    End Sub
    Private Shared Function getAllTimers() As List(Of Timer)
        Dim timers As New List(Of Timer)
        Dim components As List(Of Component) = Utils.getComponents(Form1)
        For Each c As Component In components
            If TypeOf c Is Timer Then
                timers.Add(c)
            End If
        Next
        Return timers
    End Function

End Class
