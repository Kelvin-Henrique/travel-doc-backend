using FluentValidation;
using MediatR;
using TravelDoc.Api.Domain.Viagens.Entities;
using TravelDoc.Api.Domain.Viagens.Repositories;
using TravelDoc.Api.Extensions;
using TravelDoc.Application.Domain.Usuarios.Repositories;
using TravelDoc.Infrastructure.Core.Results;
using TravelDoc.Infrastructure.Persistence.Context;

namespace TravelDoc.Api.Features.Viagens.Participantes.Inserir
{
    public sealed class ConvidarParticipanteViagemEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("viagens/{tripId}/invite", async (int tripId, ConvidarParticipanteViagemRequest request, IMediator mediator) =>
            {
                request.ViagemId = tripId;
                var result = await mediator.Send(request);

                return result.IsFailure
                    ? Results.BadRequest(new { Mensagem = result.Error })
                    : Results.Ok();
            });
        }
    }

    public record ConvidarParticipanteViagemRequest : IRequest<Result>
    {
        public int ViagemId { get; set; }
        public required string Email { get; set; }
    }

    public class ConvidarParticipanteViagemRequestHandler : IRequestHandler<ConvidarParticipanteViagemRequest, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IViagemRepository _viagemRepository;
        private readonly IViagemParticipanteRepository _viagemParticipanteRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public ConvidarParticipanteViagemRequestHandler(
            IUnitOfWork unitOfWork, 
            IViagemRepository viagemRepository,
            IViagemParticipanteRepository viagemParticipanteRepository, 
            IUsuarioRepository usuarioRepository)
        {
            _unitOfWork = unitOfWork;
            _viagemRepository = viagemRepository;
            _viagemParticipanteRepository = viagemParticipanteRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Result> Handle(ConvidarParticipanteViagemRequest request, CancellationToken cancellationToken)
        {
            // Verifica se a viagem existe
            if (!await _viagemRepository.ExisteAsync(request.ViagemId))
            {
                return Result.Failure("Viagem não encontrada!");
            }

            // Verifica se o usuário existe
            var usuarioConvidado = await _usuarioRepository.ObterAsync(request.Email);
            if (usuarioConvidado == null)
            {
                return Result.Failure("Convidado não possui cadastro no TravelDoc");
            }

            // Cria o convite/participante
            var participanteResult = ViagemParticipante.Criar(
                request.ViagemId, 
                usuarioConvidado.Id, 
                StatusViagemParticipanteEnum.Pendente
            );
            
            if (participanteResult.IsFailure)
            {
                return Result.Failure(participanteResult.Error);
            }

            await _viagemParticipanteRepository.InserirAsync(participanteResult.Value);
            await _unitOfWork.CommitAsync();

            // Mock de envio de notificação/email
            // TODO: Implementar serviço de notificação/email
            // await _notificationService.SendInviteEmailAsync(usuarioConvidado.Email, viagemId);

            return Result.Success();
        }
    }

    public class ConvidarParticipanteViagemRequestValidator : AbstractValidator<ConvidarParticipanteViagemRequest>
    {
        public ConvidarParticipanteViagemRequestValidator()
        {
            RuleFor(x => x.ViagemId)
                .GreaterThan(0)
                .WithMessage("ViagemId é obrigatório");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("O e-mail é obrigatório")
                .EmailAddress()
                .WithMessage("O e-mail deve ser válido");
        }
    }
}
