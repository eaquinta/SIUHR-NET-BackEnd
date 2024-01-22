using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apphr.Application.SiahadIngreso.DTOs
{
    public class SiahadIngresoDTOCreate
    {
        public int? Documento { get; set; }                     // Docuemnto
        public int? Correlativo { get; set; }                   // Correlativo

        [Required, Display(Name = "Administración")]            // AdministracionId
        [UIHint("DropDown")]
        public int AdministracionId { get; set; }

        [Display(Name = "Documento Ingreso")]                   // Documento Ingreso
        [Required]
        public string DocumentoIngreso { get; set; }

        [Display(Name = "Fecha Emisión")]                       // Fecha Emision
        [DataType(DataType.Date)]
        public DateTime FechaEmision { get; set; }

        public int MaterialId { get; set; }                         // MaterialId

        [Display(Name = "Código Material")]                         // MaterialCodigo [CODIGO]
        [StringLength(14), Required]
        [Remote("JsMaterialCodigoExist", "Siahad",
            HttpMethod = "POST",
            ErrorMessageResourceType = typeof(Resources.Msg),
            ErrorMessageResourceName = "Material_NotExist")]
        public string MaterialCodigo { get; set; }

        [Display(Name = " ")]
        public string MaterialCodigi { get; set; }

        [Display(Name = "Nombre Material")]                         // MaerialNombre
        public string MaterialNombre { get; set; }

        [Display(Name = "Unidad de Medida")]                        // MaerialUnidadMedida
        public string MaterialUnidadMedida {get;set;}

        [Display(Name = "Producto")]                                // MaerialUnidadMedida
        public string MaterialProducto { get; set; }

        [Display(Name = "Precio")]                                  // MaerialPrecio
        public decimal? MaterialPrecio { get; set; }

        [Display(Name = "Cantidad")]                                // MaerialCantidad
        public decimal? MaterialCantidad { get; set; }

        [Display(Name = "Valor")]                                   // MaerialValor
        public decimal? MaterialValor { get; set; }

        public int BodegaId { get; set; }                           // BodegaId


        [Display(Name = "Código Bodega")]                           // BodegaCodigo
        [StringLength(5), Required]
        [Remote("JsBodegaCodigoExist", "Siahad",
            HttpMethod = "POST",
            AdditionalFields = "AdministracionId",
            ErrorMessageResourceType = typeof(Resources.Msg),
            ErrorMessageResourceName = "Bodega_NotExist")]
        public string BodegaCodigo { get; set; }        

        [Display(Name = "Nombre Bodega")]                           // BodegaNombre

        public string BodegaNombre { get; set; }

        public decimal Cantidad { get; set; }
        public decimal Valor { get; set; }


        [Display(Name = "Fecha Vencimiento")]                       // Fecha Vencimiento
        [DataType(DataType.Date)]
        public DateTime? FechaVencimiento { get; set; }

        [Display(Name = "Fecha Movimiento")]                       // Fecha Vencimiento
        [DataType(DataType.Date)]
        public DateTime FechaMovimiento { get; set; }
    }
}
