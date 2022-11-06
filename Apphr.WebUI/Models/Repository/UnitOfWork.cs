using Apphr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Apphr.WebUI.Models.Repository
{
    public class UnitOfWork: IDisposable
    {
        public UnitOfWork()
        {
            this.context = new ApphrDbContext(System.Web.HttpContext.Current.User.Identity.Name); //ApphrDbContext();
        }
        private readonly ApphrDbContext context;

        private GenericRepository<AppUser> userRepository;
        private SolicitudMaterialSalaRepository controlMaterialSalaRepository;
        private GenericRepository<ControlMaterialSalaDetalle> controlMaterialSalaDetalleRepository;

        public GenericRepository<AppUser> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new GenericRepository<AppUser>(this.context);
                }
                return this.userRepository;
            }
        }
        public SolicitudMaterialSalaRepository ControlMaterialSalaRepository
        {
            get
            {
                if (this.controlMaterialSalaRepository == null)
                {
                    this.controlMaterialSalaRepository = new SolicitudMaterialSalaRepository(this.context);
                }
                return this.controlMaterialSalaRepository;
            }
        }
        public GenericRepository<ControlMaterialSalaDetalle> ControlMaterialSalaDetalleRepository
        {
            get
            {
                if (this.controlMaterialSalaDetalleRepository == null)
                {
                    this.controlMaterialSalaDetalleRepository = new GenericRepository<ControlMaterialSalaDetalle>(this.context);
                }
                return this.controlMaterialSalaDetalleRepository;
            }
        }

        public async Task SaveChangesAsync()
        {
            await this.context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}