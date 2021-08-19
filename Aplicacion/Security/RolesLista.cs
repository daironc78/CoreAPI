using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Security
{
    public class RolesLista
    {
        public class ListaRoles : IRequest<List<IdentityRole>> { }

        public class Manejador : IRequestHandler<ListaRoles, List<IdentityRole>>
        {
            private readonly CursosOnlineContext _context;

            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }

            public async Task<List<IdentityRole>> Handle(ListaRoles request, CancellationToken cancellationToken)
            {
                var listaRoles = await _context.Roles.ToListAsync();
                return listaRoles;
            }
        }
    }
}
