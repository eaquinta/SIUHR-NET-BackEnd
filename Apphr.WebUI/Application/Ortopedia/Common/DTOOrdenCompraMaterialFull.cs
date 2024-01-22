using System;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Ortopedia.Common
{
    public class DTOOrdenCompraMaterialFull
    {
        public Int64? SolicitudPedidoId { get; set; }

        public Int64? OrdenCompraId { get; set; }

        public int MaterialId { get; set; }

        public Decimal Precio { get; set; }
        public Decimal Solicitado { get; set; }
        public Decimal Ordenado { get; set; }
        public Decimal Despachado { get; set; }
        public Decimal Pendiente { get; set; }
        public Decimal? PrecioOC { get; set; }
        
        public int? NumeroOC { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaOC { get; set; }

        [Display(Name = "Año O.C.")]
        public int? AnioOC { get; set; }         
    }
}
