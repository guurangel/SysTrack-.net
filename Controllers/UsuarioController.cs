using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SysTrack.Infrastructure.Contexts;
using SysTrack.Infrastructure.Persistance;
using SysTrack.DTO.Request;
using SysTrack.DTO.Response;
using SysTrack.Infrastructure.Extensions;

namespace SysTrack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly SysTrackDbContext _context;

        public UsuarioController(SysTrackDbContext context)
        {
            _context = context;
        }

        // GET: api/usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioResponse>>> GetAll([FromQuery] UsuarioFiltroRequest filtro)
        {
            var query = _context.Usuarios
                .Include(u => u.Patio)
                .AsQueryable()
                .AplicarFiltros(filtro);

            var resultado = await query
                .Select(u => new UsuarioResponse
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    Cpf = u.Cpf,
                    Cargo = u.Cargo,
                    DataAdmissao = u.DataAdmissao,
                    PatioId = u.PatioId,
                    PatioNome = u.Patio.Nome
                })
                .ToListAsync();

            return Ok(resultado);
        }

        // GET: api/usuario/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UsuarioResponse>> GetById(Guid id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Patio)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                return NotFound();

            var response = new UsuarioResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Cpf = usuario.Cpf,
                Cargo = usuario.Cargo,
                DataAdmissao = usuario.DataAdmissao,
                PatioId = usuario.PatioId,
                PatioNome = usuario.Patio.Nome
            };

            return Ok(response);
        }

        // POST: api/usuario
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] UsuarioRequest request)
        {
            var usuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = request.Nome,
                Email = request.Email,
                Senha = request.Senha, // ⚠️ ideal seria hashear antes de salvar
                Cpf = request.Cpf,
                Cargo = request.Cargo,
                DataAdmissao = request.dataAdmissao ?? DateTime.UtcNow,
                PatioId = request.PatioId
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
        }

        // PUT: api/usuario/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] Usuario request)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            usuario.Nome = request.Nome;
            usuario.Email = request.Email;
            usuario.Senha = request.Senha;
            usuario.Cpf = request.Cpf;
            usuario.Cargo = request.Cargo;
            usuario.DataAdmissao = request.DataAdmissao;
            usuario.PatioId = request.PatioId;

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/usuario/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
