using TravelDoc.Api.Domain.Planos.Entities;
using TravelDoc.Application.Usuarios.Domain;

namespace TravelDoc.Api.Domain.Planos.Repositories
{
    public interface IPlanoRepository
    {
        ValueTask<Plano> ObterAsync(int id);
        ValueTask InserirAsync(Plano plano);
        //ValueTask AtualizarAsync(Plano plano);
        ValueTask ExcluirAsync(int id);
        ValueTask<bool> ExisteAsync(int id);
    }
}
