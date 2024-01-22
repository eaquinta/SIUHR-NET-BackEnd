using Apphr.WebUI.Models.Entities;
using System;
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

        public bool ImportIfNotExist(string CODIGO)
        {
            try
            {
                if (string.IsNullOrEmpty(CODIGO))
                {
                    throw new ArgumentException("Parametro CODIGO de contener algun valor");
                }
                var MaterialDBF = dbfContext.GetMaterial(CODIGO).FirstOrDefault();
                if (!db.Materiales.Any(x => x.Codigo == MaterialDBF.CODIGO))
                {
                    var reg = mapper.Map<Material>(MaterialDBF);
                    db.Materiales.Add(reg);
                }
                else
                {
                    var reg = db.Materiales.Where(x => x.Codigo == MaterialDBF.CODIGO).FirstOrDefault();
                    if (reg != null)
                    {
                        mapper.Map(MaterialDBF, reg);
                    }
                }
                db.SaveChanges();                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int? GetIdFromCodigo(string Codigo)
        {
            return db.Materiales.Where(x => x.Codigo == Codigo).FirstOrDefault()?.MaterialId;
        }
    }
}