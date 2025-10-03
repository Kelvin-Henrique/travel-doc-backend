using MediatR;
using TravelDoc.Api.Extensions;
using TravelDoc.Application.Domain.Usuarios.Repositories;
using TravelDoc.Infrastructure.Core.Results;
using TravelDoc.Infrastructure.Persistence.Context;

namespace TravelDoc.Api.Features.Usuarios.Login
{
    public sealed class LoginUsuarioEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("login", async (LoginUsuarioRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);

                return result.IsFailure
                    ? Results.BadRequest(new { Mensagem = result.Error })
                    : Results.Ok(result);
            });
        }
    }

    public class LoginUsuarioRequestHandler : IRequestHandler<LoginUsuarioRequest, Result<UsuarioViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsuarioRepository _usuarioRepository;

        public LoginUsuarioRequestHandler(IUsuarioRepository usuarioRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Result<UsuarioViewModel>> Handle(LoginUsuarioRequest request, CancellationToken cancellationToken)
        {
            var email = "kelvin.santos@threeo.com.br";

            var usuario = await _usuarioRepository.ObterAsync(email);

            if (usuario == null)
            {
                return Result<UsuarioViewModel>.Failure("Cadastro não encontrado!");
            }

            return Result<UsuarioViewModel>.Success(new UsuarioViewModel
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Cpf = usuario.Cpf,
                Telefone = usuario.Telefone,
            });
        }
    }

    public record LoginUsuarioRequest : IRequest<Result<UsuarioViewModel>>
    {
        public required string Email{ get; set; }
    }

}
