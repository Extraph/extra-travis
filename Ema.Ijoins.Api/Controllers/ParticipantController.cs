using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ema.Ijoins.Api.EfAdminModels;
using Ema.Ijoins.Api.Services;
using Ema.Ijoins.Api.Models;


namespace Ema.Ijoins.Api.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class ParticipantController : BaseController
  {

    private readonly IAdminIjoinsService _ijoinsService;
    public ParticipantController(IAdminIjoinsService ijoinsService)
    {
      _ijoinsService = ijoinsService;
    }

    [HttpPost]
    public async Task<ActionResult<List<TbmSessionUser>>> SearchParticipant(TbmSessionUser tbmSessionUser)
    {
      var tbmSessions = await _ijoinsService.GetParticipant(tbmSessionUser);

      if (tbmSessions == null)
      {
        return NotFound();
      }

      return tbmSessions;
    }

    [HttpPost("Add")]
    public async Task<ActionResult<TbmSessionUser>> AddParticipant(TbmSessionUser tbmSessionUser)
    {
      if (_ijoinsService.TbmSessionUsersExists(tbmSessionUser.SessionId, tbmSessionUser.UserId))
      {
        return Conflict();
      }

      return await _ijoinsService.AddParticipant(tbmSessionUser);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TbmSessionUser>> UpdateParticipant(string id, TbmSessionUser tbmSessionUser)
    {
      if (id != tbmSessionUser.SessionId)
      {
        return BadRequest();
      }

      if (!_ijoinsService.TbmSessionUsersExists(tbmSessionUser.SessionId, tbmSessionUser.UserId))
      {
        return NotFound();
      }

      return await _ijoinsService.UpdateParticipant(tbmSessionUser);
    }

    [HttpPost("Delete")]
    public async Task<IActionResult> DeleteParticipant(TbmSessionUser tbmSessionUser)
    {
      return Ok(await _ijoinsService.DeleteParticipant(tbmSessionUser));
    }
  }
}
