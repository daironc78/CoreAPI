using Aplicacion.Customers;
using Aplicacion.DTO;
using AutoMapper;
using Dominio.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia.Data;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
    public class Consulta
    {
        public class ListaCursos : IRequest<List<CursoDTO>> {}
        public class Manejador : IRequestHandler<ListaCursos, List<CursoDTO>>
        {
            private readonly CursosOnlineContext _context;
            private readonly IMapper _mapper;
            public Manejador(CursosOnlineContext context, IMapper mapper)
            {
                this._context = context;
                this._mapper = mapper;
            }

            public async Task<List<CursoDTO>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                var cursos = await _context.Cursos
                    .Include(x => x.ComentarioLista)
                    .Include(x => x.PrecioPromocion)
                    .Include(x => x.InstructorLink).ThenInclude(x => x.Instructor).ToListAsync();
                if (cursos != null)
                {
                    var cursosDTO = _mapper.Map<List<Curso>, List<CursoDTO>>(cursos);
                    return cursosDTO;
                }

                throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "No se han encontrado registros" });
            }
        }
    }
}
