using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravelDoc.Api.Domain.Viagens.Repositories;
using TravelDoc.Api.Extensions;
using TravelDoc.Infrastructure.Core.Results;
using TravelDoc.Infrastructure.Persistence.Context;

namespace TravelDoc.Api.Features.Viagens.Deletar
{
    public sealed class DeletarViagemEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("viagens/{id}", async (int id, [FromQuery] int usuarioId, IMediator mediator) =>
            {
                var result = await mediator.Send(new DeletarViagemRequest
                {
                    Id = id,
                    UsuarioId = usuarioId
                });

                return result.IsFailure ? Results.BadRequest(new
                {
                    Mensagem = result.Error
                }) : Results.Ok();
            });
        }
    }

    public class DeletarViagemRequestHandler : IRequestHandler<DeletarViagemRequest, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IViagemRepository _viagemRepository;

        public DeletarViagemRequestHandler(IUnitOfWork unitOfWork, IViagemRepository viagemRepository)
        {
            _unitOfWork = unitOfWork;
            _viagemRepository = viagemRepository;
        }

        public async Task<Result> Handle(DeletarViagemRequest request, CancellationToken cancellationToken)
        {
            var viagem = await _viagemRepository.ObterAsync(request.Id);
            if (viagem == null)
            {
                return Result.Failure("Viagem não encontrada!");
            }

            if (viagem.CriadorId != request.UsuarioId)
            {
                return Result.Failure("Você não tem permissão para excluir esta viagem!");
            }

            await _viagemRepository.ExcluirAsync(request.Id);
            await _unitOfWork.CommitAsync();

            return Result.Success();
        }
    }

    public record DeletarViagemRequest : IRequest<Result>
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
    }

    public class DeletarViagemRequestValidator : AbstractValidator<DeletarViagemRequest>
    {
        public DeletarViagemRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id é obrigatório");

            RuleFor(x => x.UsuarioId)
                .GreaterThan(0)
                .WithMessage("UsuarioId é obrigatório");
        }
    }
}
