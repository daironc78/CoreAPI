using Persistencia.Dapper.Instructor.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistencia.Dapper.Instructor.Interface
{
    public interface IInstructor
    {
        Task<IEnumerable<InstructorModel>> ObtenerLista();
        Task<InstructorModel> ObtenerInstructor(Guid InstructorId);
        Task<int> Nuevo(string Nombres, string Apellidos, string Titulo);
        Task<int> Actualizar(Guid InstructorId, string Nombres, string Apellidos, string Titulo);
        Task<int> Eliminar(Guid InstructorId);
    }
}
