using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apphr.Application.SolicitudesDespachos.DTOs
{
    public class SolicitudDespachoDTOCEditMS : SolicitudDespachoDTOCEdit
    {
        [Required]
        [Display(Name = "Hoja Control Material No.")]
        [MaxLength(10)]
        public string HojaControlSala { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Fecha Operación")]
        public DateTime FechaOperacion { get; set; }


        [MaxLength(20)]
        public string Servicio { get; set; }


        public int Cama { get; set; }


        public int PacienteId { get; set; }


        [Required]
        [Display(Name = "Registro médico")]
        [Remote("JsPacienteRMExist", "Pacientes", "Medica",
            HttpMethod = "POST",
            ErrorMessageResourceType = typeof(Resources.Msg),
            ErrorMessageResourceName = "PacienteRM_NotExist")]
        public string PacienteRM { get; set; }

        [Display(Name = "Nombre Paciente")]
        public string PacienteNombreCompleto { get; set; }

       
        [MaxLength(100)]
        public string Cirujano { get; set; }

        
        [Display(Name = "Auxiliar de enfermeria instrumentista")]
        [MaxLength(100)]
        public string AuxiliarEnfermeriaInstrumentista { get; set; }

        
        [Display(Name = "Auxiliar de enfermeria Circulante")]
        [MaxLength(100)]
        public string AuxiliarEnfermeriaCirculante { get; set; }


        
        
    }
}
