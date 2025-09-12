using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TravelDoc.Application.Usuarios.Domain;
using Hashcoop.Repository.Extensions;

namespace TravelDoc.Repository.Usuarios.Mappings
{
    internal class UsuarioTbMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable($"{nameof(Usuario)}Tb".ToSnakeCase());

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(i => i.Email)
                .HasMaxLength(255);
        }
    }
}
