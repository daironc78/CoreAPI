using Aplicacion.Contratos;
using Dominio.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Seguridad.TokenSeguridad
{
    public class JWTGenerador : IJWTGenerador
    {
        public string CrearToken(Usuario usuario, List<string> roles)
        {
            
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, usuario.UserName)
            };

            if (roles != null) 
            {
                foreach (var rol in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, rol));
                }
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KeyDecriptToken2021.Net+React"));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescripcion = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = credenciales
            };

            var tokenSeguridad = new JwtSecurityTokenHandler();
            var token = tokenSeguridad.CreateToken(tokenDescripcion);

            return tokenSeguridad.WriteToken(token);
        }
    }
}
