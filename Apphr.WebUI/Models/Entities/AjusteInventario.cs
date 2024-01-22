using Apphr.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
    [Table("AjustesInventario")]
    public class AjusteInventario : AuditableEntity
    {    
        [Key]       
        [MaxLength(15)]
        public string AjusteInventarioId { get; set; }
        
        
        public DateTime Fecha { get; set; }
        
        
        public string DocumentoReferencia { get; set; }
        
        
        public DateTime FechaDocumentoReferencia { get; set; }
        

        [ForeignKey("AjusteInventarioId")]
        public ICollection<AjusteInventarioDetalle> Detalle { get; set; }
        
        
        public int Lineas { get; set; }
        public bool Protegido { get; set; }
    }

    [Table("AjustesInventarioDetalle")]
    public class AjusteInventarioDetalle : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 AjusteInventarioDetalleId { get; set; }
        [MaxLength(15)]
        public string AjusteInventarioId { get; set; }

        public int Linea { get; set; }

        public int TipoMovimientoId { get; set; }
        public TipoMovimientoInventario TipoMovmientoInventario { get; set; }


        [ForeignKey("Bodega")]
        public int BodegaId { get; set; }
        public Bodega Bodega { get; set; }


        [ForeignKey("Material")]
        public int MaterialId { get; set; }
        public Material Material { get; set; }


        [ForeignKey("Proveedor")]
        public int? ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }

        [MaxLength(20)]
        public string Lote { get; set; }
        public DateTime? FechaVencimiento { get; set; }

        public Decimal Cantidad { get; set; }
        public Decimal Precio { get; set; }
        public Decimal Valor { get; set; }

        public string Observacion { get; set; }

        [ForeignKey("MovimientoInventario")]
        public Int64 MovimientoInventarioId { get; set; }
        public MovimientoInventario MovimientoInventario { get; set; }
    }
}
