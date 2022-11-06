using Apphr.Application.Common;
using PagedList;

namespace Apphr.Application.IngresosInventario.DTOs
{
    public class IngresoInventarioDTOIndex 
    {
        public IxFilter F { get; set; }
        public PagedList<IngresoInventarioDTOIxRow> Result { get; set; }
    }
}
