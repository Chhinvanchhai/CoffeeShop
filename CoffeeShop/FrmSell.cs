using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ConnectionDB;
using System.IO;

namespace CoffeeShop
{
    public partial class FrmSell : Form
    {
        public FrmSell()
        {
            InitializeComponent();
        }
        DataTable tbCate = new DataTable();
        DataTable tbPro = new DataTable();
        DataSet ds = new DataSet();
        DataSet dsAll = new DataSet();
        private void FrmSell_Load(object sender, EventArgs e)
        {
            ds.Clear();
            ds = DataBase.GetDataFromDatase(null, "SELECT * FROM dbo.tblProduct");
            dsAll = ds;
            readCategory();
            lblRate.Text = DataBase.GetScalrValue(null, "select rate FROM tblExchange");
            lblUserName.Text ="អ្នកប្រើប្រាស់ : "+ DataBase.UserName;
           
        }

        public void readCategory()
        {
            Button btn;
            tbCate = DataBase.GetDataFromDatase(null, "Select * from tblCat where isnull(lock,0)=0").Tables[0];
            for (int i = 0; i < tbCate.Rows.Count; i++)
            {
                btn = new Button();
                btn.Text = tbCate.Rows[i]["CatName"].ToString();
                btn.ForeColor = Color.Green;
                btn.BackColor = Color.White;
                btn.Tag = tbCate.Rows[i]["CatID"].ToString();
                btn.Size = new System.Drawing.Size(120, 45);
                btn.Click += Btn_Click;
                flowLayoutPanelCate.Controls.Add(btn);
            }
            readProduct(tbCate.Rows[0]["CatID"].ToString());


        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Button sennder=(Button)sender;
            string cateID = sennder.Tag.ToString();
            readProduct(cateID);


        }
        public void readProduct(string cateID)
        {
            flowLayoutPanelPro.Controls.Clear();
            ds.Tables[0].DefaultView.RowFilter = "CatID = '"+cateID+"'";
            tbPro = (ds.Tables[0].DefaultView).ToTable(); ;//DataBase.GetDataFromDatase(null, "SELECT * FROM dbo.tblProduct where CatID='" + cateID + "'").Tables[0];
            for (int i = 0; i < tbPro.Rows.Count; i++)
            {
                Button btnProduct;
                btnProduct = new Button();
                btnProduct.Tag = tbPro.Rows[i]["ProID"].ToString();
                btnProduct.Text = tbPro.Rows[i]["ProName"].ToString();
                Byte[] s = new Byte[0];
                s = (byte[])tbPro.Rows[i]["photo"];
                MemoryStream MS = new MemoryStream(s);
               
                btnProduct.Image = (Image)(new Bitmap(Image.FromStream(MS), new Size(128, 128)));//Image.FromStream(MS);
                btnProduct.ForeColor = Color.Green;
                btnProduct.BackColor = Color.White;
                btnProduct.TextImageRelation = TextImageRelation.ImageAboveText;
                btnProduct.Margin = new Padding(15);
                btnProduct.FlatAppearance.BorderSize = 0;
                btnProduct.BackgroundImageLayout = ImageLayout.Zoom;
                btnProduct.Size = new System.Drawing.Size(146, 186);
                btnProduct.Click += BtnProduct_Click;
                flowLayoutPanelPro.Controls.Add(btnProduct);
            }
            ds = dsAll;

        }
        DataTable tdD;
        private void BtnProduct_Click(object sender, EventArgs e)
        {
            Button btnSize = (Button)sender;
            FrmSize size = new FrmSize(this);
             tdD = DataBase.GetDataFromDatase(null, "select * from tblProductDetail where proid='" + btnSize.Tag.ToString() + "'").Tables[0];
            if(tdD.Rows.Count ==1)
            {
                if (dataGridView1.Rows.Count == 0)
                {
                      dataGridView1.Rows.Add(
                      btnSize.Tag.ToString(),
                      btnSize.Text.ToString(),
                      "1",
                      tdD.Rows[0]["type"].ToString(),
                      tdD.Rows[0]["price"].ToString(),
                      tdD.Rows[0]["discount"].ToString(),
                      tdD.Rows[0]["total"].ToString());
                }
                else
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value.ToString() == btnSize.Tag.ToString() && dataGridView1.Rows[i].Cells[3].Value.ToString() == tdD.Rows[0]["type"].ToString())
                        {
                            dataGridView1.Rows[i].Cells[2].Value = (int.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString()) + int.Parse("1")).ToString();
                            dataGridView1.Rows[i].Cells[6].Value = ((int.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString())) * decimal.Parse(tdD.Rows[0]["total"].ToString()));
                            sumInvoice();
                            return;
                        }
                    }
                     dataGridView1.Rows.Add(
                      btnSize.Tag.ToString(),
                      btnSize.Text.ToString(),
                      "1",
                      tdD.Rows[0]["type"].ToString(),
                      tdD.Rows[0]["price"].ToString(),
                      tdD.Rows[0]["discount"].ToString(),
                      tdD.Rows[0]["total"].ToString());
                }
                sumInvoice();
            }
            else
            {
                size.proID = btnSize.Tag.ToString();
                size.proname = btnSize.Text.ToString();
                size.ShowDialog();
            }
            
        }
        decimal total = 0;
        public void sumInvoice()
        {

            total = 0;
            decimal number;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                total += decimal.Parse(dataGridView1.Rows[i].Cells[6].Value.ToString());
            }
            txtTotal.Text = total.ToString();
            Console.Write(txtTotal.Text);
            Decimal.TryParse(lblRate.Text, out number);
            if(number > 0)
            {
                txtTotalDollar.Text =(total / number ).ToString();
            } else
            {
                txtTotalDollar.Text = total.ToString() ;
            }
            txtPay.Text = (decimal.Parse(txtTotal.Text) - (decimal.Parse(txtDiscount.Text) * decimal.Parse(txtTotal.Text)) / 100).ToString();
            txtpayDoller.Text = (decimal.Parse(txtPay.Text) / decimal.Parse(lblRate.Text)).ToString();

            // return total;
        }

        private void txtDiscount_MouseLeave(object sender, EventArgs e)
        {
            txtPay.Text = (decimal.Parse(txtTotal.Text) - (decimal.Parse(txtDiscount.Text) * decimal.Parse(txtTotal.Text)) / 100).ToString();
            txtpayDoller.Text = (decimal.Parse(txtPay.Text) / decimal.Parse(lblRate.Text)).ToString();
        }

        private void flowLayoutPanelPro_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            if(dataGridView1.Rows.Count>0)
            {
                FrmGetMoney getmoney = new FrmGetMoney(this);
                getmoney.total = txtPay.Text;
                getmoney.totalDollar = txtTotalDollar.Text;
                getmoney.ShowDialog();
            }
           
        }

        private void txtDiscount_Leave(object sender, EventArgs e)
        {
            txtPay.Text = (decimal.Parse(txtTotal.Text) - (decimal.Parse(txtDiscount.Text) * decimal.Parse(txtTotal.Text)) / 100).ToString();
            txtpayDoller.Text = (decimal.Parse(txtPay.Text) / decimal.Parse(lblRate.Text)).ToString();
        }

        private void FrmSell_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Do you want to check out", "Check Out", MessageBoxButtons.YesNo,MessageBoxIcon.Information)==DialogResult.Yes)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("From", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                dic.Add("To", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                dic.Add("User", DataBase.User);
                List<DataTable> dt = new List<DataTable>();
                List<string> datasetname = new List<string>();
                dt.Add(DataBase.GetDataFromDatase(null, "sp_rpt_daily_checkout '" + DateTime.Now.ToShortDateString() + "'" + "," + "'" + DateTime.Now.ToShortDateString() + "'").Tables[0]);
                dt.Add(DataBase.GetDataFromDatase(null, "sp_rpt_expense '" + DateTime.Now.ToShortDateString() + "'" + "," + "'" + DateTime.Now.ToShortDateString() + "'").Tables[0]);
                datasetname.Add("DsOut");
                datasetname.Add("DataSetEx");
                string path = System.IO.Directory.GetCurrentDirectory();
                path = string.Format("{0}\\{1}\\{2}", path, "Report", "RptCheckOut.rdlc");
                vReport.ViewReport(path, dt, datasetname, dic,"s");
            }

        }

        private void btnExpens_Click(object sender, EventArgs e)
        {
            FrmExpens ex =new FrmExpens();
            ex.ShowDialog();
        }
        FrmLogin l = new FrmLogin();
        private void btnCloseTime_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to change time", "Change Time", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("From", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                dic.Add("To", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                List<DataTable> dt = new List<DataTable>();
                List<string> datasetname = new List<string>();
                dt.Add(DataBase.GetDataFromDatase(null, "sp_rpt_change_time '" + DateTime.Now.ToShortDateString() + "'" + "," + "'" + DateTime.Now.ToShortDateString() + "'"+"," +"'"+DataBase.User+"'").Tables[0]);
                dt.Add(DataBase.GetDataFromDatase(null, "sp_rpt_expense_user'" + DateTime.Now.ToShortDateString() + "'" + "," + "'" + DateTime.Now.ToShortDateString() +"'"+","+"'"+DataBase.User+"'" ).Tables[0]);
                datasetname.Add("DsCheckoutDaily");
                datasetname.Add("DataSet1");
                string path = System.IO.Directory.GetCurrentDirectory();
                path = string.Format("{0}\\{1}\\{2}", path, "Report", "RptDailyCheckoutTime.rdlc");
                vReport.ViewReport(path, dt, datasetname, dic,"s");
            }

            this.Hide();
            l.ShowDialog();
           
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.RemoveAt(item.Index);
            }
            sumInvoice();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
            {
                dataGridView1.Rows[item.Index].Cells[2].Value = (int.Parse(dataGridView1.Rows[item.Index].Cells[2].Value.ToString()) + int.Parse("1")).ToString();
                dataGridView1.Rows[item.Index].Cells[6].Value = ((int.Parse(dataGridView1.Rows[item.Index].Cells[2].Value.ToString())) * decimal.Parse(dataGridView1.Rows[item.Index].Cells[4].Value.ToString()));
                sumInvoice();
            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
            {
                
                    dataGridView1.Rows[item.Index].Cells[2].Value = (int.Parse(dataGridView1.Rows[item.Index].Cells[2].Value.ToString()) - int.Parse("1")).ToString();
                    dataGridView1.Rows[item.Index].Cells[6].Value = ((int.Parse(dataGridView1.Rows[item.Index].Cells[2].Value.ToString())) * decimal.Parse(dataGridView1.Rows[item.Index].Cells[4].Value.ToString()));
                    sumInvoice();
                    if (dataGridView1.Rows[item.Index].Cells[2].Value.ToString() == "0")
                    {
                        dataGridView1.Rows.RemoveAt(item.Index);
                    }
               
                
            }
        }

        private void btnWifi_Click(object sender, EventArgs e)
        {
            FrmWifi wi = new FrmWifi();
            wi.ShowDialog();
        }

        private void btnNumberPrint_Click(object sender, EventArgs e)
        {
            FrmNumberOfPrint print = new FrmNumberOfPrint();
            print.ShowDialog();
            //MessageBox.Show(DataBase.PassWifi);
        }

        private void btnChangePass_Click(object sender, EventArgs e)
        {
            FrmChangPass pass = new FrmChangPass();
            pass.ShowDialog();
        }

        private void flowLayoutPanelPro_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}
