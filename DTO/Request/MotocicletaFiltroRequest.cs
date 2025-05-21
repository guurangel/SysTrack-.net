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
    }
}