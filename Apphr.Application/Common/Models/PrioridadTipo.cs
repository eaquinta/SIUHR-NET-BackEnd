using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Apphr.Application.Common.Models
{
    public class PrioridadTipo
    {
        public static List<SelectListItem> GetSelectList() {
            List<SelectListItem> result = new List<SelectListItem>
            {
                //var defaultItem = new SelectListItem() { Value = "", Text = "Seleccione Tipo" };
                //result.Insert(0, defaultItem);
                new SelectListItem { Text = "Regular", Value = "R" },
                new SelectListItem { Text = "Especial", Value = "E" },
                new SelectListItem { Text = "Urgente", Value = "U" }
            };
            return result;
        }
    }
}
