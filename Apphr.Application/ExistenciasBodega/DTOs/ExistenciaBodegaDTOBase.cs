using Apphr.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.ExistenciasBodega.DTOs
{
    public class ExistenciaBodegaDTOBase
    {        
        //[Display()]
        public int ExistenciaBodegaId { get; set; }
        public int BodegaId { get; set; }
        public Bodega Bodega { get; set; }     
        public int MaterialId { get; set; }
        public Material Material { get; set; }     
        public int? ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; } 
        [MaxLength(20)]
        public string Lote { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? FechaVencimiento { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Valor { get; set; }
        public decimal Minimo { get; set; }
        public decimal Maximo { get; set; }
        public decimal Pendiente { get; set; }
    }
}
