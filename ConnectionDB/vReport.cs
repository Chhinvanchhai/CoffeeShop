using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Diagnostics;
using Microsoft.Reporting.WinForms;
using app.report;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.Drawing;

namespace ConnectionDB
{
    public class vReport
    {
        public static SqlConnection cnn;
        //  public static string constr = "Data source=SNA-PC\\SNA;Database=MIS;User Id=sa;password=121194";
        public static System.Data.DataTable GetDataTable(string storprocedureName, DataGridView dgvValue, DataGridView dgvcondition)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            //if (dgvValue.Rows.Count > 0)
            //{
            Makeconnection();
            SqlCommand cmd = new SqlCommand(storprocedureName, cnn);
            SqlDataAdapter adabter;
            cmd.CommandType = CommandType.StoredProcedure;
            SqlCommandBuilder.DeriveParameters(cmd);

            for (int i = 0; i <= dgvValue.Rows.Count - 1; i++)
            {
                string conf = Readparamet(storprocedureName, dgvValue.Rows[i].Cells[0].Value.ToString());
                if (dgvValue.Rows[i].Cells[1].Value == null) { dgvValue.Rows[i].Cells[1].Value = ""; }
                string value = dgvValue.Rows[i].Cells[1].Value.ToString();
                cmd.Parameters.RemoveAt(conf);

                //cmd.Parameters.AddWithValue(conf,"N"+"\'"+ value.ToString() +"\'");

                cmd.Parameters.AddWithValue(conf, value);

            }
            for (int k = 1; k <= cmd.Parameters.Count - 1; k++)
            {
                if (cmd.Parameters[k].Value == null)
                {
                    string comp = cmd.Parameters[k].ParameterName;
                    cmd.Parameters.RemoveAt(comp);
                    cmd.Parameters.AddWithValue(comp, "");
                }
            }
            cmd.ExecuteNonQuery();
            adabter = new SqlDataAdapter(cmd);
            adabter.Fill(table);
            // }
            return table;
        }

        public static void ShowMicrosoftReportwithNamespec(string storprocedureName, DataGridView dgvValue, DataGridView dgvcondition,
            ReportViewer rptviewer, Form FormReportviewer, string DatasetName, string Namespace, string ReportName)
        {
            System.Data.DataTable Data = GetDataTable(storprocedureName, dgvValue, dgvcondition);
            rptviewer.LocalReport.DataSources.Clear();
            ReportDataSource rds = new ReportDataSource(DatasetName, Data);
            string source = string.Format("{0}.{1}.rdlc", Namespace, ReportName);
            rptviewer.LocalReport.ReportEmbeddedResource = source;
            rptviewer.LocalReport.DataSources.Add(rds);
            rptviewer.RefreshReport();
            FormReportviewer.Show();
        }
        public static void ViewReport(System.Data.DataTable tb,
            string DatasetName, string path)
        {

            ReportMicrosoftViewer f = new ReportMicrosoftViewer();

            f.rptViewer.LocalReport.DataSources.Clear();
            ReportDataSource rds = new ReportDataSource(DatasetName, tb);
            // string source = string.Format("{0}.{1}.rdlc", Namespace, ReportName);
            f.rptViewer.LocalReport.ReportPath = path;
            f.rptViewer.LocalReport.DataSources.Add(rds);
            f.rptViewer.RefreshReport();
            f.Show();
        }

        private static int m_currentPageIndex;
        private static IList<Stream> m_streams;
        private static Stream CreateStream(string name,
        string fileNameExtension, Encoding encoding,
        string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }
        // Export the given report as an EMF (Enhanced Metafile) file.
        public static void Export(LocalReport report)
        {
            string deviceInfo =
              @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>3in</PageWidth>
                <PageHeight>11in</PageHeight>
                <MarginTop>0.03in</MarginTop>
                <MarginLeft>0.03in</MarginLeft>
                <MarginRight>0.03in</MarginRight>
                <MarginBottom>0.03in</MarginBottom>
            </DeviceInfo>";
            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream,
               out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;
        }
        // Handler for PrintPageEvents
        private static void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new
               Metafile(m_streams[m_currentPageIndex]);

            // Adjust rectangular area with printer margins.
            System.Drawing.Rectangle adjustedRect = new System.Drawing.Rectangle(
                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                ev.PageBounds.Width,
                ev.PageBounds.Height);

            // Draw a white background for the report
            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

            // Draw the report content
            ev.Graphics.DrawImage(pageImage, adjustedRect);

            // Prepare for the next page. Make sure we haven't hit the end.
             m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

      
        public static void Prints()
        {
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();

            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                m_currentPageIndex = 0;
                printDoc.Print();
            }
        }
        public void Dispose()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }

        public static void ViewReport(string path, List<System.Data.DataTable> tb,
          List<string> DatasetName, Dictionary<string, object> dic,string print="a")
        {

            try
            {
                ReportMicrosoftViewer f = new ReportMicrosoftViewer();
                
                LocalReport report = new LocalReport();
          
                report.DataSources.Clear();

                f.rptViewer.LocalReport.DataSources.Clear();
                for (int i = 0; i < tb.Count; i++)
                {
                    f.rptViewer.LocalReport.DataSources.Add(new ReportDataSource(DatasetName[i], tb[i]));
                    report.DataSources.Add(new ReportDataSource(DatasetName[i], tb[i]));
                }

                // string source = string.Format("{0}.{1}.rdlc", Namespace, ReportName);
                f.rptViewer.LocalReport.ReportPath = path;
                report.ReportPath = path;
                if (dic.Count > 0)
                {
                    foreach (KeyValuePair<string, object> p in dic)
                    {
                        ReportParameter param = new ReportParameter(p.Key.ToString(), p.Value.ToString());
                        f.rptViewer.LocalReport.SetParameters(param);
                        report.SetParameters(param);
                    }
                }
                //f.rptViewer.LocalReport.ReportPath = path;
                //report.ReportPath = path;
                //f.rptViewer.RefreshReport();
                // report.Refresh();
                if(print == "a")
                {
                    // f.Show();
                    for (int i = 0; i < DataBase.NumOfPrint; i++)
                    {
                        Export(report);
                        Prints();
                    }
                }
                else
                {
                    f.Show();
                }
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void Export(LocalReport report, string paper = "R")
        {
            string deviceInfo = FormatPaper(paper);
            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream,
               out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;
        }
      
public static string FormatPaper(string paper)
        {

            if (paper == "A4")
            {
                return @"<DeviceInfo>
                            <OutputFormat>EMF</OutputFormat>
                            <PageWidth>8.5in</PageWidth>
                            <PageHeight>11in</PageHeight>
                            <MarginTop>0.5in</MarginTop>
                            <MarginLeft>1.5in</MarginLeft>
                            <MarginRight>0.5in</MarginRight>
                            <MarginBottom>0.5in</MarginBottom>
                        </DeviceInfo>";
            }

            else if (paper == "R")
            {
                return @"<DeviceInfo>
                            <OutputFormat>EMF</OutputFormat>
                            <PageWidth>4.13in</PageWidth>
                            <PageHeight>5.83in</PageHeight>
                            <MarginTop>0.1in</MarginTop>
                            <MarginLeft>0.1in</MarginLeft>
                            <MarginRight>0.1in</MarginRight>
                            <MarginBottom>0.1in</MarginBottom>
                        </DeviceInfo>";
            }

            return "";
        }

        public static void ViewReport(string path, List<System.Data.DataTable> tb,
          List<string> DatasetName, Dictionary<string, object> dic, string paper, int printnumber = 1)
        {
            LocalReport report = new LocalReport();
            report.DataSources.Clear();

            for (int i = 0; i < tb.Count; i++)
            {
                report.DataSources.Add(new ReportDataSource(DatasetName[i], tb[i]));
            }
            //if (dic.Count > 0)
            //{
            //    foreach (KeyValuePair<string, object> p in dic)
            //    {
            //        ReportParameter param = new ReportParameter(p.Key.ToString(), p.Value.ToString());    
            //        report.SetParameters(param);
            //    }
            //}
                report.ReportPath = path;
                Export(report, paper);
                Prints();
            
        }

        public static void ViewReport(string path, List<System.Data.DataTable> tb,
          List<string> DatasetName)
        {

            ReportMicrosoftViewer f = new ReportMicrosoftViewer();
            // ReportDataSource rds =;
            f.rptViewer.LocalReport.DataSources.Clear();
            for (int i = 0; i < tb.Count; i++)
            {
                f.rptViewer.LocalReport.DataSources.Add(new ReportDataSource(DatasetName[i], tb[i]));
            }
            // string source = string.Format("{0}.{1}.rdlc", Namespace, ReportName);
            f.rptViewer.LocalReport.ReportPath = path;
            f.rptViewer.RefreshReport();
            f.Show();
        }
        public static void Makeconnection()
        {
            try
            {
                cnn = new SqlConnection(DataBase.getConnectionString());
                if (cnn.State != System.Data.ConnectionState.Open)
                {
                    cnn.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
        public static void ReadConditionfield(string sql, DataGridView dgv)
        {
            Makeconnection();
            SqlDataAdapter adabter = new SqlDataAdapter(sql, cnn);
            System.Data.DataTable table = new System.Data.DataTable();
            adabter.Fill(table);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                dgv.Rows.Add(table.Rows[i][0]);
            }
        }
        public static Boolean CheckRow(DataGridView dgvCondition, DataGridView dgvValue)
        {
            Boolean b = false;
            int index = dgvCondition.CurrentRow.Index;
            string Condtion = dgvCondition.Rows[index].Cells[0].Value.ToString();

            if (dgvValue.Rows.Count > 0)
            {
                for (int i = 0; i <= dgvValue.Rows.Count - 1; i++)
                {

                    if (Condtion.Equals(dgvValue.Rows[i].Cells[0].Value.ToString()))
                    {
                        b = true;
                    }
                }
            }
            return b;
        }

        public static void AddvalueRow(DataGridView dgvCondition, DataGridView dgvValue)
        {
            int index = dgvCondition.CurrentRow.Index;
            string Condtion = dgvCondition.Rows[index].Cells[0].Value.ToString();
            dgvValue.Rows.Add(Condtion);
        }
        public static string setCondition(DataGridView dgvValue)
        {
            string condition = " ";
            int lastindex = dgvValue.Rows.Count - 1;
            try
            {
                if (dgvValue.Rows.Count > 0)
                {
                    for (int i = 0; i <= dgvValue.Rows.Count - 1; i++)
                    {
                        if (i == 0)
                        {
                            condition = dgvValue.Rows[i].Cells[0].Value.ToString() + " " + "like" + " " + "'" + dgvValue.Rows[i].Cells[1].Value.ToString() + "%" + "'" + " ";
                        }
                        else
                        {
                            if (i < lastindex)
                            {
                                condition = condition + " or " + dgvValue.Rows[i].Cells[0].Value.ToString() + " " + "like" + " " + "'" + dgvValue.Rows[i].Cells[1].Value.ToString() + "%" + "'";
                            }
                            else
                            {
                                condition = condition + " or " + dgvValue.Rows[i].Cells[0].Value.ToString() + " " + "like" + " " + "'" + dgvValue.Rows[i].Cells[1].Value.ToString() + "%" + "'";
                            }
                        }

                    }
                }
                return condition;
            }
            catch (Exception)
            {

                throw;
            }

        }

        /*      public static void ViewData(string TableName,string condtion ,DataGridView dgv, string StoreProcedureName) {
                  SqlDataAdapter adabter;
                  System.Data.DataTable table = new System.Data.DataTable();
                  string select = "select * from " + TableName + " where " + condtion;
                  Makeconnection();
                  SqlCommand cmd = new SqlCommand(StoreProcedureName, cnn);
                  cmd.CommandType = CommandType.StoredProcedure;
                  cmd.Parameters.Clear();
                  cmd.Parameters.AddWithValue("@condition", select);
                  adabter = new SqlDataAdapter(cmd);
                  //adabter = new SqlDataAdapter(select, cnn);
                  adabter.Fill(table);
                  dgv.DataSource = table;
              }*/

        public static string ReadparametList(string storprocedureName)
        {
            string plist = null;
            Makeconnection();
            SqlCommand cmd = new SqlCommand(storprocedureName, cnn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SqlCommandBuilder.DeriveParameters(cmd);
            foreach (SqlParameter p in cmd.Parameters)
            {
                // dgv.Rows.Add(p);
                plist = plist + "|" + p;
            }
            return plist;
        }

        public static void ViewData(string StoreProcedureName, DataGridView dgv)
        {
            try
            {
                SqlDataAdapter adabter;
                System.Data.DataTable table = new System.Data.DataTable();
                Makeconnection();
                SqlCommand cmd = new SqlCommand(StoreProcedureName, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                adabter = new SqlDataAdapter(cmd);
                adabter.Fill(table);
                dgv.DataSource = table;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public static void ViewData(string spName, DataGridView dgvValue,
            DataGridView dgvcondition,
            DataGridView dgvDisplay)
        {
            try
            {
                //  dgvDisplay.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                //if (dgvValue.Rows.Count > 0)
                //{
                Makeconnection();
                SqlCommand cmd = new SqlCommand(spName, cnn);
                SqlDataAdapter adabter;
                System.Data.DataTable table = new System.Data.DataTable();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlCommandBuilder.DeriveParameters(cmd);

                for (int i = 0; i <= dgvValue.Rows.Count - 1; i++)
                {
                    string conf = Readparamet(spName, dgvValue.Rows[i].Cells[0].Value.ToString());
                    string value = dgvValue.Rows[i].Cells[1].Value.ToString();
                    cmd.Parameters.RemoveAt(conf);
                    cmd.Parameters.AddWithValue(conf, value);
                }
                cmd.ExecuteNonQuery();
                adabter = new SqlDataAdapter(cmd);
                adabter.Fill(table);
                dgvDisplay.DataSource = table;

                // }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static void Addparametvalue(string storprocedureName,
            DataGridView dgvValue,
            DataGridView dgvcondition,
            DataGridView dgvDisplay,
            params object[] rptHeader)
        {
            try
            {
                if (dgvValue.Rows.Count >= 0)
                {
                    Makeconnection();
                    SqlCommand cmd = new SqlCommand(storprocedureName, cnn);
                    SqlDataAdapter adabter;
                    System.Data.DataTable table = new System.Data.DataTable();
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlCommandBuilder.DeriveParameters(cmd);

                    for (int i = 0; i <= dgvValue.Rows.Count - 1; i++)
                    {
                        string conf = Readparamet(storprocedureName, dgvValue.Rows[i].Cells[0].Value.ToString());
                        string value = dgvValue.Rows[i].Cells[1].Value.ToString();
                        cmd.Parameters.RemoveAt(conf);
                        cmd.Parameters.AddWithValue(conf, value);
                    }
                    for (int k = 1; k <= cmd.Parameters.Count - 1; k++)
                    {
                        if (cmd.Parameters[k].Value == null)
                        {
                            string comp = cmd.Parameters[k].ParameterName;
                            cmd.Parameters.RemoveAt(comp);
                            cmd.Parameters.AddWithValue(comp, "");
                        }
                    }
                    cmd.ExecuteNonQuery();
                    adabter = new SqlDataAdapter(cmd);
                    adabter.Fill(table);
                    //edited 1
                    dgvDisplay.ColumnCount = table.Columns.Count;
                    for (int i = 0; i <= table.Columns.Count - 1; i++)
                    {

                        dgvDisplay.Columns[i].Name = table.Columns[i].ToString();
                        dgvDisplay.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                    }

                    // Add Aggument to datagrid view

                    for (int i = 0; i <= rptHeader.Length - 1; i++)
                    {
                        //string conf = dgvValue.Rows[i].Cells[0].Value.ToString();
                        //string value = dgvValue.Rows[i].Cells[1].Value.ToString();
                        //  string sign = ":";
                        // dgvDisplay.Rows.Add(conf + sign + value);
                        dgvDisplay.Rows.Add(rptHeader[i].ToString());
                    }
                    //int c = dgvDisplay.Rows.Count - 1;
                    //int a = dgvDisplay.Rows.Count - 1;
                    //int c = dgvDisplay.Rows.Count;
                    //int a = dgvDisplay.Rows.Count;

                    DataGridViewRow arrows = new DataGridViewRow();
                    arrows.CreateCells(dgvDisplay);
                    for (int j = 0; j <= table.Columns.Count - 1; j++)
                    {

                        arrows.Cells[j].Value = table.Columns[j].ToString();
                    }
                    dgvDisplay.Rows.Add(arrows);

                    for (int k = 0; k <= table.Rows.Count - 1; k++)
                    {
                        DataGridViewRow Newrows = new DataGridViewRow();
                        Newrows.CreateCells(dgvDisplay);
                        for (int l = 0; l <= table.Columns.Count - 1; l++)
                        {

                            Newrows.Cells[l].Value = table.Rows[k][l];
                        }
                        dgvDisplay.Rows.Add(Newrows);
                    }
                    dgvDisplay.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                // throw;
            }
        }

        // AddValue to Paramet And Sho Data In DataGridview
        public static void Addparametvalue(string storprocedureName, DataGridView dgvValue, DataGridView dgvcondition, DataGridView dgvDisplay)
        {
            try
            {
                if (dgvValue.Rows.Count > 0)
                {
                    Makeconnection();
                    SqlCommand cmd = new SqlCommand(storprocedureName, cnn);
                    SqlDataAdapter adabter;
                    System.Data.DataTable table = new System.Data.DataTable();
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlCommandBuilder.DeriveParameters(cmd);

                    for (int i = 0; i <= dgvValue.Rows.Count - 1; i++)
                    {
                        string conf = Readparamet(storprocedureName, dgvValue.Rows[i].Cells[0].Value.ToString());
                        string value = dgvValue.Rows[i].Cells[1].Value.ToString();
                        cmd.Parameters.RemoveAt(conf);
                        cmd.Parameters.AddWithValue(conf, value);
                    }
                    for (int k = 1; k <= cmd.Parameters.Count - 1; k++)
                    {
                        if (cmd.Parameters[k].Value == null)
                        {
                            string comp = cmd.Parameters[k].ParameterName;
                            cmd.Parameters.RemoveAt(comp);
                            cmd.Parameters.AddWithValue(comp, "");
                        }
                    }
                    cmd.ExecuteNonQuery();
                    adabter = new SqlDataAdapter(cmd);
                    adabter.Fill(table);
                    dgvDisplay.ColumnCount = table.Columns.Count - 1;
                    for (int i = 0; i < table.Columns.Count - 1; i++)
                    {

                        dgvDisplay.Columns[i].Name = table.Columns[i].ToString();
                        dgvDisplay.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                    }
                    // Add Aggument to datagrid view

                    for (int i = 0; i <= dgvValue.Rows.Count - 1; i++)
                    {
                        string conf = dgvValue.Rows[i].Cells[0].Value.ToString();
                        string value = dgvValue.Rows[i].Cells[1].Value.ToString();
                        string sign = ":";
                        dgvDisplay.Rows.Add(conf + sign + value);
                    }
                    int c = dgvDisplay.Rows.Count - 1;
                    int a = dgvDisplay.Rows.Count - 1;
                    DataGridViewRow arrows = new DataGridViewRow();
                    arrows.CreateCells(dgvDisplay);
                    for (int j = 0; j < table.Columns.Count - 1; j++)
                    {

                        arrows.Cells[j].Value = table.Columns[j].ToString();
                    }
                    dgvDisplay.Rows.Add(arrows);

                    for (int k = 0; k <= table.Rows.Count - 1; k++)
                    {
                        DataGridViewRow Newrows = new DataGridViewRow();
                        Newrows.CreateCells(dgvDisplay);
                        for (int l = 0; l < table.Columns.Count - 1; l++)
                        {

                            Newrows.Cells[l].Value = table.Rows[k][l];
                        }
                        dgvDisplay.Rows.Add(Newrows);
                    }
                    dgvDisplay.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                // throw;
            }
        }
        public static Boolean CheckConditionValue(DataGridView dgv, int index)
        {
            Boolean b = false;
            for (int i = 0; i <= dgv.Rows.Count - 1; i++)
            {
                if (dgv.Rows[i].Cells[index].Value == null)
                {
                    b = true;
                }
            }
            if (b == true) { MessageBox.Show("Invalid Data."); }
            return b;
        }
        //public static void Export(DataGridView dgv, string path)
        //{
        //    try
        //    {
        //        Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
        //        xlapp.SheetsInNewWorkbook = 1;
        //        Microsoft.Office.Interop.Excel.Workbook xlworkbook = xlapp.Workbooks.Open(path, false, true, true, false, false, true);
        //        Microsoft.Office.Interop.Excel.Worksheet xlworksheet = xlapp.Worksheets.Item[1];
        //        xlworksheet.Name = "Report";
        //        for (int nrow = 0; nrow <= dgv.Rows.Count - 1; nrow++)
        //        {
        //            for (int ncol = 0; ncol <= dgv.Columns.Count - 1; ncol++)
        //            {
        //                xlworksheet.Cells[nrow + 1, ncol + 1] = dgv.Rows[nrow].Cells[ncol].Value;

        //            }
        //        }
        //        xlapp.DisplayAlerts = false;
        //        xlworksheet.Columns.AutoFit();
        //        xlapp.Visible = true;
        //        //xlworkbook.Close(false);
        //        //xlworksheet.SaveAs(path);
        //        //xlworkbook.Save();
        //        //xlapp.Quit();
        //        xlworksheet = null; xlworkbook = null; xlapp = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        throw;
        //    }
        //}
        private static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
        /*public static void doPreview(ref string fname,string condition="", string parlist=null, string p1=null)
        {
           
            CRAXDDRT.Application RA = new CRAXDDRT.Application();
            CRAXDDRT.Report RP = new CRAXDDRT.Report();
            frmViewReport vPrivew = new frmViewReport();
            //CRAXDDRT.DatabaseTable tb = new CRAXDDRT.DatabaseTable();
            //CRAXDDRT.DatabaseTables tbs = new CRAXDDRT.DatabaseTables();
            RP = RA.OpenReport(fname);

            //foreach (DatabaseTable tbl in RP.Database.Tables)
            //{
            //    tbl.ConnectionProperties.DeleteAll();
            //    //tbl.ConnectionProperties.Add("Data Source", FunctionDb.Server());
            //    //tbl.ConnectionProperties.Add("Initial Catalog", FunctionDb.Database());
            //    //tbl.ConnectionProperties.Add("User ID", FunctionDb.UserId());
            //    //tbl.ConnectionProperties.Add("Password", FunctionDb.Pwd());
            //}
           /// RP.Database.Tables[1].ConnectionProperties.DeleteAll();
           //RP.Database.Tables[1].ConnectionProperties.Add("Data Source", FunctionDb.Server());
           //RP.Database.Tables[1].ConnectionProperties.Add("Initial Catalog", FunctionDb.Database());
           //RP.Database.Tables[1].ConnectionProperties.Add("User ID", FunctionDb.UserId());
           //RP.Database.Tables[1].ConnectionProperties.Add("Password", FunctionDb.Pwd());
           
             RP.Database.Tables[1].SetLogOnInfo(FunctionDb.Server(),FunctionDb.Database(),FunctionDb.UserId(), FunctionDb.Pwd());

            if (condition != "")
            {
                RP.RecordSelectionFormula = condition;

            }
            string[] Pval = parlist.Split('|');
            string[] Pva1 = p1.Split('|');
            for (int i = 0; i < Pval.Length; i++)
            {
                if (Pva1[i] != "1")
                    RP.ParameterFields[i + 1].AddCurrentValue(DateTime.Parse(Pval[i]));
                else
                    RP.ParameterFields[i + 1].AddCurrentValue(Pval[i]);
            }
            vPrivew.CW.ReportSource = RP;
            vPrivew.CW.ViewReport();
            vPrivew.Show();
          //  cr.Database.Tables[1].SetLogOnInfo(FunctionDb.ServerName, FunctionDb.Database, FunctionDb.UserId, FunctionDb.Password);
        }*/
        public static string Readparamet(string spName, string displayparamet)
        {
            Makeconnection();
            string p = "select ParametName from tbcondition where SpName='" + spName + "' and DisplayParamet ='" + displayparamet + "'";
            SqlDataReader read;
            SqlCommand cmd = new SqlCommand(p, cnn);
            read = cmd.ExecuteReader();
            read.Read();
            return read[0].ToString();
        }

        //public static void doPreview(ref string fname, string condition = "", string parlist = null, string p1 = null)
        //{                            //file report                            //select dgv macht |    //check date
        //    CRAXDDRT.Application RA = new CRAXDDRT.Application();
        //    CRAXDDRT.Report RP = new CRAXDDRT.Report();
        //    frmViewReport vPrivew = new frmViewReport();
        //    try
        //    {
        //        RP = RA.OpenReport(fname);
        //        //***********Change Datasource at runtime************//
        //        foreach (DatabaseTable tbl in RP.Database.Tables)
        //        {

        //            tbl.ConnectionProperties.DeleteAll();
        //            tbl.ConnectionProperties.Add("Provider", "SQLOLEDB");
        //            tbl.ConnectionProperties.Add("Data Source", Manager.Server);
        //            tbl.ConnectionProperties.Add("Initial Catalog", Manager.Database);
        //            tbl.ConnectionProperties.Add("User ID", Manager.User);
        //            tbl.ConnectionProperties.Add("Password", Manager.Password);
        //        }
        //        foreach (DatabaseTable tb in RP.Database.Tables)
        //        {
        //            tb.Location = tb.Name;
        //        }
        //        //****************************************************//

        //        // RP.Database.Tables[1].SetLogOnInfo(FunctionDb.Server(), FunctionDb.Database(), FunctionDb.UserId(), FunctionDb.Pwd());

        //        if (condition != "")
        //        {
        //            RP.RecordSelectionFormula = condition;
        //        }
        //        string[] Pval = parlist.Split('|');
        //        string[] Pva1 = p1.Split('|');
        //        for (int i = 0; i < Pval.Length; i++)
        //        {
        //            if (Pva1[i] != "1")
        //                RP.ParameterFields[i + 1].AddCurrentValue(DateTime.Parse(Pval[i]));
        //            else
        //                RP.ParameterFields[i + 1].AddCurrentValue(Pval[i]);
        //        }
        //        vPrivew.CW.ReportSource = RP;
        //        vPrivew.CW.ViewReport();
        //        vPrivew.Show();
        //    }
        //    catch (Exception ex)
        //    {
        //        SuperMessage.MessageError(ex);
        //    }
        //}

        //public static void dopreview(string fname, string parlist, string p1 = null)
        //{
        //    CRAXDDRT.Application RA = new CRAXDDRT.Application();
        //    CRAXDDRT.Report RP = new CRAXDDRT.Report();
        //    ReportDocument rpt = new ReportDocument();
        //    frmViewReport vPrivew = new frmViewReport();
        //    try
        //    {
        //        RP = RA.OpenReport(fname);
        //        rpt.Load(fname);
        //        //***********Change Datasource at runtime************//
        //        foreach (DatabaseTable tbl in RP.Database.Tables)
        //        {
        //            tbl.ConnectionProperties.DeleteAll();
        //            tbl.ConnectionProperties.Add("Provider", "SQLOLEDB");
        //            tbl.ConnectionProperties.Add("Data Source", Manager.Server);
        //            tbl.ConnectionProperties.Add("Initial Catalog", Manager.Database);
        //            tbl.ConnectionProperties.Add("User ID", Manager.User);
        //            tbl.ConnectionProperties.Add("Password", Manager.Password);
        //        }
        //        foreach (DatabaseTable tb in RP.Database.Tables)
        //        {
        //            tb.Location = tb.Name;
        //        }
        //        string[] Pval = parlist.Split('|');
        //        string[] Pva1 = p1.Split('|');
        //        for (int j = 1; j <= Pval.Length - 1; j++)
        //        {
        //            string str = Pval[j].ToString();
        //            for (int i = 1; i <= RP.ParameterFields.Count; i++)
        //            {
        //                //                        string str = Pval[j].ToString();
        //                string par = RP.ParameterFields[i].ParameterFieldName;
        //                if (str.Equals(par))
        //                {
        //                    RP.ParameterFields[i].AddCurrentValue(Pva1[j].ToString());
        //                }
        //            }
        //        }
        //        for (int k = 1; k <= RP.ParameterFields.Count; k++)
        //        {
        //            if (RP.ParameterFields[k].NeedsCurrentValue == true || RP.ParameterFields[k].Value == "")
        //            {
        //                RP.ParameterFields[k].AddCurrentValue("");
        //            }
        //        }
        //        vPrivew.CW.ReportSource = RP;
        //        vPrivew.CW.Refresh();
        //        vPrivew.CW.ViewReport();
        //        vPrivew.Show();
        //    }
        //    catch (Exception ex)
        //    {
        //        SuperMessage.MessageError(ex);
        //    }
        //}

        public static string Matchcondition(DataGridView dgvvalue, int cells, string storeprocedure)
        {
            string par = null;
            for (int i = 0; i <= dgvvalue.Rows.Count - 1; i++)
            {
                par = par + "|" + Readparamet(storeprocedure, dgvvalue.Rows[i].Cells[cells].Value.ToString());
            }
            return par;
        }
        public static string Mactchvalue(DataGridView dgvvalue, int cell)
        {
            string val = null;
            for (int i = 0; i <= dgvvalue.Rows.Count - 1; i++)
            {
                if (i == 0 || i == 1)
                {

                }
                val = val + "|" + dgvvalue.Rows[i].Cells[cell].Value.ToString();
            }
            return val;
        }
        public static string GeneratePropertyFromSQL(string SQL)
        {
            string field = string.Empty;
            System.Data.DataTable dt = DataBase.GetDataFromDatase(null, SQL).Tables[0];
            foreach (DataColumn col in dt.Columns)
            {
                field += string.Format("{0} {1} {2} {3}", "public", col.DataType.Name.ToString(), col.ColumnName.ToString(), "{get;set;}") + Environment.NewLine;
            }
            string path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));


            StreamWriter sw = new StreamWriter(String.Format(@"{0}\\TempText.txt", path));
            sw.WriteLine(field);
            sw.Close();
            Process.Start(String.Format("{0}\\TempText.txt", path));
            return field;
        }
        public static string FormatStringTodate(string s)
        {
            try
            {
                string day = null;
                string month = null;
                string year = null;
                string date = null;
                if (s.Length == 8)
                {
                    day = s.Substring(0, 2);
                    month = s.Substring(2, 2);
                    year = s.Substring(4, 4);
                    date = month + "/" + day + "/" + year;
                }
                else
                {
                    MessageBox.Show("Invalid format date. Correct format is ddMMyyyy.");
                    // return "";
                }
                return date;
            }
            catch
            {
                throw;

            }
        }
        public static double sumcolumn(DataGridView dgvvalue, int col, int row)
        {
            double s = 0;
            s = s + double.Parse(dgvvalue.Rows[row].Cells[col].Value.ToString());
            return s;
        }
        public static void UnSortColunms(DataGridView grd)
        {
            for (int i = 0; i <= grd.ColumnCount - 1; i++)
            {
                grd.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        public static void MandatoryCondition(string spName = "", DataGridView grd = null)
        {
            Makeconnection();
            string sql = @"select DisplayParamet as Condition,Value from tbcondition where isMandatory=1 and  SpName='" + spName + "'  order by MandatoryNumber";
            SqlDataAdapter da = new SqlDataAdapter(sql, cnn);
            System.Data.DataTable tb = new System.Data.DataTable();
            da.Fill(tb);
            grd.DataSource = tb;

        }
        public static bool ExportToExcel(DataGridView dGV, string filename)
        {
            string stOutput = "";
            // Export titles:
            string sHeaders = "";

            for (int j = 0; j < dGV.Columns.Count; j++)
                sHeaders = sHeaders.ToString() + Convert.ToString(dGV.Columns[j].HeaderText) + "\t";
            stOutput += sHeaders + "\r\n";

            // Export data.
            for (int i = 0; i < dGV.RowCount; i++)
            {
                string stLine = "";
                for (int j = 0; j < dGV.Rows[i].Cells.Count; j++)
                    stLine = stLine.ToString() + Convert.ToString(dGV.Rows[i].Cells[j].Value) + "\t";
                stOutput += stLine + "\r\n";
            }
            Encoding utf16 = Encoding.GetEncoding(1254);
            byte[] output = utf16.GetBytes(stOutput);
            FileStream fs = new FileStream(filename, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(output, 0, output.Length); //write the encoded file
            bw.Flush();
            bw.Close();
            fs.Close();
            return true;
        }
        //public static void ExportToExcel(string path, DataGridView _gdvList, string colorHeader, string colorRows1, string colorRows2, params string[] header)
        //{

        //    int n = 2;
        //    string colorH = "";
        //    string color1 = "";
        //    string color2 = "";
        //    try
        //    {
        //        //string appPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
        //        //MessageBox.Show(appPath);
        //        if (colorRows1 == "")
        //        {
        //            color1 = "#F0F0F0";
        //        }
        //        if (colorRows1 != "")
        //        {
        //            color1 = colorRows1;
        //        }
        //        if (colorRows2 == "")
        //        {
        //            color2 = "#ADEDEC";
        //        }
        //        if (colorRows2 != "")
        //        {
        //            color2 = colorRows2;
        //        }
        //        if (colorHeader == "")
        //        {
        //            colorH = "#494529";
        //        }
        //        if (colorHeader != "")
        //        {
        //            colorH = colorHeader;
        //        }
        //        StreamWriter sw = new StreamWriter(path);
        //        sw.WriteLine("<?xml version=" + "\"1.0\"" + " encoding=" + "\"utf-8\"" + " ?>");
        //        sw.WriteLine("<?mso-application progid=" + "\"Excel.Sheet\"" + "?>");
        //        sw.WriteLine("<Workbook xmlns=" + "\"urn:schemas-microsoft-com:office:spreadsheet\"");
        //        sw.WriteLine("xmlns:o=" + "\"urn:schemas-microsoft-com:office:office\"");
        //        sw.WriteLine("xmlns:x=" + "\"urn:schemas-microsoft-com:office:excel\"");
        //        sw.WriteLine("xmlns:ss=" + "\"urn:schemas-microsoft-com:office:spreadsheet\"");
        //        sw.WriteLine("xmlns:html=" + "\"http://www.w3.org/TR/REC-html40 \"" + ">");

        //        sw.WriteLine(" <DocumentProperties xmlns=" + "\"urn: schemas - microsoft - com:office: office\"" + ">");
        //        sw.WriteLine("</DocumentProperties>");

        //        sw.WriteLine("<Styles >");

        //        sw.WriteLine("<Style ss:ID =" + "\"1\"" + ">");
        //        sw.WriteLine("<Font ss:Bold =" + "\"1\"" + " ss:Color =" + "\"white\"" + "/>");
        //        sw.WriteLine("<Interior ss:Color=" + "\"" + colorH + "\"" + " ss:Pattern=" + "\"Solid\"" + " />");
        //        sw.WriteLine("</Style >");

        //        sw.WriteLine("<Style ss:ID =" + "\"2\"" + ">");
        //        sw.WriteLine("<Alignment ss:Vertical =" + "\"Center\"" + "/>");
        //        sw.WriteLine("<Interior ss:Color=" + "\"" + color1 + "\"" + " ss:Pattern=" + "\"Solid\"" + " />");
        //        sw.WriteLine("</Style >");

        //        sw.WriteLine("<Style ss:ID =" + "\"3\"" + ">");
        //        sw.WriteLine("<Alignment ss:Vertical =" + "\"Center\"" + "/>");
        //        sw.WriteLine("<Interior ss:Color=" + "\"" + color2 + "\"" + " ss:Pattern=" + "\"Solid\"" + " />");
        //        sw.WriteLine("</Style >");

        //        sw.WriteLine("<Style ss:ID =" + "\"4\"" + ">");
        //        sw.WriteLine("<Font ss:Size=" + "\"24\"" + " ss:FontName=" + "\"Times New Roman\"" + "/>");
        //        sw.WriteLine("</Style >");

        //        sw.WriteLine("<Style ss:ID =" + "\"5\"" + ">");
        //        sw.WriteLine("<Font ss:Size=" + "\"14\"" + " ss:FontName=" + "\"Khmer OS Muol Light\"" + "/>");
        //        sw.WriteLine("</Style >");

        //        sw.WriteLine("<Style ss:ID =" + "\"6\"" + ">");
        //        sw.WriteLine("<Font ss:Size=" + "\"12\"" + " ss:FontName=" + "\"Times New Roman\"" + "/>");
        //        sw.WriteLine("</Style >");

        //        sw.WriteLine("<Style ss:ID =" + "\"7\"" + ">");
        //        sw.WriteLine("<Font ss:Size=" + "\"12\"" + " ss:FontName=" + "\"Times New Roman\"" + "/>");
        //        sw.WriteLine("</Style >");

        //        sw.WriteLine("<Style ss:ID =" + "\"8\"" + ">");
        //        sw.WriteLine("<Font ss:Size=" + "\"12\"" + " ss:FontName=" + "\"Times New Roman\"" + "/>");
        //        sw.WriteLine("</Style >");

        //        sw.WriteLine("<Style ss:ID =" + "\"9\"" + ">");
        //        sw.WriteLine("<Font ss:Size=" + "\"12\"" + " ss:FontName=" + "\"Times New Roman\"" + "/>");
        //        sw.WriteLine("</Style >");

        //        sw.WriteLine(" </Styles >");

        //        sw.WriteLine("<Worksheet ss:Name=" + "\"Sheet1\"" + ">");

        //        sw.WriteLine("<Table>");
        //        for (int i = 0; i < _gdvList.Columns.Count; i++)
        //        {
        //            sw.WriteLine("<ss:Column ss:Width =" + "\"" + _gdvList.Columns[i].Width + "\"" + "/>");

        //        }

        //        sw.WriteLine("<Row ss:StyleID=" + "\"4\"" + ">");
        //        sw.WriteLine("<Cell>");
        //        sw.WriteLine("<Data ss:Type=" + "\"String\"" + ">" + header[0] + "</Data>");
        //        sw.WriteLine("</Cell>");
        //        sw.WriteLine(" </Row>");
        //        for (int i = 0; i < header.Length - 1; i++)
        //        {
        //            sw.WriteLine("<Row ss:StyleID=" + "\"" + (i + 5) + "\"" + ">");
        //            sw.WriteLine("<Cell>");
        //            sw.WriteLine("<Data ss:Type=" + "\"String\"" + ">" + header[i + 1] + "</Data>");
        //            sw.WriteLine("</Cell>");
        //            sw.WriteLine(" </Row>");
        //        }

        //        sw.WriteLine("<Row>");
        //        for (int i = 0; i < _gdvList.Columns.Count; i++)
        //        {
        //            sw.WriteLine("<Cell ss:StyleID=" + "\"1\"" + ">");
        //            sw.WriteLine("<Data ss:Type=" + "\"String\"" + ">" + _gdvList.Columns[i].Name.ToString() + "</Data>");
        //            sw.WriteLine("</Cell>");

        //        }

        //        sw.WriteLine(" </Row>");
        //        foreach (DataGridViewRow row in _gdvList.Rows)
        //        {
        //            sw.WriteLine("<Row ss:AutoFitHeight = " + "\"0\"" + " ss:Height=" + "\"18\"" + ">");
        //            for (int i = 0; i < _gdvList.Columns.Count; i++)
        //            {

        //                sw.WriteLine("<Cell ss:StyleID=" + "\"" + n + "\"" + " >");
        //                sw.WriteLine("<Data ss:Type=" + "\"String\"" + ">" + row.Cells[i].Value.ToString() + "</Data>");
        //                sw.WriteLine("</Cell>");

        //            }
        //            if (n == 2)
        //            {
        //                n = 3;
        //            }
        //            else
        //            {
        //                n = 2;
        //            }
        //            sw.WriteLine(" </Row>");
        //        }
        //        sw.WriteLine("</Table>");
        //        sw.WriteLine(" </Worksheet>");
        //        sw.WriteLine("</Workbook>");
        //        sw.Flush();
        //        sw.Dispose();
        //        sw.Close();

        //        Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
        //        Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Open(path, false, true);
        //        Microsoft.Office.Interop.Excel.Worksheet worksheet = null;

        //        worksheet = workbook.Sheets["Sheet1"];
        //        worksheet = workbook.ActiveSheet;
        //        Microsoft.Office.Interop.Excel.Range formatRange;
        //        int x = _gdvList.Columns.Count;
        //        string ms = "";
        //        if (x == 3)
        //        {
        //            ms = "c";
        //        }
        //        else if (x == 4)
        //        {
        //            ms = "d";
        //        }
        //        else if (x == 5)
        //        {
        //            ms = "e";
        //        }
        //        else if (x == 6)
        //        {
        //            ms = "f";
        //        }
        //        else if (x == 7)
        //        {
        //            ms = "g";
        //        }
        //        else if (x == 8)
        //        {
        //            ms = "h";
        //        }
        //        else if (x == 9)
        //        {
        //            ms = "i";
        //        }
        //        else if (x == 10)
        //        {
        //            ms = "j";
        //        }
        //        else
        //        {
        //            ms = "j";
        //        }

        //        for (int i = 1; i <= header.Length; i++)
        //        {


        //            formatRange = worksheet.get_Range("a" + i, ms + 1);
        //            //  Microsoft.Office.Interop.Excel.Range cell = formatRange.Cells[i, j];
        //            worksheet.get_Range("a" + i, ms + i).Merge(false);

        //            formatRange.HorizontalAlignment = 3;
        //            formatRange.VerticalAlignment = 3;


        //        }
        //        app.Columns.AutoFit();

        //        app.Visible = true;
        //        app = null;
        //        workbook = null; worksheet = null;






        //    }
        //    catch (Exception ex)
        //    {

        //        ex.MessageError();

        //    }

        //}


    }
}
