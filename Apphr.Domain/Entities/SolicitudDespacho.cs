using Apphr.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.Domain.Entities
{

	[Table("SolicitudDespacho")]
	public class SolicitudDespacho : AuditableEntity
	{
		[Key]
		[MaxLength(15)]
		//[Column("NUMDES")]
		//[Display(Name = "Número Despacho")]
		public string SolicitudDespachoId { get; set; }

		//[Column("CORDES")]
		public string Correlativo { get; set; }

		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		[DataType(DataType.Date)]
		public DateTime? Fecha { get; set; }

		[ForeignKey("Departamento")]
		public int DepartamentoId { get; set; }
		public Destino Departamento { get; set; }


		[ForeignKey("Seccion")]
		public int SeccionId { get; set; }
		public Destino Seccion { get; set; }


		//[Column("OTRDES")]
		public string Otros { get; set; }

		//[Column("TIPDES")]
		public string Tipo { get; set; }

		//[Column("SOLDES")]
		public string Solicito { get; set; }

		//[Column("JEFDES")]
		public string Jefe { get; set; }

		//[Column("GERDES")]
		public string Gerente { get; set; }

		//[Column("OBSDES")]
		public string Observacion { get; set; }

		//[Column("NLIDES")]			
		public int? Lineas { get; set; }

		//[Column("STADES")]
		public string Estatus { get; set; }


		[ForeignKey("SolicitudDespachoId")]
		public virtual ICollection<SolicitudDespachoDetalle> Detalle { get; set; }
		//public string DIRDES { get; set; }
		//public string SITDES { get; set; }
		//public string DESPAC { get; set; }

		//public int? ANODES { get; set; }
		//public int? MESDES { get; set; }
		//public int? DIADES { get; set; }

		[MaxLength(15)]
		public string DespachoInventarioId { get; set; }

		public bool Protegido { get; set; }
	}



	[Table("SolicitudDespachoDetalle")]
	public class SolicitudDespachoDetalle : AuditableEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int SolicitudDespachoDTId { get; set; }
		
		
		[MaxLength(15)]
		public string SolicitudDespachoId { get; set; }


		[ForeignKey("Material")]
		public int MaterialId { get; set; }
		
		
		public Material Material { get; set; }
		
		
		public decimal Cantidad { get; set; }

		public decimal CantidadDespachada { get; set; }
	}
}
