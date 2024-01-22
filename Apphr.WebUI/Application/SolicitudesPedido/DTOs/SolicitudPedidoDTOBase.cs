using Apphr.WebUI.Models.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apphr.Application.SolicitudesPedido.DTOs
{
    public class SolicitudPedidoDTOBase
    {
		[Display(Name = "Solicitud Pedido")]
		public string SolicitudPedidoId { get; set; }

		
		public string Correlativo { get; set; }

		[Display(Name = "Fecha Solicitud")]
		[DataType(DataType.Date)]
		[Required]
		public DateTime? Fecha { get; set; }

		
		public int DepartamentoId { get; set; }


		public Destino Departamento { get; set; }
				

		public int? SeccionId { get; set; }


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

				
		public string Observaciones { get; set; }


		public string FechaImpresion { get; set; }


		public string Estatus { get; set; }


		[Required]
		[UIHint("DropDown")]
		public string Tipo { get; set; }


		[MaxLength(2)]
		[Display(Name = "Tipo Evento")]
		[UIHint("DropDown")]
		public string TipoEvento { get; set; }

	}

	public class SolicitudPedidoDetalleDTOBase
	{	

		public int SolicitudPedidoDetalleId { get; set; }

		public string SolicitudPedidoId { get; set; }
		
		
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
