using TravelDoc.Api.Domain.Planos.Entities;
using TravelDoc.Api.Domain.Planos.Repositories;
using TravelDoc.Repository.Contexts;

namespace TravelDoc.Api.Infrastructure.Persistence.Repositories.Planos
{
    public class PlanoRepository : IPlanoRepository
    {
        private readonly TravelDocDbContext _context;

        public PlanoRepository(TravelDocDbContext context)
        {
            _context = context;
        }

        public async ValueTask InserirAsync(Plano obj)
        {
            await _context.PlanoTb.AddAsync(obj);

            _context.Entry(obj).Property("DataInclusao").CurrentValue = DateTime.Now.ToUniversalTime();
        }
    }
}
