using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.Common.DTOs
{
    public class BaseDTODBF
    {
        #region protected
        protected DateTime? GetDate(int? anio, int? mes, int? dia)
        {
            if (anio == null || mes == null || dia == null)
                return null;
            else
            {
                DateTime dt;
                if (DateTime.TryParse(anio + "-" + mes + "-" + dia, out dt))
                    return dt;
                else
                    return null;
            }

        }
        #endregion
    }
}
