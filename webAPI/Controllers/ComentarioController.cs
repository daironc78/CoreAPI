using Aplicacion.Comentarios;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace webAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComentarioController : MiControllerBase
    {
        [HttpPost]
        [Route("CrearComentario")]
        public async Task<ActionResult<Unit>> PostCrearCurso([FromBody] Nuevo.CrearComentario comentario)
        {
            return await _mediator.Send(comentario);
        }

        [HttpDelete]
        [Route("EliminarComentario/{comentarioId}")]
        public async Task<ActionResult<Unit>> DeleteActualizarCurso(Guid comentarioId)
        {
            return await _mediator.Send(new Eliminar.EliminarComentario { ComentarioId = comentarioId });
        }
    }
}
