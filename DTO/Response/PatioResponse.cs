namespace SysTrack.DTO.Response
{
    public class PatioResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;

        public int CapacidadeMaxima { get; set; }

        public DateTime DataCriacao { get; set; }

        public List<MotocicletaResponse> Motocicletas { get; set; } = new();

        public List<UsuarioSimplesResponse> Usuarios { get; set; } = new();
    }
}