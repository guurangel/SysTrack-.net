using SysTrack.Infrastructure.Persistance.Enums;
using System.ComponentModel.DataAnnotations;

namespace SysTrack.DTO.Request
{
    public class MotocicletaRequest
    {
        [Required]
        [RegularExpression(@"^[A-Z]{3}[0-9][A-Z][0-9]{2}$", ErrorMessage = "Placa deve estar no padrão Brasil/Mercosul (ex: ABC1D23)")]
        public string Placa { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Marca { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Modelo { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string Cor { get; set; } = string.Empty;

        [Required]
        public int Ano { get; set; }

        [Required]
        public int Quilometragem { get; set; }

        [Required]
        public StatusMotocicleta Status { get; set; }

        [Required]
        public Guid PatioId { get; set; }
    }
}
