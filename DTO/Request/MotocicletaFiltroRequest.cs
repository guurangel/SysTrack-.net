using SysTrack.Infrastructure.Persistance.Enums;

namespace SysTrack.DTO.Request
{
    public class MotocicletaFiltroRequest
    {
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? Cor { get; set; }
        public string? Placa { get; set; }
        public Guid? PatioId { get; set; }

        public DateTime? DataEntradaInicio { get; set; }
        public DateTime? DataEntradaFim { get; set; }

        // 🆕 Filtros adicionais
        public StatusMotocicleta? Status { get; set; }

        public int? Ano { get; set; }
        public int? AnoInicio { get; set; }
        public int? AnoFim { get; set; }

        public int? Quilometragem { get; set; }
        public int? QuilometragemMin { get; set; }
        public int? QuilometragemMax { get; set; }
    }
}
