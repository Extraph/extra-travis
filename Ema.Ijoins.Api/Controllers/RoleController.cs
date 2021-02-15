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
  public class RoleController : ControllerBase
  {
    private readonly ema_databaseContext _context;

    public RoleController(ema_databaseContext context)
    {
      _context = context;
    }

    // GET: api/Role
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TbmRole>>> GetTbmRoles()
    {
      return await _context.TbmRoles.ToListAsync();
    }

    // GET: api/Role/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TbmRole>> GetTbmRole(int id)
    {
      var tbmRole = await _context.TbmRoles.FindAsync(id);

      if (tbmRole == null)
      {
        return NotFound();
      }

      return tbmRole;
    }

    // PUT: api/Role/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTbmRole(int id, TbmRole tbmRole)
    {
      if (id != tbmRole.RoleId)
      {
        return BadRequest();
      }

      _context.Entry(tbmRole).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!TbmRoleExists(id))
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

    // POST: api/Role
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TbmRole>> PostTbmRole(TbmRole tbmRole)
    {
      _context.TbmRoles.Add(tbmRole);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetTbmRole", new { id = tbmRole.RoleId }, tbmRole);
    }

    // DELETE: api/Role/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTbmRole(int id)
    {
      var tbmRole = await _context.TbmRoles.FindAsync(id);
      if (tbmRole == null)
      {
        return NotFound();
      }

      _context.TbmRoles.Remove(tbmRole);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool TbmRoleExists(int id)
    {
      return _context.TbmRoles.Any(e => e.RoleId == id);
    }
  }
}
