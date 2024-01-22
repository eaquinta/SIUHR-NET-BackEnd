using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apphr.Application.Ortopedia.HojaGasto.DTOs
{
    public class HojaGastoDTOCEditChild : MovimientoDTOBase
    {
        [Display(Name = "Material Nombre")]
        public string MaterialNombre { get; set; }

        [Display(Name = "Código")]
        [Required]
        [Remote("JsMaterialCodigoExist", "Materiales", "Inventario",
            HttpMethod = "POST",
            ErrorMessageResourceType = typeof(Resources.Msg),
            ErrorMessageResourceName = "Material_NotExist")]
        public string MaterialCodigo { get; set; }

        [Display(Name = "Precio")]
        public decimal MaterialPrecio { get; set; }

        [Display(Name = "Unidad de Medida")]
        public string UnidadMedida { get; set; }

        //[Display(Name = "Nit")]                                         // Nit
        //[Remote("JsProveedorNitExistOpt", "Proveedores", "Inventario",
        //    HttpMethod = "POST",
        //    ErrorMessage = "Debe ser un Nit de Proveedor valido.")]
        //public string ProveedorNit { get; set; }
        [Required]
        [Display(Name = "Proveedor")]
        [UIHint("DropDown")]
        public new int? ProveedorId { get; set; }

        [Display(Name = "Nombre")]                                      // ProveedorNombre
        public string ProveedorNombre { get; set; }

        public Int64 HojaGastoId { get; set; }

        public string Mode { get; set; }

        public string ProveedorNit { get; set; }        
    }
}
