using Apphr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.AjustesInventario.DTOs
{
    public class AjusteInventarioDTOBase
    {
        [Display(Name ="#Ajuste")]
        public string AjusteInventarioId { get; set; }

        
        [Display(Name = "Fecha Ingreso")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        
        [Display(Name = "Documento Ref.")]
        public string DocumentoReferencia { get; set; }

        
        [Display(Name = "Fecha Documento Ref.")]
        [DataType(DataType.Date)]
        public DateTime FechaDocumentoReferencia { get; set; }       


        public ICollection<AjusteInventarioDetalle> Detalle { get; set; }
        
        
        public int Lineas { get; set; }
    }

    public class AjusteInventarioDetalleDTOBase
    {
        public Int64 AjusteInventarioDetalleId { get; set; }


        public string AjusteInventarioId { get; set; }


        public int Linea { get; set; }


        [Display(Name = "Ajuste por")]
        [UIHint("DropDown")]       
        public int TipoMovimientoId { get; set; }


        public TipoMovimientoInventario TipoMovmientoInventario { get; set; }


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

        
        public Int64 MovimientoInventarioId { get; set; }
        
        
        public MovimientoInventario MovimientoInventario { get; set; }
    }
}
