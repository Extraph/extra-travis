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
  [Route("api/[controller]")]
  [ApiController]
  public class TbmKlcFileImportsController : ControllerBase
  {
    private readonly ema_databaseContext _context;

    public TbmKlcFileImportsController(ema_databaseContext context)
    {
      _context = context;
    }

    // GET: api/TbmKlcFileImports
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TbmKlcFileImport>>> GetTbmKlcFileImports()
    {
      return await _context.TbmKlcFileImports.ToListAsync();
    }

    // GET: api/TbmKlcFileImports/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TbmKlcFileImport>> GetTbmKlcFileImport(int id)
    {
      var tbmKlcFileImport = await _context.TbmKlcFileImports.FindAsync(id);

      if (tbmKlcFileImport == null)
      {
        return NotFound();
      }

      return tbmKlcFileImport;
    }

    // PUT: api/TbmKlcFileImports/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTbmKlcFileImport(int id, TbmKlcFileImport tbmKlcFileImport)
    {
      if (id != tbmKlcFileImport.Id)
      {
        return BadRequest();
      }

      _context.Entry(tbmKlcFileImport).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!TbmKlcFileImportExists(id))
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

    // POST: api/TbmKlcFileImports
    [HttpPost]
    public async Task<ActionResult<TbmKlcFileImport>> PostTbmKlcFileImport(TbmKlcFileImport tbmKlcFileImport)
    {
      _context.TbmKlcFileImports.Add(tbmKlcFileImport);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetTbmKlcFileImport", new { id = tbmKlcFileImport.Id }, tbmKlcFileImport);
    }

    // DELETE: api/TbmKlcFileImports/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTbmKlcFileImport(int id)
    {
      var tbmKlcFileImport = await _context.TbmKlcFileImports.FindAsync(id);
      if (tbmKlcFileImport == null)
      {
        return NotFound();
      }

      _context.TbmKlcFileImports.Remove(tbmKlcFileImport);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool TbmKlcFileImportExists(int id)
    {
      return _context.TbmKlcFileImports.Any(e => e.Id == id);
    }
  }
}
