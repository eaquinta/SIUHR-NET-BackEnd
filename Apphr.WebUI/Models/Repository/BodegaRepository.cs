using Apphr.WebUI.Models.Entities;
using System.Linq;

namespace Apphr.WebUI.Models.Repository
{
    public class BodegaRepository : GenericRepository<Bodega>
    {
        public BodegaRepository(ApphrDbContext context) : base(context)
        {
        }

        public decimal GetExistencia(int BodegaId, int MaterialId)
        {
            try
            {
                return db.ExistenciasBodega
                .Where(x => x.BodegaId == BodegaId && x.MaterialId == MaterialId)
                .GroupBy(x => new { x.BodegaId, x.MaterialId })
                .FirstOrDefault()
                .Sum(x => x.Cantidad);
            }
            catch (System.Exception)
            {
                return (decimal)0.00;
            }
            
        }
    }
}