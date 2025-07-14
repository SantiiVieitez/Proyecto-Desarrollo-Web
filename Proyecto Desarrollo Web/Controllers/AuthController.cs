using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Desarrollo_Web.Data;
using Proyecto_Desarrollo_Web.Models;
using Proyecto_Desarrollo_Web.Services;
using System.Security.Cryptography;

namespace Proyecto_Desarrollo_Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public AuthController(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpGet("salt")]
        public IActionResult GetSalt(string username)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Name == username);
            if (usuario == null)
                return NotFound(new { message = "Usuario no encontrado" });

            return Ok(new { salt = usuario.Salt });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Name == request.User &&
                                     u.ClaveHash == request.Password);

            if (usuario == null || !usuario.Activo)
                return Unauthorized("Credenciales inválidas");

            var token = _tokenService.GenerarToken(usuario);

            var refreshToken = new RefreshToken
            {
                UserId = usuario.Id,
                Token = GenerateRefreshToken(),
                Expires = DateTime.UtcNow.AddDays(7)
            };

            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChanges();

            return Ok(new
            {
                token,
                refreshToken = refreshToken.Token
            });
        }

        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] string refreshToken)
        {
            var storedToken = _context.RefreshTokens
                .Include(t => t.user)
                .FirstOrDefault(t => t.Token == refreshToken);

            if (storedToken == null || storedToken.Expires < DateTime.UtcNow)
                return Unauthorized("Refresh token inválido o expirado");

            var newJwt = _tokenService.GenerarToken(storedToken.user);

            // Opcional: renovar refresh token
            storedToken.Token = GenerateRefreshToken();
            storedToken.Expires = DateTime.UtcNow.AddDays(7);

            _context.SaveChanges();

            return Ok(new
            {
                token = newJwt,
                refreshToken = storedToken.Token
            });
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}