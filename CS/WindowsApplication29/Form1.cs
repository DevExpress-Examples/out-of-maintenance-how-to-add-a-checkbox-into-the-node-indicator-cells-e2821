using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraTreeList;
using DevExpress.Utils.Drawing;

namespace WindowsApplication29
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            treeList1.DataSource = CreateTreeListSource();
            treeList1.KeyFieldName = "ID";
            treeList1.ParentFieldName = "ParentID";
            treeList1.PopulateColumns();

            edit = treeList1.RepositoryItems.Add("CheckEdit") as RepositoryItemCheckEdit;
            treeList1.Columns["Bool0"].ColumnEdit = edit;
            treeList1.Columns["Bool1"].ColumnEdit = edit;
            edit.EditValueChanged += new EventHandler(edit_EditValueChanged);
            edit.AllowGrayed = true;
        }

        void edit_EditValueChanged(object sender, EventArgs e)
        {
            treeList1.PostEditor();
        }

        private DataTable CreateTreeListSource()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("ParentID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(int));
            dt.Columns.Add("Bool0", typeof(bool));
            dt.Columns.Add("Bool1", typeof(bool));



            dt.Rows.Add(new object[] { 0, -1, "Aaa", 10 });
            dt.Rows.Add(new object[] { 1, -1, "Bbb", 10 });
            dt.Rows.Add(new object[] { 2, -1, "Ccc", 10 });
            dt.Rows.Add(new object[] { 3, -1, "Ddd", 10 });

            dt.Rows.Add(new object[] { 4, 0, "Aaa 1", 10 });
            dt.Rows.Add(new object[] { 5, 0, "Aaa 2", 10 });
            dt.Rows.Add(new object[] { 6, 0, "Aaa 3", 10 });

            dt.Rows.Add(new object[] { 7, 1, "Bbb 1", 10 });
            dt.Rows.Add(new object[] { 8, 1, "Bbb 2", 10 });

            dt.Rows.Add(new object[] { 10, 2, "Ccc 1", 10 });
            dt.Rows.Add(new object[] { 9, 2, "Ccc 2", 10 });

            dt.Rows.Add(new object[] { 12, 3, "Ddd 1", 10 });
            dt.Rows.Add(new object[] { 11, 3, "Ddd 2", 10 });


            return dt;
        }

        private void treeList1_CustomDrawNodeIndicator(object sender, DevExpress.XtraTreeList.CustomDrawNodeIndicatorEventArgs e)
        {
            DrawCheckBox(e.Cache, e.Bounds, GetCheckValue(e.Node));
            e.Handled = true;
        }

        private bool? GetCheckValue(DevExpress.XtraTreeList.Nodes.TreeListNode node)
        {
            if (node.GetValue("Bool0") == null || node.GetValue("Bool1") == null || node.GetValue("Bool0") == DBNull.Value || node.GetValue("Bool1") == DBNull.Value)
                return null;
            if (Convert.ToBoolean( node.GetValue("Bool0") ) == true && Convert.ToBoolean( node.GetValue("Bool1") ) == true)
                return true;
            if ((bool)node.GetValue("Bool0") == false && (bool)node.GetValue("Bool1") == false)
                return false;
            return null;
        }

        RepositoryItemCheckEdit edit;
        protected void DrawCheckBox(GraphicsCache cache, Rectangle r, bool? Checked) {
            DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo info;
            DevExpress.XtraEditors.Drawing.CheckEditPainter painter;
            DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs args;
            info = edit.CreateViewInfo() as DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo;
            painter = edit.CreatePainter() as DevExpress.XtraEditors.Drawing.CheckEditPainter;
            info.EditValue = Checked;
            info.Bounds = r;
            info.CalcViewInfo();
            args = new DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs(info, cache, r);
            painter.Draw(args);
        }

        private void treeList1_MouseClick(object sender, MouseEventArgs e)
        {
            TreeList tl = sender as TreeList;
            TreeListHitInfo info = tl.CalcHitInfo(e.Location);
            if (info.HitInfoType == HitInfoType.RowIndicator)
            {
                bool currentValue = Convert.ToBoolean(GetCheckValue(info.Node));
                SetCheckValue(info.Node, !currentValue);
                tl.InvalidateNode(info.Node);
            }

        }

        private void SetCheckValue(DevExpress.XtraTreeList.Nodes.TreeListNode node, bool value)
        {
            node.SetValue("Bool0", value);
            node.SetValue("Bool1", value);
        }

        private void treeList1_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "Bool0" || e.Column.FieldName == "Bool1")
                ((TreeList)sender).InvalidateNode(e.Node);
        }
    }
}