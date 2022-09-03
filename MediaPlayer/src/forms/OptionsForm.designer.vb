<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class OptionsForm
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OptionsForm))
        Me.commandBox = New System.Windows.Forms.ComboBox()
        Me.r1 = New System.Windows.Forms.RadioButton()
        Me.r2 = New System.Windows.Forms.RadioButton()
        Me.r3 = New System.Windows.Forms.RadioButton()
        Me.keyButton = New System.Windows.Forms.Button()
        Me.groupCommand = New System.Windows.Forms.GroupBox()
        Me.groupModifierKey = New System.Windows.Forms.GroupBox()
        Me.r0 = New System.Windows.Forms.RadioButton()
        Me.groupMainKey = New System.Windows.Forms.GroupBox()
        Me.groupCurrentSet = New System.Windows.Forms.GroupBox()
        Me.defaultButton = New System.Windows.Forms.Button()
        Me.remButton = New System.Windows.Forms.Button()
        Me.listSet = New System.Windows.Forms.ListBox()
        Me.g3 = New System.Windows.Forms.GroupBox()
        Me.groupTimerDelay = New System.Windows.Forms.GroupBox()
        Me.numDelay = New System.Windows.Forms.NumericUpDown()
        Me.g5 = New System.Windows.Forms.GroupBox()
        Me.groupAssociations = New System.Windows.Forms.GroupBox()
        Me.clearGenreDepButton = New System.Windows.Forms.Button()
        Me.remGenreDepButton = New System.Windows.Forms.Button()
        Me.listAssociations = New System.Windows.Forms.ListBox()
        Me.addFolderButton = New System.Windows.Forms.Button()
        Me.addTrackButton = New System.Windows.Forms.Button()
        Me.groupCurrentGenres = New System.Windows.Forms.GroupBox()
        Me.listGenres = New System.Windows.Forms.ListBox()
        Me.NewGenreButton = New System.Windows.Forms.Button()
        Me.remButton2 = New System.Windows.Forms.Button()
        Me.g4 = New System.Windows.Forms.GroupBox()
        Me.labelSorting = New System.Windows.Forms.Label()
        Me.addButton = New System.Windows.Forms.Button()
        Me.sortingBox = New System.Windows.Forms.ComboBox()
        Me.groupStations = New System.Windows.Forms.GroupBox()
        Me.labelUrl = New System.Windows.Forms.Label()
        Me.moveDownButton = New System.Windows.Forms.Button()
        Me.listStations = New System.Windows.Forms.ListBox()
        Me.moveUpButton = New System.Windows.Forms.Button()
        Me.remButton4 = New System.Windows.Forms.Button()
        Me.changeUrlButton = New System.Windows.Forms.Button()
        Me.changeNameButton = New System.Windows.Forms.Button()
        Me.g8 = New System.Windows.Forms.GroupBox()
        Me.groupTCPListener = New System.Windows.Forms.GroupBox()
        Me.checkBlockExtIps = New System.Windows.Forms.CheckBox()
        Me.labelPort = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.resetButton2 = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tPort = New System.Windows.Forms.TextBox()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.startButton = New System.Windows.Forms.Button()
        Me.labelStatus = New System.Windows.Forms.Label()
        Me.checkEnableStartup = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.groupTCPConnection = New System.Windows.Forms.GroupBox()
        Me.remoteSendButton = New System.Windows.Forms.Button()
        Me.stopAllConnectionsButton = New System.Windows.Forms.Button()
        Me.listPairedIps = New System.Windows.Forms.ListBox()
        Me.checkBlockMessages = New System.Windows.Forms.CheckBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.labelExtIp = New System.Windows.Forms.Label()
        Me.labelOwnIp = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.stopConnectionButton = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.g9 = New System.Windows.Forms.GroupBox()
        Me.groupVersion = New System.Windows.Forms.GroupBox()
        Me.publishPathButton = New System.Windows.Forms.Button()
        Me.listPublish = New System.Windows.Forms.ListBox()
        Me.publishButton = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.labelPublishedVersion = New System.Windows.Forms.Label()
        Me.publishAddButton = New System.Windows.Forms.Button()
        Me.publishRemButton = New System.Windows.Forms.Button()
        Me.searchHomeIpButton = New System.Windows.Forms.Button()
        Me.tftpUser = New System.Windows.Forms.TextBox()
        Me.tftpPw = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.tftpIp = New System.Windows.Forms.TextBox()
        Me.labelftpCurrProg = New System.Windows.Forms.Label()
        Me.labelftpTotalProg = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.pBar2 = New System.Windows.Forms.ProgressBar()
        Me.pBar = New System.Windows.Forms.ProgressBar()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.DownloadLatestButton = New System.Windows.Forms.Button()
        Me.checkAutoUpdate = New System.Windows.Forms.CheckBox()
        Me.checkUpdatesButton = New System.Windows.Forms.Button()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.labelCurrVersion = New System.Windows.Forms.Label()
        Me.g2 = New System.Windows.Forms.GroupBox()
        Me.logPathReloadPic = New System.Windows.Forms.PictureBox()
        Me.logPathKeyPic = New System.Windows.Forms.PictureBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.bFtpDir = New System.Windows.Forms.Button()
        Me.bLyricsDir = New System.Windows.Forms.Button()
        Me.tFtpDir = New System.Windows.Forms.TextBox()
        Me.bDatesFile = New System.Windows.Forms.Button()
        Me.tLyricsDir = New System.Windows.Forms.TextBox()
        Me.bPlaylistFile = New System.Windows.Forms.Button()
        Me.tDatesFile = New System.Windows.Forms.TextBox()
        Me.bMusicDir = New System.Windows.Forms.Button()
        Me.tPlaylistFile = New System.Windows.Forms.TextBox()
        Me.tStatsFile = New System.Windows.Forms.TextBox()
        Me.tMusicDir = New System.Windows.Forms.TextBox()
        Me.bStatsFile = New System.Windows.Forms.Button()
        Me.listMenu = New System.Windows.Forms.ListBox()
        Me.labelMenu = New System.Windows.Forms.Label()
        Me.g6 = New System.Windows.Forms.GroupBox()
        Me.groupViewMode = New System.Windows.Forms.GroupBox()
        Me.radioFolders = New System.Windows.Forms.RadioButton()
        Me.radioPlaylists = New System.Windows.Forms.RadioButton()
        Me.checkIgnoreErrors = New System.Windows.Forms.CheckBox()
        Me.checkHiddenPlaylist = New System.Windows.Forms.CheckBox()
        Me.managePlaylistButton = New System.Windows.Forms.Button()
        Me.convertButton = New System.Windows.Forms.Button()
        Me.groupPlaylists = New System.Windows.Forms.GroupBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.radioHidden = New System.Windows.Forms.RadioButton()
        Me.radioAll = New System.Windows.Forms.RadioButton()
        Me.listPlaylists = New System.Windows.Forms.ListBox()
        Me.newPlaylistButton = New System.Windows.Forms.Button()
        Me.deletePlaylistButton = New System.Windows.Forms.Button()
        Me.g1 = New System.Windows.Forms.GroupBox()
        Me.groupUI = New System.Windows.Forms.GroupBox()
        Me.buttonFolderFont = New System.Windows.Forms.Button()
        Me.labelWinPosString = New System.Windows.Forms.Label()
        Me.buttonTrackFont = New System.Windows.Forms.Button()
        Me.buttonResetWinPos = New System.Windows.Forms.Button()
        Me.checkDarkTheme = New System.Windows.Forms.CheckBox()
        Me.buttonLyricsFont = New System.Windows.Forms.Button()
        Me.checkSavePos = New System.Windows.Forms.CheckBox()
        Me.labelWinSize = New System.Windows.Forms.Label()
        Me.labelWinSizeString = New System.Windows.Forms.Label()
        Me.labelWinPos = New System.Windows.Forms.Label()
        Me.groupMediaPlayer = New System.Windows.Forms.GroupBox()
        Me.checkRemoveTrackFromList = New System.Windows.Forms.CheckBox()
        Me.checkPlaylistHistory = New System.Windows.Forms.CheckBox()
        Me.checkRandomNextTrack = New System.Windows.Forms.CheckBox()
        Me.labelBalanceR = New System.Windows.Forms.Label()
        Me.labelBalanceL = New System.Windows.Forms.Label()
        Me.labelPlayRate = New System.Windows.Forms.Label()
        Me.labelBalance = New System.Windows.Forms.Label()
        Me.buttonProperties = New System.Windows.Forms.Button()
        Me.trackbarPlayRate = New System.Windows.Forms.TrackBar()
        Me.trackbarBalance = New System.Windows.Forms.TrackBar()
        Me.FontDlg = New System.Windows.Forms.FontDialog()
        Me.g7 = New System.Windows.Forms.GroupBox()
        Me.gadgetFormButton = New System.Windows.Forms.Button()
        Me.groupCommand.SuspendLayout()
        Me.groupModifierKey.SuspendLayout()
        Me.groupMainKey.SuspendLayout()
        Me.groupCurrentSet.SuspendLayout()
        Me.g3.SuspendLayout()
        Me.groupTimerDelay.SuspendLayout()
        CType(Me.numDelay, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.g5.SuspendLayout()
        Me.groupAssociations.SuspendLayout()
        Me.groupCurrentGenres.SuspendLayout()
        Me.g4.SuspendLayout()
        Me.groupStations.SuspendLayout()
        Me.g8.SuspendLayout()
        Me.groupTCPListener.SuspendLayout()
        Me.groupTCPConnection.SuspendLayout()
        Me.g9.SuspendLayout()
        Me.groupVersion.SuspendLayout()
        Me.g2.SuspendLayout()
        CType(Me.logPathReloadPic, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.logPathKeyPic, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.g6.SuspendLayout()
        Me.groupViewMode.SuspendLayout()
        Me.groupPlaylists.SuspendLayout()
        Me.g1.SuspendLayout()
        Me.groupUI.SuspendLayout()
        Me.groupMediaPlayer.SuspendLayout()
        CType(Me.trackbarPlayRate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trackbarBalance, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.g7.SuspendLayout()
        Me.SuspendLayout()
        '
        'commandBox
        '
        Me.commandBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.commandBox.FormattingEnabled = True
        Me.commandBox.Location = New System.Drawing.Point(26, 25)
        Me.commandBox.Name = "commandBox"
        Me.commandBox.Size = New System.Drawing.Size(121, 21)
        Me.commandBox.TabIndex = 0
        Me.commandBox.TabStop = False
        '
        'r1
        '
        Me.r1.AutoSize = True
        Me.r1.Location = New System.Drawing.Point(25, 42)
        Me.r1.Name = "r1"
        Me.r1.Size = New System.Drawing.Size(40, 17)
        Me.r1.TabIndex = 1
        Me.r1.Text = "Ctrl"
        Me.r1.UseVisualStyleBackColor = True
        '
        'r2
        '
        Me.r2.AutoSize = True
        Me.r2.Location = New System.Drawing.Point(101, 19)
        Me.r2.Name = "r2"
        Me.r2.Size = New System.Drawing.Size(48, 17)
        Me.r2.TabIndex = 2
        Me.r2.Text = "AltGr"
        Me.r2.UseVisualStyleBackColor = True
        '
        'r3
        '
        Me.r3.AutoSize = True
        Me.r3.Location = New System.Drawing.Point(101, 42)
        Me.r3.Name = "r3"
        Me.r3.Size = New System.Drawing.Size(46, 17)
        Me.r3.TabIndex = 3
        Me.r3.Text = "Shift"
        Me.r3.UseVisualStyleBackColor = True
        '
        'keyButton
        '
        Me.keyButton.Location = New System.Drawing.Point(26, 19)
        Me.keyButton.Name = "keyButton"
        Me.keyButton.Size = New System.Drawing.Size(121, 44)
        Me.keyButton.TabIndex = 4
        Me.keyButton.TabStop = False
        Me.keyButton.Text = "Assign"
        Me.keyButton.UseVisualStyleBackColor = True
        '
        'groupCommand
        '
        Me.groupCommand.Controls.Add(Me.commandBox)
        Me.groupCommand.Location = New System.Drawing.Point(7, 19)
        Me.groupCommand.Name = "groupCommand"
        Me.groupCommand.Size = New System.Drawing.Size(170, 65)
        Me.groupCommand.TabIndex = 5
        Me.groupCommand.TabStop = False
        Me.groupCommand.Text = "Command"
        '
        'groupModifierKey
        '
        Me.groupModifierKey.Controls.Add(Me.r0)
        Me.groupModifierKey.Controls.Add(Me.r1)
        Me.groupModifierKey.Controls.Add(Me.r2)
        Me.groupModifierKey.Controls.Add(Me.r3)
        Me.groupModifierKey.Location = New System.Drawing.Point(7, 90)
        Me.groupModifierKey.Name = "groupModifierKey"
        Me.groupModifierKey.Size = New System.Drawing.Size(170, 77)
        Me.groupModifierKey.TabIndex = 6
        Me.groupModifierKey.TabStop = False
        Me.groupModifierKey.Text = "Modifier Key"
        '
        'r0
        '
        Me.r0.AutoSize = True
        Me.r0.Checked = True
        Me.r0.Location = New System.Drawing.Point(25, 19)
        Me.r0.Name = "r0"
        Me.r0.Size = New System.Drawing.Size(51, 17)
        Me.r0.TabIndex = 4
        Me.r0.TabStop = True
        Me.r0.Text = "None"
        Me.r0.UseVisualStyleBackColor = True
        '
        'groupMainKey
        '
        Me.groupMainKey.Controls.Add(Me.keyButton)
        Me.groupMainKey.Location = New System.Drawing.Point(7, 173)
        Me.groupMainKey.Name = "groupMainKey"
        Me.groupMainKey.Size = New System.Drawing.Size(170, 75)
        Me.groupMainKey.TabIndex = 7
        Me.groupMainKey.TabStop = False
        Me.groupMainKey.Text = "Main Key"
        '
        'groupCurrentSet
        '
        Me.groupCurrentSet.Controls.Add(Me.defaultButton)
        Me.groupCurrentSet.Controls.Add(Me.remButton)
        Me.groupCurrentSet.Controls.Add(Me.listSet)
        Me.groupCurrentSet.Location = New System.Drawing.Point(183, 68)
        Me.groupCurrentSet.Name = "groupCurrentSet"
        Me.groupCurrentSet.Size = New System.Drawing.Size(112, 180)
        Me.groupCurrentSet.TabIndex = 8
        Me.groupCurrentSet.TabStop = False
        Me.groupCurrentSet.Text = "Current Set"
        '
        'defaultButton
        '
        Me.defaultButton.Location = New System.Drawing.Point(6, 149)
        Me.defaultButton.Name = "defaultButton"
        Me.defaultButton.Size = New System.Drawing.Size(99, 24)
        Me.defaultButton.TabIndex = 6
        Me.defaultButton.TabStop = False
        Me.defaultButton.Text = "Default"
        Me.defaultButton.UseVisualStyleBackColor = True
        '
        'remButton
        '
        Me.remButton.Location = New System.Drawing.Point(5, 116)
        Me.remButton.Name = "remButton"
        Me.remButton.Size = New System.Drawing.Size(99, 24)
        Me.remButton.TabIndex = 5
        Me.remButton.TabStop = False
        Me.remButton.Text = "Remove"
        Me.remButton.UseVisualStyleBackColor = True
        '
        'listSet
        '
        Me.listSet.FormattingEnabled = True
        Me.listSet.Location = New System.Drawing.Point(7, 17)
        Me.listSet.Name = "listSet"
        Me.listSet.Size = New System.Drawing.Size(97, 95)
        Me.listSet.TabIndex = 0
        '
        'g3
        '
        Me.g3.Controls.Add(Me.groupTimerDelay)
        Me.g3.Controls.Add(Me.groupCommand)
        Me.g3.Controls.Add(Me.groupCurrentSet)
        Me.g3.Controls.Add(Me.groupModifierKey)
        Me.g3.Controls.Add(Me.groupMainKey)
        Me.g3.Location = New System.Drawing.Point(424, 12)
        Me.g3.Name = "g3"
        Me.g3.Size = New System.Drawing.Size(302, 256)
        Me.g3.TabIndex = 9
        Me.g3.TabStop = False
        Me.g3.Text = "Key Set"
        '
        'groupTimerDelay
        '
        Me.groupTimerDelay.Controls.Add(Me.numDelay)
        Me.groupTimerDelay.Location = New System.Drawing.Point(183, 19)
        Me.groupTimerDelay.Name = "groupTimerDelay"
        Me.groupTimerDelay.Size = New System.Drawing.Size(112, 47)
        Me.groupTimerDelay.TabIndex = 6
        Me.groupTimerDelay.TabStop = False
        Me.groupTimerDelay.Text = "Timer Delay (ms)"
        '
        'numDelay
        '
        Me.numDelay.Location = New System.Drawing.Point(19, 19)
        Me.numDelay.Maximum = New Decimal(New Integer() {500, 0, 0, 0})
        Me.numDelay.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numDelay.Name = "numDelay"
        Me.numDelay.Size = New System.Drawing.Size(72, 20)
        Me.numDelay.TabIndex = 27
        Me.numDelay.TabStop = False
        Me.numDelay.Value = New Decimal(New Integer() {250, 0, 0, 0})
        '
        'g5
        '
        Me.g5.Controls.Add(Me.groupAssociations)
        Me.g5.Controls.Add(Me.groupCurrentGenres)
        Me.g5.Location = New System.Drawing.Point(1048, 12)
        Me.g5.Name = "g5"
        Me.g5.Size = New System.Drawing.Size(302, 256)
        Me.g5.TabIndex = 17
        Me.g5.TabStop = False
        Me.g5.Text = "Genres"
        '
        'groupAssociations
        '
        Me.groupAssociations.Controls.Add(Me.clearGenreDepButton)
        Me.groupAssociations.Controls.Add(Me.remGenreDepButton)
        Me.groupAssociations.Controls.Add(Me.listAssociations)
        Me.groupAssociations.Controls.Add(Me.addFolderButton)
        Me.groupAssociations.Controls.Add(Me.addTrackButton)
        Me.groupAssociations.Location = New System.Drawing.Point(138, 18)
        Me.groupAssociations.Name = "groupAssociations"
        Me.groupAssociations.Size = New System.Drawing.Size(158, 232)
        Me.groupAssociations.TabIndex = 16
        Me.groupAssociations.TabStop = False
        Me.groupAssociations.Text = "Associations"
        '
        'clearGenreDepButton
        '
        Me.clearGenreDepButton.Location = New System.Drawing.Point(82, 199)
        Me.clearGenreDepButton.Name = "clearGenreDepButton"
        Me.clearGenreDepButton.Size = New System.Drawing.Size(70, 27)
        Me.clearGenreDepButton.TabIndex = 17
        Me.clearGenreDepButton.Text = "Clear"
        Me.clearGenreDepButton.UseVisualStyleBackColor = True
        '
        'remGenreDepButton
        '
        Me.remGenreDepButton.Location = New System.Drawing.Point(6, 199)
        Me.remGenreDepButton.Name = "remGenreDepButton"
        Me.remGenreDepButton.Size = New System.Drawing.Size(70, 27)
        Me.remGenreDepButton.TabIndex = 16
        Me.remGenreDepButton.Text = "Remove"
        Me.remGenreDepButton.UseVisualStyleBackColor = True
        '
        'listAssociations
        '
        Me.listAssociations.FormattingEnabled = True
        Me.listAssociations.HorizontalScrollbar = True
        Me.listAssociations.Location = New System.Drawing.Point(6, 16)
        Me.listAssociations.Name = "listAssociations"
        Me.listAssociations.Size = New System.Drawing.Size(146, 147)
        Me.listAssociations.TabIndex = 7
        '
        'addFolderButton
        '
        Me.addFolderButton.Location = New System.Drawing.Point(82, 165)
        Me.addFolderButton.Name = "addFolderButton"
        Me.addFolderButton.Size = New System.Drawing.Size(70, 26)
        Me.addFolderButton.TabIndex = 8
        Me.addFolderButton.Text = "Add Folder"
        Me.addFolderButton.UseVisualStyleBackColor = True
        '
        'addTrackButton
        '
        Me.addTrackButton.Location = New System.Drawing.Point(6, 165)
        Me.addTrackButton.Name = "addTrackButton"
        Me.addTrackButton.Size = New System.Drawing.Size(70, 27)
        Me.addTrackButton.TabIndex = 15
        Me.addTrackButton.Text = "Add Track"
        Me.addTrackButton.UseVisualStyleBackColor = True
        '
        'groupCurrentGenres
        '
        Me.groupCurrentGenres.Controls.Add(Me.listGenres)
        Me.groupCurrentGenres.Controls.Add(Me.NewGenreButton)
        Me.groupCurrentGenres.Controls.Add(Me.remButton2)
        Me.groupCurrentGenres.Location = New System.Drawing.Point(9, 18)
        Me.groupCurrentGenres.Name = "groupCurrentGenres"
        Me.groupCurrentGenres.Size = New System.Drawing.Size(123, 232)
        Me.groupCurrentGenres.TabIndex = 12
        Me.groupCurrentGenres.TabStop = False
        Me.groupCurrentGenres.Text = "Current Genres"
        '
        'listGenres
        '
        Me.listGenres.FormattingEnabled = True
        Me.listGenres.HorizontalScrollbar = True
        Me.listGenres.Location = New System.Drawing.Point(6, 16)
        Me.listGenres.Name = "listGenres"
        Me.listGenres.Size = New System.Drawing.Size(111, 147)
        Me.listGenres.Sorted = True
        Me.listGenres.TabIndex = 7
        '
        'NewGenreButton
        '
        Me.NewGenreButton.Location = New System.Drawing.Point(6, 166)
        Me.NewGenreButton.Name = "NewGenreButton"
        Me.NewGenreButton.Size = New System.Drawing.Size(111, 27)
        Me.NewGenreButton.TabIndex = 8
        Me.NewGenreButton.Text = "New Genre"
        Me.NewGenreButton.UseVisualStyleBackColor = True
        '
        'remButton2
        '
        Me.remButton2.Location = New System.Drawing.Point(6, 199)
        Me.remButton2.Name = "remButton2"
        Me.remButton2.Size = New System.Drawing.Size(111, 27)
        Me.remButton2.TabIndex = 15
        Me.remButton2.Text = "Remove"
        Me.remButton2.UseVisualStyleBackColor = True
        '
        'g4
        '
        Me.g4.Controls.Add(Me.labelSorting)
        Me.g4.Controls.Add(Me.addButton)
        Me.g4.Controls.Add(Me.sortingBox)
        Me.g4.Controls.Add(Me.groupStations)
        Me.g4.Location = New System.Drawing.Point(732, 17)
        Me.g4.Name = "g4"
        Me.g4.Size = New System.Drawing.Size(302, 256)
        Me.g4.TabIndex = 18
        Me.g4.TabStop = False
        Me.g4.Text = "Radio Stations"
        '
        'labelSorting
        '
        Me.labelSorting.AutoEllipsis = True
        Me.labelSorting.AutoSize = True
        Me.labelSorting.Location = New System.Drawing.Point(16, 212)
        Me.labelSorting.Name = "labelSorting"
        Me.labelSorting.Size = New System.Drawing.Size(43, 13)
        Me.labelSorting.TabIndex = 24
        Me.labelSorting.Text = "Sorting:"
        '
        'addButton
        '
        Me.addButton.Location = New System.Drawing.Point(163, 215)
        Me.addButton.Name = "addButton"
        Me.addButton.Size = New System.Drawing.Size(113, 34)
        Me.addButton.TabIndex = 21
        Me.addButton.Text = "Add Station"
        Me.addButton.UseVisualStyleBackColor = True
        '
        'sortingBox
        '
        Me.sortingBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.sortingBox.FormattingEnabled = True
        Me.sortingBox.Location = New System.Drawing.Point(19, 228)
        Me.sortingBox.Name = "sortingBox"
        Me.sortingBox.Size = New System.Drawing.Size(134, 21)
        Me.sortingBox.TabIndex = 1
        '
        'groupStations
        '
        Me.groupStations.Controls.Add(Me.labelUrl)
        Me.groupStations.Controls.Add(Me.moveDownButton)
        Me.groupStations.Controls.Add(Me.listStations)
        Me.groupStations.Controls.Add(Me.moveUpButton)
        Me.groupStations.Controls.Add(Me.remButton4)
        Me.groupStations.Controls.Add(Me.changeUrlButton)
        Me.groupStations.Controls.Add(Me.changeNameButton)
        Me.groupStations.Location = New System.Drawing.Point(10, 16)
        Me.groupStations.Name = "groupStations"
        Me.groupStations.Size = New System.Drawing.Size(281, 193)
        Me.groupStations.TabIndex = 12
        Me.groupStations.TabStop = False
        Me.groupStations.Text = "Current Stations"
        '
        'labelUrl
        '
        Me.labelUrl.AutoEllipsis = True
        Me.labelUrl.Cursor = System.Windows.Forms.Cursors.Hand
        Me.labelUrl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelUrl.Location = New System.Drawing.Point(150, 139)
        Me.labelUrl.Name = "labelUrl"
        Me.labelUrl.Size = New System.Drawing.Size(116, 16)
        Me.labelUrl.TabIndex = 39
        Me.labelUrl.Text = "URL:"
        '
        'moveDownButton
        '
        Me.moveDownButton.Location = New System.Drawing.Point(212, 16)
        Me.moveDownButton.Name = "moveDownButton"
        Me.moveDownButton.Size = New System.Drawing.Size(54, 43)
        Me.moveDownButton.TabIndex = 23
        Me.moveDownButton.Text = "Move Down"
        Me.moveDownButton.UseVisualStyleBackColor = True
        '
        'listStations
        '
        Me.listStations.FormattingEnabled = True
        Me.listStations.HorizontalScrollbar = True
        Me.listStations.Location = New System.Drawing.Point(6, 15)
        Me.listStations.Name = "listStations"
        Me.listStations.Size = New System.Drawing.Size(137, 173)
        Me.listStations.TabIndex = 7
        '
        'moveUpButton
        '
        Me.moveUpButton.Location = New System.Drawing.Point(152, 16)
        Me.moveUpButton.Name = "moveUpButton"
        Me.moveUpButton.Size = New System.Drawing.Size(54, 43)
        Me.moveUpButton.TabIndex = 22
        Me.moveUpButton.Text = "Move Up"
        Me.moveUpButton.UseVisualStyleBackColor = True
        '
        'remButton4
        '
        Me.remButton4.Location = New System.Drawing.Point(152, 65)
        Me.remButton4.Name = "remButton4"
        Me.remButton4.Size = New System.Drawing.Size(114, 26)
        Me.remButton4.TabIndex = 15
        Me.remButton4.Text = "Remove"
        Me.remButton4.UseVisualStyleBackColor = True
        '
        'changeUrlButton
        '
        Me.changeUrlButton.Location = New System.Drawing.Point(152, 157)
        Me.changeUrlButton.Name = "changeUrlButton"
        Me.changeUrlButton.Size = New System.Drawing.Size(114, 26)
        Me.changeUrlButton.TabIndex = 20
        Me.changeUrlButton.Text = "Change URL"
        Me.changeUrlButton.UseVisualStyleBackColor = True
        '
        'changeNameButton
        '
        Me.changeNameButton.Location = New System.Drawing.Point(152, 100)
        Me.changeNameButton.Name = "changeNameButton"
        Me.changeNameButton.Size = New System.Drawing.Size(114, 26)
        Me.changeNameButton.TabIndex = 16
        Me.changeNameButton.Text = "Change Name"
        Me.changeNameButton.UseVisualStyleBackColor = True
        '
        'g8
        '
        Me.g8.Controls.Add(Me.groupTCPListener)
        Me.g8.Controls.Add(Me.groupTCPConnection)
        Me.g8.Location = New System.Drawing.Point(744, 286)
        Me.g8.Name = "g8"
        Me.g8.Size = New System.Drawing.Size(302, 256)
        Me.g8.TabIndex = 25
        Me.g8.TabStop = False
        Me.g8.Text = "Remote Control"
        '
        'groupTCPListener
        '
        Me.groupTCPListener.Controls.Add(Me.checkBlockExtIps)
        Me.groupTCPListener.Controls.Add(Me.labelPort)
        Me.groupTCPListener.Controls.Add(Me.Label6)
        Me.groupTCPListener.Controls.Add(Me.resetButton2)
        Me.groupTCPListener.Controls.Add(Me.Label2)
        Me.groupTCPListener.Controls.Add(Me.tPort)
        Me.groupTCPListener.Controls.Add(Me.stopButton)
        Me.groupTCPListener.Controls.Add(Me.startButton)
        Me.groupTCPListener.Controls.Add(Me.labelStatus)
        Me.groupTCPListener.Controls.Add(Me.checkEnableStartup)
        Me.groupTCPListener.Controls.Add(Me.Label5)
        Me.groupTCPListener.Location = New System.Drawing.Point(10, 140)
        Me.groupTCPListener.Name = "groupTCPListener"
        Me.groupTCPListener.Size = New System.Drawing.Size(282, 109)
        Me.groupTCPListener.TabIndex = 19
        Me.groupTCPListener.TabStop = False
        Me.groupTCPListener.Text = "TCP Listener"
        '
        'checkBlockExtIps
        '
        Me.checkBlockExtIps.AutoSize = True
        Me.checkBlockExtIps.Location = New System.Drawing.Point(158, 89)
        Me.checkBlockExtIps.Name = "checkBlockExtIps"
        Me.checkBlockExtIps.Size = New System.Drawing.Size(116, 17)
        Me.checkBlockExtIps.TabIndex = 47
        Me.checkBlockExtIps.Text = "Block non-LAN IPs"
        Me.checkBlockExtIps.UseVisualStyleBackColor = True
        '
        'labelPort
        '
        Me.labelPort.AutoSize = True
        Me.labelPort.Location = New System.Drawing.Point(158, 31)
        Me.labelPort.Name = "labelPort"
        Me.labelPort.Size = New System.Drawing.Size(37, 13)
        Me.labelPort.TabIndex = 46
        Me.labelPort.Text = "55555"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(158, 15)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(66, 13)
        Me.Label6.TabIndex = 45
        Me.Label6.Text = "Current Port:"
        '
        'resetButton2
        '
        Me.resetButton2.Location = New System.Drawing.Point(220, 54)
        Me.resetButton2.Name = "resetButton2"
        Me.resetButton2.Size = New System.Drawing.Size(53, 31)
        Me.resetButton2.TabIndex = 44
        Me.resetButton2.Text = "Reset"
        Me.resetButton2.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(158, 49)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(54, 13)
        Me.Label2.TabIndex = 43
        Me.Label2.Text = "New Port:"
        '
        'tPort
        '
        Me.tPort.Location = New System.Drawing.Point(158, 65)
        Me.tPort.Name = "tPort"
        Me.tPort.Size = New System.Drawing.Size(58, 20)
        Me.tPort.TabIndex = 42
        '
        'stopButton
        '
        Me.stopButton.Location = New System.Drawing.Point(75, 55)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(54, 31)
        Me.stopButton.TabIndex = 41
        Me.stopButton.Text = "Stop"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(15, 55)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(54, 31)
        Me.startButton.TabIndex = 40
        Me.startButton.Text = "Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'labelStatus
        '
        Me.labelStatus.AutoSize = True
        Me.labelStatus.Location = New System.Drawing.Point(50, 30)
        Me.labelStatus.Name = "labelStatus"
        Me.labelStatus.Size = New System.Drawing.Size(49, 13)
        Me.labelStatus.TabIndex = 23
        Me.labelStatus.Text = "Listening"
        '
        'checkEnableStartup
        '
        Me.checkEnableStartup.AutoSize = True
        Me.checkEnableStartup.Location = New System.Drawing.Point(15, 89)
        Me.checkEnableStartup.Name = "checkEnableStartup"
        Me.checkEnableStartup.Size = New System.Drawing.Size(111, 17)
        Me.checkEnableStartup.TabIndex = 18
        Me.checkEnableStartup.Text = "Enable on Startup"
        Me.checkEnableStartup.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(50, 16)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(40, 13)
        Me.Label5.TabIndex = 22
        Me.Label5.Text = "Status:"
        '
        'groupTCPConnection
        '
        Me.groupTCPConnection.Controls.Add(Me.remoteSendButton)
        Me.groupTCPConnection.Controls.Add(Me.stopAllConnectionsButton)
        Me.groupTCPConnection.Controls.Add(Me.listPairedIps)
        Me.groupTCPConnection.Controls.Add(Me.checkBlockMessages)
        Me.groupTCPConnection.Controls.Add(Me.Label15)
        Me.groupTCPConnection.Controls.Add(Me.labelExtIp)
        Me.groupTCPConnection.Controls.Add(Me.labelOwnIp)
        Me.groupTCPConnection.Controls.Add(Me.Label4)
        Me.groupTCPConnection.Controls.Add(Me.stopConnectionButton)
        Me.groupTCPConnection.Controls.Add(Me.Label3)
        Me.groupTCPConnection.Location = New System.Drawing.Point(10, 25)
        Me.groupTCPConnection.Name = "groupTCPConnection"
        Me.groupTCPConnection.Size = New System.Drawing.Size(282, 111)
        Me.groupTCPConnection.TabIndex = 12
        Me.groupTCPConnection.TabStop = False
        Me.groupTCPConnection.Text = "Connection"
        '
        'remoteSendButton
        '
        Me.remoteSendButton.Location = New System.Drawing.Point(191, 12)
        Me.remoteSendButton.Name = "remoteSendButton"
        Me.remoteSendButton.Size = New System.Drawing.Size(81, 21)
        Me.remoteSendButton.TabIndex = 48
        Me.remoteSendButton.Text = "Send..."
        Me.remoteSendButton.UseVisualStyleBackColor = True
        '
        'stopAllConnectionsButton
        '
        Me.stopAllConnectionsButton.Location = New System.Drawing.Point(191, 69)
        Me.stopAllConnectionsButton.Name = "stopAllConnectionsButton"
        Me.stopAllConnectionsButton.Size = New System.Drawing.Size(81, 34)
        Me.stopAllConnectionsButton.TabIndex = 50
        Me.stopAllConnectionsButton.Text = "Stop All Connections"
        Me.stopAllConnectionsButton.UseVisualStyleBackColor = True
        '
        'listPairedIps
        '
        Me.listPairedIps.FormattingEnabled = True
        Me.listPairedIps.Location = New System.Drawing.Point(94, 34)
        Me.listPairedIps.Name = "listPairedIps"
        Me.listPairedIps.Size = New System.Drawing.Size(90, 69)
        Me.listPairedIps.TabIndex = 49
        '
        'checkBlockMessages
        '
        Me.checkBlockMessages.Location = New System.Drawing.Point(11, 78)
        Me.checkBlockMessages.Name = "checkBlockMessages"
        Me.checkBlockMessages.Size = New System.Drawing.Size(76, 30)
        Me.checkBlockMessages.TabIndex = 48
        Me.checkBlockMessages.Text = "Block messages"
        Me.checkBlockMessages.UseVisualStyleBackColor = True
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(8, 47)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(61, 13)
        Me.Label15.TabIndex = 23
        Me.Label15.Text = "External IP:"
        '
        'labelExtIp
        '
        Me.labelExtIp.AutoSize = True
        Me.labelExtIp.Location = New System.Drawing.Point(7, 62)
        Me.labelExtIp.Name = "labelExtIp"
        Me.labelExtIp.Size = New System.Drawing.Size(40, 13)
        Me.labelExtIp.TabIndex = 22
        Me.labelExtIp.Text = "0.0.0.0"
        '
        'labelOwnIp
        '
        Me.labelOwnIp.AutoSize = True
        Me.labelOwnIp.Location = New System.Drawing.Point(7, 30)
        Me.labelOwnIp.Name = "labelOwnIp"
        Me.labelOwnIp.Size = New System.Drawing.Size(64, 13)
        Me.labelOwnIp.TabIndex = 21
        Me.labelOwnIp.Text = "192.168.2.0"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(8, 15)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(56, 13)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "Localhost:"
        '
        'stopConnectionButton
        '
        Me.stopConnectionButton.Location = New System.Drawing.Point(191, 34)
        Me.stopConnectionButton.Name = "stopConnectionButton"
        Me.stopConnectionButton.Size = New System.Drawing.Size(81, 34)
        Me.stopConnectionButton.TabIndex = 19
        Me.stopConnectionButton.Text = "Stop Connection"
        Me.stopConnectionButton.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(96, 16)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(84, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Currently Paired:"
        '
        'g9
        '
        Me.g9.Controls.Add(Me.groupVersion)
        Me.g9.Controls.Add(Me.searchHomeIpButton)
        Me.g9.Controls.Add(Me.tftpUser)
        Me.g9.Controls.Add(Me.tftpPw)
        Me.g9.Controls.Add(Me.Label10)
        Me.g9.Controls.Add(Me.Label11)
        Me.g9.Controls.Add(Me.Label9)
        Me.g9.Controls.Add(Me.tftpIp)
        Me.g9.Controls.Add(Me.labelftpCurrProg)
        Me.g9.Controls.Add(Me.labelftpTotalProg)
        Me.g9.Controls.Add(Me.Label13)
        Me.g9.Controls.Add(Me.pBar2)
        Me.g9.Controls.Add(Me.pBar)
        Me.g9.Controls.Add(Me.Label7)
        Me.g9.Controls.Add(Me.DownloadLatestButton)
        Me.g9.Controls.Add(Me.checkAutoUpdate)
        Me.g9.Controls.Add(Me.checkUpdatesButton)
        Me.g9.Controls.Add(Me.Label12)
        Me.g9.Controls.Add(Me.labelCurrVersion)
        Me.g9.Location = New System.Drawing.Point(1052, 277)
        Me.g9.Name = "g9"
        Me.g9.Size = New System.Drawing.Size(302, 256)
        Me.g9.TabIndex = 26
        Me.g9.TabStop = False
        Me.g9.Text = "Player Update"
        '
        'groupVersion
        '
        Me.groupVersion.Controls.Add(Me.publishPathButton)
        Me.groupVersion.Controls.Add(Me.listPublish)
        Me.groupVersion.Controls.Add(Me.publishButton)
        Me.groupVersion.Controls.Add(Me.Label8)
        Me.groupVersion.Controls.Add(Me.labelPublishedVersion)
        Me.groupVersion.Controls.Add(Me.publishAddButton)
        Me.groupVersion.Controls.Add(Me.publishRemButton)
        Me.groupVersion.Location = New System.Drawing.Point(136, 108)
        Me.groupVersion.Name = "groupVersion"
        Me.groupVersion.Size = New System.Drawing.Size(162, 146)
        Me.groupVersion.TabIndex = 63
        Me.groupVersion.TabStop = False
        '
        'publishPathButton
        '
        Me.publishPathButton.Location = New System.Drawing.Point(97, 43)
        Me.publishPathButton.Name = "publishPathButton"
        Me.publishPathButton.Size = New System.Drawing.Size(26, 27)
        Me.publishPathButton.TabIndex = 63
        Me.publishPathButton.Text = "..."
        Me.publishPathButton.UseVisualStyleBackColor = True
        '
        'listPublish
        '
        Me.listPublish.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.listPublish.FormattingEnabled = True
        Me.listPublish.HorizontalScrollbar = True
        Me.listPublish.Location = New System.Drawing.Point(4, 75)
        Me.listPublish.Name = "listPublish"
        Me.listPublish.Size = New System.Drawing.Size(128, 69)
        Me.listPublish.TabIndex = 28
        '
        'publishButton
        '
        Me.publishButton.Location = New System.Drawing.Point(32, 43)
        Me.publishButton.Name = "publishButton"
        Me.publishButton.Size = New System.Drawing.Size(66, 27)
        Me.publishButton.TabIndex = 49
        Me.publishButton.Text = "Publish"
        Me.publishButton.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(19, 11)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(117, 13)
        Me.Label8.TabIndex = 51
        Me.Label8.Text = "Publish current version:"
        '
        'labelPublishedVersion
        '
        Me.labelPublishedVersion.AutoSize = True
        Me.labelPublishedVersion.Location = New System.Drawing.Point(24, 27)
        Me.labelPublishedVersion.Name = "labelPublishedVersion"
        Me.labelPublishedVersion.Size = New System.Drawing.Size(109, 13)
        Me.labelPublishedVersion.TabIndex = 61
        Me.labelPublishedVersion.Text = "2017.08.16_16.09.11"
        '
        'publishAddButton
        '
        Me.publishAddButton.Location = New System.Drawing.Point(134, 82)
        Me.publishAddButton.Name = "publishAddButton"
        Me.publishAddButton.Size = New System.Drawing.Size(25, 25)
        Me.publishAddButton.TabIndex = 48
        Me.publishAddButton.Text = "+"
        Me.publishAddButton.UseVisualStyleBackColor = True
        '
        'publishRemButton
        '
        Me.publishRemButton.Location = New System.Drawing.Point(134, 109)
        Me.publishRemButton.Name = "publishRemButton"
        Me.publishRemButton.Size = New System.Drawing.Size(25, 25)
        Me.publishRemButton.TabIndex = 62
        Me.publishRemButton.Text = "-"
        Me.publishRemButton.UseVisualStyleBackColor = True
        '
        'searchHomeIpButton
        '
        Me.searchHomeIpButton.Location = New System.Drawing.Point(209, 14)
        Me.searchHomeIpButton.Name = "searchHomeIpButton"
        Me.searchHomeIpButton.Size = New System.Drawing.Size(67, 23)
        Me.searchHomeIpButton.TabIndex = 57
        Me.searchHomeIpButton.Text = "Search"
        Me.searchHomeIpButton.UseVisualStyleBackColor = True
        '
        'tftpUser
        '
        Me.tftpUser.Location = New System.Drawing.Point(199, 66)
        Me.tftpUser.Name = "tftpUser"
        Me.tftpUser.Size = New System.Drawing.Size(87, 20)
        Me.tftpUser.TabIndex = 53
        Me.tftpUser.Text = "updateplayer"
        '
        'tftpPw
        '
        Me.tftpPw.Location = New System.Drawing.Point(199, 88)
        Me.tftpPw.Name = "tftpPw"
        Me.tftpPw.Size = New System.Drawing.Size(87, 20)
        Me.tftpPw.TabIndex = 52
        Me.tftpPw.Text = "huan"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(134, 69)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(58, 13)
        Me.Label10.TabIndex = 55
        Me.Label10.Text = "Username:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(136, 91)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(56, 13)
        Me.Label11.TabIndex = 56
        Me.Label11.Text = "Password:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(137, 42)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(54, 13)
        Me.Label9.TabIndex = 54
        Me.Label9.Text = "Server IP:"
        '
        'tftpIp
        '
        Me.tftpIp.Location = New System.Drawing.Point(199, 39)
        Me.tftpIp.Name = "tftpIp"
        Me.tftpIp.Size = New System.Drawing.Size(87, 20)
        Me.tftpIp.TabIndex = 47
        Me.tftpIp.Text = "127.0.0.1"
        '
        'labelftpCurrProg
        '
        Me.labelftpCurrProg.AutoSize = True
        Me.labelftpCurrProg.Location = New System.Drawing.Point(5, 198)
        Me.labelftpCurrProg.Name = "labelftpCurrProg"
        Me.labelftpCurrProg.Size = New System.Drawing.Size(30, 13)
        Me.labelftpCurrProg.TabIndex = 60
        Me.labelftpCurrProg.Text = "0 / 0"
        '
        'labelftpTotalProg
        '
        Me.labelftpTotalProg.AutoSize = True
        Me.labelftpTotalProg.Location = New System.Drawing.Point(38, 238)
        Me.labelftpTotalProg.Name = "labelftpTotalProg"
        Me.labelftpTotalProg.Size = New System.Drawing.Size(33, 13)
        Me.labelftpTotalProg.TabIndex = 58
        Me.labelftpTotalProg.Text = "0 / 0 "
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(5, 238)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(31, 13)
        Me.Label13.TabIndex = 57
        Me.Label13.Text = "Files:"
        '
        'pBar2
        '
        Me.pBar2.Location = New System.Drawing.Point(7, 177)
        Me.pBar2.Name = "pBar2"
        Me.pBar2.Size = New System.Drawing.Size(123, 16)
        Me.pBar2.TabIndex = 53
        '
        'pBar
        '
        Me.pBar.Location = New System.Drawing.Point(8, 216)
        Me.pBar.Name = "pBar"
        Me.pBar.Size = New System.Drawing.Size(123, 16)
        Me.pBar.TabIndex = 52
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(12, 118)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(123, 13)
        Me.Label7.TabIndex = 50
        Me.Label7.Text = "Download latest version:"
        '
        'DownloadLatestButton
        '
        Me.DownloadLatestButton.Location = New System.Drawing.Point(34, 136)
        Me.DownloadLatestButton.Name = "DownloadLatestButton"
        Me.DownloadLatestButton.Size = New System.Drawing.Size(75, 36)
        Me.DownloadLatestButton.TabIndex = 48
        Me.DownloadLatestButton.Text = "Download"
        Me.DownloadLatestButton.UseVisualStyleBackColor = True
        '
        'checkAutoUpdate
        '
        Me.checkAutoUpdate.AutoSize = True
        Me.checkAutoUpdate.Location = New System.Drawing.Point(32, 91)
        Me.checkAutoUpdate.Name = "checkAutoUpdate"
        Me.checkAutoUpdate.Size = New System.Drawing.Size(85, 17)
        Me.checkAutoUpdate.TabIndex = 47
        Me.checkAutoUpdate.Text = "Auto Search"
        Me.checkAutoUpdate.UseVisualStyleBackColor = True
        '
        'checkUpdatesButton
        '
        Me.checkUpdatesButton.Location = New System.Drawing.Point(34, 52)
        Me.checkUpdatesButton.Name = "checkUpdatesButton"
        Me.checkUpdatesButton.Size = New System.Drawing.Size(75, 34)
        Me.checkUpdatesButton.TabIndex = 47
        Me.checkUpdatesButton.Text = "Check for Updates"
        Me.checkUpdatesButton.UseVisualStyleBackColor = True
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(16, 19)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(81, 13)
        Me.Label12.TabIndex = 21
        Me.Label12.Text = "Current version:"
        '
        'labelCurrVersion
        '
        Me.labelCurrVersion.AutoSize = True
        Me.labelCurrVersion.Location = New System.Drawing.Point(16, 36)
        Me.labelCurrVersion.Name = "labelCurrVersion"
        Me.labelCurrVersion.Size = New System.Drawing.Size(109, 13)
        Me.labelCurrVersion.TabIndex = 20
        Me.labelCurrVersion.Text = "2017.07.28_23.54.44"
        '
        'g2
        '
        Me.g2.Controls.Add(Me.logPathReloadPic)
        Me.g2.Controls.Add(Me.logPathKeyPic)
        Me.g2.Controls.Add(Me.Label20)
        Me.g2.Controls.Add(Me.Label19)
        Me.g2.Controls.Add(Me.Label18)
        Me.g2.Controls.Add(Me.Label17)
        Me.g2.Controls.Add(Me.Label16)
        Me.g2.Controls.Add(Me.Label14)
        Me.g2.Controls.Add(Me.bFtpDir)
        Me.g2.Controls.Add(Me.bLyricsDir)
        Me.g2.Controls.Add(Me.tFtpDir)
        Me.g2.Controls.Add(Me.bDatesFile)
        Me.g2.Controls.Add(Me.tLyricsDir)
        Me.g2.Controls.Add(Me.bPlaylistFile)
        Me.g2.Controls.Add(Me.tDatesFile)
        Me.g2.Controls.Add(Me.bMusicDir)
        Me.g2.Controls.Add(Me.tPlaylistFile)
        Me.g2.Controls.Add(Me.tStatsFile)
        Me.g2.Controls.Add(Me.tMusicDir)
        Me.g2.Controls.Add(Me.bStatsFile)
        Me.g2.Location = New System.Drawing.Point(116, 8)
        Me.g2.Name = "g2"
        Me.g2.Size = New System.Drawing.Size(302, 256)
        Me.g2.TabIndex = 17
        Me.g2.TabStop = False
        Me.g2.Text = "Paths"
        '
        'logPathReloadPic
        '
        Me.logPathReloadPic.BackgroundImage = Global.MediaPlayer.My.Resources.Resources.rel
        Me.logPathReloadPic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.logPathReloadPic.Cursor = System.Windows.Forms.Cursors.Hand
        Me.logPathReloadPic.Location = New System.Drawing.Point(267, 144)
        Me.logPathReloadPic.Name = "logPathReloadPic"
        Me.logPathReloadPic.Size = New System.Drawing.Size(30, 27)
        Me.logPathReloadPic.TabIndex = 56
        Me.logPathReloadPic.TabStop = False
        '
        'logPathKeyPic
        '
        Me.logPathKeyPic.BackgroundImage = Global.MediaPlayer.My.Resources.Resources.unlock
        Me.logPathKeyPic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.logPathKeyPic.Cursor = System.Windows.Forms.Cursors.Hand
        Me.logPathKeyPic.Location = New System.Drawing.Point(233, 144)
        Me.logPathKeyPic.Name = "logPathKeyPic"
        Me.logPathKeyPic.Size = New System.Drawing.Size(30, 27)
        Me.logPathKeyPic.TabIndex = 55
        Me.logPathKeyPic.TabStop = False
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label20.Location = New System.Drawing.Point(10, 214)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(106, 13)
        Me.Label20.TabIndex = 54
        Me.Label20.Text = "Ftp Sharing Directory"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label19.Location = New System.Drawing.Point(10, 174)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(79, 13)
        Me.Label19.TabIndex = 53
        Me.Label19.Text = "Lyrics Directory"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label18.Location = New System.Drawing.Point(10, 135)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(85, 13)
        Me.Label18.TabIndex = 52
        Me.Label18.Text = "Track Dates File"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label17.Location = New System.Drawing.Point(10, 95)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(63, 13)
        Me.Label17.TabIndex = 51
        Me.Label17.Text = "Playlists File"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label16.Location = New System.Drawing.Point(10, 55)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(93, 13)
        Me.Label16.TabIndex = 50
        Me.Label16.Text = "Music Root Folder"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label14.Location = New System.Drawing.Point(10, 13)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(93, 13)
        Me.Label14.TabIndex = 20
        Me.Label14.Text = "Settings/Stats File"
        '
        'bFtpDir
        '
        Me.bFtpDir.Location = New System.Drawing.Point(258, 227)
        Me.bFtpDir.Name = "bFtpDir"
        Me.bFtpDir.Size = New System.Drawing.Size(32, 21)
        Me.bFtpDir.TabIndex = 49
        Me.bFtpDir.Text = "..."
        Me.bFtpDir.UseVisualStyleBackColor = True
        '
        'bLyricsDir
        '
        Me.bLyricsDir.Location = New System.Drawing.Point(258, 187)
        Me.bLyricsDir.Name = "bLyricsDir"
        Me.bLyricsDir.Size = New System.Drawing.Size(32, 21)
        Me.bLyricsDir.TabIndex = 49
        Me.bLyricsDir.Text = "..."
        Me.bLyricsDir.UseVisualStyleBackColor = True
        '
        'tFtpDir
        '
        Me.tFtpDir.Location = New System.Drawing.Point(13, 228)
        Me.tFtpDir.Name = "tFtpDir"
        Me.tFtpDir.Size = New System.Drawing.Size(240, 20)
        Me.tFtpDir.TabIndex = 48
        '
        'bDatesFile
        '
        Me.bDatesFile.Location = New System.Drawing.Point(199, 147)
        Me.bDatesFile.Name = "bDatesFile"
        Me.bDatesFile.Size = New System.Drawing.Size(32, 21)
        Me.bDatesFile.TabIndex = 49
        Me.bDatesFile.Text = "..."
        Me.bDatesFile.UseVisualStyleBackColor = True
        '
        'tLyricsDir
        '
        Me.tLyricsDir.Location = New System.Drawing.Point(13, 188)
        Me.tLyricsDir.Name = "tLyricsDir"
        Me.tLyricsDir.Size = New System.Drawing.Size(240, 20)
        Me.tLyricsDir.TabIndex = 48
        '
        'bPlaylistFile
        '
        Me.bPlaylistFile.Location = New System.Drawing.Point(258, 107)
        Me.bPlaylistFile.Name = "bPlaylistFile"
        Me.bPlaylistFile.Size = New System.Drawing.Size(32, 21)
        Me.bPlaylistFile.TabIndex = 49
        Me.bPlaylistFile.Text = "..."
        Me.bPlaylistFile.UseVisualStyleBackColor = True
        '
        'tDatesFile
        '
        Me.tDatesFile.Location = New System.Drawing.Point(13, 148)
        Me.tDatesFile.Name = "tDatesFile"
        Me.tDatesFile.Size = New System.Drawing.Size(183, 20)
        Me.tDatesFile.TabIndex = 48
        '
        'bMusicDir
        '
        Me.bMusicDir.Location = New System.Drawing.Point(258, 67)
        Me.bMusicDir.Name = "bMusicDir"
        Me.bMusicDir.Size = New System.Drawing.Size(32, 21)
        Me.bMusicDir.TabIndex = 49
        Me.bMusicDir.Text = "..."
        Me.bMusicDir.UseVisualStyleBackColor = True
        '
        'tPlaylistFile
        '
        Me.tPlaylistFile.Location = New System.Drawing.Point(13, 108)
        Me.tPlaylistFile.Name = "tPlaylistFile"
        Me.tPlaylistFile.Size = New System.Drawing.Size(240, 20)
        Me.tPlaylistFile.TabIndex = 48
        '
        'tStatsFile
        '
        Me.tStatsFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tStatsFile.Location = New System.Drawing.Point(13, 28)
        Me.tStatsFile.Name = "tStatsFile"
        Me.tStatsFile.Size = New System.Drawing.Size(240, 20)
        Me.tStatsFile.TabIndex = 48
        '
        'tMusicDir
        '
        Me.tMusicDir.Location = New System.Drawing.Point(13, 68)
        Me.tMusicDir.Name = "tMusicDir"
        Me.tMusicDir.Size = New System.Drawing.Size(240, 20)
        Me.tMusicDir.TabIndex = 48
        '
        'bStatsFile
        '
        Me.bStatsFile.Location = New System.Drawing.Point(258, 27)
        Me.bStatsFile.Name = "bStatsFile"
        Me.bStatsFile.Size = New System.Drawing.Size(32, 21)
        Me.bStatsFile.TabIndex = 49
        Me.bStatsFile.Text = "..."
        Me.bStatsFile.UseVisualStyleBackColor = True
        '
        'listMenu
        '
        Me.listMenu.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.listMenu.FormattingEnabled = True
        Me.listMenu.HorizontalScrollbar = True
        Me.listMenu.ItemHeight = 16
        Me.listMenu.Items.AddRange(New Object() {"Player", "Paths", "Hotkeys", "Radio", "Genres", "Playlists", "Gadgets", "Remote Control", "Version Update"})
        Me.listMenu.Location = New System.Drawing.Point(4, 35)
        Me.listMenu.Name = "listMenu"
        Me.listMenu.Size = New System.Drawing.Size(111, 148)
        Me.listMenu.TabIndex = 8
        '
        'labelMenu
        '
        Me.labelMenu.AutoSize = True
        Me.labelMenu.Location = New System.Drawing.Point(6, 185)
        Me.labelMenu.MaximumSize = New System.Drawing.Size(100, 0)
        Me.labelMenu.Name = "labelMenu"
        Me.labelMenu.Size = New System.Drawing.Size(60, 13)
        Me.labelMenu.TabIndex = 27
        Me.labelMenu.Text = "Description"
        '
        'g6
        '
        Me.g6.Controls.Add(Me.groupViewMode)
        Me.g6.Controls.Add(Me.checkIgnoreErrors)
        Me.g6.Controls.Add(Me.checkHiddenPlaylist)
        Me.g6.Controls.Add(Me.managePlaylistButton)
        Me.g6.Controls.Add(Me.convertButton)
        Me.g6.Controls.Add(Me.groupPlaylists)
        Me.g6.Controls.Add(Me.deletePlaylistButton)
        Me.g6.Location = New System.Drawing.Point(1366, 17)
        Me.g6.Name = "g6"
        Me.g6.Size = New System.Drawing.Size(302, 256)
        Me.g6.TabIndex = 18
        Me.g6.TabStop = False
        Me.g6.Text = "Playlists"
        '
        'groupViewMode
        '
        Me.groupViewMode.Controls.Add(Me.radioFolders)
        Me.groupViewMode.Controls.Add(Me.radioPlaylists)
        Me.groupViewMode.Location = New System.Drawing.Point(189, 13)
        Me.groupViewMode.Name = "groupViewMode"
        Me.groupViewMode.Size = New System.Drawing.Size(105, 64)
        Me.groupViewMode.TabIndex = 69
        Me.groupViewMode.TabStop = False
        Me.groupViewMode.Text = "View Mode"
        '
        'radioFolders
        '
        Me.radioFolders.AutoSize = True
        Me.radioFolders.Location = New System.Drawing.Point(24, 38)
        Me.radioFolders.Name = "radioFolders"
        Me.radioFolders.Size = New System.Drawing.Size(59, 17)
        Me.radioFolders.TabIndex = 50
        Me.radioFolders.Text = "Folders"
        Me.radioFolders.UseVisualStyleBackColor = True
        '
        'radioPlaylists
        '
        Me.radioPlaylists.AutoSize = True
        Me.radioPlaylists.Checked = True
        Me.radioPlaylists.Location = New System.Drawing.Point(24, 17)
        Me.radioPlaylists.Name = "radioPlaylists"
        Me.radioPlaylists.Size = New System.Drawing.Size(62, 17)
        Me.radioPlaylists.TabIndex = 49
        Me.radioPlaylists.TabStop = True
        Me.radioPlaylists.Text = "Playlists"
        Me.radioPlaylists.UseVisualStyleBackColor = True
        '
        'checkIgnoreErrors
        '
        Me.checkIgnoreErrors.AutoSize = True
        Me.checkIgnoreErrors.Location = New System.Drawing.Point(199, 122)
        Me.checkIgnoreErrors.Name = "checkIgnoreErrors"
        Me.checkIgnoreErrors.Size = New System.Drawing.Size(86, 17)
        Me.checkIgnoreErrors.TabIndex = 68
        Me.checkIgnoreErrors.Text = "Ignore Errors"
        Me.checkIgnoreErrors.UseVisualStyleBackColor = True
        '
        'checkHiddenPlaylist
        '
        Me.checkHiddenPlaylist.AutoSize = True
        Me.checkHiddenPlaylist.Location = New System.Drawing.Point(199, 98)
        Me.checkHiddenPlaylist.Name = "checkHiddenPlaylist"
        Me.checkHiddenPlaylist.Size = New System.Drawing.Size(91, 17)
        Me.checkHiddenPlaylist.TabIndex = 64
        Me.checkHiddenPlaylist.Text = "Visible Playlist"
        Me.checkHiddenPlaylist.UseVisualStyleBackColor = True
        '
        'managePlaylistButton
        '
        Me.managePlaylistButton.Location = New System.Drawing.Point(188, 150)
        Me.managePlaylistButton.Name = "managePlaylistButton"
        Me.managePlaylistButton.Size = New System.Drawing.Size(111, 44)
        Me.managePlaylistButton.TabIndex = 16
        Me.managePlaylistButton.Text = "Manage Tracks"
        Me.managePlaylistButton.UseVisualStyleBackColor = True
        '
        'convertButton
        '
        Me.convertButton.Location = New System.Drawing.Point(188, 196)
        Me.convertButton.Name = "convertButton"
        Me.convertButton.Size = New System.Drawing.Size(111, 27)
        Me.convertButton.TabIndex = 67
        Me.convertButton.Text = "Convert to Folder"
        Me.convertButton.UseVisualStyleBackColor = True
        '
        'groupPlaylists
        '
        Me.groupPlaylists.Controls.Add(Me.Label22)
        Me.groupPlaylists.Controls.Add(Me.radioHidden)
        Me.groupPlaylists.Controls.Add(Me.radioAll)
        Me.groupPlaylists.Controls.Add(Me.listPlaylists)
        Me.groupPlaylists.Controls.Add(Me.newPlaylistButton)
        Me.groupPlaylists.Location = New System.Drawing.Point(9, 13)
        Me.groupPlaylists.Name = "groupPlaylists"
        Me.groupPlaylists.Size = New System.Drawing.Size(176, 241)
        Me.groupPlaylists.TabIndex = 12
        Me.groupPlaylists.TabStop = False
        Me.groupPlaylists.Text = "Current Playlists"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(6, 17)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(37, 13)
        Me.Label22.TabIndex = 48
        Me.Label22.Text = "Show:"
        '
        'radioHidden
        '
        Me.radioHidden.AutoSize = True
        Me.radioHidden.Location = New System.Drawing.Point(89, 16)
        Me.radioHidden.Name = "radioHidden"
        Me.radioHidden.Size = New System.Drawing.Size(59, 17)
        Me.radioHidden.TabIndex = 10
        Me.radioHidden.Text = "Hidden"
        Me.radioHidden.UseVisualStyleBackColor = True
        '
        'radioAll
        '
        Me.radioAll.AutoSize = True
        Me.radioAll.Checked = True
        Me.radioAll.Location = New System.Drawing.Point(47, 16)
        Me.radioAll.Name = "radioAll"
        Me.radioAll.Size = New System.Drawing.Size(36, 17)
        Me.radioAll.TabIndex = 5
        Me.radioAll.TabStop = True
        Me.radioAll.Text = "All"
        Me.radioAll.UseVisualStyleBackColor = True
        '
        'listPlaylists
        '
        Me.listPlaylists.FormattingEnabled = True
        Me.listPlaylists.HorizontalScrollbar = True
        Me.listPlaylists.Location = New System.Drawing.Point(6, 36)
        Me.listPlaylists.Name = "listPlaylists"
        Me.listPlaylists.Size = New System.Drawing.Size(164, 173)
        Me.listPlaylists.Sorted = True
        Me.listPlaylists.TabIndex = 7
        '
        'newPlaylistButton
        '
        Me.newPlaylistButton.Location = New System.Drawing.Point(6, 211)
        Me.newPlaylistButton.Name = "newPlaylistButton"
        Me.newPlaylistButton.Size = New System.Drawing.Size(164, 27)
        Me.newPlaylistButton.TabIndex = 8
        Me.newPlaylistButton.Text = "New Playlist"
        Me.newPlaylistButton.UseVisualStyleBackColor = True
        '
        'deletePlaylistButton
        '
        Me.deletePlaylistButton.Location = New System.Drawing.Point(188, 224)
        Me.deletePlaylistButton.Name = "deletePlaylistButton"
        Me.deletePlaylistButton.Size = New System.Drawing.Size(111, 27)
        Me.deletePlaylistButton.TabIndex = 15
        Me.deletePlaylistButton.Text = "Delete"
        Me.deletePlaylistButton.UseVisualStyleBackColor = True
        '
        'g1
        '
        Me.g1.Controls.Add(Me.groupUI)
        Me.g1.Controls.Add(Me.groupMediaPlayer)
        Me.g1.Location = New System.Drawing.Point(116, 286)
        Me.g1.Name = "g1"
        Me.g1.Size = New System.Drawing.Size(302, 256)
        Me.g1.TabIndex = 19
        Me.g1.TabStop = False
        Me.g1.Text = "Player Settings"
        '
        'groupUI
        '
        Me.groupUI.Controls.Add(Me.buttonFolderFont)
        Me.groupUI.Controls.Add(Me.labelWinPosString)
        Me.groupUI.Controls.Add(Me.buttonTrackFont)
        Me.groupUI.Controls.Add(Me.buttonResetWinPos)
        Me.groupUI.Controls.Add(Me.checkDarkTheme)
        Me.groupUI.Controls.Add(Me.buttonLyricsFont)
        Me.groupUI.Controls.Add(Me.checkSavePos)
        Me.groupUI.Controls.Add(Me.labelWinSize)
        Me.groupUI.Controls.Add(Me.labelWinSizeString)
        Me.groupUI.Controls.Add(Me.labelWinPos)
        Me.groupUI.Location = New System.Drawing.Point(3, 13)
        Me.groupUI.Name = "groupUI"
        Me.groupUI.Size = New System.Drawing.Size(296, 101)
        Me.groupUI.TabIndex = 27
        Me.groupUI.TabStop = False
        Me.groupUI.Text = "UI"
        '
        'buttonFolderFont
        '
        Me.buttonFolderFont.Location = New System.Drawing.Point(15, 52)
        Me.buttonFolderFont.Name = "buttonFolderFont"
        Me.buttonFolderFont.Size = New System.Drawing.Size(75, 23)
        Me.buttonFolderFont.TabIndex = 4
        Me.buttonFolderFont.Text = "Folder Font"
        Me.buttonFolderFont.UseVisualStyleBackColor = True
        '
        'labelWinPosString
        '
        Me.labelWinPosString.AutoSize = True
        Me.labelWinPosString.Location = New System.Drawing.Point(135, 9)
        Me.labelWinPosString.Name = "labelWinPosString"
        Me.labelWinPosString.Size = New System.Drawing.Size(51, 13)
        Me.labelWinPosString.TabIndex = 21
        Me.labelWinPosString.Text = "Location:"
        '
        'buttonTrackFont
        '
        Me.buttonTrackFont.Location = New System.Drawing.Point(108, 52)
        Me.buttonTrackFont.Name = "buttonTrackFont"
        Me.buttonTrackFont.Size = New System.Drawing.Size(75, 23)
        Me.buttonTrackFont.TabIndex = 3
        Me.buttonTrackFont.Text = "Track Font"
        Me.buttonTrackFont.UseVisualStyleBackColor = True
        '
        'buttonResetWinPos
        '
        Me.buttonResetWinPos.Location = New System.Drawing.Point(250, 10)
        Me.buttonResetWinPos.Name = "buttonResetWinPos"
        Me.buttonResetWinPos.Size = New System.Drawing.Size(43, 33)
        Me.buttonResetWinPos.TabIndex = 26
        Me.buttonResetWinPos.Text = "Reset"
        Me.buttonResetWinPos.UseVisualStyleBackColor = True
        '
        'checkDarkTheme
        '
        Me.checkDarkTheme.AutoSize = True
        Me.checkDarkTheme.Location = New System.Drawing.Point(5, 79)
        Me.checkDarkTheme.Name = "checkDarkTheme"
        Me.checkDarkTheme.Size = New System.Drawing.Size(101, 17)
        Me.checkDarkTheme.TabIndex = 1
        Me.checkDarkTheme.Text = "Use dark theme"
        Me.checkDarkTheme.UseVisualStyleBackColor = True
        '
        'buttonLyricsFont
        '
        Me.buttonLyricsFont.Location = New System.Drawing.Point(199, 52)
        Me.buttonLyricsFont.Name = "buttonLyricsFont"
        Me.buttonLyricsFont.Size = New System.Drawing.Size(75, 23)
        Me.buttonLyricsFont.TabIndex = 2
        Me.buttonLyricsFont.Text = "Lyrics Font"
        Me.buttonLyricsFont.UseVisualStyleBackColor = True
        '
        'checkSavePos
        '
        Me.checkSavePos.AutoSize = True
        Me.checkSavePos.Location = New System.Drawing.Point(5, 17)
        Me.checkSavePos.Name = "checkSavePos"
        Me.checkSavePos.Size = New System.Drawing.Size(129, 17)
        Me.checkSavePos.TabIndex = 0
        Me.checkSavePos.Text = "Save window settings"
        Me.checkSavePos.UseVisualStyleBackColor = True
        '
        'labelWinSize
        '
        Me.labelWinSize.AutoSize = True
        Me.labelWinSize.Location = New System.Drawing.Point(183, 27)
        Me.labelWinSize.Name = "labelWinSize"
        Me.labelWinSize.Size = New System.Drawing.Size(31, 13)
        Me.labelWinSize.TabIndex = 25
        Me.labelWinSize.Text = "(0, 0)"
        '
        'labelWinSizeString
        '
        Me.labelWinSizeString.AutoSize = True
        Me.labelWinSizeString.Location = New System.Drawing.Point(156, 27)
        Me.labelWinSizeString.Name = "labelWinSizeString"
        Me.labelWinSizeString.Size = New System.Drawing.Size(30, 13)
        Me.labelWinSizeString.TabIndex = 22
        Me.labelWinSizeString.Text = "Size:"
        '
        'labelWinPos
        '
        Me.labelWinPos.AutoSize = True
        Me.labelWinPos.Location = New System.Drawing.Point(183, 9)
        Me.labelWinPos.Name = "labelWinPos"
        Me.labelWinPos.Size = New System.Drawing.Size(31, 13)
        Me.labelWinPos.TabIndex = 24
        Me.labelWinPos.Text = "(0, 0)"
        '
        'groupMediaPlayer
        '
        Me.groupMediaPlayer.Controls.Add(Me.checkRemoveTrackFromList)
        Me.groupMediaPlayer.Controls.Add(Me.checkPlaylistHistory)
        Me.groupMediaPlayer.Controls.Add(Me.checkRandomNextTrack)
        Me.groupMediaPlayer.Controls.Add(Me.labelBalanceR)
        Me.groupMediaPlayer.Controls.Add(Me.labelBalanceL)
        Me.groupMediaPlayer.Controls.Add(Me.labelPlayRate)
        Me.groupMediaPlayer.Controls.Add(Me.labelBalance)
        Me.groupMediaPlayer.Controls.Add(Me.buttonProperties)
        Me.groupMediaPlayer.Controls.Add(Me.trackbarPlayRate)
        Me.groupMediaPlayer.Controls.Add(Me.trackbarBalance)
        Me.groupMediaPlayer.Location = New System.Drawing.Point(3, 117)
        Me.groupMediaPlayer.Name = "groupMediaPlayer"
        Me.groupMediaPlayer.Size = New System.Drawing.Size(296, 135)
        Me.groupMediaPlayer.TabIndex = 20
        Me.groupMediaPlayer.TabStop = False
        Me.groupMediaPlayer.Text = "Media Player"
        '
        'checkRemoveTrackFromList
        '
        Me.checkRemoveTrackFromList.AutoSize = True
        Me.checkRemoveTrackFromList.Location = New System.Drawing.Point(7, 31)
        Me.checkRemoveTrackFromList.Name = "checkRemoveTrackFromList"
        Me.checkRemoveTrackFromList.Size = New System.Drawing.Size(199, 17)
        Me.checkRemoveTrackFromList.TabIndex = 30
        Me.checkRemoveTrackFromList.Text = "Remove track from list when queued"
        Me.checkRemoveTrackFromList.UseVisualStyleBackColor = True
        '
        'checkPlaylistHistory
        '
        Me.checkPlaylistHistory.AutoSize = True
        Me.checkPlaylistHistory.Location = New System.Drawing.Point(138, 15)
        Me.checkPlaylistHistory.Name = "checkPlaylistHistory"
        Me.checkPlaylistHistory.Size = New System.Drawing.Size(118, 17)
        Me.checkPlaylistHistory.TabIndex = 27
        Me.checkPlaylistHistory.Text = "Save playlist history"
        Me.checkPlaylistHistory.UseVisualStyleBackColor = True
        '
        'checkRandomNextTrack
        '
        Me.checkRandomNextTrack.AutoSize = True
        Me.checkRandomNextTrack.Location = New System.Drawing.Point(7, 15)
        Me.checkRandomNextTrack.Name = "checkRandomNextTrack"
        Me.checkRandomNextTrack.Size = New System.Drawing.Size(129, 17)
        Me.checkRandomNextTrack.TabIndex = 27
        Me.checkRandomNextTrack.Text = "Randomize next track"
        Me.checkRandomNextTrack.UseVisualStyleBackColor = True
        '
        'labelBalanceR
        '
        Me.labelBalanceR.AutoSize = True
        Me.labelBalanceR.Location = New System.Drawing.Point(133, 77)
        Me.labelBalanceR.Name = "labelBalanceR"
        Me.labelBalanceR.Size = New System.Drawing.Size(15, 13)
        Me.labelBalanceR.TabIndex = 29
        Me.labelBalanceR.Text = "R"
        '
        'labelBalanceL
        '
        Me.labelBalanceL.AutoSize = True
        Me.labelBalanceL.Location = New System.Drawing.Point(1, 62)
        Me.labelBalanceL.Name = "labelBalanceL"
        Me.labelBalanceL.Size = New System.Drawing.Size(13, 13)
        Me.labelBalanceL.TabIndex = 27
        Me.labelBalanceL.Text = "L"
        '
        'labelPlayRate
        '
        Me.labelPlayRate.AutoSize = True
        Me.labelPlayRate.Location = New System.Drawing.Point(168, 51)
        Me.labelPlayRate.Name = "labelPlayRate"
        Me.labelPlayRate.Size = New System.Drawing.Size(56, 13)
        Me.labelPlayRate.TabIndex = 28
        Me.labelPlayRate.Text = "Play Rate:"
        '
        'labelBalance
        '
        Me.labelBalance.AutoSize = True
        Me.labelBalance.Location = New System.Drawing.Point(18, 51)
        Me.labelBalance.Name = "labelBalance"
        Me.labelBalance.Size = New System.Drawing.Size(49, 13)
        Me.labelBalance.TabIndex = 27
        Me.labelBalance.Text = "Balance:"
        '
        'buttonProperties
        '
        Me.buttonProperties.Location = New System.Drawing.Point(265, 11)
        Me.buttonProperties.Name = "buttonProperties"
        Me.buttonProperties.Size = New System.Drawing.Size(28, 23)
        Me.buttonProperties.TabIndex = 21
        Me.buttonProperties.Text = "?"
        Me.buttonProperties.UseVisualStyleBackColor = True
        '
        'trackbarPlayRate
        '
        Me.trackbarPlayRate.AutoSize = False
        Me.trackbarPlayRate.LargeChange = 1
        Me.trackbarPlayRate.Location = New System.Drawing.Point(161, 74)
        Me.trackbarPlayRate.Maximum = 9
        Me.trackbarPlayRate.Name = "trackbarPlayRate"
        Me.trackbarPlayRate.Size = New System.Drawing.Size(119, 30)
        Me.trackbarPlayRate.TabIndex = 23
        Me.trackbarPlayRate.Value = 2
        '
        'trackbarBalance
        '
        Me.trackbarBalance.AutoSize = False
        Me.trackbarBalance.LargeChange = 10
        Me.trackbarBalance.Location = New System.Drawing.Point(14, 64)
        Me.trackbarBalance.Maximum = 100
        Me.trackbarBalance.Minimum = -100
        Me.trackbarBalance.Name = "trackbarBalance"
        Me.trackbarBalance.Size = New System.Drawing.Size(119, 45)
        Me.trackbarBalance.TabIndex = 22
        Me.trackbarBalance.TickFrequency = 20
        Me.trackbarBalance.TickStyle = System.Windows.Forms.TickStyle.Both
        '
        'g7
        '
        Me.g7.Controls.Add(Me.gadgetFormButton)
        Me.g7.Location = New System.Drawing.Point(431, 291)
        Me.g7.Name = "g7"
        Me.g7.Size = New System.Drawing.Size(302, 256)
        Me.g7.TabIndex = 10
        Me.g7.TabStop = False
        Me.g7.Text = "Gadgets"
        '
        'gadgetFormButton
        '
        Me.gadgetFormButton.Location = New System.Drawing.Point(86, 46)
        Me.gadgetFormButton.Name = "gadgetFormButton"
        Me.gadgetFormButton.Size = New System.Drawing.Size(121, 44)
        Me.gadgetFormButton.TabIndex = 5
        Me.gadgetFormButton.TabStop = False
        Me.gadgetFormButton.Text = "Open Gadget Window"
        Me.gadgetFormButton.UseVisualStyleBackColor = True
        '
        'OptionsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1664, 850)
        Me.Controls.Add(Me.g7)
        Me.Controls.Add(Me.g1)
        Me.Controls.Add(Me.g6)
        Me.Controls.Add(Me.labelMenu)
        Me.Controls.Add(Me.listMenu)
        Me.Controls.Add(Me.g2)
        Me.Controls.Add(Me.g9)
        Me.Controls.Add(Me.g8)
        Me.Controls.Add(Me.g5)
        Me.Controls.Add(Me.g3)
        Me.Controls.Add(Me.g4)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "OptionsForm"
        Me.TopMost = True
        Me.groupCommand.ResumeLayout(False)
        Me.groupModifierKey.ResumeLayout(False)
        Me.groupModifierKey.PerformLayout()
        Me.groupMainKey.ResumeLayout(False)
        Me.groupCurrentSet.ResumeLayout(False)
        Me.g3.ResumeLayout(False)
        Me.groupTimerDelay.ResumeLayout(False)
        CType(Me.numDelay, System.ComponentModel.ISupportInitialize).EndInit()
        Me.g5.ResumeLayout(False)
        Me.groupAssociations.ResumeLayout(False)
        Me.groupCurrentGenres.ResumeLayout(False)
        Me.g4.ResumeLayout(False)
        Me.g4.PerformLayout()
        Me.groupStations.ResumeLayout(False)
        Me.g8.ResumeLayout(False)
        Me.groupTCPListener.ResumeLayout(False)
        Me.groupTCPListener.PerformLayout()
        Me.groupTCPConnection.ResumeLayout(False)
        Me.groupTCPConnection.PerformLayout()
        Me.g9.ResumeLayout(False)
        Me.g9.PerformLayout()
        Me.groupVersion.ResumeLayout(False)
        Me.groupVersion.PerformLayout()
        Me.g2.ResumeLayout(False)
        Me.g2.PerformLayout()
        CType(Me.logPathReloadPic, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.logPathKeyPic, System.ComponentModel.ISupportInitialize).EndInit()
        Me.g6.ResumeLayout(False)
        Me.g6.PerformLayout()
        Me.groupViewMode.ResumeLayout(False)
        Me.groupViewMode.PerformLayout()
        Me.groupPlaylists.ResumeLayout(False)
        Me.groupPlaylists.PerformLayout()
        Me.g1.ResumeLayout(False)
        Me.groupUI.ResumeLayout(False)
        Me.groupUI.PerformLayout()
        Me.groupMediaPlayer.ResumeLayout(False)
        Me.groupMediaPlayer.PerformLayout()
        CType(Me.trackbarPlayRate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trackbarBalance, System.ComponentModel.ISupportInitialize).EndInit()
        Me.g7.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents commandBox As System.Windows.Forms.ComboBox
    Friend WithEvents r1 As System.Windows.Forms.RadioButton
    Friend WithEvents r2 As System.Windows.Forms.RadioButton
    Friend WithEvents r3 As System.Windows.Forms.RadioButton
    Friend WithEvents keyButton As System.Windows.Forms.Button
    Friend WithEvents groupCommand As System.Windows.Forms.GroupBox
    Friend WithEvents groupModifierKey As System.Windows.Forms.GroupBox
    Friend WithEvents r0 As System.Windows.Forms.RadioButton
    Friend WithEvents groupMainKey As System.Windows.Forms.GroupBox
    Friend WithEvents groupCurrentSet As System.Windows.Forms.GroupBox
    Friend WithEvents remButton As System.Windows.Forms.Button
    Friend WithEvents listSet As System.Windows.Forms.ListBox
    Friend WithEvents defaultButton As System.Windows.Forms.Button
    Friend WithEvents g3 As System.Windows.Forms.GroupBox
    Friend WithEvents g5 As System.Windows.Forms.GroupBox
    Friend WithEvents groupCurrentGenres As System.Windows.Forms.GroupBox
    Friend WithEvents NewGenreButton As System.Windows.Forms.Button
    Friend WithEvents listGenres As System.Windows.Forms.ListBox
    Friend WithEvents remButton2 As System.Windows.Forms.Button
    Friend WithEvents g4 As System.Windows.Forms.GroupBox
    Friend WithEvents addButton As System.Windows.Forms.Button
    Friend WithEvents groupStations As System.Windows.Forms.GroupBox
    Friend WithEvents moveDownButton As System.Windows.Forms.Button
    Friend WithEvents listStations As System.Windows.Forms.ListBox
    Friend WithEvents moveUpButton As System.Windows.Forms.Button
    Friend WithEvents remButton4 As System.Windows.Forms.Button
    Friend WithEvents changeUrlButton As System.Windows.Forms.Button
    Friend WithEvents changeNameButton As System.Windows.Forms.Button
    Friend WithEvents labelSorting As System.Windows.Forms.Label
    Friend WithEvents sortingBox As System.Windows.Forms.ComboBox
    Friend WithEvents labelUrl As System.Windows.Forms.Label
    Friend WithEvents groupTimerDelay As System.Windows.Forms.GroupBox
    Friend WithEvents numDelay As System.Windows.Forms.NumericUpDown
    Friend WithEvents g8 As System.Windows.Forms.GroupBox
    Friend WithEvents groupTCPListener As System.Windows.Forms.GroupBox
    Friend WithEvents checkEnableStartup As System.Windows.Forms.CheckBox
    Friend WithEvents groupTCPConnection As System.Windows.Forms.GroupBox
    Friend WithEvents labelOwnIp As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents stopConnectionButton As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents stopButton As System.Windows.Forms.Button
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents labelStatus As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents resetButton2 As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tPort As System.Windows.Forms.TextBox
    Friend WithEvents labelPort As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents g9 As System.Windows.Forms.GroupBox
    Friend WithEvents labelCurrVersion As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents publishButton As System.Windows.Forms.Button
    Friend WithEvents DownloadLatestButton As System.Windows.Forms.Button
    Friend WithEvents checkAutoUpdate As System.Windows.Forms.CheckBox
    Friend WithEvents checkUpdatesButton As System.Windows.Forms.Button
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents tftpIp As System.Windows.Forms.TextBox
    Friend WithEvents tftpUser As System.Windows.Forms.TextBox
    Friend WithEvents tftpPw As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents labelftpCurrProg As System.Windows.Forms.Label
    Friend WithEvents labelftpTotalProg As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents pBar2 As System.Windows.Forms.ProgressBar
    Friend WithEvents pBar As System.Windows.Forms.ProgressBar
    Friend WithEvents labelPublishedVersion As System.Windows.Forms.Label
    Friend WithEvents g2 As System.Windows.Forms.GroupBox
    Friend WithEvents bStatsFile As System.Windows.Forms.Button
    Friend WithEvents tStatsFile As System.Windows.Forms.TextBox
    Friend WithEvents bFtpDir As System.Windows.Forms.Button
    Friend WithEvents tFtpDir As System.Windows.Forms.TextBox
    Friend WithEvents bLyricsDir As System.Windows.Forms.Button
    Friend WithEvents tLyricsDir As System.Windows.Forms.TextBox
    Friend WithEvents bDatesFile As System.Windows.Forms.Button
    Friend WithEvents tDatesFile As System.Windows.Forms.TextBox
    Friend WithEvents bMusicDir As System.Windows.Forms.Button
    Friend WithEvents tMusicDir As System.Windows.Forms.TextBox
    Friend WithEvents listMenu As System.Windows.Forms.ListBox
    Friend WithEvents labelMenu As System.Windows.Forms.Label
    Friend WithEvents labelExtIp As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents checkBlockExtIps As System.Windows.Forms.CheckBox
    Friend WithEvents remoteSendButton As System.Windows.Forms.Button
    Friend WithEvents publishRemButton As System.Windows.Forms.Button
    Friend WithEvents publishAddButton As System.Windows.Forms.Button
    Friend WithEvents listPublish As System.Windows.Forms.ListBox
    Friend WithEvents searchHomeIpButton As System.Windows.Forms.Button
    Friend WithEvents groupVersion As System.Windows.Forms.GroupBox
    Friend WithEvents checkBlockMessages As CheckBox
    Friend WithEvents bPlaylistFile As Button
    Friend WithEvents tPlaylistFile As TextBox
    Friend WithEvents Label20 As Label
    Friend WithEvents Label19 As Label
    Friend WithEvents Label18 As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents groupAssociations As GroupBox
    Friend WithEvents addFolderButton As Button
    Friend WithEvents addTrackButton As Button
    Friend WithEvents listAssociations As ListBox
    Friend WithEvents remGenreDepButton As Button
    Friend WithEvents clearGenreDepButton As Button
    Friend WithEvents g6 As GroupBox
    Friend WithEvents groupPlaylists As GroupBox
    Friend WithEvents listPlaylists As ListBox
    Friend WithEvents newPlaylistButton As Button
    Friend WithEvents deletePlaylistButton As Button
    Friend WithEvents managePlaylistButton As Button
    Friend WithEvents convertButton As Button
    Friend WithEvents checkHiddenPlaylist As CheckBox
    Friend WithEvents checkIgnoreErrors As CheckBox
    Friend WithEvents Label22 As Label
    Friend WithEvents radioHidden As RadioButton
    Friend WithEvents radioAll As RadioButton
    Friend WithEvents groupViewMode As GroupBox
    Friend WithEvents radioFolders As RadioButton
    Friend WithEvents radioPlaylists As RadioButton
    Friend WithEvents g1 As GroupBox
    Friend WithEvents checkDarkTheme As CheckBox
    Friend WithEvents checkSavePos As CheckBox
    Friend WithEvents buttonLyricsFont As Button
    Friend WithEvents FontDlg As FontDialog
    Friend WithEvents groupMediaPlayer As GroupBox
    Friend WithEvents trackbarPlayRate As TrackBar
    Friend WithEvents trackbarBalance As TrackBar
    Friend WithEvents buttonProperties As Button
    Friend WithEvents labelWinSizeString As Label
    Friend WithEvents labelWinPosString As Label
    Friend WithEvents labelWinSize As Label
    Friend WithEvents labelWinPos As Label
    Friend WithEvents buttonResetWinPos As Button
    Friend WithEvents groupUI As GroupBox
    Friend WithEvents buttonFolderFont As Button
    Friend WithEvents buttonTrackFont As Button
    Friend WithEvents labelBalance As Label
    Friend WithEvents labelPlayRate As Label
    Friend WithEvents labelBalanceR As Label
    Friend WithEvents labelBalanceL As Label
    Friend WithEvents publishPathButton As Button
    Friend WithEvents checkRandomNextTrack As CheckBox
    Friend WithEvents checkPlaylistHistory As CheckBox
    Friend WithEvents listPairedIps As ListBox
    Friend WithEvents stopAllConnectionsButton As Button
    Friend WithEvents g7 As GroupBox
    Friend WithEvents gadgetFormButton As Button
    Friend WithEvents checkRemoveTrackFromList As CheckBox
    Friend WithEvents logPathReloadPic As PictureBox
    Friend WithEvents logPathKeyPic As PictureBox
End Class
