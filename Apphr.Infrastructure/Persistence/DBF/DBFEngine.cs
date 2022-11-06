using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace Apphr.Infrastructure.Persistence.DBF
{
    public class DBFEngine : IDisposable
    {
        private bool disposedValue;

        public string PathFileName { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string ConString { get; set; }
        public DBFEngine(string filePath)
        {                        
            FilePath = filePath;            
            ConString = $@"Provider=VFPOLEDB.1;Data Source={FilePath};";
        }

        public DataTable GetDataTable(string FileName)
        {
            string sql = $"SELECT * FROM {FileName}";
            return GetDataSQL(sql);
        }

        public OleDbDataReader GetDataSQLReader(string sql)
        {
            using (OleDbConnection cn = new OleDbConnection(ConString))
            {
                cn.Open();
                using (OleDbCommand cmd = new OleDbCommand(sql, cn))
                {
                    //Method1
                    //OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    //da.Fill(dt);
                    //da.Dispose();
                    //da = null;
                    // Method2                 
                    return cmd.ExecuteReader();
                }
                //cn.Close();
            }
        }

        public  DataTable GetDataSQL(string sql)
        {
            using (DataTable dt = new DataTable())
            {
                using (OleDbConnection cn = new OleDbConnection(ConString))
                {
                    cn.Open();
                    using(OleDbCommand cmd = new OleDbCommand(sql, cn))
                    {
                        //Method1
                        //OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                        //da.Fill(dt);
                        //da.Dispose();
                        //da = null;
                        // Method2
                        dt.Load(cmd.ExecuteReader());
                    }
                    cn.Close();
                }
                return dt;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: eliminar el estado administrado (objetos administrados)
                }

                // TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
                // TODO: establecer los campos grandes como NULL
                disposedValue = true;
            }
        }

        // // TODO: reemplazar el finalizador solo si "Dispose(bool disposing)" tiene código para liberar los recursos no administrados
        // ~DBFEngine()
        // {
        //     // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
