using Apphr.Application.Common.DTOs;
using Apphr.Application.Common.Models;
using Apphr.Domain.Entities;
using System.Collections.Generic;

namespace Apphr.Application.Servicios.DTOs
{
    public class ServicioDTOIndex : ActionResultListDTOBase
    {        
        public IEnumerable<Servicio> Result { get; set; }
    }
}
