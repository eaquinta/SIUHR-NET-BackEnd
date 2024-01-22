using Apphr.WebUI.Models.Entities;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Apphr.WebUI.Models.Repository
{
    public class SHRMaterialRepository : GenericRepository<SHRMaterial>
    {
        public SHRMaterialRepository(ApphrDbContext context) : base(context)
        {
        }

        public async Task<SHRMaterial> GetMaterialByCodigoAsync(string codigo)
        {
            return await db.SHRMaterial.Where(x => x.Codigo == codigo).FirstOrDefaultAsync();
        }
    }
}