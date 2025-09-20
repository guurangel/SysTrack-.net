namespace SysTrack.DTO.Response
{
    public class MotocicletaResponse
    {
        public Guid Id { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Cor { get; set; } = string.Empty;
        public DateTime DataEntrada { get; set; }
        public Guid PatioId { get; set; }
        public string PatioNome { get; set; } = string.Empty;

        public List<Link> Links { get; set; } = new();

    }
}