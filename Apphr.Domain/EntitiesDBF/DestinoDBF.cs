using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Apphr.Domain.EntitiesDBF
{
    public class DestinoDBF
    {
		[Display(Name = "Código")]
		public string CODIGO { get; set; }
		[Display(Name = "Descripción")]
		public string DESCRI { get; set; }

        public string ADMINI { get; set; }
		
		[Display(Name = "Jefe Servicio")]
		public string JEFSER { get; set; }
		public static DestinoDBF MapDataRow(DataRow dr)
		{
			return new DestinoDBF()
			{
				CODIGO = dr["CODIGO"].ToString().Trim(),
				DESCRI = dr["DESCRI"].ToString().Trim(),
				ADMINI = dr["ADMINI"].ToString().Trim(),
				JEFSER = dr["JEFSER"].ToString().Trim(),
				//PRECIO = Convert.ToDecimal(dr["PRECIO"].ToString()),
			};
		}
	}	
}
