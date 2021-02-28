using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ema.Ijoins.Api.EfModels;
using Ema.Ijoins.Api.Services;
using Ema.Ijoins.Api.Models;
using Ema.Ijoins.Api.Entities;
using System.Security.Claims;

namespace Ema.Ijoins.Api.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class SessionsController : BaseController
  {
    private readonly IAdminIjoinsService _ijoinsService;

    public SessionsController(IAdminIjoinsService ijoinsService)
    {
      _ijoinsService = ijoinsService;
    }

    [HttpPost]
    public async Task<ActionResult<List<ModelSessionsQR>>> SearchSession(TbmSession tbmSession)
    {
      AssignUserAuthen();

      var tbmSessions = await _ijoinsService.GetSessions(tbmSession, UserAuthen.UserId);

      if (tbmSessions == null)
      {
        return NotFound();
      }

      return tbmSessions;
    }

    [HttpPost("ToDayClass")]
    public async Task<ActionResult<List<TbmSession>>> ToDayClass(FetchSessions tbmSession)
    {
      AssignUserAuthen();

      var tbmSessions = await _ijoinsService.GetToDayClass(tbmSession, UserAuthen.UserId);

      if (tbmSessions == null)
      {
        return NotFound();
      }

      return tbmSessions;
    }

    [HttpPost("SevenDayClass")]
    public async Task<ActionResult<List<TbmSession>>> SevenDayClass(TbmSession tbmSession)
    {
      AssignUserAuthen();

      var tbmSessions = await _ijoinsService.GetSevenDayClass(tbmSession, UserAuthen.UserId);

      if (tbmSessions == null)
      {
        return NotFound();
      }

      return tbmSessions;
    }

    [HttpPost("NextSixDayDashs")]
    public async Task<ActionResult<List<ModelNextSixDayDash>>> NextSixDayDashs()
    {
      AssignUserAuthen();

      var tbmSessions = await _ijoinsService.GetNextSixDayDashs(UserAuthen.UserId);

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


    [HttpPost("ReportSessions")]
    public async Task<ActionResult<List<TbmSession>>> GetReportSessions(TbmSession tbmSession)
    {
      AssignUserAuthen();

      var tbmSessions = await _ijoinsService.GetReportSessions(tbmSession, UserAuthen.UserId);

      if (tbmSessions == null)
      {
        return NotFound();
      }

      return tbmSessions;
    }

    [HttpPost("Report")]
    public async Task<ActionResult<List<ModelReport>>> GetReport(TbmSession tbmSession)
    {

      var tbmSessions = await _ijoinsService.GetReport(tbmSession);

      if (tbmSessions == null)
      {
        return NotFound();
      }

      return tbmSessions;
    }


  }
}
