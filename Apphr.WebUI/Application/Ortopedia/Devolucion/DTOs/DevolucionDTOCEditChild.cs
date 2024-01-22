using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apphr.Application.Ortopedia.Devolucion.DTOs
{
    public class DevolucionDTOCEditChild : MovimientoDTOBase
    {
        [UIHint("DropDown")]
        [Display(Name = "Código")]
        public new int MaterialId { get; set; }

        [Display(Name = "Material Nombre")]
        public string MaterialNombre { get; set; }

        //[Display(Name = "Código")]
        //[Required]
        //[Remote("JsMaterialCodigoExist", "Materiales", "Inventario",
        //    HttpMethod = "POST",
        //    ErrorMessageResourceType = typeof(Resources.Msg),
        //    ErrorMessageResourceName = "Material_NotExist")]
        public string MaterialCodigo { get; set; }

        [Display(Name = "No de Formulario")]
        [UIHint("DropDown")]
        public Int64 HojaGastoId { get; set; }

        
        public string HGFormulario { get; set; }
        [Display(Name = "Paciente")]
        public string HGPaciente { get; set; }
        [Display(Name = "Registro Médico")]
        public string HGRegistroMedico { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha Formulario")]
        public DateTime? HGFechaFormulario { get; set; }        

        public string Mode { get; set; }
    }
}
