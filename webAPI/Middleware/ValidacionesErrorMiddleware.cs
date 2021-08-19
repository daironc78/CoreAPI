using Aplicacion.Customers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace webAPI.Middleware
{
    public class ValidacionesErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ValidacionesErrorMiddleware> _logger;
        public ValidacionesErrorMiddleware(RequestDelegate next, ILogger<ValidacionesErrorMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await ValidacionExceptionAsync(httpContext, e, _logger);
            }
        }

        public async Task ValidacionExceptionAsync(HttpContext httpContext, Exception e, ILogger<ValidacionesErrorMiddleware> logger)
        {
            object errores = null;
            switch (e)
            {
                case ValidacionesError validacionesError:
                    logger.LogError(e, "Validacion de error");
                    errores = validacionesError._errores;
                    httpContext.Response.StatusCode = (int)validacionesError._codigo;
                    break;
                case Exception ex:
                    logger.LogError(e, "Error de servidor");
                    errores = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            httpContext.Response.ContentType = "application/json";
            if(errores != null)
            {
                var resultados = JsonConvert.SerializeObject(new { errores });
                await httpContext.Response.WriteAsync(resultados);
            }
        }
    }
}
