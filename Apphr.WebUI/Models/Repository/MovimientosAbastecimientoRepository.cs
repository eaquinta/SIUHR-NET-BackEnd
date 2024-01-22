using Apphr.Application.ControlAbastecimiento.DTOs;
using Apphr.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Apphr.WebUI.Models.Repository
{
    public class MovimientosAbastecimientoRepository : GenericRepository<MovimientoAbastecimiento>
    {
        public MovimientosAbastecimientoRepository(ApphrDbContext context) : base(context)
        {
        }
		public async Task SaveIngresoAsync(int MaterialId, int ProveedorId, string OrdenCompraId, decimal Cantidad, DateTime Fecha)
        {
			try  // INSERT
			{
				var reg = new MovimientoAbastecimiento()
				{
					TipoMovimiento = "ING",
					Efecto = 1,
					MaterialId = MaterialId,
					ProveedorId = ProveedorId,
					OrdenCompraId = OrdenCompraId,
					Cantidad = Cantidad,
					Fecha = Fecha
				};
				db.MovimientosAbastecimiento.Add(reg);
				await db.SaveChangesAsync();			
			}
			catch (Exception)
			{
				throw;
			}

			
		}

		public void UpdateIngreso(int MovimientoAbastecimientoId, decimal Cantidad, DateTime Fecha)
		{
			try // UPDATE
			{
				var reg = db.MovimientosAbastecimiento.Find(MovimientoAbastecimientoId);
				reg.Cantidad = Cantidad;
				reg.Fecha = Fecha;				
				db.SaveChanges();
			}
			catch (Exception)
			{
				throw;
			}


		}



		public async Task SaveEgresoAsync(int MaterialId, int ProveedorId, string OrdenCompraId, decimal Cantidad, DateTime Fecha)
		{
			try  // INSERT 
			{
				var reg = new MovimientoAbastecimiento()
				{
					TipoMovimiento = "EGR",
					Efecto = -1,
					MaterialId = MaterialId,
					ProveedorId = ProveedorId,
					OrdenCompraId = OrdenCompraId,
					Cantidad = Cantidad,
					Fecha = Fecha
				};
				db.MovimientosAbastecimiento.Add(reg);
				await db.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw;
			}


		}

		public void UpdateEgreso(int MovimientoAbastecimientoId, decimal Cantidad, DateTime Fecha)
		{

			try // UPDATE
			{
				var reg = db.MovimientosAbastecimiento.Find(MovimientoAbastecimientoId);				
				reg.Cantidad = Cantidad;
				reg.Fecha = Fecha;
				db.SaveChanges();
			}
			catch (Exception)
			{
				throw;
			}


		}




		public void AddEgresos(
           string SolicitudMaterialSalaId,
           int SolicitudMaterialSalaDetalleId,
           DateTime Fecha,
           int MaterialId,
		   int ProveedorId,
           List<ControlAbastecimientoDTOEgresoDist> Distribucion)
        {
            foreach (var item in Distribucion)
            {
                var RegAbastecimiento = new MovimientoAbastecimiento()
                {
                    TipoMovimiento = "EGR",
                    Efecto = -1,
                    SolicitudMaterialSalaId = SolicitudMaterialSalaId,
                    SolicitudMaterialSalaDetalleId = SolicitudMaterialSalaDetalleId,
                    Fecha = Fecha,
                    MaterialId = MaterialId,
					ProveedorId = ProveedorId,
                    Cantidad = item.Cantidad,
                    OrdenCompraId = item.OrdenCompraId
                };
                db.MovimientosAbastecimiento.Add(RegAbastecimiento);
            }
        }

        public async Task DeleteEgresos(int SolicitudMaterialSalaDetalleId)
        {
            var regs = await db.MovimientosAbastecimiento
                .Where(x => x.SolicitudMaterialSalaDetalleId == SolicitudMaterialSalaDetalleId)
                .ToListAsync();

            foreach (var item in regs)
            {
                db.MovimientosAbastecimiento.Remove(item);
            }

        }

    }
}