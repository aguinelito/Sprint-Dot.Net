using LifePetApi.Data;
using LifePetApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LifePetApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class VacinasController : ControllerBase
{
    private readonly LifePetDbContext _db;

    public VacinasController(LifePetDbContext db) => _db = db;

    /// <summary>Lista todas as vacinas.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Vacina>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Vacina>>> GetAll()
        => Ok(await _db.Vacinas.AsNoTracking().ToListAsync());

    /// <summary>Busca vacina por ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Vacina), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Vacina>> GetById(int id)
    {
        var vacina = await _db.Vacinas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
        return vacina is null ? NotFound() : Ok(vacina);
    }

    /// <summary>Lista vacinas de um pet.</summary>
    [HttpGet("pet/{petId:int}")]
    [ProducesResponseType(typeof(IEnumerable<Vacina>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Vacina>>> GetByPet(int petId)
    {
        if (!await _db.Pets.AnyAsync(p => p.Id == petId))
            return NotFound();

        var vacinas = await _db.Vacinas.AsNoTracking()
            .Where(v => v.PetId == petId)
            .ToListAsync();
        return Ok(vacinas);
    }

    /// <summary>Lista vacinas com dose em atraso (alerta).</summary>
    [HttpGet("atrasadas")]
    [ProducesResponseType(typeof(IEnumerable<Vacina>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Vacina>>> GetAtrasadas()
    {
        var hoje = DateTime.Today;
        var atrasadas = await _db.Vacinas.AsNoTracking()
            .Where(v => v.DataProximaDose != null && v.DataProximaDose < hoje)
            .ToListAsync();
        return Ok(atrasadas);
    }

    /// <summary>Registra uma nova vacina.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(Vacina), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Vacina>> Create([FromBody] Vacina vacina)
    {
        if (string.IsNullOrWhiteSpace(vacina.Nome))
            return BadRequest("Nome é obrigatório.");

        if (!await _db.Pets.AnyAsync(p => p.Id == vacina.PetId))
            return BadRequest("PetId inválido.");

        _db.Vacinas.Add(vacina);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = vacina.Id }, vacina);
    }

    /// <summary>Atualiza uma vacina.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] Vacina vacina)
    {
        if (id != vacina.Id)
            return BadRequest("ID da rota difere do corpo da requisição.");

        var existing = await _db.Vacinas.FindAsync(id);
        if (existing is null)
            return NotFound();

        existing.Nome = vacina.Nome;
        existing.DataAplicacao = vacina.DataAplicacao;
        existing.DataProximaDose = vacina.DataProximaDose;
        existing.PetId = vacina.PetId;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Remove uma vacina.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var vacina = await _db.Vacinas.FindAsync(id);
        if (vacina is null)
            return NotFound();

        _db.Vacinas.Remove(vacina);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
