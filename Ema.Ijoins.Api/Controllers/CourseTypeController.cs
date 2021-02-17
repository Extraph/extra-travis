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
  public class CourseTypeController : BaseController
  {
    private readonly ema_databaseContext _context;

    public CourseTypeController(ema_databaseContext context)
    {
      _context = context;
    }

    // GET: api/CourseType
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TbmCourseType>>> GetTbmCourseTypes()
    {
      return await _context.TbmCourseTypes.ToListAsync();
    }

    // GET: api/CourseType/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TbmCourseType>> GetTbmCourseType(int id)
    {
      var tbmCourseType = await _context.TbmCourseTypes.FindAsync(id);

      if (tbmCourseType == null)
      {
        return NotFound();
      }

      return tbmCourseType;
    }

    // PUT: api/CourseType/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTbmCourseType(int id, TbmCourseType tbmCourseType)
    {
      if (id != tbmCourseType.Id)
      {
        return BadRequest();
      }

      _context.Entry(tbmCourseType).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!TbmCourseTypeExists(id))
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

    // POST: api/CourseType
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TbmCourseType>> PostTbmCourseType(TbmCourseType tbmCourseType)
    {
      _context.TbmCourseTypes.Add(tbmCourseType);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetTbmCourseType", new { id = tbmCourseType.Id }, tbmCourseType);
    }

    // DELETE: api/CourseType/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTbmCourseType(int id)
    {
      var tbmCourseType = await _context.TbmCourseTypes.FindAsync(id);
      if (tbmCourseType == null)
      {
        return NotFound();
      }

      _context.TbmCourseTypes.Remove(tbmCourseType);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool TbmCourseTypeExists(int id)
    {
      return _context.TbmCourseTypes.Any(e => e.Id == id);
    }
  }
}
