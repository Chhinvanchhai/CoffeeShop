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
    public partial class FrmExpens : Form
    {
        public FrmExpens()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string s = "";
            DataBase.InsertByRunSP(null, "sp_expens", "@ex_date:@amount:@descriptions:@createdby", ref s, null, null,
                       dateTimePicker1.Text,
                       txtAmount.Text,
                       txtDes.Text,
                       DataBase.User
                        );
            MessageBox.Show("Save Success.");
            txtDes.Text = "";
            txtAmount.Text = "";
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataBase.numberOnly(sender, e);
        }
    }
}
