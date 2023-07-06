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
    public partial class FrmWifi : Form
    {
        public FrmWifi()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataBase.runSQL(null, "update tbwifi set pass='" + textBox1.Text + "'");
            DataBase.PassWifi = textBox1.Text;
            this.Close();
        }

        private void FrmWifi_Load(object sender, EventArgs e)
        {
            int num = int.Parse(DataBase.GetScalrValue(null, "select pass FROM tbwifi"));
            textBox1.Text = num.ToString();
        }
    }
}
