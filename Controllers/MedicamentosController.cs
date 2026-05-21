using LifePetApi.Data;
using LifePetApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LifePetApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MedicamentosController : ControllerBase
{
    private readonly LifePetDbContext _db;
    public MedicamentosController(LifePetDbContext db) => _db = db;

    /// <summary>Lista todos os medicamentos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Medicamento>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Medicamento>>> GetAll()
        => Ok(await _db.Medicamentos.AsNoTracking().ToListAsync());

    /// <summary>Busca medicamento por ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Medicamento), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Medicamento>> GetById(int id)
    {
        var med = await _db.Medicamentos.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        return med is null ? NotFound() : Ok(med);
    }

    /// <summary>Lista medicamentos de um pet.</summary>
    [HttpGet("pet/{petId:int}")]
    [ProducesResponseType(typeof(IEnumerable<Medicamento>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Medicamento>>> GetByPet(int petId)
    {
        if (!await _db.Pets.AnyAsync(p => p.Id == petId))
            return NotFound();

        var meds = await _db.Medicamentos.AsNoTracking()
            .Where(m => m.PetId == petId)
            .ToListAsync();
        return Ok(meds);
    }

    /// <summary>Lista medicamentos em uso (sem data fim ou fim futura).</summary>
    [HttpGet("ativos")]
    [ProducesResponseType(typeof(IEnumerable<Medicamento>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Medicamento>>> GetAtivos()
    {
        var hoje = DateTime.Today;
        var ativos = await _db.Medicamentos.AsNoTracking()
            .Where(m => m.DataFim == null || m.DataFim >= hoje)
            .ToListAsync();
        return Ok(ativos);
    }

    ///Registra um novo medicamento.
    [HttpPost]
    [ProducesResponseType(typeof(Medicamento), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Medicamento>> Create([FromBody] Medicamento medicamento)
    {
        if (string.IsNullOrWhiteSpace(medicamento.Nome))
            return BadRequest("Nome é obrigatório.");
        if (!await _db.Pets.AnyAsync(p => p.Id == medicamento.PetId))
            return BadRequest("PetId inválido.");
        _db.Medicamentos.Add(medicamento);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = medicamento.Id }, medicamento);
    }

    ///Atualiza um medicamento.
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] Medicamento medicamento)
    {
        if (id != medicamento.Id)
            return BadRequest("ID da rota difere do corpo da requisição.");
        var existing = await _db.Medicamentos.FindAsync(id);
        if (existing is null)
            return NotFound();
        existing.Nome = medicamento.Nome;
        existing.Dosagem = medicamento.Dosagem;
        existing.Frequencia = medicamento.Frequencia;
        existing.DataInicio = medicamento.DataInicio;
        existing.DataFim = medicamento.DataFim;
        existing.PetId = medicamento.PetId;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    ///Remove um medicamento.
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var med = await _db.Medicamentos.FindAsync(id);
        if (med is null)
            return NotFound();
        _db.Medicamentos.Remove(med);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
