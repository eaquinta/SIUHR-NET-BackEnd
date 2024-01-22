using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
	[Table("SHDContadores")]
	public class SHDContador
    {
		[Key]													// AdministracionId (*) [KEY]
		public int AdministracionId { get; set; }		

		public int Ingreso { get; set; }						// [ING]
		public int Despacho { get; set; }                       // [DES]
		public int Devolucion { get; set; }                     // [DEV]
		public int Traslado { get; set; }                       // [TRA]
		public int Ajuste { get; set; }                         // [AJU]
		public int Apertura { get; set; }                       // [APE]
		public int Proceso { get; set; }                        // [PRO]
	}
}
