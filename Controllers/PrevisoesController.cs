using Agrosphere.Data;
using Agrosphere.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agrosphere.Controllers;

[ApiController]
[Route("api/previsoes")]
public class PrevisoesController : ControllerBase
{
    private readonly LifePetDbContext _context;

    public PrevisoesController(LifePetDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Previsao>>> GetPrevisoes()
    {
        return await _context.Previsoes.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Previsao>> GetPrevisao(int id)
    {
        var previsao = await _context.Previsoes.FindAsync(id);

        if (previsao == null)
            return NotFound();

        return previsao;
    }

    [HttpGet("sensor/{sensorId}")]
    public async Task<ActionResult<IEnumerable<Previsao>>> GetPrevisoesPorSensor(int sensorId)
    {
        return await _context.Previsoes
            .Where(p => p.SensorId == sensorId)
            .OrderByDescending(p => p.DataPrevisao)
            .ToListAsync();
    }

    [HttpGet("sensor/{sensorId}/vigentes")]
    public async Task<ActionResult<IEnumerable<Previsao>>> GetPrevisoesVigentesPorSensor(int sensorId)
    {
        var agora = DateTime.Now;
        return await _context.Previsoes
            .Where(p => p.SensorId == sensorId && p.DataVigenciaAte >= agora)
            .OrderByDescending(p => p.DataPrevisao)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Previsao>> PostPrevisao(Previsao previsao)
    {
        previsao.DataPrevisao = DateTime.Now;
        _context.Previsoes.Add(previsao);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPrevisao), new { id = previsao.Id }, previsao);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPrevisao(int id, Previsao previsao)
    {
        if (id != previsao.Id)
            return BadRequest();

        _context.Entry(previsao).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PrevisaoExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePrevisao(int id)
    {
        var previsao = await _context.Previsoes.FindAsync(id);
        if (previsao == null)
            return NotFound();

        _context.Previsoes.Remove(previsao);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PrevisaoExists(int id)
    {
        return _context.Previsoes.Any(p => p.Id == id);
    }
}
