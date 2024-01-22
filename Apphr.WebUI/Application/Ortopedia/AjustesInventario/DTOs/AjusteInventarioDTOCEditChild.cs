using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apphr.Application.Ortopedia.AjustesInventario.DTOs
{
    public class AjusteInventarioDTOCEditChild : MovimientoDTOBase
    {
        public string Mode { get; set; }

        //[Display(Name = "Bodega")]                                              
        //[Required]
        //[Remote("JsNombreExist", "Bodegas", "Inventario",
        //    HttpMethod = "POST",
        //    ErrorMessageResourceType = typeof(Resources.Msg),
        //    ErrorMessageResourceName = "Bodega_NotExist")]
        public string BodegaNombre { get; set; }                                // BodegaNombre


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
        public string MaterialCodigo { get; set; }

        [Display(Name = "Precio")]
        public decimal MaterialPrecio { get; set; }

        [Display(Name = "Unidad de Medida")]
        public string UnidadMedida { get; set; }

        public Int64 AjusteInventarioId { get; set; }

        [Display(Name ="Código Bodega")]
        [UIHint("DropDown")]
        [Required]
        public int BodegaId { get; set; }

        [UIHint("DropDown")]
        [Required]
        [Display(Name = "Código Material")]                             // ProveedorNombre
        public new int MaterialId { get; set; }
        
        [Display(Name = "Nombre")]                                      // ProveedorNombre
        public string ProveedorNombre { get; set; }

        [Display(Name = "Orden Compra #")]
        public int NumeroOC { get; set; }

        [Display(Name = "Fecha O.C.")]
        [DataType(DataType.Date)]
        public DateTime FechaOC { get; set; }

        [Display(Name = "Año O.C.")]
        public int AnioOC { get; set; }


        [Display(Name = "Nit")]
        public string ProveedorNit { get; set; }

    }
}
