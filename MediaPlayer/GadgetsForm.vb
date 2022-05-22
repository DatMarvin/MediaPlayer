'06.05.2020


Public Class GadgetsForm

    ReadOnly Property inipath As String
        Get
            Return Form1.inipath
        End Get
    End Property

    Public Shared ReadOnly Property dll As Class1
        Get
            Return Form1.dll
        End Get
    End Property

    Enum GadgetState
        NONE
        CLICK_COUNTER
        CURSOR_MOVER
        AUTO_CLICKER
        MACROS
        AUTOSTARTS
        KEYLOGGER
    End Enum

    Public state As GadgetState

    Private Sub GadgetsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Size = New Size(400, 400)
        Me.Location = New Point(Form1.Left + Form1.Width / 2 - Me.Width / 2, Form1.Top + Form1.Height / 2 - Me.Height / 2)
        colorForm()
        If state = GadgetState.NONE Then
            If Form1.lastGadgetsState = GadgetState.NONE Then
                init(0)
            Else
                init(Form1.lastGadgetsState)
            End If
        Else
            init(state)
        End If

    End Sub
    Private Sub GadgetsForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        saveChanges()
        Form1.lastGadgetsState = state
    End Sub

    Sub init(state As GadgetState)
        For Each c As Control In Controls
            If TypeOf c Is GroupBox Then
                c.Visible = False
            End If
        Next
        selectIndex(state)

        If state > 0 Then
            listMenu.Size = New Size(133, ClientSize.Height)
            listMenu.Location = New Point(1, 1)
            If Controls.ContainsKey("g" & state) Then
                Controls("g" & state).Location = New Point(listMenu.Right + 3, 1)
                Controls("g" & state).Visible = True
            End If
        End If

        Select Case state
            Case GadgetState.CLICK_COUNTER
                checkCc.Checked = Form1.clickCounter
                fillClickCounterHistory()
                If clickCounterHistoryList.Items.Count > 0 Then
                    clickCounterHistoryList.SelectedIndex = 0
                End If
                clickCounterCumultative()
                averageUnitCombo.SelectedIndex = 2
            Case GadgetState.CURSOR_MOVER
                checkCursorMover.Checked = Form1.cursorMover
                numIncr.Value = Form1.cursorMoverIncr
                numDelay.Value = Form1.cursorMoverDelay
            Case GadgetState.AUTO_CLICKER
                checkAutoClicker.Checked = Form1.autoClicker
                numFreq.Value = Form1.autoClickerFreq
                numRep.Value = Form1.autoClickerRep
            Case GadgetState.MACROS
                checkMacros.Checked = Form1.macrosEnabled
                fillMacrosList()
            Case GadgetState.AUTOSTARTS
                checkAutostart.Checked = Form1.autostarts
                fillAutostartList()
                If autostartList.Items.Count > 0 Then autostartList.SelectedIndex = 0
            Case GadgetState.KEYLOGGER
                checkKeylogger.Checked = Form1.keylogger
                KeyloggerModule.updateKeyLoggerOutputPath(Form1.dll.iniReadValue("Config", "keyloggerPath", "", Form1.inipath), False)
                textKeyloggerPath.Text = KeyloggerModule.keyloggerOutputPath
                checkKeyloggerAllowHotkeys.Checked = KeyloggerModule.allowHotkeys
                checkKeyloggerRecordWindow.Checked = KeyloggerModule.recordWindow
                textKeyloggerBuffer.Text = KeyloggerModule.keylogBuffer
        End Select

    End Sub

    Function indexToState(ByVal index As Integer) As GadgetState
        Select Case index
            Case 0 : Return GadgetState.CLICK_COUNTER
            Case 1 : Return GadgetState.CURSOR_MOVER
            Case 2 : Return GadgetState.AUTO_CLICKER
            Case 3 : Return GadgetState.MACROS
            Case 4 : Return GadgetState.AUTOSTARTS
            Case 5 : Return GadgetState.KEYLOGGER
            Case Else : Return GadgetState.NONE
        End Select
    End Function
    Sub init(ByVal listIndex As Integer)
        init(indexToState(listIndex))
        listMenu.SelectedIndex = listIndex
    End Sub
    Function stateToIndex(ByVal state As GadgetState) As Integer
        Select Case state
            Case GadgetState.CLICK_COUNTER : Return 0
            Case GadgetState.CURSOR_MOVER : Return 1
            Case GadgetState.AUTO_CLICKER : Return 2
            Case GadgetState.MACROS : Return 3
            Case GadgetState.AUTOSTARTS : Return 4
            Case GadgetState.KEYLOGGER : Return 5
            Case Else : Return -1
        End Select
    End Function
    Sub selectIndex(ByVal state As GadgetState)
        listMenu.SelectedIndex = stateToIndex(state)
        Me.state = state
    End Sub
    Private Sub listMenu_MouseClick(sender As Object, e As MouseEventArgs) Handles listMenu.MouseClick
        Dim it As Integer = sender.IndexFromPoint(New Point(Cursor.Position.X - sender.PointToScreen(New Point(sender.Left, sender.Top)).X + sender.Left, Cursor.Position.Y - sender.PointToScreen(New Point(sender.Left, sender.Top)).Y + sender.top))
        If it > -1 Then
            If Not state = indexToState(listMenu.SelectedIndex) Then
                If saveChanges() Then
                    Text = "Gadgets"
                    init(listMenu.SelectedIndex)
                Else
                    selectIndex(state)
                End If
            End If
        End If
    End Sub

    Function saveChanges() As Boolean
        Select Case state
            Case GadgetState.CURSOR_MOVER
                Form1.cursorMoverIncr = numIncr.Value
                dll.iniWriteValue("Config", "cursorMoverIncr", numIncr.Value, inipath)
                Form1.cursorMoverDelay = numDelay.Value
                dll.iniWriteValue("Config", "cursorMoverDelay", numDelay.Value, inipath)
            Case GadgetState.AUTO_CLICKER
                Form1.autoClickerFreq = numFreq.Value
                Form1.clickerTimer.Interval = numFreq.Value
                dll.iniWriteValue("Config", "autoClickerFreq", numFreq.Value, inipath)
                Form1.autoClickerRep = numRep.Value
                dll.iniWriteValue("Config", "autoClickerRep", numRep.Value, inipath)
            Case GadgetState.MACROS
                saveMacros(False)
            Case GadgetState.AUTOSTARTS
                saveAutoStart(False)
            Case GadgetState.KEYLOGGER
                KeyloggerModule.updateKeyLoggerOutputPath(textKeyloggerPath.Text)
                dll.iniWriteValue("Config", "keyloggerPath", textKeyloggerPath.Text, inipath)
        End Select
        Return True
    End Function

    Sub colorForm() '06.08.19
        If inipath = "" Then Return
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


#Region "ClickCounter"
    Sub fillClickCounterHistory()

        clickCounterHistoryList.Items.Clear()
        Dim allSecs() As String = dll.iniGetAllSections(inipath)
        Dim clickSecs As New List(Of String)
        If allSecs IsNot Nothing Then
            For Each sec In allSecs
                If sec.ToLower.StartsWith("clicks") Then
                    If sec.ToLower = "clicks" Then
                        clickSecs.Add(Now.ToShortDateString())
                    Else
                        clickSecs.Add(sec.Substring(6))
                    End If
                End If
            Next
        End If
        Dim secArray() = clickSecs.ToArray()
        Array.Sort(secArray, New ClickHistoryComparer())
        clickCounterHistoryList.Items.AddRange(secArray)
    End Sub

    Class ClickHistoryComparer
        Implements IComparer
        Public Sub New()
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Dim dt1 = Date.Parse(CStr(x))
            Dim dt2 = Date.Parse(CStr(y))
            Return dt2.CompareTo(dt1)
        End Function
    End Class

    Private Sub clickCounterHistoryList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles clickCounterHistoryList.SelectedIndexChanged
        If clickCounterHistoryList.SelectedIndex > -1 Then
            Dim timestamp As String = clickCounterHistoryList.SelectedItem
            labelLeftCLick.Text = "Left: " & getClickCounterValue(timestamp, "Left").ToString("n0")
            labelRightClick.Text = "Right: " & getClickCounterValue(timestamp, "Right").ToString("n0")
            labelMiddleClick.Text = "Middle: " & getClickCounterValue(timestamp, "Middle").ToString("n0")
            labelTotalClick.Text = "Total: " & getClickCounterValue(timestamp, "Total").ToString("n0")
        End If
    End Sub

    Function getClickCounterValue(timestamp As String, key As String) As Integer
        Dim secName As String = "Clicks"
        If Not CDate(timestamp).CompareTo(CDate(Now.ToShortDateString())) = 0 Then
            secName &= timestamp
        End If
        Return dll.iniReadValue(secName, key, 0, inipath)
    End Function

    Function getClickCounterValueTotal(key As String) As Integer
        Dim total As Integer = 0
        If clickCounterHistoryList.Items.Count > 0 Then
            For Each val As String In clickCounterHistoryList.Items
                total += getClickCounterValue(val, key)
            Next
        End If
        Return total
    End Function
    Sub clickCounterCumultative()
        labelLeftTotal.Text = "Left: " & getClickCounterValueTotal("Left").ToString("n0")
        labelRightTotal.Text = "Right: " & getClickCounterValueTotal("Right").ToString("n0")
        labelMiddleTotal.Text = "Middle: " & getClickCounterValueTotal("Middle").ToString("n0")
        labelTotalTotal.Text = "Total: " & getClickCounterValueTotal("Total").ToString("n0")
    End Sub

    Function getFirstClickOccurence(key As String) As String
        For i = clickCounterHistoryList.Items.Count - 1 To 0 Step -1
            If getClickCounterValue(clickCounterHistoryList.Items(i), key) > 0 Then
                Return clickCounterHistoryList.Items(i)
            End If
        Next
        Return Now.ToShortDateString()
    End Function

    Function getAverageDiff(key As String) As Integer
        Dim f As Date = Date.Parse(getFirstClickOccurence(key))
        Dim diffSpan As TimeSpan = Now.Subtract(f)
        Dim diff As Integer = diffSpan.TotalDays
        Select Case averageUnitCombo.SelectedItem
            Case "Year" : diff = CInt(diffSpan.TotalDays / 365)
            Case "Month" : diff = CInt(diffSpan.TotalDays / 30.5)
            Case "Day" : diff = diffSpan.TotalDays
            Case "Hour" : diff = diffSpan.TotalHours
            Case "Minute" : diff = diffSpan.TotalMinutes
        End Select
        If diff = 0 Then diff = 1
        Return diff
    End Function
    Sub clickCounterAverage()
        labelLeftAv.Text = "Left: " & CInt(getClickCounterValueTotal("Left") / getAverageDiff("Left")).ToString("n0")
        labelRightAv.Text = "Right: " & CInt(getClickCounterValueTotal("Right") / getAverageDiff("Right")).ToString("n0")
        labelMiddleAv.Text = "Middle: " & CInt(getClickCounterValueTotal("Middle") / getAverageDiff("Middle")).ToString("n0")
        labelTotalAv.Text = "Total: " & CInt(getClickCounterValueTotal("Total") / getAverageDiff("Total")).ToString("n0")
    End Sub

    Private Sub averageUnitCombo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles averageUnitCombo.SelectedIndexChanged
        If averageUnitCombo.SelectedIndex = -1 Then averageUnitCombo.SelectedIndex = 0
        clickCounterAverage()
    End Sub

    Private Sub resetButton_Click(sender As Object, e As EventArgs) Handles resetButton.Click
        dll.iniWriteValue("Clicks" & Now.ToShortDateString, "Left", labelLeftCLick.Text.Substring(labelLeftCLick.Text.IndexOf(":") + 2).Replace(".", ""), Form1.inipath)
        dll.iniWriteValue("Clicks" & Now.ToShortDateString, "Right", labelRightClick.Text.Substring(labelRightClick.Text.IndexOf(":") + 2).Replace(".", ""), Form1.inipath)
        dll.iniWriteValue("Clicks" & Now.ToShortDateString, "Middle", labelMiddleClick.Text.Substring(labelMiddleClick.Text.IndexOf(":") + 2).Replace(".", ""), Form1.inipath)
        dll.iniWriteValue("Clicks" & Now.ToShortDateString, "Total", labelTotalClick.Text.Substring(labelTotalClick.Text.IndexOf(":") + 2).Replace(".", ""), Form1.inipath)
        labelLeftCLick.Text = "Left: 0"
        labelRightClick.Text = "Right: 0"
        labelMiddleClick.Text = "Middle: 0"
        labelTotalClick.Text = "Total: 0"
        dll.iniWriteValue("Clicks", "Left", 0, Form1.inipath)
        dll.iniWriteValue("Clicks", "Right", 0, Form1.inipath)
        dll.iniWriteValue("Clicks", "Middle", 0, Form1.inipath)
        dll.iniWriteValue("Clicks", "Total", 0, Form1.inipath)

    End Sub

    Private Sub checkCc_CheckedChanged(sender As Object, e As EventArgs) Handles checkCc.CheckedChanged
        Form1.clickCounter = sender.checked
        dll.iniWriteValue("Config", "clickCounter", Math.Abs(CInt(sender.checked)))
    End Sub


#End Region


#Region "Autostart"

    Sub fillAutostartList()
        autostartList.Items.Clear()
        Dim allKeys() As String = dll.iniGetAllKeys("Autostarts", inipath)
        If allKeys IsNot Nothing Then
            For Each key In allKeys
                autostartList.Items.Add(key)
            Next
        End If
    End Sub
    Private Sub autostartList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles autostartList.SelectedIndexChanged
        If autostartList.SelectedIndex > -1 Then
            Dim name As String = autostartList.SelectedItem
            textboxAutostartName.Text = name
            textboxAutostartPath.Text = getAutostartPath(name)
            textboxAutostartArgs.Text = getAutostartArgs(name)
            checkAutostartActive.Checked = getAutostartActive(name)
        End If
    End Sub

    Function getAutostartPath(name As String) As String
        Dim raw = dll.iniReadValue("Autostarts", name, "", inipath, 2048).Split(";")
        If raw IsNot Nothing AndAlso raw.Length > 1 Then
            Return raw(1)
        End If
        Return ""
    End Function
    Function getAutostartArgs(name As String) As String
        Dim raw = dll.iniReadValue("Autostarts", name, "", inipath, 2048).Split(";")
        If raw IsNot Nothing AndAlso raw.Length > 2 Then
            Return raw(2)
        End If
        Return ""
    End Function

    Function getAutostartActive(name As String) As Boolean
        Dim raw = dll.iniReadValue("Autostarts", name, "", inipath, 2048).Split(";")
        If raw IsNot Nothing AndAlso raw.Length > 0 Then
            Return CBool(raw(0))
        End If
        Return False
    End Function

    Sub saveAutoStart(showConfirmation As Boolean)
        If autostartList.SelectedIndex > -1 Then
            Dim prevName As String = textboxAutostartName.Text
            autostartSave()
            fillAutostartList()
            If autostartList.Items.Contains(prevName) Then
                autostartList.SelectedItem = prevName
            End If
            If showConfirmation Then MsgBox("Autostart '" & prevName & "' saved")
        End If
    End Sub
    Private Sub autostartSaveButton_Click(sender As Object, e As EventArgs) Handles autostartSaveButton.Click
        saveAutoStart(True)
    End Sub
    Private Sub checkAutostartActive_CheckedChanged(sender As Object, e As EventArgs) Handles checkAutostartActive.CheckedChanged
        autostartSave()
    End Sub

    Sub autostartSave()
        Dim name As String = textboxAutostartName.Text
        Dim s As String = Math.Abs(CInt(checkAutostartActive.Checked)) & ";" & textboxAutostartPath.Text & ";" & textboxAutostartArgs.Text
        dll.iniWriteValue("Autostarts", name, s, inipath)
    End Sub

    Function autoStartExists(name As String) As Boolean
        Dim keys() As String = dll.iniGetAllKeys("Autostarts", inipath)
        If keys IsNot Nothing Then
            For Each value As String In keys
                If value.ToLower = name.ToLower Then Return True
            Next
        End If
        Return False
    End Function


    Private Sub autostartAddButton_Click(sender As Object, e As EventArgs) Handles autostartAddButton.Click
        Dim newName As String = InputBox("Type in name")
        If newName = "" Or autoStartExists(newName) Then
            MsgBox("Name not allowed", MsgBoxStyle.Exclamation)
        Else
            textboxAutostartName.Text = newName
            dll.iniWriteValue("Autostarts", newName, "1", inipath)
            autostartList.Items.Add(newName)
            autostartList.SelectedItem = newName
            checkAutostartActive.Checked = True
        End If
    End Sub

    Private Sub autostartRemoveButton_Click(sender As Object, e As EventArgs) Handles autostartRemoveButton.Click
        If autostartList.SelectedIndex > -1 Then
            textboxAutostartName.Text = ""
            textboxAutostartPath.Text = ""
            textboxAutostartArgs.Text = ""
            dll.iniDeleteKey("Autostarts", autostartList.SelectedItem, inipath)
            fillAutostartList()
        End If
    End Sub


    Private Sub checkAutostart_CheckedChanged(sender As Object, e As EventArgs) Handles checkAutostart.CheckedChanged
        Form1.autostarts = sender.checked
        dll.iniWriteValue("Config", "autostarts", Math.Abs(CInt(sender.checked)))
    End Sub

    Private Sub autostartFileDialogButton_Click(sender As Object, e As EventArgs) Handles autostartFileDialogButton.Click
        Dim res = Form1.getFileDialog(My.Application.Info.DirectoryPath & "\")
        If Not res = "" Then
            textboxAutostartPath.Text = res
        End If
    End Sub

    Private Sub autostartRunButton_Click(sender As Object, e As EventArgs) Handles autostartRunButton.Click
        If Not textboxAutostartPath.Text = "" Then
            Try
                Process.Start(textboxAutostartPath.Text, textboxAutostartArgs.Text)
            Catch ex As Exception
                MsgBox("Failed to start process. Error message: " & vbNewLine & vbNewLine & ex.Message, MsgBoxStyle.Critical)
            End Try
        End If
    End Sub

#End Region


#Region "Cursor Mover" '01.06.2020

    Private Sub checkCursorMover_CheckedChanged(sender As Object, e As EventArgs) Handles checkCursorMover.CheckedChanged
        Form1.cursorMover = sender.checked
        dll.iniWriteValue("Config", "cursorMover", Math.Abs(CInt(sender.checked)))
    End Sub

    Private Sub numIncr_ValueChanged(sender As Object, e As EventArgs) Handles numIncr.ValueChanged

    End Sub

    Private Sub numDelay_ValueChanged(sender As Object, e As EventArgs) Handles numDelay.ValueChanged

    End Sub

    Private Sub cursorMoverUpButton_Click(sender As Object, e As EventArgs) Handles cursorMoverUpButton.Click
        Form1.showOptions(OptionsForm.optionState.KEYSET,,, {Key.keyName.Cursor_Up})
    End Sub

    Private Sub cursorMoverLeftButton_Click(sender As Object, e As EventArgs) Handles cursorMoverLeftButton.Click
        Form1.showOptions(OptionsForm.optionState.KEYSET,,, {Key.keyName.Cursor_Left})
    End Sub

    Private Sub cursorMoverRightButton_Click(sender As Object, e As EventArgs) Handles cursorMoverRightButton.Click
        Form1.showOptions(OptionsForm.optionState.KEYSET,,, {Key.keyName.Cursor_Right})
    End Sub

    Private Sub cursorMoverDownButton_Click(sender As Object, e As EventArgs) Handles cursorMoverDownButton.Click
        Form1.showOptions(OptionsForm.optionState.KEYSET,,, {Key.keyName.Cursor_Down})
    End Sub

#End Region

#Region "Auto Clicker"
    Private Sub autoClickerEnableButton_Click(sender As Object, e As EventArgs) Handles autoClickerEnableButton.Click
        Form1.showOptions(OptionsForm.optionState.KEYSET,,, {Key.keyName.Clicker_On})
    End Sub

    Private Sub autoClickerDisableButton_Click(sender As Object, e As EventArgs) Handles autoClickerDisableButton.Click
        Form1.showOptions(OptionsForm.optionState.KEYSET,,, {Key.keyName.Clicker_Off})
    End Sub

    Private Sub checkAutoClicker_CheckedChanged(sender As Object, e As EventArgs) Handles checkAutoClicker.CheckedChanged
        Form1.autoClicker = sender.checked
        dll.iniWriteValue("Config", "autoClicker", Math.Abs(CInt(sender.checked)))
        If Not Form1.autoClicker Then
            Form1.clickerTimer.Stop()
        End If
    End Sub


#End Region

#Region "Macros"

    Public Const MACROS_COUNT = 6

    Public Class Macro
        Public id As Integer
        Public name As String
        Public active As Boolean
        Public path As String
        Public args As String
        Public hotkeyOverride As Boolean

        Public Sub New(id As Integer, name As String)
            Me.id = id
            Me.name = name
        End Sub

        Public Overrides Function ToString() As String
            Return name
        End Function
    End Class

    Public Shared macros(MACROS_COUNT - 1) As Macro
    Public Sub initMacrosTable()
        For i = 0 To MACROS_COUNT - 1
            macros(i) = getMacroFromIni(i)
        Next
    End Sub

    Private Sub checkMacros_CheckedChanged(sender As Object, e As EventArgs) Handles checkMacros.CheckedChanged
        Form1.macrosEnabled = sender.checked
        dll.iniWriteValue("Config", "macrosEnabled", Math.Abs(CInt(sender.checked)))
    End Sub

    Private Sub macroFileButton_Click(sender As Object, e As EventArgs) Handles macroFileButton.Click
        Dim res = Form1.getFileDialog(My.Application.Info.DirectoryPath & "\")
        If Not res = "" Then
            textMacroPath.Text = res
        End If
    End Sub

    Private Sub macroRunButton_Click(sender As Object, e As EventArgs) Handles macroRunButton.Click
        If Not textMacroPath.Text = "" Then
            Try
                Process.Start(textMacroPath.Text, textMacroArgs.Text)
            Catch ex As Exception
                MsgBox("Failed to start process. Error message: " & vbNewLine & vbNewLine & ex.Message, MsgBoxStyle.Critical)
            End Try
        End If
    End Sub

    Private Sub checkmacroOverride_CheckedChanged(sender As Object, e As EventArgs) Handles checkMacroOverride.CheckedChanged
        macroSave()
    End Sub


    Private Sub macroHotkeyButton_Click(sender As Object, e As EventArgs) Handles macroHotkeyButton.Click
        If comboMacros.SelectedIndex > -1 Then
            Form1.showOptions(OptionsForm.optionState.KEYSET,,, {38 + comboMacros.SelectedIndex})
        End If
    End Sub
    Private Sub comboMacros_SelectedIndexChanged(sender As Object, e As EventArgs) Handles comboMacros.SelectedIndexChanged
        If comboMacros.SelectedIndex = -1 Then
            comboMacros.SelectedIndex = 0
        End If

        Dim macro As Macro = comboMacros.SelectedItem
        textMacroName.Text = macro.name
        textMacroPath.Text = macro.path
        textMacroArgs.Text = macro.args
        macroActiveCheckbox.Checked = macro.active
        checkMacroOverride.Checked = macro.hotkeyOverride
    End Sub

    Sub fillMacrosList()
        initMacrosTable()
        comboMacros.Items.Clear()
        For i = 0 To MACROS_COUNT - 1
            comboMacros.Items.Add(macros(i))
        Next
        comboMacros.SelectedIndex = 0
    End Sub

    Function getMacroFromIni(index As Integer) As Macro
        Dim res As New Macro(index, "Macro " & index + 1)
        Dim raw = dll.iniReadValue("Macros", index, "", inipath, 2048).Split(";")
        If raw IsNot Nothing AndAlso raw.Length = 5 Then
            res.name = raw(0)
            res.active = raw(1)
            res.path = raw(2)
            res.args = raw(3)
            res.hotkeyOverride = raw(4)
        End If
        Return res
    End Function

    Private Sub macroSaveButton_Click(sender As Object, e As EventArgs) Handles macroSaveButton.Click
        saveMacros(True)
    End Sub

    Private Sub macroActiveCheckbox_CheckedChanged(sender As Object, e As EventArgs) Handles macroActiveCheckbox.CheckedChanged
        macroSave()
    End Sub
    Sub saveMacros(showConfirmation As Boolean)
        If comboMacros.SelectedIndex > -1 Then
            Dim prevIndex = comboMacros.SelectedIndex
            macroSave()
            fillMacrosList()
            comboMacros.SelectedIndex = prevIndex
            If showConfirmation Then MsgBox("Macro '" & comboMacros.Items(prevIndex).ToString() & "' saved")
        End If
    End Sub

    Sub macroSave(Optional index As Integer = -1)
        If index = -1 Then
            index = comboMacros.SelectedIndex
        End If
        Dim name As String = textMacroName.Text
        If String.IsNullOrWhiteSpace(name) Then
            name = "Macro " & index + 1
        End If
        Dim s As String = name & ";" & Math.Abs(CInt(macroActiveCheckbox.Checked)) & ";" & textMacroPath.Text & ";" & textMacroArgs.Text & ";" & Math.Abs(CInt(checkMacroOverride.Checked))
        dll.iniWriteValue("Macros", index, s, inipath)
    End Sub


#End Region


#Region "Keylogger"


    Private Sub keyloggerFileButton_Click(sender As Object, e As EventArgs) Handles keyloggerFileButton.Click
        Dim res = Form1.getFileDialog(My.Application.Info.DirectoryPath & "\")
        If Not res = "" Then
            textKeyloggerPath.Text = res
        End If
    End Sub

    Private Sub checkKeylogger_Click(sender As Object, e As EventArgs) Handles checkKeylogger.Click
        If sender.checked Then
            KeyloggerModule.updateKeyLoggerOutputPath(textKeyloggerPath.Text)
            dll.iniWriteValue("Config", "keyloggerPath", textKeyloggerPath.Text, inipath)
            If Not KeyloggerModule.keyloggerInit(False) Then
                sender.checked = False
                textKeyloggerPath.SelectAll()
            End If
        Else
            KeyloggerModule.keyloggerDestroy()
        End If
        Form1.keylogger = sender.checked
        dll.iniWriteValue("Config", "keylogger", Math.Abs(CInt(sender.checked)))
    End Sub

    Private Sub checkKeylogger_CheckedChanged(sender As Object, e As EventArgs) Handles checkKeylogger.CheckedChanged

    End Sub

    Private Sub keyloggerOpenButton_Click(sender As Object, e As EventArgs) Handles keyloggerOpenButton.Click
        If Not textKeyloggerPath.Text = "" Then
            Try
                Process.Start(textKeyloggerPath.Text)
            Catch ex As Exception
                MsgBox("Failed to start process. Error message: " & vbNewLine & vbNewLine & ex.Message, MsgBoxStyle.Critical)
            End Try
        End If
    End Sub

    Private Sub keyloggerDeleteButton_Click(sender As Object, e As EventArgs) Handles keyloggerDeleteButton.Click
        If IO.File.Exists(textKeyloggerPath.Text) Then
            IO.File.WriteAllText(textKeyloggerPath.Text, String.Empty)
        Else
            MsgBox("File does not exist.", MsgBoxStyle.Exclamation)
        End If
    End Sub

    Private Sub checkKeyloggerAllowHotkeys_CheckedChanged(sender As Object, e As EventArgs) Handles checkKeyloggerAllowHotkeys.CheckedChanged
        KeyloggerModule.allowHotkeys = sender.checked
        dll.iniWriteValue("Config", "keyloggerAllowHotkeys", Math.Abs(CInt(sender.checked)))
    End Sub

    Private Sub checkKeyloggerRecordWindow_CheckedChanged(sender As Object, e As EventArgs) Handles checkKeyloggerRecordWindow.CheckedChanged
        KeyloggerModule.recordWindow = sender.checked
        dll.iniWriteValue("Config", "keyloggerRecordWindow", Math.Abs(CInt(sender.checked)))
    End Sub





#End Region
End Class
