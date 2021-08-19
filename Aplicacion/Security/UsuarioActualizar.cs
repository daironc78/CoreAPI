using Aplicacion.Contratos;
using Aplicacion.Customers;
using Aplicacion.Users;
using Dominio.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Security
{
    public class UsuarioActualizar
    {
        public class ActualizarUsuario : IRequest<UsuarioPropiedades>
        {
            public string NombreCompleto { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string UserName { get; set; }
        }

        public class ActualizarUsuarioValidacion : AbstractValidator<ActualizarUsuario>
        {
            public ActualizarUsuarioValidacion()
            {
                RuleFor(x => x.NombreCompleto).NotEmpty().WithMessage("Por favor envie su Nombre completo");
                RuleFor(x => x.Email).NotEmpty().WithMessage("Por favor envie el Correo");
                RuleFor(x => x.Password).NotEmpty().WithMessage("Por favor envie la Password");
                RuleFor(x => x.UserName).NotEmpty().WithMessage("Por favor envie el Correo");
            }
        }

        public class Manejador : IRequestHandler<ActualizarUsuario, UsuarioPropiedades>
        {
            private readonly CursosOnlineContext _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly IJWTGenerador _JWTGenerador;
            private readonly IPasswordHasher<Usuario> _passwordHasher;
            public Manejador(CursosOnlineContext context, UserManager<Usuario> userManager, IJWTGenerador JWTGenerador, IPasswordHasher<Usuario> passwordHasher)
            {
                _context = context;
                _userManager = userManager;
                _JWTGenerador = JWTGenerador;
                _passwordHasher = passwordHasher;
            }

            public async Task<UsuarioPropiedades> Handle(ActualizarUsuario request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user != null)
                {
                    var users = await _context.Users.Where(x => x.Email == request.Email && x.UserName != request.UserName).AnyAsync(); ;
                    if (!users)
                    {
                        user.NombreCompleto = request.NombreCompleto;
                        user.Email = request.Email;
                        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
                        var update = await _userManager.UpdateAsync(user);
                        if (update.Succeeded)
                        {
                            var listaRoles = await _userManager.GetRolesAsync(user);
                            return new UsuarioPropiedades
                            {
                                UserId = user.Id,
                                NombreCompleto = user.NombreCompleto,
                                UserName = user.UserName,
                                Email = user.Email,
                                Token = _JWTGenerador.CrearToken(user, listaRoles.ToList())
                            };
                        }

                    }

                    throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "El Email ya se encuentra registrado" });
                }

                throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "Usuario no se encuentra registrado" });
            }
        }
    }
}
