using Aplicacion.Contratos;
using Aplicacion.Customers;
using Aplicacion.Users;
using Dominio.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia.Data;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Security
{
    public class Registro
    {
        public class RegistroUsuario : IRequest<UsuarioPropiedades>
        {
            public string NombreCompleto { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string UserName { get; set; }
        }

        public class RegistroUsuarioValidacion : AbstractValidator<RegistroUsuario>
        {
            public RegistroUsuarioValidacion()
            {
                RuleFor(x => x.NombreCompleto).NotEmpty().WithMessage("Por favor envie su Nombre completo");
                RuleFor(x => x.UserName).NotEmpty().WithMessage("Por favor envie su nombre de usuario");
                RuleFor(x => x.Email).NotEmpty().WithMessage("Por favor envie su Email");
                RuleFor(x => x.Password).NotEmpty().WithMessage("Por favor envie su Password");
            }
        }

        public class Manejador : IRequestHandler<RegistroUsuario, UsuarioPropiedades>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly CursosOnlineContext _context;
            private readonly IJWTGenerador _JWTGenerador;

            public Manejador(UserManager<Usuario> userManager, CursosOnlineContext context, IJWTGenerador jWTGenerador)
            {
                this._userManager = userManager;
                this._context = context;
                this._JWTGenerador = jWTGenerador;
            }

            public async Task<UsuarioPropiedades> Handle(RegistroUsuario request, CancellationToken cancellationToken)
            {
                var existe = await _context.Users.Where(x => x.Email == request.Email && x.UserName == request.UserName).AnyAsync();
                if (!existe)
                {
                    var usuario = new Usuario
                    {
                        NombreCompleto = request.NombreCompleto,
                        Email = request.Email,
                        UserName = request.UserName
                    };

                    var result = await _userManager.CreateAsync(usuario, request.Password);
                    if (result.Succeeded)
                    {
                        var user = await _userManager.FindByEmailAsync(request.Email);
                        return new UsuarioPropiedades
                        {
                            NombreCompleto = user.NombreCompleto,
                            Token = _JWTGenerador.CrearToken(user, null),
                            Email = user.Email,
                            UserName = user.UserName,
                            Imagen = null
                        };
                    }

                    throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "Ha ocurrido un error registrando el usuario" });
                }

                throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "Usuario ya se encuentra registrado con este Email y UserName" });
            }
        }
    }
}