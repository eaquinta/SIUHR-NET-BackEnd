using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apphr.Application.Ortopedia.IngresosInventario.DTOs
{
    public class IngresoInventarioDTOCEditChild : MovimientoDTOBase
    {
        [Display(Name = "Bodega")]                                              // BodegaNombre
        [Required]
        [Remote("JsNombreExist", "Bodegas", "Inventario",
            HttpMethod = "POST",
            ErrorMessageResourceType = typeof(Resources.Msg),
            ErrorMessageResourceName = "Bodega_NotExist")]
        public string BodegaNombre { get; set; }


        [Display(Name = "Descripción")]                                         // BodegaDescripcion
        public string BodegaDescripcion { get; set; }

        [Display(Name = "Material Nombre")]
        public string MaterialNombre { get; set; }
                
        //[Display(Name = "Código")]
        //[Required]
        //[Remote("JsMaterialCodigoExist", "Materiales", "Inventario",
        //    HttpMethod = "POST",
        //    ErrorMessageResourceType = typeof(Resources.Msg),
        //    ErrorMessageResourceName = "Material_NotExist")]
        [UIHint("DropDown")]
        [Display(Name ="Código")]
        public new int MaterialId { get; set; }
        public string MaterialCodigo { get; set; }

        //[Display(Name = "Precio")]
        //public decimal MaterialPrecio { get; set; }

        [Display(Name = "Unidad de Medida")]
        public string UnidadMedida { get; set; }

        public string Mode { get; set; }

        public Int64 IngresoInventarioId { get; set; }

        public int BodegaId { get; set; }

        public decimal Solicitado { get; set; }                                 // Solicitado

        public decimal Ordenado { get; set; }                                   // Ordenado

        public decimal Pendiente { get; set; }                                  // Pendiente

        //public decimal PendienteReal { get { return Pendiente + Cantidad; } }   // PendienteReal 
    }
}
