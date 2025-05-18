namespace SysTrack.DTO.Request
{
    public class MotocicletaRequest
    {
        public string Placa { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Cor { get; set; } = string.Empty;
        public Guid PatioId { get; set; }
    }
}