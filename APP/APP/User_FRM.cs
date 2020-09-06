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
    public partial class User_FRM : Form
    {
        public User_FRM()
        {
            InitializeComponent();
        }
        private User objUser;
        private async  void User_FRM_Load(object sender, EventArgs e)
        {
            objUser = new User();
            await  objUser.GetData(dgUser);
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            objUser = new User();
            objUser.Name = txtName.Text;
            objUser.Password = txtPassword.Text;
            await objUser.Save();
        }

        private async void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            objUser = new User();
           await objUser.Delete(dgUser);

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            objUser = new User();
            if (r_true.Checked)
            {
                objUser.Admin = r_true.Text;
            }
            if (r_false.Checked)
            {
                objUser.Admin = r_false.Text;
            }
            await objUser.PromoteUser(dgUser);
        }

        private void dgUser_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            objUser = new User();
            objUser.TrasferDataToTextbox(dgUser, txtName, txtPassword, r_true, r_false);
        }

        private async void btndelete_Click(object sender, EventArgs e)
        {
            objUser = new User();
            await objUser.Delete(dgUser);
        }

        private void User_FRM_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
