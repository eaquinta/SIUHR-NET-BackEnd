using Apphr.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apphr.WebUI.Models.Repository
{
    public class SolicitudPedidoRepository : GenericRepository<SolicitudPedido>
    {
        public SolicitudPedidoRepository(ApphrDbContext context) : base(context)
        {
        }

        public string SolicitudPedidoIdFromDBF(int year, string Codigo)
        {
            return year.ToString() + "-" + Codigo.Trim().PadLeft(6, '0');
        }
    }
}