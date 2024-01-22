using Apphr.Application.Common.DTOs;
using Apphr.Application.Ortopedia.Common;
using Apphr.Application.Ortopedia.Devolucion.DTOs;
using Apphr.Application.Ortopedia.OrdenesCompra.DTOs;
using Apphr.Application.Ortopedia.ORTMovimientos.DTOs;
using Apphr.WebUI.Models.Entities.Ortopedia;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Apphr.WebUI.Models.Repository
{
    public partial class OrtopediaRepository
    {
        protected ApphrDbContext db;
        public OrtopediaRepository(ApphrDbContext context)
        {
            this.db = context;
        }

        public IEnumerable<DTOOrdenCompraMaterialFull> GetOrdenCompraMaterialFull(Int64? OrdenCompraId, Int64? MaterialId)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@OrdenCompraId", (OrdenCompraId == null) ? (object)DBNull.Value : OrdenCompraId));
            parameters.Add(new SqlParameter("@MaterialId", (MaterialId == null) ? (object)DBNull.Value : MaterialId));
            var res = db.Database.SqlQuery<DTOOrdenCompraMaterialFull>("GetOrdenCompraMaterialFull @OrdenCompraId, @MaterialId", parameters.ToArray()).ToList();
            return res;
        }

        public IQueryable<ORTMovimiento> GetORTMovimientosWithIncludes()
        {
            return from m in db.ORTMovimientos
                .Include(i => i.Material)
                .Include(i => i.Proveedor)
                .Include(i => i.OrdenCompra)
                .Include(i => i.SolicitudPedido)
                .Include(i => i.Cirujano)
                .Include(i => i.Paciente)
                 select m;
        }

        public IQueryable<OrdenCompraDTOStatus> GetOrdenesCompraStatus(Int64? OrdenCompraId = null)
        {
            return (from row in db.ORTMovimientos.Where(x => x.OrdenCompraId == OrdenCompraId || OrdenCompraId == null)
                        //.Where(x => x.Tipo == "ORD" && x.MaterialId == MaterialId && x.ProveedorId == ProveedorId)
                    group row by new { row.OrdenCompraId, row.MaterialId } into g
                    select new OrdenCompraDTOStatus
                    {
                        OrdenCompraId = g.Key.OrdenCompraId ?? 0,
                        MaterialId = g.Key.MaterialId,
                        Solicitado = g.Sum(r => (r.Tipo == "SOL" ? r.Cantidad : 0)),
                        Ordenado = g.Sum(r => (r.Tipo == "ORD" ? r.Cantidad : 0)),
                        Ingresado = g.Sum(r => (r.Tipo == "ING" ? r.Cantidad : 0)),
                        Despachado = g.Sum(r => (r.Tipo == "DES" ? r.Cantidad : 0)),
                        //Pendiente = g.Sum(r => (r.Tipo == "ING" ? r.Cantidad : 0)) - g.Sum(r => (r.Tipo == "EGR" ? r.Cantidad : 0))
                    });
        }

        public IQueryable<MovimientoDTOExistenciaMaterial> GetExistenciaByMaterialId(int? MaterialId = null)
        {
            return from m in db.ORTMovimientos
                   group m by new { m.MaterialId } into g
                   select new MovimientoDTOExistenciaMaterial
                   {
                       MaterialId = g.Key.MaterialId,
                       Ingresos = g.Sum(r => (r.Tipo == "ING" ? r.Cantidad : 0)),
                       Despachos = g.Sum(r => (r.Tipo == "DES" ? r.Cantidad : 0)),
                       Ajustes = g.Sum(r => (r.Tipo == "AJU" ? r.Cantidad : 0)),
                       Cantidad = g.Sum(r => (r.Tipo == "ING" || r.Tipo == "AJU" ? r.Cantidad : 0)) - g.Sum(r => (r.Tipo == "DES" ? r.Cantidad : 0)),
                       Valor = g.Sum(r => (r.Tipo == "ING" || r.Tipo == "AJU" ? r.Valor : 0)) - g.Sum(r => (r.Tipo == "DES" ? r.Valor : 0))
                   };
        }


        public IQueryable<MovimientoDTOExistenciaProveedor> GetExistenciaByProveedor(int? ProveedorId = null)
        {
            return from m in db.ORTMovimientos
                   group m by new { m.ProveedorId } into g
                   select new MovimientoDTOExistenciaProveedor
                   {
                       ProveedorId = g.Key.ProveedorId ?? 0,
                       Ingresos = g.Sum(r => (r.Tipo == "ING" ? r.Cantidad : 0)),
                       Despachos = g.Sum(r => (r.Tipo == "DES" ? r.Cantidad : 0)),
                       Ajustes = g.Sum(r => (r.Tipo == "AJU" ? r.Cantidad : 0)),
                       Cantidad = g.Sum(r => (r.Tipo == "ING" || r.Tipo == "AJU" ? r.Cantidad : 0)) - g.Sum(r => (r.Tipo == "DES" ? r.Cantidad : 0)),
                       Valor = g.Sum(r => (r.Tipo == "ING" || r.Tipo == "AJU" ? r.Valor : 0)) - g.Sum(r => (r.Tipo == "DES" ? r.Valor : 0))
                   };
        }

        public async Task<List<DevolucionDTOReporte>> GetImpresionDevolucion(Int64 id)
        {
            var res = new List<DevolucionDTOReporte>();
            res = await db.ORTMovimientos
                .Include(i => i.Paciente)
                .Include(i => i.Proveedor)
                .Include(i => i.Material)
                .Include(i => i.HojaGasto)
                .Where(x => x.Tipo == "DEV" && x.DevolucionId == id)
                .Select(s => new DevolucionDTOReporte { 
                    Formulario= s.HojaGasto.HojaControlSala,
                    PacienteNombre = s.Paciente.Nombre,
                    PacienteRegistro = s.Paciente.RefPac_NumHCAntiguo.ToString(),
                    Cantidad = s.Cantidad,
                    ProveedorNombre = s.Proveedor.Descripcion,
                    MaterialCodigo = s.Material.Codigo,
                    MaterialNombre = s.Material.Descripcion
                })
                .ToListAsync();
            return res;
        }

        public IQueryable<DTOHojaGastoMaterial> GetMaterialesByFilterInHojaGasto(Int64 HojaGastoId)
        {            
            var HojaGasto = from hg in db.ORTMovimientos.Where(x => x.Tipo == "GAS" && x.HojaGastoId == HojaGastoId) select hg;

            var result = from h in HojaGasto 
                         join m in db.Materiales on h.MaterialId equals m.MaterialId 
                         select new DTOHojaGastoMaterial { 
                             MaterialId = m.MaterialId,
                             Codigo = m.Codigo, 
                             Descripcion = m.Descripcion,
                             UnidadMedida = m.UnidadMedida,
                             Precio = m.Precio,
                         };

            return result;           
        }

        public IQueryable<DTOHojaGastoMaterial> GetMaterialesByFilterInHojaGasto(Int64 HojaGastoId, int ProveedorId)
        {
            var HojaGasto = from hg in db.ORTMovimientos
                            .Where(x => x.Tipo == "GAS" && x.HojaGastoId == HojaGastoId && x.ProveedorId == ProveedorId) select hg;

            var result = from h in HojaGasto
                         join m in db.Materiales on h.MaterialId equals m.MaterialId
                         select new DTOHojaGastoMaterial
                         {
                             MaterialId = m.MaterialId,
                             Codigo = m.Codigo,
                             Descripcion = m.Descripcion,
                             UnidadMedida = m.UnidadMedida,
                             Precio = m.Precio,
                         };

            return result;
        }

        public DTOHojaGastoMaterial GetMaterialByIdInHojaGasto(Int64 HojaGastoId, int MaterialId)
        {
            var HojaGasto = from hg in db.ORTMovimientos.Where(x => x.Tipo == "GAS" && x.HojaGastoId == HojaGastoId && x.MaterialId == MaterialId) select hg;

            var result = from h in HojaGasto
                         join m in db.Materiales on h.MaterialId equals m.MaterialId
                         select new DTOHojaGastoMaterial
                         {
                             MaterialId = m.MaterialId,
                             Codigo = m.Codigo,
                             Descripcion = m.Descripcion,
                             UnidadMedida = m.UnidadMedida,
                             Precio = m.Precio,
                             ProveedorId = h.ProveedorId ?? 0
                         };
            return result.FirstOrDefault();
        }

        public bool AddHojaGastoToDespacho(Int64 HojaGastoId, int BodegaId, Int64 DespachoInventarioId)
        {
            bool res = false;
            try
            {
                // Begin Transaction 
                using (DbContextTransaction t = db.Database.BeginTransaction())
                {
                    try
                    {
                        var toInsert = db.ORTMovimientos.Where(x => x.HojaGastoId == HojaGastoId).ToList();
                        foreach (var i in toInsert)
                        {
                            db.ORTMovimientos.Add(new ORTMovimiento {  
                                Tipo = "DES",
                                Documento = DespachoInventarioId,
                                BodegaId = BodegaId,
                                MaterialId = i.MaterialId,
                                Fecha = i.Fecha,
                                PacienteId = i.PacienteId,
                                CirujanoId = i.CirujanoId,
                                ProveedorId = i.ProveedorId,
                                Cantidad = i.Cantidad,
                                Precio = i.Precio,
                                Valor = i.Valor,
                            });
                        }
                        db.SaveChanges();
                        t.Commit();
                        res = true;
                    }
                    catch (Exception)
                    {
                        t.Rollback();
                        throw;
                    }   
                }
            }
            catch (Exception)
            {

                throw;
            }
            
            // Recupera Hoja de Gasto
            // Crea Egerso de Inventario 
            return res;
        }

        public bool AddHojaGastoToDevolucion(Int64 HojaGastoId, Int64 DevolucionId, int ProveedorId, DateTime Fecha)
        {
            bool res = false;
            try
            {
                // Begin Transaction 
                using (DbContextTransaction t = db.Database.BeginTransaction())
                {
                    try
                    {
                        // Recupera Hoja de Gasto que esta marcada para devolucion y corresponde al proveedor
                        var regHG = db.ORTMovimientos
                            .Where(x => x.HojaGastoId == HojaGastoId && x.ProveedorId == ProveedorId)
                            .ToList();
                        foreach (var i in regHG)
                        {
                            db.ORTMovimientos.Add(new ORTMovimiento {
                                Tipo = "DEV",
                                DevolucionId = DevolucionId,
                                HojaGastoId = i.HojaGastoId,
                                //BodegaId = BodegaId,
                                MaterialId = i.MaterialId,
                                Fecha = Fecha,
                                PacienteId = i.PacienteId,
                                CirujanoId = i.CirujanoId,
                                ProveedorId = i.ProveedorId,
                                Cantidad = i.Cantidad,
                                Precio = i.Precio,
                                Valor = i.Valor,
                            });
                        }
                        db.SaveChanges();
                        t.Commit();
                        res = true;
                    }
                    catch (Exception)
                    {
                        t.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            
            // Agrega Detalle a Devolcion 
            return res;
        }

        public List<OrdenCompraDTODespachoDistribucion> GetOrdenCompraPEPS(int MaterialId, decimal Cantidad)
        {
            List<OrdenCompraDTODespachoDistribucion> res = null;

            var regs = GetOrdenCompraMaterialFull(null, MaterialId);

            return res;
        }
        //public async Task<AjusteInventarioDetalle> GetChildAsync(int? id)
        //{
        //    return await db.AjustesInventarioDetalle
        //           .Include("Material")
        //           .Include("Proveedor")
        //           .Include("Bodega")
        //           .Where(x => x.AjusteInventarioDetalleId == id)
        //           .FirstOrDefaultAsync();
        //}
    }
}