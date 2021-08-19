using Aplicacion.Customers;
using MediatR;
using Persistencia.Dapper.Instructor.Interface;
using Persistencia.Dapper.Instructor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Instructor
{
    public class ConsultaId
    {
        public class ConsultaInstructorId : IRequest<InstructorModel>
        {
            public Guid InstructorId { get; set; }
        }

        public class Manejador : IRequestHandler<ConsultaInstructorId, InstructorModel>
        {
            private readonly IInstructor _instructor;
            public Manejador(IInstructor instructor)
            {
                _instructor = instructor;
            }

            public async Task<InstructorModel> Handle(ConsultaInstructorId request, CancellationToken cancellationToken)
            {
                try
                {
                    var instructor = await _instructor.ObtenerInstructor(request.InstructorId);
                    if (instructor != null)
                        return instructor;

                    throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "No se ha podido consultar el Instructor por id" });
                }
                catch
                {
                    throw new ValidacionesError(HttpStatusCode.NotFound, new { message = "No se ha podido consultar el Instructor por id" });
                }
            }
        }
    }
}
