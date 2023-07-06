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
    public partial class FrmSize : Form
    {
        public FrmSize(FrmSell sell)
        {
            InitializeComponent();
            this.sell = sell;
        }
        private FrmSell sell;
        public string proID { get; set; }
        public string proname { get; set; }
        string discount = "";
      
        DataTable tdD = new DataTable();
        string size = "";
        private void FrmSize_Load(object sender, EventArgs e)
        {
            tdD = DataBase.GetDataFromDatase(null, "select * from tblProductDetail where proid='"+proID+"'").Tables[0];

            ButtonClick btn;
         
            for (int i = 0; i < tdD.Rows.Count; i++)
            {
                btn = new ButtonClick();
                btn.Text = tdD.Rows[i]["type"].ToString()+"\n"+ tdD.Rows[i]["price"].ToString();
                btn.Name = tdD.Rows[i]["type"].ToString();
                btn.Discount= tdD.Rows[i]["discount"].ToString();
                //btn.ForeColor = Color.Green;
                //btn.BackColor = Color.White;
                btn.Tag = tdD.Rows[i]["total"].ToString();
                btn.Price= tdD.Rows[i]["price"].ToString();
                btn.Size = new System.Drawing.Size(260, 157);
                btn.Click += Btn_Click; ;
                flowLayoutPanel1.Controls.Add(btn);
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            ButtonClick btnPrice = (ButtonClick)sender;
            string price = btnPrice.Tag.ToString();
            size = btnPrice.Name;
            if(sell.dataGridView1.Rows.Count==0)
            {
                sell.dataGridView1.Rows.Add(
                  proID,
                  proname,
                  txtQty.Text,
                  size,
                  btnPrice.Price,
                  btnPrice.Discount,
                  decimal.Parse(txtQty.Text) * decimal.Parse(price));
            }
            else
            {
                for (int i = 0; i < sell.dataGridView1.Rows.Count; i++)
                {
                    if (sell.dataGridView1.Rows[i].Cells[0].Value.ToString() == proID && sell.dataGridView1.Rows[i].Cells[3].Value.ToString()==size)
                    {
                        sell.dataGridView1.Rows[i].Cells[2].Value = (int.Parse(sell.dataGridView1.Rows[i].Cells[2].Value.ToString()) + int.Parse(txtQty.Text)).ToString();
                        sell.dataGridView1.Rows[i].Cells[6].Value = ((int.Parse(sell.dataGridView1.Rows[i].Cells[2].Value.ToString())) * decimal.Parse(price));
                        sell.sumInvoice();
                        return;
                    }
                }
                sell.dataGridView1.Rows.Add(
                       proID,
                       proname,
                       txtQty.Text,
                       size,
                      btnPrice.Price,
                        btnPrice.Discount,
                       decimal.Parse(txtQty.Text) * decimal.Parse(price));
            }
            sell.sumInvoice();
           
        }

        private void FrmSize_MouseLeave(object sender, EventArgs e)
        {
            
        }
    }
}
