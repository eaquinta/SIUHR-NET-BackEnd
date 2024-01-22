using Apphr.WebUI.Models.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Ortopedia.SolicitudesPedido.DTOs
{
    public class SolicitudPedidoDTOBase
    {
        public Int64 SolicitudPedidoId { get; set; }					// SolicitudPedidoId

        [Display(Name = "Número de Solicitud")]                         // Numero
		[Required]
        public int Numero { get; set; }

        [Display(Name = "Año")]                                         // Anio 
        public int Anio { get; set; }

        public int Lineas { get; set; }                                 // Lineas

		[Display(Name = "Fecha Solicitud")]
		[DataType(DataType.Date)]
		[Required]
		public DateTime Fecha { get; set; }

		[Display(Name = "Número Orden de Compra")]						// NumeroOC
		public int? NumeroOC { get; set; }

		[Display(Name = "Fecha Orden de compra")]                       // FechaOC
		[DataType(DataType.Date)]
		public DateTime? FechaOC { get; set; }

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

		[Display(Name = "Observación")]
		[DataType(DataType.MultilineText)]
		public string Observacion { get; set; }


		public string FechaImpresion { get; set; }


		public string Estatus { get; set; }


		[Required]
		[Display(Name = "Tipo Prioridad")]
		[UIHint("DropDown")]
		public string TipoPrioridad { get; set; }


		[MaxLength(2)]
		[Display(Name = "Tipo Evento")]
		[UIHint("DropDown")]
		public string TipoEvento { get; set; }
	}
}
