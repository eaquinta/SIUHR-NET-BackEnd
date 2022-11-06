using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Domain.Common
{
    public class DBFReader
    {
        public string PathFileName { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string ConString { get; set; }

               
        public DBFReader(string filePath)
        {
            //PathFileName = filePathName;
            //FileName = Path.GetFileNameWithoutExtension(PathFileName);
            FilePath = filePath;
                //Path.GetDirectoryName(PathFileName);
            ConString = $@"Provider=VFPOLEDB.1;Data Source={FilePath};";
        }

        public DataTable GetDataTable(string FileName)
        {
            DataTable dt = new DataTable();

            using (OleDbConnection cn = new OleDbConnection(ConString))
            {
                cn.Open();
                string sql = $"SELECT * FROM {FileName}";
                OleDbCommand cmd = new OleDbCommand(sql, cn);

                //Method1
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);

                // Method2
                //dt.Load(cmd.ExecuteReader());

                cn.Close();
            }
            return dt;
        }

        public DataTable GetSQLQuery(string sql)
        {
            DataTable dt = new DataTable();

            using (OleDbConnection cn = new OleDbConnection(ConString))
            {
                cn.Open();
                //string sql = $"SELECT * FROM {FileName}";
                OleDbCommand cmd = new OleDbCommand(sql, cn);

                //Method1
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);

                // Method2
                //dt.Load(cmd.ExecuteReader());

                cn.Close();
            }
            return dt;
        }
    }
}


//OleDbConnection oConn = new OleDbConnection("Provider=VFPOLEDB.1;Data Source=C:\\Siahr\\");
//OleDbCommand oCmd = new OleDbCommand();

//{
//    oCmd.Connection = oConn;
//    oCmd.Connection.Open();
//    // Create a sample FoxPro table 
//    oCmd.CommandText = "CREATE TABLE Table1 (FldOne c(10))";
//    oCmd.CommandType = CommandType.Text;
//    oCmd.ExecuteNonQuery();
//    oCmd.CommandText = "SELECT * FROM Table1";
//    oCmd.CommandType = CommandType.Text;
//    dt.Load(oCmd.ExecuteReader());
//}

//oConn.Close();
//oConn.Dispose();
//oCmd.Dispose();
