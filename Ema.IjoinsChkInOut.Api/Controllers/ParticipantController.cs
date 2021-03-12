using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ema.IjoinsChkInOut.Api.Services;
using Ema.IjoinsChkInOut.Api.EfUserModels;


namespace Ema.IjoinsChkInOut.Api.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class ParticipantController : ControllerBase
  {

    private readonly IUserIjoinsService _userIjoinsService;
    public ParticipantController(IUserIjoinsService userIjoinsService)
    {
      _userIjoinsService = userIjoinsService;
    }

    [HttpPost("Add")]
    public async Task<ActionResult> AddParticipant(TbmUserSessionUser suIn)
    {
      await _userIjoinsService.CreateSessionUser(suIn);
      return Ok();
    }

    [HttpPost("Delete")]
    public async Task<ActionResult> DeleteParticipant(TbmUserSessionUser suIn)
    {
      await _userIjoinsService.RemoveSessionUser(suIn);
      return Ok();
    }



  }
}
