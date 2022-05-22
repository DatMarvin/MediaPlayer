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

    ReadOnly Property dll() As Class1
        Get
            Return Form1.dll
        End Get
    End Property

    ReadOnly Property inipath() As String
        Get
            Return Form1.inipath
        End Get
    End Property
    ReadOnly Property lyrPath() As String
        Get
            Return Form1.lyrpath
        End Get
    End Property

    Dim currLyrTrack As Track
    Dim state As lyricState
    Dim autoSave As Boolean
    Shared queuedTrack As Track

    Enum lyricState
        INIT
        READ
        MODIFIED
    End Enum

    Private Sub LyricsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim yNorm As Integer = Form1.Top + Form1.Height / 2 - Me.Height / 2
        Me.Location = New Point(Math.Min(My.Computer.Screen.WorkingArea.Width, Form1.Right + Width) - Width, yNorm + Math.Max(0, 0 - yNorm) + Math.Min(0, My.Computer.Screen.WorkingArea.Height - (yNorm + Height)))
        colorForm()
        setState(lyricState.INIT)

        autoSave = dll.iniReadValue("Config", "lyricsAutoSave", 0, Form1.inipath)
        checkAutoSave.Checked = autoSave
    End Sub

    Private Sub LyricsForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If state = lyricState.MODIFIED Then
            saveLyrics(Not autoSave)
        End If
        overlayMode -= Form1.eOverlayMode.LYRICS
    End Sub

    Sub colorForm()
        Dim inverted As Boolean = dll.iniReadValue("Config", "invColors", 0, inipath)
        Dim lightCol As Color = IIf(inverted, Color.FromArgb(50, 50, 50), Color.White)
        Dim darkCol As Color = IIf(inverted, Color.FromArgb(20, 20, 20), Color.FromArgb(255, 240, 240, 240))

        Dim invLightCol As Color = IIf(Not inverted, Color.Black, Color.White)
        Dim invDarkCol As Color = IIf(Not inverted, Color.Black, Color.FromArgb(255, 240, 240, 240))

        Dim elements As New List(Of Control)
        elements.Add(Me)
        For Each c As Control In Me.Controls
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
            If TypeOf c Is ListBox Or TypeOf c Is TreeView Or TypeOf c Is ListView Or TypeOf c Is TextBox Then
                c.BackColor = lightCol
                c.ForeColor = invLightCol
            ElseIf TypeOf c Is Button Then
                CType(c, Button).FlatStyle = FlatStyle.System
                c.BackColor = lightCol
                c.ForeColor = invLightCol
            Else
                c.BackColor = darkCol
                c.ForeColor = invDarkCol
            End If
        Next
    End Sub

    Sub updateUI()
        Form1.loadFont(tLyrics, "fontLyrics", New Font("Microsoft Sans Serif", 14))
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
            If Form1.isValidDirectoryPath(lyrPath) Then
                If state = lyricState.MODIFIED Then
                    saveLyrics(Not autoSave)
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
        Dim path As String = lyrPath & track.name & ".txt"
        If Not File.Exists(path) Then
            Directory.CreateDirectory(lyrPath)
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
        Dim path As String = lyrPath & currLyrTrack.name & ".txt"
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
        If Not File.Exists(lyrPath & track.name & ".txt") Then
            Return False
        End If
        Dim sr As StreamReader = Nothing
        Try
            sr = New StreamReader(lyrPath & track.name & ".txt")
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
        autoSave = checkAutoSave.Checked
        dll.iniWriteValue("Config", "lyricsAutoSave", Convert.ToInt32(autoSave), Form1.inipath)
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
            Dim iniKey As String = "fontLyrics"
            If senderControl.Font.Size >= 1 And senderControl.Font.Size <= 70 Then
                If e.Delta > 0 Then
                    OptionsForm.saveFont(senderControl, New Font(senderControl.Font.FontFamily.Name, senderControl.Font.Size + IIf(senderControl.Font.Size < 70, 1, 0), senderControl.Font.Style), iniKey)
                Else
                    OptionsForm.saveFont(senderControl, New Font(senderControl.Font.FontFamily.Name, senderControl.Font.Size + IIf(senderControl.Font.Size > 1, -1, 0), senderControl.Font.Style), iniKey)
                End If

            End If
            Dim x As Integer = senderControl.Width - 85
            Dim y As Integer = Cursor.Position.Y - senderControl.Top - Me.Top - 40
            Form1.ttShow(senderControl.Font.Size, senderControl, x, y, 1500)
        End If
    End Sub
End Class