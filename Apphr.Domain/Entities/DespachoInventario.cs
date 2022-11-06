using Apphr.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.Domain.Entities
{
    [Table("DespachosInventario")]
    public class DespachoInventario : AuditableEntity
    {
        [Key]
        [MaxLength(15)]
        public string DespachoInventarioId { get; set; } 
        public DateTime Fecha { get; set; }
        [MaxLength(20)]
        public string DocumentoReferencia { get; set; }
        [MaxLength(10)]
        public string TipoDocumentoReferencia { get; set; }

        public DateTime FechaDocumentoReferencia { get; set; }


        [ForeignKey("Departamento")]
        public int DepartamentoId { get; set; }
        public Destino Departamento { get; set; }

        [ForeignKey("DespachoInventarioId")]
        public List<DespachoInventarioDetalle> Detalle { get; set; }
        public int Lineas { get; set; }
        public bool Protegido { get; set; }
    }

    [Table("DespachosInventarioDetalle")]
    public class DespachoInventarioDetalle : AuditableEntity
    {
        [Key]
        [Column(TypeName = "bigint")]
        public int DespachoInventarioDetalleId { get; set; }
        public int Linea { get; set; }

        [MaxLength(15)]
        public string DespachoInventarioId { get; set; }

        [ForeignKey("Material")]
        public int MaterialId { get; set; }
        public Material Material { get; set; }

        [ForeignKey("Proveedor")]
        public int? ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }

        [MaxLength(20)]
        public string Lote { get; set; }
        public DateTime? FechaVencimiento { get; set; }

        [ForeignKey("Paciente")]
        public int? PacienteId { get; set; }
        public Paciente Paciente { get; set; }


        public decimal Cantidad {get;set;}
        public decimal Valor { get; set; }
        public Decimal Precio { get; set; }
        public string Observacion { get; set; }

        public int? ControlMaterialSalaId { get; set; }

        [ForeignKey("MovimientoInventario")]
        public Int64 MovimientoInventarioId { get; set; }
        public MovimientoInventario MovimientoInventario { get; set; }


        [ForeignKey("Bodega")]
        public int BodegaId { get; set; }
        public Bodega Bodega { get; set; }
    }
}
