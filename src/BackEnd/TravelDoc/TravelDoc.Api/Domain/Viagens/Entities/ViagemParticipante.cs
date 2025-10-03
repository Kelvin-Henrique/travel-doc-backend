using TravelDoc.Application.Usuarios.Domain;
using TravelDoc.Infrastructure.Core.Domain;
using TravelDoc.Infrastructure.Core.Results;

namespace TravelDoc.Api.Domain.Viagens.Entities
{
    public class ViagemParticipante : Entity, IAggregateRoot
    {
        protected ViagemParticipante()
        {
        }

        private ViagemParticipante(int id, int viagemId, int participanteId, StatusViagemParticipanteEnum status)
        {
            Id = id;
            ViagemId = viagemId;
            ParticipanteId = participanteId;
            Status = status;

        }

        public int ViagemId { get; private set; }
        public int ParticipanteId{ get; private set; }
        public StatusViagemParticipanteEnum Status { get; private set; }

        public static Result<ViagemParticipante> Criar(int viagemId, int participanteId, StatusViagemParticipanteEnum status)
        {
            return Result<ViagemParticipante>.Success(new ViagemParticipante (0, viagemId, participanteId, status));
        }

        public Result Atualizar (StatusViagemParticipanteEnum status)
        {
            Status = status;

            return Result.Success();
        }


    }
}
