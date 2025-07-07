using Microsoft.EntityFrameworkCore;
using Proyecto_Desarrollo_Web.Models;
namespace Proyecto_Desarrollo_Web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<InsumoInformatico> Insumos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InsumoInformatico>()
                .Property(i => i.Precio)
                .HasColumnType("decimal(18,2)");
        }
    }
}
