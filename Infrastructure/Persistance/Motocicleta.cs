using System.ComponentModel.DataAnnotations;
using SysTrack.Infrastructure.Persistance.Enums;

namespace SysTrack.Infrastructure.Persistence
{
    public class Motocicleta
    {
        public Guid Id { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]{3}[0-9][A-Z][0-9]{2}$", ErrorMessage = "Placa deve estar no padrão Brasil/Mercosul (ex: ABC1D23)")]
        public string Placa { get; set; } = string.Empty;

        public string Marca { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Modelo { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string Cor { get; set; } = string.Empty;

        [Required]
        public DateTime DataEntrada { get; set; } = DateTime.UtcNow;

        [Required]
        public Guid PatioId { get; set; }

        [Required]
        public Patio Patio { get; set; } = null!;

        [Required]
        public int Ano { get; set; }

        [Required]
        public int Quilometragem { get; set; }

        [Required]
        [EnumDataType(typeof(StatusMotocicleta))]
        public StatusMotocicleta Status { get; set; }
    }
}
