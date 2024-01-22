using System;
using System.Data;

namespace Apphr.Domain.EntitiesDBF
{
    public class ProveedorDBF
    {
        public string NIT { get; set; }
        public string DESCRI { get; set; }
        public string DIRECC { get; set; }
        public string TELEFO { get; set; }
        public string CONTAC { get; set; }
        public int CREDIT { get; set; }
        public static ProveedorDBF MapDataRow(DataRow dr)
        {
            return new ProveedorDBF()
            {
                NIT = dr["NIT"].ToString().Trim(),
                DESCRI = dr["DESCRI"].ToString().Trim(),
                DIRECC = dr["DIRECC"].ToString().Trim(),
                TELEFO = dr["TELEFO"].ToString().Trim(),
                CONTAC = dr["CONTAC"].ToString().Trim(),
                CREDIT = Convert.ToInt32(dr["CREDIT"].ToString().Trim())
            };
        }
    }
}
