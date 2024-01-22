using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.IngresosInventario.DTOs
{
    public class IngresoInventarioDTODetails : IngresoInventarioDTOBase
    {
        [Display(Name = "Bodega")]
        public string BodegaNombre { get; set; }
        [Display(Name = "Descripción")]
        public string BodegaDescripcion { get; set; }
        public IEnumerable<IngresoInventarioDetalleDTODetails> Detalle { get; set; }
    }

    public class IngresoInventarioDetalleDTODetails : IngresoInventarioDetalleDTOBase
    {
        [Display(Name = "Código Material")]
        public string MaterialCodigo { get; set; }

        [Display(Name = "Material Nombre")]
        public string MaterialNombre { get; set; }

        [Display(Name = "Unidad de Medida")]
        public string MaterialUM { get; set; }
        [Display(Name = "Precio")]
        public Decimal MaterialPrecio { get; set; }
        [Display(Name = "Nit")]        
        public string ProveedorNit { get; set; }
        [Display(Name = "Nombre")]
        public string ProveedorNombre { get; set; }
        //public int BodegaId { get; set; }
        public DateTime Fecha { get; set; }
        public string DocumentoReferencia { get; set; }
    }
}