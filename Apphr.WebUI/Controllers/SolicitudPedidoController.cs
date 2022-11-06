using Apphr.Domain.EntitiesDBF;
using Apphr.WebUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Apphr.WebUI.Controllers
{
    [Authorize]
    public class SolicitudesPedidoController : DbApiController
    {
        //GET: api/SolicitudesPedidoController
        [HttpGet]
        public IEnumerable<SolicitudPedidoDBF> GetAll()
        {
            return dbfContext.GetSolicitudesPedido();
        }

        // GET: api/SolicitudesPedidoController/5
        [HttpGet]
        [ResponseType(typeof(SolicitudPedidoDBF))]
        public IHttpActionResult GetId(string id)
        {
            id = SolicitudPedidoRepository.FixFormatId(id);
            SolicitudPedidoDBF reg = this.dbfContext.GetSolicitudPedido(id);
            if (reg != null)
            {
                List<SolicitudPedidoDetalleDBF> regDet = this.dbfContext.GetSolicitudPedidoDetalle(id);
                reg.Detalle = regDet;
            }
            //if (reg == null)
            //{
            //    return NotFound();
            //}

            return Ok(reg);
            //return Json(reg);
        }
    }
}
