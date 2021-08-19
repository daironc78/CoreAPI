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
    public class Consulta
    {
        public class ListaInstructores : IRequest<List<InstructorModel>> { }
        public class Manejador : IRequestHandler<ListaInstructores, List<InstructorModel>>
        {
            private readonly IInstructor _instructor;
            public Manejador(IInstructor instructor)
            {
                this._instructor = instructor;
            }

            public async Task<List<InstructorModel>> Handle(ListaInstructores request, CancellationToken cancellationToken)
            {
                try
                {
                    var instructores = await _instructor.ObtenerLista();
                    if (instructores != null)
                        return instructores.ToList();

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
