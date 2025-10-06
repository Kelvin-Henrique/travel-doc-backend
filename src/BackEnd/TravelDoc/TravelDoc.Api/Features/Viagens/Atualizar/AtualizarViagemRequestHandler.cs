using FluentValidation;
using MediatR;
using TravelDoc.Api.Domain.Viagens.Entities;
using TravelDoc.Api.Domain.Viagens.Repositories;
using TravelDoc.Api.Extensions;
using TravelDoc.Infrastructure.Core.Results;
using TravelDoc.Infrastructure.Persistence.Context;

namespace TravelDoc.Api.Features.Viagens.Atualizar
{
    public sealed class AtualizarViagemEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("viagens/{id}", async (int id, AtualizarViagemRequest request, IMediator mediator) =>
            {
                request.Id = id;
                var result = await mediator.Send(request);

                return result.IsFailure ? Results.BadRequest(new
                {
                    Mensagem = result.Error
                }) : Results.Ok();
            });
        }
    }

    public class AtualizarViagemRequestHandler : IRequestHandler<AtualizarViagemRequest, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IViagemRepository _viagemRepository;

        public AtualizarViagemRequestHandler(IUnitOfWork unitOfWork, IViagemRepository viagemRepository)
        {
            _unitOfWork = unitOfWork;
            _viagemRepository = viagemRepository;
        }

        public async Task<Result> Handle(AtualizarViagemRequest request, CancellationToken cancellationToken)
        {
            var viagem = await _viagemRepository.ObterAsync(request.Id);
            if (viagem == null)
            {
                return Result.Failure("Viagem não encontrada!");
            }

            if (viagem.CriadorId != request.UsuarioId)
            {
                return Result.Failure("Você não tem permissão para editar esta viagem!");
            }

            var resultado = viagem.Atualizar(
                request.NomeViagem,
                request.Destino,
                request.DataInicio.Date.ToUniversalTime(),
                request.DataFim.Date.ToUniversalTime(),
                request.Descricao ?? "",
                request.UsuarioId,
                request.Status ?? StatusViagemEnum.Planejada
            );

            if (resultado.IsFailure)
            {
                return Result.Failure(resultado.Error);
            }

            await _unitOfWork.CommitAsync();

            return Result.Success();
        }
    }

    public record AtualizarViagemRequest : IRequest<Result>
    {
        public int Id { get; set; }
        public required int UsuarioId { get; set; }
        public required string NomeViagem { get; set; }
        public required string Destino { get; set; }
        public required DateTime DataInicio { get; set; }
        public required DateTime DataFim { get; set; }
        public string? Descricao { get; set; }
        public StatusViagemEnum? Status { get; set; }
    }

    public class AtualizarViagemRequestValidator : AbstractValidator<AtualizarViagemRequest>
    {
        public AtualizarViagemRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id é obrigatório");

            RuleFor(x => x.UsuarioId)
                .GreaterThan(0)
                .WithMessage("UsuarioId é obrigatório");

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
