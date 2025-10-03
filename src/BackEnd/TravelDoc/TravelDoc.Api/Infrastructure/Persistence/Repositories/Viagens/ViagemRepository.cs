using Microsoft.EntityFrameworkCore;
using TravelDoc.Api.Domain.Planos.Entities;
using TravelDoc.Api.Domain.Viagens.Entities;
using TravelDoc.Api.Domain.Viagens.Repositories;
using TravelDoc.Api.Features.Viagens.Obter;
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
            _context.ViagemTb.Remove(_context.ViagemTb.Find(id));

            await Task.CompletedTask;
        }

        public async ValueTask<bool> ExisteAsync(int id)
        {
            return await _context.ViagemTb.AnyAsync(x => x.Id == id);
        }

        public async ValueTask InserirAsync(Viagem obj)
        {
            await _context.ViagemTb.AddAsync(obj);

        }
        public async ValueTask<IEnumerable<ViagemViewModel>> ObterPorUsuarioAsync(int usuarioId)
        {
            return await _context.ViagemTb
                .AsNoTracking()
                .Where(x => x.CriadorId == usuarioId)
                .Select(x => new ViagemViewModel
                {
                    Id = x.Id,
                    NomeViagem = x.NomeViagem,
                    Destino = x.Destino,
                    DataInicio = x.DataInicio.Date,
                    DataFim = x.DataFim.Date,
                    Descricao = x.Descricao,
                    CriadorId = x.CriadorId,
                    Status = (int)x.Status
                })
                .ToListAsync();
        }


        public async ValueTask<Viagem> ObterAsync(int id)
        {
            return await _context.ViagemTb.FindAsync(id);
        }
    }
}
