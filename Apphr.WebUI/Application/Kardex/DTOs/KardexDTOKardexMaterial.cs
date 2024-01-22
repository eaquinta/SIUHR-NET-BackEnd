using Apphr.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Apphr.Application.Kardex.DTOs.KardexDTOKardexMaterial
namespace Apphr.Application.Kardex.DTOs
{
    public class KardexDTOKardexMaterial 
    {
        public KardexDTOSaldo SaldoAnterior { get; set; }
        public IEnumerable<KardexDTOBase> Detalle { get; set; }
        public KardexDTOSaldo SaldoActual { get; set; }
        public IEnumerable<KardexDTOResumenMovimiento> Resumen { get; set; }
        public int MaterialId { get; set; }
        public Material Material { get; set; }
        public int BodegaId { get; set; }
        public Bodega Bodega { get; set; }
    }
}
