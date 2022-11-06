using Apphr.Domain.Entities;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Apphr.WebUI.Models.Repository
{
    public class MaterialRepository : GenericRepository<Material>
    {
        public MaterialRepository(ApphrDbContext context) : base(context)
        {
        }

        public async Task<Material> GetMaterialByCodigoAsync(string codigo)
        {
            return await db.Materiales.Where(x => x.Codigo == codigo).FirstOrDefaultAsync();
        }

        public decimal GetMateialExistencia(int MaterialId, int? BodegaId)
        {

            if (BodegaId != null && db.ExistenciasBodega.Any(x => x.BodegaId == BodegaId && x.MaterialId == MaterialId))
            {
                return db.ExistenciasBodega
                .Where(x => x.BodegaId == BodegaId && x.MaterialId == MaterialId)
                .GroupBy(x => new { x.BodegaId, x.MaterialId })
                .FirstOrDefault()
                .Sum(x => x.Cantidad);
            }
            else
            {
                return (decimal)0.00;
            }

        }
    }
}