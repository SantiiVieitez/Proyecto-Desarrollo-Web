using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Desarrollo_Web.Data;
using Proyecto_Desarrollo_Web.Models;
using Proyecto_Desarrollo_Web.Services;

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
            var insumo = _context.Insumos
                .FirstOrDefault(i => i.Id == id);

            if (insumo == null)
                return NotFound("Insumo no encontrado");

            return Ok(insumo);
        }

        [HttpPost]
        public IActionResult CrearInsumo([FromBody] Insumo insumo)
        {
            _context.Insumos.Add(insumo);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetInsumos), new { id = insumo.Id }, insumo);
        }

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
