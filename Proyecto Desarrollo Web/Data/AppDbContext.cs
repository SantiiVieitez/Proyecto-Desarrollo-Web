using Microsoft.EntityFrameworkCore;
using Proyecto_Desarrollo_Web.Models;

namespace Proyecto_Desarrollo_Web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Insumo> Insumos { get; set; }
        public DbSet<Privilegio> Privilegios { get; set; }
        public DbSet<UsuariosPrivilegios> UsuariosPrivilegios { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Decimal para precio
            modelBuilder.Entity<Insumo>()
                .Property(i => i.Precio)
                .HasColumnType("decimal(18,2)");

            // Configuración para la tabla intermedia
            modelBuilder.Entity<UsuariosPrivilegios>()
                .HasKey(up => new { up.UsuarioId, up.PrivilegiosId });

            modelBuilder.Entity<UsuariosPrivilegios>()
                .HasOne(up => up.Usuario)
                .WithMany(u => u.Privilegios)
                .HasForeignKey(up => up.UsuarioId);

            modelBuilder.Entity<UsuariosPrivilegios>()
                .HasOne(up => up.Privilegio)
                .WithMany(p => p.Usuarios)
                .HasForeignKey(up => up.PrivilegiosId);

            modelBuilder.Entity<UsuariosPrivilegios>()
                .ToTable("UsuariosPrivilegios");
        }
    }
}