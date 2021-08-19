using Aplicacion.Customers;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Security
{
    public class RolEliminar
    {
        public class EliminarRol : IRequest
        {
            public string Nombre { get; set; }
        }

        public class RegistroUsuarioValidacion : AbstractValidator<EliminarRol>
        {
            public RegistroUsuarioValidacion()
            {
                RuleFor(x => x.Nombre).NotEmpty().WithMessage("Por favor envie nombre del Rol");
            }
        }

        public class Manejador : IRequestHandler<EliminarRol>
        {
            private readonly RoleManager<IdentityRole> _roleManager;

            public Manejador(RoleManager<IdentityRole> roleManager)
            {
                _roleManager = roleManager;
            }

            public async Task<Unit> Handle(EliminarRol request, CancellationToken cancellationToken)
            {
                var existe = await _roleManager.FindByNameAsync(request.Nombre);
                if (existe == null)
                {
                    var resultado = await _roleManager.DeleteAsync(existe);
                    if (resultado.Succeeded)
                        return Unit.Value;

                    throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "Ha ocurrido un problema al eliminar el rol" });
                }

                throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "No existe el rol solitado" });
            }
        }
    }
}
