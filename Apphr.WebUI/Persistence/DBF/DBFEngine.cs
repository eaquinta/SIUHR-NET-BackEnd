using System;
using System.Configuration;
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
        
        private int Anio = 0; 
        public DBFEngine() 
        {            
            Anio = Convert.ToInt32(ConfigurationManager.AppSettings["SiahrAnio"].ToString());
            SetYear(Anio);           
        }

        public void SetYear(int? Anio = null)
        {
            this.Anio = Anio ?? DateTime.Now.Year;
            string filePath;
            switch (this.Anio)
            {
                case 2016:
                    filePath = @"\\192.168.0.20\F\Informat\siahr36";
                    break;
                case 2017:
                    filePath = @"\\192.168.0.20\F\Informat\siahr37";
                    break;
                case 2018:
                    filePath = @"\\192.168.0.20\F\Informat\siahr38";
                    break;
                case 2019:
                    filePath = @"\\192.168.0.20\F\Informat\siahr39";
                    break;
                case 2020:
                    filePath = @"\\192.168.0.20\F\Informat\siahr40";
                    break;
                case 2021:
                    filePath = @"\\192.168.0.20\F\Informat\siahr41";
                    break;
                case 2022:
                    filePath = @"\\192.168.0.20\F\Informat\siahr42";
                    break;
                case 2023:
                    filePath = @"\\192.168.0.20\F\Informat\siahr43";
                    break;
                case 2024:
                    filePath = @"\\192.168.0.20\F\Informat\siahr44";
                    break;
                case 2025:
                    filePath = @"\\192.168.0.20\F\Informat\siahr45";
                    break;
                case 2026:
                    filePath = @"\\192.168.0.20\F\Informat\siahr46";
                    break;
                default:
                    //filePath = DefaultConString;
                    throw new ArgumentException("El Periodo "+ Anio +" SIAHR no existe");
                    //break;
            }
            SetConnString(filePath);
        }
        public int GetYear()
        {
            return this.Anio;
        }


        public void SetConnString(string filePath)
        {
            FilePath = filePath;
            ConString = $@"Provider=VFPOLEDB.1;Data Source={FilePath};";
        }

        public DataTable GetDataTable(string FileName)
        {
            string sql = $"SELECT * FROM {FileName}";
            return GetDataSQL(sql);
        }

        public object GetEscalarSQL (OleDbCommand cmd)
        {
            object res;
            try
            {
                using (OleDbConnection cn = new OleDbConnection(ConString))
                {
                    cn.Open();
                    cmd.Connection = cn;
                    res = cmd.ExecuteScalar();
                    cn.Close();
                }
                return res;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable GetDataSQL(OleDbCommand cmd)
        {
            try
            {
                using (DataTable dt = new DataTable())
                {
                    using (OleDbConnection cn = new OleDbConnection(ConString))
                    {
                        cn.Open();
                        //var pre = new OleDbCommand("SET NULL ON", cn);
                        //pre.ExecuteNonQuery();
                        cmd.Connection = cn;
                        dt.Load(cmd.ExecuteReader());
                        cn.Close();
                    }
                    return dt;
                }
            }
            catch (Exception)
            {
                throw;
            }
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

        public DataTable GetDataSQL(string sql)
        {
            try
            {
                using (DataTable dt = new DataTable())
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
                            dt.Load(cmd.ExecuteReader());
                        }
                        cn.Close();
                    }
                    return dt;
                }

            }
            catch (Exception)
            {                
                throw;
            }
        }

        public int ExecNoQuery(string sql, bool setNullOn = true)
        {
            int res = 0;
            using (OleDbConnection cn = new OleDbConnection(ConString))
            {
                cn.Open();
                OleDbCommand cmd;
                if (setNullOn == false)
                {
                    cmd = new OleDbCommand("SET NULL OFF", cn);
                    res = cmd.ExecuteNonQuery();
                }
                cmd = new OleDbCommand(sql, cn);
                res = cmd.ExecuteNonQuery();
            }
            return res;
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
