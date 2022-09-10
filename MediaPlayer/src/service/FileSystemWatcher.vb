Imports System.IO

Public Class FileSystemWatcher

    Shared fswEventRaised As Boolean

    Public Shared Sub fswInit()
        Form1.fsw.Path = path
    End Sub

    Public Shared Sub handleCreatedChangedDeleted(e As FileSystemEventArgs)
        If Utils.hasAudioExt(e.FullPath) OrElse Not e.FullPath.Contains(".") Then
            fswHandle()
        End If
    End Sub

    Public Shared Sub handleRenamed(e As RenamedEventArgs)
        If File.Exists(e.FullPath) Then
            If Utils.hasAudioExt(e.FullPath) Then
                fswHandle()
            End If
        ElseIf Directory.Exists(e.FullPath) Then
            Dim oldFolder As Folder = Folder.getFolder(e.OldFullPath)
            If oldFolder IsNot Nothing Then
                If Not oldFolder.isExcluded Then
                    fswHandle()
                End If
            End If
        End If
    End Sub

    Private Shared Sub fswHandle()
        If Form.ActiveForm IsNot Form1 And Form.ActiveForm IsNot OptionsForm Then
            If Not Form1.fswSleep.Enabled Then
                Form1.fswSleep.Start()
                Form1.tv_refill()
            Else
                fswEventRaised = True
            End If
        End If
    End Sub

    Public Shared Sub handleFswSleepTimer()
        If fswEventRaised Then Form1.tv_refill()
        fswEventRaised = False
        Form1.fswSleep.Stop()
    End Sub
End Class
