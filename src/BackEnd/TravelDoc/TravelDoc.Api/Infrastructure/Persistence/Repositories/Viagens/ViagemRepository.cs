using Microsoft.EntityFrameworkCore;
using TravelDoc.Api.Domain.Planos.Entities;
using TravelDoc.Api.Domain.Viagens.Entities;
using TravelDoc.Api.Domain.Viagens.Repositories;
using TravelDoc.Repository.Contexts;

namespace TravelDoc.Api.Infrastructure.Persistence.Repositories.Viagens
{
    public class ViagemRepository : IViagemRepository
    {
        private readonly TravelDocDbContext _context;

        public ViagemRepository(TravelDocDbContext context)
        {
            _context = context;
        }

        public async ValueTask ExcluirAsync(int id)
        {
            _context.PlanoTb.Remove(_context.PlanoTb.Find(id));

            await Task.CompletedTask;
        }

        public async ValueTask<bool> ExisteAsync(int id)
        {
            return await _context.PlanoTb.AnyAsync(x => x.Id == id);
        }

        public async ValueTask InserirAsync(Viagem obj)
        {
            await _context.ViagemTb.AddAsync(obj);

            _context.Entry(obj).Property("DataInclusao").CurrentValue = DateTime.Now.ToUniversalTime();
        }

        public async ValueTask<Plano> ObterAsync(int id)
        {
            return await _context.PlanoTb.FindAsync(id);
        }
    }
}
