using Aplicacion.Contratos;
using Aplicacion.Customers;
using Aplicacion.Users;
using Dominio.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Security
{
    public class Login
    {
        public class Loggeo : IRequest<UsuarioPropiedades>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class LoggeoValidacion : AbstractValidator<Loggeo>
        {
            public LoggeoValidacion()
            {
                RuleFor(x => x.Email).NotEmpty().WithMessage("Por favor envie el Correo");
                RuleFor(x => x.Password).NotEmpty().WithMessage("Por favor envie la Password");
            }
        }

        public class Manejador : IRequestHandler<Loggeo, UsuarioPropiedades>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly SignInManager<Usuario> _signInManager;
            private readonly IJWTGenerador _JWTGenerador;
            public Manejador(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IJWTGenerador JWTGenerador)
            {
                this._userManager = userManager;
                this._signInManager = signInManager;
                this._JWTGenerador = JWTGenerador;
            }

            public async Task<UsuarioPropiedades> Handle(Loggeo request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByEmailAsync(request.Email);
                if (usuario != null)
                {
                    var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);
                    var listaRoles = await _userManager.GetRolesAsync(usuario);
                    if(resultado.Succeeded)
                    {
                        return new UsuarioPropiedades
                        {
                            UserId = usuario.Id,
                            NombreCompleto = usuario.NombreCompleto,
                            Token = _JWTGenerador.CrearToken(usuario, listaRoles.ToList()),
                            Email = usuario.Email,
                            UserName = usuario.UserName,
                            Imagen = null
                        };
                    }

                    throw new ValidacionesError(HttpStatusCode.Unauthorized);
                }

                throw new ValidacionesError(HttpStatusCode.Unauthorized);
            }
        }
    }
}
