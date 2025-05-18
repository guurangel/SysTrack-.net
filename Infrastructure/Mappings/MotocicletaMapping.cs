using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SysTrack.Infrastructure.Persistence;

namespace SysTrack.Infrastructure.Mappings
{
    public class MotocicletaMapping : IEntityTypeConfiguration<Motocicleta>
    {
        public void Configure(EntityTypeBuilder<Motocicleta> builder)
        {
            builder.ToTable("Motocicletas");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Placa)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(m => m.Marca)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(m => m.Modelo)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(m => m.Cor)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(m => m.DataEntrada)
                .IsRequired();

            builder.HasOne(m => m.Patio)
                .WithMany(p => p.Motocicletas)
                .HasForeignKey(m => m.PatioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}