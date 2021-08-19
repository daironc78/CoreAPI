using Aplicacion.Customers;
using Dominio.Models;
using FluentValidation;
using MediatR;
using Persistencia.Data;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
    public class Nuevo
    {
        public class CrearCurso : IRequest
        {
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid> ListaInstructor { get; set; }
            public decimal Precio { get; set; }
            public decimal Promocion { get; set; }
        }

        public class CrearCursoValidacion : AbstractValidator<CrearCurso>
        {
            public CrearCursoValidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty().WithMessage("Por favor envie el Titulo");
                RuleFor(x => x.Descripcion).NotEmpty().WithMessage("Por favor envie la Descripcion");
                RuleFor(x => x.FechaPublicacion).NotEmpty().WithMessage("Por favor envie la de Fecha Publicacion");
                RuleFor(x => x.Precio).NotEmpty().WithMessage("Por favor envie el precio");
                RuleFor(x => x.Promocion).NotEmpty().WithMessage("Por favor envie el precio promocion");
            }
        }

        public class Manejador : IRequestHandler<CrearCurso>
        {
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                this._context = context;
            }

            public async Task<Unit> Handle(CrearCurso request, CancellationToken cancellationToken)
            {
                // Curso
                var curso = new Curso
                {
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion
                };
                _context.Cursos.Add(curso);

                // Precio
                var precio = new Precio
                {
                    CursoId = curso.CursoId,
                    PrecioActual = request.Precio,
                    Promocion = request.Promocion
                };
                _context.Precios.Add(precio);

                // Instructor
                if (request.ListaInstructor != null)
                {
                    CursoInstructor cursoInstructor;
                    foreach (var id in request.ListaInstructor)
                    {
                        cursoInstructor = new CursoInstructor
                        {
                            CursoId = curso.CursoId,
                            InstructorId = id
                        };
                        _context.CursoInstructors.Add(cursoInstructor);
                    }
                    
                    var resultCurso = await _context.SaveChangesAsync();
                    if (resultCurso > 0)
                        return Unit.Value;

                    throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "No se ha podido crear el Curso ni asociar el Instructor" });
                }
                else
                {
                    var resultCurso = await _context.SaveChangesAsync();
                    if (resultCurso > 0)
                        return Unit.Value;

                    throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "No se ha podido crear el curso" });
                }

                throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "No se pudo crear el curso" });
            }
        }
    }
}
