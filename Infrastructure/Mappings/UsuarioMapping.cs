using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SysTrack.Infrastructure.Persistance;
using SysTrack.Infrastructure.Persistence;

namespace SysTrack.Infrastructure.Mappings
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(u => u.Id);

            builder
                .HasIndex(u => u.Email).IsUnique();
            builder
                .HasIndex(u => u.Cpf).IsUnique();

            builder.Property(u => u.Nome)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(u => u.Email)
                   .HasMaxLength(150)
                   .IsRequired();

            builder.Property(u => u.Senha)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(u => u.Cpf)
                   .HasMaxLength(11)
                   .IsRequired()
                   .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Property(u => u.Cargo)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(u => u.DataAdmissao)
                   .HasColumnType("DATE")
                   .IsRequired();

            builder.HasOne(u => u.Patio)
                   .WithMany(u => u.Usuarios)
                   .HasForeignKey(u => u.PatioId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}