using Aplicacion.Cursos;
using Aplicacion.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistencia.Dapper.Paginacion.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace webAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CursosController : MiControllerBase
    {
        [HttpGet]
        [Route("ListaCursos")]
        public async Task<ActionResult<List<CursoDTO>>> GetListaCursos()
        {
            return await _mediator.Send(new Consulta.ListaCursos());
        }

        [HttpGet]
        [Route("CursoId/{cursoId}")]
        public async Task<ActionResult<CursoDTO>> GetCursoId(Guid cursoId)
        {
            return await _mediator.Send(new ConsultaId.CursoUnico{ CursoId = cursoId });
        }

        [HttpPost]
        [Route("CrearCurso")]
        public async Task<ActionResult<Unit>> PostCrearCurso([FromBody] Nuevo.CrearCurso curso)
        {
            return await _mediator.Send(curso);
        }

        [HttpPut]
        [Route("ActualizarCurso/{cursoId}")]
        public async Task<ActionResult<Unit>> PutActualizarCurso(Guid cursoId, [FromBody] Editar.EditarCurso curso)
        {
            curso.CursoId = cursoId;
            return await _mediator.Send(curso);
        }

        [HttpDelete]
        [Route("EliminarCurso/{cursoId}")]
        public async Task<ActionResult<Unit>> DeleteActualizarCurso(Guid cursoId)
        {
            return await _mediator.Send(new Eliminar.EliminarCurso { CursoId = cursoId });
        }

        [HttpPost]
        [Route("CursosPaginacion")]
        public async Task<ActionResult<PaginacionModel>> PostPaginacionCursos([FromBody] PaginacionCurso.Ejecuta cursos)
        {
            return await _mediator.Send(cursos);
        }
    }
}