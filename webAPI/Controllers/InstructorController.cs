using Aplicacion.Instructor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistencia.Dapper.Instructor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InstructorController : MiControllerBase
    {
        [HttpGet]
        [Route("ListaInstructores")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<InstructorModel>>> ObtenerInstructores()
        {
            return await _mediator.Send(new Consulta.ListaInstructores());
        }

        [HttpPost]
        [Route("NuevoInstructor")]
        public async Task<ActionResult<Unit>> NuevoInstructor(Nuevo.NuevoInstructor instructor)
        {
            return await _mediator.Send(instructor);
        }

        [HttpPut]
        [Route("EditarInstructor/{InstructorId}")]
        public async Task<ActionResult<Unit>> EditarInstructor(Guid InstructorId, Editar.EditarInstructor instructor)
        {
            instructor.InstructorId = InstructorId;
            return await _mediator.Send(instructor);
        }

        [HttpDelete]
        [Route("EliminarInstructor/{InstructorId}")]
        public async Task<ActionResult<Unit>> EditarInstructor(Guid InstructorId)
        {
            return await _mediator.Send(new Eliminar.EliminarInstructor { InstructorId = InstructorId } );
        }

        [HttpGet]
        [Route("InstructorID/{InstructorId}")]
        public async Task<ActionResult<InstructorModel>> Instructor(Guid InstructorId)
        {
            return await _mediator.Send(new ConsultaId.ConsultaInstructorId { InstructorId = InstructorId });
        }
    }
}
