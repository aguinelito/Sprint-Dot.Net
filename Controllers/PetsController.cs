using LifePetApi.Data;
using LifePetApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LifePetApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PetsController : ControllerBase
{
    private readonly LifePetDbContext _db;

    public PetsController(LifePetDbContext db) => _db = db;

    /// <summary>Lista todos os pets.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Pet>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Pet>>> GetAll()
        => Ok(await _db.Pets.AsNoTracking().ToListAsync());

    /// <summary>Busca pet por ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Pet), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Pet>> GetById(int id)
    {
        var pet = await _db.Pets.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        return pet is null ? NotFound() : Ok(pet);
    }

    /// <summary>Lista pets por espécie.</summary>
    [HttpGet("especie/{especie}")]
    [ProducesResponseType(typeof(IEnumerable<Pet>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Pet>>> GetByEspecie(string especie)
    {
        var pets = await _db.Pets.AsNoTracking()
            .Where(p => p.Especie == especie)
            .ToListAsync();
        return Ok(pets);
    }

    /// <summary>Lista pets de um tutor.</summary>
    [HttpGet("tutor/{tutorId:int}")]
    [ProducesResponseType(typeof(IEnumerable<Pet>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Pet>>> GetByTutor(int tutorId)
    {
        if (!await _db.Tutores.AnyAsync(t => t.Id == tutorId))
            return NotFound();

        var pets = await _db.Pets.AsNoTracking()
            .Where(p => p.TutorId == tutorId)
            .ToListAsync();
        return Ok(pets);
    }

    /// <summary>Cria um novo pet.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(Pet), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pet>> Create([FromBody] Pet pet)
    {
        if (string.IsNullOrWhiteSpace(pet.Nome))
            return BadRequest("Nome é obrigatório.");

        if (!await _db.Tutores.AnyAsync(t => t.Id == pet.TutorId))
            return BadRequest("TutorId inválido.");

        _db.Pets.Add(pet);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = pet.Id }, pet);
    }

    /// <summary>Atualiza um pet existente.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] Pet pet)
    {
        if (id != pet.Id)
            return BadRequest("ID da rota difere do corpo da requisição.");

        var existing = await _db.Pets.FindAsync(id);
        if (existing is null)
            return NotFound();

        existing.Nome = pet.Nome;
        existing.Especie = pet.Especie;
        existing.Raca = pet.Raca;
        existing.DataNascimento = pet.DataNascimento;
        existing.TutorId = pet.TutorId;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Remove um pet.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var pet = await _db.Pets.FindAsync(id);
        if (pet is null)
            return NotFound();

        _db.Pets.Remove(pet);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
