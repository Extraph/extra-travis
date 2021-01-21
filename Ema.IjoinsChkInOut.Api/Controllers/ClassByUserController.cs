using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ema.IjoinsChkInOut.Api.EfModels;
using Ema.IjoinsChkInOut.Api.Services;
using System.Globalization;
using Ema.IjoinsChkInOut.Api.Helpers;

namespace Ema.IjoinsChkInOut.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ClassByUsersController : ControllerBase
  {
    //private readonly ema_databaseContext _context;
    //private readonly IjoinsService _userService;

    //public ClassByUsersController(ema_databaseContext context, IjoinsService userService)
    //{
    //  _context = context;
    //  _userService = userService;
    //}

    //[HttpGet("{userId}")]
    //public async Task<ActionResult<IEnumerable<TbmSegment>>> GetClassByUserID(string userId)
    //{
    //  CultureInfo enUS = new CultureInfo("en-US");
    //  DateTime.TryParseExact(DateTime.Now.ToString("yyyyMMdd") + " " + "01AM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime StartDay);
    //  DateTime.TryParseExact(DateTime.Now.ToString("yyyyMMdd") + " " + "11PM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime EndDay);

    //  List<TbmSegment> segmnents = new List<TbmSegment>();
    //  List<TbmSegmentUser> tbmSegmentUsers = await _context.TbmSegmentUsers.Where(w => w.UserId == userId).ToListAsync();
    //  if (tbmSegmentUsers == null)
    //  {
    //    return NotFound();
    //  }

    //  foreach (TbmSegmentUser tbmSegmentUser in tbmSegmentUsers)
    //  {
    //    TbmSegment tbmSegment = await _context.TbmSegments.Where(w =>
    //      w.Id == tbmSegmentUser.SegmentId
    //      && w.IsCancel == '0'
    //      && ((w.StartDateTime >= StartDay && w.StartDateTime <= EndDay) || (w.EndDateTime >= StartDay && w.EndDateTime <= EndDay))
    //    ).FirstOrDefaultAsync();
    //    if (tbmSegment != null) segmnents.Add(tbmSegment);
    //  }

    //  segmnents.ForEach(w => { w.TbmSegmentUsers = null; w.TbmSegmentUserHis = null; });
    //  return segmnents.OrderBy(o => o.StartDateTime).ToList();
    //}

    //// GET: api/TbmSegments/NextDay/5
    //[HttpGet("NextSevenDay/{userId}")]
    //public async Task<ActionResult<IEnumerable<TbmSegment>>> GetClassByUserIDNextSevenDay(string userId)
    //{
    //  CultureInfo enUS = new CultureInfo("en-US");
    //  DateTime.TryParseExact(DateTime.Now.ToString("yyyyMMdd") + " " + "11PM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime EndDay);
    //  DateTime nextSevenDate = Utility.AddBusinessDays(DateTime.Now, 7);

    //  List<TbmSegment> segmnents = new List<TbmSegment>();
    //  List<TbmSegmentUser> tbmSegmentUsers = await _context.TbmSegmentUsers.Where(w => w.UserId == userId).ToListAsync();
    //  if (tbmSegmentUsers == null)
    //  {
    //    return NotFound();
    //  }

    //  foreach (TbmSegmentUser tbmSegmentUser in tbmSegmentUsers)
    //  {
    //    TbmSegment tbmSegment = await _context.TbmSegments.Where(w =>
    //      w.Id == tbmSegmentUser.SegmentId
    //      && w.IsCancel == '0'
    //      && w.StartDateTime > EndDay
    //      && w.StartDateTime <= DateTime.Now.AddDays(7)
    //    ).FirstOrDefaultAsync();
    //    if (tbmSegment != null) segmnents.Add(tbmSegment);
    //  }

    //  segmnents.ForEach(w => { w.TbmSegmentUsers = null; w.TbmSegmentUserHis = null; });
    //  return segmnents.OrderBy(o => o.StartDateTime).ToList();


    //}


  }
}
