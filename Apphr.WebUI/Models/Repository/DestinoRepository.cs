using Apphr.WebUI.Models.Entities;
using Apphr.Domain.EntitiesDBF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Apphr.WebUI.Models.Repository
{
    public class DestinoRepository : GenericRepository<Destino>
    {
        public DestinoRepository(ApphrDbContext context) : base(context)
        {
        }
        public int? GetIdFromCodigo(string Codigo)
        {
            return db.Destinos.Where(x => x.Codigo == Codigo).FirstOrDefault()?.DestinoId;
        }

        public bool ImportIfNotExist(string CODIGO)
        {
            try
            {
                if (string.IsNullOrEmpty(CODIGO))
                {
                    throw new ArgumentException("Parametro CODIGO de contener algun valor");
                }
                var DestinoDBF = dbfContext.GetDestino(CODIGO).FirstOrDefault();
                if (!db.Destinos.Any(x => x.Codigo == DestinoDBF.CODIGO))
                {
                    var reg = mapper.Map<Destino>(DestinoDBF);
                    db.Destinos.Add(reg);
                }
                else
                {
                    var reg = db.Destinos.Where(x => x.Codigo == DestinoDBF.CODIGO).FirstOrDefault();
                    if (reg != null)
                    {
                        mapper.Map(DestinoDBF, reg);
                    }
                }
                 db.SaveChanges();
                //return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                return true;
            }
            catch (Exception)
            {
                return false;
                //return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}