using TravelDoc.Infrastructure.Core.Results;
using MediatR;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using FluentValidation;
using TravelDoc.Api.Extensions;

namespace TravelDoc.Application.Features.Usuarios.Inserir
{
    internal sealed class InserirUsuarioEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("usuarios", async (InserirUsuarioRequest request, IMediator _mediator) =>
            {
                var result = await _mediator.Send(request);

                return result.IsFailure ? Results.BadRequest() : Results.Ok();
            });
        }
    }

    public class InserirUsuarioRequestHandler : IRequestHandler<InserirUsuarioRequest, Result>
    {
        public async Task<Result> Handle(InserirUsuarioRequest request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(Result.Success());
        }
    }

    public class InserirUsuarioRequest : IRequest<Result>
    {
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string Senha { get; set; }
    }

    public class InserirUsuarioRequestValidator : AbstractValidator<InserirUsuarioRequest>
    {
        public InserirUsuarioRequestValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Nome é obrigatório")
                .MaximumLength(500)
                .WithMessage("O nome deve ter no máximo 500 caracteres")
                .MinimumLength(3)
                .WithMessage("O nome deve ter no mínimo 3 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("O e-mail é obrigatório")
                .MinimumLength(3)
                .WithMessage("O nome deve ter no mínimo 3 caracteres");

            RuleFor(x => x.Senha)
                .NotEmpty()
                .WithMessage("A senha é obrigatória");
        }
    }
}
