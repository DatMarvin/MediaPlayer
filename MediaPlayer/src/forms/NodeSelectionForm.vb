'08.07.19
Public Class NodeSelectionForm

    Dim dll As New Utils

    Public selNodes As List(Of Folder)
    Public arguments() As String


    ReadOnly Property args As List(Of String)
        Get
            If arguments Is Nothing Then Return New List(Of String)
            Return arguments.ToList()
        End Get
    End Property

    Private Sub NodeSelectionForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Size = New Size(325, 300)
        Me.Location = New Point(Form1.Left + Form1.Width / 2 - Me.Width / 2, Form1.Top + Form1.Height / 2 - Me.Height / 2)
        colorMe()
        selNodes = New List(Of Folder)
        tvSelection.Nodes.Clear()
        insTv(tvSelection.Nodes.Add(Folder.top.name, Folder.top.name), Folder.top)
        tvSelection.Nodes(Folder.top.name).Expand()
        listNodes.Items.Clear()
    End Sub

    Sub colorMe() '06.08.19
        Dim inverted As Boolean = SettingsService.getSetting(SettingsIdentifier.DARK_THEME)
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
            If TypeOf c Is ListBox Then
                c.BackColor = lightCol
                c.ForeColor = invLightCol
            ElseIf TypeOf c Is Button Then
                c.BackColor = lightCol
                c.ForeColor = invLightCol
            Else
                c.BackColor = darkCol
                c.ForeColor = invDarkCol
            End If

        Next
    End Sub

    Function selectNode(Optional title As String = "", Optional args() As String = Nothing) As Folder
        arguments = args
        dll.ExtendArray(arguments, "single")
        OptionsForm.TopMost = False
        TopMost = True
        Text = title
        tvSelection.Nodes.Clear()
        tvSelection.SelectedNode = Nothing
        ShowDialog()
        If selNodes.Count > 0 Then
            Return selNodes(0)
        End If
        Return Nothing
    End Function

    Function selectNodes(Optional title As String = "", Optional args() As String = Nothing) As List(Of Folder)
        arguments = args
        OptionsForm.TopMost = False
        TopMost = True
        Text = title
        tvSelection.Nodes.Clear()
        tvSelection.SelectedNode = Nothing
        ShowDialog()
        If selNodes.Count > 0 Then
            Return selNodes
        End If
        Return Nothing
    End Function

    Private Sub insTv(ByVal currNode As TreeNode, ByVal currFolder As Folder)
        If currFolder.children IsNot Nothing Then
            currFolder.children.Sort(Function(x, y) x.name.CompareTo(y.name))
            For i = 0 To currFolder.children.Count - 1
                If Not args.Contains("folder") Or currFolder.children(i).isVirtual Then
                    If Not args.Contains("virtual") Or Not currFolder.children(i).isVirtual Then
                        If Not args.Contains("excluded") Or Not Folder.getExcludedFolders().Contains(currFolder.children(i)) Then
                            insTv(currNode.Nodes.Add(currFolder.children(i).nodePath, currFolder.children(i).name), currFolder.children(i))
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub okButton_Click(sender As Object, e As EventArgs) Handles okButton.Click
        For Each fol As Folder In listNodes.Items
            selNodes.Add(fol)
        Next
        endNodeSelection()
    End Sub

    Sub endNodeSelection()
        OptionsForm.listMenu.Enabled = True
        Close()
    End Sub

    Private Sub tvSelection_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvSelection.AfterSelect
        If Not IsNothing(e.Node) Then
            Dim currFol As Folder = Folder.getFolder(Form1.root & e.Node.Name)
            If args.Contains("single") Then
                listNodes.Items.Clear()
                listNodes.Items.Add(currFol)
            Else
                If Not listNodes.Items.Contains(currFol) Then listNodes.Items.Add(currFol)
            End If
        End If
    End Sub

    Private Sub remButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles remButton3.Click
        If listNodes.SelectedIndex > -1 Then
            listNodes.Items.RemoveAt(listNodes.SelectedIndex)
        End If
    End Sub

    Private Sub cancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cancelNodeButton.Click
        listNodes.Items.Clear()
        endNodeSelection()
    End Sub

    Private Sub NodeSelectionForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        tvSelection.SelectedNode = Nothing
        tvSelection.Nodes.Clear()
        listNodes.Select()
        listNodes.Focus()
    End Sub
End Class