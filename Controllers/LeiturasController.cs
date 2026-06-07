using Agrosphere.Data;
using Agrosphere.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agrosphere.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeiturasController : ControllerBase
{
    private readonly LifePetDbContext _context;

    public LeiturasController(LifePetDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Leitura>>> GetLeituras()
    {
        return await _context.Leituras.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Leitura>> GetLeitura(int id)
    {
        var leitura = await _context.Leituras.FindAsync(id);

        if (leitura == null)
            return NotFound();

        return leitura;
    }

    [HttpGet("sensor/{sensorId}")]
    public async Task<ActionResult<IEnumerable<Leitura>>> GetLeiturasPorSensor(int sensorId)
    {
        return await _context.Leituras
            .Where(l => l.SensorId == sensorId)
            .OrderByDescending(l => l.DataHora)
            .ToListAsync();
    }

    [HttpGet("sensor/{sensorId}/ultimas/{quantidade}")]
    public async Task<ActionResult<IEnumerable<Leitura>>> GetUltimasLeituras(int sensorId, int quantidade)
    {
        return await _context.Leituras
            .Where(l => l.SensorId == sensorId)
            .OrderByDescending(l => l.DataHora)
            .Take(quantidade)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Leitura>> PostLeitura(Leitura leitura)
    {
        _context.Leituras.Add(leitura);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLeitura), new { id = leitura.Id }, leitura);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutLeitura(int id, Leitura leitura)
    {
        if (id != leitura.Id)
            return BadRequest();

        _context.Entry(leitura).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!LeituraExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLeitura(int id)
    {
        var leitura = await _context.Leituras.FindAsync(id);
        if (leitura == null)
            return NotFound();

        _context.Leituras.Remove(leitura);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool LeituraExists(int id)
    {
        return _context.Leituras.Any(l => l.Id == id);
    }
}
