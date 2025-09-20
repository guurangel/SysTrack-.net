using SysTrack.Infrastructure.Persistance.Enums;

namespace SysTrack.DTO.Response
{
    public class UsuarioSimplesResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Cargo Cargo { get; set; }
    }
}
