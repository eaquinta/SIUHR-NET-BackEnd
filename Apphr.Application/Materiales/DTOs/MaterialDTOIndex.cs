using PagedList;

namespace Apphr.Application.Materiales.DTOs
{
    public class MaterialDTOIndex
    {
        public MaterialDTOIxFilter F { get; set; }
        public PagedList<MaterialDTOIxRow> Result { get; set; }
    }
}
