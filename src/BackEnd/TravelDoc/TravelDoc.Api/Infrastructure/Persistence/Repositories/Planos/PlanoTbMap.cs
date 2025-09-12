using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TravelDoc.Application.Usuarios.Domain;
using TravelDoc.Api.Domain.Planos.Entities;
using Hashcoop.Repository.Extensions;

namespace TravelDoc.Api.Infrastructure.Persistence.Repositories.Planos
{
    internal class PlanoTbMap : IEntityTypeConfiguration<Plano>
    {
        private string NOME_TABELA = $"{nameof(Plano)}Tb".ToSnakeCase();

        public void Configure(EntityTypeBuilder<Plano> builder)
        {
            builder.ToTable(NOME_TABELA);

            builder.HasKey(p => p.Id)
                .HasName($"pk_{NOME_TABELA}");

            builder.Property(p => p.Id)
                .UseIdentityColumn();

            builder.Property(i => i.Nome)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(i => i.Descricao)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(i => i.Valor)
                .IsRequired();

            builder.Property(i => i.Icone)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(i => i.Ativo)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property<DateTime>("DataInclusao")
                .IsRequired();

            builder.Property<DateTime?>("DataAlteracao");

            builder.Ignore(x => x.Events);
        }
    }
}
