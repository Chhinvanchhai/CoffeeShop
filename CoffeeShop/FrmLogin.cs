using ConnectionDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CoffeeShop
{
    public partial class FrmLogin : Form
    {
        
        public FrmLogin()
        {
            InitializeComponent();
           
        }

     

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        DataTable dt = new DataTable();
        private void btnLogin_Click(object sender, EventArgs e)
        {
            FrmSell sell = new FrmSell();
            dt = DataBase.GetDataFromDatase(null,"select top 1 UserID, Name,UserName,Password,Position from tblUser where UserName='" + txtUserName.Text + "' and Password='" + txtPass.Text + "'").Tables[0];
           
            if (dt.Rows.Count>0)
            {
                DataBase.PassWifi = DataBase.GetScalrValue(null, "select pass FROM tbwifi");
                DataBase.NumOfPrint =int.Parse(DataBase.GetScalrValue(null, "select num FROM tbprint"));
                DataBase.User = dt.Rows[0][0].ToString();
                DataBase.UserName= dt.Rows[0][1].ToString();
                this.Hide();
                    sell.ShowDialog();
                
            }
            else
            {
                MessageBox.Show("Incorrect user name or password");
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
           // getClass();


        }

        private void txtPass_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        public void getClass()
        {
            string field = string.Empty;
            DataTable dt = DataBase.GetDataFromDatase(null, "sp_rpt_expense '2018-01-10' ,'2018-01-10' ,'0001'").Tables[0];
            foreach (DataColumn col in dt.Columns)
            {
                field += string.Format("{0} {1} {2} {3}", "public", col.DataType.Name.ToString(), col.ColumnName.ToString(), "{get;set;}") + Environment.NewLine;
            }
            string path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            StreamWriter sw = new StreamWriter(String.Format(@"{0}\\TempText.txt", path));
            sw.WriteLine(field);
            sw.Close();
            Process.Start(String.Format("{0}\\TempText.txt", path));
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                btnLogin_Click(sender, null);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            SendKeys.Send(btn.Text);
        }
    }
}
