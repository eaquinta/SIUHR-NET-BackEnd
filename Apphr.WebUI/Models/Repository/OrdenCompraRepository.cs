using Apphr.Application.ControlAbastecimiento.DTOs;
using Apphr.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Apphr.WebUI.Models.Repository
{
    public class OrdenCompraRepository : GenericRepository<OrdenCompra>
    {
        public OrdenCompraRepository(ApphrDbContext context) : base(context)
        {
        }

        public string OrdenCompraIdFromDBF(int year, string Codigo)
        {
            if (string.IsNullOrEmpty(Codigo) || year == 0)
            {
                return null;
            }
            return year.ToString() + "-" + Codigo.Trim().PadLeft(6, '0');
        }

        public List<ControlAbastecimientoDTOEgresoDist> GetOrdenCompraIdPEPS(int MaterialId, int ProveedorId, decimal CantidadEgreso, int? SolicitudMaterialSalaDetalleId = null)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@MaterialId", MaterialId));
            parameters.Add(new SqlParameter("@ProveedorId", ProveedorId));
            parameters.Add(new SqlParameter("@SolicitudMaterialSalaDetalleId", (Int32)(SolicitudMaterialSalaDetalleId??0)));            

            var res = new List<ControlAbastecimientoDTOEgresoDist>();
            string query = "AbastecimientoExistencias @MaterialId, @ProveedorId, @SolicitudMaterialSalaDetalleId";
            var regs = db.Database.SqlQuery<ControlAbastecimientoDTOExistencias>(query, parameters.ToArray()).ToList();
            foreach (var item in regs)
            {
                if (item.Existencia >= CantidadEgreso)
                {                    
                    res.Add(new ControlAbastecimientoDTOEgresoDist() {
                         Cantidad = CantidadEgreso,
                         OrdenCompraId = item.OrdenCompraId
                    });
                    CantidadEgreso = 0;
                    break;
                }
                if(item.Existencia > 0)
                {                    
                    res.Add(new ControlAbastecimientoDTOEgresoDist()
                    {
                        Cantidad = item.Existencia,
                        OrdenCompraId = item.OrdenCompraId
                    });
                    CantidadEgreso -= item.Existencia;
                }
                
            }
            if(CantidadEgreso > 0)
            {
                throw new ArgumentException("Existencias insufientes para operar este egreso");
            }
            return res;

            //Return 
        }
    }
}