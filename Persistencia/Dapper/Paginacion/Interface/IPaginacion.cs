using Persistencia.Dapper.Paginacion.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistencia.Dapper.Paginacion.Interface
{
    public interface IPaginacion
    {
        Task<PaginacionModel> DevolverPaginacion(string procedure, int numPagina, int numElementos, IDictionary<string,object> parametrosFiltro, string ordenamientoColumna);
    }
}
