using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using SysTrack.Infrastructure.Mappings;
using SysTrack.Infrastructure.Persistence;

namespace SysTrack.Infrastructure.Contexts
{
    public class MotocicletaDbContext : DbContext
    {
        public MotocicletaDbContext(DbContextOptions<MotocicletaDbContext> options)
        : base(options)
        { }

        public DbSet<Motocicleta> Motocicletas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MotocicletaMapping());
        }
    }
}