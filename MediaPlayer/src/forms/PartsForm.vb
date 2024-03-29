﻿'04.09.2019
Imports System.IO
Public Class PartsForm

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

    Dim currTrack As Track
    Dim currTrackPart As TrackPart
    Dim queuedTrack As Track
    Dim state As partState
    Public ReadOnly Property autoSave() As Boolean
        Get
            Return SettingsService.getSetting(SettingsIdentifier.TRACK_PARTS_AUTO_SAVE)
        End Get
    End Property
    Public ReadOnly Property playOnChange() As Boolean
        Get
            Return SettingsService.getSetting(SettingsIdentifier.TRACK_PARTS_PLAY_ON_CHANGE)
        End Get
    End Property


    Enum partState
        INIT
        READ
        MODIFIED
    End Enum

    Private Sub PartsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim yNorm As Integer = Form1.Top + Form1.Height / 2 - Me.Height / 2
        Me.Location = New Point(Math.Max(0, Form1.Left - Width), yNorm + Math.Max(0, 0 - yNorm) + Math.Min(0, My.Computer.Screen.WorkingArea.Height - (yNorm + Height)))
        FormUtils.colorForm(Me)

        checkAutoSave.Checked = autoSave
        checkPlayOnChange.Checked = playOnChange
    End Sub

    Private Sub PartsForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If state = partState.MODIFIED Then
            saveParts(Not autoSave)
        End If
        overlayMode -= Form1.eOverlayMode.PARTS
        PlayerInterface.resetLoop()
    End Sub

    Sub setState(toState As partState)
        state = toState
        updateUI()
    End Sub

    Sub updateUI()
        listParts.Size = New Size(Width - 16, Height - 185)
        listParts.Location = New Point(0, 0)
        listParts.Columns(3).Width = listParts.Width - 171
        groupEdit.Size = New Size(370, 110)
        groupEdit.Location = New Point(6, listParts.Bottom + 32)
        buttonAddPart.Location = New Point(groupEdit.Left + 6, groupEdit.Top - buttonAddPart.Height - 3)
        buttonOpenRaw.Location = New Point(groupEdit.Right - buttonOpenRaw.Width - 6, buttonAddPart.Top)

        If currTrackPart Is Nothing Then
            labelPartSelection.Text = "Part:"
            tPartFrom.Text = ""
            tPartTo.Text = ""
            tPartName.Text = ""
        End If
        tPartFrom.Enabled = (currTrackPart IsNot Nothing)
        tPartTo.Enabled = (currTrackPart IsNot Nothing)
        tPartName.Enabled = (currTrackPart IsNot Nothing)
        buttonApplyEdit.Enabled = currTrackPart IsNot Nothing
        buttonDeletePart.Enabled = currTrackPart IsNot Nothing
    End Sub

    Sub loadParts(track As Track)
        If queuedTrack Is Nothing Then
            queuedTrack = track
            If OperatingSystem.isValidDirectoryPath(lyrpath) Then
                If state = partState.MODIFIED Then
                    savePart(Not autoSave)
                End If
                currTrack = queuedTrack
                loadList()
                setState(partState.READ)
                Text = "Parts - " & queuedTrack.name
                queuedTrack = Nothing
            Else
                Form1.showOptions(OptionsForm.optionState.PATHS)
            End If
        Else
            queuedTrack = track
        End If
    End Sub

    Sub loadList()
        currTrack.updateParts()
        listParts.BeginUpdate()
        listParts.Items.Clear()
        Dim selItem As ListViewItem = Nothing
        If currTrack.partsCount > 0 Then
            For i = 0 To currTrack.partsCount - 1
                Dim it As New ListViewItem(i + 1)
                it.SubItems.Add(currTrack.parts(i).fromFormat)
                it.SubItems.Add(currTrack.parts(i).toFormat)
                it.SubItems.Add(currTrack.parts(i).name)
                listParts.Items.Add(it)
                If currTrackPartEquals(currTrack.parts(i)) Then
                    currTrackPart = currTrack.parts(i)
                    selItem = it
                End If
            Next
        End If
        listParts.EndUpdate()
        If selItem Is Nothing Then
            currTrackPart = Nothing
        Else
            listParts.SelectedItems.Clear()
            listParts.SelectedIndices.Add(listParts.Items.IndexOf(selItem))
            selItem.EnsureVisible()
        End If
        updateUI()
    End Sub


    Function writePartsStream() As Boolean
        Dim path As String = lyrPath & currTrack.name & ".ini"
        TrackPart.sortTrackParts(currTrack.parts)
        Try
            For i = 0 To currTrack.partsCount - 1
                Dim tp As TrackPart = currTrack.parts(i)
                IniService.iniWriteValue(i + 1, "time", tp.fromFormat & "," & tp.toFormat, path)
                If tp.name = "" Then
                    IniService.iniDeleteKey(i + 1, "name", path)
                Else
                    IniService.iniWriteValue(i + 1, "name", tp.name, path)
                End If
            Next
            IniService.iniDeleteSection(currTrack.partsCount + 1, path)
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function


    Function saveParts(prompt As Boolean) As Boolean
        If Not prompt OrElse MsgBox("Parts have been modified." & vbNewLine & "Do you want to save changes?", MsgBoxStyle.YesNo, "Parts Manager") = MsgBoxResult.Yes Then
            If playOnChange AndAlso Player.getPlayState() = WMPLib.WMPPlayState.wmppsPlaying Then
                PlayerInterface.resetLoop()
            End If
1:          If state = partState.MODIFIED Then
                If writePartsStream() Then
                    setState(partState.READ)
                    Form1.labelStatsUpdate()
                Else
                    If MsgBox("Failed to write to file. Try again?", MsgBoxStyle.YesNo + MsgBoxStyle.Critical, "Parts Manager") = MsgBoxResult.Yes Then
                        GoTo 1
                    Else
                        Return False
                    End If
                End If
            End If
            loadList()
            Return True
        Else
            Return False
        End If
    End Function

    Function savePart(prompt As Boolean) As Boolean
        If currTrackPart IsNot Nothing Then
            If Not prompt OrElse MsgBox("Part " & currTrackPart.id + 1 & " has been modified." & vbNewLine & "Do you want to save changes?", MsgBoxStyle.YesNo, "Parts Manager") = MsgBoxResult.Yes Then
                currTrackPart.fromSec = dll.minFormatToSec(tPartFrom.Text)
                currTrackPart.toSec = dll.minFormatToSec(tPartTo.Text)
                currTrackPart.name = tPartName.Text

                currTrack.parts(currTrackPart.id).fromSec = currTrackPart.fromSec
                currTrack.parts(currTrackPart.id).toSec = currTrackPart.toSec
                currTrack.parts(currTrackPart.id).name = currTrackPart.name

                tPartFrom.Text = currTrackPart.fromFormat
                tPartTo.Text = currTrackPart.toFormat
                tPartName.Text = currTrackPart.name
                Return saveParts(False)
            Else
                setState(partState.READ)
            End If
        End If
        Return False
    End Function

    Private Sub listParts_SelectedIndexChanged(sender As Object, e As EventArgs) Handles listParts.SelectedIndexChanged
        If listParts.SelectedIndices.Count > 0 AndAlso listParts.SelectedIndices(0) > -1 Then
            If state = partState.MODIFIED Then
                savePart(Not autoSave)
            End If
            Dim tp As TrackPart = currTrack.parts(listParts.SelectedIndices(0))
            currTrackPart = tp
            labelPartSelection.Text = "Part:   " & tp.id + 1
            tPartFrom.Text = tp.fromFormat
            tPartTo.Text = tp.toFormat
            tPartName.Text = tp.name
            updateUI()
        End If
    End Sub

    Private Sub listParts_ItemActivate(sender As Object, e As EventArgs) Handles listParts.ItemActivate
        If listParts.SelectedIndices.Count > 0 AndAlso listParts.SelectedIndices(0) > -1 Then
            If Not radioEnabled Then
                PlayerInterface.launchTrack(currTrack)
                Form1.setLoop(currTrackPart.fromSec, currTrackPart.toSec)
            End If
        End If
    End Sub
    Private Sub listParts_Click(sender As Object, e As EventArgs) Handles listParts.Click
        Dim inf As ListViewHitTestInfo = listParts.HitTest(New Point(Cursor.Position.X - sender.PointToScreen(New Point(sender.Left, sender.Top)).X + sender.Left, Cursor.Position.Y - sender.PointToScreen(New Point(sender.Left, sender.Top)).Y + sender.top))
        For i = 0 To listParts.Items.Count - 1
            If listParts.Items(i).Equals(inf.Item) Then
                If listParts.Items(i).SubItems(1).Equals(inf.SubItem) Then tPartFrom.Focus()
                If listParts.Items(i).SubItems(2).Equals(inf.SubItem) Then tPartTo.Focus()
                If listParts.Items(i).SubItems(3).Equals(inf.SubItem) Then tPartName.Focus()
            End If
        Next
    End Sub


    Dim changeFromKey As Boolean = False
    Private Sub tPartFrom_TextChanged(sender As Object, e As EventArgs) Handles tPartFrom.TextChanged
        If changeFromKey And playOnChange Then
            changeFromKey = False
            Dim secs As Integer = dll.minFormatToSec(tPartFrom.Text)
            currTrack.updateLength()
            If dll.minFormatToSec(tPartTo.Text) > secs And secs >= 0 And secs < currTrack.length Then
                PlayerInterface.launchTrack(currTrack)
                Form1.setLoop(secs, Math.Min(secs + 2, currTrack.length))
                Player.setCurrentPosition(secs)
                currTrack.currPart = currTrack.getCurrentPart()
            Else
                PlayerInterface.resetLoop()
                Player.pause()
            End If
        End If
    End Sub
    Private Sub tPartTo_TextChanged(sender As Object, e As EventArgs) Handles tPartTo.TextChanged
        If changeFromKey And playOnChange Then
            changeFromKey = False
            Dim secs As Integer = dll.minFormatToSec(tPartTo.Text)
            currTrack.updateLength()
            If dll.minFormatToSec(tPartFrom.Text) < secs And secs <= currTrack.length And secs > 0 Then
                PlayerInterface.launchTrack(currTrack)
                Form1.setLoop(Math.Max(0, secs - 2), secs)
                Player.setCurrentPosition(Math.Max(0, secs - 2))
                currTrack.currPart = currTrack.getCurrentPart()
            Else
                PlayerInterface.resetLoop()
                Player.pause()
            End If
        End If
    End Sub
    Private Sub tPartFrom_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tPartFrom.KeyPress
        If Char.IsLetter(e.KeyChar) Or Char.IsSymbol(e.KeyChar) Or Char.IsWhiteSpace(e.KeyChar) Then
            e.Handled = True
        Else
            setState(partState.MODIFIED)
            changeFromKey = True
        End If
    End Sub

    Private Sub tPartTo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tPartTo.KeyPress
        If Char.IsLetter(e.KeyChar) Or Char.IsSymbol(e.KeyChar) Or Char.IsWhiteSpace(e.KeyChar) Then
            e.Handled = True
        Else
            setState(partState.MODIFIED)
            changeFromKey = True
        End If
    End Sub


    Private Sub tPartName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tPartName.KeyPress
        setState(partState.MODIFIED)
    End Sub

    Private Sub buttonApplyEdit_Click(sender As Object, e As EventArgs) Handles buttonApplyEdit.Click
        savePart(False)
    End Sub

    Private Sub buttonDeletePart_Click(sender As Object, e As EventArgs) Handles buttonDeletePart.Click
        If currTrackPart IsNot Nothing Then
            If Not currTrack.parts.Remove(currTrackPart) Then 'not working if all parts added and then one deleted
                For i = 0 To currTrack.partsCount - 1
                    If currTrackPartEquals(currTrack.parts(i)) Then
                        currTrack.parts.RemoveAt(i)
                    End If
                Next
            End If
            currTrack.partsCount -= 1
            currTrackPart = Nothing
            tPartFrom.Text = ""
            tPartTo.Text = ""
            tPartName.Text = ""
            labelPartSelection.Text = "Part:"
            setState(partState.MODIFIED)
            saveParts(False)
        End If
    End Sub

    Function currTrackPartEquals(part As TrackPart) As Boolean
        If currTrackPart IsNot Nothing Then
            If part.fromSec = currTrackPart.fromSec And part.toSec = currTrackPart.toSec And part.name = currTrackPart.name Then
                Return True
            End If
        End If
        Return False
    End Function

    Private Sub PartsForm_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        updateUI()
    End Sub
    Private Sub PartsForm_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        updateUI()
    End Sub

    Private Sub buttonAddPart_Click(sender As Object, e As EventArgs) Handles buttonAddPart.Click
        If state = partState.MODIFIED Then
            savePart(Not autoSave)
        End If
        If currTrack.parts Is Nothing Then currTrack.parts = New List(Of TrackPart)
        Dim fromTime As String = "00:00"
        currTrack.updateLength()
        Dim toTime As String = dll.secondsTo_ms_Format(currTrack.length)
        If currTrack.partsCount > 0 Then
            fromTime = dll.secondsTo_ms_Format(Math.Min(currTrack.parts(currTrack.partsCount - 1).toSec, currTrack.length - 1))
        End If
        Dim newPart As New TrackPart(Form1, currTrack, currTrack.partsCount, {fromTime & "," & toTime, ""})

        currTrack.parts.Add(newPart)
        currTrack.partsCount += 1
        currTrackPart = newPart
        setState(partState.MODIFIED)
        If saveParts(False) Then
            listParts.SelectedIndices.Clear()
            listParts.SelectedIndices.Add(listParts.Items.Count - 1)
            listParts.SelectedItems(0).EnsureVisible()
            tPartFrom.Select()
        End If
    End Sub

    Private Sub buttonOpenRaw_Click(sender As Object, e As EventArgs) Handles buttonOpenRaw.Click
        Dim para As String = lyrPath & currTrack.name & ".ini"
        Process.Start("notepad.exe", para)
    End Sub

    Private Sub checkAutoSave_CheckedChanged(sender As Object, e As EventArgs) Handles checkAutoSave.CheckedChanged
        SettingsService.saveSetting(SettingsIdentifier.TRACK_PARTS_AUTO_SAVE, sender.checked)
    End Sub

    Private Sub checkPlayOnChange_CheckedChanged(sender As Object, e As EventArgs) Handles checkPlayOnChange.CheckedChanged
        SettingsService.saveSetting(SettingsIdentifier.TRACK_PARTS_PLAY_ON_CHANGE, sender.checked)
    End Sub
    Private Sub checkPlayOnChange_Click(sender As Object, e As EventArgs) Handles checkPlayOnChange.Click
        If Not playOnChange And Player.getUrl().ToLower = currTrack.fullPath.ToLower And Player.getPlayState() = WMPLib.WMPPlayState.wmppsPlaying Then
            PlayerInterface.resetLoop()
            Player.pause()
        End If
    End Sub
End Class