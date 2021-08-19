using Aplicacion.Customers;
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
    public class Eliminar
    {
        public class EliminarInstructor : IRequest
        {
            public Guid InstructorId { get; set; }
        }

        public class Manejador : IRequestHandler<EliminarInstructor>
        {
            private readonly IInstructor _instructor;
            public Manejador(IInstructor instructor)
            {
                _instructor = instructor;
            }

            public async Task<Unit> Handle(EliminarInstructor request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await _instructor.Eliminar(request.InstructorId);
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
