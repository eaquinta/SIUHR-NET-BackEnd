using Apphr.Application.ControlAbastecimiento.DTOs;
using Apphr.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Apphr.WebUI.Models.Repository
{
    public class ControlAbastecimientoRepository : GenericRepository<ControlAbastecimiento>
    {
        public ControlAbastecimientoRepository(ApphrDbContext context) : base(context)
        {
        }
        public bool AddMaterial( int MaterialId) //int DestinoId,
        {
            try
            {
                if (!db.ControlAbastecimiento.Any(x => x.MaterialId == MaterialId)) // && x.DestinoId == DestinoId))
                {
                    db.ControlAbastecimiento.Add(new ControlAbastecimiento()
                    {
                        //DestinoId = DestinoId,
                        MaterialId = MaterialId
                    });

                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }            
        }        

        public List<ControlAbastecimientoDTOListaEgresos> GetEgresos(int MaterialId, string OrdenCompraId)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@MaterialId", MaterialId));
            parameters.Add(new SqlParameter("@OrdenCompraId", OrdenCompraId));
            // Q007
            string query = @"
                SELECT
	                ma.MaterialId, 
	                sms.HojaControlSala, 
	                ma.MovimientoAbastecimientoId, 
	                ma.SolicitudMaterialSalaId, 
	                ma.Cantidad, 
	                ma.Fecha
                FROM
	                dbo.MovimientosAbastecimiento AS ma
	            LEFT JOIN
	                dbo.SolicitudMaterialesSala AS sms 
                    ON
                      ma.SolicitudMaterialSalaId = sms.SolicitudMaterialSalaId
                WHERE
	                ma.OrdenCompraId = @OrdenCompraId AND
	                ma.MaterialId = @MaterialId AND
	                ma.TipoMovimiento = 'EGR'
            ";
            return db.Database.SqlQuery<ControlAbastecimientoDTOListaEgresos>(query, parameters.ToArray()).ToList();
        }

        public List<ControlAbastecimientoDTORptExistencias> GetExistencias()
        {
            //Q008
            string query = @"
                SELECT
	                ca.MaterialId, 
                    Materiales.Codigo,
	                ISNULL(ea.Cantidad, 0) AS Cantidad, 
	                Materiales.Descripcion
                FROM
	                dbo.ControlAbastecimiento AS ca
	            LEFT JOIN
	                (
		                SELECT
			                ea.MaterialId, 
			                ISNULL(SUM ( ea.Cantidad * ea.Efecto	), 0) AS Cantidad
		                FROM
			                dbo.MovimientosAbastecimiento AS ea
		                GROUP BY
			                ea.MaterialId
	                ) AS ea
	                ON 
		                ca.MaterialId = ea.MaterialId
	            LEFT JOIN
	                dbo.Materiales ON ca.MaterialId = Materiales.MaterialId
            ";
            return db.Database.SqlQuery<ControlAbastecimientoDTORptExistencias>(query).ToList();
        }

        public List<SelectListItem> AnioList()
        {
            List<SelectListItem> result = new List<SelectListItem>
            {
                new SelectListItem { Text = "2024", Value = "2024" },
                new SelectListItem { Text = "2023", Value = "2023" },
                new SelectListItem { Text = "2022", Value = "2022" },
                new SelectListItem { Text = "2021", Value = "2021" },
                new SelectListItem { Text = "2020", Value = "2020" },
                new SelectListItem { Text = "2019", Value = "2019" },
                new SelectListItem { Text = "2018", Value = "2018" }
            };
            return result;
        }
    }
}