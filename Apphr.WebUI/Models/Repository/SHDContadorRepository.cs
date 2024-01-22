using Apphr.WebUI.Models.Entities;
using Apphr.Domain.Enums;

namespace Apphr.WebUI.Models.Repository
{
    public class SHDContadorRepository : GenericRepository<SHDContador>
    {
        public SHDContadorRepository(ApphrDbContext context) : base(context)
        {
        }

        public int GetNextNum(int AdministracionId, SHDTipoMovimiento Tipo)
        {
            int res = 0; 
            var Contador = db.SHDContador.Find(AdministracionId);
            if (Contador == null)
            {
                var reg = new SHDContador()
                {
                    AdministracionId = AdministracionId,
                    Ingreso = 0,
                    Despacho = 0,
                    Devolucion = 0,
                    Ajuste = 0,
                    Apertura = 0,
                    Proceso = 0,
                    Traslado = 0
                };                
                Contador = db.SHDContador.Add(reg);
            }
            
            switch (Tipo)
            {
                case SHDTipoMovimiento.Ingreso:
                    Contador.Ingreso += 1;
                    res = Contador.Ingreso;
                    break;
                case SHDTipoMovimiento.Despacho:
                    Contador.Despacho += 1;
                    res = Contador.Despacho;
                    break;
                case SHDTipoMovimiento.Devolucion:
                    Contador.Devolucion += 1;
                    res = Contador.Devolucion;
                    break;
                case SHDTipoMovimiento.Ajuste:
                    Contador.Ajuste += 1;
                    res = Contador.Ajuste;
                    break;
                case SHDTipoMovimiento.Apertura:
                    Contador.Apertura += 1;
                    res = Contador.Apertura;
                    break;
                case SHDTipoMovimiento.Proceso:
                    Contador.Proceso += 1;
                    res = Contador.Proceso;
                    break;
                case SHDTipoMovimiento.Traslado:
                    Contador.Traslado += 1;
                    res = Contador.Traslado;
                    break;
                default:
                    // code block
                    break;
            }
            db.SaveChanges();

            return res;
        }
    }
}