using FluentValidation;
using MediatR;
using TravelDoc.Api.Domain.Viagens.Entities;
using TravelDoc.Api.Domain.Viagens.Repositories;
using TravelDoc.Api.Extensions;
using TravelDoc.Infrastructure.Core.Results;
using TravelDoc.Infrastructure.Persistence.Context;

namespace TravelDoc.Api.Features.Viagens.Participantes.Inserir
{
    public sealed class InserirViagemParticipanteEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("viagens/participantes", async (InserirViagemParticipanteRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);

                return result.IsFailure
                    ? Results.BadRequest(new { Mensagem = result.Error })
                    : Results.Ok();
            });
        }
    }

    public record InserirViagemParticipanteRequest : IRequest<Result>
    {
        public required int ViagemId { get; set; }
        public required int ParticipanteId { get; set; }
        public required StatusViagemParticipanteEnum Status { get; set; }
    }

    public class InserirViagemParticipanteRequestHandler : IRequestHandler<InserirViagemParticipanteRequest, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IViagemParticipanteRepository _viagemParticipanteRepository;

        public InserirViagemParticipanteRequestHandler(IUnitOfWork unitOfWork, IViagemParticipanteRepository viagemParticipanteRepository)
        {
            _unitOfWork = unitOfWork;
            _viagemParticipanteRepository = viagemParticipanteRepository;
        }

        public async Task<Result> Handle(InserirViagemParticipanteRequest request, CancellationToken cancellationToken)
        {
            var participanteResult = ViagemParticipante.Criar(request.ViagemId, request.ParticipanteId, request.Status);
            if (participanteResult.IsFailure)
            {
                return Result.Failure(participanteResult.Error);
            }

            await _viagemParticipanteRepository.InserirAsync(participanteResult.Value);
            await _unitOfWork.CommitAsync();

            return Result.Success();
        }
    }

    public class InserirViagemParticipanteRequestValidator : AbstractValidator<InserirViagemParticipanteRequest>
    {
        public InserirViagemParticipanteRequestValidator()
        {
            RuleFor(x => x.ViagemId)
                .GreaterThan(0)
                .WithMessage("ViagemId é obrigatório");

            RuleFor(x => x.ParticipanteId)
                .GreaterThan(0)
                .WithMessage("ParticipanteId é obrigatório");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Status inválido");
        }
    }
}