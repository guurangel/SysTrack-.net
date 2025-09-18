using SysTrack.DTO.Request;
using SysTrack.Infrastructure.Persistance;

namespace SysTrack.Infrastructure.Extensions
{
    public static class UsuarioFiltroExtensions
    {
        public static IQueryable<Usuario> AplicarFiltros(
            this IQueryable<Usuario> query,
            UsuarioFiltroRequest filtro)
        {
            if (!string.IsNullOrEmpty(filtro.Nome))
                query = query.Where(u => u.Nome.Contains(filtro.Nome));

            if (!string.IsNullOrEmpty(filtro.Email))
                query = query.Where(u => u.Email.Contains(filtro.Email));

            if (!string.IsNullOrEmpty(filtro.Cpf))
                query = query.Where(u => u.Cpf.Contains(filtro.Cpf));

            if (filtro.Cargo.HasValue)
                query = query.Where(u => u.Cargo == filtro.Cargo.Value);

            if (filtro.PatioId.HasValue)
                query = query.Where(u => u.PatioId == filtro.PatioId.Value);

            if (filtro.DataAdmissaoInicio.HasValue)
                query = query.Where(u => u.DataAdmissao >= filtro.DataAdmissaoInicio.Value);

            if (filtro.DataAdmissaoFim.HasValue)
                query = query.Where(u => u.DataAdmissao <= filtro.DataAdmissaoFim.Value);

            return query;
        }
    }
}
