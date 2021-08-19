using Dominio.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Persistencia.Data
{
    public class DataPrueba
    {
        public static async Task InsertarData(CursosOnlineContext context, UserManager<Usuario> usuarioManager)
        {
            if (!usuarioManager.Users.Any())
            {
                var usuario = new Usuario
                {
                    NombreCompleto = "dairon Castro",
                    UserName = "nanimo",
                    Email = "daironc78@gmail.com",
                };
                await usuarioManager.CreateAsync(usuario, "Ninbus2000$");
            }
        }
    }
}
