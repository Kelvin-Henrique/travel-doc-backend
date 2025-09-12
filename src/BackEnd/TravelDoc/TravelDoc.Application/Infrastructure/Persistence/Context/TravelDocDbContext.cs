using Microsoft.EntityFrameworkCore;
using TravelDoc.Application.Usuarios.Domain;

namespace TravelDoc.Repository.Contexts
{
    public class TravelDocDbContext : DbContext
    {
        public TravelDocDbContext(DbContextOptions<TravelDocDbContext> opts) : base(opts) { }

        public DbSet<Usuario> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ItemMap());

            // Se quiser aplicar todos os mapeamentos automaticamente:
            // modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
