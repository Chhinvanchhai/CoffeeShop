using ConnectionDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CoffeeShop
{
    public partial class FrmGetMoney : Form
    {
        public FrmGetMoney(FrmSell sell)
        {
            InitializeComponent();
            this.sell = sell;
            txtReceive.GotFocus += TxtReceive_GotFocus;
        }

        private void TxtReceive_GotFocus(object sender, EventArgs e)
        {
           
        }

        private FrmSell sell;
        public string total { get; set; }
        public string totalDollar { get; set; }
        private void FrmGetMoney_Load(object sender, EventArgs e)
        {
            txtTotalCheck.Text = total;
            txtTotalDollar.Text = totalDollar;
        }
       
        private void numRecieve_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtReceive_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtReceive_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataBase.numberOnly(sender, e);
        }

        private void txtReceive_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtBack.Text = (decimal.Parse(txtReceive.Text) - decimal.Parse(txtTotalCheck.Text)).ToString();
                txtBackDollar.Text = "0";
                textBox1.Text = "0";
            }
        }

        private void txtBackDollar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtBack.Text = "0";
                txtReceive.Text = "0";
                textBox1.Text = (decimal.Parse(txtBackDollar.Text) - decimal.Parse(txtTotalDollar.Text)).ToString();
            }
        }

        private void txtReceive_Leave(object sender, EventArgs e)
        {
            if(txtReceive.Text=="0")
            {
                txtBack.Text = "0";
            }
            else
            {
                txtBack.Text = (decimal.Parse(txtReceive.Text) - decimal.Parse(txtTotalCheck.Text)).ToString();
                txtBackDollar.Text = "0";
                textBox1.Text = "0";
            }
            
        }

        private void txtBackDollar_Leave(object sender, EventArgs e)
        {
            if(txtBackDollar.Text=="0")
            {
                textBox1.Text = "0";
            }
            else
            {
                txtBack.Text = "0";
                txtReceive.Text = "0";
                textBox1.Text = (decimal.Parse(txtBackDollar.Text) - decimal.Parse(txtTotalDollar.Text)).ToString();
            }
           
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
           //if(decimal.Parse(txtReceive.Text) < decimal.Parse(txtTotalCheck.Text) && decimal.Parse(txtBackDollar.Text) < decimal.Parse(txtTotalDollar.Text))
           // {
           //     MessageBox.Show("Money is not enough");
           // }
           // else
           // {
                string invID = "";
                string s = "";
                try
                {
                    var cn = new System.Data.SqlClient.SqlConnection(DataBase.getConnectionString());
                    if (cn.State == ConnectionState.Closed) cn.Open();
                    var t = cn.BeginTransaction();
                    DataBase.InsertByRunSP(cn, "insert_Inv", "@UserID:@InvDate:@invTime:@Amount:@discount:@payment", ref invID, "@id", t,
                        DataBase.User,
                        DateTime.Today.ToShortDateString(),
                        DateTime.Now.ToShortTimeString(),
                        sell.txtTotal.Text,
                        sell.txtDiscount.Text,
                        sell.txtPay.Text
                        );

                    for (int i = 0; i < sell.dataGridView1.Rows.Count; i++)
                    {
                        DataBase.InsertByRunSP(cn, "sp_insert_inv_detail", "@InvID:@ProID:@ProName:@Qty:@Price:@Total:@size:@decount", ref s, null, t,
                        invID,
                       sell.dataGridView1.Rows[i].Cells["Column5"].Value.ToString(),
                       sell.dataGridView1.Rows[i].Cells["Column1"].Value.ToString(),
                       sell.dataGridView1.Rows[i].Cells["Column2"].Value.ToString(),
                       sell.dataGridView1.Rows[i].Cells["Column3"].Value.ToString(),
                       sell.dataGridView1.Rows[i].Cells["Column4"].Value.ToString(),
                       sell.dataGridView1.Rows[i].Cells["Column6"].Value.ToString(),
                       sell.dataGridView1.Rows[i].Cells["Column7"].Value.ToString()
                       );
                    }
                    t.Commit();
                    sell.txtTotal.Text = "0";
                    sell.txtDiscount.Text = "0";
                    sell.txtPay.Text = "0";
                    sell.txtpayDoller.Text = "0";
                    sell.txtTotalDollar.Text = "0";
                    sell.dataGridView1.Rows.Clear();
                  
                    print(invID);
                 this.Close();





            }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
           // }
            //
        }

        public void print(string invID)
        {
           int number= DataBase.Waiting += 1;
            string outputValue = String.Format("{0:D3}", number);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Recieve", txtReceive.Text);
            dic.Add("RecievD", txtBackDollar.Text);
            dic.Add("Back", txtBack.Text);
            dic.Add("BackD", textBox1.Text);
            dic.Add("Waiting", outputValue);
            dic.Add("Wifi",DataBase.PassWifi);
            if(int.Parse(outputValue)==100)
            {
                DataBase.Waiting = 0;
            }

            List<DataTable> dt = new List<DataTable>();
            List<string> datasetname = new List<string>();
            dt.Add(DataBase.GetDataFromDatase(null, "sp_invoice '" + invID + "'").Tables[0]);
            datasetname.Add("DataSet1");
            string path = System.IO.Directory.GetCurrentDirectory();
            path = string.Format("{0}\\{1}\\{2}", path, "Report", "rptReciep.rdlc");
            vReport.ViewReport(path, dt, datasetname, dic,"a");
            
        }

        private void txtBackDollar_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataBase.numberOnly(sender, e);
        }
    }
}
