using Apphr.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apphr.Application.SolicitudesDespachos.DTOs
{
    public class SolicitudDespachoDTOBase
    {
        [Display(Name = "Solicitud")]
        public string SolicitudDespachoId { get; set; }


        public string Correlativo { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? Fecha { get; set; }


        [Display(Name = "Departamento")]
        public int DepartamentoId { get; set; }
        public Destino Departamento { get; set; }


        [Display(Name = "Sección")]
        public int SeccionId { get; set; }
        public Destino Seccion { get; set; }


        public string Otros { get; set; }
        public string Tipo { get; set; }
        public string Solicito { get; set; }
        public string Jefe { get; set; }
        public string Gerente { get; set; }
        public string Observaciones { get; set; }

        [Display(Name = "Lineas")]
        public int? NumeroLineas { get; set; }
        public string Estatus { get; set; }
    }



    public class SolicitudDespachoDTDTOBase
    {
        public int SolicitudDespachoDTId { get; set; }
        public string SolicitudDespachoId { get; set; }

        public int MaterialId { get; set; }
        public Material Material { get; set; }
       

        [Display(Name = "Cantidad Solicitada")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal? Cantidad { get; set; }
    }
}
