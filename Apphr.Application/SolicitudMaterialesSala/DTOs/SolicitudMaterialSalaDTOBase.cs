using Apphr.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apphr.Application.SolicitudMaterialesSala.DTOs
{
    public class SolicitudMaterialSalaDTOBase
    {
        [Display(Name = "Solicitud Material Sala No.")]
        [MaxLength(15)]
        public string SolicitudMaterialSalaId { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Operación")]
        public DateTime FechaOperacion { get; set; }
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required]
        [Display(Name = "Hoja Control Material No.")]
        public string HojaControlSala { get; set; }
        
        public int Cama { get; set; }
        public int PacienteId { get; set; }
        
        public Paciente Paciente { get; set; }
        [MaxLength(20)]
        public string Servicio { get; set; }

        [MaxLength(100)]
        public string Cirujano { get; set; }

        [MaxLength(100)]
        [Display(Name = "Auxiliar de enfermeria instrumentista")]
        public string AuxiliarEnfermeriaInstrumentista { get; set; }

        [MaxLength(100)]
        [Display(Name = "Auxiliar de enfermeria Circulante")]
        public string AuxiliarEnfermeriaCirculante { get; set; }


        [Display(Name = "Observación")]
        [DataType(DataType.MultilineText)]
        public string Observacion { get; set; }


        [Display(Name = "Despacho No.")]
        [MaxLength(15)]
        public string DespachoInventarioId { get; set; }
        public int Lineas { get; set; }
        public bool Protegido { get; set; }
    }

    public class SolicitudMaterialSalaDetalleDTOBase
    {
        public int SolicitudMaterialSalaDetalleId { get; set; }
        
        [MaxLength(15)]
        public string SolicitudMaterialSalaId { get; set; }

        public int MaterialId { get; set; }
        
        
        public Material Material { get; set; }

        
        public decimal Cantidad { get; set; }


        [Display(Name ="Despachado")]
        public decimal CantidadDespachada { get; set; }
        //public string CasaComercial { get; set; }


        [Display(Name = "Es Intercambio")]
        public bool Intercambio { get; set; }
        
        
        public int linea { get; set; }

        
        public int? ProveedorId { get; set; }

        
        public Proveedor Proveedor { get; set; }
    }
}
