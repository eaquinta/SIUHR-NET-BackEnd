using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apphr.Application.OrdenesCompra.DTOs
{
    public class OrdenCompraDTODetailsDBF : OrdenCompraDTOBaseDBF
    {
        [Display(Name = "Proveedor Nombre")]
        public string ProveedorNombre { get; set; }
        public List<OrdenCompraDetalleDTODetailsDBF> Detalle { get; set; }
    }
}
