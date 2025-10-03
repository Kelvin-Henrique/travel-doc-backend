using TravelDoc.Api.Domain.Planos.Entities;
using TravelDoc.Api.Domain.Viagens.Entities;
using TravelDoc.Api.Features.Viagens.Obter;

namespace TravelDoc.Api.Domain.Viagens.Repositories
{
    public interface IViagemRepository
    {
        ValueTask<Viagem> ObterAsync(int id);
        ValueTask InserirAsync(Viagem viagem);
        ValueTask ExcluirAsync(int id);
        ValueTask<bool> ExisteAsync(int id);
        ValueTask<IEnumerable<ViagemViewModel>> ObterPorUsuarioAsync(int usuarioId);
    }
}
