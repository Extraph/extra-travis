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
  public class ParticipantController : ControllerBase
  {

    private readonly UserIjoinsService _userIjoinsService;
    public ParticipantController(UserIjoinsService userIjoinsService)
    {
      _userIjoinsService = userIjoinsService;
    }

    [HttpPost("Add")]
    public async Task<ActionResult> AddParticipant(SessionUser suIn)
    {
      await _userIjoinsService.CreateSessionUser(suIn);
      return Ok();
    }

    [HttpPost("Delete")]
    public async Task<ActionResult> DeleteParticipant(SessionUser suIn)
    {
      await _userIjoinsService.RemoveSessionUser(suIn);
      return Ok();
    }



  }
}
