using TravelDoc.Infrastructure.Core.Results;

namespace TravelDoc.Application.Usuarios.Domain
{
    public class Usuario
    {
        private Usuario(int id, string nome, string email)
        {
            Id = id;
            Nome = nome;
            Email = email;
        }

        public int Id { get; }
        public string Nome { get; private set; }
        public string Email { get; private set; }

        public static Result<Usuario> Criar(string nome, string email)
        {
            if (string.IsNullOrEmpty(nome))
            {
                return Result.Failure("O nome deve ser informado!");
            }

            if (nome.Length > 500)
            {
                return Result.Failure("O nome deve ter no máximo 500 caracteres!");
            }

            if (string.IsNullOrEmpty(email))
            {
                return Result.Failure("O email deve ser informado!");
            }

            if (email.Length > 200)
            {
                return Result.Failure("O email deve ter no máximo 200 caracteres!");
            }

            return Result<Usuario>.Success(new Usuario(0, nome, email));
        }
    }
}
