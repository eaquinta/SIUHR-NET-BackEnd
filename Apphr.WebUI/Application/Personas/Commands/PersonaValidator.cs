using FluentValidation;
using Apphr.Application.Personas.DTOs;

namespace Apphr.Application.Personas.Commands
{
    public class PersonaValidator : AbstractValidator<PersonaDTOEdit>
    {
        public PersonaValidator()            
        {
            //RuleFor(p => p.Nit).NotEmpty();
        }
    }
}
