using Agrosphere.Data;
using Agrosphere.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agrosphere.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlertasClimaticosController : ControllerBase
{
    private readonly LifePetDbContext _context;

    public AlertasClimaticosController(LifePetDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AlertaClimatico>>> GetAlertas()
    {
        return await _context.AlertasClimaticos
            .Where(a => a.DataResolucao == null)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AlertaClimatico>> GetAlerta(int id)
    {
        var alerta = await _context.AlertasClimaticos.FindAsync(id);

        if (alerta == null)
            return NotFound();

        return alerta;
    }

    [HttpGet("sensor/{sensorId}")]
    public async Task<ActionResult<IEnumerable<AlertaClimatico>>> GetAlertasPorSensor(int sensorId)
    {
        return await _context.AlertasClimaticos
            .Where(a => a.SensorId == sensorId)
            .OrderByDescending(a => a.DataAlerta)
            .ToListAsync();
    }

    [HttpGet("sensor/{sensorId}/ativos")]
    public async Task<ActionResult<IEnumerable<AlertaClimatico>>> GetAlertasAtivosPorSensor(int sensorId)
    {
        return await _context.AlertasClimaticos
            .Where(a => a.SensorId == sensorId && a.DataResolucao == null)
            .OrderByDescending(a => a.DataAlerta)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<AlertaClimatico>> PostAlerta(AlertaClimatico alerta)
    {
        alerta.DataAlerta = DateTime.Now;
        _context.AlertasClimaticos.Add(alerta);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAlerta), new { id = alerta.Id }, alerta);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAlerta(int id, AlertaClimatico alerta)
    {
        if (id != alerta.Id)
            return BadRequest();

        _context.Entry(alerta).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AlertaExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpPut("{id}/resolver")]
    public async Task<IActionResult> ResolverAlerta(int id)
    {
        var alerta = await _context.AlertasClimaticos.FindAsync(id);
        if (alerta == null)
            return NotFound();

        alerta.DataResolucao = DateTime.Now;
        _context.Entry(alerta).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAlerta(int id)
    {
        var alerta = await _context.AlertasClimaticos.FindAsync(id);
        if (alerta == null)
            return NotFound();

        _context.AlertasClimaticos.Remove(alerta);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AlertaExists(int id)
    {
        return _context.AlertasClimaticos.Any(a => a.Id == id);
    }
}
