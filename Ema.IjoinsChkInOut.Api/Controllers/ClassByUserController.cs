using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ema.IjoinsChkInOut.Api.EfModels;
using Ema.IjoinsChkInOut.Api.Services;

namespace Ema.Ijoins.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ClassByUsersController : ControllerBase
  {
    private readonly ema_databaseContext _context;
    private readonly IjoinsService _userService;

    public ClassByUsersController(ema_databaseContext context, IjoinsService userService)
    {
      _context = context;
      _userService = userService;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<TbmSegment>>> GetClassByUserID(string userId)
    {
      List<TbmSegment> segmnents = new List<TbmSegment>();
      List<TbmSegmentUser> tbmSegmentUsers = await _context.TbmSegmentUsers.Where(w => w.UserId == userId).ToListAsync();
      if (tbmSegmentUsers == null)
      {
        return NotFound();
      }

      foreach (TbmSegmentUser tbmSegmentUser in tbmSegmentUsers)
      {
        segmnents.Add(await _context.TbmSegments.Where(w => w.Id == tbmSegmentUser.SegmentId).FirstOrDefaultAsync());
      }

      segmnents.ForEach(w => { w.TbmSegmentUsers = null; w.TbmSegmentUserHis = null; });
      return segmnents;
    }


  }
}
