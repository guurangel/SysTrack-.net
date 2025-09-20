using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SysTrack.Infrastructure.Contexts;
using SysTrack.Infrastructure.Persistence;
using SysTrack.DTO.Request;
using SysTrack.DTO.Response;
using SysTrack.Infrastructure.Extensions;
using SysTrack.Services;

namespace SysTrack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotocicletaController : ControllerBase
    {
        private readonly SysTrackDbContext _context;
        private readonly MotocicletaService _service;

        public MotocicletaController(SysTrackDbContext context, MotocicletaService service)
        {
            _context = context;
            _service = service;
        }

        // GET: api/motocicleta?pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<ActionResult<PagedResponse<MotocicletaResponse>>> GetAll(
            [FromQuery] MotocicletaFiltroRequest filtro,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.Motocicletas.Include(m => m.Patio).AplicarFiltros(filtro);
            var totalItems = await query.CountAsync();

            var motos = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = motos.Select(m => new MotocicletaResponse
            {
                Id = m.Id,
                Placa = m.Placa,
                Marca = m.Marca,
                Modelo = m.Modelo,
                Cor = m.Cor,
                DataEntrada = m.DataEntrada,
                PatioId = m.PatioId,
                PatioNome = m.Patio.Nome,
                Links = new List<Link>
                {
                    new Link { Href = Url.Action(nameof(GetById), new { id = m.Id })!, Rel = "self", Method = "GET" },
                    new Link { Href = Url.Action(nameof(Update), new { id = m.Id })!, Rel = "update", Method = "PUT" },
                    new Link { Href = Url.Action(nameof(Delete), new { id = m.Id })!, Rel = "delete", Method = "DELETE" }
                }
            }).ToList();

            return Ok(new PagedResponse<MotocicletaResponse>(response, pageNumber, pageSize, totalItems));
        }

        // GET: api/motocicleta/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MotocicletaResponse>> GetById(Guid id)
        {
            var m = await _context.Motocicletas.Include(m => m.Patio).FirstOrDefaultAsync(m => m.Id == id);
            if (m == null) return NotFound();

            var response = new MotocicletaResponse
            {
                Id = m.Id,
                Placa = m.Placa,
                Marca = m.Marca,
                Modelo = m.Modelo,
                Cor = m.Cor,
                DataEntrada = m.DataEntrada,
                PatioId = m.PatioId,
                PatioNome = m.Patio.Nome,
                Links = new List<Link>
                {
                    new Link { Href = Url.Action(nameof(GetById), new { id = m.Id })!, Rel = "self", Method = "GET" },
                    new Link { Href = Url.Action(nameof(Update), new { id = m.Id })!, Rel = "update", Method = "PUT" },
                    new Link { Href = Url.Action(nameof(Delete), new { id = m.Id })!, Rel = "delete", Method = "DELETE" }
                }
            };

            return Ok(response);
        }

        // POST: api/motocicleta
        [HttpPost]
        public async Task<ActionResult<MotocicletaResponse>> Create([FromBody] MotocicletaRequest request)
        {
            if (!await _service.PodeAdicionarMotocicletaAsync(request.PatioId))
                return BadRequest("Capacidade máxima do pátio atingida.");

            var moto = new Motocicleta
            {
                Id = Guid.NewGuid(),
                Placa = request.Placa,
                Marca = request.Marca,
                Modelo = request.Modelo,
                Cor = request.Cor,
                DataEntrada = DateTime.UtcNow,
                PatioId = request.PatioId
            };

            _context.Motocicletas.Add(moto);
            await _context.SaveChangesAsync();

            var response = new MotocicletaResponse
            {
                Id = moto.Id,
                Placa = moto.Placa,
                Marca = moto.Marca,
                Modelo = moto.Modelo,
                Cor = moto.Cor,
                DataEntrada = moto.DataEntrada,
                PatioId = moto.PatioId,
                PatioNome = (await _context.Patios.FindAsync(moto.PatioId))!.Nome,
                Links = new List<Link>
                {
                    new Link { Href = Url.Action(nameof(GetById), new { id = moto.Id })!, Rel = "self", Method = "GET" }
                }
            };

            return CreatedAtAction(nameof(GetById), new { id = moto.Id }, response);
        }

        // PUT: api/motocicleta/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] AtualizarMotocicletaRequest request)
        {
            var moto = await _context.Motocicletas.FindAsync(id);
            if (moto == null) return NotFound();

            if (moto.PatioId != request.PatioId)
            {
                if (!await _service.PodeAdicionarMotocicletaAsync(request.PatioId))
                    return BadRequest("Capacidade máxima do pátio de destino atingida.");
            }

            moto.Marca = request.Marca;
            moto.Modelo = request.Modelo;
            moto.Cor = request.Cor;
            moto.PatioId = request.PatioId;

            _context.Motocicletas.Update(moto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/motocicleta/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var moto = await _context.Motocicletas.FindAsync(id);
            if (moto == null) return NotFound();

            _context.Motocicletas.Remove(moto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
