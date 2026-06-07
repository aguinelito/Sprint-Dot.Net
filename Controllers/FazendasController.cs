using Agrosphere.Data;
using Agrosphere.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agrosphere.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FazendasController : ControllerBase
{
    private readonly LifePetDbContext _context;

    public FazendasController(LifePetDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Fazenda>>> GetFazendas()
    {
        return await _context.Fazendas.Include(f => f.Sensores).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Fazenda>> GetFazenda(int id)
    {
        var fazenda = await _context.Fazendas
            .Include(f => f.Sensores)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (fazenda == null)
            return NotFound();

        return fazenda;
    }

    [HttpGet("usuario/{usuarioId}")]
    public async Task<ActionResult<IEnumerable<Fazenda>>> GetFazendasPorUsuario(int usuarioId)
    {
        return await _context.Fazendas
            .Where(f => f.UsuarioId == usuarioId)
            .Include(f => f.Sensores)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Fazenda>> PostFazenda(Fazenda fazenda)
    {
        _context.Fazendas.Add(fazenda);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetFazenda), new { id = fazenda.Id }, fazenda);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutFazenda(int id, Fazenda fazenda)
    {
        if (id != fazenda.Id)
            return BadRequest();

        _context.Entry(fazenda).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!FazendaExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFazenda(int id)
    {
        var fazenda = await _context.Fazendas.FindAsync(id);
        if (fazenda == null)
            return NotFound();

        _context.Fazendas.Remove(fazenda);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool FazendaExists(int id)
    {
        return _context.Fazendas.Any(f => f.Id == id);
    }
}
