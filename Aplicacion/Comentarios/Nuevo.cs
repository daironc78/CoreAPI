using Aplicacion.Customers;
using Dominio.Models;
using FluentValidation;
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
    public class Nuevo
    {
        public class CrearComentario : IRequest
        {
            public string Alumno { get; set; }
            public int? Puntaje { get; set; }
            public string Comentario { get; set; }
            public Guid CursoId { get; set; }
        }

        public class CrearCursoValidacion : AbstractValidator<CrearComentario>
        {
            public CrearCursoValidacion()
            {
                RuleFor(x => x.Alumno).NotEmpty().WithMessage("Por favor envie el Alumno");
                RuleFor(x => x.Puntaje).NotEmpty().WithMessage("Por favor envie el Puntaje");
                RuleFor(x => x.Comentario).NotEmpty().WithMessage("Por favor envie su Comentario");
                RuleFor(x => x.CursoId).NotEmpty().WithMessage("Por favor envie el Id del curso");
            }
        }

        public class Manejador : IRequestHandler<CrearComentario>
        {
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(CrearComentario request, CancellationToken cancellationToken)
            {
                var comentario = new Comentario
                {
                    Alumno = request.Alumno,
                    ComentarioTexto = request.Comentario,
                    Puntaje = request.Puntaje ?? 0,
                    CursoId = request.CursoId
                };

                _context.Comentarios.Add(comentario);
                var resultCurso = await _context.SaveChangesAsync();
                if (resultCurso > 0)
                    return Unit.Value;

                throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "No se pudo crear el comentario" });
            }
        }
    }
}
