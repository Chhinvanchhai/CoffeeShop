using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ConnectionDB
{
    public class DataBase
    {
        public static string UserName = "";
        public static String User { get; set; }
        public String Position { get; set; }
        public static int Waiting { get; set; } = 0;
        public static String PassWifi{get;set;}

        public static int NumOfPrint { get; set; }
        public static void Clear(params object[] ctr)
        {
            foreach (var c in ctr)
            {
                if (c is TextBox)
                    ((TextBox)c).Text = string.Empty;
                else if (c is Label)
                    ((Label)c).Text = String.Empty;
                else if (c is ComboBox)
                    ((ComboBox)c).Items.Clear();
                else if (c is DataGridView)
                    ((DataGridView)c).Rows.Clear();
                else if (c is CheckBox)
                    ((CheckBox)c).Checked = false;
                else if (c is DateTimePicker)
                    ((DateTimePicker)c).ResetText();
                else if (c is NumericUpDown)
                    ((NumericUpDown)c).Value = 0;
               

            }
        }
        public static DataSet GetDataFromDatase(string connection = null, string sql = null)//Select data from database
        {
            try
            {
                if (connection == null) connection = getConnectionString();
                System.Console.Write(connection);
                using (var cn = new SqlConnection(connection))
                {
                    if (cn.State == ConnectionState.Closed) cn.Open();
                    using (var da = new SqlDataAdapter())
                    {
                        da.SelectCommand = new SqlCommand("sp_runSQL", cn); //create store procedure in sql server name sp_runSQL in progaram
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.Parameters.AddWithValue("@sql", sql);
                        using (var ds = new DataSet())
                        {
                            da.Fill(ds);
                            cn.Close();
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return null;
            }
        }
        public static void FillDataSet(DataSet ds, string spName, string tableName, string par, string connection = null, params object[] parVal)
        {
            try
            {
                if (connection == null) connection = getConnectionString();
                using (var cn = new SqlConnection(connection))
                {
                    if (cn.State == ConnectionState.Closed) cn.Open();
                    using (var da = new SqlDataAdapter())
                    {
                        da.SelectCommand = new SqlCommand(spName, cn);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        var str = par.Split(':');
                        for (byte i = 0; i < parVal.Length; i++)
                            da.SelectCommand.Parameters.AddWithValue(str[i], parVal[i]);
                        da.Fill(ds, tableName);
                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());;
            }
        }
        public static string GetScalrValue(string connection = null, string sql = null)//select only want value from database
        {
            try
            {
                return GetDataFromDatase(connection == null ? getConnectionString() : connection, sql).Tables[0].Rows[0][0].ToString();
            }
            catch
            {
                //  SuperMessage.SetErrorMessage("Error while read data from database.");
                return "";
            }
        }
        public static bool runSQL(SqlConnection cn = null, string sql = "", SqlTransaction t = null)
        {
            bool b;
            try
            {
                if (cn == null) cn = new SqlConnection(getConnectionString());
                if (cn.State == ConnectionState.Closed) cn.Open();
                var cmd = t != null ? new SqlCommand("sp_runSQL", cn, t) : new SqlCommand("sp_runSQL", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@sql", sql);
                b = Convert.ToBoolean(cmd.ExecuteNonQuery());

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b;
        }
        public static bool runSQL(SqlConnection cn, string spName, string spParam, SqlTransaction t = null, params object[] parametter)
        {
            try
            {
                var par = spParam.Split(':');
                if (cn == null) cn = new SqlConnection(getConnectionString());
                if (cn.State == ConnectionState.Closed) cn.Open();
                var cmd = t != null ? new SqlCommand(spName, cn, t) : new SqlCommand(spName, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                for (byte i = 0; i < parametter.Length; i++)
                    cmd.Parameters.AddWithValue(par[i], parametter[i]);
                cmd.ExecuteNonQuery();
                t.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }
        public static bool InsertByRunSP(SqlConnection cn, string spName, string spParam, ref string outParVal, string outPar = null, SqlTransaction t = null, params object[] parametter)
        {
            try
            {
                if (cn == null) cn = new SqlConnection(getConnectionString());
                if (cn.State == ConnectionState.Closed) cn.Open();
                //if (t == null) t = cn.BeginTransaction();
                var par = spParam.Split(':');
                var cmd = t != null ? new SqlCommand(spName, cn, t) : new SqlCommand(spName, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                for (byte i = 0; i < parametter.Length; i++)
                    cmd.Parameters.AddWithValue(par[i], parametter[i]);
                if (outPar != null) cmd.Parameters.Add(outPar, SqlDbType.NVarChar, 1000).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                if (outPar != null) outParVal = cmd.Parameters[outPar].Value.ToString();
                //if(t!=null) t.Commit();
            }
            catch (Exception ex)
            {
                 MessageBox.Show(ex.ToString());;
            }
            return false;
        }
        public static void SaveImage(SqlConnection cn, string path, string des, string userId, SqlTransaction t = null)
        {

            runSQL(cn, "sp_addImage", "@img:@des:@User", t, File.ReadAllBytes(path), des, userId);
        }
        public static Image LoadImage(SqlConnection cn, Int32 id, SqlTransaction t = null)
        {
            using (var ds = GetDataFromDatase(getConnectionString(), "Select Photo as p from tbPhoto Where PhotoID=" + id))
            {
                var data = (byte[])(ds.Tables[0].Rows[0]["p"]);
                var ms = new MemoryStream(data);
                return Image.FromStream(ms);
            }
        }
        public static Int32 SaveAddImage(SqlConnection cn, string path, string user, SqlTransaction t = null)
        {
            if (cn.State == ConnectionState.Closed) cn.Open();
            var cmd = (t != null) ? new SqlCommand("sp_saveImage", cn, t) : new SqlCommand("sp_saveImage", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@img", File.ReadAllBytes(path));
            cmd.Parameters.AddWithValue("@user", user);
            cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
            if (Convert.ToBoolean(cmd.ExecuteNonQuery()) == false) throw new Exception();
            // t.Commit();
            return Convert.ToInt32(cmd.Parameters["@id"].Value.ToString());
        }
        public static string Server()
        {
            return ServerName;
        }
        public static string Database()
        {
            return DbName;
        }

        public static string DbName = DBConnection.Database, ServerName = DBConnection.Server; //System.Net.Dns.GetHostName() + "\\sql2008r2";
        public static string DbuserId = DBConnection.User, Password = DBConnection.Password;
        public static string UserId()
        {
            return DbuserId;
        }
        public static string Pwd() { return Password; }
        public static string getConnectionString()
        {
            return "Server=" + DBConnection.Server + ";Initial Catalog=" + DBConnection.Database + ";User ID=" + DBConnection.User + ";Password=" + DBConnection.Password + ";Connection Timeout=2000;Pooling=true;Min Pool size=1;Max Pool Size=5000";
        }
        public static string getConnectionString(string s, string db, string id, string p, Int64 timeout = 2000)
        {
            return "Server=" + s + ";Database=" + db + ";User ID=" + id + ";Password=" + p + ";Connection Timeout=" + timeout + ";";
        }
        public static string GetAutoId(string table, string position, string fd = "''")
        {
            var cn = new SqlConnection(getConnectionString());
            if (cn.State == ConnectionState.Closed) cn.Open();
            var cmd = new SqlCommand("sp_getAutoID", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@table", table);
            cmd.Parameters.AddWithValue("@function", position);
            cmd.Parameters.AddWithValue("@fd", fd);
            cmd.Parameters.Add("@id", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            cn.Close();
            return (cmd.Parameters["@id"].Value.ToString());
        }

        //public static ReportDocument LoadReport(string path, params string[] par)
        //{
        //    // Initialize Server and Database
        //    var cnn = new ConnectionInfo();
        //    cnn.ServerName = Server();
        //    cnn.DatabaseName = Database();
        //    cnn.UserID = UserId();
        //    cnn.Password = Pwd();
        //    // Set Model properties
        //    var connectionAttributes = new PropertyBag();
        //    connectionAttributes.EnsureCapacity(11);
        //    connectionAttributes.Add("Connect Timeout", "15");
        //    connectionAttributes.Add("Data Source", DBConnection.Server);
        //    connectionAttributes.Add("General Timeout", "0");
        //    connectionAttributes.Add("Initial Catalog", DBConnection.Database);
        //    connectionAttributes.Add("Integrated Security", false);
        //    connectionAttributes.Add("Locale Identifier", "1033");
        //    connectionAttributes.Add("OLE DB Services", "-5");
        //    connectionAttributes.Add("Provider", "SQLOLEDB");
        //    connectionAttributes.Add("Tag with column collation when possible", "0");
        //    connectionAttributes.Add("Use DSN Default Properties", false);
        //    connectionAttributes.Add("Use Encryption for Data", "0");

        //    var attributes = new DbConnectionAttributes();
        //    attributes.Collection.Add(new NameValuePair2("Database DLL", "crdb_ado.dll"));
        //    attributes.Collection.Add(new NameValuePair2("QE_DatabaseName", DBConnection.Database));
        //    attributes.Collection.Add(new NameValuePair2("QE_DatabaseType", "OLE DB (ADO)"));
        //    attributes.Collection.Add(new NameValuePair2("QE_LogonProperties", connectionAttributes));
        //    attributes.Collection.Add(new NameValuePair2("QE_ServerDescription", DBConnection.Server));
        //    attributes.Collection.Add(new NameValuePair2("SSO Enabled", false));
        //    //
        //    cnn.Attributes = attributes;
        //    cnn.Type = ConnectionInfoType.SQL;
        //    var tb = new TableLogOnInfo();
        //    tb.ConnectionInfo = cnn;
        //    var rptDocument = new ReportDocument();
        //    rptDocument.Load(path);
        //    rptDocument.Refresh();
        //    for (byte i = 0; i < par.Length; i++)
        //        rptDocument.SetParameterValue(i, par[i]);
        //    for (var i = 0; i < rptDocument.Database.Tables.Count; i++)
        //    {
        //        var table = rptDocument.Database.Tables[i];
        //        table.ApplyLogOnInfo(tb);

        //    }
        //    return rptDocument;
        //}
        public static Image GetImage(string id, string connectionString)
        {
            var ds = GetDataFromDatase(connectionString, "Select Photo From tbPhoto Where PhotoCode='" + id + "'");
            if (ds.Tables[0].Rows.Count > 0)
            {
                var data = new Byte[0];
                data = (Byte[])ds.Tables[0].Rows[0][0];
                var m = new MemoryStream(data);
                return Image.FromStream(m);
            }
            return null;
        }
        public static Image GetImage2(string id, string connectionString)
        {
            var ds = GetDataFromDatase(connectionString, "Select Photo1 From tbPhoto Where PhotoCode='" + id + "'");
            if (ds.Tables[0].Rows.Count > 0)
            {
                var data = new Byte[0];
                data = (Byte[])ds.Tables[0].Rows[0][0];
                var m = new MemoryStream(data);
                return Image.FromStream(m);
            }
            return null;
        }
        public static Image GetImage3(string id, string connectionString)
        {
            var ds = GetDataFromDatase(connectionString, "Select Photo2 From tbPhoto Where PhotoCode='" + id + "'");
            if (ds.Tables[0].Rows.Count > 0)
            {
                var data = new Byte[0];
                data = (Byte[])ds.Tables[0].Rows[0][0];
                var m = new MemoryStream(data);
                return Image.FromStream(m);
            }
            return null;
        }
        public static Image GetImage4(string id, string connectionString)
        {
            var ds = GetDataFromDatase(connectionString, "Select Photo3 From tbPhoto Where PhotoCode='" + id + "'");
            if (ds.Tables[0].Rows.Count > 0)
            {
                var data = new Byte[0];
                data = (Byte[])ds.Tables[0].Rows[0][0];
                var m = new MemoryStream(data);
                return Image.FromStream(m);
            }
            return null;
        }
        public static Image GetImage(string id, string type, string connectionString = null)
        {
            var ds = GetDataFromDatase(connectionString, "Select Photo From tbPhoto Where PhotoCode='" + id + "' and AppID='" + type + "'");
            if (ds.Tables[0].Rows.Count > 0)
            {
                var data = new Byte[0];
                data = (Byte[])ds.Tables[0].Rows[0][0];
                var m = new MemoryStream(data);
                return Image.FromStream(m);
            }
            return null;
        }

        public static DateTime GetDateTime()
        {
            return Convert.ToDateTime(GetScalrValue(null, "select Top 1 ISNULL(CurrentDate,getdate()) From tbDateTime"));
        }

        //public static Report PreviewReport(string fileName, string server = null, string database = null, string userId = null, string password = null)
        //{
        //    var crxApp = new Application();
        //    var crxReport = crxApp.OpenReport(fileName);


        //    //foreach (DatabaseTable tb in crxReport.Database.Tables)
        //    //{

        //    //    //  tb.SetLogOnInfo(server ?? ServerName, database ?? DbName, userId ?? DbuserId, password ?? Password);
        //    //}
        //    foreach (DatabaseTable tbl in crxReport.Database.Tables)
        //    {

        //        tbl.ConnectionProperties.DeleteAll();
        //        tbl.ConnectionProperties.Add("Provider", "SQLOLEDB");
        //        tbl.ConnectionProperties.Add("Data Source", DBConnection.Server);
        //        tbl.ConnectionProperties.Add("Initial Catalog", DBConnection.Database);
        //        tbl.ConnectionProperties.Add("User ID", DBConnection.User);
        //        tbl.ConnectionProperties.Add("Password", DBConnection.Password);
        //    }
        //    foreach (DatabaseTable tb in crxReport.Database.Tables)
        //    {
        //        tb.Location = tb.Name;
        //    }
        //    return crxReport;
        //}
        // **********************************************************
        //public static int AutoID(string sql)
        //{
        //    int count = 0;
        //    int countID = 0;
        //    SqlCommand cmd;
        //    SqlDataReader read;
        //    var cn = new SqlConnection(getConnectionString());
        //    if (cn.State != ConnectionState.Open) cn.Open();
        //    ArrayList al = new ArrayList();
        //    try
        //    {
        //        cmd = new SqlCommand(sql, cn);
        //        read = cmd.ExecuteReader();
        //        while (read.Read())
        //        {
        //            count += 1;
        //            al.Add(read[0].ToString());
        //        }
        //        if (count <= 0)
        //        {
        //            countID = 1;

        //        }
        //        else
        //        {
        //            countID = Convert.ToInt16(al[0].ToString());
        //            if (countID > 1)
        //            {
        //                countID = 1;
        //            }
        //            else
        //            {
        //                foreach (Object str in al)
        //                {
        //                    if ((countID + 1) != int.Parse(str.ToString()) + 1)
        //                        continue;
        //                    else
        //                        countID = countID + 1;
        //                }
        //            }
        //        }

        //    }
        //    catch (SqlException ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //        return 0;
        //    }
        //    return countID;
        //}
        public static byte[] ImageToByte(System.Drawing.Image imageToConvert,
                                                  System.Drawing.Imaging.ImageFormat formatOfImage)
        {
            byte[] Ret;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    imageToConvert.Save(ms, formatOfImage);
                    Ret = ms.ToArray();
                }
            }
            catch (Exception) { throw; }
            return Ret;
        }
        public static void EditUser(string userID, string Status)
        {
            DataBase.runSQL(null, "sp_Users_Status '" + userID + "','" + Status + "'", null);
        }

        public static void numberOnly(object sender, KeyPressEventArgs e)
        {
            int code = (int)Convert.ToChar(e.KeyChar.ToString());
            if ((code >= 48 && code <= 57) || code == 46)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
            if (code == 8)
            {
                e.Handled = false;
            }
        }
    }
    
}
