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
    public partial class FrmAddCategory : Form
    {
        public FrmAddCategory()
        {
            InitializeComponent();
        }
        int update = 1;
        string delete = "";
        string id="";
        DataTable tb;
        private void btnClose_Click(object sender, EventArgs e)
        {
           // Application.Exit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(update==1)
                {
                    string s = "";
                    DataBase.InsertByRunSP(null, "sp_cate", "@name:@dec:@createby", ref s, null, null, txtType.Text, txtDes.Text, DataBase.User);
                    MessageBox.Show("Save Sucessfull");
                    reloadDatagrid();
                }
                else
                {
                    string s = "";
                    DataBase.InsertByRunSP(null, "sp_cate_e", "@name:@dec:@createby:@id", ref s, null, null, txtType.Text, txtDes.Text, "",id);
                    MessageBox.Show("Update Sucessfull");
                    reloadDatagrid();
                    update = 1;
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }

        private void FrmAddCategory_Load(object sender, EventArgs e)
        {
            reloadDatagrid();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtType.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            txtDes.Text= dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            id= dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            update = 2;
        }
        public void reloadDatagrid()
        {
             tb = new DataTable();
            tb = DataBase.GetDataFromDatase(null, "SELECT  CatID AS 'លេខកូត',CatName AS 'ឈ្មោះប្រភេទ',description AS 'ពណ៍នា',createdate AS 'ថ្ងៃបង្កើត' FROM dbo.tblCat where isnull(lock,0)=0").Tables[0];
            dataGridView1.DataSource = tb;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Do you want to delete this item?","Delet",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)==DialogResult.Yes)
            {
                delete = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                tb = new DataTable();
                DataBase.runSQL(null, "update  dbo.tblCat set lock=1 WHERE CatID='" + delete + "'",null);
                MessageBox.Show("Delete sucessfull.");
                reloadDatagrid();
                //tb = DataBase.GetDataFromDatase(null, "SELECT * FROM dbo.tblProduct where ProID='" + delete + "'").Tables[0];
                //if(tb.Rows.Count>0)
                //{
                //    MessageBox.Show("1");
                //}
                //else
                //{
                //    DataBase.runSQL(null, "delete from dbo.tblProduct where ProID='" + delete + "'",null);
                //}

            }
        }
    }
}
