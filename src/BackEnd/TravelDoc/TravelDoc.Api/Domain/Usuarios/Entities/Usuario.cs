using TravelDoc.Api.Domain.Usuarios.Entities;
using TravelDoc.Infrastructure.Core.Results;

namespace TravelDoc.Application.Usuarios.Domain
{
    public class Usuario
    {
        private Usuario(int id, string cpf, string nome, string email, string telefone, TipoUsuarioEnum tipo, bool autenticacaoDoisFatores)
        {
            Id = id;
            Cpf = cpf;
            Nome = nome;
            Email = email;
            Telefone = telefone;
            Tipo = tipo;
            AutenticacaoDoisFatores = autenticacaoDoisFatores;
        }

        public int Id { get; private set; }
        public string Cpf { get; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Telefone { get; }
        public TipoUsuarioEnum Tipo { get; }
        public bool AutenticacaoDoisFatores { get; private set; }

        public static Result<Usuario> Criar(string cpf, string nome, string email, string telefone, TipoUsuarioEnum tipo)
        {
            if (string.IsNullOrEmpty(cpf))
            {
                return Result.Failure("O CPF deve ser informado!");
            }

            if (cpf.Length > 11)
            {
                return Result.Failure("O CPF deve ter no máximo 11 caracteres!");
            }

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

            if (string.IsNullOrEmpty(telefone))
            {
                return Result.Failure("O telefone deve ser informado!");
            }

            if (telefone.Length > 14)
            {
                return Result.Failure("O telefone deve ter no máximo 14 caracteres!");
            }

            return Result<Usuario>.Success(new Usuario(0, cpf, nome, email, telefone, tipo, false));
        }

        public void HabilitarAutenticacaoDoisFatores()
        {
            AutenticacaoDoisFatores = true;
        }
    }
}
