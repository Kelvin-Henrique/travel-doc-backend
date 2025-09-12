using FluentValidation;
using MediatR;
using TravelDoc.Api.Domain.Planos.Entities;
using TravelDoc.Api.Domain.Planos.Repositories;
using TravelDoc.Api.Domain.Usuarios.Entities;
using TravelDoc.Api.Extensions;
using TravelDoc.Application.Domain.Usuarios.Repositories;
using TravelDoc.Application.Usuarios.Domain;
using TravelDoc.Infrastructure.Core.Results;
using TravelDoc.Infrastructure.Persistence.Context;

namespace TravelDoc.Api.Features.Planos.Inserir
{
    public sealed class UpsertPlanoEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("planos", async (UpsertPlanoRequest request, IMediator _mediator) =>
            {
                var result = await _mediator.Send(request);

                return result.IsFailure ? Results.BadRequest(new
                {
                    Mensagem = result.Error
                }) : Results.Ok();
            });
        }
    }

    public class UpsertPlanoRequestHandler : IRequestHandler<UpsertPlanoRequest, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPlanoRepository _planoRepository;

        public UpsertPlanoRequestHandler(IUnitOfWork unitOfWork, IPlanoRepository planoRepository)
        {
            _unitOfWork = unitOfWork;
            _planoRepository = planoRepository;
        }

        public async Task<Result> Handle(UpsertPlanoRequest request, CancellationToken cancellationToken)
        {
            return request.Id <= 0 ?
                await Incluir(request) :
                await Atualizar(request);
        }

        private async ValueTask<Result> Incluir(UpsertPlanoRequest request)
        {
            var obj = Plano.Criar(request.Nome, request.Descricao, request.Valor, request.Icone);
            if (obj.IsFailure)
            {
                return Result.Failure(obj.Error);
            }

            await _planoRepository.InserirAsync(obj.Value);

            await _unitOfWork.CommitAsync();

            return Result.Success();
        }

        private async ValueTask<Result> Atualizar(UpsertPlanoRequest request)
        { 
            var obj = await _planoRepository.ObterAsync(request.Id);
            if (obj is null)
            {
                return Result.Failure("Plano não encontrado!");
            }

            var atualizado = obj.Atualizar(request.Nome, request.Descricao, request.Valor, request.Icone);
            if (atualizado.IsFailure)
            {
                return Result.Failure(atualizado.Error);
            }

            await _planoRepository.Atualizar(obj);
        }
    }

    public record UpsertPlanoRequest : IRequest<Result>
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public string Icone { get; set; }
    }

    public class UpsertPlanoRequestValidator : AbstractValidator<UpsertPlanoRequest>
    {
        public UpsertPlanoRequestValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Nome é obrigatório")
                .MaximumLength(500)
                .WithMessage("O nome deve ter no máximo 500 caracteres")
                .MinimumLength(3)
                .WithMessage("O nome deve ter no mínimo 3 caracteres");

            RuleFor(x => x.Descricao)
                .NotEmpty()
                .WithMessage("A descrição é obrigatória")
                .MaximumLength(500)
                .WithMessage("A descrição deve ter no máximo 500 caracteres");

            RuleFor(x => x.Valor)
                .NotEmpty()
                .WithMessage("O valor do plano deveser informado");

            RuleFor(x => x.Icone)
                .NotEmpty()
                .WithMessage("O ícone deve ser informado!");
        }
    }
}