using Microsoft.AspNetCore.Mvc;
using Proyecto_Desarrollo_Web.Data;
using Proyecto_Desarrollo_Web.Models;
using Proyecto_Desarrollo_Web.Services;
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

        [HttpPost("login")]
        public IActionResult Login([FromBody] string user, string password)
        {
            string passwordHash = CriptographyService.GetSHA256(password);

            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Name == user &&
                                     u.ClaveHash == passwordHash);

            if (usuario == null || !usuario.Activo)
                return Unauthorized("Credenciales inválidas");

            var token = _tokenService.GenerarToken(usuario);
            return Ok(new { token });
        }
    }
}
