using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravelDoc.Api.Domain.Viagens.Repositories;
using TravelDoc.Api.Extensions;

namespace TravelDoc.Api.Features.Viagens.Obter
{
    public sealed class ObterViagensPorUsuarioEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("viagens/usuario/{usuarioId}", async ([FromRoute] int usuarioId, IMediator mediator) =>
            {
                var viagens = await mediator.Send(new ObterViagensPorUsuarioRequest(usuarioId));
                return Results.Ok(viagens);
            });
        }
    }

    public record ObterViagensPorUsuarioRequest(int UsuarioId) : IRequest<IEnumerable<ViagemViewModel>>;

    public class ObterViagensPorUsuarioRequestHandler : IRequestHandler<ObterViagensPorUsuarioRequest, IEnumerable<ViagemViewModel>>
    {
        private readonly IViagemRepository _viagemRepository;

        public ObterViagensPorUsuarioRequestHandler(IViagemRepository viagemRepository)
        {
            _viagemRepository = viagemRepository;
        }

        public async Task<IEnumerable<ViagemViewModel>> Handle(ObterViagensPorUsuarioRequest request, CancellationToken cancellationToken)
        {
            return await _viagemRepository.ObterPorUsuarioAsync(request.UsuarioId);
        }
    }
}