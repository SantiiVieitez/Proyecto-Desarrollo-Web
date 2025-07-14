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
            var usuarios = _context.Usuarios
                .Include(u => u.Privilegios)
                .ToList()
                .Select(usuario => new
                {
                    usuario.Id,
                    usuario.Name,
                    usuario.Activo,
                    Privilegios = usuario.Privilegios.Select(p => p.PrivilegiosId).ToList()
                });

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

            var usuarioDTO = new
            {
                usuario.Id,
                usuario.Name,
                usuario.Activo,
                Privilegios = usuario.Privilegios.Select(p => p.PrivilegiosId).ToList()
            };

            return Ok(usuarioDTO);
        }


        [HttpPost]
        public IActionResult CrearUsuario([FromBody] Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Name) || usuario.PrivilegiosIds == null || usuario.PrivilegiosIds.Count == 0)
            {
                return BadRequest("Nombre de usuario y privilegios son obligatorios.");
            }

            usuario.Salt = CriptographyService.GenerarSalt();
            usuario.ClaveHash = CriptographyService.GetSHA256(usuario.ClaveHash + usuario.Salt);

            // Recuperar IDs de privilegios enviados
            var privilegiosIds = usuario.PrivilegiosIds;

            // Validar que existan en la DB
            var privilegios = _context.Privilegios
                .Where(p => privilegiosIds.Contains(p.Id))
                .ToList();

            if (privilegios.Count != privilegiosIds.Count)
            {
                return BadRequest("Uno o más privilegios no son válidos.");
            }

            // Crear la relación Usuario-Privilegios
            usuario.Privilegios = privilegios
                .Select(p => new UsuariosPrivilegios
                {
                    PrivilegiosId = p.Id
                }).ToList();

            _context.Usuarios.Add(usuario);

            try
            {
                _context.SaveChanges();

                // Aquí devolvemos solo los datos que queremos
                var usuarioDTO = new
                {
                    usuario.Id,
                    usuario.Name,
                    usuario.Activo,
                    Privilegios = usuario.Privilegios.Select(p => p.PrivilegiosId).ToList()
                };

                return CreatedAtAction(nameof(GetUsuarios), new { id = usuario.Id }, usuarioDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult EditarUsuario(int id, [FromBody] Usuario usuarioActualizado)
        {
            if (id != usuarioActualizado.Id)
                return BadRequest("El ID del usuario no coincide.");

            var usuarioExistente = _context.Usuarios
                .Include(u => u.Privilegios)
                .FirstOrDefault(u => u.Id == id);

            if (usuarioExistente == null)
                return NotFound("Usuario no encontrado.");

            usuarioExistente.Name = usuarioActualizado.Name;
            usuarioExistente.Activo = usuarioActualizado.Activo;

            if (!string.IsNullOrWhiteSpace(usuarioActualizado.ClaveHash))
            {
                usuarioExistente.ClaveHash =
                    CriptographyService.GetSHA256(usuarioActualizado.ClaveHash + usuarioExistente.Salt);
            }

            usuarioExistente.Privilegios.Clear();

            var privilegiosIds = usuarioActualizado.PrivilegiosIds;

            var privilegios = _context.Privilegios
                .Where(p => privilegiosIds.Contains(p.Id))
                .ToList();

            usuarioExistente.Privilegios = privilegios
                .Select(p => new UsuariosPrivilegios
                {
                    UsuarioId = usuarioExistente.Id,
                    PrivilegiosId = p.Id
                }).ToList();

            try
            {
                _context.SaveChanges();

                var usuarioDTO = new
                {
                    usuarioExistente.Id,
                    usuarioExistente.Name,
                    usuarioExistente.Activo,
                    Privilegios = usuarioExistente.Privilegios.Select(p => p.PrivilegiosId).ToList()
                };

                return Ok(usuarioDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el usuario: {ex.Message}");
            }
        }

    }
}
