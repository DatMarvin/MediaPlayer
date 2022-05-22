<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PartsForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PartsForm))
        Me.listParts = New System.Windows.Forms.ListView()
        Me.number = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.from = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.toCol = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.nameCol = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.buttonAddPart = New System.Windows.Forms.Button()
        Me.buttonDeletePart = New System.Windows.Forms.Button()
        Me.checkAutoSave = New System.Windows.Forms.CheckBox()
        Me.groupEdit = New System.Windows.Forms.GroupBox()
        Me.checkPlayOnChange = New System.Windows.Forms.CheckBox()
        Me.labelPartSelection = New System.Windows.Forms.Label()
        Me.labelPartName = New System.Windows.Forms.Label()
        Me.labelPartTo = New System.Windows.Forms.Label()
        Me.labelPartFrom = New System.Windows.Forms.Label()
        Me.buttonApplyEdit = New System.Windows.Forms.Button()
        Me.tPartName = New System.Windows.Forms.TextBox()
        Me.tPartTo = New System.Windows.Forms.TextBox()
        Me.tPartFrom = New System.Windows.Forms.TextBox()
        Me.buttonOpenRaw = New System.Windows.Forms.Button()
        Me.groupEdit.SuspendLayout()
        Me.SuspendLayout()
        '
        'listParts
        '
        Me.listParts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.number, Me.from, Me.toCol, Me.nameCol})
        Me.listParts.FullRowSelect = True
        Me.listParts.GridLines = True
        Me.listParts.Location = New System.Drawing.Point(0, 0)
        Me.listParts.MultiSelect = False
        Me.listParts.Name = "listParts"
        Me.listParts.Size = New System.Drawing.Size(384, 200)
        Me.listParts.TabIndex = 76
        Me.listParts.TabStop = False
        Me.listParts.UseCompatibleStateImageBehavior = False
        Me.listParts.View = System.Windows.Forms.View.Details
        '
        'number
        '
        Me.number.Text = "#"
        Me.number.Width = 30
        '
        'from
        '
        Me.from.Text = "From"
        '
        'toCol
        '
        Me.toCol.Text = "To"
        '
        'nameCol
        '
        Me.nameCol.Text = "Name"
        Me.nameCol.Width = 213
        '
        'buttonAddPart
        '
        Me.buttonAddPart.Location = New System.Drawing.Point(13, 209)
        Me.buttonAddPart.Name = "buttonAddPart"
        Me.buttonAddPart.Size = New System.Drawing.Size(75, 23)
        Me.buttonAddPart.TabIndex = 77
        Me.buttonAddPart.Text = "Add"
        Me.buttonAddPart.UseVisualStyleBackColor = True
        '
        'buttonDeletePart
        '
        Me.buttonDeletePart.Location = New System.Drawing.Point(289, 10)
        Me.buttonDeletePart.Name = "buttonDeletePart"
        Me.buttonDeletePart.Size = New System.Drawing.Size(75, 23)
        Me.buttonDeletePart.TabIndex = 78
        Me.buttonDeletePart.TabStop = False
        Me.buttonDeletePart.Text = "Delete"
        Me.buttonDeletePart.UseVisualStyleBackColor = True
        '
        'checkAutoSave
        '
        Me.checkAutoSave.AutoSize = True
        Me.checkAutoSave.Location = New System.Drawing.Point(290, 62)
        Me.checkAutoSave.Name = "checkAutoSave"
        Me.checkAutoSave.Size = New System.Drawing.Size(76, 17)
        Me.checkAutoSave.TabIndex = 79
        Me.checkAutoSave.TabStop = False
        Me.checkAutoSave.Text = "Auto Save"
        Me.checkAutoSave.UseVisualStyleBackColor = True
        '
        'groupEdit
        '
        Me.groupEdit.Controls.Add(Me.checkPlayOnChange)
        Me.groupEdit.Controls.Add(Me.labelPartSelection)
        Me.groupEdit.Controls.Add(Me.labelPartName)
        Me.groupEdit.Controls.Add(Me.labelPartTo)
        Me.groupEdit.Controls.Add(Me.buttonDeletePart)
        Me.groupEdit.Controls.Add(Me.checkAutoSave)
        Me.groupEdit.Controls.Add(Me.labelPartFrom)
        Me.groupEdit.Controls.Add(Me.buttonApplyEdit)
        Me.groupEdit.Controls.Add(Me.tPartName)
        Me.groupEdit.Controls.Add(Me.tPartTo)
        Me.groupEdit.Controls.Add(Me.tPartFrom)
        Me.groupEdit.Location = New System.Drawing.Point(7, 235)
        Me.groupEdit.Name = "groupEdit"
        Me.groupEdit.Size = New System.Drawing.Size(370, 110)
        Me.groupEdit.TabIndex = 81
        Me.groupEdit.TabStop = False
        '
        'checkPlayOnChange
        '
        Me.checkPlayOnChange.AutoSize = True
        Me.checkPlayOnChange.Location = New System.Drawing.Point(119, 43)
        Me.checkPlayOnChange.Name = "checkPlayOnChange"
        Me.checkPlayOnChange.Size = New System.Drawing.Size(103, 17)
        Me.checkPlayOnChange.TabIndex = 87
        Me.checkPlayOnChange.TabStop = False
        Me.checkPlayOnChange.Text = "Play On Change"
        Me.checkPlayOnChange.UseVisualStyleBackColor = True
        '
        'labelPartSelection
        '
        Me.labelPartSelection.AutoSize = True
        Me.labelPartSelection.Location = New System.Drawing.Point(16, 10)
        Me.labelPartSelection.Name = "labelPartSelection"
        Me.labelPartSelection.Size = New System.Drawing.Size(29, 13)
        Me.labelPartSelection.TabIndex = 86
        Me.labelPartSelection.Text = "Part:"
        '
        'labelPartName
        '
        Me.labelPartName.AutoSize = True
        Me.labelPartName.Location = New System.Drawing.Point(7, 83)
        Me.labelPartName.Name = "labelPartName"
        Me.labelPartName.Size = New System.Drawing.Size(38, 13)
        Me.labelPartName.TabIndex = 85
        Me.labelPartName.Text = "Name:"
        '
        'labelPartTo
        '
        Me.labelPartTo.AutoSize = True
        Me.labelPartTo.Location = New System.Drawing.Point(22, 57)
        Me.labelPartTo.Name = "labelPartTo"
        Me.labelPartTo.Size = New System.Drawing.Size(23, 13)
        Me.labelPartTo.TabIndex = 84
        Me.labelPartTo.Text = "To:"
        '
        'labelPartFrom
        '
        Me.labelPartFrom.AutoSize = True
        Me.labelPartFrom.Location = New System.Drawing.Point(12, 31)
        Me.labelPartFrom.Name = "labelPartFrom"
        Me.labelPartFrom.Size = New System.Drawing.Size(33, 13)
        Me.labelPartFrom.TabIndex = 83
        Me.labelPartFrom.Text = "From:"
        '
        'buttonApplyEdit
        '
        Me.buttonApplyEdit.Location = New System.Drawing.Point(289, 80)
        Me.buttonApplyEdit.Name = "buttonApplyEdit"
        Me.buttonApplyEdit.Size = New System.Drawing.Size(75, 23)
        Me.buttonApplyEdit.TabIndex = 82
        Me.buttonApplyEdit.TabStop = False
        Me.buttonApplyEdit.Text = "Apply"
        Me.buttonApplyEdit.UseVisualStyleBackColor = True
        '
        'tPartName
        '
        Me.tPartName.Location = New System.Drawing.Point(48, 80)
        Me.tPartName.Name = "tPartName"
        Me.tPartName.Size = New System.Drawing.Size(216, 20)
        Me.tPartName.TabIndex = 2
        '
        'tPartTo
        '
        Me.tPartTo.Location = New System.Drawing.Point(48, 54)
        Me.tPartTo.Name = "tPartTo"
        Me.tPartTo.Size = New System.Drawing.Size(56, 20)
        Me.tPartTo.TabIndex = 1
        '
        'tPartFrom
        '
        Me.tPartFrom.Location = New System.Drawing.Point(48, 28)
        Me.tPartFrom.Name = "tPartFrom"
        Me.tPartFrom.Size = New System.Drawing.Size(56, 20)
        Me.tPartFrom.TabIndex = 0
        '
        'buttonOpenRaw
        '
        Me.buttonOpenRaw.Location = New System.Drawing.Point(296, 209)
        Me.buttonOpenRaw.Name = "buttonOpenRaw"
        Me.buttonOpenRaw.Size = New System.Drawing.Size(75, 23)
        Me.buttonOpenRaw.TabIndex = 82
        Me.buttonOpenRaw.TabStop = False
        Me.buttonOpenRaw.Text = "Open Raw"
        Me.buttonOpenRaw.UseVisualStyleBackColor = True
        '
        'PartsForm
        '
        Me.AcceptButton = Me.buttonApplyEdit
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(384, 351)
        Me.Controls.Add(Me.buttonOpenRaw)
        Me.Controls.Add(Me.groupEdit)
        Me.Controls.Add(Me.buttonAddPart)
        Me.Controls.Add(Me.listParts)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(400, 390)
        Me.Name = "PartsForm"
        Me.Text = "Parts"
        Me.groupEdit.ResumeLayout(False)
        Me.groupEdit.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents listParts As ListView
    Friend WithEvents number As ColumnHeader
    Friend WithEvents nameCol As ColumnHeader
    Friend WithEvents from As ColumnHeader
    Friend WithEvents toCol As ColumnHeader
    Friend WithEvents buttonAddPart As Button
    Friend WithEvents buttonDeletePart As Button
    Friend WithEvents checkAutoSave As CheckBox
    Friend WithEvents groupEdit As GroupBox
    Friend WithEvents buttonApplyEdit As Button
    Friend WithEvents tPartName As TextBox
    Friend WithEvents tPartTo As TextBox
    Friend WithEvents tPartFrom As TextBox
    Friend WithEvents labelPartName As Label
    Friend WithEvents labelPartTo As Label
    Friend WithEvents labelPartFrom As Label
    Friend WithEvents labelPartSelection As Label
    Friend WithEvents buttonOpenRaw As Button
    Friend WithEvents checkPlayOnChange As CheckBox
End Class
