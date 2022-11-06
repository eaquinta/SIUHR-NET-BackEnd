using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Domain.Entities
{
    [Table("Empleados")]
    public class Empleado
    {
        [Key]
        [ForeignKey("Persona")]
        public int EncargadoId { get; set; }
        public Persona Persona { get; set; }
        public int Codigo { get; set; }
    }
}
