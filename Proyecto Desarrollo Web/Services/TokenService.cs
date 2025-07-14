using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Proyecto_Desarrollo_Web.Data;
using Proyecto_Desarrollo_Web.Models;

namespace Proyecto_Desarrollo_Web.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public TokenService(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        public string GenerarToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Name),
                new Claim("UsuarioId", usuario.Id.ToString())
            };


            var privilegios = _context.UsuariosPrivilegios
                .Where(up => up.UsuarioId == usuario.Id)
                .Select(up => up.Privilegio.Descripcion)
                .ToList();

            foreach (var priv in privilegios)
            {
                claims.Add(new Claim("Privilegio", priv));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
