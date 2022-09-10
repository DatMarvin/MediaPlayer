<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class StatsForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(StatsForm))
        Me.tvSelection = New System.Windows.Forms.TreeView()
        Me.listFolderStats = New System.Windows.Forms.ListView()
        Me.colFolder = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colTracks = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colCount = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colTime = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colPop = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colLength = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colAge = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.listTrackStats = New System.Windows.Forms.ListView()
        Me.colTrack = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colCount2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colTime2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colPerDay = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colLength2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colDate = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colParts = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colGenre = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.listRadioStats = New System.Windows.Forms.ListView()
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.avCheck = New System.Windows.Forms.CheckBox()
        Me.labelCount = New System.Windows.Forms.Label()
        Me.radTracks = New System.Windows.Forms.RadioButton()
        Me.radRadio = New System.Windows.Forms.RadioButton()
        Me.radFolders = New System.Windows.Forms.RadioButton()
        Me.groupMode = New System.Windows.Forms.GroupBox()
        Me.checkAll = New System.Windows.Forms.CheckBox()
        Me.buttonApply = New System.Windows.Forms.Button()
        Me.buttonTotal = New System.Windows.Forms.Button()
        Me.groupMode.SuspendLayout()
        Me.SuspendLayout()
        '
        'tvSelection
        '
        Me.tvSelection.FullRowSelect = True
        Me.tvSelection.HideSelection = False
        Me.tvSelection.Location = New System.Drawing.Point(5, 142)
        Me.tvSelection.Name = "tvSelection"
        Me.tvSelection.Size = New System.Drawing.Size(210, 311)
        Me.tvSelection.TabIndex = 65
        '
        'listFolderStats
        '
        Me.listFolderStats.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colFolder, Me.colTracks, Me.colCount, Me.colTime, Me.colPop, Me.colLength, Me.colAge})
        Me.listFolderStats.GridLines = True
        Me.listFolderStats.HideSelection = False
        Me.listFolderStats.Location = New System.Drawing.Point(217, 182)
        Me.listFolderStats.MultiSelect = False
        Me.listFolderStats.Name = "listFolderStats"
        Me.listFolderStats.Size = New System.Drawing.Size(660, 164)
        Me.listFolderStats.TabIndex = 74
        Me.listFolderStats.UseCompatibleStateImageBehavior = False
        Me.listFolderStats.View = System.Windows.Forms.View.Details
        Me.listFolderStats.Visible = False
        '
        'colFolder
        '
        Me.colFolder.Text = "Folder"
        Me.colFolder.Width = 187
        '
        'colTracks
        '
        Me.colTracks.Text = "Tracks"
        Me.colTracks.Width = 50
        '
        'colCount
        '
        Me.colCount.Text = "Count"
        '
        'colTime
        '
        Me.colTime.Text = "Time"
        Me.colTime.Width = 120
        '
        'colPop
        '
        Me.colPop.Text = "Popularity"
        Me.colPop.Width = 75
        '
        'colLength
        '
        Me.colLength.Text = "Length"
        Me.colLength.Width = 75
        '
        'colAge
        '
        Me.colAge.Text = "Age"
        Me.colAge.Width = 70
        '
        'listTrackStats
        '
        Me.listTrackStats.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colTrack, Me.colCount2, Me.colTime2, Me.colPerDay, Me.colLength2, Me.colDate, Me.colParts, Me.colGenre})
        Me.listTrackStats.GridLines = True
        Me.listTrackStats.HideSelection = False
        Me.listTrackStats.Location = New System.Drawing.Point(217, 17)
        Me.listTrackStats.MultiSelect = False
        Me.listTrackStats.Name = "listTrackStats"
        Me.listTrackStats.Size = New System.Drawing.Size(660, 164)
        Me.listTrackStats.TabIndex = 75
        Me.listTrackStats.UseCompatibleStateImageBehavior = False
        Me.listTrackStats.View = System.Windows.Forms.View.Details
        Me.listTrackStats.Visible = False
        '
        'colTrack
        '
        Me.colTrack.Text = "Track"
        Me.colTrack.Width = 178
        '
        'colCount2
        '
        Me.colCount2.Text = "Count"
        Me.colCount2.Width = 40
        '
        'colTime2
        '
        Me.colTime2.Text = "Time"
        Me.colTime2.Width = 90
        '
        'colPerDay
        '
        Me.colPerDay.Text = "Per Day"
        Me.colPerDay.Width = 75
        '
        'colLength2
        '
        Me.colLength2.Text = "Length"
        Me.colLength2.Width = 75
        '
        'colDate
        '
        Me.colDate.Text = "Date"
        Me.colDate.Width = 70
        '
        'colParts
        '
        Me.colParts.Text = "Parts"
        Me.colParts.Width = 30
        '
        'colGenre
        '
        Me.colGenre.Text = "Genre"
        Me.colGenre.Width = 88
        '
        'listRadioStats
        '
        Me.listRadioStats.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader2, Me.ColumnHeader3})
        Me.listRadioStats.GridLines = True
        Me.listRadioStats.HideSelection = False
        Me.listRadioStats.Location = New System.Drawing.Point(217, 352)
        Me.listRadioStats.MultiSelect = False
        Me.listRadioStats.Name = "listRadioStats"
        Me.listRadioStats.Size = New System.Drawing.Size(660, 101)
        Me.listRadioStats.TabIndex = 76
        Me.listRadioStats.UseCompatibleStateImageBehavior = False
        Me.listRadioStats.View = System.Windows.Forms.View.Details
        Me.listRadioStats.Visible = False
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Radio Station"
        Me.ColumnHeader2.Width = 225
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Time"
        Me.ColumnHeader3.Width = 150
        '
        'avCheck
        '
        Me.avCheck.AutoSize = True
        Me.avCheck.Location = New System.Drawing.Point(88, 74)
        Me.avCheck.Name = "avCheck"
        Me.avCheck.Size = New System.Drawing.Size(34, 17)
        Me.avCheck.TabIndex = 77
        Me.avCheck.Text = "Ø"
        Me.avCheck.UseVisualStyleBackColor = True
        Me.avCheck.Visible = False
        '
        'labelCount
        '
        Me.labelCount.AutoSize = True
        Me.labelCount.Location = New System.Drawing.Point(224, 1)
        Me.labelCount.Name = "labelCount"
        Me.labelCount.Size = New System.Drawing.Size(44, 13)
        Me.labelCount.TabIndex = 78
        Me.labelCount.Text = "Items: 0"
        '
        'radTracks
        '
        Me.radTracks.AutoSize = True
        Me.radTracks.Location = New System.Drawing.Point(7, 19)
        Me.radTracks.Name = "radTracks"
        Me.radTracks.Size = New System.Drawing.Size(58, 17)
        Me.radTracks.TabIndex = 79
        Me.radTracks.TabStop = True
        Me.radTracks.Text = "Tracks"
        Me.radTracks.UseVisualStyleBackColor = True
        '
        'radRadio
        '
        Me.radRadio.AutoSize = True
        Me.radRadio.Location = New System.Drawing.Point(77, 19)
        Me.radRadio.Name = "radRadio"
        Me.radRadio.Size = New System.Drawing.Size(53, 17)
        Me.radRadio.TabIndex = 80
        Me.radRadio.TabStop = True
        Me.radRadio.Text = "Radio"
        Me.radRadio.UseVisualStyleBackColor = True
        '
        'radFolders
        '
        Me.radFolders.AutoSize = True
        Me.radFolders.Location = New System.Drawing.Point(145, 19)
        Me.radFolders.Name = "radFolders"
        Me.radFolders.Size = New System.Drawing.Size(59, 17)
        Me.radFolders.TabIndex = 81
        Me.radFolders.TabStop = True
        Me.radFolders.Text = "Folders"
        Me.radFolders.UseVisualStyleBackColor = True
        '
        'groupMode
        '
        Me.groupMode.Controls.Add(Me.radTracks)
        Me.groupMode.Controls.Add(Me.radFolders)
        Me.groupMode.Controls.Add(Me.radRadio)
        Me.groupMode.Location = New System.Drawing.Point(5, 24)
        Me.groupMode.Name = "groupMode"
        Me.groupMode.Size = New System.Drawing.Size(211, 45)
        Me.groupMode.TabIndex = 82
        Me.groupMode.TabStop = False
        Me.groupMode.Text = "Stats Mode"
        '
        'checkAll
        '
        Me.checkAll.AutoSize = True
        Me.checkAll.Location = New System.Drawing.Point(12, 74)
        Me.checkAll.Name = "checkAll"
        Me.checkAll.Size = New System.Drawing.Size(70, 17)
        Me.checkAll.TabIndex = 83
        Me.checkAll.Text = "Select All"
        Me.checkAll.UseVisualStyleBackColor = True
        Me.checkAll.Visible = False
        '
        'buttonApply
        '
        Me.buttonApply.Location = New System.Drawing.Point(154, 70)
        Me.buttonApply.Name = "buttonApply"
        Me.buttonApply.Size = New System.Drawing.Size(56, 22)
        Me.buttonApply.TabIndex = 84
        Me.buttonApply.Text = "Update"
        Me.buttonApply.UseVisualStyleBackColor = True
        Me.buttonApply.Visible = False
        '
        'buttonTotal
        '
        Me.buttonTotal.Location = New System.Drawing.Point(154, 2)
        Me.buttonTotal.Name = "buttonTotal"
        Me.buttonTotal.Size = New System.Drawing.Size(56, 25)
        Me.buttonTotal.TabIndex = 85
        Me.buttonTotal.Text = "Total "
        Me.buttonTotal.UseVisualStyleBackColor = True
        '
        'StatsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(884, 461)
        Me.Controls.Add(Me.buttonTotal)
        Me.Controls.Add(Me.buttonApply)
        Me.Controls.Add(Me.checkAll)
        Me.Controls.Add(Me.groupMode)
        Me.Controls.Add(Me.labelCount)
        Me.Controls.Add(Me.avCheck)
        Me.Controls.Add(Me.listRadioStats)
        Me.Controls.Add(Me.listTrackStats)
        Me.Controls.Add(Me.listFolderStats)
        Me.Controls.Add(Me.tvSelection)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(900, 500)
        Me.Name = "StatsForm"
        Me.Text = "Statistics"
        Me.groupMode.ResumeLayout(False)
        Me.groupMode.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tvSelection As TreeView
    Friend WithEvents listFolderStats As ListView
    Friend WithEvents colFolder As ColumnHeader
    Friend WithEvents colTracks As ColumnHeader
    Friend WithEvents colCount As ColumnHeader
    Friend WithEvents colTime As ColumnHeader
    Friend WithEvents colPop As ColumnHeader
    Friend WithEvents colLength As ColumnHeader
    Friend WithEvents colAge As ColumnHeader
    Friend WithEvents listTrackStats As ListView
    Friend WithEvents colTrack As ColumnHeader
    Friend WithEvents colCount2 As ColumnHeader
    Friend WithEvents colTime2 As ColumnHeader
    Friend WithEvents colPerDay As ColumnHeader
    Friend WithEvents colLength2 As ColumnHeader
    Friend WithEvents colDate As ColumnHeader
    Friend WithEvents colParts As ColumnHeader
    Friend WithEvents colGenre As ColumnHeader
    Friend WithEvents listRadioStats As ListView
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents avCheck As CheckBox
    Friend WithEvents labelCount As Label
    Friend WithEvents radTracks As RadioButton
    Friend WithEvents radRadio As RadioButton
    Friend WithEvents radFolders As RadioButton
    Friend WithEvents groupMode As GroupBox
    Friend WithEvents checkAll As CheckBox
    Friend WithEvents buttonApply As Button
    Friend WithEvents buttonTotal As Button
End Class
