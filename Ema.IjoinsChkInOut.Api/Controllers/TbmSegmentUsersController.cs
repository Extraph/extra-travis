using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ema.IjoinsChkInOut.Api.EfModels;
using Ema.IjoinsChkInOut.Api.Services;

namespace Ema.IjoinsChkInOut.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TbmSegmentUsersController : ControllerBase
  {
    //private readonly ema_databaseContext _context;
    //private readonly IjoinsService _userService;

    //public TbmSegmentUsersController(ema_databaseContext context, IjoinsService userService)
    //{
    //  _context = context;
    //  _userService = userService;
    //}

    //[HttpGet("{userId}")]
    //public async Task<ActionResult<IEnumerable<TbmSegment>>> GetClassByUserID(string userId)
    //{
    //  List<TbmSegment> segmnents = new List<TbmSegment>();
    //  List<TbmSegmentUser> tbmSegmentUsers = await _context.TbmSegmentUsers.Where(w => w.UserId == userId).ToListAsync();
    //  if (tbmSegmentUsers == null)
    //  {
    //    return NotFound();
    //  }

    //  foreach (TbmSegmentUser tbmSegmentUser in tbmSegmentUsers)
    //  {
    //    segmnents.Add(await _context.TbmSegments.Where(w => w.Id == tbmSegmentUser.SegmentId).FirstOrDefaultAsync());
    //  }

    //  segmnents.ForEach(w => { w.TbmSegmentUsers = null; w.TbmSegmentUserHis = null; });
    //  return segmnents;
    //}





    //// PUT: api/TbmSegmentUsers/5
    //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    //[HttpPut("{id}")]
    //public async Task<IActionResult> PutTbmSegmentUser(int id, TbmSegmentUser tbmSegmentUser)
    //{
    //  if (id != tbmSegmentUser.SegmentId)
    //  {
    //    return BadRequest();
    //  }

    //  _context.Entry(tbmSegmentUser).State = EntityState.Modified;

    //  try
    //  {
    //    await _context.SaveChangesAsync();
    //  }
    //  catch (DbUpdateConcurrencyException)
    //  {
    //    if (!TbmSegmentUserExists(id, tbmSegmentUser.UserId))
    //    {
    //      return NotFound();
    //    }
    //    else
    //    {
    //      throw;
    //    }
    //  }

    //  return NoContent();
    //}

    //// POST: api/TbmSegmentUsers
    //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    //[HttpPost]
    //public async Task<IActionResult> PostTbmSegmentUser(TbmSegmentUser tbmSegmentUser)
    //{
    //  tbmSegmentUser.UserId = tbmSegmentUser.UserId.PadLeft(8, '0');
    //  _context.TbmSegmentUsers.Add(tbmSegmentUser);
    //  try
    //  {
    //    await _context.SaveChangesAsync();
    //  }
    //  catch (DbUpdateException)
    //  {
    //    if (TbmSegmentUserExists(tbmSegmentUser.SegmentId, tbmSegmentUser.UserId))
    //    {
    //      return Conflict();
    //    }
    //    else
    //    {
    //      throw;
    //    }
    //  }
    //  //tbmSegmentUser.Segment = null;
    //  //return CreatedAtAction("GetTbmSegmentUser", new { segmentId = tbmSegmentUser.SegmentId, userId = tbmSegmentUser.UserId }, tbmSegmentUser);
    //  return Ok(new
    //  {
    //    success = true,
    //    message = "",
    //  });
    //}

    //// DELETE: api/TbmSegmentUsers/5
    //[HttpDelete("{id}")]
    //public async Task<IActionResult> DeleteTbmSegmentUser(int id)
    //{
    //  var tbmSegmentUser = await _context.TbmSegmentUsers.FindAsync(id);
    //  if (tbmSegmentUser == null)
    //  {
    //    return NotFound();
    //  }

    //  _context.TbmSegmentUsers.Remove(tbmSegmentUser);
    //  await _context.SaveChangesAsync();

    //  return NoContent();
    //}

    //private bool TbmSegmentUserExists(int segmentId, string userId)
    //{
    //  return _context.TbmSegmentUsers.Any(e => e.SegmentId == segmentId && e.UserId == userId);
    //}
  }
}
