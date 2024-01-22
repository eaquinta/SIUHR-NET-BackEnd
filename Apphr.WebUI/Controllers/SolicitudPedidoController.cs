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
        //[Route("GetAll")]
        public IEnumerable<SolicitudPedidoDBF> GetAll()
        {
            return dbfContext.GetSolicitudesPedido();
        }

        // GET: api/SolicitudesPedidoController/5
        [HttpGet]
        //[Route("GetById")]
        [ResponseType(typeof(SolicitudPedidoDBF))]
        public IHttpActionResult GetById(string id)
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
