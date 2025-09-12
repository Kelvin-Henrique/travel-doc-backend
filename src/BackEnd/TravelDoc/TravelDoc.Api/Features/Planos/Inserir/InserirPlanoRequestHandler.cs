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
    public sealed class InserirPlanoEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("planos", async (InserirPlanoRequest request, IMediator _mediator) =>
            {
                var result = await _mediator.Send(request);

                return result.IsFailure ? Results.BadRequest(new
                {
                    Mensagem = result.Error
                }) : Results.Ok();
            });
        }
    }

    public class InserirPlanoRequestHandler : IRequestHandler<InserirPlanoRequest, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPlanoRepository _planoRepository;

        public InserirPlanoRequestHandler(IUnitOfWork unitOfWork, IPlanoRepository planoRepository)
        {
            _unitOfWork = unitOfWork;
            _planoRepository = planoRepository;
        }

        public async Task<Result> Handle(InserirPlanoRequest request, CancellationToken cancellationToken)
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
    }

    public record InserirPlanoRequest : IRequest<Result>
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public string Icone { get; set; }
    }

    public class InserirPlanoRequestValidator : AbstractValidator<InserirPlanoRequest>
    {
        public InserirPlanoRequestValidator()
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