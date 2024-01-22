using Apphr.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
    [Table("SolicitudPedido")]
    public class SolicitudPedido : AuditableEntity
	{
		[Key]
		[MaxLength(15)]
        public string SolicitudPedidoId { get; set; }
		public string Correlativo { get; set; }
		public DateTime? Fecha { get; set; }		


		[ForeignKey("Departamento")]
		public int DepartamentoId { get; set; }
		public Destino Departamento { get; set; }


		[ForeignKey("Seccion")]
		public int? SeccionId { get; set; }
		public Destino Seccion { get; set; }


		[MaxLength(3)]
		public string Tipo { get; set; }
		public string Solicito { get; set; }
		public string Elaboro { get; set; }
		public string JefeDepartamento { get; set; }
		public string Gerente { get; set; }
		public string Director { get; set; }
		public string Observaciones { get; set; }
		public string FechaImpresion { get; set; }
		public string Estatus { get; set; }
		public bool Protegida { get; set; } = false;
		
		[MaxLength(2)]
		public string TipoEvento { get; set; }

		[ForeignKey("SolicitudPedidoId")]
		public ICollection<SolicitudPedidoDetalle> Detalle { get; set; }


		//[Column("DEPSOL")]
		//[Display(Name = "Departamento")]
		//[Required]
		//public string DepartamentoNombre { get; set; }

		//[Column("SECSOL")]
		//[Display(Name = "Sección")]
		//[Required]
		//public string SeccionNombre { get; set; }

		//[Column("OTRSOL")]
		//[Display(Name = "Elaboró")]
		//[Required]


		//[Display(Name = "Tipo")]
		//public string TIPSOL { get; set; }

		//[Column("SOLSOL")]
		//[Display(Name = "Solicitante.")]
		//[Required]


		//[Column("JEFSOL")]
		//[Display(Name = "Jefe Depto.")]
		//[Required]
		//public string Jefe { get; set; }
		//[Column("GERSOL")]
		//[Display(Name = "Gerente")]

		//[Column("DIRSOL")]
		//[Display(Name = "Director")]
		//[Required]


		//[Column("OBSSOL")]
		//[Required]

		//public string OBSSOM { get; set; }
		//public string OBSSON { get; set; }
		//public string OBSSOO { get; set; }

		//[Column("NLISOL")]
		//[Display(Name = "Lineas")]
		//public int? NumeroLineas { get; set; }

		//[Column("STASOL")]



		//[Column("APROVE")]
		//[Display(Name = "Fecha Impresión")]

		//public string BPROVE { get; set; }

		//[Column("CPROVE")]
		//[Display(Name = "Código Departamento")]
		//[Required]
		//public string CPROVE { get; set; }

		//[Column("DPROVE")]
		//[Display(Name = "Código Sección")]
		//[Required]


		//[NotMapped]
		//public string SMesSolicitud
		//{
		//	get
		//	{
		//		string res;
		//		switch (MESSOL)
		//		{
		//			case 1:
		//				res = "  Enero   ";
		//				break;
		//			case 2:
		//				res = " Febrero  ";
		//				break;
		//			case 3:
		//				res = "  Marzo   ";
		//				break;
		//			case 4:
		//				res = "  Abril   ";
		//				break;
		//			case 5:
		//				res = "   Mayo   ";
		//				break;
		//			case 6:
		//				res = "  Junio   ";
		//				break;
		//			case 7:
		//				res = "  Julio   ";
		//				break;
		//			case 8:
		//				res = "  Agosto  ";
		//				break;
		//			case 9:
		//				res = "Septiembre";
		//				break;
		//			case 10:
		//				res = " Octubre  ";
		//				break;
		//			case 11:
		//				res = "Noviembre ";
		//				break;
		//			case 12:
		//				res = "Diciembre ";
		//				break;
		//			default:
		//				res = "";
		//				break;
		//		}
		//		return res;
		//	}
		//}
		////[NotMapped]
		////public DateTime FechaNL { get { return Fecha.HasValue ? Fecha.Value : DateTime.MinValue; } protected set {}}
		////[NotMapped]
		////[Display(Name = "Fecha Solicitud")]
		////[DataType(DataType.Date)]
		////[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		////[Required]


	}


	[Table("SolicitudPedidoDetalle")]
	public class SolicitudPedidoDetalle : AuditableEntity
	{
		[Key]
		public int SolicitudPedidoDetalleId {get;set;}

		
		public string SolicitudPedidoId { get; set; }


		[MaxLength(15)]
		public string OrdenCompraId { get; set; }


		[ForeignKey("Material")]
		public int MaterialId { get; set; }
		public Material Material { get; set; }
		public string MaterialCodigo { get; set; }		
		public decimal? Cantidad { get; set; }
		public string UnidadMedida { get; set; }
		public decimal? Precio { get; set; }
		public decimal Valor { get; set; }

	}
}
