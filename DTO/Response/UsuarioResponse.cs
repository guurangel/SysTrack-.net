using SysTrack.Infrastructure.Persistance.Enums;

namespace SysTrack.DTO.Response
{
    public class UsuarioResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public Cargo Cargo { get; set; }

        public DateTime DataAdmissao { get; set; }
        public Guid PatioId { get; set; }
        public string PatioNome { get; set; } = string.Empty;
    }
}
