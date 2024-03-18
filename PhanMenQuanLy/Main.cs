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
                    group.Text = item["MenuID"].ToString();
                    group.Tag = item["Link"];
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
                    Child.Tag = item["Link"];
                    Child.Tag = item["PageID"];
                    Child.Click += AccordionControlElement_Click;
                    group.Elements.Add(Child);
                }
            }
        }
        private UserControl CreateUserControlInstance(string ucControl)
        {

            Permission.ObjectSelect objectSelect = new Permission.ObjectSelect();
            return new UC.ucMainDesign(objectSelect.GetTable(ucControl), "Sách",this);


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
                string link = element.Tag.ToString();
                //string Role = element.Hint.ToString();
                //if (!Permission.AsEnumerable().Any(row => row.Field<string>("PageID") == Role))
                //{
                //    MessageBox.Show("Permission Denied");
                //    return;
                //}
                //else
                //{

                try
                {
                    UserControl userControl = CreateUserControlInstance("App_VehicleType");
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
