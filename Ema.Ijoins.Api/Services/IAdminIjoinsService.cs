using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Ema.Ijoins.Api.Helpers;
using Ema.Ijoins.Api.EfModels;
using Ema.Ijoins.Api.Models;
using Ema.Ijoins.Api.Services;
using System.Globalization;


namespace Ema.Ijoins.Api.Services
{
  public interface IAdminIjoinsService
  {
    Task<object> UploadFileKlc(IFormFile file);
    Task<object> ImportKlcData(TbmKlcFileImport tbmKlcFileImport);
    Task<List<ModelSessionsQR>> GetSessions(TbmSession tbmSession);
    Task<TbmSession> UpdateSession(TbmSession tbmSession);
    Task<List<TbmSession>> GetToDayClass(TbmSession tbmSession);
    Task<List<TbmSession>> GetSevenDayClass(TbmSession tbmSession);
    Task<List<TbmSessionUser>> GetParticipant(TbmSessionUser tbmSessionUser);
    Task<TbmSessionUser> AddParticipant(TbmSessionUser tbmSessionUser);
    Task<TbmSessionUser> UpdateParticipant(TbmSessionUser tbmSessionUser);
    Task<object> DeleteParticipant(TbmSessionUser tbmSessionUser);
    bool TbmSessionUsersExists(string SessionId, string UserId);
    bool TbmSessionExists(string SessionId);
  }

  public class AdminIjoinsService : IAdminIjoinsService
  {
    private readonly ema_databaseContext _context;
    private readonly UserIjoinsService _userIjoinsService;
    public AdminIjoinsService(ema_databaseContext context, UserIjoinsService userIjoinsService)
    {
      _context = context;
      _userIjoinsService = userIjoinsService;
    }
    public async Task<object> UploadFileKlc(IFormFile file)
    {
      using var transaction = _context.Database.BeginTransaction();
      try
      {
        if (file == null || file.Length == 0) return new { Message = "file not selected" };

        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "FileUploaded", "KLC"));

        Guid guid = Guid.NewGuid();
        var path = Path.Combine(Directory.GetCurrentDirectory(), "FileUploaded", "KLC", file.GetFilename());
        var ext = Path.GetExtension(path).ToLowerInvariant();
        var pathGuid = Path.Combine(Directory.GetCurrentDirectory(), "FileUploaded", "KLC", guid.ToString() + ext);

        TbmKlcFileImport attachFiles;
        using (var stream = new FileStream(pathGuid, FileMode.Create))
        {
          await file.CopyToAsync(stream);

          attachFiles = new TbmKlcFileImport
          {
            FileName = file.GetFilename(),
            GuidName = guid.ToString() + ext,
            Status = "upload success",
            ImportBy = "รัฐวิชญ์"
          };
          _context.TbmKlcFileImports.Add(attachFiles);
          await _context.SaveChangesAsync();
          await transaction.CreateSavepointAsync("UploadFileSuccess");
        }

        List<TbKlcDataMaster> tbKlcDatas = Utility.ReadExcelEPPlus(pathGuid, attachFiles);
        List<TbKlcDataMaster> tbKlcDatasInvalid = Utility.ValidateData(tbKlcDatas);

        string strMessage = "";
        try
        {
          _context.TbKlcDataMasters.AddRange(tbKlcDatas);
          await _context.SaveChangesAsync();
        }
        catch (System.Exception e)
        {
          strMessage = e.InnerException.Message;
          await transaction.RollbackToSavepointAsync("UploadFileSuccess");
        }

        await transaction.CommitAsync();

        return new
        {
          Success = true,
          Message = strMessage,
          DataInvalid = tbKlcDatasInvalid,
          FileUploadId = attachFiles.Id,
          TotalNo = tbKlcDatas.Count,
          ValidNo = tbKlcDatas.Count - tbKlcDatasInvalid.Count,
          InvalidNo = tbKlcDatasInvalid.Count
        };
      }
      catch (System.Exception e)
      {
        await transaction.RollbackToSavepointAsync("UploadFileSuccess");
        return new
        {
          Success = false,
          Message = e.Message
        };
      }
    }

    public async Task<object> ImportKlcData(TbmKlcFileImport tbmKlcFileImport)
    {
      using var transaction = _context.Database.BeginTransaction();

      try
      {
        await transaction.CreateSavepointAsync("BeginImport");


        IEnumerable<TbKlcDataMaster> tbKlcDataMasters = await _context.TbKlcDataMasters.Where(w => w.FileId == tbmKlcFileImport.Id).ToListAsync();


        var tbKlcDataMastersGroupsBySessionId =
            from klc in tbKlcDataMasters
            group klc by new
            {
              klc.SessionId
            } into gresult
            select gresult.FirstOrDefault();
        ;

        foreach (TbKlcDataMaster ts in tbKlcDataMastersGroupsBySessionId)
        {
          TbmSession tbmSession = await _context.TbmSessions.Where(w => w.SessionId == ts.SessionId).FirstOrDefaultAsync();
          if (tbmSession != null) _context.TbmSegments.RemoveRange(await _context.TbmSegments.Where(w => w.SessionId == tbmSession.SessionId).ToListAsync());

          if (tbmKlcFileImport.ImportType == "Upload session and participants")
          {
            if (tbmSession != null) _context.TbmSessionUsers.RemoveRange(await _context.TbmSessionUsers.Where(w => w.SessionId == tbmSession.SessionId).ToListAsync());
            if (tbmSession != null) _context.TbmSessionUserHis.RemoveRange(await _context.TbmSessionUserHis.Where(w => w.SessionId == tbmSession.SessionId).ToListAsync());

            if (tbmSession != null) _userIjoinsService.RemoveSessionUser(new SessionUser { SessionId = tbmSession.SessionId });
          }

          await _context.SaveChangesAsync();
        }



        foreach (TbKlcDataMaster klcDataMaster in tbKlcDataMasters)
        {
          int CourseTypeId;
          if (!TbmCourseTypeExists(klcDataMaster.CourseType))
          {
            TbmCourseType tbmCourseType = new TbmCourseType { CourseType = klcDataMaster.CourseType, CourseId = klcDataMaster.CourseId };
            _context.TbmCourseTypes.Add(tbmCourseType);
            await _context.SaveChangesAsync();
            CourseTypeId = tbmCourseType.Id;
          }
          else
          {
            TbmCourseType tbmCourseType = await _context.TbmCourseTypes.Where(w => w.CourseType == klcDataMaster.CourseType).FirstOrDefaultAsync();
            CourseTypeId = tbmCourseType.Id;
          }

          if (!TbmCourseExists(klcDataMaster.CourseId))
          {
            TbmCourse tbmCourse = new TbmCourse { CourseId = klcDataMaster.CourseId, CourseName = klcDataMaster.CourseName, CourseNameTh = klcDataMaster.CourseNameTh };
            _context.TbmCourses.Add(tbmCourse);
            await _context.SaveChangesAsync();
          }
          else
          {
            TbmCourse tbmCourse = await _context.TbmCourses.Where(w => w.CourseId == klcDataMaster.CourseId).FirstOrDefaultAsync();
            tbmCourse.CourseName = klcDataMaster.CourseName;
            tbmCourse.CourseNameTh = klcDataMaster.CourseNameTh;
            _context.Entry(tbmCourse).State = EntityState.Modified;
            await _context.SaveChangesAsync();
          }

          if (!TbmSessionExists(klcDataMaster.SessionId))
          {
            TbmSession tbmSession = new TbmSession
            {
              FileId = klcDataMaster.FileId,
              CourseTypeId = CourseTypeId,
              CourseId = klcDataMaster.CourseId,
              CourseName = klcDataMaster.CourseName,
              CourseNameTh = klcDataMaster.CourseNameTh,
              SessionId = klcDataMaster.SessionId,
              SessionName = klcDataMaster.SessionName,
              //StartDateTime = null,
              //EndDateTime = null
              CourseOwnerEmail = klcDataMaster.CourseOwnerEmail,
              CourseOwnerContactNo = klcDataMaster.CourseOwnerContactNo,
              Venue = klcDataMaster.Venue,
              Instructor = klcDataMaster.Instructor,
              CourseCreditHoursInit = klcDataMaster.CourseCreditHours,
              PassingCriteriaExceptionInit = klcDataMaster.PassingCriteriaException,
              CourseCreditHours = klcDataMaster.CourseCreditHours,
              PassingCriteriaException = klcDataMaster.PassingCriteriaException
            };
            _context.TbmSessions.Add(tbmSession);
            await _context.SaveChangesAsync();
          }
          else
          {
            TbmSession tbmSession = await _context.TbmSessions.Where(w => w.SessionId == klcDataMaster.SessionId).FirstOrDefaultAsync();
            tbmSession.FileId = klcDataMaster.FileId;
            tbmSession.CourseTypeId = CourseTypeId;
            tbmSession.CourseId = klcDataMaster.CourseId;
            tbmSession.CourseName = klcDataMaster.CourseName;
            tbmSession.CourseNameTh = klcDataMaster.CourseNameTh;
            tbmSession.SessionId = klcDataMaster.SessionId;
            tbmSession.SessionName = klcDataMaster.SessionName;
            //tbmSession.StartDateTime = null;
            //tbmSession.EndDateTime = null;
            tbmSession.CourseOwnerEmail = klcDataMaster.CourseOwnerEmail;
            tbmSession.CourseOwnerContactNo = klcDataMaster.CourseOwnerContactNo;
            tbmSession.Venue = klcDataMaster.Venue;
            tbmSession.Instructor = klcDataMaster.Instructor;
            tbmSession.CourseCreditHoursInit = klcDataMaster.CourseCreditHours;
            tbmSession.PassingCriteriaExceptionInit = klcDataMaster.PassingCriteriaException;
            tbmSession.CourseCreditHours = klcDataMaster.CourseCreditHours;
            tbmSession.PassingCriteriaException = klcDataMaster.PassingCriteriaException;
            tbmSession.IsCancel = '0';
            tbmSession.UpdateBy = "Admin Uploader";
            tbmSession.UpdateDatetime = DateTime.Now;
            _context.Entry(tbmSession).State = EntityState.Modified;
            await _context.SaveChangesAsync();
          }

          if (!TbmSegmentExists(klcDataMaster.StartDateTime, klcDataMaster.EndDateTime, klcDataMaster.SessionId))
          {
            TbmSegment tbmSegment = new TbmSegment
            {
              SessionId = klcDataMaster.SessionId,
              SegmentNo = klcDataMaster.SegmentNo,
              SegmentName = klcDataMaster.SegmentName,
              StartDate = klcDataMaster.StartDate,
              EndDate = klcDataMaster.EndDate,
              StartTime = klcDataMaster.StartTime,
              EndTime = klcDataMaster.EndTime,
              StartDateTime = klcDataMaster.StartDateTime,
              EndDateTime = klcDataMaster.EndDateTime,
            };
            _context.TbmSegments.Add(tbmSegment);
            await _context.SaveChangesAsync();
          }
          else
          {
            TbmSegment tbmSegment = await _context.TbmSegments.Where(
              w =>
              w.SessionId == klcDataMaster.SessionId &&
              w.StartDateTime == klcDataMaster.StartDateTime &&
              w.EndDateTime == klcDataMaster.EndDateTime
            )
              .FirstOrDefaultAsync();
            tbmSegment.SegmentNo = klcDataMaster.SegmentNo;
            tbmSegment.SegmentName = klcDataMaster.SegmentName;
            tbmSegment.StartDate = klcDataMaster.StartDate;
            tbmSegment.EndDate = klcDataMaster.EndDate;
            tbmSegment.StartTime = klcDataMaster.StartTime;
            tbmSegment.EndTime = klcDataMaster.EndTime;
            _context.Entry(tbmSegment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
          }

          if (!TbmRegistrationStatusExists(klcDataMaster.RegistrationStatus))
          {
            TbmRegistrationStatus tbmRegistrationStatus = new TbmRegistrationStatus { RegistrationStatus = klcDataMaster.RegistrationStatus };
            _context.TbmRegistrationStatuses.Add(tbmRegistrationStatus);
            await _context.SaveChangesAsync();
          }

          if (!TbmSessionUsersExists(klcDataMaster.SessionId, klcDataMaster.UserId))
          {
            TbmSessionUser tbmSessionUser = new TbmSessionUser
            {
              SessionId = klcDataMaster.SessionId,
              UserId = klcDataMaster.UserId,
              RegistrationStatus = klcDataMaster.RegistrationStatus
            };
            _context.TbmSessionUsers.Add(tbmSessionUser);
            await _context.SaveChangesAsync();
          }
          else
          {
            TbmSessionUser tbmSessionUser = await _context.TbmSessionUsers.Where(w => w.SessionId == klcDataMaster.SessionId && w.UserId == klcDataMaster.UserId).FirstOrDefaultAsync();
            tbmSessionUser.RegistrationStatus = klcDataMaster.RegistrationStatus;
            tbmSessionUser.UpdateDatetime = DateTime.Now;
            _context.Entry(tbmSessionUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();
          }
        }



        foreach (TbKlcDataMaster ts in tbKlcDataMastersGroupsBySessionId)
        {
          TbmSession tbmSession = await _context.TbmSessions.Where(w => w.SessionId == ts.SessionId).FirstOrDefaultAsync();
          if (tbmSession != null)
          {
            List<TbmSegment> tbmSegments = await _context.TbmSegments.Where(w => w.SessionId == ts.SessionId).ToListAsync();

            DateTime minStartDate = tbmSegments.OrderBy(o => o.StartDateTime).FirstOrDefault().StartDateTime;
            DateTime maxEndDate = tbmSegments.OrderByDescending(o => o.EndDateTime).FirstOrDefault().EndDateTime;
            tbmSession.StartDateTime = minStartDate;
            tbmSession.EndDateTime = maxEndDate;
            _context.Entry(tbmSession).State = EntityState.Modified;
            await _context.SaveChangesAsync();
          }
        }

        //Initial Data For Ijoin Session and Session User!!!!!
        foreach (TbKlcDataMaster ts in tbKlcDataMastersGroupsBySessionId)
        {
          TbmSession tbmSession = await _context.TbmSessions.Where(w => w.SessionId == ts.SessionId).FirstOrDefaultAsync();
          _userIjoinsService.CreateSession(new Session
          {
            CourseId = tbmSession.CourseId,
            CourseName = tbmSession.CourseName,
            CourseNameTh = tbmSession.CourseNameTh,
            SessionId = tbmSession.SessionId,
            SessionName = tbmSession.SessionName,
            StartDateTime = tbmSession.StartDateTime,
            EndDateTime = tbmSession.EndDateTime,
            CourseOwnerEmail = tbmSession.CourseOwnerEmail,
            CourseOwnerContactNo = tbmSession.CourseOwnerContactNo,
            Venue = tbmSession.Venue,
            Instructor = tbmSession.Instructor,
            CourseCreditHoursInit = tbmSession.CourseCreditHours,
            PassingCriteriaExceptionInit = tbmSession.PassingCriteriaException,
            CourseCreditHours = tbmSession.CourseCreditHours,
            PassingCriteriaException = tbmSession.PassingCriteriaException,
            IsCancel = '0'
          });

          List<TbmSessionUser> tbmSessionUsers = await _context.TbmSessionUsers.Where(w => w.SessionId == ts.SessionId).ToListAsync();
          foreach (TbmSessionUser su in tbmSessionUsers)
          {
            if (su.RegistrationStatus == "Enrolled")
            {
              _userIjoinsService.CreateSessionUser(new SessionUser
              {
                SessionId = su.SessionId,
                UserId = su.UserId
              });
            }
          }
        }





        var klcFileImport = await _context.TbmKlcFileImports.FindAsync(tbmKlcFileImport.Id);
        klcFileImport.ImportTotalrecords = tbKlcDataMasters.Count().ToString();
        klcFileImport.Status = "import success";
        klcFileImport.ImportType = tbmKlcFileImport.ImportType;
        _context.Entry(klcFileImport).State = EntityState.Modified;
        await _context.SaveChangesAsync();


        List<TbKlcDataMasterHi> klcDataMasterHi = Utility.MoveDataKlcUploadToHis(tbKlcDataMasters.ToList());
        _context.TbKlcDataMasterHis.AddRange(klcDataMasterHi);
        await _context.SaveChangesAsync();
        _context.TbKlcDataMasters.RemoveRange(tbKlcDataMasters.ToList());
        await _context.SaveChangesAsync();



        await transaction.CommitAsync();

        var tbmKlcFileImports = await _context.TbmKlcFileImports.OrderByDescending(o => o.Createdatetime).ToListAsync();
        tbmKlcFileImports.ForEach(w => { w.TbKlcDataMasters = null; w.TbKlcDataMasterHis = null; w.TbmSessions = null; });

        return new
        {
          success = true,
          message = "",
          dataList = tbmKlcFileImports
        };
      }
      catch (System.Exception e)
      {
        await transaction.RollbackToSavepointAsync("BeginImport");

        try
        {
          using var contextEx = new ema_databaseContext();
          var klcFileImport = await contextEx.TbmKlcFileImports.FindAsync(tbmKlcFileImport.Id);
          klcFileImport.Status = "import failed";
          klcFileImport.ImportMessage = e.Message;
          contextEx.Entry(klcFileImport).State = EntityState.Modified;
          await contextEx.SaveChangesAsync();
        }
        catch (Exception ex)
        {
          return new
          {
            success = false,
            message = ex.Message
          };
        }

        return new
        {
          success = false,
          message = e.Message
        };
      }
    }

    private bool TbmCourseTypeExists(string CourseType)
    {
      return _context.TbmCourseTypes.Any(e => e.CourseType.ToLower() == CourseType.ToLower());
    }
    private bool TbmCourseExists(string CourseId)
    {
      return _context.TbmCourses.Any(e => e.CourseId == CourseId);
    }
    public bool TbmSessionExists(string SessionId)
    {
      return _context.TbmSessions.Any(e => e.SessionId == SessionId);
    }
    private bool TbmSegmentExists(DateTime StartDateTime, DateTime EndDateTime, string SessionId)
    {
      return _context.TbmSegments.Any(
        e =>
       e.SessionId == SessionId
      && e.StartDateTime == StartDateTime
      && e.EndDateTime == EndDateTime
      );
    }
    private bool TbmRegistrationStatusExists(String RegistrationStatus)
    {
      return _context.TbmRegistrationStatuses.Any(e => e.RegistrationStatus == RegistrationStatus);
    }
    public bool TbmSessionUsersExists(string SessionId, string UserId)
    {
      return _context.TbmSessionUsers.Any(
        e =>
         e.SessionId == SessionId
      && e.UserId == UserId
      );
    }

    public async Task<List<ModelSessionsQR>> GetSessions(TbmSession tbmSession)
    {
      CultureInfo enUS = new CultureInfo("en-US");
      DateTime.TryParseExact(DateTime.Now.ToString("yyyyMMdd") + " " + "01AM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime StartDay);

      var tbmSessions = await _context.TbmSessions
        .Where(
                w => w.EndDateTime >= StartDay
                && w.SessionId.Contains(tbmSession.SessionId)
              )
        .OrderBy(o => o.StartDateTime).ToListAsync();


      List<ModelSessionsQR> segmentsQRs = new List<ModelSessionsQR>();
      foreach (TbmSession session in tbmSessions)
      {

        List<VSegmentGenQr> vSegmentGenQrs = await _context.VSegmentGenQrs.Where(w => w.SessionId == session.SessionId).OrderBy(o => o.StartDateTime).ToListAsync();

        segmentsQRs.Add(new ModelSessionsQR
        {
          FileId = session.FileId,
          CourseTypeId = session.FileId,
          CourseId = session.CourseId,
          CourseName = session.CourseName,
          CourseNameTh = session.CourseNameTh,
          SessionId = session.SessionId,
          SessionName = session.SessionName,
          StartDateTime = session.StartDateTime,
          EndDateTime = session.EndDateTime,
          CourseOwnerEmail = session.CourseOwnerEmail,
          CourseOwnerContactNo = session.CourseOwnerContactNo,
          Venue = session.Venue,
          Instructor = session.Instructor,
          CourseCreditHoursInit = session.CourseCreditHoursInit,
          PassingCriteriaExceptionInit = session.PassingCriteriaExceptionInit,
          CourseCreditHours = session.CourseCreditHours,
          PassingCriteriaException = session.PassingCriteriaException,
          IsCancel = session.IsCancel,
          Createdatetime = session.Createdatetime,
          UpdateBy = session.UpdateBy,
          UpdateDatetime = session.UpdateDatetime,
          SegmentsQr = vSegmentGenQrs
        });
      }


      return segmentsQRs;
    }
    public async Task<List<TbmSession>> GetToDayClass(TbmSession tbmSession)
    {
      CultureInfo enUS = new CultureInfo("en-US");
      DateTime.TryParseExact(DateTime.Now.ToString("yyyyMMdd") + " " + "01AM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime StartDay);
      DateTime.TryParseExact(DateTime.Now.ToString("yyyyMMdd") + " " + "11PM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime EndDay);

      return await _context.TbmSessions.Where(
        w =>
        w.IsCancel == '0'
        && w.StartDateTime <= EndDay
        && w.EndDateTime >= StartDay
        && (
           w.CourseId.Contains(tbmSession.CourseId)
        || w.CourseName.Contains(tbmSession.CourseId)
        || w.CourseName.Contains(tbmSession.CourseId.ToLower())
        || w.CourseName.Contains(tbmSession.CourseId.ToUpper())
        )
        ).OrderBy(o => o.StartDateTime).ToListAsync();
    }
    public async Task<List<TbmSession>> GetSevenDayClass(TbmSession tbmSession)
    {
      //CultureInfo enUS = new CultureInfo("en-US");
      //DateTime.TryParseExact(DateTime.Now.ToString("yyyyMMdd") + " " + "01AM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime StartDay);
      //DateTime.TryParseExact(DateTime.Now.ToString("yyyyMMdd") + " " + "11PM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime EndDay);
      DateTime nextSevenDate = DateTime.Now.AddDays(7);

      return await _context.TbmSessions.Where(
        w =>
        w.IsCancel == '0'
        && (w.EndDateTime >= DateTime.Now && w.EndDateTime <= nextSevenDate)
        && (
           w.CourseId.Contains(tbmSession.CourseId)
        || w.CourseName.Contains(tbmSession.CourseId)
        || w.CourseName.Contains(tbmSession.CourseId.ToLower())
        || w.CourseName.Contains(tbmSession.CourseId.ToUpper())
        )
        ).OrderBy(o => o.StartDateTime).ToListAsync();
    }
    public async Task<TbmSession> UpdateSession(TbmSession tbmSession)
    {
      tbmSession.UpdateDatetime = DateTime.Now;
      _context.Entry(tbmSession).State = EntityState.Modified;
      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        throw;
      }

      var retSession = await _context.TbmSessions.Where(w => w.SessionId == tbmSession.SessionId).FirstOrDefaultAsync();

      return retSession;
    }
    public async Task<List<TbmSessionUser>> GetParticipant(TbmSessionUser tbmSessionUser)
    {
      return await _context.TbmSessionUsers
          .Where(
                 w => w.SessionId == tbmSessionUser.SessionId
                 && w.UserId.Contains(tbmSessionUser.UserId)
                )
          .OrderBy(o => o.UserId).ToListAsync();
    }
    public async Task<TbmSessionUser> AddParticipant(TbmSessionUser tbmSessionUser)
    {
      _context.TbmSessionUsers.Add(tbmSessionUser);
      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateException)
      {
        throw;
      }

      var retSessionUser = await _context.TbmSessionUsers.Where(w => w.SessionId == tbmSessionUser.SessionId && w.UserId == tbmSessionUser.UserId).FirstOrDefaultAsync();

      return retSessionUser;
    }
    public async Task<TbmSessionUser> UpdateParticipant(TbmSessionUser tbmSessionUser)
    {
      tbmSessionUser.UpdateDatetime = DateTime.Now;
      _context.Entry(tbmSessionUser).State = EntityState.Modified;
      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        throw;
      }

      var retSessionUser = await _context.TbmSessionUsers.Where(w => w.SessionId == tbmSessionUser.SessionId && w.UserId == tbmSessionUser.UserId).FirstOrDefaultAsync();

      return retSessionUser;
    }
    public async Task<object> DeleteParticipant(TbmSessionUser tbmSessionUser)
    {
      var chkSessionUser = await _context.TbmSessionUsers.Where(w => w.SessionId == tbmSessionUser.SessionId && w.UserId == tbmSessionUser.UserId).FirstOrDefaultAsync();
      if (chkSessionUser == null)
      {
        return new { Success = false, Message = "NotFound" };
      }
      _context.TbmSessionUsers.Remove(chkSessionUser);
      await _context.SaveChangesAsync();

      return new { Success = true };
    }

  }















  //[HttpGet("GetKlc")]
  //  public IActionResult GetKlc()
  //  {
  //    var model = new FilesViewModel();
  //    foreach (var item in _fileProvider.GetDirectoryContents("KLC"))
  //    {
  //      model.Files.Add(
  //          new FileDetails { Name = item.Name, Path = item.PhysicalPath });
  //    }
  //    return Ok(new
  //    {
  //      success = true,
  //      message = "",
  //      data = model
  //    });
  //  }

  //  [HttpGet("GetBanner")]
  //  public IActionResult GetBanner()
  //  {
  //    var model = new FilesViewModel();
  //    foreach (var item in _fileProvider.GetDirectoryContents("Banner"))
  //    {
  //      model.Files.Add(
  //          new FileDetails { Name = item.Name, Path = item.PhysicalPath });
  //    }
  //    return Ok(new
  //    {
  //      success = true,
  //      message = "",
  //      data = model
  //    });
  //  }

  //  [HttpPost("UploadFileBanner")]
  //  [DisableRequestSizeLimit]
  //  public async Task<IActionResult> UploadFileBanner(IFormFile file)
  //  {
  //    try
  //    {
  //      if (file == null || file.Length == 0)
  //        return Content("file not selected");

  //      Guid guid = Guid.NewGuid();

  //      var path = Path.Combine(
  //                  Directory.GetCurrentDirectory(), "FileUploaded", "Banner",
  //                  file.GetFilename());

  //      var ext = Path.GetExtension(path).ToLowerInvariant();

  //      var pathGuid = Path.Combine(
  //                Directory.GetCurrentDirectory(), "FileUploaded", "Banner",
  //                guid.ToString() + ext);

  //      using (var stream = new FileStream(pathGuid, FileMode.Create))
  //      {
  //        await file.CopyToAsync(stream);
  //      }

  //      TbmKlcFileImport attachFiles = new TbmKlcFileImport
  //      {
  //        FileName = file.GetFilename(),
  //        GuidName = guid.ToString() + ext
  //      };

  //      return Ok(new
  //      {
  //        success = true,
  //        message = "",
  //        data = attachFiles
  //      });
  //    }
  //    catch (System.Exception e)
  //    {
  //      return Ok(new
  //      {
  //        success = false,
  //        message = e.Message,
  //        data = ""
  //      });
  //    }
  //  }

  //  [HttpGet("DeleteBanner/{filename}")]
  //  public IActionResult DeleteBanner(string filename)
  //  {
  //    try
  //    {
  //      if (filename == null)
  //        return Content("filename not present");

  //      var path = Path.Combine(
  //                  Directory.GetCurrentDirectory(), "FileUploaded", "Banner",
  //                  filename);

  //      if (System.IO.File.Exists(path))
  //      {
  //        System.IO.File.Delete(path);
  //      }
  //      return Ok(new
  //      {
  //        success = true,
  //        message = "Delete Success",
  //        data = ""
  //      });
  //    }
  //    catch (System.Exception e)
  //    {
  //      return Ok(new
  //      {
  //        success = false,
  //        message = e.Message,
  //        data = ""
  //      });
  //    }
  //  }

  //  [HttpGet("DeleteKlc/{filename}")]
  //  public IActionResult DeleteKlc(string filename)
  //  {
  //    try
  //    {
  //      if (filename == null)
  //        return Content("filename not present");

  //      var path = Path.Combine(
  //                  Directory.GetCurrentDirectory(), "FileUploaded", "KLC",
  //                  filename);

  //      if (System.IO.File.Exists(path))
  //      {
  //        System.IO.File.Delete(path);
  //      }
  //      return Ok(new
  //      {
  //        success = true,
  //        message = "Delete Success",
  //        data = ""
  //      });
  //    }
  //    catch (System.Exception e)
  //    {
  //      return Ok(new
  //      {
  //        success = false,
  //        message = e.Message,
  //        data = ""
  //      });
  //    }
  //  }

  //#region Move To His After 30 Day On Board Wait For Production Maybe When Gen Report Complete Trigger
  //List<TbmSession> tbmSessions = await _context.TbmSessions.Where(w => w.EndDateTime <= DateTime.Now.AddDays(-60)).ToListAsync();
  //tbmSessions.ForEach(ts =>
  //{
  //  List<TbmSessionUserHi> tbmSessionUserHis = new List<TbmSessionUserHi>();

  //  List<TbmSessionUser> tbmSessionUsers = _context.TbmSessionUsers.Where(w => w.SessionId == ts.SessionId).ToList();

  //  tbmSessionUsers.ForEach(tsu =>
  //  {
  //    tbmSessionUserHis.Add(new TbmSessionUserHi
  //    {
  //      SessionId = tsu.SessionId,
  //      UserId = tsu.UserId,
  //      RegistrationStatus = tsu.RegistrationStatus,
  //      Createdatetime = tsu.Createdatetime,
  //      UpdateBy = tsu.UpdateBy,
  //      UpdateDatetime = tsu.UpdateDatetime
  //    });
  //  });
  //  _context.TbmSessionUserHis.AddRange(tbmSessionUserHis);
  //  _context.TbmSessionUsers.RemoveRange(tbmSessionUsers);
  //});
  //await _context.SaveChangesAsync();
  //#endregion

}
