<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class GadgetsForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GadgetsForm))
        Me.listMenu = New System.Windows.Forms.ListBox()
        Me.g1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.averageUnitCombo = New System.Windows.Forms.ComboBox()
        Me.labelLeftAv = New System.Windows.Forms.Label()
        Me.labelRightAv = New System.Windows.Forms.Label()
        Me.labelTotalAv = New System.Windows.Forms.Label()
        Me.labelMiddleAv = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.labelLeftTotal = New System.Windows.Forms.Label()
        Me.labelRightTotal = New System.Windows.Forms.Label()
        Me.labelTotalTotal = New System.Windows.Forms.Label()
        Me.labelMiddleTotal = New System.Windows.Forms.Label()
        Me.clickCounterHistoryList = New System.Windows.Forms.ListBox()
        Me.labelMiddleClick = New System.Windows.Forms.Label()
        Me.checkCc = New System.Windows.Forms.CheckBox()
        Me.resetButton = New System.Windows.Forms.Button()
        Me.labelTotalClick = New System.Windows.Forms.Label()
        Me.labelRightClick = New System.Windows.Forms.Label()
        Me.labelLeftCLick = New System.Windows.Forms.Label()
        Me.g5 = New System.Windows.Forms.GroupBox()
        Me.autostartRemoveButton = New System.Windows.Forms.Button()
        Me.autostartAddButton = New System.Windows.Forms.Button()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.autostartRunButton = New System.Windows.Forms.Button()
        Me.checkAutostartActive = New System.Windows.Forms.CheckBox()
        Me.autostartSaveButton = New System.Windows.Forms.Button()
        Me.autostartFileDialogButton = New System.Windows.Forms.Button()
        Me.textboxAutostartArgs = New System.Windows.Forms.TextBox()
        Me.labelAutostartArgs = New System.Windows.Forms.Label()
        Me.textboxAutostartPath = New System.Windows.Forms.TextBox()
        Me.textboxAutostartName = New System.Windows.Forms.TextBox()
        Me.labelAutostartPath = New System.Windows.Forms.Label()
        Me.labelAutostartName = New System.Windows.Forms.Label()
        Me.autostartList = New System.Windows.Forms.ListBox()
        Me.checkAutostart = New System.Windows.Forms.CheckBox()
        Me.g2 = New System.Windows.Forms.GroupBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.cursorMoverLeftButton = New System.Windows.Forms.Button()
        Me.cursorMoverDownButton = New System.Windows.Forms.Button()
        Me.cursorMoverRightButton = New System.Windows.Forms.Button()
        Me.cursorMoverUpButton = New System.Windows.Forms.Button()
        Me.delayLabel = New System.Windows.Forms.Label()
        Me.numDelay = New System.Windows.Forms.NumericUpDown()
        Me.labelIncr = New System.Windows.Forms.Label()
        Me.numIncr = New System.Windows.Forms.NumericUpDown()
        Me.checkCursorMover = New System.Windows.Forms.CheckBox()
        Me.g3 = New System.Windows.Forms.GroupBox()
        Me.labelAutoClickerRepititions = New System.Windows.Forms.Label()
        Me.labelFreq = New System.Windows.Forms.Label()
        Me.numFreq = New System.Windows.Forms.NumericUpDown()
        Me.numRep = New System.Windows.Forms.NumericUpDown()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.autoClickerDisableButton = New System.Windows.Forms.Button()
        Me.autoClickerEnableButton = New System.Windows.Forms.Button()
        Me.checkAutoClicker = New System.Windows.Forms.CheckBox()
        Me.g4 = New System.Windows.Forms.GroupBox()
        Me.comboMacros = New System.Windows.Forms.ComboBox()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.textMacroName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.macroActiveCheckbox = New System.Windows.Forms.CheckBox()
        Me.checkMacroOverride = New System.Windows.Forms.CheckBox()
        Me.macroSaveButton = New System.Windows.Forms.Button()
        Me.textMacroArgs = New System.Windows.Forms.TextBox()
        Me.macroHotkeyButton = New System.Windows.Forms.Button()
        Me.labelCommandCenterArgs = New System.Windows.Forms.Label()
        Me.textMacroPath = New System.Windows.Forms.TextBox()
        Me.macroRunButton = New System.Windows.Forms.Button()
        Me.labelCommandCenterPath = New System.Windows.Forms.Label()
        Me.macroFileButton = New System.Windows.Forms.Button()
        Me.checkMacros = New System.Windows.Forms.CheckBox()
        Me.g6 = New System.Windows.Forms.GroupBox()
        Me.checkKeyloggerAllowHotkeys = New System.Windows.Forms.CheckBox()
        Me.GroupBox9 = New System.Windows.Forms.GroupBox()
        Me.textKeyloggerPath = New System.Windows.Forms.TextBox()
        Me.keyloggerOpenButton = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.keyloggerFileButton = New System.Windows.Forms.Button()
        Me.checkKeylogger = New System.Windows.Forms.CheckBox()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.checkKeyloggerRecordWindow = New System.Windows.Forms.CheckBox()
        Me.textKeyloggerBuffer = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.keyloggerDeleteButton = New System.Windows.Forms.Button()
        Me.g1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.g5.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.g2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me.numDelay, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numIncr, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.g3.SuspendLayout()
        CType(Me.numFreq, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numRep, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox6.SuspendLayout()
        Me.g4.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.g6.SuspendLayout()
        Me.GroupBox9.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.SuspendLayout()
        '
        'listMenu
        '
        Me.listMenu.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.listMenu.FormattingEnabled = True
        Me.listMenu.HorizontalScrollbar = True
        Me.listMenu.ItemHeight = 16
        Me.listMenu.Items.AddRange(New Object() {"Click Counter", "Cursor Mover", "Auto Clicker", "Macros", "Autostart", "Keylogger"})
        Me.listMenu.Location = New System.Drawing.Point(4, 6)
        Me.listMenu.Name = "listMenu"
        Me.listMenu.Size = New System.Drawing.Size(133, 116)
        Me.listMenu.TabIndex = 9
        '
        'g1
        '
        Me.g1.Controls.Add(Me.GroupBox2)
        Me.g1.Controls.Add(Me.GroupBox1)
        Me.g1.Controls.Add(Me.clickCounterHistoryList)
        Me.g1.Controls.Add(Me.labelMiddleClick)
        Me.g1.Controls.Add(Me.checkCc)
        Me.g1.Controls.Add(Me.resetButton)
        Me.g1.Controls.Add(Me.labelTotalClick)
        Me.g1.Controls.Add(Me.labelRightClick)
        Me.g1.Controls.Add(Me.labelLeftCLick)
        Me.g1.Location = New System.Drawing.Point(141, 5)
        Me.g1.Name = "g1"
        Me.g1.Size = New System.Drawing.Size(240, 353)
        Me.g1.TabIndex = 12
        Me.g1.TabStop = False
        Me.g1.Text = "Click Counter"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.averageUnitCombo)
        Me.GroupBox2.Controls.Add(Me.labelLeftAv)
        Me.GroupBox2.Controls.Add(Me.labelRightAv)
        Me.GroupBox2.Controls.Add(Me.labelTotalAv)
        Me.GroupBox2.Controls.Add(Me.labelMiddleAv)
        Me.GroupBox2.Location = New System.Drawing.Point(6, 242)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(225, 105)
        Me.GroupBox2.TabIndex = 30
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Average"
        '
        'averageUnitCombo
        '
        Me.averageUnitCombo.FormattingEnabled = True
        Me.averageUnitCombo.Items.AddRange(New Object() {"Year", "Month", "Day", "Hour", "Minute"})
        Me.averageUnitCombo.Location = New System.Drawing.Point(140, 12)
        Me.averageUnitCombo.Name = "averageUnitCombo"
        Me.averageUnitCombo.Size = New System.Drawing.Size(80, 21)
        Me.averageUnitCombo.TabIndex = 25
        '
        'labelLeftAv
        '
        Me.labelLeftAv.AutoSize = True
        Me.labelLeftAv.Location = New System.Drawing.Point(19, 38)
        Me.labelLeftAv.Name = "labelLeftAv"
        Me.labelLeftAv.Size = New System.Drawing.Size(28, 13)
        Me.labelLeftAv.TabIndex = 21
        Me.labelLeftAv.Text = "Left:"
        '
        'labelRightAv
        '
        Me.labelRightAv.AutoSize = True
        Me.labelRightAv.Location = New System.Drawing.Point(12, 52)
        Me.labelRightAv.Name = "labelRightAv"
        Me.labelRightAv.Size = New System.Drawing.Size(35, 13)
        Me.labelRightAv.TabIndex = 22
        Me.labelRightAv.Text = "Right:"
        '
        'labelTotalAv
        '
        Me.labelTotalAv.AutoSize = True
        Me.labelTotalAv.Location = New System.Drawing.Point(13, 83)
        Me.labelTotalAv.Name = "labelTotalAv"
        Me.labelTotalAv.Size = New System.Drawing.Size(34, 13)
        Me.labelTotalAv.TabIndex = 23
        Me.labelTotalAv.Text = "Total:"
        '
        'labelMiddleAv
        '
        Me.labelMiddleAv.AutoSize = True
        Me.labelMiddleAv.Location = New System.Drawing.Point(6, 68)
        Me.labelMiddleAv.Name = "labelMiddleAv"
        Me.labelMiddleAv.Size = New System.Drawing.Size(41, 13)
        Me.labelMiddleAv.TabIndex = 24
        Me.labelMiddleAv.Text = "Middle:"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.labelLeftTotal)
        Me.GroupBox1.Controls.Add(Me.labelRightTotal)
        Me.GroupBox1.Controls.Add(Me.labelTotalTotal)
        Me.GroupBox1.Controls.Add(Me.labelMiddleTotal)
        Me.GroupBox1.Location = New System.Drawing.Point(7, 153)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(225, 88)
        Me.GroupBox1.TabIndex = 29
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Cumultative"
        '
        'labelLeftTotal
        '
        Me.labelLeftTotal.AutoSize = True
        Me.labelLeftTotal.Location = New System.Drawing.Point(17, 22)
        Me.labelLeftTotal.Name = "labelLeftTotal"
        Me.labelLeftTotal.Size = New System.Drawing.Size(28, 13)
        Me.labelLeftTotal.TabIndex = 21
        Me.labelLeftTotal.Text = "Left:"
        '
        'labelRightTotal
        '
        Me.labelRightTotal.AutoSize = True
        Me.labelRightTotal.Location = New System.Drawing.Point(10, 36)
        Me.labelRightTotal.Name = "labelRightTotal"
        Me.labelRightTotal.Size = New System.Drawing.Size(35, 13)
        Me.labelRightTotal.TabIndex = 22
        Me.labelRightTotal.Text = "Right:"
        '
        'labelTotalTotal
        '
        Me.labelTotalTotal.AutoSize = True
        Me.labelTotalTotal.Location = New System.Drawing.Point(11, 67)
        Me.labelTotalTotal.Name = "labelTotalTotal"
        Me.labelTotalTotal.Size = New System.Drawing.Size(34, 13)
        Me.labelTotalTotal.TabIndex = 23
        Me.labelTotalTotal.Text = "Total:"
        '
        'labelMiddleTotal
        '
        Me.labelMiddleTotal.AutoSize = True
        Me.labelMiddleTotal.Location = New System.Drawing.Point(4, 52)
        Me.labelMiddleTotal.Name = "labelMiddleTotal"
        Me.labelMiddleTotal.Size = New System.Drawing.Size(41, 13)
        Me.labelMiddleTotal.TabIndex = 24
        Me.labelMiddleTotal.Text = "Middle:"
        '
        'clickCounterHistoryList
        '
        Me.clickCounterHistoryList.FormattingEnabled = True
        Me.clickCounterHistoryList.Location = New System.Drawing.Point(6, 40)
        Me.clickCounterHistoryList.Name = "clickCounterHistoryList"
        Me.clickCounterHistoryList.Size = New System.Drawing.Size(90, 108)
        Me.clickCounterHistoryList.TabIndex = 20
        '
        'labelMiddleClick
        '
        Me.labelMiddleClick.AutoSize = True
        Me.labelMiddleClick.Location = New System.Drawing.Point(102, 118)
        Me.labelMiddleClick.Name = "labelMiddleClick"
        Me.labelMiddleClick.Size = New System.Drawing.Size(41, 13)
        Me.labelMiddleClick.TabIndex = 19
        Me.labelMiddleClick.Text = "Middle:"
        '
        'checkCc
        '
        Me.checkCc.AutoSize = True
        Me.checkCc.Location = New System.Drawing.Point(6, 17)
        Me.checkCc.Name = "checkCc"
        Me.checkCc.Size = New System.Drawing.Size(56, 17)
        Me.checkCc.TabIndex = 18
        Me.checkCc.Text = "Active"
        Me.checkCc.UseVisualStyleBackColor = True
        '
        'resetButton
        '
        Me.resetButton.Location = New System.Drawing.Point(102, 40)
        Me.resetButton.Name = "resetButton"
        Me.resetButton.Size = New System.Drawing.Size(54, 28)
        Me.resetButton.TabIndex = 17
        Me.resetButton.Text = "Reset"
        Me.resetButton.UseVisualStyleBackColor = True
        '
        'labelTotalClick
        '
        Me.labelTotalClick.AutoSize = True
        Me.labelTotalClick.Location = New System.Drawing.Point(109, 133)
        Me.labelTotalClick.Name = "labelTotalClick"
        Me.labelTotalClick.Size = New System.Drawing.Size(34, 13)
        Me.labelTotalClick.TabIndex = 9
        Me.labelTotalClick.Text = "Total:"
        '
        'labelRightClick
        '
        Me.labelRightClick.AutoSize = True
        Me.labelRightClick.Location = New System.Drawing.Point(108, 102)
        Me.labelRightClick.Name = "labelRightClick"
        Me.labelRightClick.Size = New System.Drawing.Size(35, 13)
        Me.labelRightClick.TabIndex = 8
        Me.labelRightClick.Text = "Right:"
        '
        'labelLeftCLick
        '
        Me.labelLeftCLick.AutoSize = True
        Me.labelLeftCLick.Location = New System.Drawing.Point(115, 88)
        Me.labelLeftCLick.Name = "labelLeftCLick"
        Me.labelLeftCLick.Size = New System.Drawing.Size(28, 13)
        Me.labelLeftCLick.TabIndex = 7
        Me.labelLeftCLick.Text = "Left:"
        '
        'g5
        '
        Me.g5.Controls.Add(Me.autostartRemoveButton)
        Me.g5.Controls.Add(Me.autostartAddButton)
        Me.g5.Controls.Add(Me.GroupBox4)
        Me.g5.Controls.Add(Me.autostartList)
        Me.g5.Controls.Add(Me.checkAutostart)
        Me.g5.Location = New System.Drawing.Point(409, 12)
        Me.g5.Name = "g5"
        Me.g5.Size = New System.Drawing.Size(240, 353)
        Me.g5.TabIndex = 31
        Me.g5.TabStop = False
        Me.g5.Text = "Autostart"
        '
        'autostartRemoveButton
        '
        Me.autostartRemoveButton.Location = New System.Drawing.Point(151, 74)
        Me.autostartRemoveButton.Name = "autostartRemoveButton"
        Me.autostartRemoveButton.Size = New System.Drawing.Size(80, 28)
        Me.autostartRemoveButton.TabIndex = 32
        Me.autostartRemoveButton.Text = "Remove"
        Me.autostartRemoveButton.UseVisualStyleBackColor = True
        '
        'autostartAddButton
        '
        Me.autostartAddButton.Location = New System.Drawing.Point(151, 40)
        Me.autostartAddButton.Name = "autostartAddButton"
        Me.autostartAddButton.Size = New System.Drawing.Size(80, 28)
        Me.autostartAddButton.TabIndex = 31
        Me.autostartAddButton.Text = "Add"
        Me.autostartAddButton.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.autostartRunButton)
        Me.GroupBox4.Controls.Add(Me.checkAutostartActive)
        Me.GroupBox4.Controls.Add(Me.autostartSaveButton)
        Me.GroupBox4.Controls.Add(Me.autostartFileDialogButton)
        Me.GroupBox4.Controls.Add(Me.textboxAutostartArgs)
        Me.GroupBox4.Controls.Add(Me.labelAutostartArgs)
        Me.GroupBox4.Controls.Add(Me.textboxAutostartPath)
        Me.GroupBox4.Controls.Add(Me.textboxAutostartName)
        Me.GroupBox4.Controls.Add(Me.labelAutostartPath)
        Me.GroupBox4.Controls.Add(Me.labelAutostartName)
        Me.GroupBox4.Location = New System.Drawing.Point(6, 111)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(225, 229)
        Me.GroupBox4.TabIndex = 30
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Details"
        '
        'autostartRunButton
        '
        Me.autostartRunButton.Location = New System.Drawing.Point(178, 118)
        Me.autostartRunButton.Name = "autostartRunButton"
        Me.autostartRunButton.Size = New System.Drawing.Size(42, 28)
        Me.autostartRunButton.TabIndex = 34
        Me.autostartRunButton.Text = "Run"
        Me.autostartRunButton.UseVisualStyleBackColor = True
        '
        'checkAutostartActive
        '
        Me.checkAutostartActive.AutoSize = True
        Me.checkAutostartActive.Location = New System.Drawing.Point(4, 198)
        Me.checkAutostartActive.Name = "checkAutostartActive"
        Me.checkAutostartActive.Size = New System.Drawing.Size(56, 17)
        Me.checkAutostartActive.TabIndex = 33
        Me.checkAutostartActive.Text = "Active"
        Me.checkAutostartActive.UseVisualStyleBackColor = True
        '
        'autostartSaveButton
        '
        Me.autostartSaveButton.Location = New System.Drawing.Point(139, 191)
        Me.autostartSaveButton.Name = "autostartSaveButton"
        Me.autostartSaveButton.Size = New System.Drawing.Size(80, 28)
        Me.autostartSaveButton.TabIndex = 33
        Me.autostartSaveButton.Text = "Save"
        Me.autostartSaveButton.UseVisualStyleBackColor = True
        '
        'autostartFileDialogButton
        '
        Me.autostartFileDialogButton.Location = New System.Drawing.Point(178, 80)
        Me.autostartFileDialogButton.Name = "autostartFileDialogButton"
        Me.autostartFileDialogButton.Size = New System.Drawing.Size(42, 20)
        Me.autostartFileDialogButton.TabIndex = 31
        Me.autostartFileDialogButton.Text = "..."
        Me.autostartFileDialogButton.UseVisualStyleBackColor = True
        '
        'textboxAutostartArgs
        '
        Me.textboxAutostartArgs.Location = New System.Drawing.Point(4, 163)
        Me.textboxAutostartArgs.Name = "textboxAutostartArgs"
        Me.textboxAutostartArgs.Size = New System.Drawing.Size(216, 20)
        Me.textboxAutostartArgs.TabIndex = 5
        '
        'labelAutostartArgs
        '
        Me.labelAutostartArgs.AutoSize = True
        Me.labelAutostartArgs.Location = New System.Drawing.Point(3, 148)
        Me.labelAutostartArgs.Name = "labelAutostartArgs"
        Me.labelAutostartArgs.Size = New System.Drawing.Size(60, 13)
        Me.labelAutostartArgs.TabIndex = 4
        Me.labelAutostartArgs.Text = "Arguments:"
        '
        'textboxAutostartPath
        '
        Me.textboxAutostartPath.Location = New System.Drawing.Point(4, 80)
        Me.textboxAutostartPath.Multiline = True
        Me.textboxAutostartPath.Name = "textboxAutostartPath"
        Me.textboxAutostartPath.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.textboxAutostartPath.Size = New System.Drawing.Size(169, 65)
        Me.textboxAutostartPath.TabIndex = 3
        '
        'textboxAutostartName
        '
        Me.textboxAutostartName.Location = New System.Drawing.Point(4, 34)
        Me.textboxAutostartName.Name = "textboxAutostartName"
        Me.textboxAutostartName.Size = New System.Drawing.Size(216, 20)
        Me.textboxAutostartName.TabIndex = 2
        '
        'labelAutostartPath
        '
        Me.labelAutostartPath.AutoSize = True
        Me.labelAutostartPath.Location = New System.Drawing.Point(3, 64)
        Me.labelAutostartPath.Name = "labelAutostartPath"
        Me.labelAutostartPath.Size = New System.Drawing.Size(63, 13)
        Me.labelAutostartPath.TabIndex = 1
        Me.labelAutostartPath.Text = "Executable:"
        '
        'labelAutostartName
        '
        Me.labelAutostartName.AutoSize = True
        Me.labelAutostartName.Location = New System.Drawing.Point(6, 18)
        Me.labelAutostartName.Name = "labelAutostartName"
        Me.labelAutostartName.Size = New System.Drawing.Size(38, 13)
        Me.labelAutostartName.TabIndex = 0
        Me.labelAutostartName.Text = "Name:"
        '
        'autostartList
        '
        Me.autostartList.FormattingEnabled = True
        Me.autostartList.Location = New System.Drawing.Point(6, 40)
        Me.autostartList.Name = "autostartList"
        Me.autostartList.Size = New System.Drawing.Size(139, 69)
        Me.autostartList.TabIndex = 20
        '
        'checkAutostart
        '
        Me.checkAutostart.AutoSize = True
        Me.checkAutostart.Location = New System.Drawing.Point(6, 17)
        Me.checkAutostart.Name = "checkAutostart"
        Me.checkAutostart.Size = New System.Drawing.Size(56, 17)
        Me.checkAutostart.TabIndex = 18
        Me.checkAutostart.Text = "Active"
        Me.checkAutostart.UseVisualStyleBackColor = True
        '
        'g2
        '
        Me.g2.Controls.Add(Me.GroupBox3)
        Me.g2.Controls.Add(Me.delayLabel)
        Me.g2.Controls.Add(Me.numDelay)
        Me.g2.Controls.Add(Me.labelIncr)
        Me.g2.Controls.Add(Me.numIncr)
        Me.g2.Controls.Add(Me.checkCursorMover)
        Me.g2.Location = New System.Drawing.Point(689, 12)
        Me.g2.Name = "g2"
        Me.g2.Size = New System.Drawing.Size(240, 353)
        Me.g2.TabIndex = 33
        Me.g2.TabStop = False
        Me.g2.Text = "Cursor Mover"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.cursorMoverLeftButton)
        Me.GroupBox3.Controls.Add(Me.cursorMoverDownButton)
        Me.GroupBox3.Controls.Add(Me.cursorMoverRightButton)
        Me.GroupBox3.Controls.Add(Me.cursorMoverUpButton)
        Me.GroupBox3.Location = New System.Drawing.Point(9, 121)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(225, 116)
        Me.GroupBox3.TabIndex = 31
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Hotkeys"
        '
        'cursorMoverLeftButton
        '
        Me.cursorMoverLeftButton.Location = New System.Drawing.Point(63, 47)
        Me.cursorMoverLeftButton.Name = "cursorMoverLeftButton"
        Me.cursorMoverLeftButton.Size = New System.Drawing.Size(30, 30)
        Me.cursorMoverLeftButton.TabIndex = 34
        Me.cursorMoverLeftButton.Text = "←"
        Me.cursorMoverLeftButton.UseVisualStyleBackColor = True
        '
        'cursorMoverDownButton
        '
        Me.cursorMoverDownButton.Location = New System.Drawing.Point(95, 80)
        Me.cursorMoverDownButton.Name = "cursorMoverDownButton"
        Me.cursorMoverDownButton.Size = New System.Drawing.Size(30, 30)
        Me.cursorMoverDownButton.TabIndex = 33
        Me.cursorMoverDownButton.Text = "↓"
        Me.cursorMoverDownButton.UseVisualStyleBackColor = True
        '
        'cursorMoverRightButton
        '
        Me.cursorMoverRightButton.Location = New System.Drawing.Point(127, 47)
        Me.cursorMoverRightButton.Name = "cursorMoverRightButton"
        Me.cursorMoverRightButton.Size = New System.Drawing.Size(30, 30)
        Me.cursorMoverRightButton.TabIndex = 32
        Me.cursorMoverRightButton.Text = "→"
        Me.cursorMoverRightButton.UseVisualStyleBackColor = True
        '
        'cursorMoverUpButton
        '
        Me.cursorMoverUpButton.Location = New System.Drawing.Point(95, 13)
        Me.cursorMoverUpButton.Name = "cursorMoverUpButton"
        Me.cursorMoverUpButton.Size = New System.Drawing.Size(30, 30)
        Me.cursorMoverUpButton.TabIndex = 31
        Me.cursorMoverUpButton.Text = "↑"
        Me.cursorMoverUpButton.UseVisualStyleBackColor = True
        '
        'delayLabel
        '
        Me.delayLabel.AutoSize = True
        Me.delayLabel.Location = New System.Drawing.Point(6, 82)
        Me.delayLabel.Name = "delayLabel"
        Me.delayLabel.Size = New System.Drawing.Size(59, 13)
        Me.delayLabel.TabIndex = 29
        Me.delayLabel.Text = "Delay (ms):"
        '
        'numDelay
        '
        Me.numDelay.Location = New System.Drawing.Point(68, 79)
        Me.numDelay.Maximum = New Decimal(New Integer() {500, 0, 0, 0})
        Me.numDelay.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numDelay.Name = "numDelay"
        Me.numDelay.Size = New System.Drawing.Size(60, 20)
        Me.numDelay.TabIndex = 30
        Me.numDelay.Value = New Decimal(New Integer() {200, 0, 0, 0})
        '
        'labelIncr
        '
        Me.labelIncr.AutoSize = True
        Me.labelIncr.Location = New System.Drawing.Point(6, 55)
        Me.labelIncr.Name = "labelIncr"
        Me.labelIncr.Size = New System.Drawing.Size(57, 13)
        Me.labelIncr.TabIndex = 27
        Me.labelIncr.Text = "Increment:"
        '
        'numIncr
        '
        Me.numIncr.Location = New System.Drawing.Point(68, 52)
        Me.numIncr.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numIncr.Name = "numIncr"
        Me.numIncr.Size = New System.Drawing.Size(60, 20)
        Me.numIncr.TabIndex = 28
        Me.numIncr.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'checkCursorMover
        '
        Me.checkCursorMover.AutoSize = True
        Me.checkCursorMover.Location = New System.Drawing.Point(6, 17)
        Me.checkCursorMover.Name = "checkCursorMover"
        Me.checkCursorMover.Size = New System.Drawing.Size(56, 17)
        Me.checkCursorMover.TabIndex = 18
        Me.checkCursorMover.Text = "Active"
        Me.checkCursorMover.UseVisualStyleBackColor = True
        '
        'g3
        '
        Me.g3.Controls.Add(Me.labelAutoClickerRepititions)
        Me.g3.Controls.Add(Me.labelFreq)
        Me.g3.Controls.Add(Me.numFreq)
        Me.g3.Controls.Add(Me.numRep)
        Me.g3.Controls.Add(Me.GroupBox6)
        Me.g3.Controls.Add(Me.checkAutoClicker)
        Me.g3.Location = New System.Drawing.Point(960, 22)
        Me.g3.Name = "g3"
        Me.g3.Size = New System.Drawing.Size(240, 353)
        Me.g3.TabIndex = 34
        Me.g3.TabStop = False
        Me.g3.Text = "Auto Clicker"
        '
        'labelAutoClickerRepititions
        '
        Me.labelAutoClickerRepititions.AutoSize = True
        Me.labelAutoClickerRepititions.Location = New System.Drawing.Point(26, 75)
        Me.labelAutoClickerRepititions.Name = "labelAutoClickerRepititions"
        Me.labelAutoClickerRepititions.Size = New System.Drawing.Size(59, 13)
        Me.labelAutoClickerRepititions.TabIndex = 35
        Me.labelAutoClickerRepititions.Text = "Repititions:"
        '
        'labelFreq
        '
        Me.labelFreq.AutoSize = True
        Me.labelFreq.Location = New System.Drawing.Point(3, 49)
        Me.labelFreq.Name = "labelFreq"
        Me.labelFreq.Size = New System.Drawing.Size(82, 13)
        Me.labelFreq.TabIndex = 32
        Me.labelFreq.Text = "Frequency (ms):"
        '
        'numFreq
        '
        Me.numFreq.Location = New System.Drawing.Point(88, 47)
        Me.numFreq.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.numFreq.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numFreq.Name = "numFreq"
        Me.numFreq.Size = New System.Drawing.Size(60, 20)
        Me.numFreq.TabIndex = 34
        Me.numFreq.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'numRep
        '
        Me.numRep.Location = New System.Drawing.Point(88, 73)
        Me.numRep.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numRep.Name = "numRep"
        Me.numRep.Size = New System.Drawing.Size(60, 20)
        Me.numRep.TabIndex = 33
        Me.numRep.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.autoClickerDisableButton)
        Me.GroupBox6.Controls.Add(Me.autoClickerEnableButton)
        Me.GroupBox6.Location = New System.Drawing.Point(9, 111)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(225, 74)
        Me.GroupBox6.TabIndex = 31
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Hotkeys"
        '
        'autoClickerDisableButton
        '
        Me.autoClickerDisableButton.Location = New System.Drawing.Point(114, 25)
        Me.autoClickerDisableButton.Name = "autoClickerDisableButton"
        Me.autoClickerDisableButton.Size = New System.Drawing.Size(80, 28)
        Me.autoClickerDisableButton.TabIndex = 34
        Me.autoClickerDisableButton.Text = "Stop Hotkey"
        Me.autoClickerDisableButton.UseVisualStyleBackColor = True
        '
        'autoClickerEnableButton
        '
        Me.autoClickerEnableButton.Location = New System.Drawing.Point(28, 25)
        Me.autoClickerEnableButton.Name = "autoClickerEnableButton"
        Me.autoClickerEnableButton.Size = New System.Drawing.Size(80, 28)
        Me.autoClickerEnableButton.TabIndex = 33
        Me.autoClickerEnableButton.Text = "Start Hotkey"
        Me.autoClickerEnableButton.UseVisualStyleBackColor = True
        '
        'checkAutoClicker
        '
        Me.checkAutoClicker.AutoSize = True
        Me.checkAutoClicker.Location = New System.Drawing.Point(6, 17)
        Me.checkAutoClicker.Name = "checkAutoClicker"
        Me.checkAutoClicker.Size = New System.Drawing.Size(56, 17)
        Me.checkAutoClicker.TabIndex = 18
        Me.checkAutoClicker.Text = "Active"
        Me.checkAutoClicker.UseVisualStyleBackColor = True
        '
        'g4
        '
        Me.g4.Controls.Add(Me.comboMacros)
        Me.g4.Controls.Add(Me.GroupBox5)
        Me.g4.Controls.Add(Me.checkMacros)
        Me.g4.Location = New System.Drawing.Point(1215, 22)
        Me.g4.Name = "g4"
        Me.g4.Size = New System.Drawing.Size(240, 353)
        Me.g4.TabIndex = 36
        Me.g4.TabStop = False
        Me.g4.Text = "Macros"
        '
        'comboMacros
        '
        Me.comboMacros.FormattingEnabled = True
        Me.comboMacros.Location = New System.Drawing.Point(61, 36)
        Me.comboMacros.Name = "comboMacros"
        Me.comboMacros.Size = New System.Drawing.Size(121, 21)
        Me.comboMacros.TabIndex = 35
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.textMacroName)
        Me.GroupBox5.Controls.Add(Me.Label1)
        Me.GroupBox5.Controls.Add(Me.macroActiveCheckbox)
        Me.GroupBox5.Controls.Add(Me.checkMacroOverride)
        Me.GroupBox5.Controls.Add(Me.macroSaveButton)
        Me.GroupBox5.Controls.Add(Me.textMacroArgs)
        Me.GroupBox5.Controls.Add(Me.macroHotkeyButton)
        Me.GroupBox5.Controls.Add(Me.labelCommandCenterArgs)
        Me.GroupBox5.Controls.Add(Me.textMacroPath)
        Me.GroupBox5.Controls.Add(Me.macroRunButton)
        Me.GroupBox5.Controls.Add(Me.labelCommandCenterPath)
        Me.GroupBox5.Controls.Add(Me.macroFileButton)
        Me.GroupBox5.Location = New System.Drawing.Point(7, 60)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(225, 287)
        Me.GroupBox5.TabIndex = 34
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Details"
        '
        'textMacroName
        '
        Me.textMacroName.Location = New System.Drawing.Point(6, 38)
        Me.textMacroName.Name = "textMacroName"
        Me.textMacroName.Size = New System.Drawing.Size(216, 20)
        Me.textMacroName.TabIndex = 36
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 13)
        Me.Label1.TabIndex = 35
        Me.Label1.Text = "Name:"
        '
        'macroActiveCheckbox
        '
        Me.macroActiveCheckbox.AutoSize = True
        Me.macroActiveCheckbox.Location = New System.Drawing.Point(7, 258)
        Me.macroActiveCheckbox.Name = "macroActiveCheckbox"
        Me.macroActiveCheckbox.Size = New System.Drawing.Size(56, 17)
        Me.macroActiveCheckbox.TabIndex = 35
        Me.macroActiveCheckbox.Text = "Active"
        Me.macroActiveCheckbox.UseVisualStyleBackColor = True
        '
        'checkMacroOverride
        '
        Me.checkMacroOverride.AutoSize = True
        Me.checkMacroOverride.Location = New System.Drawing.Point(93, 217)
        Me.checkMacroOverride.Name = "checkMacroOverride"
        Me.checkMacroOverride.Size = New System.Drawing.Size(124, 17)
        Me.checkMacroOverride.TabIndex = 35
        Me.checkMacroOverride.Text = "Override hotkey lock"
        Me.checkMacroOverride.UseVisualStyleBackColor = True
        '
        'macroSaveButton
        '
        Me.macroSaveButton.Location = New System.Drawing.Point(140, 251)
        Me.macroSaveButton.Name = "macroSaveButton"
        Me.macroSaveButton.Size = New System.Drawing.Size(80, 28)
        Me.macroSaveButton.TabIndex = 36
        Me.macroSaveButton.Text = "Save"
        Me.macroSaveButton.UseVisualStyleBackColor = True
        '
        'textMacroArgs
        '
        Me.textMacroArgs.Location = New System.Drawing.Point(6, 176)
        Me.textMacroArgs.Name = "textMacroArgs"
        Me.textMacroArgs.Size = New System.Drawing.Size(213, 20)
        Me.textMacroArgs.TabIndex = 36
        '
        'macroHotkeyButton
        '
        Me.macroHotkeyButton.Location = New System.Drawing.Point(8, 210)
        Me.macroHotkeyButton.Name = "macroHotkeyButton"
        Me.macroHotkeyButton.Size = New System.Drawing.Size(80, 28)
        Me.macroHotkeyButton.TabIndex = 33
        Me.macroHotkeyButton.Text = "Start Hotkey"
        Me.macroHotkeyButton.UseVisualStyleBackColor = True
        '
        'labelCommandCenterArgs
        '
        Me.labelCommandCenterArgs.AutoSize = True
        Me.labelCommandCenterArgs.Location = New System.Drawing.Point(5, 161)
        Me.labelCommandCenterArgs.Name = "labelCommandCenterArgs"
        Me.labelCommandCenterArgs.Size = New System.Drawing.Size(60, 13)
        Me.labelCommandCenterArgs.TabIndex = 35
        Me.labelCommandCenterArgs.Text = "Arguments:"
        '
        'textMacroPath
        '
        Me.textMacroPath.Location = New System.Drawing.Point(6, 82)
        Me.textMacroPath.Multiline = True
        Me.textMacroPath.Name = "textMacroPath"
        Me.textMacroPath.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.textMacroPath.Size = New System.Drawing.Size(169, 72)
        Me.textMacroPath.TabIndex = 36
        '
        'macroRunButton
        '
        Me.macroRunButton.Location = New System.Drawing.Point(177, 126)
        Me.macroRunButton.Name = "macroRunButton"
        Me.macroRunButton.Size = New System.Drawing.Size(42, 28)
        Me.macroRunButton.TabIndex = 38
        Me.macroRunButton.Text = "Run"
        Me.macroRunButton.UseVisualStyleBackColor = True
        '
        'labelCommandCenterPath
        '
        Me.labelCommandCenterPath.AutoSize = True
        Me.labelCommandCenterPath.Location = New System.Drawing.Point(5, 66)
        Me.labelCommandCenterPath.Name = "labelCommandCenterPath"
        Me.labelCommandCenterPath.Size = New System.Drawing.Size(63, 13)
        Me.labelCommandCenterPath.TabIndex = 35
        Me.labelCommandCenterPath.Text = "Executable:"
        '
        'macroFileButton
        '
        Me.macroFileButton.Location = New System.Drawing.Point(178, 82)
        Me.macroFileButton.Name = "macroFileButton"
        Me.macroFileButton.Size = New System.Drawing.Size(42, 20)
        Me.macroFileButton.TabIndex = 37
        Me.macroFileButton.Text = "..."
        Me.macroFileButton.UseVisualStyleBackColor = True
        '
        'checkMacros
        '
        Me.checkMacros.AutoSize = True
        Me.checkMacros.Location = New System.Drawing.Point(6, 17)
        Me.checkMacros.Name = "checkMacros"
        Me.checkMacros.Size = New System.Drawing.Size(56, 17)
        Me.checkMacros.TabIndex = 18
        Me.checkMacros.Text = "Active"
        Me.checkMacros.UseVisualStyleBackColor = True
        '
        'g6
        '
        Me.g6.Controls.Add(Me.GroupBox7)
        Me.g6.Controls.Add(Me.GroupBox9)
        Me.g6.Controls.Add(Me.checkKeylogger)
        Me.g6.Location = New System.Drawing.Point(133, 379)
        Me.g6.Name = "g6"
        Me.g6.Size = New System.Drawing.Size(240, 353)
        Me.g6.TabIndex = 37
        Me.g6.TabStop = False
        Me.g6.Text = "Keylogger"
        '
        'checkKeyloggerAllowHotkeys
        '
        Me.checkKeyloggerAllowHotkeys.AutoSize = True
        Me.checkKeyloggerAllowHotkeys.Location = New System.Drawing.Point(4, 19)
        Me.checkKeyloggerAllowHotkeys.Name = "checkKeyloggerAllowHotkeys"
        Me.checkKeyloggerAllowHotkeys.Size = New System.Drawing.Size(172, 17)
        Me.checkKeyloggerAllowHotkeys.TabIndex = 35
        Me.checkKeyloggerAllowHotkeys.Text = "Allow concurrent hotkey usage"
        Me.checkKeyloggerAllowHotkeys.UseVisualStyleBackColor = True
        '
        'GroupBox9
        '
        Me.GroupBox9.Controls.Add(Me.keyloggerDeleteButton)
        Me.GroupBox9.Controls.Add(Me.Label3)
        Me.GroupBox9.Controls.Add(Me.textKeyloggerBuffer)
        Me.GroupBox9.Controls.Add(Me.textKeyloggerPath)
        Me.GroupBox9.Controls.Add(Me.keyloggerOpenButton)
        Me.GroupBox9.Controls.Add(Me.Label2)
        Me.GroupBox9.Controls.Add(Me.keyloggerFileButton)
        Me.GroupBox9.Location = New System.Drawing.Point(7, 40)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(225, 214)
        Me.GroupBox9.TabIndex = 34
        Me.GroupBox9.TabStop = False
        Me.GroupBox9.Text = "Output"
        '
        'textKeyloggerPath
        '
        Me.textKeyloggerPath.Location = New System.Drawing.Point(6, 34)
        Me.textKeyloggerPath.Multiline = True
        Me.textKeyloggerPath.Name = "textKeyloggerPath"
        Me.textKeyloggerPath.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.textKeyloggerPath.Size = New System.Drawing.Size(166, 85)
        Me.textKeyloggerPath.TabIndex = 36
        '
        'keyloggerOpenButton
        '
        Me.keyloggerOpenButton.Location = New System.Drawing.Point(175, 59)
        Me.keyloggerOpenButton.Name = "keyloggerOpenButton"
        Me.keyloggerOpenButton.Size = New System.Drawing.Size(46, 28)
        Me.keyloggerOpenButton.TabIndex = 38
        Me.keyloggerOpenButton.Text = "Open"
        Me.keyloggerOpenButton.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(5, 18)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(51, 13)
        Me.Label2.TabIndex = 35
        Me.Label2.Text = "File Path:"
        '
        'keyloggerFileButton
        '
        Me.keyloggerFileButton.Location = New System.Drawing.Point(175, 34)
        Me.keyloggerFileButton.Name = "keyloggerFileButton"
        Me.keyloggerFileButton.Size = New System.Drawing.Size(46, 20)
        Me.keyloggerFileButton.TabIndex = 37
        Me.keyloggerFileButton.Text = "..."
        Me.keyloggerFileButton.UseVisualStyleBackColor = True
        '
        'checkKeylogger
        '
        Me.checkKeylogger.AutoSize = True
        Me.checkKeylogger.Location = New System.Drawing.Point(6, 17)
        Me.checkKeylogger.Name = "checkKeylogger"
        Me.checkKeylogger.Size = New System.Drawing.Size(56, 17)
        Me.checkKeylogger.TabIndex = 18
        Me.checkKeylogger.Text = "Active"
        Me.checkKeylogger.UseVisualStyleBackColor = True
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.checkKeyloggerRecordWindow)
        Me.GroupBox7.Controls.Add(Me.checkKeyloggerAllowHotkeys)
        Me.GroupBox7.Location = New System.Drawing.Point(7, 262)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(225, 65)
        Me.GroupBox7.TabIndex = 39
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "Options"
        '
        'checkKeyloggerRecordWindow
        '
        Me.checkKeyloggerRecordWindow.AutoSize = True
        Me.checkKeyloggerRecordWindow.Location = New System.Drawing.Point(4, 42)
        Me.checkKeyloggerRecordWindow.Name = "checkKeyloggerRecordWindow"
        Me.checkKeyloggerRecordWindow.Size = New System.Drawing.Size(132, 17)
        Me.checkKeyloggerRecordWindow.TabIndex = 36
        Me.checkKeyloggerRecordWindow.Text = "Record active window"
        Me.checkKeyloggerRecordWindow.UseVisualStyleBackColor = True
        '
        'textKeyloggerBuffer
        '
        Me.textKeyloggerBuffer.Location = New System.Drawing.Point(6, 145)
        Me.textKeyloggerBuffer.Multiline = True
        Me.textKeyloggerBuffer.Name = "textKeyloggerBuffer"
        Me.textKeyloggerBuffer.ReadOnly = True
        Me.textKeyloggerBuffer.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.textKeyloggerBuffer.Size = New System.Drawing.Size(212, 55)
        Me.textKeyloggerBuffer.TabIndex = 40
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 129)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(75, 13)
        Me.Label3.TabIndex = 41
        Me.Label3.Text = "Current Buffer:"
        '
        'keyloggerDeleteButton
        '
        Me.keyloggerDeleteButton.Location = New System.Drawing.Point(175, 91)
        Me.keyloggerDeleteButton.Name = "keyloggerDeleteButton"
        Me.keyloggerDeleteButton.Size = New System.Drawing.Size(46, 28)
        Me.keyloggerDeleteButton.TabIndex = 42
        Me.keyloggerDeleteButton.Text = "Delete"
        Me.keyloggerDeleteButton.UseVisualStyleBackColor = True
        '
        'GadgetsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1529, 759)
        Me.Controls.Add(Me.g6)
        Me.Controls.Add(Me.g4)
        Me.Controls.Add(Me.g3)
        Me.Controls.Add(Me.g2)
        Me.Controls.Add(Me.g5)
        Me.Controls.Add(Me.g1)
        Me.Controls.Add(Me.listMenu)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "GadgetsForm"
        Me.Text = "Gadgets"
        Me.g1.ResumeLayout(False)
        Me.g1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.g5.ResumeLayout(False)
        Me.g5.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.g2.ResumeLayout(False)
        Me.g2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        CType(Me.numDelay, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numIncr, System.ComponentModel.ISupportInitialize).EndInit()
        Me.g3.ResumeLayout(False)
        Me.g3.PerformLayout()
        CType(Me.numFreq, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numRep, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox6.ResumeLayout(False)
        Me.g4.ResumeLayout(False)
        Me.g4.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.g6.ResumeLayout(False)
        Me.g6.PerformLayout()
        Me.GroupBox9.ResumeLayout(False)
        Me.GroupBox9.PerformLayout()
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents listMenu As ListBox
    Friend WithEvents g1 As GroupBox
    Friend WithEvents labelMiddleClick As Label
    Friend WithEvents checkCc As CheckBox
    Friend WithEvents resetButton As Button
    Friend WithEvents labelTotalClick As Label
    Friend WithEvents labelRightClick As Label
    Friend WithEvents labelLeftCLick As Label
    Friend WithEvents clickCounterHistoryList As ListBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents labelLeftAv As Label
    Friend WithEvents labelRightAv As Label
    Friend WithEvents labelTotalAv As Label
    Friend WithEvents labelMiddleAv As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents labelLeftTotal As Label
    Friend WithEvents labelRightTotal As Label
    Friend WithEvents labelTotalTotal As Label
    Friend WithEvents labelMiddleTotal As Label
    Friend WithEvents averageUnitCombo As ComboBox
    Friend WithEvents g5 As GroupBox
    Friend WithEvents autostartRemoveButton As Button
    Friend WithEvents autostartAddButton As Button
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents autostartFileDialogButton As Button
    Friend WithEvents textboxAutostartArgs As TextBox
    Friend WithEvents labelAutostartArgs As Label
    Friend WithEvents textboxAutostartPath As TextBox
    Friend WithEvents textboxAutostartName As TextBox
    Friend WithEvents labelAutostartPath As Label
    Friend WithEvents labelAutostartName As Label
    Friend WithEvents autostartList As ListBox
    Friend WithEvents checkAutostart As CheckBox
    Friend WithEvents autostartSaveButton As Button
    Friend WithEvents checkAutostartActive As CheckBox
    Friend WithEvents autostartRunButton As Button
    Friend WithEvents g2 As GroupBox
    Friend WithEvents checkCursorMover As CheckBox
    Friend WithEvents labelIncr As Label
    Friend WithEvents numIncr As NumericUpDown
    Friend WithEvents delayLabel As Label
    Friend WithEvents numDelay As NumericUpDown
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents cursorMoverLeftButton As Button
    Friend WithEvents cursorMoverDownButton As Button
    Friend WithEvents cursorMoverRightButton As Button
    Friend WithEvents cursorMoverUpButton As Button
    Friend WithEvents g3 As GroupBox
    Friend WithEvents GroupBox6 As GroupBox
    Friend WithEvents checkAutoClicker As CheckBox
    Friend WithEvents labelAutoClickerRepititions As Label
    Friend WithEvents labelFreq As Label
    Friend WithEvents numFreq As NumericUpDown
    Friend WithEvents numRep As NumericUpDown
    Friend WithEvents autoClickerDisableButton As Button
    Friend WithEvents autoClickerEnableButton As Button
    Friend WithEvents g4 As GroupBox
    Friend WithEvents macroHotkeyButton As Button
    Friend WithEvents checkMacros As CheckBox
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents textMacroPath As TextBox
    Friend WithEvents macroRunButton As Button
    Friend WithEvents labelCommandCenterPath As Label
    Friend WithEvents macroFileButton As Button
    Friend WithEvents checkMacroOverride As CheckBox
    Friend WithEvents textMacroArgs As TextBox
    Friend WithEvents labelCommandCenterArgs As Label
    Friend WithEvents g6 As GroupBox
    Friend WithEvents GroupBox9 As GroupBox
    Friend WithEvents textKeyloggerPath As TextBox
    Friend WithEvents keyloggerOpenButton As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents keyloggerFileButton As Button
    Friend WithEvents checkKeylogger As CheckBox
    Friend WithEvents checkKeyloggerAllowHotkeys As CheckBox
    Friend WithEvents comboMacros As ComboBox
    Friend WithEvents textMacroName As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents macroActiveCheckbox As CheckBox
    Friend WithEvents macroSaveButton As Button
    Friend WithEvents GroupBox7 As GroupBox
    Friend WithEvents checkKeyloggerRecordWindow As CheckBox
    Friend WithEvents textKeyloggerBuffer As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents keyloggerDeleteButton As Button
End Class
