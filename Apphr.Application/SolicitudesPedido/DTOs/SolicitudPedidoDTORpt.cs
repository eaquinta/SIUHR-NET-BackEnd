using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Apphr.Application.SolicitudesPedido.DTOs.SolicitudPedidoDTORpt
namespace Apphr.Application.SolicitudesPedido.DTOs
{
    public class SolicitudPedidoDTORpt
    {
        public string SolicitudNumero { get; set; }
        public decimal Cantidad { get; set; }
        public string UnidadMedida { get; set; }
        public string Descripcion { get; set; }
        public string Elaboro { get; set; }
        public string RenglonCodigo { get; set; }
        public decimal Valor { get; set; }
        public string Observaciones { get; set; }
        public string Jefe { get; set; }
        public string Gerente { get; set; }
        public string Director { get; set; }
        public string CPROVE { get; set; }
        public string DestinoDescripcion { get; set; }
        public DateTime Fecha { get; set; }
        public string MaterialCodigo { get; set; }
        public string SigesCodigo { get; set; }
    }
}
