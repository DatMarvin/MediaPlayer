<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TrackSelectionForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TrackSelectionForm))
        Me.g_sub_tracks = New System.Windows.Forms.GroupBox()
        Me.sortRevButton = New System.Windows.Forms.Button()
        Me.sortCombo = New System.Windows.Forms.ComboBox()
        Me.sortLabel = New System.Windows.Forms.Label()
        Me.checkPlayOnClick = New System.Windows.Forms.CheckBox()
        Me.playButton = New System.Windows.Forms.Button()
        Me.labelItemCount = New System.Windows.Forms.Label()
        Me.checkSearchSource = New System.Windows.Forms.CheckBox()
        Me.picCancel = New System.Windows.Forms.PictureBox()
        Me.tSearch = New System.Windows.Forms.TextBox()
        Me.addExternalButton = New System.Windows.Forms.Button()
        Me.infoButton = New System.Windows.Forms.Button()
        Me.checkAll = New System.Windows.Forms.CheckBox()
        Me.removeButton = New System.Windows.Forms.Button()
        Me.changeSourceButton = New System.Windows.Forms.Button()
        Me.addButton = New System.Windows.Forms.Button()
        Me.listTrackSelection = New System.Windows.Forms.ListView()
        Me.colTrack = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colSource = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.okButton2 = New System.Windows.Forms.Button()
        Me.cancelButton2 = New System.Windows.Forms.Button()
        Me.colStat = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.g_sub_tracks.SuspendLayout()
        CType(Me.picCancel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'g_sub_tracks
        '
        Me.g_sub_tracks.Controls.Add(Me.sortRevButton)
        Me.g_sub_tracks.Controls.Add(Me.sortCombo)
        Me.g_sub_tracks.Controls.Add(Me.sortLabel)
        Me.g_sub_tracks.Controls.Add(Me.checkPlayOnClick)
        Me.g_sub_tracks.Controls.Add(Me.playButton)
        Me.g_sub_tracks.Controls.Add(Me.labelItemCount)
        Me.g_sub_tracks.Controls.Add(Me.checkSearchSource)
        Me.g_sub_tracks.Controls.Add(Me.picCancel)
        Me.g_sub_tracks.Controls.Add(Me.tSearch)
        Me.g_sub_tracks.Controls.Add(Me.addExternalButton)
        Me.g_sub_tracks.Controls.Add(Me.infoButton)
        Me.g_sub_tracks.Controls.Add(Me.checkAll)
        Me.g_sub_tracks.Controls.Add(Me.removeButton)
        Me.g_sub_tracks.Controls.Add(Me.changeSourceButton)
        Me.g_sub_tracks.Controls.Add(Me.addButton)
        Me.g_sub_tracks.Controls.Add(Me.listTrackSelection)
        Me.g_sub_tracks.Controls.Add(Me.okButton2)
        Me.g_sub_tracks.Controls.Add(Me.cancelButton2)
        Me.g_sub_tracks.Location = New System.Drawing.Point(2, 2)
        Me.g_sub_tracks.Name = "g_sub_tracks"
        Me.g_sub_tracks.Size = New System.Drawing.Size(546, 398)
        Me.g_sub_tracks.TabIndex = 67
        Me.g_sub_tracks.TabStop = False
        Me.g_sub_tracks.Text = "Track Selection"
        '
        'sortRevButton
        '
        Me.sortRevButton.Location = New System.Drawing.Point(479, 10)
        Me.sortRevButton.Name = "sortRevButton"
        Me.sortRevButton.Size = New System.Drawing.Size(23, 23)
        Me.sortRevButton.TabIndex = 92
        Me.sortRevButton.Text = "↑"
        Me.sortRevButton.UseVisualStyleBackColor = True
        '
        'sortCombo
        '
        Me.sortCombo.FormattingEnabled = True
        Me.sortCombo.Items.AddRange(New Object() {"Name", "Date Added", "Time Listened", "Count", "Length", "Popularity"})
        Me.sortCombo.Location = New System.Drawing.Point(387, 11)
        Me.sortCombo.Name = "sortCombo"
        Me.sortCombo.Size = New System.Drawing.Size(92, 21)
        Me.sortCombo.TabIndex = 91
        '
        'sortLabel
        '
        Me.sortLabel.AutoSize = True
        Me.sortLabel.Location = New System.Drawing.Point(342, 14)
        Me.sortLabel.Name = "sortLabel"
        Me.sortLabel.Size = New System.Drawing.Size(43, 13)
        Me.sortLabel.TabIndex = 90
        Me.sortLabel.Text = "Sort by:"
        '
        'checkPlayOnClick
        '
        Me.checkPlayOnClick.AutoSize = True
        Me.checkPlayOnClick.Location = New System.Drawing.Point(307, 371)
        Me.checkPlayOnClick.Name = "checkPlayOnClick"
        Me.checkPlayOnClick.Size = New System.Drawing.Size(66, 17)
        Me.checkPlayOnClick.TabIndex = 89
        Me.checkPlayOnClick.Text = "On Click"
        Me.checkPlayOnClick.UseVisualStyleBackColor = True
        '
        'playButton
        '
        Me.playButton.Location = New System.Drawing.Point(261, 364)
        Me.playButton.Name = "playButton"
        Me.playButton.Size = New System.Drawing.Size(42, 28)
        Me.playButton.TabIndex = 88
        Me.playButton.Text = "Play"
        Me.playButton.UseVisualStyleBackColor = True
        '
        'labelItemCount
        '
        Me.labelItemCount.AutoSize = True
        Me.labelItemCount.Location = New System.Drawing.Point(503, 15)
        Me.labelItemCount.Name = "labelItemCount"
        Me.labelItemCount.Size = New System.Drawing.Size(13, 13)
        Me.labelItemCount.TabIndex = 87
        Me.labelItemCount.Text = "0"
        '
        'checkSearchSource
        '
        Me.checkSearchSource.AutoSize = True
        Me.checkSearchSource.Location = New System.Drawing.Point(239, 14)
        Me.checkSearchSource.Name = "checkSearchSource"
        Me.checkSearchSource.Size = New System.Drawing.Size(97, 17)
        Me.checkSearchSource.TabIndex = 85
        Me.checkSearchSource.Text = "Search Source"
        Me.checkSearchSource.UseVisualStyleBackColor = True
        Me.checkSearchSource.Visible = False
        '
        'picCancel
        '
        Me.picCancel.BackgroundImage = Global.MediaPlayer.My.Resources.Resources.cancelinv
        Me.picCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.picCancel.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picCancel.Location = New System.Drawing.Point(217, 12)
        Me.picCancel.Name = "picCancel"
        Me.picCancel.Size = New System.Drawing.Size(18, 18)
        Me.picCancel.TabIndex = 84
        Me.picCancel.TabStop = False
        Me.picCancel.Visible = False
        '
        'tSearch
        '
        Me.tSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tSearch.Location = New System.Drawing.Point(90, 10)
        Me.tSearch.Name = "tSearch"
        Me.tSearch.Size = New System.Drawing.Size(127, 22)
        Me.tSearch.TabIndex = 83
        Me.tSearch.TabStop = False
        Me.tSearch.Text = "Search..."
        '
        'addExternalButton
        '
        Me.addExternalButton.Location = New System.Drawing.Point(5, 364)
        Me.addExternalButton.Name = "addExternalButton"
        Me.addExternalButton.Size = New System.Drawing.Size(111, 28)
        Me.addExternalButton.TabIndex = 82
        Me.addExternalButton.Text = "Add External Files"
        Me.addExternalButton.UseVisualStyleBackColor = True
        '
        'infoButton
        '
        Me.infoButton.Location = New System.Drawing.Point(261, 330)
        Me.infoButton.Name = "infoButton"
        Me.infoButton.Size = New System.Drawing.Size(111, 28)
        Me.infoButton.TabIndex = 81
        Me.infoButton.Text = "Info"
        Me.infoButton.UseVisualStyleBackColor = True
        '
        'checkAll
        '
        Me.checkAll.AutoSize = True
        Me.checkAll.Location = New System.Drawing.Point(12, 15)
        Me.checkAll.Name = "checkAll"
        Me.checkAll.Size = New System.Drawing.Size(70, 17)
        Me.checkAll.TabIndex = 80
        Me.checkAll.Text = "Select All"
        Me.checkAll.UseVisualStyleBackColor = True
        '
        'removeButton
        '
        Me.removeButton.Location = New System.Drawing.Point(133, 330)
        Me.removeButton.Name = "removeButton"
        Me.removeButton.Size = New System.Drawing.Size(111, 28)
        Me.removeButton.TabIndex = 79
        Me.removeButton.Text = "Remove Track"
        Me.removeButton.UseVisualStyleBackColor = True
        '
        'changeSourceButton
        '
        Me.changeSourceButton.Location = New System.Drawing.Point(133, 364)
        Me.changeSourceButton.Name = "changeSourceButton"
        Me.changeSourceButton.Size = New System.Drawing.Size(111, 28)
        Me.changeSourceButton.TabIndex = 77
        Me.changeSourceButton.Text = "Change Source"
        Me.changeSourceButton.UseVisualStyleBackColor = True
        '
        'addButton
        '
        Me.addButton.Location = New System.Drawing.Point(5, 330)
        Me.addButton.Name = "addButton"
        Me.addButton.Size = New System.Drawing.Size(111, 28)
        Me.addButton.TabIndex = 75
        Me.addButton.Text = "Add Tracks"
        Me.addButton.UseVisualStyleBackColor = True
        '
        'listTrackSelection
        '
        Me.listTrackSelection.CheckBoxes = True
        Me.listTrackSelection.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colTrack, Me.colStat, Me.colSource})
        Me.listTrackSelection.GridLines = True
        Me.listTrackSelection.HideSelection = False
        Me.listTrackSelection.Location = New System.Drawing.Point(5, 33)
        Me.listTrackSelection.MultiSelect = False
        Me.listTrackSelection.Name = "listTrackSelection"
        Me.listTrackSelection.Size = New System.Drawing.Size(532, 291)
        Me.listTrackSelection.TabIndex = 74
        Me.listTrackSelection.UseCompatibleStateImageBehavior = False
        Me.listTrackSelection.View = System.Windows.Forms.View.Details
        '
        'colTrack
        '
        Me.colTrack.Text = "Tracks"
        Me.colTrack.Width = 225
        '
        'colSource
        '
        Me.colSource.Text = "Source"
        Me.colSource.Width = 400
        '
        'okButton2
        '
        Me.okButton2.Location = New System.Drawing.Point(426, 330)
        Me.okButton2.Name = "okButton2"
        Me.okButton2.Size = New System.Drawing.Size(111, 28)
        Me.okButton2.TabIndex = 65
        Me.okButton2.Text = "Ok"
        Me.okButton2.UseVisualStyleBackColor = True
        '
        'cancelButton2
        '
        Me.cancelButton2.Location = New System.Drawing.Point(426, 364)
        Me.cancelButton2.Name = "cancelButton2"
        Me.cancelButton2.Size = New System.Drawing.Size(111, 28)
        Me.cancelButton2.TabIndex = 18
        Me.cancelButton2.Text = "Cancel"
        Me.cancelButton2.UseVisualStyleBackColor = True
        '
        'colStat
        '
        Me.colStat.Text = "Stat"
        Me.colStat.Width = 90
        '
        'TrackSelectionForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(551, 401)
        Me.Controls.Add(Me.g_sub_tracks)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "TrackSelectionForm"
        Me.g_sub_tracks.ResumeLayout(False)
        Me.g_sub_tracks.PerformLayout()
        CType(Me.picCancel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents g_sub_tracks As GroupBox
    Friend WithEvents listTrackSelection As ListView
    Friend WithEvents colTrack As ColumnHeader
    Friend WithEvents colSource As ColumnHeader
    Friend WithEvents okButton2 As Button
    Friend WithEvents cancelButton2 As Button
    Friend WithEvents addButton As Button
    Friend WithEvents changeSourceButton As Button
    Friend WithEvents removeButton As Button
    Friend WithEvents checkAll As CheckBox
    Friend WithEvents infoButton As Button
    Friend WithEvents addExternalButton As Button
    Friend WithEvents tSearch As TextBox
    Friend WithEvents picCancel As PictureBox
    Friend WithEvents checkSearchSource As CheckBox
    Friend WithEvents labelItemCount As Label
    Friend WithEvents playButton As Button
    Friend WithEvents checkPlayOnClick As CheckBox
    Friend WithEvents sortCombo As ComboBox
    Friend WithEvents sortLabel As Label
    Friend WithEvents sortRevButton As Button
    Friend WithEvents colStat As ColumnHeader
End Class
