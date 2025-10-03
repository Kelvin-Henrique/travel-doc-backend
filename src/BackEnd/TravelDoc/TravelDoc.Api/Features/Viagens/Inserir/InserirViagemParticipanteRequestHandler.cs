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
        public required string EmailConvidado { get; set; }
        public  StatusViagemParticipanteEnum? Status { get; set; }
    }

    public class InserirViagemParticipanteRequestHandler : IRequestHandler<InserirViagemParticipanteRequest, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IViagemParticipanteRepository _viagemParticipanteRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public InserirViagemParticipanteRequestHandler(IUnitOfWork unitOfWork, IViagemParticipanteRepository viagemParticipanteRepository, IUsuarioRepository usuarioRepository)
        {
            _unitOfWork = unitOfWork;
            _viagemParticipanteRepository = viagemParticipanteRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Result> Handle(InserirViagemParticipanteRequest request, CancellationToken cancellationToken)
        {
            var usuarioConvidado = await _usuarioRepository.ObterAsync(request.EmailConvidado);

            if (usuarioConvidado == null)
            {
                return Result.Failure("Convidado não possui no cadastro no TravelDoc");
            }
         
            var participanteResult = ViagemParticipante.Criar(request.ViagemId, usuarioConvidado.Id, StatusViagemParticipanteEnum.Pendente);
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

            RuleFor(x => x.EmailConvidado)
                .NotEmpty()
                .WithMessage("O e-mail é obrigatório")
                .MinimumLength(3)
                .WithMessage("O nome deve ter no mínimo 3 caracteres");


        }
    }
}