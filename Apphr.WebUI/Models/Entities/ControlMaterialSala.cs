using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apphr.WebUI.Models.Entities
{
    [Table("ControlesMaterialSala")]
    public class ControlMaterialSala
    {
        [Key]
        public int ControlMaterialSalaId { get; set; }
        public DateTime FechaOperacion { get; set; }
        public DateTime Fecha { get; set; }
        public string HojaControlSala { get; set; }

        public int Cama { get; set; }

        [ForeignKey("Paciente")]
        public int PacienteId {get;set;}
        public Paciente Paciente { get; set; }
        [MaxLength(20)]
        public string Servicio { get; set; }

        [MaxLength(100)]
        public string Cirujano {get;set;}

        [MaxLength(100)]
        public string AuxiliarEnfermeriaInstrumentista { get; set; }

        [MaxLength(100)]
        public string AuxiliarEnfermeriaCirculante { get; set; }
        public string Observacion { get; set; }
        [MaxLength(15)]
        public string DespachoInventarioId { get; set; }
        public int Lineas { get; set; }
        [ForeignKey("ControlMaterialSalaId")]
        public List<ControlMaterialSalaDetalle> Detalle { get; set; }
    }
    [Table("ControlesMaterialSalaDetalle")]
    public class ControlMaterialSalaDetalle
    {
        [Key]
        public int ControlMaterialSalaDetalleId { get; set; }
        public int ControlMaterialSalaId { get; set; }

        [ForeignKey("Material")]
        public int MaterialId { get; set; }
        public Material Material { get; set; }

        public decimal Cantidad { get; set; }
        
        [ForeignKey("Proveedor")]
        public int? ProveedorId { get; set; }
        public Proveedor Proveedor {get;set;}

        public string CasaComercial { get; set; }
        public bool Intercambio { get; set; }
        public int linea { get; set; }        
    }
}
