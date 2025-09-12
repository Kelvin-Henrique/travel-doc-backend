using TravelDoc.Api.Domain.Planos.Entities;
using TravelDoc.Application.Usuarios.Domain;

namespace TravelDoc.Api.Domain.Planos.Repositories
{
    public interface IPlanoRepository
    {
        ValueTask InserirAsync(Plano plano);
    }
}
