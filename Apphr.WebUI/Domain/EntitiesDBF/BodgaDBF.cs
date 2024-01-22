using System.Data;

namespace Apphr.Domain.EntitiesDBF
{
    public class BodegaDBF
    {
        public string CODIGO { get; set; }
        public string DESCRI { get; set; }
        public string PROCED { get; set; }
        public string BODMAT { get; set; }
		public static BodegaDBF MapDataRow(DataRow dr)
		{
			return new BodegaDBF()
			{
                CODIGO = dr["CODIGO"].ToString().Trim(),
                DESCRI = dr["DESCRI"].ToString().Trim(),
                PROCED = dr["PROCED"].ToString().Trim(),
                BODMAT = dr["BODMAT"].ToString().Trim(),
            };
		}
        public string CmdInsert()
        {
            return $@"INSERT INTO BODEGAS.DBF (CODIGO, DESCRI, PROCED ,BODMAT) VALUES('{CODIGO}','{DESCRI}','{PROCED}','{BODMAT}')";
        }
        public string CmdDelete()
        {
            return $@"DELETE FROM BODEGAS.DBF WHERE CODIGO = '{CODIGO}'";
        }
        public string CmdUpdate()
        {
            return $@"UPDATE BODEGAS.DBF SET DESCRI = '{DESCRI}', PROCED = '{PROCED}', BODMAT = '{BODMAT}' WHERE CODIGO = '{CODIGO}'";
        }
    }
}
