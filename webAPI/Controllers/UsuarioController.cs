using Aplicacion.Security;
using Aplicacion.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace webAPI.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class UsuarioController : MiControllerBase
    {
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<UsuarioPropiedades>> Login(Login.Loggeo parametros)
        {
            return await _mediator.Send(parametros);
        }

        [HttpPost]
        [Route("Registro")]
        public async Task<ActionResult<UsuarioPropiedades>> Registrar(Registro.RegistroUsuario parametros)
        {
            return await _mediator.Send(parametros);
        }

        [HttpGet]
        [Route("UsuarioSesion")]
        public async Task<ActionResult<UsuarioPropiedades>> UsuarioSesion()
        {
            return await _mediator.Send(new UsuarioActual.ActualUsuario());
        }

        [HttpPut]
        [Route("ActualizarUsuario")]
        public async Task<ActionResult<UsuarioPropiedades>> ActualizarUsuario(UsuarioActualizar.ActualizarUsuario parametros)
        {
            return await _mediator.Send(parametros);
        }
    }
}
