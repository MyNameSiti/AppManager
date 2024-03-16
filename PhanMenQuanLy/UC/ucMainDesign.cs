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

        public ucMainDesign(DataTable dataTable,string controlname)
        {

            InitializeComponent();
            grTable.DataSource = dataTable;
            grName.Text = controlname + "Information";
            grListName.Text = controlname + "List";
        }
        public void List()
        {

        }
    }
}
