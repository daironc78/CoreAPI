using Dapper;
using Persistencia.Dapper.Instructor.Interface;
using Persistencia.Dapper.Instructor.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Persistencia.Dapper.Instructor
{
    public class InstructorRepositorio : IInstructor
    {
        private readonly IFactoryConnection _factoryConnection;
        public InstructorRepositorio(IFactoryConnection factoryConnection)
        {
            this._factoryConnection = factoryConnection;
        }

        public async Task<int> Actualizar(Guid InstructorId, string Nombres, string Apellidos, string Titulo)
        {
            var result = 0;
            try
            {
                var connection = _factoryConnection.GetConnection();
                result = await connection.ExecuteAsync("sp_instructor_update", new
                {
                    InstructorId = InstructorId,
                    Nombres = Nombres,
                    Apellidos = Apellidos,
                    Titulo = Titulo
                }, commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                throw new Exception("Error guardando en la base de datos", e);
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }
            return result;
        }

        public async Task<int> Eliminar(Guid InstructorId)
        {
            var result = 0;
            try
            {
                var connection = _factoryConnection.GetConnection();
                result = await connection.ExecuteAsync("sp_instructor_borrar", new { InstructorId = InstructorId }, commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                throw new Exception("Error borrando en la base de datos", e);
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }
            return result;
        }

        public async Task<int> Nuevo(string Nombres, string Apellidos, string Titulo)
        {
            var result = 0;
            try
            {
                var connection = _factoryConnection.GetConnection();
                result = await connection.ExecuteAsync("sp_instructor_nuevo", new
                {
                    Nombres = Nombres,
                    Apellidos = Apellidos,
                    Titulo = Titulo
                }, commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                throw new Exception("Error guardando en la base de datos", e);
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }
            return result;
        }

        public async Task<InstructorModel> ObtenerInstructor(Guid InstructorId)
        {
            IEnumerable<InstructorModel> instructor;
            try
            {
                var connection = _factoryConnection.GetConnection();
                instructor = await connection.QueryAsync<InstructorModel>("sp_instructor_id", new { InstructorId = InstructorId }, commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                throw new Exception("Error consultando al instructor por id en la base de datos", e);
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }
            return instructor.FirstOrDefault();
        }

        public async Task<IEnumerable<InstructorModel>> ObtenerLista()
        {
            IEnumerable<InstructorModel> instructorList = null;
            try
            {
                var connection = _factoryConnection.GetConnection();
                instructorList = await connection.QueryAsync<InstructorModel>("sp_instructores_lista", null, commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                throw new Exception("Error consultando la base de datos", e);
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }
            return instructorList;
        }
    }
}
