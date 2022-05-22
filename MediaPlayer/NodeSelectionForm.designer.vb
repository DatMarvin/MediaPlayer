<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NodeSelectionForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(NodeSelectionForm))
        Me.g_sub_nodes = New System.Windows.Forms.GroupBox()
        Me.okButton = New System.Windows.Forms.Button()
        Me.cancelNodeButton = New System.Windows.Forms.Button()
        Me.Group9 = New System.Windows.Forms.GroupBox()
        Me.remButton3 = New System.Windows.Forms.Button()
        Me.listNodes = New System.Windows.Forms.ListBox()
        Me.tvSelection = New System.Windows.Forms.TreeView()
        Me.g_sub_nodes.SuspendLayout()
        Me.Group9.SuspendLayout()
        Me.SuspendLayout()
        '
        'g_sub_nodes
        '
        Me.g_sub_nodes.Controls.Add(Me.okButton)
        Me.g_sub_nodes.Controls.Add(Me.cancelNodeButton)
        Me.g_sub_nodes.Controls.Add(Me.Group9)
        Me.g_sub_nodes.Controls.Add(Me.tvSelection)
        Me.g_sub_nodes.Location = New System.Drawing.Point(2, 2)
        Me.g_sub_nodes.Name = "g_sub_nodes"
        Me.g_sub_nodes.Size = New System.Drawing.Size(302, 256)
        Me.g_sub_nodes.TabIndex = 19
        Me.g_sub_nodes.TabStop = False
        Me.g_sub_nodes.Text = "Node Selection"
        '
        'okButton
        '
        Me.okButton.Location = New System.Drawing.Point(179, 186)
        Me.okButton.Name = "okButton"
        Me.okButton.Size = New System.Drawing.Size(111, 28)
        Me.okButton.TabIndex = 65
        Me.okButton.Text = "Ok"
        Me.okButton.UseVisualStyleBackColor = True
        '
        'cancelNodeButton
        '
        Me.cancelNodeButton.Location = New System.Drawing.Point(179, 220)
        Me.cancelNodeButton.Name = "cancelNodeButton"
        Me.cancelNodeButton.Size = New System.Drawing.Size(111, 28)
        Me.cancelNodeButton.TabIndex = 18
        Me.cancelNodeButton.Text = "Cancel"
        Me.cancelNodeButton.UseVisualStyleBackColor = True
        '
        'Group9
        '
        Me.Group9.Controls.Add(Me.remButton3)
        Me.Group9.Controls.Add(Me.listNodes)
        Me.Group9.Location = New System.Drawing.Point(173, 9)
        Me.Group9.Name = "Group9"
        Me.Group9.Size = New System.Drawing.Size(123, 154)
        Me.Group9.TabIndex = 13
        Me.Group9.TabStop = False
        Me.Group9.Text = "Selected Nodes"
        '
        'remButton3
        '
        Me.remButton3.Location = New System.Drawing.Point(6, 117)
        Me.remButton3.Name = "remButton3"
        Me.remButton3.Size = New System.Drawing.Size(111, 28)
        Me.remButton3.TabIndex = 17
        Me.remButton3.Text = "Remove"
        Me.remButton3.UseVisualStyleBackColor = True
        '
        'listNodes
        '
        Me.listNodes.FormattingEnabled = True
        Me.listNodes.HorizontalScrollbar = True
        Me.listNodes.Location = New System.Drawing.Point(6, 16)
        Me.listNodes.Name = "listNodes"
        Me.listNodes.Size = New System.Drawing.Size(111, 95)
        Me.listNodes.Sorted = True
        Me.listNodes.TabIndex = 7
        '
        'tvSelection
        '
        Me.tvSelection.FullRowSelect = True
        Me.tvSelection.HideSelection = False
        Me.tvSelection.Location = New System.Drawing.Point(6, 17)
        Me.tvSelection.Name = "tvSelection"
        Me.tvSelection.ShowLines = False
        Me.tvSelection.Size = New System.Drawing.Size(161, 231)
        Me.tvSelection.TabIndex = 64
        '
        'NodeSelectionForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(311, 265)
        Me.Controls.Add(Me.g_sub_nodes)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "NodeSelectionForm"
        Me.g_sub_nodes.ResumeLayout(False)
        Me.Group9.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents g_sub_nodes As GroupBox
    Friend WithEvents okButton As Button
    Friend WithEvents cancelNodeButton As Button
    Friend WithEvents Group9 As GroupBox
    Friend WithEvents remButton3 As Button
    Friend WithEvents listNodes As ListBox
    Friend WithEvents tvSelection As TreeView
End Class
