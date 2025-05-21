using SysTrack.DTO.Request;
using SysTrack.Infrastructure.Persistence;

namespace SysTrack.Infrastructure.Extensions
{
    public static class MotocicletaFiltroExtensions
    {
        public static IQueryable<Motocicleta> AplicarFiltros(
            this IQueryable<Motocicleta> query,
            MotocicletaFiltroRequest filtro)
        {
            if (!string.IsNullOrEmpty(filtro.Marca))
                query = query.Where(m => m.Marca.Contains(filtro.Marca));

            if (!string.IsNullOrEmpty(filtro.Modelo))
                query = query.Where(m => m.Modelo.Contains(filtro.Modelo));

            if (!string.IsNullOrEmpty(filtro.Cor))
                query = query.Where(m => m.Cor.Contains(filtro.Cor));

            if (!string.IsNullOrEmpty(filtro.Placa))
                query = query.Where(m => m.Placa.Contains(filtro.Placa));

            if (filtro.PatioId.HasValue)
                query = query.Where(m => m.PatioId == filtro.PatioId.Value);

            if (filtro.DataEntradaInicio.HasValue)
                query = query.Where(m => m.DataEntrada >= filtro.DataEntradaInicio.Value);

            if (filtro.DataEntradaFim.HasValue)
                query = query.Where(m => m.DataEntrada <= filtro.DataEntradaFim.Value);

            return query;
        }
    }
}