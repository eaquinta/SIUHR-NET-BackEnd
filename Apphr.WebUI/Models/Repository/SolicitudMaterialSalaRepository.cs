using Apphr.Application.SolicitudMaterialesSala.DTOs;
using Apphr.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Apphr.WebUI.Models.Repository
{
    public class SolicitudMaterialSalaRepository : GenericRepository<ControlMaterialSala>
    {
        public SolicitudMaterialSalaRepository(ApphrDbContext context) : base(context)
        {
        }

        public async Task<SolicitudMaterialSala> InsertMaster(SolicitudMaterialSalaDTOCEdit dto)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    dto.SolicitudMaterialSalaId = db.GetYearlyAutonumeric("SolicitudMaterialSala", DateTime.Now.Year);
                    var reg = new SolicitudMaterialSala()
                    {
                        SolicitudMaterialSalaId = dto.SolicitudMaterialSalaId,
                        HojaControlSala = dto.HojaControlSala,
                        Fecha = dto.Fecha,
                        FechaOperacion = dto.FechaOperacion,
                        PacienteId = dto.PacienteId,
                        Servicio = dto.Servicio,
                        Cama = dto.Cama,
                        Cirujano = dto.Cirujano,
                        AuxiliarEnfermeriaCirculante = dto.AuxiliarEnfermeriaCirculante,
                        AuxiliarEnfermeriaInstrumentista = dto.AuxiliarEnfermeriaInstrumentista,
                        Observacion = dto.Observacion,
                        Lineas = 0
                    };


                    db.SolicitudMaterialesSala.Add(reg);
                    await db.SaveChangesAsync();
                    transaction.Commit();
                    return reg;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public async Task<SolicitudMaterialSala> UpdateMaster(SolicitudMaterialSalaDTOCEdit dto)
        {
            using (DbContextTransaction transaction =db.Database.BeginTransaction())
            {
                try
                {
                    var reg = await db.SolicitudMaterialesSala
                        .Where(x => x.SolicitudMaterialSalaId == dto.SolicitudMaterialSalaId)
                        .FirstOrDefaultAsync();

                    //Asginaciones
                    reg.HojaControlSala = dto.HojaControlSala;
                    reg.Fecha = dto.Fecha;
                    reg.FechaOperacion = dto.FechaOperacion;
                    reg.PacienteId = dto.PacienteId;
                    reg.Servicio = dto.Servicio;
                    reg.Cama = dto.Cama;
                    reg.Cirujano = dto.Cirujano;
                    reg.AuxiliarEnfermeriaCirculante = dto.AuxiliarEnfermeriaCirculante;
                    reg.AuxiliarEnfermeriaInstrumentista = dto.AuxiliarEnfermeriaInstrumentista;
                    reg.Observacion = dto.Observacion;

                    await db.SaveChangesAsync();
                    transaction.Commit();
                    return reg;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public async Task<List<SolicitudMaterialSalaDetalle>> GetChildGridAsync(string id)
        {
            return await db.SolicitudMaterialesSalaDetalle
                .Include("Material")
                .Include("Proveedor")
                .Where(x => x.SolicitudMaterialSalaId == id)
                .ToListAsync();
        }
        public async Task<SolicitudMaterialSala> GetDetailsAsync(string id)
        {
            return await db.SolicitudMaterialesSala
                .Where(x => x.SolicitudMaterialSalaId == id)
                .Include(x => x.Paciente)
                .Include(x => x.Detalle)
                .Include("Detalle.Material")
                .Include("Paciente.Persona")
                .FirstOrDefaultAsync();
        }
        public async Task<List<SolicitudMaterialSala>> GetSolicitudesDisponiblesAsync()
        {
            return await db.SolicitudMaterialesSala
                .Where(x => x.DespachoInventarioId == null)
                .ToListAsync();
        }
        public List<Material> GetMaterialesSolicitados(string id)
        {
            return db.SolicitudMaterialesSalaDetalle
                .Include(x => x.Material)
                .Where(x => x.SolicitudMaterialSalaId == id)
                .Select(x => x.Material)
                .ToList();
        }
        public decimal GetCantidadSolicitada(int? MaterialId, string Id)
        {
            return db.SolicitudMaterialesSalaDetalle
                .Where(x => x.SolicitudMaterialSalaId == Id && x.MaterialId == MaterialId)
                .FirstOrDefault()?
                .Cantidad ?? 0;
        }
        public bool IsAvalible(string id)
        {
            return db.SolicitudMaterialesSala
                .Any(x => string.IsNullOrEmpty(x.DespachoInventarioId) && x.SolicitudMaterialSalaId == id);
        }
        public string GetHojaControlSalaById(string id)
        {
            return db.SolicitudMaterialesSala
                .Find(id)?
                .HojaControlSala;
        }
        public async Task<Paciente> GetPacienteInfo(string id)
        {
            var reg = await db.SolicitudMaterialesSala
                .Include(x => x.Paciente)
                .Include("Paciente.Persona")
                .Where(x => x.SolicitudMaterialSalaId == id)
                .FirstOrDefaultAsync();
            return reg?.Paciente;
        }
    }
}