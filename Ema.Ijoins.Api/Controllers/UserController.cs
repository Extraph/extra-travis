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
  public class UserController : BaseController
  {
    private readonly ema_databaseContext _context;

    public UserController(ema_databaseContext context)
    {
      _context = context;
    }

    // GET: api/User
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TbmUser>>> GetTbmUsers()
    {
      return await _context.TbmUsers.OrderBy(o => o.UserId).ToListAsync();
    }

    // GET: api/User/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TbmUser>> GetTbmUser(string id)
    {
      var tbmUser = await _context.TbmUsers.FindAsync(id);

      if (tbmUser == null)
      {
        return NotFound();
      }

      return tbmUser;
    }

    // PUT: api/User/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTbmUser(string id, TbmUser tbmUser)
    {
      if (id != tbmUser.UserId)
      {
        return BadRequest();
      }

      _context.Entry(tbmUser).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!TbmUserExists(id))
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

    // POST: api/User
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TbmUser>> PostTbmUser(TbmUser tbmUser)
    {
      _context.TbmUsers.Add(tbmUser);
      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateException)
      {
        if (TbmUserExists(tbmUser.UserId))
        {
          return Conflict();
        }
        else
        {
          throw;
        }
      }

      return CreatedAtAction("GetTbmUser", new { id = tbmUser.UserId }, tbmUser);
    }

    // DELETE: api/User/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTbmUser(string id)
    {
      var tbmUser = await _context.TbmUsers.FindAsync(id);
      if (tbmUser == null)
      {
        return NotFound();
      }

      _context.TbmUsers.Remove(tbmUser);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool TbmUserExists(string id)
    {
      return _context.TbmUsers.Any(e => e.UserId == id);
    }
  }
}
