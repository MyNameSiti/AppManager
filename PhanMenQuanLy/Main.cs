using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Navigation;
using Permission;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;


namespace PhanMenQuanLy
{
    public partial class Main : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        public DataTable Permison;
        public DataTable ListMenu;
        public AccordionControlElement Control(DataRow Name)
        {
            AccordionControlElement Menu = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            Menu.Name = Name["MenuID"].ToString();
            Menu.Text = Name["PageName"].ToString();
            Menu.Tag = Name["Link"];
            Menu.Click += AccordionControlElement_Click;
            return Menu;
        }

        public AccordionControl AddControl()
        {
            AccordionControl listMenu = new AccordionControl();
            foreach (DataRow item in ListMenu.Rows)
            {
                if (item["MenuParent"].ToString() == "0")
                {
                    listMenu.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] { Control(item) });
                }
                else
                {

                    if (listMenu != null)
                    {
                        foreach (AccordionControlElement parentElement in listMenu.Elements)
                        {
                            if (parentElement.Name == item["MenuParent"].ToString())
                            {
                                parentElement.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] { Control(item) });
                                listMenu.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] { parentElement });
                                break;
                            }
                        }

                    }

                }
            }
            return listMenu;
        }
        private UserControl CreateUserControlInstance(string controlName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            Type controlType = types.FirstOrDefault(t => t.Name == $"UC_{controlName}" && t.IsSubclassOf(typeof(UserControl)));

            if (controlType != null)
            {
                return (UserControl)Activator.CreateInstance(controlType);
            }
            else
            {
                MessageBox.Show($"UserControl {controlName} not found.");
            }
            return null;
        }
        public Main(string UserID)
        {
            InitializeComponent();
            Permission.Permission premission = new Permission.Permission();
            ListMenu = premission.LoadMenu();
            Permison = premission.GetPermisson(UserID);
            this.lstMenu = AddControl();

        }
        private void AccordionControlElement_Click(object sender, EventArgs e)
        {
            if (sender is AccordionControlElement)
            {
                AccordionControlElement element = (AccordionControlElement)sender;
                string link = element.Tag.ToString();
                UserControl userControl = CreateUserControlInstance(link);
                if (userControl != null)
                {
                    // Xóa tất cả các UserControl khác trong frMain
                    this.fmMain.Controls.Clear();
                    // Thêm UserControl vào frMain
                    userControl.Dock = DockStyle.Fill;
                    this.fmMain.Controls.Add(userControl);
                }
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
