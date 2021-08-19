using Aplicacion.Customers;
using FluentValidation;
using MediatR;
using Persistencia.Dapper.Instructor.Interface;
using Persistencia.Dapper.Instructor.Model;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Instructor
{
    public class Nuevo
    {
        public class NuevoInstructor : IRequest { 
            public string Nombres { get; set; }
            public string Apellidos { get; set; }
            public string Titulo { get; set; }
        }

        public class CrearInstructorValidacion : AbstractValidator<NuevoInstructor>
        {
            public CrearInstructorValidacion()
            {
                RuleFor(x => x.Nombres).NotEmpty().WithMessage("Por favor envie su/s Nombre/s");
                RuleFor(x => x.Apellidos).NotEmpty().WithMessage("Por favor envie sus Apellidos");
                RuleFor(x => x.Titulo).NotEmpty().WithMessage("Por favor envie el Titulo");
            }
        }

        public class Manejador : IRequestHandler<NuevoInstructor>
        {
            private readonly IInstructor _instructor;
            public Manejador(IInstructor instructor)
            {
                _instructor = instructor;
            }

            public async Task<Unit> Handle(NuevoInstructor request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await _instructor.Nuevo(request.Nombres, request.Apellidos, request.Titulo);
                    if (result > 0)
                        return Unit.Value;

                    throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "No se ha podido crear el Instructor" });
                }
                catch
                {
                    throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "No se ha podido crear el Instructor" });
                }
            }
        }
    }
}
