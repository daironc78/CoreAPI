using MediatR;
using Persistencia.Dapper.Paginacion.Interface;
using Persistencia.Dapper.Paginacion.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
    public class PaginacionCurso
    {
        public class Ejecuta : IRequest<PaginacionModel>
        {
            public int NumPagina { get; set; }
            public int NumRegistros { get; set; }
            public string Titulo { get; set; }
        }
        public class Manejador : IRequestHandler<Ejecuta, PaginacionModel>
        {
            private readonly IPaginacion _paginacion;
            public Manejador(IPaginacion paginacion)
            {
                _paginacion = paginacion;
            }

            public async Task<PaginacionModel> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("NombreCurso",request.Titulo);

                return await _paginacion.DevolverPaginacion("sp_obtenerPaginacion", request.NumPagina,request.NumRegistros,parametros, "Titulo");
            }
        }
    }
}
