using TravelDoc.Application.Usuarios.Domain;
using TravelDoc.Infrastructure.Core.Domain;
using TravelDoc.Infrastructure.Core.Results;

namespace TravelDoc.Api.Domain.Viagens.Entities
{
    public class Viagem : Entity, IAggregateRoot
    {
#pragma warning disable CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere adicionar o modificador "obrigatório" ou declarar como anulável.
        protected Viagem()
#pragma warning restore CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere adicionar o modificador "obrigatório" ou declarar como anulável.
        {
        }

        private Viagem(int id, string nomeViagem, string destino, DateTime dataInicio, DateTime dataFim, string descricao, int criadorId, StatusViagemEnum status)
        {
            Id = id;
            NomeViagem = nomeViagem;
            Destino = destino;
            DataInicio = dataInicio;
            DataFim = dataFim;
            Descricao = descricao;
            CriadorId = criadorId;
            Status = status;

        }

        public string NomeViagem { get; private set; }
        public string Destino { get; private set; }
        public DateTime DataInicio{ get; private set; }
        public DateTime DataFim { get; private set; }
        public string Descricao { get; private set; }
        public int CriadorId { get; private set; }
        public Usuario Criador { get; set; }
        public StatusViagemEnum Status { get; private set; }

        public static Result<Viagem> Criar(string nomeViagem, string destino, DateTime dataInicio, DateTime dataFim, string descricao, int criadorId, StatusViagemEnum status)
        {
            if (string.IsNullOrEmpty(nomeViagem))
            {
                return Result.Failure("O nome da viagem deve ser informado!");
            }

            if (nomeViagem.Length > 500)
            {
                return Result.Failure("O nome deve ter no máximo 200 caracteres!");
            }

            if (string.IsNullOrWhiteSpace(destino))
            {
                return Result.Failure("O destino da viagem deve ser informado!");
            }

            if (destino.Length > 100)
            {
                return Result.Failure("O destino deve ter no máximo 100 caracteres!");
            }

            if (dataInicio == default(DateTime))
            {
                return Result.Failure("A data de início da viagem deve ser informada!");
            }

            if (dataFim == default(DateTime))
            {
                return Result.Failure("A data de fim da viagem deve ser informada!");
            }

            if (dataInicio.Date > dataFim.Date)
            {
                return Result.Failure("A data de início não pode ser posterior à data de fim da viagem!");
            }


            if (descricao.Length > 500)
            {
                return Result.Failure("A descrição deve ter no máximo 500 caracteres!");
            }

            if (criadorId <= 0)
            {
                return Result.Failure("O ID do criador da viagem é inválido!");
            }



            return Result<Viagem>.Success(new Viagem(0, nomeViagem, destino, dataInicio, dataFim, descricao, criadorId, status));
        }

        public Result Atualizar(string nomeViagem, string destino, DateTime dataInicio, DateTime dataFim, string descricao, int criadorId, StatusViagemEnum status)
        {
            if (string.IsNullOrEmpty(nomeViagem))
            {
                return Result.Failure("O nome da viagem deve ser informado!");
            }

            if (nomeViagem.Length > 500)
            {
                return Result.Failure("O nome deve ter no máximo 200 caracteres!");
            }

            if (string.IsNullOrWhiteSpace(destino))
            {
                return Result.Failure("O destino da viagem deve ser informado!");
            }

            if (destino.Length > 100)
            {
                return Result.Failure("O destino deve ter no máximo 100 caracteres!");
            }

            if (dataInicio == default(DateTime))
            {
                return Result.Failure("A data de início da viagem deve ser informada!");
            }

            if (dataFim == default(DateTime))
            {
                return Result.Failure("A data de fim da viagem deve ser informada!");
            }

            if (dataInicio.Date > dataFim.Date)
            {
                return Result.Failure("A data de início não pode ser posterior à data de fim da viagem!");
            }


            if (descricao.Length > 500)
            {
                return Result.Failure("A descrição deve ter no máximo 500 caracteres!");
            }

            if (criadorId <= 0)
            {
                return Result.Failure("O ID do criador da viagem é inválido!");
            }
            
            NomeViagem = nomeViagem;
            Destino = destino;
            DataInicio = dataInicio;
            DataFim = dataFim;
            Descricao = descricao;
            CriadorId = criadorId;
            Status = status;

            return Result.Success();
        }
    }
}
