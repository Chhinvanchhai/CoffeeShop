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
using System.Globalization;

namespace Admin
{
    public partial class FrmAddProduct : Form
    {
        public FrmAddProduct()
        {
            InitializeComponent();
        }
        
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void FrmAddProduct_Load(object sender, EventArgs e)
        {
            cboSize.Items.AddRange(new object[] { "L", "M", "S" });
            cboSize.SelectedIndex = 0;
            cbCategory.DataSource = DataBase.GetDataFromDatase(null, "SELECT CatID,CatName FROM dbo.tblCat").Tables[0];
            cbCategory.DisplayMember = "CatName";
            cbCategory.ValueMember = "CatID";
            getDataToDataGrid();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        string fileName = "";
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Choose Image";
            open.Filter = "Image Files|*.jpg;*.png;*.gif;*.bmp;*.jpeg|All Files|*.*";

            if(open.ShowDialog() == DialogResult.OK)
            {
                fileName = open.FileName.ToString();
                pictureBox1.Load(open.FileName);
            }
            
        
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add(txtNameProduct.Text, cboSize.Text, textBox1.Text,txtDiscount.Text,txtSol.Text);
            DataBase.Clear(textBox1,txtSol);
            txtDiscount.Text = "0";
            getDataToDataGrid();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //DataBase.ImageToByte(pictureBox1.Image, pictureBox1.Image.RawFormat),
            string proid = "";
            string s = "";
            try
            {
                if(btnSave.Tag.ToString()=="1")
                {
                    var cn = new System.Data.SqlClient.SqlConnection(DataBase.getConnectionString());
                    if (cn.State == ConnectionState.Closed) cn.Open();
                    var t = cn.BeginTransaction();
                    DataBase.InsertByRunSP(cn, "sp_product_i", "@proname:@photo:@cateid:@note:@createdby", ref proid, "@id", t,
                        txtNameProduct.Text,
                        DataBase.ImageToByte(pictureBox1.Image, pictureBox1.Image.RawFormat),
                        cbCategory.SelectedValue.ToString(),
                        txtDes.Text,
                        DataBase.User
                        );

                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        DataBase.InsertByRunSP(cn, "sp_product_detail_i", "@pid:@type:@price:@discount", ref s, null, t,
                        proid,
                       dataGridView1.Rows[i].Cells[1].Value.ToString(),
                       dataGridView1.Rows[i].Cells[2].Value.ToString(),
                       dataGridView1.Rows[i].Cells[3].Value.ToString()
                       );
                    }
                    t.Commit();
                    MessageBox.Show("Save Success.");
                }
                else
                {
                    var cn = new System.Data.SqlClient.SqlConnection(DataBase.getConnectionString());
                    if (cn.State == ConnectionState.Closed) cn.Open();
                    var t = cn.BeginTransaction();
                    DataBase.InsertByRunSP(cn, "sp_product_e", "@proname:@photo:@cateid:@note:@createdby:@id", ref proid, null, t,
                        txtNameProduct.Text,
                        DataBase.ImageToByte(pictureBox1.Image, pictureBox1.Image.RawFormat),
                        cbCategory.SelectedValue.ToString(),
                        txtDes.Text,
                        DataBase.User,
                        txtProductID.Text
                        );

                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        DataBase.InsertByRunSP(cn, "sp_product_detail_i", "@pid:@type:@price:@discount", ref s, null, t,
                        txtProductID.Text,
                       dataGridView1.Rows[i].Cells[1].Value.ToString(),
                       dataGridView1.Rows[i].Cells[2].Value.ToString(),
                       dataGridView1.Rows[i].Cells[3].Value.ToString()
                       );
                    }
                    t.Commit();
                    btnSave.Tag = "1";
                    MessageBox.Show("Update Success.");
                    
                }
                DataBase.Clear(txtDes, txtNameProduct, txtProductID, txtSol);
                txtDiscount.Text = "0";
                dataGridView1.Rows.Clear();
                getDataToDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        DataTable dt ;
        public void getDataToDataGrid()
        {
            dt = new DataTable();
            dt = DataBase.GetDataFromDatase(null, " select p.ProID AS N'លេចសម្គាល់',p.ProName AS N'ឈ្មោះ',p.Price AS N'តម្លៃ' ,c.CatName AS N'ប្រភេទ',p.CatID ,Photo ,note as 'សម្កាល់' from tblProduct p "+
                                 "inner join tblCat c on p.CatID = c.CatID order by ProID Asc").Tables[0];
            dataGridView2.DataSource = dt;
            dataGridView2.Columns[4].Visible = false;
            dataGridView2.Columns[5].Visible = false;

            //dt = new DataTable();
            //dt = DataBase.GetDataFromDatase(null, "select ProID , ProName , Price ,CatName​ , tblProduct.CatID,Photo ,note  from tblProduct" +
            //                    " inner join tblCat on tblProduct.CatID=tblCat.CatID order by ProID Asc").Tables[0];
            //dataGridView2.DataSource = dt;
            //dataGridView2.Columns[4].Visible = false;
            //dataGridView2.Columns[5].Visible = false;
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            btnSave.Tag = "2";
            Byte[] s = new Byte[0];
            s = (byte[])dataGridView2.SelectedRows[0].Cells[5].Value;
            MemoryStream MS = new MemoryStream(s);
            pictureBox1.Image = Image.FromStream(MS);

            txtProductID.Text = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
            txtNameProduct.Text = dataGridView2.SelectedRows[0].Cells[1].Value.ToString();
            txtDes.Text = dataGridView2.SelectedRows[0].Cells[6].Value.ToString();
            cbCategory.SelectedValue = dataGridView2.SelectedRows[0].Cells[4].Value.ToString();

            dt = new DataTable();
            dt = DataBase.GetDataFromDatase(null, "select * from tblProductDetail where proid='"+ txtProductID.Text + "'").Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dataGridView1.Rows.Add(
                        txtNameProduct.Text,
                        dt.Rows[i]["type"].ToString(),
                        dt.Rows[i]["price"].ToString(),
                        dt.Rows[i]["discount"].ToString(),
                        dt.Rows[i]["total"].ToString()

                    );
            }
            tabControl1.SelectedIndex = 0;
        }
        string discount = "";
        string prices = "";
        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
             discount= txtDiscount.Text==""?"0" : txtDiscount.Text;
             prices= textBox1.Text == "" ? "0" : textBox1.Text;
            txtSol.Text = (decimal.Parse(prices) - (decimal.Parse(prices) * (decimal.Parse(discount)) / 100)).ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            discount = txtDiscount.Text == "" ? "0" : txtDiscount.Text;
            prices = textBox1.Text == "" ? "0" : textBox1.Text;
            txtSol.Text = (decimal.Parse(prices) - (decimal.Parse(prices) * (decimal.Parse(discount)) / 100)).ToString();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                txtDiscount.Focus();
            }
        }

        private void txtDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataBase.numberOnly(sender, e);
            if(e.KeyChar==13)
            {
                txtDiscount_TextChanged(sender, null);
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            CultureInfo TypeOfLanguage = CultureInfo.CreateSpecificCulture("en-US");
            System.Threading.Thread.CurrentThread.CurrentCulture = TypeOfLanguage;
            InputLanguage l = InputLanguage.FromCulture(TypeOfLanguage);
            InputLanguage.CurrentInputLanguage = l;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
           DataBase.numberOnly(sender, e);
        }
      
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string delete = "";
            delete = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
            if (MessageBox.Show("Do you want to delete this product", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                DataBase.runSQL(null, "delete from dbo.tblProduct where ProID='" + delete + "'", null);
                MessageBox.Show("Delete sucessfull.");
                getDataToDataGrid();
            }
           
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            for(int i=0;i<dataGridView1.Rows.Count;i++)
            {
                try
                {
                    dataGridView1.Rows[i].Cells[4].Value = (decimal.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString()) - (decimal.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString()) * decimal.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString()) / 100)).ToString();
                }
                catch (Exception)
                {

                    MessageBox.Show("Please Insert only number");
                }
                
            }
        }
    }
}
