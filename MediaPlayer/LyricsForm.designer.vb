<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LyricsForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LyricsForm))
        Me.tLyrics = New System.Windows.Forms.TextBox()
        Me.buttonSaveExit = New System.Windows.Forms.Button()
        Me.checkAutoSave = New System.Windows.Forms.CheckBox()
        Me.buttonSearchOnline = New System.Windows.Forms.Button()
        Me.buttonLyricsOpenSource = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'tLyrics
        '
        Me.tLyrics.Location = New System.Drawing.Point(1, 1)
        Me.tLyrics.Multiline = True
        Me.tLyrics.Name = "tLyrics"
        Me.tLyrics.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.tLyrics.Size = New System.Drawing.Size(482, 680)
        Me.tLyrics.TabIndex = 34
        '
        'buttonSaveExit
        '
        Me.buttonSaveExit.Location = New System.Drawing.Point(408, 685)
        Me.buttonSaveExit.Name = "buttonSaveExit"
        Me.buttonSaveExit.Size = New System.Drawing.Size(75, 23)
        Me.buttonSaveExit.TabIndex = 35
        Me.buttonSaveExit.Text = "Save && Exit"
        Me.buttonSaveExit.UseMnemonic = False
        Me.buttonSaveExit.UseVisualStyleBackColor = True
        '
        'checkAutoSave
        '
        Me.checkAutoSave.AutoSize = True
        Me.checkAutoSave.Location = New System.Drawing.Point(326, 689)
        Me.checkAutoSave.Name = "checkAutoSave"
        Me.checkAutoSave.Size = New System.Drawing.Size(76, 17)
        Me.checkAutoSave.TabIndex = 36
        Me.checkAutoSave.Text = "Auto Save"
        Me.checkAutoSave.UseVisualStyleBackColor = True
        '
        'buttonSearchOnline
        '
        Me.buttonSearchOnline.Location = New System.Drawing.Point(1, 685)
        Me.buttonSearchOnline.Name = "buttonSearchOnline"
        Me.buttonSearchOnline.Size = New System.Drawing.Size(85, 23)
        Me.buttonSearchOnline.TabIndex = 37
        Me.buttonSearchOnline.Text = "Search Online"
        Me.buttonSearchOnline.UseMnemonic = False
        Me.buttonSearchOnline.UseVisualStyleBackColor = True
        '
        'buttonLyricsOpenSource
        '
        Me.buttonLyricsOpenSource.Location = New System.Drawing.Point(173, 685)
        Me.buttonLyricsOpenSource.Name = "buttonLyricsOpenSource"
        Me.buttonLyricsOpenSource.Size = New System.Drawing.Size(85, 23)
        Me.buttonLyricsOpenSource.TabIndex = 38
        Me.buttonLyricsOpenSource.Text = "Open Source"
        Me.buttonLyricsOpenSource.UseMnemonic = False
        Me.buttonLyricsOpenSource.UseVisualStyleBackColor = True
        '
        'LyricsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(484, 711)
        Me.Controls.Add(Me.buttonLyricsOpenSource)
        Me.Controls.Add(Me.buttonSearchOnline)
        Me.Controls.Add(Me.checkAutoSave)
        Me.Controls.Add(Me.buttonSaveExit)
        Me.Controls.Add(Me.tLyrics)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(500, 750)
        Me.Name = "LyricsForm"
        Me.Text = "Lyrics"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tLyrics As TextBox
    Friend WithEvents buttonSaveExit As Button
    Friend WithEvents checkAutoSave As CheckBox
    Friend WithEvents buttonSearchOnline As Button
    Friend WithEvents buttonLyricsOpenSource As Button
End Class
