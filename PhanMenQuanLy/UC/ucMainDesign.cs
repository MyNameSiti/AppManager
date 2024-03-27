using DevExpress.XtraEditors;
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
            grName.Text = controlname + "Information";
            grListName.Text = controlname + "List";
            frMain.Permission.Select("PageID = " + controlname + "or Reference = " + controlname);
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
            foreach (var item in ControlList.Rows)
            {
                
            }
        }
        public void List()
        {
            DataTable dataTable = new DataTable();


            grTable.DataSource = dataTable;
        }
    }
}
