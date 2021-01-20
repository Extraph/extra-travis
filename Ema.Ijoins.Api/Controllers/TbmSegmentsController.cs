using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ema.Ijoins.Api.EfModels;
using Ema.Ijoins.Api.Models;
using System.Globalization;
using Ema.Ijoins.Api.Helpers;

namespace Ema.Ijoins.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TbmSegmentsController : ControllerBase
  {
    private readonly ema_databaseContext _context;

    public TbmSegmentsController(ema_databaseContext context)
    {
      _context = context;
    }


    #region get Qr Code and Search
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ModelSegmentsQR>>> GetTbmSegments()
    {
      CultureInfo enUS = new CultureInfo("en-US");
      DateTime.TryParseExact(DateTime.Now.ToString("yyyyMMdd") + " " + "01AM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime StartDay);


      List<TbmSegment> tbmSegments = await _context.TbmSegments.Where(
        w =>
      w.IsCancel == '0'
      && w.StartDateTime >= StartDay
      ).OrderBy(o => o.StartDateTime).ToListAsync();

      
      return CreateQrPrint(tbmSegments);
    }
    [HttpGet("{Id}")]
    public async Task<ActionResult<IEnumerable<ModelSegmentsQR>>> GetTbmSegment(string Id)
    {
      CultureInfo enUS = new CultureInfo("en-US");
      DateTime.TryParseExact(DateTime.Now.ToString("yyyyMMdd") + " " + "01AM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime StartDay);

      List<TbmSegment> tbmSegments = await _context.TbmSegments.Where(
        w =>
      w.IsCancel == '0'
      && w.StartDateTime >= StartDay
      && w.SessionId.Contains(Id)
      ).OrderBy(o => o.StartDateTime).ToListAsync();

      return CreateQrPrint(tbmSegments);
    }
    private static List<ModelSegmentsQR> CreateQrPrint(List<TbmSegment> tbmSegments)
    {
      CultureInfo enUS = new CultureInfo("en-US");

      List<ModelSegmentsQR> segmentsQRs = new List<ModelSegmentsQR>();
      tbmSegments.ForEach(ts =>
      {

        List<TbmSegment> segments = new List<TbmSegment>();

        System.TimeSpan diffResult = ts.EndDateTime.ToUniversalTime().Subtract(ts.StartDateTime.ToUniversalTime());
        DateTime dateTimeStartTodayQR = ts.StartDateTime;
        DateTime dateTimeEndTodayQR = ts.EndDateTime;
        for (int i = 1; i <= Math.Ceiling(diffResult.TotalDays); i++)
        {

          DateTime.TryParseExact(dateTimeStartTodayQR.ToString("yyyyMMdd") + " " + dateTimeStartTodayQR.ToString("HH:mm:ss"), "yyyyMMdd HH:mm:ss", enUS, DateTimeStyles.None, out DateTime StartDateTime);
          DateTime.TryParseExact(dateTimeStartTodayQR.ToString("yyyyMMdd") + " " + dateTimeEndTodayQR.ToString("HH:mm:ss"), "yyyyMMdd HH:mm:ss", enUS, DateTimeStyles.None, out DateTime EndDateTime);

          segments.Add(new TbmSegment
          {
            Id = i,
            FileId = ts.FileId,
            CourseTypeId = ts.CourseTypeId,
            StartDateTime = StartDateTime,
            EndDateTime = EndDateTime,
            SessionId = ts.SessionId,
            SessionName = ts.SessionName,
            CourseId = ts.CourseId,
            CourseName = ts.CourseName,
            CourseNameTh = ts.CourseNameTh,
            CourseOwnerEmail = ts.CourseOwnerEmail,
            CourseOwnerContactNo = ts.CourseOwnerContactNo,
            Venue = ts.Venue,
            Instructor = ts.Instructor,
            CourseCreditHoursInit = ts.CourseCreditHoursInit,
            PassingCriteriaExceptionInit = ts.PassingCriteriaExceptionInit,
            CourseCreditHours = ts.CourseCreditHours,
            PassingCriteriaException = ts.PassingCriteriaException,
            IsCancel = ts.IsCancel,
            Createdatetime = ts.Createdatetime,
          });

          dateTimeStartTodayQR = dateTimeStartTodayQR.AddDays(1);
        }

        segmentsQRs.Add(new ModelSegmentsQR
        {
          Id = ts.Id,
          FileId = ts.FileId,
          CourseTypeId = ts.CourseTypeId,
          StartDateTime = ts.StartDateTime,
          EndDateTime = ts.EndDateTime,
          SessionId = ts.SessionId,
          SessionName = ts.SessionName,
          CourseId = ts.CourseId,
          CourseName = ts.CourseName,
          CourseNameTh = ts.CourseNameTh,
          CourseOwnerEmail = ts.CourseOwnerEmail,
          CourseOwnerContactNo = ts.CourseOwnerContactNo,
          Venue = ts.Venue,
          Instructor = ts.Instructor,
          CourseCreditHoursInit = ts.CourseCreditHoursInit,
          PassingCriteriaExceptionInit = ts.PassingCriteriaExceptionInit,
          CourseCreditHours = ts.CourseCreditHours,
          PassingCriteriaException = ts.PassingCriteriaException,
          IsCancel = ts.IsCancel,
          Createdatetime = ts.Createdatetime,
          SegmentsQr = segments
        });
      });
      return segmentsQRs;
    }
    #endregion





    #region To Day
    [HttpPost("ToDay")]
    public async Task<ActionResult<IEnumerable<TbmSegment>>> GetTbmSegmentToDay(TbmSegment tbmSegment)
    {
      CultureInfo enUS = new CultureInfo("en-US");
      DateTime.TryParseExact(DateTime.Now.ToString("yyyyMMdd") + " " + "01AM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime StartDay);
      DateTime.TryParseExact(DateTime.Now.ToString("yyyyMMdd") + " " + "11PM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime EndDay);

      return await _context.TbmSegments.Where(
        w =>
        w.IsCancel == '0'
        && w.StartDateTime >= StartDay
        && w.StartDateTime <= EndDay
        && (
           w.CourseId.Contains(tbmSegment.CourseId)
        || w.CourseName.Contains(tbmSegment.CourseId)
        || w.CourseName.Contains(tbmSegment.CourseId.ToLower()) 
        || w.CourseName.Contains(tbmSegment.CourseId.ToUpper())
        )
        ).OrderBy(o => o.StartDateTime).ToListAsync();
    }
    #endregion






    #region NextSevenDay
    [HttpPost("NextSevenDay")]
    public async Task<ActionResult<IEnumerable<TbmSegment>>> GetTbmSegmentNextSevenDay(TbmSegment tbmSegment)
    {
      CultureInfo enUS = new CultureInfo("en-US");
      DateTime.TryParseExact(DateTime.Now.ToString("yyyyMMdd") + " " + "11PM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime EndDay);
      DateTime nextSevenDate = Utility.AddBusinessDays(DateTime.Now, 7);

      return await _context.TbmSegments.Where(
        w =>
        w.IsCancel == '0'
        && w.StartDateTime > EndDay
        && w.StartDateTime <= nextSevenDate
        && (
           w.CourseId.Contains(tbmSegment.CourseId)
        || w.CourseName.Contains(tbmSegment.CourseId)
        || w.CourseName.Contains(tbmSegment.CourseId.ToLower()) 
        || w.CourseName.Contains(tbmSegment.CourseId.ToUpper())
        )
        ).OrderBy(o => o.StartDateTime).ToListAsync();
    }
    #endregion








    [HttpPut("{id}")]
    public async Task<IActionResult> PutTbmSegment(int id, TbmSegment tbmSegment)
    {
      if (id != tbmSegment.Id)
      {
        return BadRequest();
      }

      _context.Entry(tbmSegment).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!TbmSegmentExists(id))
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

    [HttpPost]
    public async Task<ActionResult<TbmSegment>> PostTbmSegment(TbmSegment tbmSegment)
    {
      _context.TbmSegments.Add(tbmSegment);
      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateException)
      {
        if (TbmSegmentExists(tbmSegment.Id))
        {
          return Conflict();
        }
        else
        {
          throw;
        }
      }

      return CreatedAtAction("GetTbmSegment", new { id = tbmSegment.Id }, tbmSegment);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTbmSegment(int id)
    {
      var tbmSegment = await _context.TbmSegments.FindAsync(id);
      if (tbmSegment == null)
      {
        return NotFound();
      }

      _context.TbmSegments.Remove(tbmSegment);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool TbmSegmentExists(int id)
    {
      return _context.TbmSegments.Any(e => e.Id == id);
    }
  }
}
