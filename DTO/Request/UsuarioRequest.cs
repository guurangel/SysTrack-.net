using SysTrack.Infrastructure.Persistance.Enums;

namespace SysTrack.DTO.Request
{
    public class UsuarioRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public Cargo Cargo { get; set; }
        public DateTime? dataAdmissao { get; set; }
        public Guid PatioId { get; set; }
    }
}
