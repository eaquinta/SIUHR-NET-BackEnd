using System;

namespace Apphr.Application.Ortopedia.OrdenesCompra.DTOs
{
    public class OrdenCompraDTOStatus
    {
        public Int64 OrdenCompraId { get; set; }
        public int MaterialId { get; set; }
        public decimal Solicitado { get; set; }
        public decimal Ordenado { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Ingresado { get; set; }
        public decimal Despachado { get; set; }
        public decimal Pendiente { get { return Ingresado - Despachado; } }
    }
}
