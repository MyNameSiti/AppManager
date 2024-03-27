using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhanMenQuanLy.UC
{
    public partial class ucMainDesign : DevExpress.XtraEditors.XtraUserControl
    {

        public ucMainDesign(DataTable ControlList, string controlname, Main frMain)
        {

            InitializeComponent();
            grName.Text = controlname + " Information";
            grListName.Text = controlname + " List";
            // frMain.Permission.Select("PageID = " + controlname);
            foreach (DataRow row in frMain.Permission.Rows)
            {
                if (row["Action"].ToString().Equals("ADD"))
                {
                    btnAdd.Enabled = true;
                    btnSave.Enabled = true;
                    continue;
                }
                if (row["Action"].ToString().Equals("EDIT"))
                {
                    btnEdit.Enabled = true;
                    btnSave.Enabled = true;
                    continue;
                }
                if (row["Action"].ToString().Equals("DELETE"))
                {
                    btnDelete.Enabled = true;
                    continue;
                }
                if (row["Action"].ToString().Equals("EXPORT"))
                {
                    btnExport.Enabled = true;
                    continue;
                }
            }
            System.Drawing.Point locationA = new System.Drawing.Point(0, 0);
            System.Drawing.Point locationB = new System.Drawing.Point(455, 0);
            bool A = true;
            foreach (DataRow item in ControlList.Rows)
            {
                //layoutControl1.Controls.Add(this.txtGiaNhap);
                switch (item["ControlType"])
                {
                    case "TXT":
                        DevExpress.XtraEditors.TextEdit TextBox = new DevExpress.XtraEditors.TextEdit();
                        TextBox.MenuManager = this.barManager1;
                        TextBox.Properties.Mask.BeepOnError = true;
                        TextBox.Size = new System.Drawing.Size(235, 20);
                        TextBox.StyleController = this.layoutControl1;
                        this.layoutControl1.Controls.Add(TextBox);
                        DevExpress.XtraLayout.LayoutControlItem layoutControlItem = new DevExpress.XtraLayout.LayoutControlItem(); ;
                        layoutControlItem.Control = TextBox;
                        if (A)
                            layoutControlItem.Location = locationA;
                        else
                            layoutControlItem.Location = locationB;
                        layoutControlItem.Size = new System.Drawing.Size(455, 59);
                        layoutControlItem.Text = item["ControlName"].ToString();
                        layoutControlItem.TextSize = new System.Drawing.Size(70, 13);
                        this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutControlItem });




                        break;
                    case "LST":

                        break;
                    default:
                        break;
                }

                A = !A;

            }
        }
        public void List()
        {
            DataTable dataTable = new DataTable();


            grTable.DataSource = dataTable;
        }
    }
}
