using Apphr.Domain.Entities;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Apphr.WebUI.Models.Repository
{
    public class AjusteInventarioDetalleRepository : GenericRepository<AjusteInventarioDetalle>
    {
        public AjusteInventarioDetalleRepository(ApphrDbContext context) : base(context)
        {
        }

        public async Task<AjusteInventarioDetalle> GetChildAsync(int? id)
        {
            return await db.AjustesInventarioDetalle
                   .Include("Material")
                   .Include("Proveedor")
                   .Include("Bodega")
                   .Where(x => x.AjusteInventarioDetalleId == id)
                   .FirstOrDefaultAsync();
        }
    }
}