using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Desarrollo_Web.Data;
using Proyecto_Desarrollo_Web.Models;

namespace Proyecto_Desarrollo_Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InsumosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InsumosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetInsumos()
        {
            var insumos = _context.Insumos.ToList();
            return Ok(insumos);
        }

        [HttpGet("{id}")]
        public IActionResult GetInsumoById(int id)
        {
            var insumo = _context.Insumos.FirstOrDefault(i => i.Id == id);

            if (insumo == null)
                return NotFound("Insumo no encontrado");

            return Ok(insumo);
        }

        [Authorize]
        [HttpPost]
        public IActionResult CrearInsumo([FromBody] Insumo insumo)
        {
            var username = User.Identity?.Name;

            if (string.IsNullOrEmpty(username))
                return Unauthorized("No se pudo identificar al usuario");

            var usuario = _context.Usuarios
                .Include(u => u.Privilegios)
                    .ThenInclude(up => up.Privilegio)
                .FirstOrDefault(u => u.Name == username);

            if (usuario == null)
                return Unauthorized("Usuario no encontrado");

            bool tienePermiso = usuario.Privilegios.Any(p => p.Privilegio.Descripcion == "CrearInsumo");

            if (!tienePermiso)
                return Forbid("No tienes permiso para crear insumos");

            _context.Insumos.Add(insumo);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetInsumos), new { id = insumo.Id }, insumo);
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult ModificarInsumo(int id, [FromBody] Insumo insumo)
        {
            var insumoExistente = _context.Insumos.FirstOrDefault(i => i.Id == id);

            if (insumoExistente == null)
                return NotFound("Insumo no encontrado");

            insumoExistente.Nombre = insumo.Nombre;
            insumoExistente.Descripcion = insumo.Descripcion;
            insumoExistente.Marca = insumo.Marca;
            insumoExistente.Stock = insumo.Stock;
            insumoExistente.Precio = insumo.Precio;
            insumoExistente.Codigo = insumo.Codigo;

            _context.SaveChanges();

            return Ok("Insumo actualizado correctamente");
        }
    }
}