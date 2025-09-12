using Microsoft.EntityFrameworkCore;
using TravelDoc.Api.Domain.Planos.Entities;
using TravelDoc.Api.Domain.Planos.Repositories;
using TravelDoc.Application.Usuarios.Domain;
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

        public async ValueTask ExcluirAsync(int id)
        {
            _context.PlanoTb.Remove(_context.PlanoTb.Find(id));

            await Task.CompletedTask;
        }

        public async ValueTask<bool> ExisteAsync(int id)
        {
            return await _context.PlanoTb.AnyAsync(x => x.Id == id);
        }

        public async ValueTask InserirAsync(Plano obj)
        {
            await _context.PlanoTb.AddAsync(obj);

            _context.Entry(obj).Property("DataInclusao").CurrentValue = DateTime.Now.ToUniversalTime();
        }

        public async ValueTask<Plano> ObterAsync(int id)
        {
            return await _context.PlanoTb.FindAsync(id);
        }
    }
}
