using LifePetApi.Data;
using LifePetApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LifePetApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ConsultasController : ControllerBase
{
    private readonly LifePetDbContext _db;

    public ConsultasController(LifePetDbContext db) => _db = db;

    /// <summary>Lista todas as consultas.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Consulta>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Consulta>>> GetAll()
        => Ok(await _db.Consultas.AsNoTracking().ToListAsync());

    /// <summary>Busca consulta por ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Consulta), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Consulta>> GetById(int id)
    {
        var consulta = await _db.Consultas.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        return consulta is null ? NotFound() : Ok(consulta);
    }

    /// <summary>Lista consultas de um pet.</summary>
    [HttpGet("pet/{petId:int}")]
    [ProducesResponseType(typeof(IEnumerable<Consulta>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Consulta>>> GetByPet(int petId)
    {
        if (!await _db.Pets.AnyAsync(p => p.Id == petId))
            return NotFound();

        var consultas = await _db.Consultas.AsNoTracking()
            .Where(c => c.PetId == petId)
            .OrderBy(c => c.DataHora)
            .ToListAsync();
        return Ok(consultas);
    }

    /// <summary>Lista consultas futuras (agendadas).</summary>
    [HttpGet("futuras")]
    [ProducesResponseType(typeof(IEnumerable<Consulta>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Consulta>>> GetFuturas()
    {
        var agora = DateTime.Now;
        var futuras = await _db.Consultas.AsNoTracking()
            .Where(c => c.DataHora >= agora)
            .OrderBy(c => c.DataHora)
            .ToListAsync();
        return Ok(futuras);
    }

    /// <summary>Agenda uma nova consulta.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(Consulta), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Consulta>> Create([FromBody] Consulta consulta)
    {
        if (string.IsNullOrWhiteSpace(consulta.Motivo))
            return BadRequest("Motivo é obrigatório.");

        if (!await _db.Pets.AnyAsync(p => p.Id == consulta.PetId))
            return BadRequest("PetId inválido.");

        _db.Consultas.Add(consulta);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = consulta.Id }, consulta);
    }

    /// <summary>Atualiza uma consulta.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] Consulta consulta)
    {
        if (id != consulta.Id)
            return BadRequest("ID da rota difere do corpo da requisição.");

        var existing = await _db.Consultas.FindAsync(id);
        if (existing is null)
            return NotFound();

        existing.DataHora = consulta.DataHora;
        existing.Veterinario = consulta.Veterinario;
        existing.Motivo = consulta.Motivo;
        existing.PetId = consulta.PetId;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Remove uma consulta.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var consulta = await _db.Consultas.FindAsync(id);
        if (consulta is null)
            return NotFound();

        _db.Consultas.Remove(consulta);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
