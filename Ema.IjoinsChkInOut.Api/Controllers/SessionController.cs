using Ema.IjoinsChkInOut.Api.Models;
using Ema.IjoinsChkInOut.Api.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

namespace Ema.IjoinsChkInOut.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SessionController : ControllerBase
  {
    private readonly UserIjoinsService _userIjoinsService;

    public SessionController(UserIjoinsService userIjoinsService)
    {
      _userIjoinsService = userIjoinsService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Session>>> GetSession() => await _userIjoinsService.GetSession();

    [HttpGet("{id:length(24)}", Name = "GetSession")]
    public async Task<ActionResult<Session>> GetSession(string id)
    {
      var session = await _userIjoinsService.GetSession(id);

      if (session == null)
      {
        return NotFound();
      }

      return session;
    }

  }
}
