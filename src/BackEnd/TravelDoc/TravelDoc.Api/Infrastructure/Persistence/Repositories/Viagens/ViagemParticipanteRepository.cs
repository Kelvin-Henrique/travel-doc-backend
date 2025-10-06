using Microsoft.EntityFrameworkCore;
using TravelDoc.Api.Domain.Viagens.Entities;
using TravelDoc.Api.Domain.Viagens.Repositories;
using TravelDoc.Repository.Contexts;

namespace TravelDoc.Api.Infrastructure.Persistence.Repositories.Viagens
{
    public class ViagemParticipanteRepository : IViagemParticipanteRepository
    {
        private readonly TravelDocDbContext _context;

        public ViagemParticipanteRepository(TravelDocDbContext context)
        {
            _context = context;
        }

        public async ValueTask InserirAsync(ViagemParticipante obj)
        {
            await _context.ViagemParticipanteTb.AddAsync(obj);
        }

        public async ValueTask ExcluirAsync(int id)
        {
            var entity = await _context.ViagemParticipanteTb.FindAsync(id);
            if (entity is not null)
            {
                _context.ViagemParticipanteTb.Remove(entity);
            }
        }

        public async ValueTask<bool> ExisteAsync(int id)
        {
            return await _context.ViagemParticipanteTb.AnyAsync(x => x.Id == id);
        }

        public async ValueTask<ViagemParticipante> ObterAsync(int id)
        {
            return await _context.ViagemParticipanteTb.FindAsync(id);
        }

        public async ValueTask<IEnumerable<ViagemParticipante>> ObterPorViagemAsync(int viagemId)
        {
            return await _context.ViagemParticipanteTb
                .AsNoTracking()
                .Where(x => x.ViagemId == viagemId)
                .ToListAsync();
        }

        public async ValueTask<IEnumerable<ViagemParticipante>> ObterPorParticipanteAsync(int participanteId)
        {
            return await _context.ViagemParticipanteTb
                .AsNoTracking()
                .Where(x => x.ParticipanteId == participanteId)
                .ToListAsync();
        }

        public async ValueTask<ViagemParticipante?> ObterPorViagemEParticipanteAsync(int viagemId, int participanteId)
        {
            return await _context.ViagemParticipanteTb
                .AsNoTracking()
                .Where(x => x.ViagemId == viagemId && x.ParticipanteId == participanteId)
                .FirstOrDefaultAsync();
        }
    }
}