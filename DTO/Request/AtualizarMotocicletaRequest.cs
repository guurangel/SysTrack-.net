using SysTrack.Infrastructure.Persistance.Enums;

namespace SysTrack.DTO.Request
{
    public class AtualizarMotocicletaRequest
    {
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Cor { get; set; } = string.Empty;
        public Guid PatioId { get; set; }

        // 🆕 Novos campos
        public int Ano { get; set; }
        public int Quilometragem { get; set; }
        public StatusMotocicleta Status { get; set; }
    }
}
