using Apphr.Application.Common.DTOs;
using System;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.SolicitudesPedido.DTOs
{
    public class SolicitudPedidoDTOIxDBFFilter : DTOJsIxFilter
    {
        [Display(Name = "Año")]
        [UIHint("DropDown")]
        public int Anio { get; set; } = DateTime.Now.Year;
    }
}
