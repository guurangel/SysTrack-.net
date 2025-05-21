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
    public class MotocicletaController : ControllerBase
    {
        private readonly SysTrackDbContext _context;

        public MotocicletaController(SysTrackDbContext context)
        {
            _context = context;
        }

        // GET: api/motocicleta
        [HttpGet]
        public async Task<ActionResult<List<MotocicletaResponse>>> GetAll()
        {
            var motos = await _context.Motocicletas
                .Include(m => m.Patio)
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
                PatioNome = m.Patio.Nome
            }).ToList();

            return Ok(response);
        }

        // GET: api/motocicleta/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MotocicletaResponse>> GetById(Guid id)
        {
            var m = await _context.Motocicletas
                .Include(m => m.Patio)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (m == null)
                return NotFound();

            var response = new MotocicletaResponse
            {
                Id = m.Id,
                Placa = m.Placa,
                Marca = m.Marca,
                Modelo = m.Modelo,
                Cor = m.Cor,
                DataEntrada = m.DataEntrada,
                PatioId = m.PatioId,
                PatioNome = m.Patio.Nome
            };

            return Ok(response);
        }

        // POST: api/motocicleta
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] MotocicletaRequest request)
        {
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

            return CreatedAtAction(nameof(GetById), new { id = moto.Id }, moto);
        }

        // PUT: api/motocicleta/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] MotocicletaRequest request)
        {
            var moto = await _context.Motocicletas.FindAsync(id);
            if (moto == null)
                return NotFound();

            moto.Placa = request.Placa;
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
            if (moto == null)
                return NotFound();

            _context.Motocicletas.Remove(moto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
