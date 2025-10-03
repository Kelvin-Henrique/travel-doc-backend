using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelDoc.Api.Domain.Viagens.Entities;
using Hashcoop.Repository.Extensions;
using TravelDoc.Application.Usuarios.Domain;

namespace TravelDoc.Api.Infrastructure.Persistence.Repositories.Viagens
{
    internal class ViagemParticipanteTbMap : IEntityTypeConfiguration<ViagemParticipante>
    {
        private string NOME_TABELA = $"{nameof(ViagemParticipante)}Tb".ToSnakeCase();

        public void Configure(EntityTypeBuilder<ViagemParticipante> builder)
        {
            builder.ToTable(NOME_TABELA);

            builder.HasKey(p => p.Id)
                .HasName($"pk_{NOME_TABELA}");

            builder.Property(p => p.Id)
                .UseIdentityColumn();

            builder.Property(p => p.ViagemId)
                .IsRequired();

            builder.Property(p => p.ParticipanteId)
                .IsRequired();

            builder.Property(p => p.Status)
                .IsRequired();

            builder.Property<DateTime?>("DataInclusao");

            builder.Property<DateTime?>("DataAlteracao");

            builder.Ignore(x => x.Events);

            builder.HasOne<Viagem>()
                .WithMany()
                .HasForeignKey(p => p.ViagemId)
                .HasConstraintName($"fk_{NOME_TABELA}_viagem_tb");

             builder.HasOne<Usuario>()
                .WithMany()
                 .HasForeignKey(p => p.ParticipanteId)
                 .HasConstraintName($"fk_{NOME_TABELA}_usuario_tb");
        }
    }
}