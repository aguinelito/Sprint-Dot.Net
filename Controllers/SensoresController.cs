using Agrosphere.Data;
using Agrosphere.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agrosphere.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SensoresController : ControllerBase
{
    private readonly LifePetDbContext _context;

    public SensoresController(LifePetDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Sensor>>> GetSensores()
    {
        return await _context.Sensores
            .Include(s => s.Leituras)
            .Include(s => s.AlertasClimaticos)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Sensor>> GetSensor(int id)
    {
        var sensor = await _context.Sensores
            .Include(s => s.Leituras)
            .Include(s => s.AlertasClimaticos)
            .Include(s => s.Previsoes)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (sensor == null)
            return NotFound();

        return sensor;
    }

    [HttpGet("fazenda/{fazendaId}")]
    public async Task<ActionResult<IEnumerable<Sensor>>> GetSensoresPorFazenda(int fazendaId)
    {
        return await _context.Sensores
            .Where(s => s.FazendaId == fazendaId)
            .Include(s => s.Leituras)
            .Include(s => s.AlertasClimaticos)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Sensor>> PostSensor(Sensor sensor)
    {
        _context.Sensores.Add(sensor);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSensor), new { id = sensor.Id }, sensor);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutSensor(int id, Sensor sensor)
    {
        if (id != sensor.Id)
            return BadRequest();

        _context.Entry(sensor).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SensorExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSensor(int id)
    {
        var sensor = await _context.Sensores.FindAsync(id);
        if (sensor == null)
            return NotFound();

        _context.Sensores.Remove(sensor);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool SensorExists(int id)
    {
        return _context.Sensores.Any(s => s.Id == id);
    }
}
