using Microsoft.EntityFrameworkCore;
using SysTrack.Infrastructure.Mappings;
using SysTrack.Infrastructure.Persistance;
using SysTrack.Infrastructure.Persistence;

namespace SysTrack.Infrastructure.Contexts
{
    public class SysTrackDbContext : DbContext
    {
        public SysTrackDbContext(DbContextOptions<SysTrackDbContext> options)
            : base(options)
        { }

        public DbSet<Motocicleta> Motocicletas { get; set; }
        public DbSet<Patio> Patios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MotocicletaMapping());
            modelBuilder.ApplyConfiguration(new PatioMapping());
            modelBuilder.ApplyConfiguration(new UsuarioMapping());
        }
    }
}