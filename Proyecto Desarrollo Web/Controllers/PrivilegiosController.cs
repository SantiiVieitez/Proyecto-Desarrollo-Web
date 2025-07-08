using Microsoft.AspNetCore.Mvc;
using Proyecto_Desarrollo_Web.Data;
using Proyecto_Desarrollo_Web.Models;

namespace Proyecto_Desarrollo_Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrivilegiosControler : ControllerBase
    {
        private readonly AppDbContext _context;

        public PrivilegiosControler(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetPrivilegios()
        {
            var privilegios = _context.Privilegios.ToList();
            return Ok(privilegios);
        }

        [HttpPost]
        public IActionResult CreatePrivilegio([FromBody] Privilegio privilegio)
        {
            _context.Privilegios.Add(privilegio);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetPrivilegios), new { id = privilegio.Id }, privilegio);
        }

        [HttpPut("{id}")]
        public IActionResult ActualizarPrivilegio(int id, [FromBody] Privilegio privilegio)
        {
            var privilegioExistente = _context.Privilegios.FirstOrDefault(p => p.Id == id);

            if (privilegioExistente == null)
                return NotFound("Privilegio no encontrado");

            privilegioExistente.Descripcion = privilegio.Descripcion;

            _context.SaveChanges();

            return Ok("Privilegio actualizado correctamente");
        }
    }
}