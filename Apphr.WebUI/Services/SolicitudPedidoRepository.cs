using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apphr.WebUI.Services
{
    public class SolicitudPedidoRepository
    {
        public static string FixFormatId(string id)
        {
            return id.PadLeft(4);
        }
    }
}