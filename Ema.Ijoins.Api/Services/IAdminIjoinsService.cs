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
    Task<object> ImportKlcData(KlcFileImportRequest tbmKlcFileImport);
    Task<List<ModelSessionsQR>> GetSessions(TbmSession tbmSession, string userId);
    Task<List<ModelNextSixDayDash>> GetNextSixDayDashs(string userId);
    Task<TbmSession> UpdateSession(TbmSession tbmSession);
    Task<List<TbmSession>> GetToDayClass(FetchSessions tbmSession, string userId);
    Task<List<TbmSession>> GetSevenDayClass(TbmSession tbmSession, string userId);
    Task<List<TbmSessionUser>> GetParticipant(TbmSessionUser tbmSessionUser);
    Task<TbmSessionUser> AddParticipant(TbmSessionUser tbmSessionUser);
    Task<TbmSessionUser> UpdateParticipant(TbmSessionUser tbmSessionUser);
    Task<object> DeleteParticipant(TbmSessionUser tbmSessionUser);
    bool TbmSessionUsersExists(string SessionId, string UserId);
    bool TbmSessionExists(string SessionId);

    Task<List<TbmSession>> GetReportSessions(TbmSession tbmSession, string userId);
    Task<List<ModelReport>> GetReport(TbmSession tbmSession);

    Task<List<TbmCompany>> GetUserCompany(string userId);
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
        var tbKlcDatasCheck = tbKlcDatas.Select(d => new { d.CourseId, d.CourseName, d.SessionId, d.SessionName, d.StartDateTime, d.EndDateTime })
                                                          .Distinct()
                                                          .ToList();

        var tbmCourseTypes = await _context.TbmCourseTypes.ToListAsync();

        List<TbKlcDataMaster> tbKlcDatasInvalid = Utility.ValidateData(tbKlcDatas, tbmCourseTypes);

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
          DataCheck = tbKlcDatasCheck,
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

    public async Task<object> ImportKlcData(KlcFileImportRequest tbmKlcFileImport)
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

          if (tbmSession != null) _userIjoinsService.RemoveSegmentUser(new Segment { SessionId = tbmSession.SessionId });

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
          //int CourseTypeId;
          //if (!TbmCourseTypeExists(klcDataMaster.CourseType))
          //{
          //  TbmCourseType tbmCourseType = new TbmCourseType { CourseType = klcDataMaster.CourseType, CourseId = klcDataMaster.CourseId };
          //  _context.TbmCourseTypes.Add(tbmCourseType);
          //  await _context.SaveChangesAsync();
          //  CourseTypeId = tbmCourseType.Id;
          //}
          //else
          //{
          //  TbmCourseType tbmCourseType = await _context.TbmCourseTypes.Where(w => w.CourseType == klcDataMaster.CourseType).FirstOrDefaultAsync();
          //  CourseTypeId = tbmCourseType.Id;
          //}

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
              CourseType = klcDataMaster.CourseType,

              CompanyId = tbmKlcFileImport.CompanyId,
              CompanyCode = tbmKlcFileImport.CompanyCode,

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
            tbmSession.CourseType = klcDataMaster.CourseType;

            tbmSession.CompanyId = tbmKlcFileImport.CompanyId;
            tbmSession.CompanyCode = tbmKlcFileImport.CompanyCode;

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

              StartDate = klcDataMaster.StartDateTime.ToString("yyyyMMdd"),
              EndDate = klcDataMaster.EndDateTime.ToString("yyyyMMdd"),

              StartTime = klcDataMaster.StartDateTime.ToString("HHmm"),
              EndTime = klcDataMaster.EndDateTime.ToString("HHmm"),

              StartDateTime = klcDataMaster.StartDateTime,
              EndDateTime = klcDataMaster.EndDateTime,
              Venue = klcDataMaster.Venue,
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

            tbmSegment.StartDate = klcDataMaster.StartDateTime.ToString("yyyyMMdd");
            tbmSegment.EndDate = klcDataMaster.EndDateTime.ToString("yyyyMMdd");

            tbmSegment.StartTime = klcDataMaster.StartDateTime.ToString("HHmm");
            tbmSegment.EndTime = klcDataMaster.EndDateTime.ToString("HHmm");

            tbmSegment.Venue = klcDataMaster.Venue;
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

          List<VSegmentGenQr> tbmSegments = await _context.VSegmentGenQrs.Where(w => w.SessionId == ts.SessionId).ToListAsync();
          foreach (VSegmentGenQr se in tbmSegments)
          {
            _userIjoinsService.CreateSegment(new Segment
            {
              SessionId = se.SessionId,
              //SegmentNo = se.SegmentNo,
              SegmentName = se.SegmentName,
              StartDate = se.StartDateTime.Value.ToString("yyyyMMdd"),
              EndDate = se.EndDateTime.Value.ToString("yyyyMMdd"),
              StartTime = se.StartDateTime.Value.ToString("HHmm"),
              EndTime = se.EndDateTime.Value.ToString("HHmm"),
              StartDateTime = se.StartDateTime.Value,
              EndDateTime = se.EndDateTime.Value,
              Venue = se.Venue,
              Createdatetime = DateTime.Now
            });

          }

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

    public async Task<List<ModelSessionsQR>> GetSessions(TbmSession tbmSession, string userId)
    {
      CultureInfo enUS = new CultureInfo("en-US");
      DateTime.TryParseExact(DateTime.Now.ToString("yyyyMMdd") + " " + "01AM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime StartDay);

      var tbmSessions = await _context.TbmSessions
        .Where(
                w => w.EndDateTime >= StartDay
                && w.SessionId.Contains(tbmSession.SessionId)
              )
        .OrderByDescending(o => o.StartDateTime).ToListAsync();

      var tbUserCompanies = await _context.TbUserCompanies.Where(w => w.UserId == userId).ToListAsync();
      var tbmSessionsCompanies = (
            from s in tbmSessions
            where tbUserCompanies.Select(x => x.CompanyId).Contains(s.CompanyId)
            select s
            );

      //var sess = tbmSessionsCompanies.ToList();

      List<ModelSessionsQR> segmentsQRs = new List<ModelSessionsQR>();
      foreach (TbmSession session in tbmSessionsCompanies.ToList())
      {

        List<VSegmentGenQr> vSegmentGenQrs = await _context.VSegmentGenQrs.Where(w => w.SessionId == session.SessionId).OrderBy(o => o.StartDateTime).ToListAsync();

        segmentsQRs.Add(new ModelSessionsQR
        {
          FileId = session.FileId,
          CourseTypeId = session.FileId,
          CompanyCode = session.CompanyCode,
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

    public async Task<List<ModelNextSixDayDash>> GetNextSixDayDashs(string userId)
    {
      List<ModelNextSixDayDash> retNextSixDayDash = new List<ModelNextSixDayDash>();
      CultureInfo enUS = new CultureInfo("en-US");
      var session = await _context.TbmSessions.Where(
        w =>
        w.IsCancel == '0'
        && w.EndDateTime >= DateTime.Now
        ).OrderBy(o => o.StartDateTime).ToListAsync();

      var tbUserCompanies = await _context.TbUserCompanies.Where(w => w.UserId == userId).ToListAsync();
      var tbmSessionsCompanies = (
            from s in session
            where tbUserCompanies.Select(x => x.CompanyId).Contains(s.CompanyId)
            select s
            );

      for (int i = 1; i <= 6; i++)
      {
        DateTime date = DateTime.Now.AddDays(i);
        DateTime.TryParseExact(date.ToString("yyyyMMdd") + " " + "01AM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime StartDay);
        DateTime.TryParseExact(date.ToString("yyyyMMdd") + " " + "11PM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime EndDay);
        retNextSixDayDash.Add(new ModelNextSixDayDash
        {
          DateTime = date,
          AddDay = i,
          SessionCount = tbmSessionsCompanies.ToList().Where(w => w.StartDateTime <= EndDay && w.EndDateTime >= StartDay).ToList().Count.ToString()
        });
      }

      return retNextSixDayDash;
    }
    public async Task<List<TbmSession>> GetToDayClass(FetchSessions tbmSession, string userId)
    {

      CultureInfo enUS = new CultureInfo("en-US");
      DateTime.TryParseExact(DateTime.Now.AddDays(tbmSession.AddDay).ToString("yyyyMMdd") + " " + "01AM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime StartDay);
      DateTime.TryParseExact(DateTime.Now.AddDays(tbmSession.AddDay).ToString("yyyyMMdd") + " " + "11PM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime EndDay);

      string today = DateTime.Now.AddDays(tbmSession.AddDay).ToString("yyyyMMdd");

      var session = await _context.TbmSessions.Where(
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

      var tbUserCompanies = await _context.TbUserCompanies.Where(w => w.UserId == userId).ToListAsync();
      var tbmSessionsCompanies = (
            from s in session
            where tbUserCompanies.Select(x => x.CompanyId).Contains(s.CompanyId)
            select s
            );

      foreach (TbmSession sessionItem in tbmSessionsCompanies.ToList())
      {
        var segment = await _context.TbmSegments.Where(
        w => w.SessionId == sessionItem.SessionId &&
          w.StartDate == today &&
          w.EndDate == today
        ).FirstOrDefaultAsync();

        if (segment != null)
          sessionItem.Venue = segment.Venue;

        sessionItem.Createdatetime = DateTime.Now.AddDays(tbmSession.AddDay);
        sessionItem.TbmSegments = null;
      }

      return tbmSessionsCompanies.ToList();
    }
    public async Task<List<TbmSession>> GetSevenDayClass(TbmSession tbmSession, string userId)
    {
      CultureInfo enUS = new CultureInfo("en-US");
      //DateTime.TryParseExact(DateTime.Now.ToString("yyyyMMdd") + " " + "01AM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime StartDay);
      DateTime.TryParseExact(DateTime.Now.ToString("yyyyMMdd") + " " + "11PM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime EndDay);
      DateTime nextSevenDate = DateTime.Now.AddDays(7);

      var session = await _context.TbmSessions.Where(
        w =>
        w.IsCancel == '0'
        && (w.EndDateTime > EndDay && w.StartDateTime <= nextSevenDate)
        && (
           w.CourseId.Contains(tbmSession.CourseId)
        || w.CourseName.Contains(tbmSession.CourseId)
        || w.CourseName.Contains(tbmSession.CourseId.ToLower())
        || w.CourseName.Contains(tbmSession.CourseId.ToUpper())
        )
        ).OrderBy(o => o.StartDateTime).ToListAsync();

      var tbUserCompanies = await _context.TbUserCompanies.Where(w => w.UserId == userId).ToListAsync();
      var tbmSessionsCompanies = (
            from s in session
            where tbUserCompanies.Select(x => x.CompanyId).Contains(s.CompanyId)
            select s
            );

      return tbmSessionsCompanies.ToList();
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
          .OrderBy(o => o.UserId).ToListAsync(); //order by name asc
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


    public async Task<List<TbmSession>> GetReportSessions(TbmSession tbmSession, string userId)
    {
      CultureInfo enUS = new CultureInfo("en-US");
      DateTime.TryParseExact(DateTime.Now.ToString("yyyyMMdd") + " " + "01AM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime StartDay);

      var tbmSessions = await _context.TbmSessions
        .Where(
                w =>
                //w.EndDateTime >= StartDay && 
                w.SessionId.Contains(tbmSession.SessionId)
              )
        .OrderByDescending(o => o.StartDateTime).ToListAsync();

      var tbUserCompanies = await _context.TbUserCompanies.Where(w => w.UserId == userId).ToListAsync();
      var tbmSessionsCompanies = (
            from s in tbmSessions
            where tbUserCompanies.Select(x => x.CompanyId).Contains(s.CompanyId)
            select s
            );

      return tbmSessionsCompanies.ToList();
    }
    public async Task<List<ModelReport>> GetReport(TbmSession tbmSession)
    {
      var sessionData = await _context.TbmSessions
          .Where(
                 w => w.SessionId == tbmSession.SessionId
                )
          .FirstOrDefaultAsync();

      var segmentData = await _context.VSegmentGenQrs.Where(w => w.SessionId == tbmSession.SessionId).OrderBy(o => o.StartDateTime).ToListAsync();

      var tbmSessionUsers = await _context.TbmSessionUsers
          .Where(
                 w => w.SessionId == tbmSession.SessionId
                )
          .OrderBy(o => o.UserId).ToListAsync();

      List<ModelReport> reportList = new List<ModelReport>();
      int row = 1;
      foreach (TbmSessionUser su in tbmSessionUsers)
      {

        string SegmentTrainingStatus = "";
        string CurrentTrainingStatus = "";
        string FinalTrainingStatus = "";
        DateTime CheckInDateTime = DateTime.MinValue;
        DateTime CheckOutDateTime = DateTime.MinValue;
        var userRegistrations = await _userIjoinsService.GetUserRegistration(new UserRegistration { SessionId = su.SessionId, UserId = su.UserId });

        List<ModelSegmentReport> segmentReports = new List<ModelSegmentReport>();

        double realMinuteAttend = 0;
        double checkMinuteSegment = 0;
        double percentAttend = 0;
        foreach (VSegmentGenQr sg in segmentData)
        {
          if (sg.StartDateTime.HasValue && sg.EndDateTime.HasValue)
          {
            TimeSpan ts = sg.EndDateTime.Value - sg.StartDateTime.Value;
            checkMinuteSegment += ts.TotalMinutes;
          }

          var userRegis = userRegistrations.Where(w => w.CheckInDate == sg.StartDateTime.Value.ToString("yyyyMMdd")).FirstOrDefault();
          if (userRegis != null)
          {

            segmentReports.Add(new ModelSegmentReport
            {
              CheckInDateTime = userRegis.IsCheckIn == '1' ? userRegis.CheckInDateTime.ToLocalTime().ToString("hh:mm tt") : "",
              CheckOutDateTime = userRegis.IsCheckOut == '1' ? userRegis.CheckOutDateTime.ToLocalTime().ToString("hh:mm tt") : "",
              StartDateTime = sg.StartDateTime.Value.ToString("dd'/'MM'/'yyyy"),
              EndDateTime = sg.EndDateTime.Value.ToString("dd'/'MM'/'yyyy")
            });

            if (userRegis.CheckInDateTime.ToLocalTime() < sg.StartDateTime)
            {
              CheckInDateTime = sg.StartDateTime.Value;
            }
            else
            {
              CheckInDateTime = userRegis.CheckInDateTime.ToLocalTime();
            }

            if (userRegis.CheckOutDateTime.ToLocalTime() > sg.EndDateTime)
            {
              CheckOutDateTime = sg.EndDateTime.Value;
            }
            else
            {
              CheckOutDateTime = userRegis.CheckOutDateTime.ToLocalTime();
            }

            if (userRegis.CheckInDateTime != DateTime.MinValue && userRegis.CheckOutDateTime != DateTime.MinValue)
            {
              TimeSpan ts = CheckOutDateTime - CheckInDateTime;
              if (ts.TotalMinutes > 0)
                realMinuteAttend += ts.TotalMinutes;
            }
          }
          else
          {
            segmentReports.Add(new ModelSegmentReport
            {
              CheckInDateTime = "",
              CheckOutDateTime = "",
              StartDateTime = sg.StartDateTime.Value.ToString("dd'/'MM'/'yyyy"),
              EndDateTime = sg.EndDateTime.Value.ToString("dd'/'MM'/'yyyy")
            });
          }
        }

        if (checkMinuteSegment > double.Parse(sessionData.CourseCreditHours) * 60)
        {
          realMinuteAttend -= checkMinuteSegment - (double.Parse(sessionData.CourseCreditHours) * 60);
        }
        percentAttend = (realMinuteAttend / (double.Parse(sessionData.CourseCreditHours) * 60)) * 100;
        if (percentAttend >= (double.Parse(sessionData.PassingCriteriaException) * 100))
        {
          SegmentTrainingStatus = "Completed";
        }
        else
        {
          SegmentTrainingStatus = "Incompleted";
        }


        bool isNoshow = true;
        bool isIncomplete = true;
        foreach (ModelSegmentReport segmentReport in segmentReports)
        {
          if (su.RegistrationStatus == "Cancelled")
          {
            FinalTrainingStatus = "Cancelled";
          }
          else if (segmentReport.CheckInDateTime != "" && segmentReport.CheckOutDateTime == "")
          {
            isNoshow = false;
          }
          else if (segmentReport.CheckInDateTime != "" && segmentReport.CheckOutDateTime != "")
          {
            isNoshow = false;
            isIncomplete = false;
          }
        }
        if (su.RegistrationStatus == "Cancelled")
        {
          FinalTrainingStatus = "Cancelled";
        }
        else if (isNoshow)
        {
          FinalTrainingStatus = "No Show";
        }
        else if (isIncomplete)
        {
          FinalTrainingStatus = "Incompleted";
        }
        else
        {
          FinalTrainingStatus = SegmentTrainingStatus;
        }



        int isRowSpan = 0;
        foreach (ModelSegmentReport segmentReport in segmentReports)
        {
          isRowSpan++;

          if (su.RegistrationStatus == "Cancelled")
          {
            CurrentTrainingStatus = su.RegistrationStatus;
          }
          else if (segmentReport.CheckInDateTime == "" && segmentReport.CheckOutDateTime == "")
          {
            CurrentTrainingStatus = su.RegistrationStatus;
          }
          else if (segmentReport.CheckInDateTime != "" && segmentReport.CheckOutDateTime == "")
          {
            CurrentTrainingStatus = "Check-in";
          }
          else if (segmentReport.CheckInDateTime != "" && segmentReport.CheckOutDateTime != "")
          {
            CurrentTrainingStatus = "Check-out";
          }

          CultureInfo enUS = new CultureInfo("en-US");
          DateTime.TryParseExact(sessionData.EndDateTime.ToString("yyyyMMdd") + " " + "11PM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime EndDay);

          reportList.Add(new ModelReport
          {
            No = row,
            UserId = su.UserId,
            UserNameSurname = "",
            UserDepartment = "",
            UserCompany = "",
            Date = segmentReport.StartDateTime,
            CheckInTime = segmentReport.CheckInDateTime,
            CheckOutTime = segmentReport.CheckOutDateTime,
            TrainingStatus = DateTime.Now > EndDay ? FinalTrainingStatus : CurrentTrainingStatus,
            Segments = isRowSpan == 1 ? segmentReports : null
          });
        }

        row++;
      }

      return reportList;
    }

    public async Task<List<TbmCompany>> GetUserCompany(string userId)
    {
      var tbUserCompanies = await _context.TbUserCompanies
          .Where(
                 w => w.UserId == userId
                )
          .OrderByDescending(o => o.IsDefault)
          .ToListAsync();

      var tbmCompanies = await _context.TbmCompanies.ToListAsync();

      //var tbmCompanies = (
      //      from c in _context.TbmCompanies
      //      where tbUserCompanies.Select(x => x.CompanyId).Contains(c.CompanyId)
      //      select c
      //      );

      //return tbUserCompanies;

      var query = from UserCom in tbUserCompanies
                  join Com in tbmCompanies on UserCom.CompanyId equals Com.CompanyId into UserComGroup
                  from uc in UserComGroup.DefaultIfEmpty()
                  select new { UserCom.CompanyId, uc.CompanyCode, UserCom.IsDefault }
                  ;

      List<TbmCompany> retUserCom = new List<TbmCompany>();
      foreach (var v in query.OrderByDescending(o => o.IsDefault))
      {
        retUserCom.Add(new TbmCompany { CompanyId = v.CompanyId, CompanyCode = v.CompanyCode });
      }


      return retUserCom;
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
