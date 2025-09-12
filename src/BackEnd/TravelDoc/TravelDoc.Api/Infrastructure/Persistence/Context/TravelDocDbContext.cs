using Hashcoop.Repository.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TravelDoc.Api.Domain.Planos.Entities;
using TravelDoc.Api.Infrastructure.Persistence.Repositories.Planos;
using TravelDoc.Application.Usuarios.Domain;
using TravelDoc.Repository.Usuarios.Mappings;

namespace TravelDoc.Repository.Contexts
{
    public class TravelDocDbContext : DbContext
    {
        public TravelDocDbContext(DbContextOptions<TravelDocDbContext> opts) : base(opts) { }

        public DbSet<Usuario> UsuarioTb { get; set; }
        public DbSet<Plano> PlanoTb { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioTbMap());
            modelBuilder.ApplyConfiguration(new PlanoTbMap());

            base.OnModelCreating(modelBuilder);

            ConfigureConventions(modelBuilder);
        }

        protected static new Action<ModelBuilder> ConfigureConventions => modelBuilder =>
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                if (string.IsNullOrEmpty(entity.GetViewName()))
                {
                    // Replace table names
                    entity.SetTableName(entity.GetTableName()?.ToSnakeCase());
                }

                // Replace column names            
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToSnakeCase());
                }

                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName()?.ToSnakeCase());
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(key.GetConstraintName()?.ToSnakeCase());
                }

                foreach (var key in entity.GetIndexes())
                {
                    key.SetDatabaseName(key?.Name?.ToSnakeCase());
                }

                foreach (var index in entity.GetProperties().Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
                {
                    index.SetColumnType("numeric(18,2)");
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.DeleteBehavior = DeleteBehavior.Restrict;
                }
            }

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().Where(e => e.IsOwned()).SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Cascade;
            }
        };
    }
}
