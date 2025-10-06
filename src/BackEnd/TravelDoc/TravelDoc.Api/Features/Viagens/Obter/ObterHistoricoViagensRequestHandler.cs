using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravelDoc.Api.Domain.Viagens.Repositories;
using TravelDoc.Api.Extensions;

namespace TravelDoc.Api.Features.Viagens.Obter
{
    public sealed class ObterHistoricoViagensEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("viagens/historico", async ([FromQuery] int usuarioId, IMediator mediator) =>
            {
                var viagens = await mediator.Send(new ObterHistoricoViagensRequest(usuarioId));
                return Results.Ok(viagens);
            });
        }
    }

    public record ObterHistoricoViagensRequest(int UsuarioId) : IRequest<IEnumerable<ViagemViewModel>>;

    public class ObterHistoricoViagensRequestHandler : IRequestHandler<ObterHistoricoViagensRequest, IEnumerable<ViagemViewModel>>
    {
        private readonly IViagemRepository _viagemRepository;

        public ObterHistoricoViagensRequestHandler(IViagemRepository viagemRepository)
        {
            _viagemRepository = viagemRepository;
        }

        public async Task<IEnumerable<ViagemViewModel>> Handle(ObterHistoricoViagensRequest request, CancellationToken cancellationToken)
        {
            return await _viagemRepository.ObterHistoricoAsync(request.UsuarioId);
        }
    }
}
