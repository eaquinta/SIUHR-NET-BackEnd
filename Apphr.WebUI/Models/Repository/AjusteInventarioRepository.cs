using Apphr.Domain.Entities;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Apphr.WebUI.Models.Repository
{
    public class AjusteInventarioRepository : GenericRepository<AjusteInventario>
    {
        public AjusteInventarioRepository(ApphrDbContext context) : base(context)
        {
        }

     
    }
}