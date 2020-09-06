using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APP
{
    public partial class Login_FRM : Form
    {
        public Login_FRM()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            User objUser = new User();
            objUser.Name = txtName.Text;
            objUser.Password = txtPassword.Text;
            await objUser.Login(this);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Login_FRM_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
