using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace app.report
{
    public partial class ReportMicrosoftViewer : Form
    {
        public Microsoft.Reporting.WinForms.ReportViewer rptViewer;

        public ReportMicrosoftViewer()
        {
            InitializeComponent();
        }

        private void ReportMicrosoftViewer_Load(object sender, EventArgs e)
        {

            this.rptViewer.RefreshReport();
        }

        private void InitializeComponent()
        {
            this.rptViewer = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // rptViewer
            // 
            this.rptViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rptViewer.Location = new System.Drawing.Point(0, 0);
            this.rptViewer.Name = "rptViewer";
            this.rptViewer.Size = new System.Drawing.Size(447, 413);
            this.rptViewer.TabIndex = 0;
            // 
            // ReportMicrosoftViewer
            // 
            this.ClientSize = new System.Drawing.Size(447, 413);
            this.Controls.Add(this.rptViewer);
            this.Name = "ReportMicrosoftViewer";
            this.Load += new System.EventHandler(this.ReportMicrosoftViewer_Load_1);
            this.ResumeLayout(false);

        }

        private void ReportMicrosoftViewer_Load_1(object sender, EventArgs e)
        {

            this.rptViewer.RefreshReport();
        }
    }
}
