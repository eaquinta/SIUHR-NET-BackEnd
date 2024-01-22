using FluentValidation;
using Apphr.Application.Proveedores.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apphr.Application.Proveedores.Commands
{
    public class ProveedorValidator : AbstractValidator<ProveedorDTOCEdit>
    {
        public ProveedorValidator()            
        {
            RuleFor(p => p.Nit).NotEmpty();
        }
    }
}
