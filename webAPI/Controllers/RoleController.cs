using Aplicacion.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace webAPI.Controllers
{
    public class RoleController : MiControllerBase
    {
        [HttpPost]
        [Route("CrearRole")]
        public async Task<ActionResult<Unit>> CrearRole([FromBody] RolNuevo.NuevoRol role)
        {
            return await _mediator.Send(role);
        }

        [HttpDelete]
        [Route("EliminarRole")]
        public async Task<ActionResult<Unit>> EliminarRole([FromBody] RolEliminar.EliminarRol role)
        {
            return await _mediator.Send(role);
        }

        [HttpGet]
        [Route("RolesLista")]
        public async Task<ActionResult<List<IdentityRole>>> ListaRoles()
        {
            return await _mediator.Send(new RolesLista.ListaRoles());
        }

        [HttpPost]
        [Route("AgregarUsuarioRol")]
        public async Task<ActionResult<Unit>> RoleUsuarioAgregar([FromBody] UsuarioRolAgregar.UserRole userRol)
        {
            return await _mediator.Send(userRol);
        }

        [HttpDelete]
        [Route("EliminarUsuarioRol")]
        public async Task<ActionResult<Unit>> RoleUsuarioQuitar([FromBody] UsuarioRolEliminar.UserRole userRol)
        {
            return await _mediator.Send(userRol);
        }

        [HttpGet]
        [Route("UsuariosRolesLista/{UserName}")]
        public async Task<ActionResult<List<string>>> ListaRoles(string userName)
        {
            return await _mediator.Send(new UsuarioRolUserName.UserRole { UserName = userName });
        }
    }
}
