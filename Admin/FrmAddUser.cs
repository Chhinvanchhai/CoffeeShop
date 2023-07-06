using ConnectionDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Admin
{
    public partial class FrmAddUser : Form
    {
        public FrmAddUser()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string s = "";
                if (txtName.Text == "" || txtAccount.Text == "" || txtPassword.Text == "" || txtRePassword.Text == "")
                {
                    if (txtName.Text == "")
                        errorProvider1.SetError(txtName, "មិនអាចទទេ");
                    if (txtAccount.Text == "")
                        errorProvider1.SetError(txtAccount, "មិនអាចទទេ");
                    if (txtPassword.Text == "")
                        errorProvider1.SetError(txtPassword, "មិនអាចទទេ");
                    if (txtRePassword.Text == "")
                        errorProvider1.SetError(txtRePassword, "មិនអាចទទេ");
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt = DataBase.GetDataFromDatase(null, "select top 1 UserID, Name,UserName,Password,Position from tblUser where UserName='" + txtAccount.Text + "'").Tables[0];
                    if (txtPassword.Text == txtRePassword.Text)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            MessageBox.Show("User name already.");
                        }
                        else
                        {
                            DataBase.InsertByRunSP(null, "sp_user", "@SureName:@UserName:@Password:@ProfileID", ref s, null, null,
                            txtName.Text,
                            txtAccount.Text,
                            txtPassword.Text,
                            comboBox1.SelectedIndex == 0 ? 0 : 1);
                            MessageBox.Show("Save Seccessfully.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Password not match.");
                    }
                }

            }
            catch (Exception ex)
            {


            }
        }

        private void FrmAddUser_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }
    }
}
