using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravelDoc.Api.Domain.Viagens.Repositories;
using TravelDoc.Api.Extensions;

namespace TravelDoc.Api.Features.Viagens.Obter
{
    public sealed class ObterProximasViagensEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("viagens/proximas", async ([FromQuery] int usuarioId, IMediator mediator) =>
            {
                var viagens = await mediator.Send(new ObterProximasViagensRequest(usuarioId));
                return Results.Ok(viagens);
            });
        }
    }

    public record ObterProximasViagensRequest(int UsuarioId) : IRequest<IEnumerable<ViagemViewModel>>;

    public class ObterProximasViagensRequestHandler : IRequestHandler<ObterProximasViagensRequest, IEnumerable<ViagemViewModel>>
    {
        private readonly IViagemRepository _viagemRepository;

        public ObterProximasViagensRequestHandler(IViagemRepository viagemRepository)
        {
            _viagemRepository = viagemRepository;
        }

        public async Task<IEnumerable<ViagemViewModel>> Handle(ObterProximasViagensRequest request, CancellationToken cancellationToken)
        {
            return await _viagemRepository.ObterProximasAsync(request.UsuarioId);
        }
    }
}
