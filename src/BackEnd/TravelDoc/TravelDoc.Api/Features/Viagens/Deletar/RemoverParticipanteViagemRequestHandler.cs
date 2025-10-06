using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravelDoc.Api.Domain.Viagens.Repositories;
using TravelDoc.Api.Extensions;
using TravelDoc.Infrastructure.Core.Results;
using TravelDoc.Infrastructure.Persistence.Context;

namespace TravelDoc.Api.Features.Viagens.Deletar
{
    public sealed class RemoverParticipanteViagemEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("viagens/{viagemId}/participantes/{participanteId}", 
                async (int viagemId, int participanteId, [FromQuery] int usuarioId, IMediator mediator) =>
            {
                var result = await mediator.Send(new RemoverParticipanteViagemRequest
                {
                    ViagemId = viagemId,
                    ParticipanteId = participanteId,
                    UsuarioId = usuarioId
                });

                return result.IsFailure ? Results.BadRequest(new
                {
                    Mensagem = result.Error
                }) : Results.Ok();
            });
        }
    }

    public class RemoverParticipanteViagemRequestHandler : IRequestHandler<RemoverParticipanteViagemRequest, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IViagemRepository _viagemRepository;
        private readonly IViagemParticipanteRepository _viagemParticipanteRepository;

        public RemoverParticipanteViagemRequestHandler(
            IUnitOfWork unitOfWork, 
            IViagemRepository viagemRepository,
            IViagemParticipanteRepository viagemParticipanteRepository)
        {
            _unitOfWork = unitOfWork;
            _viagemRepository = viagemRepository;
            _viagemParticipanteRepository = viagemParticipanteRepository;
        }

        public async Task<Result> Handle(RemoverParticipanteViagemRequest request, CancellationToken cancellationToken)
        {
            var viagem = await _viagemRepository.ObterAsync(request.ViagemId);
            if (viagem == null)
            {
                return Result.Failure("Viagem não encontrada!");
            }

            if (viagem.CriadorId != request.UsuarioId)
            {
                return Result.Failure("Você não tem permissão para editar esta viagem!");
            }

            var participante = await _viagemParticipanteRepository.ObterPorViagemEParticipanteAsync(
                request.ViagemId, 
                request.ParticipanteId
            );

            if (participante == null)
            {
                return Result.Failure("Participante não encontrado nesta viagem!");
            }

            await _viagemParticipanteRepository.ExcluirAsync(participante.Id);
            await _unitOfWork.CommitAsync();

            return Result.Success();
        }
    }

    public record RemoverParticipanteViagemRequest : IRequest<Result>
    {
        public int ViagemId { get; set; }
        public int ParticipanteId { get; set; }
        public int UsuarioId { get; set; }
    }

    public class RemoverParticipanteViagemRequestValidator : AbstractValidator<RemoverParticipanteViagemRequest>
    {
        public RemoverParticipanteViagemRequestValidator()
        {
            RuleFor(x => x.ViagemId)
                .GreaterThan(0)
                .WithMessage("ViagemId é obrigatório");

            RuleFor(x => x.ParticipanteId)
                .GreaterThan(0)
                .WithMessage("ParticipanteId é obrigatório");

            RuleFor(x => x.UsuarioId)
                .GreaterThan(0)
                .WithMessage("UsuarioId é obrigatório");
        }
    }
}
