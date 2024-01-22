using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Apphr.Application.Common.Models
{
    public class TipoEvento
    {
        public List<SelectListItem> Lista = new List<SelectListItem>
            {               
                new SelectListItem { Text = "Seleccione Tipo", Value = "" },
                new SelectListItem { Text = "Compra Directa", Value = "CD" },
                new SelectListItem { Text = "Evento Cotización", Value = "EC" }
            };
        public static List<SelectListItem> GetSelectList()
        {
            var o = new TipoEvento();
            return o.Lista;
        }
        public static string GetText(string Value)
        {
            var o = new TipoEvento();
            return o.Lista.Where(x => x.Value == Value).FirstOrDefault()?.Text;
        }
    }
    public class TipoPrioridad
    {
        public List<SelectListItem> Lista = new List<SelectListItem>
            {
                new SelectListItem { Text = "Regular", Value = "R" },
                new SelectListItem { Text = "Especial", Value = "E" },
                new SelectListItem { Text = "Urgente", Value = "U" }
            };
        public static List<SelectListItem> GetSelectList() {
            var o = new TipoPrioridad();
            return o.Lista;
        }
        public static string GetText(string Value)
        {
            var o = new TipoPrioridad();
            return o.Lista.Where(x => x.Value == Value).FirstOrDefault()?.Text;
        }
    }
}
