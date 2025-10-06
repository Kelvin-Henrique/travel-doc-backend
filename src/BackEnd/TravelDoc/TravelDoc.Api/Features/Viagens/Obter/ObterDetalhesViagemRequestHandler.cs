using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravelDoc.Api.Domain.Viagens.Repositories;
using TravelDoc.Api.Extensions;

namespace TravelDoc.Api.Features.Viagens.Obter
{
    public sealed class ObterDetalhesViagemEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("viagens/{id}", async ([FromRoute] int id, [FromQuery] int usuarioId, IMediator mediator) =>
            {
                var viagem = await mediator.Send(new ObterDetalhesViagemRequest(id, usuarioId));
                
                if (viagem == null)
                {
                    return Results.NotFound(new { Mensagem = "Viagem não encontrada ou você não tem permissão para visualizá-la." });
                }
                
                return Results.Ok(viagem);
            });
        }
    }

    public record ObterDetalhesViagemRequest(int Id, int UsuarioId) : IRequest<ViagemViewModel?>;

    public class ObterDetalhesViagemRequestHandler : IRequestHandler<ObterDetalhesViagemRequest, ViagemViewModel?>
    {
        private readonly IViagemRepository _viagemRepository;

        public ObterDetalhesViagemRequestHandler(IViagemRepository viagemRepository)
        {
            _viagemRepository = viagemRepository;
        }

        public async Task<ViagemViewModel?> Handle(ObterDetalhesViagemRequest request, CancellationToken cancellationToken)
        {
            return await _viagemRepository.ObterDetalhesAsync(request.Id, request.UsuarioId);
        }
    }
}
