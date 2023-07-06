using ConnectionDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CoffeeShop
{
    public partial class FrmChangPass : Form
    {
        public FrmChangPass()
        {
            InitializeComponent();
        }

        private void FrmChangPass_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DataTable count =new DataTable();
            count=DataBase.GetDataFromDatase(null, "select * from tblUser where UserId='"+DataBase.User+ "'").Tables[0];
            if(count.Rows.Count>0)
            {
                if(txtPassword.Text !=txtRePassword.Text)
                {
                    MessageBox.Show("Password not matched");
                }
                else
                {
                    DataBase.runSQL(null, "update tblUser set Password='" + txtPassword.Text + "' where UserId='" + DataBase.User + "'");
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Old password not matched");
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to reset password", "Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                DataBase.runSQL(null, "update tblUser set Password='123' where UserId='" + DataBase.User + "'");
                MessageBox.Show("Your new password: 123");
            }
        }
    }
}
