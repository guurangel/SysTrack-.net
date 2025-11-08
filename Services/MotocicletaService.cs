using Microsoft.EntityFrameworkCore;
using SysTrack.Infrastructure.Contexts;
using SysTrack.Infrastructure.Persistence;
using System.Text.RegularExpressions;

namespace SysTrack.Services
{
    public class MotocicletaService
    {
        private readonly SysTrackDbContext _context;

        public MotocicletaService(SysTrackDbContext context)
        {
            _context = context;
        }

        public async Task<bool> PodeAdicionarMotocicletaAsync(Guid patioId)
        {
            var patio = await _context.Patios
                .Include(p => p.Motocicletas)
                .FirstOrDefaultAsync(p => p.Id == patioId);

            if (patio == null)
                throw new Exception("Pátio não encontrado.");

            return patio.Motocicletas.Count < patio.CapacidadeMaxima;
        }

        public async Task<Motocicleta> CriarMotocicletaAsync(Motocicleta moto)
        {
            // 🔍 Validação de integridade dos dados
            ValidarMotocicleta(moto);

            // 🏠 Verifica se o pátio existe e se ainda tem vagas
            var podeAdicionar = await PodeAdicionarMotocicletaAsync(moto.PatioId);
            if (!podeAdicionar)
                throw new InvalidOperationException("Capacidade máxima do pátio atingida.");

            // 🚫 Verifica se já existe uma moto com a mesma placa (forma compatível com Oracle)
            var placaExistente = await _context.Motocicletas
                .Where(m => m.Placa == moto.Placa)
                .Select(m => m.Id)
                .FirstOrDefaultAsync();

            if (placaExistente != Guid.Empty)
                throw new InvalidOperationException($"Já existe uma motocicleta cadastrada com a placa {moto.Placa}.");

            // 💾 Persiste no banco
            _context.Motocicletas.Add(moto);
            await _context.SaveChangesAsync();

            return moto;
        }

        /// <summary>
        /// Valida regras de negócio e integridade dos dados da motocicleta
        /// </summary>
        private void ValidarMotocicleta(Motocicleta moto)
        {
            // Placa no formato Mercosul
            var placaRegex = new Regex(@"^[A-Z]{3}[0-9][A-Z][0-9]{2}$");
            if (!placaRegex.IsMatch(moto.Placa))
                throw new ArgumentException("Placa inválida. Use o formato Mercosul (ex: ABC1D23).");

            if (string.IsNullOrWhiteSpace(moto.Marca))
                throw new ArgumentException("A marca da motocicleta é obrigatória.");

            if (string.IsNullOrWhiteSpace(moto.Modelo))
                throw new ArgumentException("O modelo da motocicleta é obrigatório.");

            if (moto.Quilometragem < 0)
                throw new ArgumentException("A quilometragem não pode ser negativa.");

            if (moto.Ano < 1885)
                throw new ArgumentException("Ano inválido. A primeira motocicleta foi criada em 1885.");

            if (moto.Ano > DateTime.UtcNow.Year + 1)
                throw new ArgumentException("Ano inválido. Não é permitido cadastrar motocicletas de anos muito à frente.");

            if (moto.PatioId == Guid.Empty)
                throw new ArgumentException("O ID do pátio é obrigatório.");
        }
    }
}
