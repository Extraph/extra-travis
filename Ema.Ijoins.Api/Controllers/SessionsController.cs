using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ema.Ijoins.Api.EfModels;
using Ema.Ijoins.Api.Services;
using Ema.Ijoins.Api.Models;

namespace Ema.Ijoins.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SessionsController : ControllerBase
  {
    private readonly IAdminIjoinsService _ijoinsService;

    public SessionsController(IAdminIjoinsService ijoinsService)
    {
      _ijoinsService = ijoinsService;
    }

    [HttpPost]
    public async Task<ActionResult<List<ModelSessionsQR>>> SearchSession(TbmSession tbmSession)
    {
      var tbmSessions = await _ijoinsService.GetSessions(tbmSession);

      if (tbmSessions == null)
      {
        return NotFound();
      }

      return tbmSessions;
    }

    [HttpPost("ToDayClass")]
    public async Task<ActionResult<List<TbmSession>>> ToDayClass(TbmSession tbmSession)
    {
      var tbmSessions = await _ijoinsService.GetToDayClass(tbmSession);

      if (tbmSessions == null)
      {
        return NotFound();
      }

      return tbmSessions;
    }

    [HttpPost("SevenDayClass")]
    public async Task<ActionResult<List<TbmSession>>> SevenDayClass(TbmSession tbmSession)
    {
      var tbmSessions = await _ijoinsService.GetSevenDayClass(tbmSession);

      if (tbmSessions == null)
      {
        return NotFound();
      }

      return tbmSessions;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TbmSession>> UpdateSession(string id, TbmSession tbmSession)
    {
      if (id != tbmSession.SessionId)
      {
        return BadRequest();
      }

      if (!_ijoinsService.TbmSessionExists(tbmSession.SessionId))
      {
        return NotFound();
      }

      return await _ijoinsService.UpdateSession(tbmSession);
    }
    
  }
}
