using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ConnectionDB
{
   public class DBConnection
    {
        public static iniReader ini = new iniReader("conn.vi");//read connection from file conn.vi


        public static string User
        {
            get
            {
                return "chhai";
            }
        }


        public static string Password { get { return "123456"; } }
        public static string Server { get { return "DESKTOP-8G1NREG";  } }
        public static string Wifi { get { return "fsdf"; } }
        public static string Instance { get; set; }
        public static DateTime CurrentDate { get { return Convert.ToDateTime("07-04-2015"); } set { CurrentDate = value; } }
        public static string Database { get { return "poscoffe"; } }
        public static string UserID { get; set; }
        public static string UserName { get; set; }
        public static string BranchName { get; set; }
        public static string BranchID { get; set; }
        //  public static string 
       

        public static string PrinterName
        {
            get { return ini.Read("PrinterName", "PrinterParameters"); }
        }
        public static string isPrinterSizeSmall
        {
            get { return ini.Read("isSmall", "PrinterParameters"); }
        }
        public static string getBranchOffice(string branchCode)
        {
            var cn = new SqlConnection
            {
                ConnectionString =
                    string.Format(@"Server={0}; Database={1};User ID={2};Password={3};Connection Timeout=2000;Pooling=true;Min Pool size=1;Max Pool Size=5000", Server, Database, User, Password)
            };
            cn.Open();
            var cmd = new SqlCommand
            {
                CommandText = "select top 1 BranchName from tbBranch where Code='" + branchCode + "'",
                CommandType = CommandType.Text,
                Connection = cn
            };
            var d = (string)cmd.ExecuteScalar();
            return d;
        }

        public static DateTime GetDateTime()
        {
            var cn = new SqlConnection
            {
                ConnectionString =
                    string.Format(@"Server={0}; Database={1};User ID={2};Password={3};Connection Timeout=2000;Pooling=true;Min Pool size=1;Max Pool Size=5000", Server, Database, User, Password)
            };
            cn.Open();
            var cmd = new SqlCommand
            {
                CommandText = "SELECT TOP 1 ISNULL(convert(date,CurrentDate),GETDATE()) FROM dbo.tbDateTime ORDER BY UpdateDate DESC",
                CommandType = CommandType.Text,
                Connection = cn
            };
            var d = (DateTime)cmd.ExecuteScalar();
            return d;
        }
    }
}

