using Microsoft.EntityFrameworkCore;
using Proyecto_Desarrollo_Web.Models;
namespace Proyecto_Desarrollo_Web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Insumo> Insumos { get; set; }

        public DbSet<Privilegio> Privilegios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Insumo>()
                .Property(i => i.Precio)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Privilegios)
                .WithMany()
                .UsingEntity(j => j.ToTable("UsuarioPrivilegios"));
        }
    }
}
