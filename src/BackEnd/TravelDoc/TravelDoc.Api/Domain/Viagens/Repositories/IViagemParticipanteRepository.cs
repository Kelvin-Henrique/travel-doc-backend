using TravelDoc.Api.Domain.Viagens.Entities;
using TravelDoc.Api.Features.Viagens.Obter;

namespace TravelDoc.Api.Domain.Viagens.Repositories
{
    public interface IViagemParticipanteRepository
    {
        ValueTask<ViagemParticipante> ObterAsync(int id);
        ValueTask InserirAsync(ViagemParticipante viagemParticiopante);
        ValueTask ExcluirAsync(int id);
        ValueTask<bool> ExisteAsync(int id);
        ValueTask<IEnumerable<ViagemParticipante>> ObterPorViagemAsync(int viagemId);
        ValueTask<IEnumerable<ViagemParticipante>> ObterPorParticipanteAsync(int participanteId);
        ValueTask<ViagemParticipante?> ObterPorViagemEParticipanteAsync(int viagemId, int participanteId);
    }
}
