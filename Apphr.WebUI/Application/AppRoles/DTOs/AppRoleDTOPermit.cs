using Apphr.WebUI.Application.AppPermissions.DTOs;
using Apphr.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apphr.WebUI.Application.AppRoles.DTOs
{
    public class AppRoleDTOPermit
    {
        public AppRoleDTOBase Role { get; set; }
        public List<AppPermissionDTOAssign> Permisos { get; set; }
    }
}