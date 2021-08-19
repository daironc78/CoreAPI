using System;
using System.Net;

namespace Aplicacion.Customers
{
    public class ValidacionesError : Exception
    {
        public HttpStatusCode _codigo { get; }
        public object _errores { get; }

        public ValidacionesError(HttpStatusCode codigo, object errores = null)
        {
            this._codigo = codigo;
            this._errores = errores;
        }

    }
}
