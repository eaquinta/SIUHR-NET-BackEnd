using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.ControlAbastecimiento.DTOs
{
    public class ControlAbastecimientoDTOIndex
    {
        public int MaterialId { get; set; }
        public string MaterialCodigo { get; set; }
        public string MaterialDescripcion { get; set; }
        public string TipoCompra { get; set; }
        public string MaterialCodigoSiges { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public string SolicitudPedidoId { get; set; }
        public decimal? CantidadSolicitado { get; set; }
        public decimal? PrecioUnitario { get; set; }
        public decimal? ValorTotal { get; set; }
        public string OrdenCompraId { get; set; }
        public int? ProveedorId { get; set; }
        public string ProveedorNombre { get; set; }
        public decimal? CantidadOrdenado { get; set; }
        public DateTime? FechaOrden { get; set; }
        public DateTime? FechaIngresoAlmacen { get; set; }
        public decimal? CantidadIngreso { get; set; }
        public decimal? CantidadEgreso { get; set; }
        public decimal? Existencia
        {
            get
            {
                if (CantidadIngreso == null)
                {
                    return null;
                }
                if (CantidadEgreso == null)
                {
                    return CantidadIngreso;
                }
                else
                {
                    return CantidadIngreso - CantidadEgreso;
                }
            }
        }
        public decimal CantidadPendienteIngreso { get 
            { 
                if (OrdenCompraId == "INICIAL")
                {
                    return (decimal)0.00;
                }
                return (CantidadOrdenado ?? 0) - (CantidadIngreso ?? 0);
            } 
        } 

    }
}
