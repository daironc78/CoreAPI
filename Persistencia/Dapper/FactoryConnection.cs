using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Data;

namespace Persistencia.Dapper
{
    public class FactoryConnection : IFactoryConnection
    {
        private IDbConnection _connection;
        private readonly IOptions<ConexionConfiguracion> _configs;
        public FactoryConnection(IOptions<ConexionConfiguracion> configs)
        {
            this._configs = configs;
        }

        public void CloseConnection()
        {
            // cerrar conexion
            if (_connection != null && _connection.State == ConnectionState.Open)
                _connection.Close();
        }

        public IDbConnection GetConnection()
        {
            // crear conexion
            if (_connection == null)
                _connection = new SqlConnection(_configs.Value.DefaultConnection);

            // abrir conexion
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            return _connection;
        }
    }
}
