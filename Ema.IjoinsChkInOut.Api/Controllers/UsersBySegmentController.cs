using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ema.IjoinsChkInOut.Api.EfModels;
using Ema.IjoinsChkInOut.Api.Services;
using Ema.IjoinsChkInOut.Api.Models;

namespace Ema.Ijoins.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UsersBySegmentController : ControllerBase
  {
    private readonly ema_databaseContext _context;
    private readonly IjoinsService _userService;

    public UsersBySegmentController(ema_databaseContext context, IjoinsService userService)
    {
      _context = context;
      _userService = userService;
    }

    [HttpGet("{segmentId}")]
    public async Task<IActionResult> GetUsersBySegment(int segmentId)
    {
      var tbmSegmentUsers = await _context.TbmSegmentUsers.Where(w => w.SegmentId == segmentId).ToListAsync();
      if (tbmSegmentUsers == null)
      {
        return NotFound();
      }
      List<UsersChecking> usersCheckings = _userService.Get().Where(w => w.SegmentId == segmentId).OrderByDescending(o => o.Createdatetime).ToList();

      List<UsersChecking> usersCheckIn = usersCheckings.Where(w => w.CheckingStatus == "Check-In").ToList();

      List<UsersChecking> usersCheckOut = usersCheckings.Where(w => w.CheckingStatus == "Check-Out").ToList();

      var query = from users in tbmSegmentUsers
                  join usersCheck in usersCheckings on users.UserId equals usersCheck.UserId into gj
                  from subUsers in gj.DefaultIfEmpty()
                  select new { users.SegmentId, users.UserId, Createdatetime = subUsers?.Createdatetime ?? users.Createdatetime, RegistrationStatus = subUsers?.CheckingStatus ?? users.RegistrationStatus };

      var results =
        from gb in query
        group gb by gb.UserId into newGroup
        orderby newGroup.Key
        select newGroup.FirstOrDefault();

      var resultsCheckIn =
        from gb in usersCheckIn
        group gb by gb.UserId into newGroup
        orderby newGroup.Key
        select newGroup.FirstOrDefault();

      var resultsCheckOut =
        from gb in usersCheckOut
        group gb by gb.UserId into newGroup
        orderby newGroup.Key
        select newGroup.FirstOrDefault();


      return Ok(new
      {
        data = results.OrderByDescending(o => o.Createdatetime).ToList(),
        checkInNumber = resultsCheckIn.ToList().Count,
        checkOutNumber = resultsCheckOut.ToList().Count
      });
    }

  }
}
