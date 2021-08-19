using Dapper;
using Persistencia.Dapper.Paginacion.Interface;
using Persistencia.Dapper.Paginacion.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Persistencia.Dapper.Paginacion
{
    public class PaginacionRepositorio : IPaginacion
    {
        private readonly IFactoryConnection _factoryConnection;
        public PaginacionRepositorio(IFactoryConnection factoryConnection)
        {
            _factoryConnection = factoryConnection;
        }

        public async Task<PaginacionModel> DevolverPaginacion(string procedure, int numPagina, int numElementos, IDictionary<string, object> parametrosFiltro, string ordenamientoColumna)
        {
            PaginacionModel paginacion = new PaginacionModel();
            DynamicParameters parameters = new DynamicParameters();
            List<IDictionary<string, object>> listaReporte = null;
            int totalRecords = 0;
            int totalPaginas = 0;
            try
            {
                var connection = _factoryConnection.GetConnection();
                // ENTRADA
                foreach (var param in parametrosFiltro)
                {
                    parameters.Add("@" + param.Key, param.Value);
                }
                parameters.Add("@NumPagina", numPagina);
                parameters.Add("@NumElementos", numElementos);
                parameters.Add("@Ordenamiento", ordenamientoColumna);
                
                // SALIDA
                parameters.Add("@TotalRecords", totalRecords, DbType.Int32, ParameterDirection.Output);
                parameters.Add("@TotalPaginas", totalPaginas, DbType.Int32, ParameterDirection.Output);

                var result = await connection.QueryAsync(procedure, parameters, commandType : CommandType.StoredProcedure);
                listaReporte = result.Select(x => (IDictionary<string,object>)x).ToList();
                paginacion.listaRecords = listaReporte;
                paginacion.numPaginas = parameters.Get<int>("@TotalPaginas");
                paginacion.numRecords = parameters.Get<int>("@TotalRecords");
            }
            catch (Exception e)
            {
                throw new Exception("No se pudo ejecutar el procedimiento almacenado",e);
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }
            return paginacion;
        }
    }
}
