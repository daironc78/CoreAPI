using Aplicacion.Customers;
using FluentValidation;
using MediatR;
using Persistencia.Dapper.Instructor.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Instructor
{
    public class Editar
    {
        public class EditarInstructor : IRequest
        {
            public Guid InstructorId;
            public string Nombres { get; set; }
            public string Apellidos { get; set; }
            public string Titulo { get; set; }
        }

        public class EditarInstructorValidacion : AbstractValidator<EditarInstructor>
        {
            public EditarInstructorValidacion()
            {
                RuleFor(x => x.Nombres).NotEmpty().WithMessage("Por favor envie su/s Nombre/s");
                RuleFor(x => x.Apellidos).NotEmpty().WithMessage("Por favor envie sus Apellidos");
                RuleFor(x => x.Titulo).NotEmpty().WithMessage("Por favor envie el Titulo");
            }
        }

        public class Manejador : IRequestHandler<EditarInstructor>
        {
            private readonly IInstructor _instructor;
            public Manejador(IInstructor instructor)
            {
                _instructor = instructor;
            }

            public async Task<Unit> Handle(EditarInstructor request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await _instructor.Actualizar(request.InstructorId, request.Nombres, request.Apellidos, request.Titulo);
                    if (result > 0)
                        return Unit.Value;

                    throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "No se ha podido actualizar el Instructor" });
                }
                catch
                {
                    throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "No se ha podido actualizar el Instructor" });
                }
            }
        }
    }
}
