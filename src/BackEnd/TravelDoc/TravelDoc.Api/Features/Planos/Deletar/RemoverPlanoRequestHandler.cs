using FluentValidation;
using MediatR;
using TravelDoc.Api.Domain.Planos.Entities;
using TravelDoc.Api.Domain.Planos.Repositories;
using TravelDoc.Api.Extensions;
using TravelDoc.Infrastructure.Core.Results;
using TravelDoc.Infrastructure.Persistence.Context;

namespace TravelDoc.Api.Features.Planos.Deletar
{
    public sealed class RemoverPlanoEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("planos/{id}", async (int id, IMediator _mediator) =>
            {
                var result = await _mediator.Send(new RemoverPlanoRequest
                { 
                    Id = id
                });

                return result.IsFailure ? Results.BadRequest(new
                {
                    Mensagem = result.Error
                }) : Results.Ok();
            });
        }
    }

    public class RemoverPlanoRequestHandler : IRequestHandler<RemoverPlanoRequest, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPlanoRepository _planoRepository;

        public RemoverPlanoRequestHandler(IUnitOfWork unitOfWork, IPlanoRepository planoRepository)
        {
            _unitOfWork = unitOfWork;
            _planoRepository = planoRepository;
        }

        public async Task<Result> Handle(RemoverPlanoRequest request, CancellationToken cancellationToken)
        {
            if (!await _planoRepository.ExisteAsync(request.Id))
            {
                return Result.Failure("O plano informado não existe!");
            }

            await _planoRepository.ExcluirAsync(request.Id);

            await _unitOfWork.CommitAsync();

            return Result.Success();
        }
    }

    public record RemoverPlanoRequest : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class RemoverPlanoRequestValidator : AbstractValidator<RemoverPlanoRequest>
    {
        public RemoverPlanoRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id é obrigatório");
        }
    }
}
