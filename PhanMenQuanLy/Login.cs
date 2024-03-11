using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhanMenQuanLy
{
    public partial class fmLogin : Form
    {
        private bool _isLoggedIn = false;
        public fmLogin()
        {
            InitializeComponent();
        }

        private void fmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_isLoggedIn)
            {
               Application.Exit();
            }
        }
    }
}
