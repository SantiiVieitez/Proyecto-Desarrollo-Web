using Microsoft.EntityFrameworkCore;
using Proyecto_Desarrollo_Web.Models;
namespace Proyecto_Desarrollo_Web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
