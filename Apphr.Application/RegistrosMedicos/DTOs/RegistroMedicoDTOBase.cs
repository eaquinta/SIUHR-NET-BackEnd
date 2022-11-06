using Apphr.Application.Personas.DTOs;
using Apphr.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.RegistrosMedicos.DTOs
{
    public class RegistroMedicoDTOBase
    {
        [Display(Name = "RMI")]
        public string RegistroMedicoId { get; set; }
        
        //public string RegMedCod { get; set; }
        public PersonaDTOBase Person { get; set; }
       
    }
}
