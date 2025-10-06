using TravelDoc.Application.Usuarios.Domain;

namespace TravelDoc.Api.Features.Viagens.Obter
{
    public class ViagemViewModel
    {
        public int? Id { get; set; }
        public string? NomeViagem { get; set; }
        public string? Destino { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string? Descricao{ get; set; }
        public int? CriadorId { get; set; }
        public int? Status { get; set; }
        public List<Usuario>? Participantes { get; set; }
    }
}
