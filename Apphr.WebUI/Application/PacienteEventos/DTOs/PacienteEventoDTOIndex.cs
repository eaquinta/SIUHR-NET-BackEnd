﻿using Apphr.Application.Common.DTOs;
using Apphr.WebUI.Models.Entities;
using System.Collections.Generic;

namespace Apphr.Application.PacienteEventos.DTOs
{
    public class PacienteEventoDTOIndex : ActionResultListDTOBase
    {
        public PacienteEventoDTOIxFilter F { get; set; }
        public IEnumerable<PacienteEvento> Result { get; set; }
    }
}
