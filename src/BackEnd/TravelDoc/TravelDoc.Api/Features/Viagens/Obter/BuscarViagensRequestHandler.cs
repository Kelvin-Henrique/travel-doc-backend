using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravelDoc.Api.Domain.Viagens.Repositories;
using TravelDoc.Api.Extensions;

namespace TravelDoc.Api.Features.Viagens.Obter
{
    public sealed class BuscarViagensEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("viagens", async ([FromQuery] int usuarioId, 
                                         [FromQuery] string? nome, 
                                         [FromQuery] string? destino, 
                                         [FromQuery] DateTime? dataInicio, 
                                         [FromQuery] DateTime? dataFim, 
                                         IMediator mediator) =>
            {
                var viagens = await mediator.Send(new BuscarViagensRequest(usuarioId, nome, destino, dataInicio, dataFim));
                return Results.Ok(viagens);
            });
        }
    }

    public record BuscarViagensRequest(int UsuarioId, string? Nome, string? Destino, DateTime? DataInicio, DateTime? DataFim) : IRequest<IEnumerable<ViagemViewModel>>;

    public class BuscarViagensRequestHandler : IRequestHandler<BuscarViagensRequest, IEnumerable<ViagemViewModel>>
    {
        private readonly IViagemRepository _viagemRepository;

        public BuscarViagensRequestHandler(IViagemRepository viagemRepository)
        {
            _viagemRepository = viagemRepository;
        }

        public async Task<IEnumerable<ViagemViewModel>> Handle(BuscarViagensRequest request, CancellationToken cancellationToken)
        {
            return await _viagemRepository.BuscarAsync(request.UsuarioId, request.Nome, request.Destino, request.DataInicio, request.DataFim);
        }
    }
}
