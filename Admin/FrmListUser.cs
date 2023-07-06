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
    public partial class FrmListUser : Form
    {
        public FrmListUser()
        {
            InitializeComponent();
        }

        private void FrmListUser_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = DataBase.GetDataFromDatase(null, "SELECT UserID AS N'លេខសម្គាល់',Name AS N'ឈ្មោះ', UserName as N'គណនី',position as N'តួនាទី',Status as N'ស្ថានភាព' FROM dbo.tblUser").Tables[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to reset password", "Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                DataBase.runSQL(null, "update tblUser set Password='123' where UserId='" + DataBase.User + "'");
                MessageBox.Show("Your new password: 123");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
            {
                try
                {
                    DataBase.runSQL(null, @"update tblUser set Name='" + item.Cells[1].Value.ToString() + "',UserName='" + item.Cells[2].Value.ToString() + "',position='" + item.Cells[3].Value.ToString() + "',Status='" + item.Cells[4].Value.ToString() + "' where UserID='" + item.Cells[0].Value.ToString() + "'");
                    MessageBox.Show("Update sucessfull");
                }
                catch (Exception)
                {

                    MessageBox.Show("Please try again.","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
        }
    }
}
