using Apphr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apphr.Application.SolicitudesPedido.DTOs
{
	

	public class SolicitudPedidoDTODetails : SolicitudPedidoDTOBase
    {
		public List<SolicitudPedidoDTOBaseDT> Detalle { get; set; }
	}
    public class SolicitudPedidoDTOBase
    {
		[Display(Name = "Solicitud")]
		public string SolicitudPedidoId { get; set; }

		
		public string Correlativo { get; set; }

		[Display(Name = "Fecha Solicitud")]
		[DataType(DataType.Date)]
		[Required]
		public DateTime? Fecha { get; set; }

		
		public int DepartamentoId { get; set; }


		public Destino Departamento { get; set; }
				

		public int SeccionId { get; set; }


		public Destino Seccion { get; set; }


		[Display(Name = "Solicitó")]
		[Required]
		public string Solicito { get; set; }

		[Display(Name = "Elaboró")]
		[Required]
		public string Elaboro { get; set; }

		[Display(Name = "Jefe Departamento")]
		[Required]
		public string JefeDepartamento { get; set; }

		[Required]
		public string Gerente { get; set; }

		[Required]
		public string Director { get; set; }

		[Required]
		public string Observaciones { get; set; }
		public string FechaImpresion { get; set; }
		public string Estatus { get; set; }
		[Required]
		public string Tipo { get; set; }

  //      [Display(Name = "Código Departamento")]
		//[Required]
		//[Remote("IfCodigoDepartamentoExist", "Destinos", AdditionalFields = "mode", HttpMethod = "POST", ErrorMessage = "Este departamento no se ecuentra registrado")]
		//public string CodigoDepartamento { get; set; }

  //      [Display(Name = "Código Sección")]
		//[Required]
  //      [Remote("IfCodigoSeccionExist", "Destinos", AdditionalFields = "mode", HttpMethod = "POST", ErrorMessage = "Esta sección no se ecuentra registrada")]
		//public string CodigoSeccion { get; set; }
		
	}

	public class SolicitudPedidoDTOBaseDT
	{
		//public string mode { get; set; }

		public int SolicitudPedidoDTId { get; set; }
		public string SolicitudId { get; set; }
		//public SolicitudPedido SolicitudPedidoDF { get; set; }
		
		[Required]
		[Display(Name = "Código")]
		public int MaterialId { get; set; }
		public Material Material { get; set; }

		
		[Required]
		[DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
		public decimal? Cantidad { get; set; }
		

		[Required]
		[DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
		[RegularExpression(@"^\$?\d+(\.(\d{0,4}))?$", ErrorMessage = "El campo {0} debe tener máximo 4 decimales")]
		public decimal? Precio { get; set; }
		public decimal? Valor { get; set; }
	}
}
