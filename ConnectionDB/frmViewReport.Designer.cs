namespace app.report
{
    partial class frmViewReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmViewReport));
            this.CW = new AxCrystalActiveXReportViewerLib105.AxCrystalActiveXReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this.CW)).BeginInit();
            this.SuspendLayout();
            // 
            // CW
            // 
            this.CW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CW.Enabled = true;
            this.CW.Location = new System.Drawing.Point(0, 0);
            this.CW.Name = "CW";
            this.CW.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("CW.OcxState")));
            this.CW.Size = new System.Drawing.Size(864, 349);
            this.CW.TabIndex = 0;
            // 
            // frmViewReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 349);
            this.Controls.Add(this.CW);
            this.Name = "frmViewReport";
            this.Text = "frmViewReport";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.CW)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public AxCrystalActiveXReportViewerLib105.AxCrystalActiveXReportViewer CW;



    }
}