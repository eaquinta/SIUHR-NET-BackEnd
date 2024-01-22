using Apphr.Application.ControlAbastecimiento.DTOs;
using Apphr.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Apphr.WebUI.Models.Repository
{
    public class EgresoAbastecimientoRepositoy : GenericRepository<EgresoAbastecimiento>
    {
        public EgresoAbastecimientoRepositoy(ApphrDbContext context) : base(context)
        {
        }

        //public void AddEgresos(
        //    string SolicitudMaterialSalaId,
        //    int SolicitudMaterialSalaDetalleId,
        //    DateTime Fecha,
        //    int MaterialId,
        //    List<ControlAbastecimientoDTOEgresoDist> Distribucion)         
        //{
        //    foreach (var item in Distribucion)
        //    {
        //        var RegAbastecimiento = new EgresoAbastecimiento()
        //        {
        //            SolicitudMaterialSalaId = SolicitudMaterialSalaId,
        //            SolicitudMaterialSalaDetalleId = SolicitudMaterialSalaDetalleId,
        //            Fecha = Fecha,
        //            MaterialId = MaterialId,
        //            Cantidad = item.Cantidad,
        //            OrdenCompraId = item.OrdenCompraId
        //        };
        //        db.EgresosAbastecimiento.Add(RegAbastecimiento);
        //    }            
        //}
        // Mover a MovimientosAbastecimientoRepository
        //public async Task DeleteEgresos(int SolicitudMaterialSalaDetalleId) 
        //{
        //    var regs = await db.MovimientosAbastecimiento
        //        .Where(x => x.SolicitudMaterialSalaDetalleId == SolicitudMaterialSalaDetalleId )
        //        .ToListAsync();

        //    foreach (var item in regs)
        //    {
        //        db.MovimientosAbastecimiento.Remove(item);
        //    }

        //}
    }
}