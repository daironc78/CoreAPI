using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.Dapper.Paginacion.Model
{
    public class PaginacionModel
    {
        public List<IDictionary<string, object>> listaRecords { get; set; }
        public int numRecords { get; set; }
        public int numPaginas { get; set; }
    }
}
