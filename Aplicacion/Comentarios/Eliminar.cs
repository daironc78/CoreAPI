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

namespace Aplicacion.Comentarios
{
    public class Eliminar
    {
        public class EliminarComentario : IRequest
        {
            public Guid ComentarioId { get; set; }
        }

        public class Manejador : IRequestHandler<EliminarComentario>
        {
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                this._context = context;
            }

            public async Task<Unit> Handle(EliminarComentario request, CancellationToken cancellationToken)
            {
                var comentarios = _context.Comentarios.Where(x => x.ComentarioID == request.ComentarioId);
                foreach (var comentario in comentarios)
                {
                    _context.Comentarios.Remove(comentario);
                }
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return Unit.Value;

                throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "No se ha encontrado el curso" });
            }
        }
    }
}
