using LifePetApi.Data;
using LifePetApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LifePetApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TutoresController : ControllerBase
{
    private readonly LifePetDbContext _db;

    public TutoresController(LifePetDbContext db) => _db = db;

    /// <summary>lista os tutores.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Tutor>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Tutor>>> GetAll()
        => Ok(await _db.Tutores.AsNoTracking().ToListAsync());

    /// <summary>buscar por id</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Tutor), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Tutor>> GetById(int id)
    {
        var tutor = await _db.Tutores.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        return tutor is null ? NotFound() : Ok(tutor);
    }

    /// <summary>busca os tutores via email</summary>
    [HttpGet("email/{email}")]
    [ProducesResponseType(typeof(Tutor), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Tutor>> GetByEmail(string email)
    {
        var tutor = await _db.Tutores.AsNoTracking()
            .FirstOrDefaultAsync(t => t.Email == email);
        return tutor is null ? NotFound() : Ok(tutor);
    }

    /// <summary>lista os pets de um tutor</summary>
    [HttpGet("{tutorId:int}/pets")]
    [ProducesResponseType(typeof(IEnumerable<Pet>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Pet>>> GetPets(int tutorId)
    {
        if (!await _db.Tutores.AnyAsync(t => t.Id == tutorId))
            return NotFound();

        var pets = await _db.Pets.AsNoTracking()
            .Where(p => p.TutorId == tutorId)
            .ToListAsync();
        return Ok(pets);
    }

    /// <summary>Cria um novo tutor.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(Tutor), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Tutor>> Create([FromBody] Tutor tutor)
    {
        if (string.IsNullOrWhiteSpace(tutor.Nome) || string.IsNullOrWhiteSpace(tutor.Email))
            return BadRequest("Nome e Email são obrigatórios.");

        _db.Tutores.Add(tutor);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = tutor.Id }, tutor);
    }

    /// <summary>atualizado dados de um tutor que ja tem no sistema</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] Tutor tutor)
    {
        if (id != tutor.Id)
            return BadRequest("ID da rota difere do corpo da requisição.");

        var existing = await _db.Tutores.FindAsync(id);
        if (existing is null)
            return NotFound();
        existing.Nome = tutor.Nome;
        existing.Email = tutor.Email;
        existing.Telefone = tutor.Telefone;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>deleta um tutor do sistema</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var tutor = await _db.Tutores.FindAsync(id);
        if (tutor is null)
            return NotFound();
        _db.Tutores.Remove(tutor);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
