'25.08.2019
Imports System.IO
Public Class LyricsForm

    Property overlayMode() As Form1.eOverlayMode
        Get
            Return Form1.overlayMode
        End Get
        Set(value As Form1.eOverlayMode)
            Form1.overlayMode = value
        End Set
    End Property

    ReadOnly Property dll() As Utils
        Get
            Return Form1.dll
        End Get
    End Property


    Dim currLyrTrack As Track
    Dim state As lyricState
    Public ReadOnly Property lyricsAutoSave() As Boolean
        Get
            Return SettingsService.getSetting(SettingsIdentifier.LYRICS_AUTO_SAVE)
        End Get
    End Property
    Shared queuedTrack As Track

    Enum lyricState
        INIT
        READ
        MODIFIED
    End Enum

    Private Sub LyricsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim yNorm As Integer = Form1.Top + Form1.Height / 2 - Me.Height / 2
        Me.Location = New Point(Math.Min(My.Computer.Screen.WorkingArea.Width, Form1.Right + Width) - Width, yNorm + Math.Max(0, 0 - yNorm) + Math.Min(0, My.Computer.Screen.WorkingArea.Height - (yNorm + Height)))
        FormUtils.colorForm(Me)
        setState(lyricState.INIT)

        checkAutoSave.Checked = lyricsAutoSave
    End Sub

    Private Sub LyricsForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If state = lyricState.MODIFIED Then
            saveLyrics(Not lyricsAutoSave)
        End If
        overlayMode -= Form1.eOverlayMode.LYRICS
    End Sub

    Sub updateUI()
        Form1.loadFont(tLyrics, SettingsIdentifier.FONT_LYRICS, New Font("Microsoft Sans Serif", 14))
        tLyrics.Size = New Size(Width - 17, Height - 75)
        tLyrics.Location = New Point(0, 0)
        buttonSaveExit.Location = New Point(tLyrics.Right - buttonSaveExit.Width - 3, tLyrics.Bottom + 5)
        buttonSaveExit.Text = IIf(state = lyricState.MODIFIED, "Save && ", "") & "Exit"
        buttonSearchOnline.Top = buttonSaveExit.Top
        buttonLyricsOpenSource.Top = buttonSaveExit.Top
        checkAutoSave.Top = buttonSaveExit.Top + 5
    End Sub

    Sub openLyrics(track As Track)
        If queuedTrack Is Nothing Then
            queuedTrack = track
            If OperatingSystem.isValidDirectoryPath(lyrpath) Then
                If state = lyricState.MODIFIED Then
                    saveLyrics(Not lyricsAutoSave)
                End If
                updateUI()
                currLyrTrack = queuedTrack
                tLyrics.Text = readLyricsStream(queuedTrack)
                setState(lyricState.READ)
                tLyrics.Select()
                Text = "Lyrics - " & queuedTrack.name
                queuedTrack = Nothing
            Else
                Form1.showOptions(OptionsForm.optionState.PATHS)
            End If
        Else
            queuedTrack = track
        End If
    End Sub


    Function readLyricsStream(ByVal track As Track) As String
        Dim path As String = lyrpath & track.name & ".txt"
        If Not File.Exists(path) Then
            Directory.CreateDirectory(lyrpath)
            Return ""
        End If
        Dim s As String = ""
        Try
            Using sr As New StreamReader(path)
                s = sr.ReadToEnd
            End Using
        Catch ex As Exception
            Return ""
        End Try
        s = s.Trim()
        Return s
    End Function

    Function writeLyricsStream() As Boolean
        Dim path As String = lyrpath & currLyrTrack.name & ".txt"
        Try
            Using writer As New StreamWriter(path, False)
                writer.Write(tLyrics.Text)
            End Using
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function


    Public Function hasLyrics(track As Track) As Boolean
        If Not File.Exists(lyrpath & track.name & ".txt") Then
            Return False
        End If
        Dim sr As StreamReader = Nothing
        Try
            sr = New StreamReader(lyrpath & track.name & ".txt")
            Using sr
                Return sr.Peek() > -1
            End Using
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub LyricsForm_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        updateUI()
    End Sub

    Private Sub tLyrics_TextChanged(sender As Object, e As EventArgs) Handles tLyrics.TextChanged
        If state = lyricState.READ Then
            setState(lyricState.MODIFIED)
        End If
    End Sub


    Sub setState(toState As lyricState)
        state = toState
        updateUI()
    End Sub


    Private Sub LyricsForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown, tLyrics.KeyDown
        If e.Control And e.KeyCode = Keys.S Then
            saveLyrics(False)
        End If
    End Sub

    Function saveLyrics(prompt As Boolean) As Boolean
        If Not prompt OrElse MsgBox("Lyrics have been modified." & vbNewLine & "Do you want to save changes?", MsgBoxStyle.YesNo, "Lyrics Manager") = MsgBoxResult.Yes Then
1:          If state = lyricState.MODIFIED Then
                If writeLyricsStream() Then
                    setState(lyricState.READ)
                    Form1.setLyricsImage()
                Else
                    If MsgBox("Failed to write to file. Try again?", MsgBoxStyle.YesNo + MsgBoxStyle.Critical, "Lyrics Manager") = MsgBoxResult.Yes Then
                        GoTo 1
                    Else
                        Return False
                    End If
                End If
            End If
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub buttonSaveExit_Click(sender As Object, e As EventArgs) Handles buttonSaveExit.Click
        saveLyrics(False)
        Close()
    End Sub

    Private Sub checkAutoSave_CheckedChanged(sender As Object, e As EventArgs) Handles checkAutoSave.CheckedChanged
        SettingsService.saveSetting(SettingsIdentifier.LYRICS_AUTO_SAVE, sender.checked)
    End Sub

    Private Sub buttonSearchOnline_Click(sender As Object, e As EventArgs) Handles buttonSearchOnline.Click
        Dim currPart As TrackPart = currLyrTrack.getCurrentPart()
        If currPart Is Nothing Then
            Process.Start("https://www.google.com/search?q=" & currLyrTrack.name.Replace(" ", "+").Replace(" & ", " ").Replace("&", " ") & "+lyrics")
        Else
            Process.Start("https://www.google.com/search?q=" & currPart.name.Replace(" ", "+").Replace(" & ", " ").Replace("&", " ") & "+lyrics")
        End If

    End Sub

    Private Sub buttonLyricsOpenSource_Click(sender As Object, e As EventArgs) Handles buttonLyricsOpenSource.Click
        Dim filePath As String = lyrPath & currLyrTrack.name & ".txt"
        If File.Exists(filePath) Then
            Process.Start("explorer.exe", " /n, /e,/select," & filePath)
        Else
            Process.Start("explorer.exe", " /n, /e," & lyrPath)
        End If

    End Sub

    Private Sub tLyrics_MouseWheel(sender As Object, e As MouseEventArgs) Handles tLyrics.MouseWheel
        Dim senderControl As Control = CType(sender, Control)
        If Key.ctrlKey Then
            If senderControl.Font.Size >= 1 And senderControl.Font.Size <= 70 Then
                Dim fontSize As Integer = senderControl.Font.Size + IIf(senderControl.Font.Size > 1, -1, 0)
                If e.Delta > 0 Then
                    fontSize = senderControl.Font.Size + IIf(senderControl.Font.Size < 70, 1, 0)
                End If
                OptionsForm.saveFont(senderControl, New Font(senderControl.Font.FontFamily.Name, fontSize, senderControl.Font.Style), SettingsIdentifier.FONT_LYRICS)
            End If
            Dim x As Integer = senderControl.Width - 85
            Dim y As Integer = Cursor.Position.Y - senderControl.Top - Me.Top - 40
            Form1.ttShow(senderControl.Font.Size, senderControl, x, y, 1500)
        End If
    End Sub
End Class