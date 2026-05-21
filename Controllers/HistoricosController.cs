using LifePetApi.Data;
using LifePetApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LifePetApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class HistoricosController : ControllerBase
{
    private readonly LifePetDbContext _db;

    public HistoricosController(LifePetDbContext db) => _db = db;

    /// <summary>Lista todo o histórico clínico.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Historico>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Historico>>> GetAll()
        => Ok(await _db.Historicos.AsNoTracking().OrderByDescending(h => h.Data).ToListAsync());

    /// <summary>Busca registro por ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Historico), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Historico>> GetById(int id)
    {
        var historico = await _db.Historicos.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
        return historico is null ? NotFound() : Ok(historico);
    }

    /// <summary>Lista histórico de um pet.</summary>
    [HttpGet("pet/{petId:int}")]
    [ProducesResponseType(typeof(IEnumerable<Historico>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Historico>>> GetByPet(int petId)
    {
        if (!await _db.Pets.AnyAsync(p => p.Id == petId))
            return NotFound();

        var historicos = await _db.Historicos.AsNoTracking()
            .Where(h => h.PetId == petId)
            .OrderByDescending(h => h.Data)
            .ToListAsync();
        return Ok(historicos);
    }

    /// <summary>Lista histórico por tipo (exame, cirurgia, observação).</summary>
    [HttpGet("tipo/{tipo}")]
    [ProducesResponseType(typeof(IEnumerable<Historico>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Historico>>> GetByTipo(string tipo)
    {
        var historicos = await _db.Historicos.AsNoTracking()
            .Where(h => h.Tipo == tipo)
            .OrderByDescending(h => h.Data)
            .ToListAsync();
        return Ok(historicos);
    }

    /// <summary>Adiciona um registro ao histórico.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(Historico), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Historico>> Create([FromBody] Historico historico)
    {
        if (string.IsNullOrWhiteSpace(historico.Descricao))
            return BadRequest("Descrição é obrigatória.");

        if (!await _db.Pets.AnyAsync(p => p.Id == historico.PetId))
            return BadRequest("PetId inválido.");

        _db.Historicos.Add(historico);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = historico.Id }, historico);
    }

    /// <summary>Atualiza um registro do histórico.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] Historico historico)
    {
        if (id != historico.Id)
            return BadRequest("ID da rota difere do corpo da requisição.");

        var existing = await _db.Historicos.FindAsync(id);
        if (existing is null)
            return NotFound();

        existing.Data = historico.Data;
        existing.Descricao = historico.Descricao;
        existing.Tipo = historico.Tipo;
        existing.PetId = historico.PetId;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Remove um registro do histórico.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var historico = await _db.Historicos.FindAsync(id);
        if (historico is null)
            return NotFound();

        _db.Historicos.Remove(historico);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
