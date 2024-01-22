using Apphr.WebUI.Models.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apphr.Application.Ortopedia.OrdenesCompra.DTOs
{
    public class OrdenCompraDTOBase
    {
        public Int64 OrdenCompraId { get; set; }						// OrdenCompraId
		        
        [Display(Name = "Número de Orden")]
		[Required]
        public int Numero { get; set; }

        [Display(Name = "Año")]                                         // Anio 
        public int Anio { get; set; }

        public int Lineas { get; set; }                                 // Lineas

		[Display(Name = "Fecha Orden")]
		[DataType(DataType.Date)]
		[Required]
		public DateTime Fecha { get; set; }

		[Display(Name = "Número de Solicitud")]							// NumeroOC
		public int NumeroSP { get; set; }

		[Display(Name = "Año")]											// NumeroOC
		public int AnioSP { get; set; }

		[Display(Name = "Fecha de Solicitud")]							// FechaOC
		[DataType(DataType.Date)]
		public DateTime? FechaSP { get; set; }

		[Display(Name = "Nit")]											// Nit
		[Remote("JsProveedorNitExistOpt", "Proveedores", "Inventario",
			HttpMethod = "POST",
			ErrorMessage = "Debe ser un Nit de Proveedor valido.")]
		public string ProveedorNit { get; set; }

		[Display(Name = "Nombre")]										// ProveedorNombre
		public string ProveedorNombre { get; set; }


		public Int64 SolicitudPedidoId { get; set; }					// SolicitudPedidoId



		public int DepartamentoId { get; set; }


		public Destino Departamento { get; set; }


		


		public Destino Seccion { get; set; }


		//[Display(Name = "Solicitó")]
		//[Required]
		//public string Solicito { get; set; }


		//[Display(Name = "Elaboró")]
		//[Required]
		//public string Elaboro { get; set; }


		//[Display(Name = "Jefe Departamento")]
		//[Required]
		//public string JefeDepartamento { get; set; }


		//[Required]
		//public string Gerente { get; set; }


		//[Required]
		//public string Director { get; set; }

		[Display(Name = "Observación")]
		[DataType(DataType.MultilineText)]
		public string Observacion { get; set; }


		public string FechaImpresion { get; set; }


		public string Estatus { get; set; }

		public int ProveedorId { get; set; }
	}
}
