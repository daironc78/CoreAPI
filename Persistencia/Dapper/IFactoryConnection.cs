using System.Data;

namespace Persistencia.Dapper
{
    public interface IFactoryConnection
    {
        void CloseConnection();
        IDbConnection GetConnection();
    }
}
