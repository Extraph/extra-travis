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
  public class ParticipantController : ControllerBase
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
  }
}
