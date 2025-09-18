using System.ComponentModel.DataAnnotations;
using SysTrack.Infrastructure.Persistance.Enums;
using SysTrack.Infrastructure.Persistence;

namespace SysTrack.Infrastructure.Persistance
{
    public class Usuario
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(150, ErrorMessage = "O email deve ter no máximo 150 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória")]
        [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF é obrigatório")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter 11 caracteres")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve conter apenas números")]
        public string Cpf { get; set; } = string.Empty;

        [Required(ErrorMessage = "O cargo é obrigatório")]
        public Cargo Cargo { get; set; }

        [Required(ErrorMessage = "A data de admissão é obrigatória")]
        public DateTime DataAdmissao { get; set; } = DateTime.Now;

        [Required]
        public Guid PatioId { get; set; }

        [Required]
        public Patio Patio { get; set; } = null!;
    }
}
