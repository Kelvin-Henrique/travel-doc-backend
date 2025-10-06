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

        public async ValueTask<ViagemViewModel?> ObterDetalhesAsync(int id, int usuarioId)
        {
            return await _context.ViagemTb
                .AsNoTracking()
                .Where(x => x.Id == id && x.CriadorId == usuarioId)
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
                .FirstOrDefaultAsync();
        }

        public async ValueTask<IEnumerable<ViagemViewModel>> ObterProximasAsync(int usuarioId)
        {
            var dataAtual = DateTime.UtcNow.Date;
            return await _context.ViagemTb
                .AsNoTracking()
                .Where(x => x.CriadorId == usuarioId && x.DataInicio.Date >= dataAtual)
                .OrderBy(x => x.DataInicio)
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

        public async ValueTask<IEnumerable<ViagemViewModel>> ObterHistoricoAsync(int usuarioId)
        {
            var dataAtual = DateTime.UtcNow.Date;
            return await _context.ViagemTb
                .AsNoTracking()
                .Where(x => x.CriadorId == usuarioId && x.DataFim.Date < dataAtual)
                .OrderByDescending(x => x.DataFim)
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

        public async ValueTask<IEnumerable<ViagemViewModel>> BuscarAsync(int usuarioId, string? nome, string? destino, DateTime? dataInicio, DateTime? dataFim)
        {
            var query = _context.ViagemTb
                .AsNoTracking()
                .Where(x => x.CriadorId == usuarioId);

            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(x => x.NomeViagem.ToLower().Contains(nome.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(destino))
            {
                query = query.Where(x => x.Destino.ToLower().Contains(destino.ToLower()));
            }

            if (dataInicio.HasValue)
            {
                query = query.Where(x => x.DataInicio.Date >= dataInicio.Value.Date);
            }

            if (dataFim.HasValue)
            {
                query = query.Where(x => x.DataFim.Date <= dataFim.Value.Date);
            }

            return await query
                .OrderBy(x => x.DataInicio)
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
    }
}
