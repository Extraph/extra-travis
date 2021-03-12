using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ema.IjoinsChkInOut.Api.Services;
using Ema.IjoinsChkInOut.Api.Models;
using Ema.IjoinsChkInOut.Api.EfUserModels;


namespace Ema.IjoinsChkInOut.Api.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class SessionsController : ControllerBase
  {

    private readonly IUserIjoinsService _userIjoinsService;
    public SessionsController(IUserIjoinsService userIjoinsService)
    {
      _userIjoinsService = userIjoinsService;
    }

    [HttpPost("Update")]
    public async Task<ActionResult> UpdateSession(TbmUserSession sIn)
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
