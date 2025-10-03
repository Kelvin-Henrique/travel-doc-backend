using TravelDoc.Infrastructure.Core.Results;
using TravelDoc.Api.Extensions;
using MediatR;
using FluentValidation;
using TravelDoc.Infrastructure.Persistence.Context;
using TravelDoc.Application.Domain.Usuarios.Repositories;
using TravelDoc.Application.Usuarios.Domain;
using TravelDoc.Api.Domain.Usuarios.Entities;
using TravelDoc.Api.Infrastructure.Common.Extensions.Validators;
using TravelDoc.Api.Domain.Planos.Entities;
using TravelDoc.Api.Features.Planos.Inserir;

namespace TravelDoc.Application.Features.Usuarios.Inserir
{
    public sealed class InserirUsuarioEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("usuarios", async (InserirUsuarioRequest request, IMediator _mediator) =>
            {
                var result = await _mediator.Send(request);

                return result.IsFailure ? Results.BadRequest(new 
                { 
                    Mensagem = result.Error
                }) : Results.Ok();
            });

        }
    }

    public class InserirUsuarioRequestHandler : IRequestHandler<InserirUsuarioRequest, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsuarioRepository _usuarioRepository;

        public InserirUsuarioRequestHandler(IUsuarioRepository usuarioRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Result> Handle(InserirUsuarioRequest request, CancellationToken cancellationToken)
        {
            return request.Id <= 0 ?
                await Incluir(request) :
                await Atualizar(request);
        }

        private async ValueTask<Result> Incluir(InserirUsuarioRequest request)
        {
            var usuario = Usuario.Criar(request.Cpf, request.Nome, request.Email, request.Telefone, request.Tipo);
            if (usuario.IsFailure)
            {
                return Result.Failure(usuario.Error);
            }

            if (await _usuarioRepository.ExisteAsync(usuario.Value))
            {
                return Result.Failure("Já existe um usuário com esse cpf ou e-mail!");
            }

            await _usuarioRepository.InserirAsync(usuario.Value);

            await _unitOfWork.CommitAsync();

            return Result.Success();
        }

        private async ValueTask<Result> Atualizar(InserirUsuarioRequest request)
        {
            var obj = await _usuarioRepository.ObterAsync(request.Email);
            if (obj is null)
            {
                return Result.Failure("Plano não encontrado!");
            }

            var atualizado = obj.Atualizar(request.Nome, request.Email, request.Telefone);
            if (atualizado.IsFailure)
            {
                return Result.Failure(atualizado.Error);
            }

            return Result.Success();
        }
    }

    public record InserirUsuarioRequest : IRequest<Result>
    {
        public int? Id { get; set; }
        public required string Cpf { get; set; }
        public required string Telefone { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public TipoUsuarioEnum Tipo { get; set; }
    }

    public class InserirUsuarioRequestValidator : AbstractValidator<InserirUsuarioRequest>
    {
        public InserirUsuarioRequestValidator()
        {
            RuleFor(x => x.Cpf)
                .IsValidCpf();

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

            RuleFor(x => x.Telefone)
                .NotEmpty()
                .WithMessage("O telefone é obrigatório")
                .MaximumLength(14)
                .WithMessage("O telefone deve ter no máximo 500 caracteres");

            RuleFor(x => x.Tipo)
                .NotEmpty()
                .WithMessage("O tipo é obrigatório!");
        }
    }
}
