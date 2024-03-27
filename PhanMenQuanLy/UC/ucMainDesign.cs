using System;
using System.Data;
using System.Linq;

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
                        DevExpress.XtraLayout.LayoutControlItem layoutTextBox = new DevExpress.XtraLayout.LayoutControlItem(); ;
                        layoutTextBox.Control = TextBox;
                        if (A)
                            layoutTextBox.Location = locationA;
                        else
                            layoutTextBox.Location = locationB;
                        layoutTextBox.Size = new System.Drawing.Size(455, 59);
                        layoutTextBox.Text = item["ControlName"].ToString();
                        layoutTextBox.TextSize = new System.Drawing.Size(70, 13);
                        this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutTextBox });

                        break;
                    case "CBB":
                        DevExpress.XtraEditors.ComboBoxEdit comboBox = new DevExpress.XtraEditors.ComboBoxEdit();
                        comboBox.MenuManager = this.barManager1;
                        comboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
                        comboBox.Size = new System.Drawing.Size(801, 20);
                        comboBox.StyleController = this.layoutControl1;
                        DevExpress.XtraLayout.LayoutControlItem layoutComboBox = new DevExpress.XtraLayout.LayoutControlItem(); ;
                        layoutComboBox.Control = comboBox;
                        if (A)
                            layoutComboBox.Location = locationA;
                        else
                            layoutComboBox.Location = locationB;
                        layoutComboBox.Size = new System.Drawing.Size(455, 59);
                        layoutComboBox.Text = item["ControlName"].ToString();
                        layoutComboBox.TextSize = new System.Drawing.Size(70, 13);
                        this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutComboBox });
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
