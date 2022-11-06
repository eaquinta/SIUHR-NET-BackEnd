using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apphr.Application.AjustesInventario.DTOs
{
    public class AjusteInventarioDTOCEdit : AjusteInventarioDTOBase
    {
        public AjusteInventarioDTOCEdit()
        {
            Child = new AjusteInventarioDetalleDTOCEdit();
        }

        
        public AjusteInventarioDetalleDTOCEdit Child { get; set; }
    }

    public class AjusteInventarioDetalleDTOCEdit : AjusteInventarioDetalleDTOBase
    {
        [Display(Name = "Bodega")]
        [Required]
        [Remote("JsNombreExist", "Bodegas",
            HttpMethod = "POST",
            ErrorMessageResourceType = typeof(Resources.Msg),
            ErrorMessageResourceName = "Bodega_NotExist")]
        public string BodegaNombre { get; set; }
        
        
        [Display(Name = "Descripción")]
        public string BodegaDescripcion { get; set; }


        [Display(Name = "Código Material")]
        [Required]
        [Remote("JsMaterialCodigoExist", "Materiales",
           HttpMethod = "POST",
           ErrorMessageResourceType = typeof(Resources.Msg),
           ErrorMessageResourceName = "Material_NotExist")]
        public string MaterialCodigo { get; set; }


        [Display(Name = "Material Nombre")]
        public string MaterialNombre { get; set; }
        
        
        [Display(Name = "Unidad de Medida")]
        public string MaterialUM { get; set; }
        
        
        [Display(Name = "Precio")]
        public Decimal MaterialPrecio { get; set; }
        
        
        [Display(Name = "Nit")]
        [Remote("JsProveedorNitExistOpt", "Proveedores", HttpMethod = "POST", ErrorMessage = "Debe ser un Nit de Proveedor valido.")]
        public string ProveedorNit { get; set; }
        
        
        [Display(Name = "Nombre")]
        public string ProveedorNombre { get; set; }
        

        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }
        
        
        public string DocumentoReferencia { get; set; }

        
        [Display(Name = "Existencia")]
        public decimal MaterialExistencia { get; set; }

        
        [Display(Name = "Mínimo")]
        public decimal MaterialMinimo { get; set; }
    }
}
