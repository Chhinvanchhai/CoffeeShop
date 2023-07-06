using ConnectionDB;
using Microsoft.Reporting.WinForms;
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
    public partial class FrmRptProduct : Form
    {
        public FrmRptProduct()
        {
            InitializeComponent();
        }

        private void FrmRptProduct_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
           // this.reportViewer2.RefreshReport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            reportViewer1.LocalReport.DataSources.Clear();
            List<DataTable> dt = new List<DataTable>();
            List<string> datasetname = new List<string>();
            dt.Add(DataBase.GetDataFromDatase(null, "sp_rpt_product '" + dateTimePickerFrom.Value.ToShortDateString() + "'" + "," + "'" + dateTimePickerTo.Value.ToShortDateString() + "'").Tables[0]);
            datasetname.Add("DsProduct");
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource(datasetname[0], dt[0]));
            string path = System.IO.Directory.GetCurrentDirectory();
            path = string.Format("{0}\\{1}\\{2}", path, "Report", "RptProduct.rdlc");
            reportViewer1.LocalReport.ReportPath = path;
            ReportParameter[] param = new ReportParameter[2];
            param[0] = new ReportParameter("From", dateTimePickerFrom.Value.ToShortDateString());
            param[1] = new ReportParameter("To", dateTimePickerTo.Value.ToShortDateString());
            reportViewer1.LocalReport.SetParameters(param);


            reportViewer1.RefreshReport();
        }
    }
}
