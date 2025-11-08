using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using SysTrack.DTO.Request;
using SysTrack.DTO.Response;
using SysTrack.Infrastructure.Contexts;
using SysTrack.Infrastructure.Extensions;
using SysTrack.Infrastructure.Persistance.Enums;
using SysTrack.Infrastructure.Persistence;
using SysTrack.ML;
using SysTrack.Services;

namespace SysTrack.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class MotocicletaController : ControllerBase
    {
        private readonly SysTrackDbContext _context;
        private readonly MotocicletaService _service;
        private readonly ManutencaoPredictionService _predictionService;

        public MotocicletaController(SysTrackDbContext context, MotocicletaService service, ManutencaoPredictionService predictionService)
        {
            _context = context;
            _service = service;
            _predictionService = predictionService;
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
                Ano = m.Ano,
                Quilometragem = m.Quilometragem,
                Status = m.Status,
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
                Ano = m.Ano,
                Quilometragem = m.Quilometragem,
                Status = m.Status,
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
            try
            {
                var moto = new Motocicleta
                {
                    Id = Guid.NewGuid(),
                    Placa = request.Placa.Trim().ToUpper(),
                    Marca = request.Marca.Trim(),
                    Modelo = request.Modelo.Trim(),
                    Cor = request.Cor.Trim(),
                    DataEntrada = DateTime.UtcNow,
                    PatioId = request.PatioId,
                    Ano = request.Ano,
                    Quilometragem = request.Quilometragem,
                    Status = request.Status
                };

                var criada = await _service.CriarMotocicletaAsync(moto);

                var patio = await _context.Patios.FindAsync(criada.PatioId);

                var response = new MotocicletaResponse
                {
                    Id = criada.Id,
                    Placa = criada.Placa,
                    Marca = criada.Marca,
                    Modelo = criada.Modelo,
                    Cor = criada.Cor,
                    DataEntrada = criada.DataEntrada,
                    PatioId = criada.PatioId,
                    PatioNome = patio?.Nome ?? "Pátio desconhecido",
                    Ano = criada.Ano,
                    Quilometragem = criada.Quilometragem,
                    Status = criada.Status,
                    Links = new List<Link>
                    {
                        new Link { Href = Url.Action(nameof(GetById), new { id = criada.Id })!, Rel = "self", Method = "GET" }
                    }
                };

                return CreatedAtAction(nameof(GetById), new { id = criada.Id }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Erro = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Erro = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Erro = $"Erro interno: {ex.Message}" });
            }
        }

        // PUT: api/motocicleta/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] AtualizarMotocicletaRequest request)
        {
            var moto = await _context.Motocicletas.FindAsync(id);
            if (moto == null) return NotFound("Motocicleta não encontrada.");

            try
            {
                if (moto.PatioId != request.PatioId)
                {
                    if (!await _service.PodeAdicionarMotocicletaAsync(request.PatioId))
                        return BadRequest("Capacidade máxima do pátio de destino atingida.");
                }

                moto.Marca = request.Marca.Trim();
                moto.Modelo = request.Modelo.Trim();
                moto.Cor = request.Cor.Trim();
                moto.PatioId = request.PatioId;
                moto.Ano = request.Ano;
                moto.Quilometragem = request.Quilometragem;
                moto.Status = request.Status;

                // reaproveita a validação do service
                await _service.CriarMotocicletaAsync(moto); // apenas validação, não será realmente recriada
                _context.Motocicletas.Update(moto);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Erro = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Erro = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Erro = $"Erro interno: {ex.Message}" });
            }
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

        [HttpPost("prever-manutencao")]
        public ActionResult<object> PreverManutencao([FromBody] ManutencaoInput input) // ✅ tipo correto
        {
            // Calcula a idade automaticamente, caso não venha no JSON
            if (input.IdadeMoto <= 0)
                input.IdadeMoto = DateTime.UtcNow.Year - input.Ano;

            var resultado = _predictionService.PreverManutencao(input);

            return Ok(new
            {
                input.Marca,
                input.Modelo,
                input.Quilometragem,
                input.Ano,
                input.IdadeMoto,
                resultado.NecessitaManutencao,
                Probabilidade = $"{resultado.Probability * 100:F1}%", // <-- formatado
                StatusSugerido = resultado.NecessitaManutencao ? "MANUTENCAO" : "FUNCIONAL"
            });
        }

    }
}
