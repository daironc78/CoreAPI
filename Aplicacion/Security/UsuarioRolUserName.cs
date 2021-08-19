using Aplicacion.Customers;
using Dominio.Models;
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
    public class UsuarioRolUserName
    {
        public class UserRole : IRequest<List<string>>
        {
            public string UserName { get; set; }
        }

        public class Manejador : IRequestHandler<UserRole, List<string>>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            public Manejador(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;

            }

            public async Task<List<string>> Handle(UserRole request, CancellationToken cancellationToken)
            {
                try
                {
                    var user = await _userManager.FindByNameAsync(request.UserName);
                    if (user != null)
                    {
                        var result = await _userManager.GetRolesAsync(user);
                        return result.ToList();
                    }

                    throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "Usuario no existe favor validar" });

                }
                catch (Exception e)
                {
                    throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "Ha ocurrido un error en la transacción - " + e.Message });
                }

            }
        }
    }
}
