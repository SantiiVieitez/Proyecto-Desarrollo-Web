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
            //string passwordHash = CriptographyService.GetSHA256(request.Password);

            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Name == request.User &&
                                     u.ClaveHash == request.Password);

            if (usuario == null || !usuario.Activo)
                return Unauthorized("Credenciales inválidas");

            var token = _tokenService.GenerarToken(usuario);
            return Ok(new { token });
        }
    }
}
