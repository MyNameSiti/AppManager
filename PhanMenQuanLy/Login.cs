using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Connection;

namespace PhanMenQuanLy
{
    public partial class fmLogin : Form
    {
        private bool _isLoggedIn = false;

        public fmLogin()
        {
            InitializeComponent();
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string rs = new Permission.Permission().Login(txtUserName.Text, txtPassword.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Login Error");
                return;
            } 
            
            _isLoggedIn = true;
            this.Hide();
            Main main = new Main(txtUserName.Text);
            main.Show();

        }

        private void fmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
