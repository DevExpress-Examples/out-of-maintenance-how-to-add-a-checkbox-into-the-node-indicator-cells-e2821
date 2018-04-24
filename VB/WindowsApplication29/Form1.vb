Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraTreeList

Namespace WindowsApplication29
    Partial Public Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            treeList1.DataSource = CreateTreeListSource()
            treeList1.KeyFieldName = "ID"
            treeList1.ParentFieldName = "ParentID"
            treeList1.PopulateColumns()

            edit = TryCast(treeList1.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
            treeList1.Columns("Bool0").ColumnEdit = edit
            treeList1.Columns("Bool1").ColumnEdit = edit
            AddHandler edit.EditValueChanged, AddressOf edit_EditValueChanged
            edit.AllowGrayed = True
        End Sub

        Private Sub edit_EditValueChanged(ByVal sender As Object, ByVal e As EventArgs)
            treeList1.PostEditor()
        End Sub

        Private Function CreateTreeListSource() As DataTable
            Dim dt As New DataTable()
            dt.Columns.Add("ID", GetType(Integer))
            dt.Columns.Add("ParentID", GetType(Integer))
            dt.Columns.Add("Name", GetType(String))
            dt.Columns.Add("Value", GetType(Integer))
            dt.Columns.Add("Bool0", GetType(Boolean))
            dt.Columns.Add("Bool1", GetType(Boolean))



            dt.Rows.Add(New Object() { 0, -1, "Aaa", 10 })
            dt.Rows.Add(New Object() { 1, -1, "Bbb", 10 })
            dt.Rows.Add(New Object() { 2, -1, "Ccc", 10 })
            dt.Rows.Add(New Object() { 3, -1, "Ddd", 10 })

            dt.Rows.Add(New Object() { 4, 0, "Aaa 1", 10 })
            dt.Rows.Add(New Object() { 5, 0, "Aaa 2", 10 })
            dt.Rows.Add(New Object() { 6, 0, "Aaa 3", 10 })

            dt.Rows.Add(New Object() { 7, 1, "Bbb 1", 10 })
            dt.Rows.Add(New Object() { 8, 1, "Bbb 2", 10 })

            dt.Rows.Add(New Object() { 10, 2, "Ccc 1", 10 })
            dt.Rows.Add(New Object() { 9, 2, "Ccc 2", 10 })

            dt.Rows.Add(New Object() { 12, 3, "Ddd 1", 10 })
            dt.Rows.Add(New Object() { 11, 3, "Ddd 2", 10 })


            Return dt
        End Function

        Private Sub treeList1_CustomDrawNodeIndicator(ByVal sender As Object, ByVal e As DevExpress.XtraTreeList.CustomDrawNodeIndicatorEventArgs) Handles treeList1.CustomDrawNodeIndicator
            DrawCheckBox(e.Graphics, e.Bounds, GetCheckValue(e.Node))
            e.Handled = True
        End Sub

        Private Function GetCheckValue(ByVal node As DevExpress.XtraTreeList.Nodes.TreeListNode) As Boolean?
            If node.GetValue("Bool0") Is Nothing OrElse node.GetValue("Bool1") Is Nothing OrElse node.GetValue("Bool0") Is DBNull.Value OrElse node.GetValue("Bool1") Is DBNull.Value Then
                Return Nothing
            End If
            If Convert.ToBoolean(node.GetValue("Bool0")) = True AndAlso Convert.ToBoolean(node.GetValue("Bool1")) = True Then
                Return True
            End If
            If CBool(node.GetValue("Bool0")) = False AndAlso CBool(node.GetValue("Bool1")) = False Then
                Return False
            End If
            Return Nothing
        End Function



        Private edit As RepositoryItemCheckEdit
        Protected Sub DrawCheckBox(ByVal g As Graphics, ByVal r As Rectangle, ByVal Checked? As Boolean)
            Dim info As DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo
            Dim painter As DevExpress.XtraEditors.Drawing.CheckEditPainter
            Dim args As DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs
            info = TryCast(edit.CreateViewInfo(), DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo)
            painter = TryCast(edit.CreatePainter(), DevExpress.XtraEditors.Drawing.CheckEditPainter)
            info.EditValue = Checked
            info.Bounds = r
            info.CalcViewInfo(g)
            args = New DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs(info, New DevExpress.Utils.Drawing.GraphicsCache(g), r)
            painter.Draw(args)
            args.Cache.Dispose()
        End Sub

        Private Sub treeList1_MouseClick(ByVal sender As Object, ByVal e As MouseEventArgs) Handles treeList1.MouseClick
            Dim tl As TreeList = TryCast(sender, TreeList)
            Dim info As TreeListHitInfo = tl.CalcHitInfo(e.Location)
            If info.HitInfoType = HitInfoType.RowIndicator Then
                Dim currentValue As Boolean = Convert.ToBoolean(GetCheckValue(info.Node))
                SetCheckValue(info.Node, Not currentValue)
                tl.InvalidateNode(info.Node)
            End If

        End Sub

        Private Sub SetCheckValue(ByVal node As DevExpress.XtraTreeList.Nodes.TreeListNode, ByVal value As Boolean)
            node.SetValue("Bool0", value)
            node.SetValue("Bool1", value)
        End Sub

        Private Sub treeList1_CellValueChanged(ByVal sender As Object, ByVal e As CellValueChangedEventArgs) Handles treeList1.CellValueChanged
            If e.Column.FieldName = "Bool0" OrElse e.Column.FieldName = "Bool1" Then
                DirectCast(sender, TreeList).InvalidateNode(e.Node)
            End If
        End Sub
    End Class
End Namespace