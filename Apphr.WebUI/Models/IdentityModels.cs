﻿using Apphr.Domain.Common;
using Apphr.Infrastructure.Persistence.Configuration;
using Apphr.WebUI.Models.Entities;
using Apphr.WebUI.Models.Entities.Ortopedia;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace Apphr.WebUI.Models
{
    //Agregado 2022-06-17
    public class AppUserStore 
        : UserStore<AppUser, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim>, 
        IUserStore<AppUser, int>,
        IDisposable
    {
        public AppUserStore() : this(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public AppUserStore(DbContext context) : base(context)
        {
        }
    }

    //Agregado 2022-06-17
    public class AppRoleStore 
        : RoleStore<AppRole, int, AppUserRole>, 
        IQueryableRoleStore<AppRole, int>, 
        IRoleStore<AppRole, int>,
        IDisposable
    {
        public AppRoleStore() : base(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public AppRoleStore(DbContext context) : base(context)
        {
        }
    }


    public class ApphrDbContext : IdentityDbContext<AppUser, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim>
    {
        public int UserId { get; set; } = -1;
        public string UserName { get; private set; } = "Anonymus";

        public ApphrDbContext() : base("DefaultConnection")
        {
            //UserId = userId ?? -1;            
            //UserId = -1;
            //if (HttpContext.Current.User != null)
            //    UserId = HttpContext.Current.User.Identity.GetUserId<int>();
            UserName = "Anonymus";
        }
        public ApphrDbContext(int userId) : base("DefaultConnection")
        {
        //    //    UserId = HttpContext.Current.User.Identity.GetUserId<int>();
            UserId = userId;
            ////    UserName = !string.IsNullOrEmpty(userName)
            ////    ? userName
            ////    : "Anonymous";
            ////    // this.getUserName();
           // UserName = this.getUserName();
        }

        // Aplication Entites
        
        
        public DbSet<Persona> Personas { get; set; }
        public DbSet<AppGroup> AppGroups { get; set; }
        public DbSet<AppPermission> AppPermissions { get; set; }
        public DbSet<AppRolePermission> AppRolePermissions { get; set; }
        public DbSet<AppRoleGroup> AppRoleGroups { get; set; }
        public DbSet<AppUserGroup> AppUserGroups { get; set; }
        public DbSet<AppUserRole> AppUserRoles { get; set; }
        public DbSet<AppEntityLog> AppLogs { get; set; }
        public DbSet<AppEntityChangeLog> AppChangeLogs { get; set; }
        public DbSet<AppAuditLog> AppAuditLogs { get; set; }
        public DbSet<AppYearlyCounter> AppYearlyCounters { get; set; }
        public DbSet<RegistroMedico> RegistrosMedicos { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Encargado> Encargados { get; set; }
        public DbSet<AccessRule> AccessRules { get; set; }
        public DbSet<AccessRulePermitAssignment> AccessRulePermitAssignments { get; set; }
        public DbSet<AccessRuleRoleAssignment> AccessRuleRoleAssignments { get; set; }
        //public DbSet<Facilitador> Facilitadores { get; set; }
        public DbSet<PacienteEvento> PacienteEventos { get; set; }
        public DbSet<PacienteEventoTraslado> PacienteEventoTraslados { get; set; }
        public DbSet<PacienteEventoHistorial> PacienteEventoHistoriales { get; set; }
        public DbSet<Controlador> Controladores { get; set; }
        public DbSet<ControladorRolAsignacion> ControladorRolAsignaciones { get; set; }
        //public DbSet<Empleado> Empleados { get; set; }
        public DbSet<ControladorPermiso> ControladorPermisos { get; set; }
        public DbSet<Bodega> Bodegas { get; set; }
        public DbSet<Destino> Destinos { get; set; }
        public DbSet<Material> Materiales { get; set; }
        public DbSet<SolicitudPedido> SolicitudesPedido { get; set; }
        public DbSet<SolicitudPedidoDetalle> SolicitudesPedidoDetalle { get; set; }
        public DbSet<DespachoInventario> DespachosInventario { get; set; }
        public DbSet<DespachoInventarioDetalle> DespachosInventarioDetalle { get; set; }
        public DbSet<IngresoInventario> IngresosInventario { get; set; }
        public DbSet<IngresoInventarioDetalle> IngresosInventarioDetalle { get; set; }
        public DbSet<MovimientoInventario> MovimientosInvnentario { get; set; }
        public DbSet<SolicitudDespacho> SolicitudesDespacho { get; set; }
        public DbSet<SolicitudDespachoDetalle> SolicitudesDespachoDetalle { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<AppDefault> Defaults { get; set; }
        public DbSet<ExistenciaBodega> ExistenciasBodega { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<AjusteInventario> AjustesInventario { get; set; }
        public DbSet<AjusteInventarioDetalle> AjustesInventarioDetalle { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<CierreInventario> CierresInventario { get; set; }
        public DbSet<TipoMovimientoInventario> TiposMovimientoInventario { get; set; }
        public DbSet<SolicitudMaterialSala> SolicitudMaterialesSala { get; set; }
        public DbSet<SolicitudMaterialSalaDetalle> SolicitudMaterialesSalaDetalle { get; set; }
        public DbSet<OrdenCompra> OrdenesCompra { get; set; }
        public DbSet<OrdenCompraDetalle> OrdenesCompraDetalle { get; set; }
        public DbSet<ControlAbastecimiento> ControlAbastecimiento { get; set; }
        public DbSet<IngresoAbastecimiento> IngresosAbastecimiento { get; set; }
        public DbSet<EgresoAbastecimiento> EgresosAbastecimiento { get; set; }
        public DbSet<MovimientoAbastecimiento> MovimientosAbastecimiento { get; set; }
        public DbSet<InicialAbastecimiento> InicialAbastecimiento { get; set; }

        // Siahad
        public DbSet<SHDAdministracion> SHDAdministracion { get; set; }
        public DbSet<SHDContador> SHDContador { get; set; }
        public DbSet<SHDBodega> SHDBodega { get; set; }
        public DbSet<SHDExistenciaBodega> SHDExistenciaBodega { get; set; }
        public DbSet<SHDExistenciaLote> SHDExistenciaLote { get; set; }
        public DbSet<SHDExistenciaTotal> SHDExistenciaTotal { get; set; }
        public DbSet<SHRMaterial> SHRMaterial { get; set; }
        public DbSet<SHDMovimientoInventario> SHDMovimientosInventario { get; set; }
        public DbSet<SHRDestino> SHRDestino { get; set; }

        // ORTOPEDIA
        public DbSet<ORTMovimiento> ORTMovimientos { get; set; }
        public DbSet<ORTCirujano> ORTCirujanos { get; set; }
        public DbSet<ORTHojaGasto> ORTHojasGasto { get; set; }
        public DbSet<ORTPaciente> ORTPacientes { get; set; }
        public DbSet<ORTOrdenCompra> ORTOrdenesCompra { get; set; }
        public DbSet<ORTSolicitudPedido> ORTSolicitudesPedido { get; set; }
        public DbSet<ORTAjusteInventario> ORTAjustesInventario { get; set; }
        public DbSet<ORTIngresoInventario> ORTIngresosInventario { get; set; }
        public DbSet<ORTDespachoInventario> ORTDespachosInventario { get; set; }
        public DbSet<ORTDevolucionInventario> ORTDevoluciones { get; set; }


        public static ApphrDbContext Create()
        {
           return new ApphrDbContext();           
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This needs to go before the other rules!

            modelBuilder.Entity<AppUser>().ToTable("AppUser");
            modelBuilder.Entity<AppRole>().ToTable("AppRole");
            modelBuilder.Entity<AppUserRole>().ToTable("AppUserRole");
            modelBuilder.Entity<AppUserClaim>().ToTable("AppUserClaim");
            modelBuilder.Entity<AppUserLogin>().ToTable("AppUserLogin");            
            modelBuilder.Entity<SolicitudPedidoDetalle>().Property(x => x.Precio).HasPrecision(18, 4);
            modelBuilder.Entity<Paciente>().Property(x => x.RefPac_NumHC).HasPrecision(18, 0);
            modelBuilder.Entity<Paciente>().Property(x => x.RefPac_NumHCAntiguo).HasPrecision(18, 0);             
            modelBuilder.Configurations.Add(new PersonaMap());
            modelBuilder.Configurations.Add(new ProveedorEntityConfiguration());


            //modelBuilder.Entity<AppRole>()
            //.HasMany(r => r.Permissions)
            //.WithMany(p => p.AppRoles)
            //.Map(m =>
            //{
            //    m.ToTable("AppRolePermissions");
            //    m.MapLeftKey("AppRoleId");
            //    m.MapRightKey("AppPermissionId");
            //});
        }

        public override int SaveChanges()
        {
            DateTime currentDateTime = DateTime.Now;
            AddTimestamps(currentDateTime, this.UserName);            

            foreach (var modifiedEntry in this.GetUPDEntry()) // Ciclo de Entradas
            {
                ApplyAuditLog(modifiedEntry, currentDateTime, this.UserName, "UPD");
            }

            int changes = base.SaveChanges();

            foreach (var entry in this.GetINSEntry())
            {
                ApplyAuditLog(entry, currentDateTime, this.UserName, "INS");
            }

            base.SaveChanges();

            return changes;
        }

        public override async Task<int> SaveChangesAsync()
        {
            DateTime currentDateTime = DateTime.Now;
            
            AddTimestamps(currentDateTime, this.UserName);

            foreach (var modifiedEntry in this.GetUPDEntry()) // Ciclo de Entradas
            {
                ApplyAuditLog(modifiedEntry, currentDateTime, this.UserName, "UPD");
            }

            int changes = await base.SaveChangesAsync();

            foreach (var entry in this.GetINSEntry())
            {
                ApplyAuditLog(entry, currentDateTime, this.UserName, "INS");
            }

            await base.SaveChangesAsync();

            return changes;
        }

        private List<DbEntityEntry> GetUPDEntry()
        {
            return ChangeTracker.Entries().Where(p => p.State == EntityState.Modified && p.Entity.GetType().Name != "AppAuditLog").ToList();
        }
        private List<DbEntityEntry> GetINSEntry()
        {
            return ChangeTracker.Entries().Where(e => e.State == EntityState.Added && e.Entity.GetType().Name != "AppAuditLog").ToList();
        }
        private List<DbEntityEntry> GetDLTEntry()
        {
            return ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).ToList();
        }
                

        private void ApplyAuditLog(DbEntityEntry entry, DateTime currentDateTime, string currentUsername, string eventType)
        {
            int cnt = 0;
            string tableName = entry.Entity.GetType().Name;
            Guid entityLogId = Guid.NewGuid();

            foreach (var prop in entry.CurrentValues.PropertyNames)
            {
                var logIgnore = Attribute.IsDefined(entry.Entity.GetType().GetProperty(prop), typeof(LogIgnoreAttribute));
                if (!logIgnore)
                {
                    //string currentValue = entry.CurrentValues[prop] == null ? null : entry.CurrentValues[prop].ToString();
                    string currentValue = entry.CurrentValues[prop]?.ToString();
                    string originalValue = null;

                    if (entry.State == EntityState.Modified)
                    {
                        //originalValue = entry.OriginalValues[prop] == null ? null : entry.OriginalValues[prop].ToString();
                        originalValue = entry.OriginalValues[prop]?.ToString();
                    }

                    if (eventType == "INS" || (eventType == "UPD" && currentValue != originalValue))
                    {
                        cnt++;
                        AppEntityChangeLog logDet = new AppEntityChangeLog()
                        {
                            EntityLogId = entityLogId,
                            EntityChangeLogId = Guid.NewGuid(),
                            //EventType = eventType,
                            //TableName = tableName,
                            //RecordID = getPrimaryKeyValue(entry),
                            ColumnName = prop,
                            OriginalValue = originalValue,
                            NewValue = currentValue,
                            //Created_by = currentUsername,
                            //Created_date = currentDateTime
                        };                        
                        AppChangeLogs.Add(logDet);
                    }
                }
            }
            if (cnt > 0)
            {
                AppEntityLog log = new AppEntityLog()
                {
                    EntityLogId = entityLogId,
                    //Created_by = currentUsername,
                    CreatedByUser = this.UserId,
                    EventType = eventType,
                    Created_date = currentDateTime,
                    RecordID = GetPrimaryKeyValue(entry),
                    TableName = tableName
                };
                AppLogs.Add(log);
            }
        }
        
        private void AddTimestamps(DateTime currentDateTime, string currentUsername)
        {
            foreach (var auditableEntity in ChangeTracker.Entries<AuditableEntity>())
            {
                if (auditableEntity.State == EntityState.Added || auditableEntity.State == EntityState.Modified)
                {                   
                    switch (auditableEntity.State)
                    {
                        case EntityState.Added:
                            auditableEntity.Entity.CreatedDate = currentDateTime;
                            auditableEntity.Entity.CreatedByUser = this.UserId;
                            break;
                        case EntityState.Modified:
                            auditableEntity.Entity.LastModifiedDate = currentDateTime;
                            auditableEntity.Entity.LastModifiedByUser = this.UserId;
                            if (auditableEntity.Property(p => p.CreatedDate).IsModified || auditableEntity.Property(p => p.CreatedByUser).IsModified)
                            {
                                throw new DbEntityValidationException(string.Format("Se esta intentando actualizar la fecha de creación en {0}", auditableEntity.Entity.GetType().FullName));
                            }
                            break;
                    }
                }
            }
        }

        
        private string GetPrimaryKeyValue(DbEntityEntry entry)
        {
            var objectStateEntry = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            //return JsonConvert.SerializeObject(objectStateEntry.EntityKey.EntityKeyValues);
            //return objectStateEntry.EntityKey.EntityKeyValues[0].ToString();
            return objectStateEntry.EntityKey.EntityKeyValues[0].Value.ToString();
        }

        public string GetYearlyAutonumeric(string name, int year)
        {
            int val = 0;
            //using (DbContextTransaction transaction = Database.BeginTransaction())
            //{
                try
                {
                    AppYearlyCounter yc = AppYearlyCounters.Find(name, year);
                    if (yc == null)
                    {
                        val = 1;
                        AppYearlyCounters.Add(new AppYearlyCounter() { Name = name, Year = year, LastNumber = val });
                    }
                    else
                    {
                        val = yc.LastNumber + 1;
                        yc.LastNumber = val;
                    }
                    //SaveChanges();
                    //transaction.Commit();
                }
                catch (Exception )
                {
              //      transaction.Rollback();
                    //Console.WriteLine("Error occurred. "+ex.Message);
                    throw;
                }
            //}
            return year.ToString() +"-"+ val.ToString("D6"); // "2022-000001";
        }
    }
}
