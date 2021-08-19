using Aplicacion.Contratos;
using Aplicacion.Customers;
using Aplicacion.Users;
using Dominio.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
    public class UsuarioActual
    {
        public class ActualUsuario : IRequest<UsuarioPropiedades> { }

        public class Manejador : IRequestHandler<ActualUsuario, UsuarioPropiedades>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly IJWTGenerador _JWTGenerador;
            private readonly IUsuarioSesion _usuarioSesion;

            public Manejador(UserManager<Usuario> userManager, IJWTGenerador jWTGenerador, IUsuarioSesion usuarioSesion)
            {
                this._userManager = userManager;
                this._usuarioSesion = usuarioSesion;
                this._JWTGenerador = jWTGenerador;
            }

            public async Task<UsuarioPropiedades> Handle(ActualUsuario request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());
                var listaRoles = await _userManager.GetRolesAsync(usuario);
                return new UsuarioPropiedades
                {
                    UserId = usuario.Id,
                    NombreCompleto = usuario.NombreCompleto,
                    UserName = usuario.UserName,
                    Token = _JWTGenerador.CrearToken(usuario,listaRoles.ToList()),
                    Imagen = null,
                    Email = usuario.Email
                };
            }
        }
    }
}
