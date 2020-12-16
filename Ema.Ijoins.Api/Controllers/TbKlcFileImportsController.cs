using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ema.Ijoins.Api.EfModels;

namespace Ema.Ijoins.Api.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class TbKlcFileImportsController : ControllerBase
  {
    private readonly ema_databaseContext _context;

    public TbKlcFileImportsController(ema_databaseContext context)
    {
      _context = context;
    }

    // GET: api/TbKlcFileImports
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TbKlcFileImport>>> GetTbKlcFileImports()
    {
      return await _context.TbKlcFileImports.ToListAsync();
    }

    // GET: api/TbKlcFileImports/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TbKlcFileImport>> GetTbKlcFileImport(int id)
    {
      var tbKlcFileImport = await _context.TbKlcFileImports.FindAsync(id);

      if (tbKlcFileImport == null)
      {
        return NotFound();
      }

      return tbKlcFileImport;
    }

    // PUT: api/TbKlcFileImports/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTbKlcFileImport(int id, TbKlcFileImport tbKlcFileImport)
    {
      if (id != tbKlcFileImport.Id)
      {
        return BadRequest();
      }

      _context.Entry(tbKlcFileImport).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!TbKlcFileImportExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }

    // POST: api/TbKlcFileImports
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TbKlcFileImport>> PostTbKlcFileImport(TbKlcFileImport tbKlcFileImport)
    {
      _context.TbKlcFileImports.Add(tbKlcFileImport);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetTbKlcFileImport", new { id = tbKlcFileImport.Id }, tbKlcFileImport);
    }

    // DELETE: api/TbKlcFileImports/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTbKlcFileImport(int id)
    {
      var tbKlcFileImport = await _context.TbKlcFileImports.FindAsync(id);
      if (tbKlcFileImport == null)
      {
        return NotFound();
      }

      _context.TbKlcFileImports.Remove(tbKlcFileImport);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool TbKlcFileImportExists(int id)
    {
      return _context.TbKlcFileImports.Any(e => e.Id == id);
    }
  }
}
