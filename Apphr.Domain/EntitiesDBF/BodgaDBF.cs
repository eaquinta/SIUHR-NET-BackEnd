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
                CODIGO = dr["CODIGO"].ToString(),
                DESCRI = dr["DESCRI"].ToString(),
                PROCED = dr["PROCED"].ToString(),
                BODMAT = dr["BODMAT"].ToString()
            };
		}
	}
}
