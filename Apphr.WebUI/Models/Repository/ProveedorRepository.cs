using Apphr.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apphr.WebUI.Models.Repository
{
    public class ProveedorRepository : GenericRepository<Proveedor>
    {
        public ProveedorRepository(ApphrDbContext context) : base(context)
        {
        }

        public int? GetIdFromNit(string Nit)
        {
            return db.Proveedores.Where(x => x.Nit == Nit).FirstOrDefault()?.ProveedorId;
        }

        public bool ImportIfNotExist(string Nit)
        {
            try
            {
                if (string.IsNullOrEmpty(Nit))
                {
                    throw new ArgumentException("Parametro CODIGO de contener algun valor");
                }
                var ProveedorDBF = dbfContext.GetProveedor(Nit).FirstOrDefault();
                if (!db.Proveedores.Any(x => x.Nit == ProveedorDBF.NIT))
                {
                    var reg = mapper.Map<Proveedor>(ProveedorDBF);
                    db.Proveedores.Add(reg);
                }
                else
                {
                    var reg = db.Proveedores.Where(x => x.Nit == ProveedorDBF.NIT).FirstOrDefault();
                    if (reg != null)
                    {
                        mapper.Map(ProveedorDBF, reg);
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

    }
}