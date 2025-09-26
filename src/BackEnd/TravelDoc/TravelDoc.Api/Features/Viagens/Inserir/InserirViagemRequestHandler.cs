using FluentValidation;
using MediatR;
using TravelDoc.Api.Domain.Usuarios.Entities;
using TravelDoc.Api.Domain.Viagens.Entities;
using TravelDoc.Api.Domain.Viagens.Repositories;
using TravelDoc.Api.Extensions;
using TravelDoc.Application.Domain.Usuarios.Repositories;
using TravelDoc.Application.Usuarios.Domain;
using TravelDoc.Infrastructure.Core.Results;
using TravelDoc.Infrastructure.Persistence.Context;

namespace TravelDoc.Api.Features.Viagens.Inserir
{
    public sealed class InserirViagemEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("viagens", async (InserirViagemRequest request, IMediator _mediator) =>
            {
                var result = await _mediator.Send(request);

                return result.IsFailure ? Results.BadRequest(new
                {
                    Mensagem = result.Error
                }) : Results.Ok();
            });
        }
    }

    public class InserirViagemRequestHandler : IRequestHandler<InserirViagemRequest, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IViagemRepository _viagemRepository;

        public InserirViagemRequestHandler(IUsuarioRepository usuarioRepository, IUnitOfWork unitOfWork, IViagemRepository viagemRepository)
        {
            _unitOfWork = unitOfWork;
            _viagemRepository = viagemRepository;
        }

        public async Task<Result> Handle(InserirViagemRequest request, CancellationToken cancellationToken)
        {
            var usuario = Viagem.Criar(request.NomeViagem, request.Destino, request.DataInicio, request.DataFim, request.Descricao ?? "", request.UsuarioId, StatusViagemEnum.Planejada);
            if (usuario.IsFailure)
            {
                return Result.Failure(usuario.Error);
            }

            await _viagemRepository.InserirAsync(usuario.Value);

            await _unitOfWork.CommitAsync();

            return Result.Success();
        }
    }

    public record InserirViagemRequest : IRequest<Result>
    {
        public required int UsuarioId { get; set; }
        public required string NomeViagem { get; set; }
        public required string Destino{ get; set; }
        public required DateTime DataInicio{ get; set; }
        public required DateTime DataFim { get; set; }
        public string? Descricao { get; set; }
    }

    public class InserirViagemRequestValidator : AbstractValidator<InserirViagemRequest>
    {
        public InserirViagemRequestValidator()
        {

            RuleFor(x => x.NomeViagem)
                .NotEmpty()
                .WithMessage("Nome da viagem é obrigatório")
                .MaximumLength(500)
                .WithMessage("O nome da viagem deve ter no máximo 500 caracteres")
                .MinimumLength(3)
                .WithMessage("O nome da viagem deve ter no mínimo 3 caracteres");

            RuleFor(x => x.Destino)
                .NotEmpty()
                .WithMessage("O destino é obrigatório")
                .MinimumLength(3)
                .WithMessage("O nome do destino deve ter no mínimo 3 caracteres");

            RuleFor(x => x.DataInicio)
                .NotEmpty()
                .WithMessage("A data de início é obrigatória");

            RuleFor(x => x.DataFim)
                .NotEmpty()
                .WithMessage("A data de fim é obrigatória");
        }
    }
}
