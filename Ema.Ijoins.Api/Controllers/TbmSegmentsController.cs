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
  public class TbmSegmentsController : ControllerBase
  {
    private readonly ema_databaseContext _context;

    public TbmSegmentsController(ema_databaseContext context)
    {
      _context = context;
    }

    // GET: api/TbmSegments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TbmSegment>>> GetTbmSegments()
    {
      return await _context.TbmSegments.ToListAsync();
      //return await _context.TbmSegments.Where(w => w.StartDateTime >= DateTime.Now).OrderBy(o => o.StartDateTime).ToListAsync();
    }

    // GET: api/TbmSegments/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TbmSegment>> GetTbmSegment(DateTime id)
    {
      var tbmSegment = await _context.TbmSegments.FindAsync(id);

      if (tbmSegment == null)
      {
        return NotFound();
      }

      return tbmSegment;
    }

    // PUT: api/TbmSegments/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTbmSegment(DateTime id, TbmSegment tbmSegment)
    {
      if (id != tbmSegment.StartDateTime)
      {
        return BadRequest();
      }

      _context.Entry(tbmSegment).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!TbmSegmentExists(id))
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

    // POST: api/TbmSegments
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TbmSegment>> PostTbmSegment(TbmSegment tbmSegment)
    {
      _context.TbmSegments.Add(tbmSegment);
      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateException)
      {
        if (TbmSegmentExists(tbmSegment.StartDateTime))
        {
          return Conflict();
        }
        else
        {
          throw;
        }
      }

      return CreatedAtAction("GetTbmSegment", new { id = tbmSegment.StartDateTime }, tbmSegment);
    }

    // DELETE: api/TbmSegments/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTbmSegment(DateTime id)
    {
      var tbmSegment = await _context.TbmSegments.FindAsync(id);
      if (tbmSegment == null)
      {
        return NotFound();
      }

      _context.TbmSegments.Remove(tbmSegment);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool TbmSegmentExists(DateTime id)
    {
      return _context.TbmSegments.Any(e => e.StartDateTime == id);
    }
  }
}
