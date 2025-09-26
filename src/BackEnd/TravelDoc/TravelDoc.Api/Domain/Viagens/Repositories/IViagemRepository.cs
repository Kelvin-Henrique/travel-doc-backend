using TravelDoc.Api.Domain.Planos.Entities;
using TravelDoc.Api.Domain.Viagens.Entities;

namespace TravelDoc.Api.Domain.Viagens.Repositories
{
    public interface IViagemRepository
    {
        ValueTask<Plano> ObterAsync(int id);
        ValueTask InserirAsync(Viagem viagem);
        //ValueTask AtualizarAsync(Plano plano);
        ValueTask ExcluirAsync(int id);
        ValueTask<bool> ExisteAsync(int id);
    }
}
