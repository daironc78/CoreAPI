using Aplicacion.Customers;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Security
{
    public class RolNuevo
    {
        public class NuevoRol : IRequest
        {
            public string Nombre { get; set; }
        }

        public class RegistroUsuarioValidacion : AbstractValidator<NuevoRol>
        {
            public RegistroUsuarioValidacion()
            {
                RuleFor(x => x.Nombre).NotEmpty().WithMessage("Por favor envie nombre del Rol");
            }
        }

        public class Manejador : IRequestHandler<NuevoRol>
        {
            private readonly RoleManager<IdentityRole> _roleManager;

            public Manejador(RoleManager<IdentityRole> roleManager)
            {
                _roleManager = roleManager;
            }

            public async Task<Unit> Handle(NuevoRol request, CancellationToken cancellationToken)
            {
                var existe = await _roleManager.FindByNameAsync(request.Nombre);
                if (existe == null)
                {
                    var resultado = await _roleManager.CreateAsync(new IdentityRole(request.Nombre));
                    if (resultado.Succeeded)
                        return Unit.Value;

                    throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "Ha ocurrido un problema registrando el rol" });
                }

                throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "El rol ya se encuentra registrado" });
            }
        }
    }
}