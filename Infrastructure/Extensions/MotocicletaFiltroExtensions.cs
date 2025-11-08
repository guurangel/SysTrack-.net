using SysTrack.DTO.Request;
using SysTrack.Infrastructure.Persistence;
using SysTrack.Infrastructure.Persistance.Enums; // para o enum StatusMotocicleta

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

            // 🔹 Novo: filtro por status
            if (filtro.Status.HasValue)
                query = query.Where(m => m.Status == filtro.Status.Value);

            // 🔹 Novo: filtro por ano exato
            if (filtro.Ano.HasValue)
                query = query.Where(m => m.Ano == filtro.Ano.Value);

            // 🔹 Novo: intervalo de ano
            if (filtro.AnoInicio.HasValue)
                query = query.Where(m => m.Ano >= filtro.AnoInicio.Value);

            if (filtro.AnoFim.HasValue)
                query = query.Where(m => m.Ano <= filtro.AnoFim.Value);

            // 🔹 Novo: quilometragem exata
            if (filtro.Quilometragem.HasValue)
                query = query.Where(m => m.Quilometragem == filtro.Quilometragem.Value);

            // 🔹 Novo: intervalo de quilometragem
            if (filtro.QuilometragemMin.HasValue)
                query = query.Where(m => m.Quilometragem >= filtro.QuilometragemMin.Value);

            if (filtro.QuilometragemMax.HasValue)
                query = query.Where(m => m.Quilometragem <= filtro.QuilometragemMax.Value);

            return query;
        }
    }
}
