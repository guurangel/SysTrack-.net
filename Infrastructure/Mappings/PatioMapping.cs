using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SysTrack.Infrastructure.Persistence;
using System;

namespace SysTrack.Infrastructure.Mappings
{
    public class PatioMapping : IEntityTypeConfiguration<Patio>
    {
        public void Configure(EntityTypeBuilder<Patio> builder)
        {
            builder.ToTable("Patios");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Endereco)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(p => p.DataCriacao)
                .IsRequired();

            builder.HasMany(p => p.Motocicletas)
                .WithOne(m => m.Patio)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
