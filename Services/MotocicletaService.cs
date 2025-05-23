using Microsoft.EntityFrameworkCore;
using SysTrack.Infrastructure.Contexts;
using SysTrack.Infrastructure.Persistence;

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
            var podeAdicionar = await PodeAdicionarMotocicletaAsync(moto.PatioId);

            if (!podeAdicionar)
                throw new InvalidOperationException("Capacidade máxima do pátio atingida.");

            _context.Motocicletas.Add(moto);
            await _context.SaveChangesAsync();
            return moto;
        }
    }
}
