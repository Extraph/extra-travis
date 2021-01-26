using Ema.Ijoins.Api.Models;
using Ema.Ijoins.Api.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

namespace Ema.Ijoins.Api.Controllers
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

    //[HttpPost]
    //public async Task<ActionResult<Session>> CreateSession(Session session)
    //{
    //  await _userIjoinsService.Create(session);

    //  return CreatedAtRoute("GetSession", new { id = session.Id.ToString() }, session);
    //}


  }
}
