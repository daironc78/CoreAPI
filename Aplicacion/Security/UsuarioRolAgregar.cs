using Aplicacion.Customers;
using Dominio.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Security
{
    public class UsuarioRolAgregar
    {
        public class UserRole : IRequest
        {
            public string UserName { get; set; }
            public string RoleName { get; set; }
        }

        public class RegistroUsuarioValidacion : AbstractValidator<UserRole>
        {
            public RegistroUsuarioValidacion()
            {
                RuleFor(x => x.UserName).NotEmpty().WithMessage("Por favor envie Username");
                RuleFor(x => x.RoleName).NotEmpty().WithMessage("Por favor envie nombre del Rol");
            }
        }

        public class Manejador : IRequestHandler<UserRole>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            public Manejador(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;

            }

            public async Task<Unit> Handle(UserRole request, CancellationToken cancellationToken)
            {
                var role = await _roleManager.FindByNameAsync(request.RoleName);
                if (role != null)
                {
                    var user = await _userManager.FindByNameAsync(request.UserName);
                    if (user != null)
                    {
                        var result = await _userManager.AddToRoleAsync(user, request.RoleName);
                        if (result.Succeeded)
                            return Unit.Value;

                        throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "Ha ocurrido un error en la transacción" });
                    }

                    throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "Usuario no existe favor validar" });
                }

                throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "Rol no existe favor validar" });
            }
        }
    }
}
