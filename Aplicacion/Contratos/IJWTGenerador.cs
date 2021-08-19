using Dominio.Models;
using System.Collections.Generic;

namespace Aplicacion.Contratos
{
    public interface IJWTGenerador
    {
        string CrearToken(Usuario usuario, List<string> roles);
    }
}