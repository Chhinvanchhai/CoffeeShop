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
    public partial class FrmAdminLogin : Form
    {
        public FrmAdminLogin()
        {
            InitializeComponent();
        }
        DataTable dt = new DataTable();
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string queryString = "select top 1 UserID, Name,UserName,Password,Position from tblUser where UserName='" + txtUserName.Text + "' and Password='" + txtPass.Text + "'";
            System.Console.Write(queryString);

            dt = DataBase.GetDataFromDatase(null, sql: queryString).Tables[0];
            if (dt.Rows.Count > 0)
            {
                FrmMain main = new FrmMain();
                if (dt.Rows[0]["position"].ToString() == "1")
                {
                    DataBase.User = dt.Rows[0][0].ToString();
                    this.Hide();
                    main.ShowDialog();
                }
                else
                {
                    MessageBox.Show("You don't have permission.");
                }
            }
            else
            {
                MessageBox.Show("Incorrect user name or password");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                btnLogin_Click(sender, null);
            }
        }

        private void txtUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==13)
            {
                txtPass.Focus();
               
            }
        }
    }
}
