<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.alltime = New System.Windows.Forms.Timer(Me.components)
        Me.con1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.con1QueueAllTracks = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ManageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.con1Sep = New System.Windows.Forms.ToolStripSeparator()
        Me.con1RefreshList = New System.Windows.Forms.ToolStripMenuItem()
        Me.con1NewPlaylist = New System.Windows.Forms.ToolStripMenuItem()
        Me.l2 = New System.Windows.Forms.ListBox()
        Me.con2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.con2TrackTasks = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoveFromPlaylistToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.con2TrackTasksEditTrackParts = New System.Windows.Forms.ToolStripMenuItem()
        Me.con2TrackTasksCopyName = New System.Windows.Forms.ToolStripMenuItem()
        Me.con2TrackTasksShowLocations = New System.Windows.Forms.ToolStripMenuItem()
        Me.con2SourceTasks = New System.Windows.Forms.ToolStripMenuItem()
        Me.con2SourceTasksSetDate = New System.Windows.Forms.ToolStripMenuItem()
        Me.con2SourceTasksDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.con2SourceTasksRename = New System.Windows.Forms.ToolStripMenuItem()
        Me.con2SourceTasksReplace = New System.Windows.Forms.ToolStripMenuItem()
        Me.con2Sep = New System.Windows.Forms.ToolStripSeparator()
        Me.con2ListTasks = New System.Windows.Forms.ToolStripMenuItem()
        Me.con2ListTasksQueueInOrder = New System.Windows.Forms.ToolStripMenuItem()
        Me.con2TracksTasksRemove = New System.Windows.Forms.ToolStripMenuItem()
        Me.con2ListTasksClear = New System.Windows.Forms.ToolStripMenuItem()
        Me.con2ListTasksRefill = New System.Windows.Forms.ToolStripMenuItem()
        Me.con2ListTasksGenreDistributionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.con2Sep2 = New System.Windows.Forms.ToolStripSeparator()
        Me.con2AddToQueue = New System.Windows.Forms.ToolStripMenuItem()
        Me.con2AddToPlaylist = New System.Windows.Forms.ToolStripMenuItem()
        Me.con2AddToPlaylistNew = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExistingPlaylistToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.keyt = New System.Windows.Forms.Timer(Me.components)
        Me.keydelayt = New System.Windows.Forms.Timer(Me.components)
        Me.labelL2Count = New System.Windows.Forms.Label()
        Me.l2_2 = New System.Windows.Forms.ListBox()
        Me.menuStrip = New System.Windows.Forms.MenuStrip()
        Me.menuLock = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuRemote = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuGadgets = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuSource = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuSourceLocalRadio = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuSourceExternalMedia = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuStatistics = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuStatisticsTracks = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuStatisticsFolders = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuStatisticsRadio = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuLyrics = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuSortBy = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuSortByName = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuSortByDateAdded = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuSortByTimeListened = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuSortByCount = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuSortByLength = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuSortByPopularity = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.menuSortByReverse = New System.Windows.Forms.ToolStripMenuItem()
        Me.labelCount = New System.Windows.Forms.Label()
        Me.labelCount2 = New System.Windows.Forms.Label()
        Me.radiotimer = New System.Windows.Forms.Timer(Me.components)
        Me.tSearch = New System.Windows.Forms.TextBox()
        Me.labelPartName = New System.Windows.Forms.Label()
        Me.labelVolume = New System.Windows.Forms.Label()
        Me.clickerTimer = New System.Windows.Forms.Timer(Me.components)
        Me.labelTimeListened2 = New System.Windows.Forms.Label()
        Me.labelTimeListened = New System.Windows.Forms.Label()
        Me.tt = New System.Windows.Forms.ToolTip(Me.components)
        Me.picRepeat = New System.Windows.Forms.PictureBox()
        Me.picRandom = New System.Windows.Forms.PictureBox()
        Me.labelL2_2Count = New System.Windows.Forms.Label()
        Me.labelLength2 = New System.Windows.Forms.Label()
        Me.labelLength = New System.Windows.Forms.Label()
        Me.labelNextTrack = New System.Windows.Forms.Label()
        Me.labelPrevTrack = New System.Windows.Forms.Label()
        Me.labelPartsCount = New System.Windows.Forms.Label()
        Me.labelPartsCount2 = New System.Windows.Forms.Label()
        Me.clickcountt = New System.Windows.Forms.Timer(Me.components)
        Me.iniValT = New System.Windows.Forms.Timer(Me.components)
        Me.tv = New System.Windows.Forms.TreeView()
        Me.labelDateAdded = New System.Windows.Forms.Label()
        Me.labelDateAdded2 = New System.Windows.Forms.Label()
        Me.labelGenre2 = New System.Windows.Forms.Label()
        Me.labelGenre = New System.Windows.Forms.Label()
        Me.labelPopularity = New System.Windows.Forms.Label()
        Me.labelPopularity2 = New System.Windows.Forms.Label()
        Me.labelLoop = New System.Windows.Forms.Label()
        Me.dragDelayT = New System.Windows.Forms.Timer(Me.components)
        Me.fsw = New System.IO.FileSystemWatcher()
        Me.fswSleep = New System.Windows.Forms.Timer(Me.components)
        Me.checkSearchParts = New System.Windows.Forms.CheckBox()
        Me.checkSeachAllFolders = New System.Windows.Forms.CheckBox()
        Me.cancelLabel = New System.Windows.Forms.Label()
        Me.picCancel = New System.Windows.Forms.PictureBox()
        Me.keyloggerTimer = New System.Windows.Forms.Timer(Me.components)
        Me.iconTray = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.wmp = New AxWMPLib.AxWindowsMediaPlayer()
        Me.con1.SuspendLayout()
        Me.con2.SuspendLayout()
        Me.menuStrip.SuspendLayout()
        CType(Me.picRepeat, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picRandom, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.fsw, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picCancel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.wmp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'alltime
        '
        Me.alltime.Interval = 1
        '
        'con1
        '
        Me.con1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.con1QueueAllTracks, Me.ToolStripSeparator1, Me.ManageToolStripMenuItem, Me.con1Sep, Me.con1RefreshList, Me.con1NewPlaylist})
        Me.con1.Name = "con1"
        Me.con1.ShowImageMargin = False
        Me.con1.Size = New System.Drawing.Size(135, 104)
        '
        'con1QueueAllTracks
        '
        Me.con1QueueAllTracks.Name = "con1QueueAllTracks"
        Me.con1QueueAllTracks.Size = New System.Drawing.Size(134, 22)
        Me.con1QueueAllTracks.Text = "Queue all Tracks"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(131, 6)
        '
        'ManageToolStripMenuItem
        '
        Me.ManageToolStripMenuItem.Name = "ManageToolStripMenuItem"
        Me.ManageToolStripMenuItem.Size = New System.Drawing.Size(134, 22)
        Me.ManageToolStripMenuItem.Text = "Manage ..."
        '
        'con1Sep
        '
        Me.con1Sep.Name = "con1Sep"
        Me.con1Sep.Size = New System.Drawing.Size(131, 6)
        '
        'con1RefreshList
        '
        Me.con1RefreshList.Name = "con1RefreshList"
        Me.con1RefreshList.Overflow = System.Windows.Forms.ToolStripItemOverflow.AsNeeded
        Me.con1RefreshList.Size = New System.Drawing.Size(134, 22)
        Me.con1RefreshList.Text = "Refresh List"
        '
        'con1NewPlaylist
        '
        Me.con1NewPlaylist.Name = "con1NewPlaylist"
        Me.con1NewPlaylist.Size = New System.Drawing.Size(134, 22)
        Me.con1NewPlaylist.Text = "New Playlist"
        '
        'l2
        '
        Me.l2.AllowDrop = True
        Me.l2.CausesValidation = False
        Me.l2.ColumnWidth = 10
        Me.l2.ContextMenuStrip = Me.con2
        Me.l2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.l2.ItemHeight = 16
        Me.l2.Location = New System.Drawing.Point(232, 67)
        Me.l2.Name = "l2"
        Me.l2.Size = New System.Drawing.Size(380, 468)
        Me.l2.TabIndex = 4
        '
        'con2
        '
        Me.con2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.con2TrackTasks, Me.con2SourceTasks, Me.con2Sep, Me.con2ListTasks, Me.con2Sep2, Me.con2AddToQueue, Me.con2AddToPlaylist})
        Me.con2.Name = "con1"
        Me.con2.ShowImageMargin = False
        Me.con2.Size = New System.Drawing.Size(137, 126)
        '
        'con2TrackTasks
        '
        Me.con2TrackTasks.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RemoveFromPlaylistToolStripMenuItem, Me.con2TrackTasksEditTrackParts, Me.con2TrackTasksCopyName, Me.con2TrackTasksShowLocations})
        Me.con2TrackTasks.Name = "con2TrackTasks"
        Me.con2TrackTasks.Size = New System.Drawing.Size(136, 22)
        Me.con2TrackTasks.Text = "Track Tasks"
        '
        'RemoveFromPlaylistToolStripMenuItem
        '
        Me.RemoveFromPlaylistToolStripMenuItem.Name = "RemoveFromPlaylistToolStripMenuItem"
        Me.RemoveFromPlaylistToolStripMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.RemoveFromPlaylistToolStripMenuItem.Text = "Remove from Playlist"
        '
        'con2TrackTasksEditTrackParts
        '
        Me.con2TrackTasksEditTrackParts.Name = "con2TrackTasksEditTrackParts"
        Me.con2TrackTasksEditTrackParts.Size = New System.Drawing.Size(186, 22)
        Me.con2TrackTasksEditTrackParts.Text = "Edit Track Parts"
        '
        'con2TrackTasksCopyName
        '
        Me.con2TrackTasksCopyName.Name = "con2TrackTasksCopyName"
        Me.con2TrackTasksCopyName.Size = New System.Drawing.Size(186, 22)
        Me.con2TrackTasksCopyName.Text = "Copy Name"
        '
        'con2TrackTasksShowLocations
        '
        Me.con2TrackTasksShowLocations.Name = "con2TrackTasksShowLocations"
        Me.con2TrackTasksShowLocations.Size = New System.Drawing.Size(186, 22)
        Me.con2TrackTasksShowLocations.Text = "Show Locations"
        '
        'con2SourceTasks
        '
        Me.con2SourceTasks.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.con2SourceTasksSetDate, Me.con2SourceTasksDelete, Me.con2SourceTasksRename, Me.con2SourceTasksReplace})
        Me.con2SourceTasks.Name = "con2SourceTasks"
        Me.con2SourceTasks.Size = New System.Drawing.Size(136, 22)
        Me.con2SourceTasks.Text = "Source File Tasks"
        '
        'con2SourceTasksSetDate
        '
        Me.con2SourceTasksSetDate.Name = "con2SourceTasksSetDate"
        Me.con2SourceTasksSetDate.Size = New System.Drawing.Size(117, 22)
        Me.con2SourceTasksSetDate.Text = "Set Date"
        '
        'con2SourceTasksDelete
        '
        Me.con2SourceTasksDelete.Name = "con2SourceTasksDelete"
        Me.con2SourceTasksDelete.Size = New System.Drawing.Size(117, 22)
        Me.con2SourceTasksDelete.Text = "Delete"
        '
        'con2SourceTasksRename
        '
        Me.con2SourceTasksRename.Name = "con2SourceTasksRename"
        Me.con2SourceTasksRename.Size = New System.Drawing.Size(117, 22)
        Me.con2SourceTasksRename.Text = "Rename"
        '
        'con2SourceTasksReplace
        '
        Me.con2SourceTasksReplace.Name = "con2SourceTasksReplace"
        Me.con2SourceTasksReplace.Size = New System.Drawing.Size(117, 22)
        Me.con2SourceTasksReplace.Text = "Replace"
        '
        'con2Sep
        '
        Me.con2Sep.Name = "con2Sep"
        Me.con2Sep.Size = New System.Drawing.Size(133, 6)
        '
        'con2ListTasks
        '
        Me.con2ListTasks.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.con2ListTasksQueueInOrder, Me.con2TracksTasksRemove, Me.con2ListTasksClear, Me.con2ListTasksRefill, Me.con2ListTasksGenreDistributionToolStripMenuItem})
        Me.con2ListTasks.Name = "con2ListTasks"
        Me.con2ListTasks.Size = New System.Drawing.Size(136, 22)
        Me.con2ListTasks.Text = "List Tasks"
        '
        'con2ListTasksQueueInOrder
        '
        Me.con2ListTasksQueueInOrder.Name = "con2ListTasksQueueInOrder"
        Me.con2ListTasksQueueInOrder.Size = New System.Drawing.Size(191, 22)
        Me.con2ListTasksQueueInOrder.Text = "Queue Items in Order"
        '
        'con2TracksTasksRemove
        '
        Me.con2TracksTasksRemove.Name = "con2TracksTasksRemove"
        Me.con2TracksTasksRemove.Size = New System.Drawing.Size(191, 22)
        Me.con2TracksTasksRemove.Text = "Remove Selected Item"
        '
        'con2ListTasksClear
        '
        Me.con2ListTasksClear.Name = "con2ListTasksClear"
        Me.con2ListTasksClear.Size = New System.Drawing.Size(191, 22)
        Me.con2ListTasksClear.Text = "Clear List"
        '
        'con2ListTasksRefill
        '
        Me.con2ListTasksRefill.Name = "con2ListTasksRefill"
        Me.con2ListTasksRefill.Size = New System.Drawing.Size(191, 22)
        Me.con2ListTasksRefill.Text = "Refill List"
        '
        'con2ListTasksGenreDistributionToolStripMenuItem
        '
        Me.con2ListTasksGenreDistributionToolStripMenuItem.Name = "con2ListTasksGenreDistributionToolStripMenuItem"
        Me.con2ListTasksGenreDistributionToolStripMenuItem.Size = New System.Drawing.Size(191, 22)
        Me.con2ListTasksGenreDistributionToolStripMenuItem.Text = "Genre Distribution"
        '
        'con2Sep2
        '
        Me.con2Sep2.Name = "con2Sep2"
        Me.con2Sep2.Size = New System.Drawing.Size(133, 6)
        '
        'con2AddToQueue
        '
        Me.con2AddToQueue.Name = "con2AddToQueue"
        Me.con2AddToQueue.Size = New System.Drawing.Size(136, 22)
        Me.con2AddToQueue.Text = "Add To Queue"
        '
        'con2AddToPlaylist
        '
        Me.con2AddToPlaylist.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.con2AddToPlaylistNew, Me.ExistingPlaylistToolStripMenuItem})
        Me.con2AddToPlaylist.Name = "con2AddToPlaylist"
        Me.con2AddToPlaylist.Size = New System.Drawing.Size(136, 22)
        Me.con2AddToPlaylist.Text = "Add To Playlist"
        '
        'con2AddToPlaylistNew
        '
        Me.con2AddToPlaylistNew.Name = "con2AddToPlaylistNew"
        Me.con2AddToPlaylistNew.Size = New System.Drawing.Size(164, 22)
        Me.con2AddToPlaylistNew.Text = "New Playlist..."
        '
        'ExistingPlaylistToolStripMenuItem
        '
        Me.ExistingPlaylistToolStripMenuItem.Name = "ExistingPlaylistToolStripMenuItem"
        Me.ExistingPlaylistToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.ExistingPlaylistToolStripMenuItem.Text = "Existing Playlist..."
        '
        'keyt
        '
        Me.keyt.Interval = 30
        '
        'keydelayt
        '
        Me.keydelayt.Interval = 250
        '
        'labelL2Count
        '
        Me.labelL2Count.AutoSize = True
        Me.labelL2Count.BackColor = System.Drawing.Color.White
        Me.labelL2Count.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelL2Count.Location = New System.Drawing.Point(563, 517)
        Me.labelL2Count.Name = "labelL2Count"
        Me.labelL2Count.Size = New System.Drawing.Size(14, 16)
        Me.labelL2Count.TabIndex = 14
        Me.labelL2Count.Text = "0"
        '
        'l2_2
        '
        Me.l2_2.AllowDrop = True
        Me.l2_2.CausesValidation = False
        Me.l2_2.ContextMenuStrip = Me.con2
        Me.l2_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.l2_2.ItemHeight = 16
        Me.l2_2.Location = New System.Drawing.Point(617, 67)
        Me.l2_2.Name = "l2_2"
        Me.l2_2.Size = New System.Drawing.Size(270, 468)
        Me.l2_2.TabIndex = 15
        '
        'menuStrip
        '
        Me.menuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuLock, Me.menuRemote, Me.menuSettings, Me.menuGadgets, Me.MenuSource, Me.menuStatistics, Me.menuLyrics, Me.menuSortBy})
        Me.menuStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.menuStrip.Location = New System.Drawing.Point(0, 0)
        Me.menuStrip.Name = "menuStrip"
        Me.menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.menuStrip.Size = New System.Drawing.Size(892, 24)
        Me.menuStrip.TabIndex = 21
        '
        'menuLock
        '
        Me.menuLock.Image = Global.MediaPlayer.My.Resources.Resources.lock
        Me.menuLock.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.menuLock.Name = "menuLock"
        Me.menuLock.Size = New System.Drawing.Size(28, 20)
        Me.menuLock.ToolTipText = "Lock Hotkeys"
        '
        'menuRemote
        '
        Me.menuRemote.Image = Global.MediaPlayer.My.Resources.Resources.online
        Me.menuRemote.Name = "menuRemote"
        Me.menuRemote.Size = New System.Drawing.Size(28, 20)
        Me.menuRemote.ToolTipText = "Status: Undefined"
        '
        'menuSettings
        '
        Me.menuSettings.Image = Global.MediaPlayer.My.Resources.Resources.settings
        Me.menuSettings.Name = "menuSettings"
        Me.menuSettings.Size = New System.Drawing.Size(28, 20)
        Me.menuSettings.ToolTipText = "Settings"
        '
        'menuGadgets
        '
        Me.menuGadgets.Image = Global.MediaPlayer.My.Resources.Resources.gadgets
        Me.menuGadgets.Name = "menuGadgets"
        Me.menuGadgets.Size = New System.Drawing.Size(28, 20)
        Me.menuGadgets.ToolTipText = "Gadgets"
        '
        'MenuSource
        '
        Me.MenuSource.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuSourceLocalRadio, Me.MenuSourceExternalMedia})
        Me.MenuSource.Name = "MenuSource"
        Me.MenuSource.Size = New System.Drawing.Size(55, 20)
        Me.MenuSource.Text = "Source"
        '
        'MenuSourceLocalRadio
        '
        Me.MenuSourceLocalRadio.Name = "MenuSourceLocalRadio"
        Me.MenuSourceLocalRadio.Size = New System.Drawing.Size(152, 22)
        Me.MenuSourceLocalRadio.Text = "Radio"
        '
        'MenuSourceExternalMedia
        '
        Me.MenuSourceExternalMedia.Name = "MenuSourceExternalMedia"
        Me.MenuSourceExternalMedia.Size = New System.Drawing.Size(152, 22)
        Me.MenuSourceExternalMedia.Text = "External Media"
        '
        'menuStatistics
        '
        Me.menuStatistics.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuStatisticsTracks, Me.menuStatisticsFolders, Me.menuStatisticsRadio})
        Me.menuStatistics.Name = "menuStatistics"
        Me.menuStatistics.Size = New System.Drawing.Size(65, 20)
        Me.menuStatistics.Text = "Statistics"
        '
        'menuStatisticsTracks
        '
        Me.menuStatisticsTracks.Name = "menuStatisticsTracks"
        Me.menuStatisticsTracks.Size = New System.Drawing.Size(112, 22)
        Me.menuStatisticsTracks.Text = "Tracks"
        '
        'menuStatisticsFolders
        '
        Me.menuStatisticsFolders.Name = "menuStatisticsFolders"
        Me.menuStatisticsFolders.Size = New System.Drawing.Size(112, 22)
        Me.menuStatisticsFolders.Text = "Folders"
        '
        'menuStatisticsRadio
        '
        Me.menuStatisticsRadio.Name = "menuStatisticsRadio"
        Me.menuStatisticsRadio.Size = New System.Drawing.Size(112, 22)
        Me.menuStatisticsRadio.Text = "Radio"
        '
        'menuLyrics
        '
        Me.menuLyrics.Image = Global.MediaPlayer.My.Resources.Resources.tick
        Me.menuLyrics.Name = "menuLyrics"
        Me.menuLyrics.Size = New System.Drawing.Size(64, 20)
        Me.menuLyrics.Text = "Lyrics"
        '
        'menuSortBy
        '
        Me.menuSortBy.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuSortByName, Me.menuSortByDateAdded, Me.menuSortByTimeListened, Me.menuSortByCount, Me.menuSortByLength, Me.menuSortByPopularity, Me.ToolStripSeparator3, Me.menuSortByReverse})
        Me.menuSortBy.Name = "menuSortBy"
        Me.menuSortBy.Size = New System.Drawing.Size(56, 20)
        Me.menuSortBy.Text = "Sort by"
        '
        'menuSortByName
        '
        Me.menuSortByName.Name = "menuSortByName"
        Me.menuSortByName.Size = New System.Drawing.Size(155, 22)
        Me.menuSortByName.Text = "Name"
        '
        'menuSortByDateAdded
        '
        Me.menuSortByDateAdded.Name = "menuSortByDateAdded"
        Me.menuSortByDateAdded.Size = New System.Drawing.Size(155, 22)
        Me.menuSortByDateAdded.Text = "Date Added"
        '
        'menuSortByTimeListened
        '
        Me.menuSortByTimeListened.Name = "menuSortByTimeListened"
        Me.menuSortByTimeListened.Size = New System.Drawing.Size(155, 22)
        Me.menuSortByTimeListened.Text = "Time Listened"
        '
        'menuSortByCount
        '
        Me.menuSortByCount.Name = "menuSortByCount"
        Me.menuSortByCount.Size = New System.Drawing.Size(155, 22)
        Me.menuSortByCount.Text = "Count"
        '
        'menuSortByLength
        '
        Me.menuSortByLength.Name = "menuSortByLength"
        Me.menuSortByLength.Size = New System.Drawing.Size(155, 22)
        Me.menuSortByLength.Text = "Length"
        '
        'menuSortByPopularity
        '
        Me.menuSortByPopularity.Name = "menuSortByPopularity"
        Me.menuSortByPopularity.Size = New System.Drawing.Size(155, 22)
        Me.menuSortByPopularity.Text = "Popularity"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(152, 6)
        '
        'menuSortByReverse
        '
        Me.menuSortByReverse.Name = "menuSortByReverse"
        Me.menuSortByReverse.Size = New System.Drawing.Size(155, 22)
        Me.menuSortByReverse.Text = "Reverse Sorting"
        '
        'labelCount
        '
        Me.labelCount.AutoSize = True
        Me.labelCount.Cursor = System.Windows.Forms.Cursors.Hand
        Me.labelCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelCount.Location = New System.Drawing.Point(297, 48)
        Me.labelCount.Name = "labelCount"
        Me.labelCount.Size = New System.Drawing.Size(14, 16)
        Me.labelCount.TabIndex = 32
        Me.labelCount.Text = "0"
        '
        'labelCount2
        '
        Me.labelCount2.AutoSize = True
        Me.labelCount2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelCount2.Location = New System.Drawing.Point(259, 50)
        Me.labelCount2.Name = "labelCount2"
        Me.labelCount2.Size = New System.Drawing.Size(38, 13)
        Me.labelCount2.TabIndex = 31
        Me.labelCount2.Text = "Count:"
        '
        'radiotimer
        '
        Me.radiotimer.Interval = 3000
        '
        'tSearch
        '
        Me.tSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tSearch.Location = New System.Drawing.Point(3, 44)
        Me.tSearch.Name = "tSearch"
        Me.tSearch.Size = New System.Drawing.Size(127, 22)
        Me.tSearch.TabIndex = 35
        Me.tSearch.TabStop = False
        '
        'labelPartName
        '
        Me.labelPartName.AutoEllipsis = True
        Me.labelPartName.Cursor = System.Windows.Forms.Cursors.Default
        Me.labelPartName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelPartName.Location = New System.Drawing.Point(2, 27)
        Me.labelPartName.Name = "labelPartName"
        Me.labelPartName.Size = New System.Drawing.Size(132, 16)
        Me.labelPartName.TabIndex = 38
        Me.labelPartName.Text = "           Part name"
        Me.labelPartName.UseMnemonic = False
        '
        'labelVolume
        '
        Me.labelVolume.AutoSize = True
        Me.labelVolume.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelVolume.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.labelVolume.Location = New System.Drawing.Point(203, 522)
        Me.labelVolume.Name = "labelVolume"
        Me.labelVolume.Size = New System.Drawing.Size(23, 12)
        Me.labelVolume.TabIndex = 43
        Me.labelVolume.Text = "100"
        '
        'clickerTimer
        '
        Me.clickerTimer.Interval = 1
        '
        'labelTimeListened2
        '
        Me.labelTimeListened2.AutoSize = True
        Me.labelTimeListened2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelTimeListened2.Location = New System.Drawing.Point(458, 49)
        Me.labelTimeListened2.Name = "labelTimeListened2"
        Me.labelTimeListened2.Size = New System.Drawing.Size(33, 13)
        Me.labelTimeListened2.TabIndex = 44
        Me.labelTimeListened2.Text = "Time:"
        '
        'labelTimeListened
        '
        Me.labelTimeListened.AutoSize = True
        Me.labelTimeListened.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelTimeListened.Location = New System.Drawing.Point(492, 48)
        Me.labelTimeListened.Name = "labelTimeListened"
        Me.labelTimeListened.Size = New System.Drawing.Size(14, 16)
        Me.labelTimeListened.TabIndex = 45
        Me.labelTimeListened.Text = "0"
        '
        'picRepeat
        '
        Me.picRepeat.BackgroundImage = Global.MediaPlayer.My.Resources.Resources.rep2
        Me.picRepeat.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.picRepeat.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picRepeat.Location = New System.Drawing.Point(37, 506)
        Me.picRepeat.Name = "picRepeat"
        Me.picRepeat.Size = New System.Drawing.Size(41, 26)
        Me.picRepeat.TabIndex = 76
        Me.picRepeat.TabStop = False
        Me.tt.SetToolTip(Me.picRepeat, "Repeat Mode")
        '
        'picRandom
        '
        Me.picRandom.BackgroundImage = Global.MediaPlayer.My.Resources.Resources.shuffle
        Me.picRandom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.picRandom.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picRandom.Location = New System.Drawing.Point(78, 504)
        Me.picRandom.Name = "picRandom"
        Me.picRandom.Size = New System.Drawing.Size(30, 30)
        Me.picRandom.TabIndex = 75
        Me.picRandom.TabStop = False
        Me.tt.SetToolTip(Me.picRandom, "Shuffle Mode")
        '
        'labelL2_2Count
        '
        Me.labelL2_2Count.AutoSize = True
        Me.labelL2_2Count.BackColor = System.Drawing.Color.White
        Me.labelL2_2Count.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelL2_2Count.Location = New System.Drawing.Point(840, 517)
        Me.labelL2_2Count.Name = "labelL2_2Count"
        Me.labelL2_2Count.Size = New System.Drawing.Size(14, 16)
        Me.labelL2_2Count.TabIndex = 50
        Me.labelL2_2Count.Text = "0"
        '
        'labelLength2
        '
        Me.labelLength2.AutoSize = True
        Me.labelLength2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelLength2.Location = New System.Drawing.Point(678, 49)
        Me.labelLength2.Name = "labelLength2"
        Me.labelLength2.Size = New System.Drawing.Size(43, 13)
        Me.labelLength2.TabIndex = 51
        Me.labelLength2.Text = "Length:"
        '
        'labelLength
        '
        Me.labelLength.AutoSize = True
        Me.labelLength.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelLength.Location = New System.Drawing.Point(721, 48)
        Me.labelLength.Name = "labelLength"
        Me.labelLength.Size = New System.Drawing.Size(14, 16)
        Me.labelLength.TabIndex = 52
        Me.labelLength.Text = "0"
        '
        'labelNextTrack
        '
        Me.labelNextTrack.AutoSize = True
        Me.labelNextTrack.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.labelNextTrack.Cursor = System.Windows.Forms.Cursors.Hand
        Me.labelNextTrack.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelNextTrack.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.labelNextTrack.Location = New System.Drawing.Point(199, 491)
        Me.labelNextTrack.Name = "labelNextTrack"
        Me.labelNextTrack.Size = New System.Drawing.Size(23, 13)
        Me.labelNextTrack.TabIndex = 53
        Me.labelNextTrack.Text = "→ "
        '
        'labelPrevTrack
        '
        Me.labelPrevTrack.AutoSize = True
        Me.labelPrevTrack.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.labelPrevTrack.Cursor = System.Windows.Forms.Cursors.Hand
        Me.labelPrevTrack.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelPrevTrack.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.labelPrevTrack.Location = New System.Drawing.Point(10, 491)
        Me.labelPrevTrack.Name = "labelPrevTrack"
        Me.labelPrevTrack.Size = New System.Drawing.Size(19, 13)
        Me.labelPrevTrack.TabIndex = 54
        Me.labelPrevTrack.Text = "←"
        '
        'labelPartsCount
        '
        Me.labelPartsCount.AutoSize = True
        Me.labelPartsCount.Cursor = System.Windows.Forms.Cursors.Hand
        Me.labelPartsCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelPartsCount.Location = New System.Drawing.Point(184, 48)
        Me.labelPartsCount.Name = "labelPartsCount"
        Me.labelPartsCount.Size = New System.Drawing.Size(14, 16)
        Me.labelPartsCount.TabIndex = 55
        Me.labelPartsCount.Text = "0"
        '
        'labelPartsCount2
        '
        Me.labelPartsCount2.AutoSize = True
        Me.labelPartsCount2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelPartsCount2.Location = New System.Drawing.Point(150, 49)
        Me.labelPartsCount2.Name = "labelPartsCount2"
        Me.labelPartsCount2.Size = New System.Drawing.Size(34, 13)
        Me.labelPartsCount2.TabIndex = 56
        Me.labelPartsCount2.Text = "Parts:"
        '
        'clickcountt
        '
        Me.clickcountt.Interval = 60000
        '
        'iniValT
        '
        Me.iniValT.Interval = 1000
        '
        'tv
        '
        Me.tv.AllowDrop = True
        Me.tv.CausesValidation = False
        Me.tv.ContextMenuStrip = Me.con1
        Me.tv.FullRowSelect = True
        Me.tv.HideSelection = False
        Me.tv.Location = New System.Drawing.Point(3, 67)
        Me.tv.Name = "tv"
        Me.tv.ShowLines = False
        Me.tv.Size = New System.Drawing.Size(224, 379)
        Me.tv.TabIndex = 63
        Me.tv.TabStop = False
        '
        'labelDateAdded
        '
        Me.labelDateAdded.AutoSize = True
        Me.labelDateAdded.Cursor = System.Windows.Forms.Cursors.Hand
        Me.labelDateAdded.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelDateAdded.Location = New System.Drawing.Point(297, 27)
        Me.labelDateAdded.Name = "labelDateAdded"
        Me.labelDateAdded.Size = New System.Drawing.Size(14, 16)
        Me.labelDateAdded.TabIndex = 65
        Me.labelDateAdded.Text = "0"
        '
        'labelDateAdded2
        '
        Me.labelDateAdded2.AutoSize = True
        Me.labelDateAdded2.Cursor = System.Windows.Forms.Cursors.Hand
        Me.labelDateAdded2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelDateAdded2.Location = New System.Drawing.Point(256, 28)
        Me.labelDateAdded2.Name = "labelDateAdded2"
        Me.labelDateAdded2.Size = New System.Drawing.Size(41, 13)
        Me.labelDateAdded2.TabIndex = 64
        Me.labelDateAdded2.Text = "Added:"
        '
        'labelGenre2
        '
        Me.labelGenre2.AutoSize = True
        Me.labelGenre2.Cursor = System.Windows.Forms.Cursors.Hand
        Me.labelGenre2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelGenre2.Location = New System.Drawing.Point(682, 28)
        Me.labelGenre2.Name = "labelGenre2"
        Me.labelGenre2.Size = New System.Drawing.Size(39, 13)
        Me.labelGenre2.TabIndex = 66
        Me.labelGenre2.Text = "Genre:"
        '
        'labelGenre
        '
        Me.labelGenre.AutoSize = True
        Me.labelGenre.Cursor = System.Windows.Forms.Cursors.Hand
        Me.labelGenre.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelGenre.Location = New System.Drawing.Point(721, 27)
        Me.labelGenre.Name = "labelGenre"
        Me.labelGenre.Size = New System.Drawing.Size(14, 16)
        Me.labelGenre.TabIndex = 67
        Me.labelGenre.Text = "0"
        '
        'labelPopularity
        '
        Me.labelPopularity.AutoSize = True
        Me.labelPopularity.Cursor = System.Windows.Forms.Cursors.Hand
        Me.labelPopularity.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelPopularity.Location = New System.Drawing.Point(491, 27)
        Me.labelPopularity.Name = "labelPopularity"
        Me.labelPopularity.Size = New System.Drawing.Size(14, 16)
        Me.labelPopularity.TabIndex = 69
        Me.labelPopularity.Text = "0"
        '
        'labelPopularity2
        '
        Me.labelPopularity2.AutoSize = True
        Me.labelPopularity2.Cursor = System.Windows.Forms.Cursors.Hand
        Me.labelPopularity2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelPopularity2.Location = New System.Drawing.Point(435, 28)
        Me.labelPopularity2.Name = "labelPopularity2"
        Me.labelPopularity2.Size = New System.Drawing.Size(56, 13)
        Me.labelPopularity2.TabIndex = 68
        Me.labelPopularity2.Text = "Popularity:"
        '
        'labelLoop
        '
        Me.labelLoop.AutoSize = True
        Me.labelLoop.Cursor = System.Windows.Forms.Cursors.Hand
        Me.labelLoop.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelLoop.Location = New System.Drawing.Point(150, 27)
        Me.labelLoop.Name = "labelLoop"
        Me.labelLoop.Size = New System.Drawing.Size(65, 16)
        Me.labelLoop.TabIndex = 71
        Me.labelLoop.Text = "0:00 - 0:00"
        '
        'dragDelayT
        '
        '
        'fsw
        '
        Me.fsw.EnableRaisingEvents = True
        Me.fsw.IncludeSubdirectories = True
        Me.fsw.SynchronizingObject = Me
        '
        'fswSleep
        '
        Me.fswSleep.Interval = 5000
        '
        'checkSearchParts
        '
        Me.checkSearchParts.AutoSize = True
        Me.checkSearchParts.Location = New System.Drawing.Point(134, 27)
        Me.checkSearchParts.Name = "checkSearchParts"
        Me.checkSearchParts.Size = New System.Drawing.Size(15, 14)
        Me.checkSearchParts.TabIndex = 78
        Me.checkSearchParts.UseVisualStyleBackColor = True
        Me.checkSearchParts.Visible = False
        '
        'checkSeachAllFolders
        '
        Me.checkSeachAllFolders.AutoSize = True
        Me.checkSeachAllFolders.Location = New System.Drawing.Point(3, 26)
        Me.checkSeachAllFolders.Name = "checkSeachAllFolders"
        Me.checkSeachAllFolders.Size = New System.Drawing.Size(15, 14)
        Me.checkSeachAllFolders.TabIndex = 79
        Me.checkSeachAllFolders.UseVisualStyleBackColor = True
        Me.checkSeachAllFolders.Visible = False
        '
        'cancelLabel
        '
        Me.cancelLabel.AutoSize = True
        Me.cancelLabel.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cancelLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cancelLabel.Location = New System.Drawing.Point(151, 49)
        Me.cancelLabel.Name = "cancelLabel"
        Me.cancelLabel.Size = New System.Drawing.Size(0, 16)
        Me.cancelLabel.TabIndex = 81
        Me.cancelLabel.Visible = False
        '
        'picCancel
        '
        Me.picCancel.BackgroundImage = Global.MediaPlayer.My.Resources.Resources.cancelinv
        Me.picCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.picCancel.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picCancel.Location = New System.Drawing.Point(132, 47)
        Me.picCancel.Name = "picCancel"
        Me.picCancel.Size = New System.Drawing.Size(17, 17)
        Me.picCancel.TabIndex = 80
        Me.picCancel.TabStop = False
        Me.picCancel.Visible = False
        '
        'keyloggerTimer
        '
        Me.keyloggerTimer.Interval = 1
        '
        'iconTray
        '
        Me.iconTray.Icon = CType(resources.GetObject("iconTray.Icon"), System.Drawing.Icon)
        Me.iconTray.Text = "Media Player"
        Me.iconTray.Visible = True
        '
        'wmp
        '
        Me.wmp.Enabled = True
        Me.wmp.Location = New System.Drawing.Point(3, 447)
        Me.wmp.Name = "wmp"
        Me.wmp.OcxState = CType(resources.GetObject("wmp.OcxState"), System.Windows.Forms.AxHost.State)
        Me.wmp.Size = New System.Drawing.Size(224, 88)
        Me.wmp.TabIndex = 0
        Me.wmp.TabStop = False
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(892, 538)
        Me.Controls.Add(Me.cancelLabel)
        Me.Controls.Add(Me.picCancel)
        Me.Controls.Add(Me.checkSeachAllFolders)
        Me.Controls.Add(Me.checkSearchParts)
        Me.Controls.Add(Me.picRepeat)
        Me.Controls.Add(Me.picRandom)
        Me.Controls.Add(Me.labelLoop)
        Me.Controls.Add(Me.labelPopularity)
        Me.Controls.Add(Me.labelPopularity2)
        Me.Controls.Add(Me.labelGenre2)
        Me.Controls.Add(Me.labelGenre)
        Me.Controls.Add(Me.labelDateAdded)
        Me.Controls.Add(Me.labelDateAdded2)
        Me.Controls.Add(Me.tv)
        Me.Controls.Add(Me.labelPartsCount2)
        Me.Controls.Add(Me.labelPartsCount)
        Me.Controls.Add(Me.labelPrevTrack)
        Me.Controls.Add(Me.labelNextTrack)
        Me.Controls.Add(Me.labelL2_2Count)
        Me.Controls.Add(Me.labelLength2)
        Me.Controls.Add(Me.labelLength)
        Me.Controls.Add(Me.tSearch)
        Me.Controls.Add(Me.labelTimeListened)
        Me.Controls.Add(Me.labelTimeListened2)
        Me.Controls.Add(Me.labelVolume)
        Me.Controls.Add(Me.labelPartName)
        Me.Controls.Add(Me.labelCount2)
        Me.Controls.Add(Me.labelL2Count)
        Me.Controls.Add(Me.menuStrip)
        Me.Controls.Add(Me.labelCount)
        Me.Controls.Add(Me.wmp)
        Me.Controls.Add(Me.l2)
        Me.Controls.Add(Me.l2_2)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.menuStrip
        Me.MinimumSize = New System.Drawing.Size(908, 577)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.con1.ResumeLayout(False)
        Me.con2.ResumeLayout(False)
        Me.menuStrip.ResumeLayout(False)
        Me.menuStrip.PerformLayout()
        CType(Me.picRepeat, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picRandom, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.fsw, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picCancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.wmp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents wmp As AxWMPLib.AxWindowsMediaPlayer
    Friend WithEvents alltime As System.Windows.Forms.Timer
    Friend WithEvents l2 As System.Windows.Forms.ListBox
    Friend WithEvents keyt As System.Windows.Forms.Timer
    Friend WithEvents keydelayt As System.Windows.Forms.Timer
    Friend WithEvents labelL2Count As System.Windows.Forms.Label
    Friend WithEvents l2_2 As System.Windows.Forms.ListBox
    Friend WithEvents menuStrip As System.Windows.Forms.MenuStrip
    Friend WithEvents con2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents listTrackStats As System.Windows.Forms.ListView
    Friend WithEvents colTrack As System.Windows.Forms.ColumnHeader
    Friend WithEvents colCount2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents labelCount As System.Windows.Forms.Label
    Friend WithEvents labelCount2 As System.Windows.Forms.Label
    Friend WithEvents radiotimer As System.Windows.Forms.Timer
    Friend WithEvents con1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents con1RefreshList As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tSearch As System.Windows.Forms.TextBox
    Friend WithEvents listRadioStats As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents labelPartName As System.Windows.Forms.Label
    Friend WithEvents labelVolume As System.Windows.Forms.Label
    Friend WithEvents colTime2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents con2ListTasks As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents con2ListTasksRefill As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents con2SourceTasks As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents con2SourceTasksDelete As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents clickerTimer As System.Windows.Forms.Timer
    Friend WithEvents labelTimeListened2 As System.Windows.Forms.Label
    Friend WithEvents labelTimeListened As System.Windows.Forms.Label
    Friend WithEvents colPerDay As System.Windows.Forms.ColumnHeader
    Friend WithEvents colLength2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents colDate As System.Windows.Forms.ColumnHeader
    Friend WithEvents colParts As System.Windows.Forms.ColumnHeader
    Friend WithEvents MenuSource As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuStatistics As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuSourceExternalMedia As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuSettings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuSortBy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuSortByName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuSortByDateAdded As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuSortByTimeListened As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuSortByLength As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuSortByPopularity As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuSortByReverse As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tt As System.Windows.Forms.ToolTip
    Friend WithEvents labelL2_2Count As System.Windows.Forms.Label
    Friend WithEvents labelLength2 As System.Windows.Forms.Label
    Friend WithEvents labelLength As System.Windows.Forms.Label
    Friend WithEvents labelNextTrack As System.Windows.Forms.Label
    Friend WithEvents labelPrevTrack As System.Windows.Forms.Label
    Friend WithEvents con1NewPlaylist As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents con2TrackTasks As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents con2TrackTasksCopyName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents con2ListTasksClear As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents con2ListTasksQueueInOrder As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents labelPartsCount As System.Windows.Forms.Label
    Friend WithEvents labelPartsCount2 As System.Windows.Forms.Label
    Friend WithEvents clickcountt As System.Windows.Forms.Timer
    Friend WithEvents menuSortByCount As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents con2TrackTasksShowLocations As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents iniValT As System.Windows.Forms.Timer
    Friend WithEvents tv As System.Windows.Forms.TreeView
    Friend WithEvents menuLock As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuStatisticsRadio As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents labelDateAdded As System.Windows.Forms.Label
    Friend WithEvents labelDateAdded2 As System.Windows.Forms.Label
    Friend WithEvents labelGenre2 As System.Windows.Forms.Label
    Friend WithEvents labelGenre As System.Windows.Forms.Label
    Friend WithEvents labelPopularity As System.Windows.Forms.Label
    Friend WithEvents labelPopularity2 As System.Windows.Forms.Label
    Friend WithEvents MenuSourceLocalRadio As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents labelLoop As System.Windows.Forms.Label
    Friend WithEvents con2SourceTasksRename As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents colGenre As System.Windows.Forms.ColumnHeader
    Friend WithEvents menuStatisticsTracks As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuStatisticsFolders As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents listFolderStats As System.Windows.Forms.ListView
    Friend WithEvents colFolder As System.Windows.Forms.ColumnHeader
    Friend WithEvents colCount As System.Windows.Forms.ColumnHeader
    Friend WithEvents colTime As System.Windows.Forms.ColumnHeader
    Friend WithEvents colPop As System.Windows.Forms.ColumnHeader
    Friend WithEvents colLength As System.Windows.Forms.ColumnHeader
    Friend WithEvents colAge As System.Windows.Forms.ColumnHeader
    Friend WithEvents colTracks As System.Windows.Forms.ColumnHeader
    Friend WithEvents con2ListTasksGenreDistributionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents con2SourceTasksReplace As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents dragDelayT As System.Windows.Forms.Timer
    Friend WithEvents picRandom As System.Windows.Forms.PictureBox
    Friend WithEvents picRepeat As System.Windows.Forms.PictureBox
    Friend WithEvents menuRemote As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents con2SourceTasksSetDate As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Button2 As Button
    Friend WithEvents con2AddToQueue As ToolStripMenuItem
    Friend WithEvents con2AddToPlaylist As ToolStripMenuItem
    Friend WithEvents con2AddToPlaylistNew As ToolStripMenuItem
    Friend WithEvents con1Sep As ToolStripSeparator
    Friend WithEvents fsw As IO.FileSystemWatcher
    Friend WithEvents con1QueueAllTracks As ToolStripMenuItem
    Friend WithEvents con2Sep As ToolStripSeparator
    Friend WithEvents con2Sep2 As ToolStripSeparator
    Friend WithEvents fswSleep As Timer
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents checkSeachAllFolders As CheckBox
    Friend WithEvents checkSearchParts As CheckBox
    Friend WithEvents cancelLabel As Label
    Friend WithEvents picCancel As PictureBox
    Friend WithEvents con2TrackTasksEditTrackParts As ToolStripMenuItem
    Friend WithEvents ManageToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents menuLyrics As ToolStripMenuItem
    Friend WithEvents ExistingPlaylistToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents menuGadgets As ToolStripMenuItem
    Friend WithEvents keyloggerTimer As Timer
    Friend WithEvents RemoveFromPlaylistToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents con2TracksTasksRemove As ToolStripMenuItem
    Friend WithEvents iconTray As NotifyIcon
End Class
