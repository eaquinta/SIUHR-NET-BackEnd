//Apphr.Application.SolicitudesDespachos.DTOs.SolicitudDespachoDTORpt
using System;

namespace Apphr.Application.SolicitudesDespachos.DTOs
{
    public class SolicitudDespachoDTORpt
    {
        public string SolicitudDespachoNumero { get; set; }
        public decimal CantidadSolicitada { get; set; }
        public string UnidadMedida { get; set; }
        public string Descripcion { get; set; }
        public decimal CantidadExistencia { get; set; }
        public DateTime Fecha { get; set; }
        public string Departamento { get; set; }
        public string TipoDespacho { get; set; }
        public string Solicito { get; set; }
        public string Jefe { get; set; }
        public string Gerente { get; set; }
        public string Director { get; set; }
        public string Bodega { get; set; }
        public string NumeroFormatoAutorizado { get; set; }

    }
}
