using Apphr.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Apphr.WebUI.Models.Repository
{
    public class SolicitudDespachoRepository : GenericRepository<SolicitudDespacho>
    {
        public SolicitudDespachoRepository(ApphrDbContext context) : base(context)
        {
        }
  
        public bool IsAvalible(string id)
        {
            return db.SolicitudesDespacho
                .Any(x => string.IsNullOrEmpty(x.DespachoInventarioId) && x.SolicitudDespachoId == id);
        }
        public decimal GetCantidadSolicitada(int? MaterialId, string Id)
        {
            return db.SolicitudesDespachoDetalle
                .Where(x => x.SolicitudDespachoId == Id && x.MaterialId == MaterialId)
                .FirstOrDefault()?
                .Cantidad ?? 0;
        }
        public List<Material> GetMaterialesSolicitados(string Id)
        {
            return db.SolicitudesDespachoDetalle
                .Where(x => x.SolicitudDespachoId == Id)
                .Include(x => x.Material)
                .Select(x => x.Material)
                .ToList();
        }
    }
}