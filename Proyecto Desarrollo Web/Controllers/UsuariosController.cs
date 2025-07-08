using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Desarrollo_Web.Data;
using Proyecto_Desarrollo_Web.Models;
using Proyecto_Desarrollo_Web.Services;

namespace Proyecto_Desarrollo_Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetUsuarios()
        {
            var usuarios = _context.Usuarios.ToList();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public IActionResult GetUsuario(int id)
        {
            var usuario = _context.Usuarios
                .Include(u => u.Privilegios)
                .FirstOrDefault(u => u.Id == id);

            if (usuario == null)
                return NotFound("Usuario no encontrado");

            return Ok(usuario);
        }

        [HttpPost]
        public IActionResult CrearUsuario([FromBody] Usuario usuario)
        {
            usuario.Salt = CriptographyService.GenerarSalt();
            usuario.ClaveHash = CriptographyService.GetSHA256(usuario.ClaveHash + usuario.Salt);

            // Obtener IDs de los privilegios que vinieron
            var privilegioIds = usuario.Privilegios.Select(p => p.Id).ToList();

            // Buscar privilegios reales desde la base
            var privilegiosExistentes = _context.Privilegios
                .Where(p => privilegioIds.Contains(p.Id))
                .ToList();

            // Asignarlos al usuario
            usuario.Privilegios = privilegiosExistentes;

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUsuarios), new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id}")]
        public IActionResult EditarUsuario(int id, [FromBody] Usuario usuarioActualizado)
        {
            if (id != usuarioActualizado.Id)
            {
                return BadRequest("El ID del usuario no coincide.");
            }

            var usuarioExistente = _context.Usuarios
                .Include(u => u.Privilegios) // ⬅️ Necesario para que EF pueda gestionar la relación
                .FirstOrDefault(u => u.Id == id);

            if (usuarioExistente == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Actualizar propiedades básicas
            usuarioExistente.Name = usuarioActualizado.Name;
            usuarioExistente.Activo = usuarioActualizado.Activo;
            usuarioExistente.ClaveHash = CriptographyService.GetSHA256(usuarioActualizado.ClaveHash + usuarioExistente.Salt);

            // 🔄 Actualizar privilegios (reemplazar completamente)
            usuarioExistente.Privilegios.Clear();

            var nuevosPrivilegiosIds = usuarioActualizado.Privilegios.Select(p => p.Id).ToList();

            var privilegiosActualizados = _context.Privilegios
                .Where(p => nuevosPrivilegiosIds.Contains(p.Id))
                .ToList();

            usuarioExistente.Privilegios = privilegiosActualizados;

            try
            {
                _context.SaveChanges();
                return Ok("Usuario actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el usuario: {ex.Message}");
            }
        }
    }
}
