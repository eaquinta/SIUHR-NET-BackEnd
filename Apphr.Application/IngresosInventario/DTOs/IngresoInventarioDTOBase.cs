using Apphr.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.IngresosInventario.DTOs
{
    public class IngresoInventarioDTOBase
    {
        [Display(Name = "Ingreso")]
        public string IngresoInventarioId { get; set; }


        [Display(Name = "Fecha Ingreso")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }


        [Display(Name = "Documento Ingreso")]
        public string DocumentoReferencia { get; set; }


        [Display(Name = "Fecha Emisión Documento")]
        [DataType(DataType.Date)]
        public DateTime FechaDocumentoReferencia { get; set; }

        public int Lineas { get; set; }
    }


    public class IngresoInventarioDetalleDTOBase
    {
        
        public int IngresoInventarioDetalleId { get; set; }

        [MaxLength(15)]
        public string IngresoInventarioId { get; set; }

        public int BodegaId { get; set; }
        public Bodega Bodega { get; set; }


        public int MaterialId { get; set; }
        public Material Material { get; set; }

        public int? ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }
        [MaxLength(20)]
        public string Lote { get; set; }

        [Display(Name = "Fecha Vencimiento")]
        [DataType(DataType.Date)]
        public DateTime? FechaVencimiento { get; set; }

        public Decimal Cantidad { get; set; }
        public Decimal Precio { get; set; }
        public Decimal Valor { get; set; }

        
        [Display(Name = "Observación")]
        [DataType(DataType.MultilineText)]
        public string Observacion { get; set; }
        public int Linea { get; set; }
    }
}
