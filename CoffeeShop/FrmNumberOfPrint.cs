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
    public partial class FrmNumberOfPrint : Form
    {
        public FrmNumberOfPrint()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "")
            {
                textBox1.Text = "1";
            }
            DataBase.runSQL(null, "Update tbprint set num='" + textBox1.Text + "'");
            DataBase.NumOfPrint = int.Parse(textBox1.Text);
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataBase.numberOnly(sender, e);
        }

        private void FrmNumberOfPrint_Load(object sender, EventArgs e)
        {
            int num = int.Parse(DataBase.GetScalrValue(null, "select num FROM tbprint"));
            textBox1.Text = num.ToString();
        }
    }
}
