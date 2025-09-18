using SysTrack.Infrastructure.Persistance.Enums;

namespace SysTrack.DTO.Request
{
    public class UsuarioFiltroRequest
    {
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
        public string? Cpf { get; set; }
        public Cargo? Cargo { get; set; }
        public Guid? PatioId { get; set; }
        public DateTime? DataAdmissaoInicio { get; set; }
        public DateTime? DataAdmissaoFim { get; set; }
    }
}
