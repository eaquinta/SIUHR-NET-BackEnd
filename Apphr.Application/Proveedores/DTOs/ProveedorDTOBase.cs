using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.Proveedores.DTOs
{
    public class ProveedorDTOBase 
    {
        public ProveedorDTOBase()
        {
            this.IsActive = true;
        }

        public int ProveedorId { get; set; }
        [Display(Name = "Nit Proveedor")]
        [Required]
        public string Nit { get; set; }

        [Display(Name = "Descripción")]
        [Required]
        public string Descripcion { get; set; }

        [Display(Name = "Dirección")]
        [Required]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono")]
        [MaxLength(10)]
        public string Telefono { get; set; }

        [Display(Name = "Contacto")]
        public string Contacto { get; set; }


        [Display(Name = "Días Crédito")]
        public int? DiasCredito { get; set; }


        [Display(Name = "e-mail")]
        [MaxLength(25)]
        public string Email { get; set; }

        [Display(Name = "Banco #1")]
        [MaxLength(20)]
        public string Banco1 { get; set; }


        [Display(Name = "Cuenta #1")]
        [MaxLength(20)]
        public string Cuenta1 { get; set; }


        [Display(Name = "Banco #2")]
        [MaxLength(20)]
        public string Banco2 { get; set; }


        [Display(Name = "Cuenta #2")]
        [MaxLength(20)]
        public string Cuenta2 { get; set; }


        [Display(Name = "Banco #3")]
        [MaxLength(20)]
        public string Banco3 { get; set; }


        [Display(Name = "Cuenta #3")]
        [MaxLength(20)]
        public string Cuenta3 { get; set; }

        [Display(Name = "Registro habilitado")]
        [UIHint("Switch")]
        public bool IsActive { get; set; }
    }
}
