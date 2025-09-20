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

        // GET: api/usuario?pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<ActionResult<PagedResponse<UsuarioResponse>>> GetAll(
            [FromQuery] UsuarioFiltroRequest filtro,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.Usuarios.Include(u => u.Patio).AplicarFiltros(filtro);
            var totalItems = await query.CountAsync();

            var usuarios = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = usuarios.Select(u => new UsuarioResponse
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                Cpf = u.Cpf,
                Cargo = u.Cargo,
                DataAdmissao = u.DataAdmissao,
                PatioId = u.PatioId,
                PatioNome = u.Patio.Nome,
                Links = new List<Link>
                {
                    new Link { Href = Url.Action(nameof(GetById), new { id = u.Id })!, Rel = "self", Method = "GET" },
                    new Link { Href = Url.Action(nameof(Update), new { id = u.Id })!, Rel = "update", Method = "PUT" },
                    new Link { Href = Url.Action(nameof(Delete), new { id = u.Id })!, Rel = "delete", Method = "DELETE" }
                }
            }).ToList();

            return Ok(new PagedResponse<UsuarioResponse>(response, pageNumber, pageSize, totalItems));
        }

        // GET: api/usuario/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UsuarioResponse>> GetById(Guid id)
        {
            var u = await _context.Usuarios.Include(u => u.Patio).FirstOrDefaultAsync(u => u.Id == id);
            if (u == null) return NotFound();

            var response = new UsuarioResponse
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                Cpf = u.Cpf,
                Cargo = u.Cargo,
                DataAdmissao = u.DataAdmissao,
                PatioId = u.PatioId,
                PatioNome = u.Patio.Nome,
                Links = new List<Link>
                {
                    new Link { Href = Url.Action(nameof(GetById), new { id = u.Id })!, Rel = "self", Method = "GET" },
                    new Link { Href = Url.Action(nameof(Update), new { id = u.Id })!, Rel = "update", Method = "PUT" },
                    new Link { Href = Url.Action(nameof(Delete), new { id = u.Id })!, Rel = "delete", Method = "DELETE" }
                }
            };

            return Ok(response);
        }

        // POST: api/usuario
        [HttpPost]
        public async Task<ActionResult<UsuarioResponse>> Create([FromBody] UsuarioRequest request)
        {
            var u = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = request.Nome,
                Email = request.Email,
                Senha = request.Senha,
                Cpf = request.Cpf,
                Cargo = request.Cargo,
                DataAdmissao = request.dataAdmissao ?? DateTime.UtcNow,
                PatioId = request.PatioId
            };

            _context.Usuarios.Add(u);
            await _context.SaveChangesAsync();

            var response = new UsuarioResponse
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                Cpf = u.Cpf,
                Cargo = u.Cargo,
                DataAdmissao = u.DataAdmissao,
                PatioId = u.PatioId,
                PatioNome = (await _context.Patios.FindAsync(u.PatioId))!.Nome,
                Links = new List<Link>
                {
                    new Link { Href = Url.Action(nameof(GetById), new { id = u.Id })!, Rel = "self", Method = "GET" }
                }
            };

            return CreatedAtAction(nameof(GetById), new { id = u.Id }, response);
        }

        // PUT: api/usuario/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UsuarioRequest request)
        {
            var u = await _context.Usuarios.FindAsync(id);
            if (u == null) return NotFound();

            u.Nome = request.Nome;
            u.Email = request.Email;
            u.Senha = request.Senha;
            u.Cpf = request.Cpf;
            u.Cargo = request.Cargo;
            u.DataAdmissao = request.dataAdmissao ?? u.DataAdmissao;
            u.PatioId = request.PatioId;

            _context.Usuarios.Update(u);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/usuario/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var u = await _context.Usuarios.FindAsync(id);
            if (u == null) return NotFound();

            _context.Usuarios.Remove(u);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
