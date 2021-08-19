using Aplicacion.Customers;
using Aplicacion.DTO;
using AutoMapper;
using Dominio.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
    public class ConsultaId
    {
        public class CursoUnico : IRequest<CursoDTO>
        {
            public Guid CursoId { get; set; }
        }

        public class Manejador : IRequestHandler<CursoUnico, CursoDTO>
        {
            private readonly CursosOnlineContext _context;
            private readonly IMapper _mapper;
            public Manejador(CursosOnlineContext context, IMapper mapper)
            {
                this._context = context;
                this._mapper = mapper;
            }

            public async Task<CursoDTO> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                var curso = await _context.Cursos.Include(x => x.InstructorLink).ThenInclude(x => x.Instructor).FirstOrDefaultAsync(x => x.CursoId == request.CursoId);
                if (curso != null)
                {
                    var cursoDTO = _mapper.Map<Curso, CursoDTO>(curso);
                    return cursoDTO;
                }

                throw new ValidacionesError(HttpStatusCode.NotFound, new { codeError = 100, message = "No se ha encontrado el usuario con el ID : " + request.CursoId });
                
            }
        }
    }
}
