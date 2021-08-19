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
using static Aplicacion.Cursos.Nuevo;

namespace Aplicacion.Cursos
{
    public class Editar
    {
        public class EditarCurso : IRequest
        {
            public Guid CursoId { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid> ListaInstructor { get; set; }
            public decimal? Precio { get; set; }
            public decimal? Promocion { get; set; }
        }

        public class Manejador : IRequestHandler<EditarCurso>
        {
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                this._context = context;
            }

            public async Task<Unit> Handle(EditarCurso request, CancellationToken cancellationToken)
            {
                var curso = await _context.Cursos.FindAsync(request.CursoId);
                var precio = _context.Precios.Where(x => x.CursoId == request.CursoId).FirstOrDefault();
                if (curso != null)
                {
                    curso.Titulo = request.Titulo ?? curso.Titulo;
                    curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                    curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;

                    if (precio != null)
                    {
                        precio.PrecioActual = request.Precio ?? precio.PrecioActual;
                        precio.Promocion = request.Promocion ?? precio.Promocion;
                    }
                    else
                    {
                        precio = new Precio
                        {
                            PrecioActual = request.Precio ?? 0,
                            Promocion = request.Promocion ?? 0,
                            CursoId = request.CursoId
                        };

                        _context.Precios.Add(precio);
                    }

                    if (request.ListaInstructor != null)
                    {
                        if (request.ListaInstructor.Count > 0)
                        {
                            var InstructoresDB = _context.CursoInstructors.Where(x => x.CursoId == request.CursoId).ToList();
                            foreach(var id in InstructoresDB)
                            {
                                _context.CursoInstructors.Remove(id);
                            }

                            foreach (var id in request.ListaInstructor)
                            {
                                var cursoInstructor = new CursoInstructor
                                {
                                    CursoId = request.CursoId,
                                    InstructorId = id
                                };

                                _context.CursoInstructors.Add(cursoInstructor);
                            }
                        }
                    }

                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                        return Unit.Value;

                    throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "No se ha podido realizar una actualizacion el curso" });
                }

                throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "No se ha encontrado el curso" });
            }
        }
    }
}
