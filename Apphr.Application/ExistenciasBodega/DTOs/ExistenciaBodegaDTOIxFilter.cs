using Apphr.Application.Common.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apphr.Application.ExistenciasBodega.DTOs
{
    public class ExistenciaBodegaDTOIxFilter : DTOIxFilterBodMat
    {  
        [UIHint("DropDown")]
        public int TipoExistencia { get; set; }
    }
}
