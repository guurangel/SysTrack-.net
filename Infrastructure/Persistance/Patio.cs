using System.ComponentModel.DataAnnotations;
using SysTrack.Infrastructure.Persistance;

namespace SysTrack.Infrastructure.Persistence
{
    public class Patio
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "O nome do pátio deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(200, ErrorMessage = "O endereço deve ter no máximo 200 caracteres.")]
        public string Endereco { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "A capacidade máxima deve ser maior que zero.")]
        public int CapacidadeMaxima { get; set; }

        [Required]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public ICollection<Motocicleta> Motocicletas { get; set; } = new List<Motocicleta>();

        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
