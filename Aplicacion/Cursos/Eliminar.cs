using Aplicacion.Customers;
using MediatR;
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
    public class Eliminar
    {
        public class EliminarCurso : IRequest
        {
            public Guid CursoId { get; set; }
        }

        public class Manejador : IRequestHandler<EliminarCurso>
        {
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                this._context = context;
            }

            public async Task<Unit> Handle(EliminarCurso request, CancellationToken cancellationToken)
            {
                var cursosInstructores = _context.CursoInstructors.Where(x => x.CursoId == request.CursoId);
                foreach (var CursoInstructor in cursosInstructores)
                {
                    _context.CursoInstructors.Remove(CursoInstructor);
                }

                var cursoComentarios = _context.Comentarios.Where(x => x.CursoId == request.CursoId);
                foreach (var CursoComentario in cursoComentarios)
                {
                    _context.Comentarios.Remove(CursoComentario);
                }

                var cursoPrecio = _context.Precios.Where(x => x.CursoId == request.CursoId).FirstOrDefault();
                if (cursoPrecio != null)
                {
                    _context.Precios.Remove(cursoPrecio);
                }

                var curso = await _context.Cursos.FindAsync(request.CursoId);
                if (curso != null)
                {
                    _context.Remove(curso);
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                        return Unit.Value;

                    throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "Ha ocurrido un error al eliminar el curso" });
                }

                throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "No se ha encontrado el curso" });
            }
        }
    }
}
