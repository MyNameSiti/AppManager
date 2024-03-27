using DevExpress.XtraBars.Navigation;
using Permission;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;


namespace PhanMenQuanLy
{
    public partial class Main : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        public DataTable Permission;
        public DataTable ListMenu;
        public void AddControl()
        {
            foreach (DataRow item in ListMenu.Rows)
            {
                if (item["MenuParent"].ToString() == "0")
                {
                    AccordionControlElement group = new AccordionControlElement();
                    group.Style = ElementStyle.Group;
                    group.Name = item["MenuID"].ToString();
                    group.Text = item["MenuName"].ToString();
                    group.Tag = item["PageID"];
                    lstMenu.Elements.Add(group);
                }
                else
                {
                    AccordionControlElement group = lstMenu.Elements.OfType<AccordionControlElement>().FirstOrDefault(el => el.Name == item["MenuParent"].ToString());
                    AccordionControlElement Child = new AccordionControlElement();
                    Child.Style = ElementStyle.Group;
                    Child.Name = item["MenuID"].ToString();
                    Child.Text = item["MenuID"].ToString();
                    Child.Tag = item["PageID"];
                    Child.Click += AccordionControlElement_Click;
                    group.Elements.Add(Child);
                }
            }
        }
        private UserControl CreateUserControlInstance(string PageID,string PageName)
        {

            Permission.ObjectSelect objectSelect = new Permission.ObjectSelect();
            return new UC.ucMainDesign(objectSelect.GetTable(PageID), PageName, this);


        }
        public Main(string UserID)
        {
            InitializeComponent();
            Permission.Permission premission = new Permission.Permission();
            ListMenu = premission.LoadMenu();
            Permission = premission.GetPermisson(UserID);
            AddControl();

        }
        private void AccordionControlElement_Click(object sender, EventArgs e)
        {
            if (sender is AccordionControlElement)
            {
                AccordionControlElement element = (AccordionControlElement)sender;
                string PageID = element.Tag.ToString();
                string PageName = element.Name.ToString();

                try
                {
                    UserControl userControl = CreateUserControlInstance(PageID, PageName);
                    if (userControl != null)
                    {
                        // Xóa tất cả các UserControl khác trong frMain
                        this.fmMain.Controls.Clear();
                        // Thêm UserControl vào frMain
                        userControl.Dock = DockStyle.Fill;
                        this.fmMain.Controls.Add(userControl);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                //}
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
