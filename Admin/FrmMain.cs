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
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }



        private void addProductToolStripMenuItem_Click(object sender, EventArgs e)
        {

            FrmAddProduct product = new FrmAddProduct();
            product.MdiParent = this;
            product.StartPosition = FormStartPosition.CenterScreen;
            product.BringToFront();
            product.Show();
        }

        private void addCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAddCategory cate = new FrmAddCategory();
            cate.MdiParent = this;
            cate.StartPosition = FormStartPosition.CenterScreen;
            cate.BringToFront();
            cate.Show();
        }

        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void addUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAddUser user = new FrmAddUser();
            user.MdiParent = this;
            user.StartPosition = FormStartPosition.CenterScreen;
            user.BringToFront();
            user.Show();
        }

        private void listUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
           FrmListUser list = new FrmListUser();
            list.MdiParent = this;
            list.StartPosition = FormStartPosition.CenterScreen;
            list.BringToFront();
            list.Show();
        }

        private void sellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmRptSell sell = new FrmRptSell();
            sell.MdiParent = this;
            sell.StartPosition = FormStartPosition.CenterScreen;
            sell.BringToFront();
            sell.Show();
        }

        private void productToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmRptProduct pro = new FrmRptProduct();
            pro.MdiParent = this;
            pro.StartPosition = FormStartPosition.CenterScreen;
            pro.BringToFront();
            pro.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ករកណតToolStripMenuItem_Click(object sender, EventArgs e)
        {
      
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
          
        }
    }
}
