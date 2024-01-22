using Apphr.Application.DespachosInventario.DTOs;
using Apphr.WebUI.Models.Entities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Apphr.WebUI.Models.Repository
{
    public class DespachoInventarioRepository : GenericRepository<DespachoInventario>
    {
        public DespachoInventarioRepository(ApphrDbContext context) : base(context)
        {
        }

		public async Task<DespachoInventario> InsertMaster(DespachoInventarioDTOCEdit dto, string IdAutoNum)
		{
			using (var dbTrn = db.Database.BeginTransaction())
			{
                try
                {
					// Numeracion automatica
					dto.DespachoInventarioId = db.GetYearlyAutonumeric(IdAutoNum, DateTime.Now.Year);

					if (dto.TipoDocumentoReferencia == "SD")
					{
						var rel = await db.SolicitudesDespacho.FindAsync(dto.DocumentoReferencia);
						rel.DespachoInventarioId = dto.DespachoInventarioId;
						rel.Protegido = true;
					}
					else
					{
						var rel = await db.SolicitudMaterialesSala.FindAsync(dto.DocumentoReferencia);
						rel.DespachoInventarioId = dto.DespachoInventarioId;
						rel.Protegido = true;
					}

					var reg = new DespachoInventario()
					{
						DespachoInventarioId = dto.DespachoInventarioId,
						Fecha = dto.Fecha,
						DepartamentoId = dto.DepartamentoId,
						FechaDocumentoReferencia = dto.FechaDocumentoReferencia,
						TipoDocumentoReferencia = dto.TipoDocumentoReferencia,
						DocumentoReferencia = dto.DocumentoReferencia,
						Lineas = 0
					};


					db.DespachosInventario.Add(reg);
					await db.SaveChangesAsync();
					dbTrn.Commit();
					return reg;
				}
                catch (Exception)
                {
					dbTrn.Rollback();
					throw;
				}
			}
		}
		public async Task<DespachoInventario> UpdateMaster(DespachoInventarioDTOCEdit dto)
        {
			using (var dbTrn = db.Database.BeginTransaction())
			{
				try
				{
					var reg = await db.DespachosInventario
											.Where(x => x.DespachoInventarioId == dto.DespachoInventarioId)
											.Include("Detalle")
											.Include("Detalle.MovimientoInventario")
											.FirstOrDefaultAsync();
					reg.DepartamentoId = dto.DepartamentoId;
					reg.Fecha = dto.Fecha;
					reg.FechaDocumentoReferencia = dto.FechaDocumentoReferencia;
					reg.DocumentoReferencia = dto.DocumentoReferencia;
					// L2
					foreach (var item in reg.Detalle)
					{
						item.MovimientoInventario.Fecha = dto.Fecha;
					}
					await db.SaveChangesAsync();
					
					dbTrn.Commit();
					return reg;
				}
				catch (Exception)
				{
					dbTrn.Rollback();
					throw;
				}
			}
		}
        public async Task DeleteMaster(string id)
        {
			using (DbContextTransaction transaction = db.Database.BeginTransaction())
			{
				try
				{
					var reg = await db.DespachosInventario.Where(x => x.DespachoInventarioId == id).FirstOrDefaultAsync();
					var det = await db.DespachosInventarioDetalle.Where(x => x.DespachoInventarioId == reg.DespachoInventarioId).ToListAsync();
					if (reg.TipoDocumentoReferencia == "SD")
					{
						var rel = await db.SolicitudesDespacho.FindAsync(reg.DocumentoReferencia);
						if (rel != null)
							rel.DespachoInventarioId = null;
					}
					else
					{
						var rel = await db.SolicitudMaterialesSala.FindAsync(reg.DocumentoReferencia);
						if (rel != null)
							rel.DespachoInventarioId = null;
					}
					foreach (var item in det)
					{
						var mov = await db.MovimientosInvnentario.FindAsync(item.MovimientoInventarioId);
						db.MovimientosInvnentario.Remove(mov);
						db.DespachosInventarioDetalle.Remove(item);
					}
					db.DespachosInventario.Remove(reg);
					await db.SaveChangesAsync();
					transaction.Commit();
				}
				catch (Exception)
				{
					transaction.Rollback();
					throw;
				}

			}
		}
    }
}