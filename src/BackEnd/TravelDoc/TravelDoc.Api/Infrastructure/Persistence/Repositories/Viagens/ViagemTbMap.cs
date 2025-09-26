using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TravelDoc.Api.Domain.Viagens.Entities;
using Hashcoop.Repository.Extensions;

namespace TravelDoc.Api.Infrastructure.Persistence.Repositories.Viagens
{
    internal class ViagemTbMap : IEntityTypeConfiguration<Viagem>
    {
        private string NOME_TABELA = $"{nameof(Viagem)}Tb".ToSnakeCase();

        public void Configure(EntityTypeBuilder<Viagem> builder)
        {
            builder.ToTable(NOME_TABELA);

            builder.HasKey(p => p.Id)
                .HasName($"pk_{NOME_TABELA}");

            builder.Property(p => p.Id)
                .UseIdentityColumn();

            builder.Property(i => i.NomeViagem)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(i => i.Destino)
                .IsRequired();

            builder.Property(i => i.DataInicio)
                .IsRequired();

            builder.Property(i => i.DataFim)
                .IsRequired();

            builder.Property(i => i.Descricao)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(i => i.CriadorId)
                .IsRequired();

            builder.Property(i => i.Status)
                .IsRequired();

            builder.Property<DateTime>("DataInclusao")
                .IsRequired();

            builder.Property<DateTime?>("DataAlteracao");

            builder.Ignore(x => x.Events);

            builder.HasOne(v => v.Criador)
            .WithMany()
            .HasForeignKey(v => v.CriadorId)
            .HasConstraintName($"fk_{NOME_TABELA}_usuario_tb");

        }
    }
}
