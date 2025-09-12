using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TravelDoc.Application.Usuarios.Domain;

namespace TravelDoc.Repository.Usuarios.Mappings
{
    internal class UsuarioTbMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Items");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(i => i.Email)
                .HasMaxLength(255);
        }
    }
}
