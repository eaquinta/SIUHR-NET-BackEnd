using System;
using System.Globalization;

namespace Apphr.Domain.Common
{
    public class BaseDBF
    {
        #region protected
        protected DateTime? GetDateStr(string cadena)
        {
            if (string.IsNullOrEmpty(cadena) || cadena.Length != 8)
            {
                return null;
            }
            string anio = cadena.Substring(0, 4);
            string mes  = cadena.Substring(4, 2);
            string dia  = cadena.Substring(6, 2);
            DateTime dt;
            if (DateTime.TryParse(anio + "-" + mes + "-" + dia, out dt))
            {
                if(IsValidSqlDateTime(dt))
                {
                    return dt;
                }                
            }                
            return null;
        }
        protected DateTime? GetDate(int? anio, int? mes, int? dia)
        {
            if (anio == null || mes == null || dia == null)
                return null;
            else
            {
                DateTime dt;
                if (DateTime.TryParse(anio + "-" + mes + "-" + dia, out dt))
                {
                    if (IsValidSqlDateTime(dt))
                    {
                        return dt;
                    }
                }
                return null;
            }
        }

        protected DateTime? GetTime(string sTime)
        {
            DateTime res = DateTime.ParseExact(sTime, "HH:mm:ss", CultureInfo.InvariantCulture);
            return res;
        }

        protected DateTime? ConcatDateTime(DateTime? date, DateTime? time)
        {
            if (date == null)
            {
                return null;
            }
            if (time == null)
            {
                return date.Value.Date;
            }
            return date.Value.Date.Add(time.Value.TimeOfDay);
        }

        private bool IsValidSqlDateTime(DateTime? dateTime)
        {
            if (dateTime == null) return true;

            DateTime minValue = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            DateTime maxValue = (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue;

            if (minValue > dateTime.Value || maxValue < dateTime.Value)
                return false;

            return true;
        }
        #endregion
    }
}
