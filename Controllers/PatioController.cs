using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SysTrack.Infrastructure.Contexts;
using SysTrack.Infrastructure.Persistence;
using SysTrack.DTO.Request;
using SysTrack.DTO.Response;

namespace SysTrack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatioController : ControllerBase
    {
        private readonly SysTrackDbContext _context;

        public PatioController(SysTrackDbContext context)
        {
            _context = context;
        }

        // GET: api/patio
        [HttpGet]
        public async Task<ActionResult<List<PatioResponse>>> GetAll()
        {
            var patios = await _context.Patios
                .Include(p => p.Motocicletas)
                .ToListAsync();

            var response = patios.Select(p => new PatioResponse
            {
                Id = p.Id,
                Nome = p.Nome,
                Endereco = p.Endereco,
                CapacidadeMaxima = p.CapacidadeMaxima,
                DataCriacao = p.DataCriacao,
                Motocicletas = p.Motocicletas.Select(m => new MotocicletaResponse
                {
                    Id = m.Id,
                    Placa = m.Placa,
                    Marca = m.Marca,
                    Modelo = m.Modelo,
                    Cor = m.Cor,
                    DataEntrada = m.DataEntrada,
                    PatioId = m.PatioId,
                    PatioNome = p.Nome
                }).ToList(),
                Usuarios = p.Usuarios.Select(u => new UsuarioResponse
                {
                    Id =u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    Cpf = u.Cpf,
                    DataAdmissao = u.DataAdmissao,
                    Cargo = u.Cargo,
                    PatioId = u.PatioId,
                    PatioNome = p.Nome
                }).ToList()
            }).ToList();

            return Ok(response);
        }

        // GET: api/patio/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PatioResponse>> GetById(Guid id)
        {
            var p = await _context.Patios
                .Include(p => p.Motocicletas)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (p == null)
                return NotFound();

            var response = new PatioResponse
            {
                Id = p.Id,
                Nome = p.Nome,
                Endereco = p.Endereco,
                CapacidadeMaxima = p.CapacidadeMaxima,
                DataCriacao = p.DataCriacao,
                Motocicletas = p.Motocicletas.Select(m => new MotocicletaResponse
                {
                    Id = m.Id,
                    Placa = m.Placa,
                    Marca = m.Marca,
                    Modelo = m.Modelo,
                    Cor = m.Cor,
                    DataEntrada = m.DataEntrada,
                    PatioId = m.PatioId,
                    PatioNome = p.Nome
                }).ToList()
            };

            return Ok(response);
        }

        // POST: api/patio
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] PatioRequest request)
        {
            var patio = new Patio
            {
                Id = Guid.NewGuid(),
                Nome = request.Nome,
                Endereco = request.Endereco,
                CapacidadeMaxima = request.CapacidadeMaxima,
                DataCriacao = DateTime.UtcNow
            };

            _context.Patios.Add(patio);
            await _context.SaveChangesAsync();

            var response = new PatioResponse
            {
                Id = patio.Id,
                Nome = patio.Nome,
                Endereco = patio.Endereco,
                CapacidadeMaxima = patio.CapacidadeMaxima,
                DataCriacao = patio.DataCriacao,
                Motocicletas = new List<MotocicletaResponse>()
            };

            return CreatedAtAction(nameof(GetById), new { id = patio.Id }, response);
        }

        // PUT: api/patio/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] PatioRequest request)
        {
            var patio = await _context.Patios.FindAsync(id);
            if (patio == null)
                return NotFound();

            patio.Nome = request.Nome;
            patio.Endereco = request.Endereco;
            patio.CapacidadeMaxima = request.CapacidadeMaxima;

            _context.Patios.Update(patio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/patio/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var patio = await _context.Patios.FindAsync(id);
            if (patio == null)
                return NotFound();

            _context.Patios.Remove(patio);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
