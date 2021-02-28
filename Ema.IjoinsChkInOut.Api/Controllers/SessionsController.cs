using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ema.IjoinsChkInOut.Api.Services;
using Ema.IjoinsChkInOut.Api.Models;


namespace Ema.IjoinsChkInOut.Api.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class SessionsController : ControllerBase
  {

    private readonly UserIjoinsService _userIjoinsService;
    public SessionsController(UserIjoinsService userIjoinsService)
    {
      _userIjoinsService = userIjoinsService;
    }

    [HttpPost("Update")]
    public async Task<ActionResult> UpdateSession(Session sIn)
    {
      await _userIjoinsService.UpdateSession(sIn);
      return Ok();
    }

    [HttpGet("TodayClass/{userId}")]
    public async Task<ActionResult<List<SessionMobile>>> TodayClass(String userId)
    {
      var sessionMobiles = await _userIjoinsService.GetSessionTodayForMobileByUserId(userId);

      if (sessionMobiles == null)
      {
        return NotFound();
      }

      return sessionMobiles;
    }

    [HttpGet("SevendayClass/{userId}")]
    public async Task<ActionResult<List<SessionMobile>>> SevendayClass(String userId)
    {
      var sessionMobiles = await _userIjoinsService.GetSessionSevendayForMobileByUserId(userId);

      if (sessionMobiles == null)
      {
        return NotFound();
      }

      return sessionMobiles;
    }
  }
}
