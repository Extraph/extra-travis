using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ema.IjoinsChkInOut.Api.Services;
using Ema.IjoinsChkInOut.Api.Models;


namespace Ema.IjoinsChkInOut.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SessionsController : ControllerBase
  {

    private readonly UserIjoinsService _userIjoinsService;
    public SessionsController(UserIjoinsService userIjoinsService)
    {
      _userIjoinsService = userIjoinsService;
    }

    [HttpPost]
    public async Task<ActionResult<List<SessionMobile>>> SearchSessionCheckInOutByParticipant(SessionUser suIn)
    {
      var sessionMobiles = await _userIjoinsService.GetSessionForMobileByUserId(suIn);

      if (sessionMobiles == null)
      {
        return NotFound();
      }

      return sessionMobiles;
    }


  }
}
