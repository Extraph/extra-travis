﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ema.Ijoins.Api.EfModels;

namespace Ema.Ijoins.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TbtIjoinScanQrsController : ControllerBase
  {
    private readonly ema_databaseContext _context;

    public TbtIjoinScanQrsController(ema_databaseContext context)
    {
      _context = context;
    }

    // GET: api/TbtIjoinScanQrs
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TbtIjoinScanQr>>> GetTbtIjoinScanQrs()
    {
      return await _context.TbtIjoinScanQrs.ToListAsync();
    }

    // GET: api/TbtIjoinScanQrs/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TbtIjoinScanQr>> GetTbtIjoinScanQr(int id)
    {
      var tbtIjoinScanQr = await _context.TbtIjoinScanQrs.FindAsync(id);

      if (tbtIjoinScanQr == null)
      {
        return NotFound();
      }

      return tbtIjoinScanQr;
    }

    // PUT: api/TbtIjoinScanQrs/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTbtIjoinScanQr(int id, TbtIjoinScanQr tbtIjoinScanQr)
    {
      if (id != tbtIjoinScanQr.CourseId)
      {
        return BadRequest();
      }

      _context.Entry(tbtIjoinScanQr).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!TbtIjoinScanQrExists(id))
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

    // POST: api/TbtIjoinScanQrs
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TbtIjoinScanQr>> PostTbtIjoinScanQr(TbtIjoinScanQr tbtIjoinScanQr)
    {

      //if (!TbtIjoinScanQrExists(tbtIjoinScanQr.StartDateTime, tbtIjoinScanQr.EndDateTime, tbtIjoinScanQr.SessionId, tbtIjoinScanQr.CourseId, tbtIjoinScanQr.UserId))
      //{
      //  TbtIjoinScanQr tbtIjoinScanQr = new TbtIjoinScanQr
      //  {
      //    Id = klcDataMaster.Id,
      //    FileId = klcDataMaster.FileId,
      //    CourseTypeId = CourseTypeId,
      //    CourseId = int.Parse(klcDataMaster.CourseId),
      //    SessionId = int.Parse(klcDataMaster.SessionId),
      //    StartDateTime = klcDataMaster.StartDateTime,
      //    EndDateTime = klcDataMaster.EndDateTime,
      //    UserId = klcDataMaster.UserId,
      //    RegistrationStatus = klcDataMaster.RegistrationStatus
      //  };
      //  _context.TbtIjoinScanQrs.Add(tbtIjoinScanQr);
      //  await _context.SaveChangesAsync();
      //}
      //else
      //{
      //  TbtIjoinScanQr tbtIjoinScanQrOld = await _context.TbtIjoinScanQrs.Where(
      //    w =>
      //    w.StartDateTime == klcDataMaster.StartDateTime
      //    && w.EndDateTime == klcDataMaster.EndDateTime
      //    && w.SessionId == int.Parse(klcDataMaster.SessionId)
      //    && w.CourseId == int.Parse(klcDataMaster.CourseId)
      //    && w.UserId == klcDataMaster.UserId
      //  ).FirstOrDefaultAsync();
      //  tbtIjoinScanQrOld.Id = klcDataMaster.Id;
      //  tbtIjoinScanQrOld.FileId = klcDataMaster.FileId;
      //  tbtIjoinScanQrOld.RegistrationStatus = klcDataMaster.RegistrationStatus;
      //  tbtIjoinScanQrOld.Updatedatetime = DateTime.Now;
      //  _context.Entry(tbtIjoinScanQrOld).State = EntityState.Modified;
      //  await _context.SaveChangesAsync();
      //}

      _context.TbtIjoinScanQrs.Add(tbtIjoinScanQr);
      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateException)
      {
        if (TbtIjoinScanQrExists(tbtIjoinScanQr.CourseId))
        {
          return Conflict();
        }
        else
        {
          throw;
        }
      }

      return CreatedAtAction("GetTbtIjoinScanQr", new { id = tbtIjoinScanQr.CourseId }, tbtIjoinScanQr);
    }

    private bool TbtIjoinScanQrExists(DateTime StartDateTime, DateTime EndDateTime, string SessionId, string CourseId, string UserId)
    {
      return _context.TbtIjoinScanQrs.Any(
        e =>
         e.CourseId == int.Parse(CourseId)
      && e.SessionId == int.Parse(SessionId)
      && e.StartDateTime == StartDateTime
      && e.EndDateTime == EndDateTime
      && e.UserId == UserId
      );
    }
    private bool TbtIjoinScanQrAddmoreExists(DateTime StartDateTime, DateTime EndDateTime, string SessionId, string CourseId, string UserId)
    {
      return _context.TbtIjoinScanQrAddMores.Any(
        e =>
         e.CourseId == int.Parse(CourseId)
      && e.SessionId == int.Parse(SessionId)
      && e.StartDateTime == StartDateTime
      && e.EndDateTime == EndDateTime
      && e.UserId == UserId
      );
    }

    // DELETE: api/TbtIjoinScanQrs/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTbtIjoinScanQr(int id)
    {
      var tbtIjoinScanQr = await _context.TbtIjoinScanQrs.FindAsync(id);
      if (tbtIjoinScanQr == null)
      {
        return NotFound();
      }

      _context.TbtIjoinScanQrs.Remove(tbtIjoinScanQr);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool TbtIjoinScanQrExists(int id)
    {
      return _context.TbtIjoinScanQrs.Any(e => e.CourseId == id);
    }
  }
}
