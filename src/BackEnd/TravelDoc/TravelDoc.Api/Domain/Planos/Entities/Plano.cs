using TravelDoc.Infrastructure.Core.Domain;
using TravelDoc.Infrastructure.Core.Results;

namespace TravelDoc.Api.Domain.Planos.Entities
{
    public class Plano : Entity
    {
        protected Plano()
        { 
        }

        private Plano(int id, string nome, string descricao, decimal valor, string icone, bool ativo)
        {
            Id = id;
            Nome = nome;
            Descricao = descricao;
            Valor = valor;
            Icone = icone;
            Ativo = ativo;
        }

        public string Nome { get; }
        public string Descricao { get; }
        public decimal Valor { get; }
        public string Icone { get; }
        public bool Ativo { get; }

        public static Result<Plano> Criar(string nome, string descricao, decimal valor, string icone)
        {
            if (string.IsNullOrEmpty(nome))
            {
                return Result.Failure("O nome deve ser informado!");
            }

            if (nome.Length > 500)
            {
                return Result.Failure("O nome deve ter no máximo 500 caracteres!");
            }

            if (string.IsNullOrEmpty(descricao))
            {
                return Result.Failure("A descrição deve ser informada!");
            }

            if (descricao.Length > 500)
            {
                return Result.Failure("A descrição deve ter no máximo 500 caracteres!");
            }

            if (valor <= 0)
            {
                return Result.Failure("O valor deve ser maior ou igual a zero!");
            }

            if (string.IsNullOrEmpty(icone))
            {
                return Result.Failure("O ícone deve ser informado!");
            }

            if (icone.Length > 200)
            {
                return Result.Failure("O ícone deve ter no máximo 200 caracteres!");
            }

            return Result<Plano>.Success(new Plano(0, nome, descricao, valor, icone, true));
        }
    }
}
