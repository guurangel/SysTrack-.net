using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using SysTrack.Infrastructure.Mappings;
using SysTrack.Infrastructure.Persistence;

namespace SysTrack.Infrastructure.Contexts
{
    public class PatioDbContext : DbContext
    {
        public PatioDbContext(DbContextOptions<PatioDbContext> options)
        : base(options)
        { }

        public DbSet<Patio> Patios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PatioMapping());
        }
    }
}