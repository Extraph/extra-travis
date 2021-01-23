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
  public class TbmSessionsController : ControllerBase
  {
    private readonly ema_databaseContext _context;
    private readonly IIjoinsService _ijoinsService;

    public TbmSessionsController(ema_databaseContext context, IIjoinsService ijoinsService)
    {
      _context = context;
      _ijoinsService = ijoinsService;
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<ModelSessionsQR>>> PostTbmSession(TbmSession tbmSession)
    {
      var tbmSessions = await _ijoinsService.GetSessions(tbmSession);

      if (tbmSessions == null)
      {
        return NotFound();
      }

      return tbmSessions.ToList();
    }

    [HttpPost("ToDayClass")]
    public async Task<ActionResult<IEnumerable<TbmSession>>> ToDayClass(TbmSession tbmSession)
    {
      var tbmSessions = await _ijoinsService.GetToDayClass(tbmSession);

      if (tbmSessions == null)
      {
        return NotFound();
      }

      return tbmSessions;
    }

    [HttpPost("SevenDayClass")]
    public async Task<ActionResult<IEnumerable<TbmSession>>> SevenDayClass(TbmSession tbmSession)
    {
      var tbmSessions = await _ijoinsService.GetSevenDayClass(tbmSession);

      if (tbmSessions == null)
      {
        return NotFound();
      }

      return tbmSessions;
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> PutTbmSession(string id, TbmSession tbmSession)
    {
      if (id != tbmSession.SessionId)
      {
        return BadRequest();
      }

      _context.Entry(tbmSession).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!TbmSessionExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }

    private bool TbmSessionExists(string id)
    {
      return _context.TbmSessions.Any(e => e.SessionId == id);
    }
  }
}
