using Apphr.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Apphr.Application.Personas.DTOs
{
    public class PersonaDTOBase
    {
        public int PersonId { get; set; }

        [Required]
        [Display(Name = "Primer Nombre")]
        public string FirstName { get; set; }

        [Display(Name = "Segundo Nombre")]
        public string MiddleName { get; set; }

        [Display(Name = "Tercer Nombre")]
        public string ThirdName { get; set; }

        [Display(Name = "Primer Apellido")]
        public string LastName { get; set; }

        [Display(Name = "Segundo Apellido")]
        public string MatriName { get; set; }

        [Display(Name = "Apellido de Casada")]
        public string MarriedName { get; set; }

        [Display(Name = "Fecha Nacimiento")]
        [DataType(DataType.Date)]
        public DateTime? Birth { get; set; }
        

        [Display(Name = "Nombre Completo")]
        public string Name { get; set; }

        public string Image { get; set; }
        public DateTime? ImageDate { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }

        [Display(Name = "Genero")]
        [UIHint("DropDown")]
        public Gender? Gender { get; set; }

        [Display(Name = "Estado Civil")]
        [UIHint("DropDown")]
        public EstadoCivil EstadoCivil { get; set; }

        [Display(Name = "Regligión")]
        [UIHint("DropDown")]
        public Religion? Regligion { get; set; }

        [Display(Name = "Etnia")]
        [UIHint("DropDown")]
        public Etnia? Etnia { get; set; }

        [Display(Name = "Registro habilitado")]
        [UIHint("Switch")]
        public bool IsActive { get; set; }
    }
}
