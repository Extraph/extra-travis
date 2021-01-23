using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Ema.Ijoins.Api.Helpers;
using Ema.Ijoins.Api.EfModels;

namespace Ema.Ijoins.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UploadFileController : ControllerBase
  {
    private readonly IFileProvider _fileProvider;
    private readonly ema_databaseContext _context;

    public UploadFileController(IFileProvider fileProvider, ema_databaseContext context)
    {
      _fileProvider = fileProvider;
      _context = context;
    }



    [HttpPost("UploadFileKlc")]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> UploadFileKlc(IFormFile file)
    {
      using var transaction = _context.Database.BeginTransaction();

      try
      {

        if (file == null || file.Length == 0)
          return Content("file not selected");

        Guid guid = Guid.NewGuid();

        var path = Path.Combine(
                    Directory.GetCurrentDirectory(), "FileUploaded", "KLC",
                    file.GetFilename());

        var ext = Path.GetExtension(path).ToLowerInvariant();

        var pathGuid = Path.Combine(
                  Directory.GetCurrentDirectory(), "FileUploaded", "KLC",
                  guid.ToString() + ext);

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

        return Ok(new
        {
          success = true,
          message = strMessage,
          dataInvalid = tbKlcDatasInvalid,
          fileUploadId = attachFiles.Id,
          totalNo = tbKlcDatas.Count,
          validNo = tbKlcDatas.Count - tbKlcDatasInvalid.Count,
          invalidNo = tbKlcDatasInvalid.Count
        });
      }
      catch (System.Exception e)
      {
        await transaction.RollbackToSavepointAsync("UploadFileSuccess");
        return Ok(new
        {
          success = false,
          message = e.Message
        });
      }
    }

    [HttpPost("ImportKlcData")]
    public async Task<IActionResult> ImportKlcData(TbmKlcFileImport tbmKlcFileImport)
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

        tbKlcDataMastersGroupsBySessionId.ToList().ForEach(ts =>
        {
          TbmSession tbmSession = _context.TbmSessions.Where(w => w.SessionId == ts.SessionId).FirstOrDefault();
          if (tbmSession != null) _context.TbmSegments.RemoveRange(_context.TbmSegments.Where(w => w.SessionId == tbmSession.SessionId).ToList());
          if (tbmKlcFileImport.ImportType == "Upload session and participants")
          {
            if (tbmSession != null) _context.TbmSessionUsers.RemoveRange(_context.TbmSessionUsers.Where(w => w.SessionId == tbmSession.SessionId).ToList());
            if (tbmSession != null) _context.TbmSessionUserHis.RemoveRange(_context.TbmSessionUserHis.Where(w => w.SessionId == tbmSession.SessionId).ToList());
          }
          _context.SaveChanges();
        });



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

        tbKlcDataMastersGroupsBySessionId.ToList().ForEach(ts =>
        {
          TbmSession tbmSession = _context.TbmSessions.Where(w => w.SessionId == ts.SessionId).FirstOrDefault();
          if (tbmSession != null)
          {
            List<TbmSegment> tbmSegments = _context.TbmSegments.Where(w => w.SessionId == ts.SessionId).ToList();

            DateTime minStartDate = tbmSegments.OrderBy(o => o.StartDateTime).FirstOrDefault().StartDateTime;
            DateTime maxEndDate = tbmSegments.OrderByDescending(o => o.EndDateTime).FirstOrDefault().EndDateTime;
            tbmSession.StartDateTime = minStartDate;
            tbmSession.EndDateTime = maxEndDate;
            _context.Entry(tbmSession).State = EntityState.Modified;
            _context.SaveChanges();
          }
        });


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

        return Ok(new
        {
          success = true,
          message = "",
          dataList = tbmKlcFileImports
        });
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
          return Ok(new
          {
            success = false,
            message = ex.Message
          });
        }

        return Ok(new
        {
          success = false,
          message = e.Message
        });
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
    private bool TbmSessionExists(string SessionId)
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
    private bool TbmSessionUsersExists(string SessionId, string UserId)
    {
      return _context.TbmSessionUsers.Any(
        e =>
         e.SessionId == SessionId
      && e.UserId == UserId
      );
    }


    [HttpGet("GetKlc")]
    public IActionResult GetKlc()
    {
      var model = new FilesViewModel();
      foreach (var item in _fileProvider.GetDirectoryContents("KLC"))
      {
        model.Files.Add(
            new FileDetails { Name = item.Name, Path = item.PhysicalPath });
      }
      return Ok(new
      {
        success = true,
        message = "",
        data = model
      });
    }

    [HttpGet("GetBanner")]
    public IActionResult GetBanner()
    {
      var model = new FilesViewModel();
      foreach (var item in _fileProvider.GetDirectoryContents("Banner"))
      {
        model.Files.Add(
            new FileDetails { Name = item.Name, Path = item.PhysicalPath });
      }
      return Ok(new
      {
        success = true,
        message = "",
        data = model
      });
    }

    [HttpPost("UploadFileBanner")]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> UploadFileBanner(IFormFile file)
    {
      try
      {
        if (file == null || file.Length == 0)
          return Content("file not selected");

        Guid guid = Guid.NewGuid();

        var path = Path.Combine(
                    Directory.GetCurrentDirectory(), "FileUploaded", "Banner",
                    file.GetFilename());

        var ext = Path.GetExtension(path).ToLowerInvariant();

        var pathGuid = Path.Combine(
                  Directory.GetCurrentDirectory(), "FileUploaded", "Banner",
                  guid.ToString() + ext);

        using (var stream = new FileStream(pathGuid, FileMode.Create))
        {
          await file.CopyToAsync(stream);
        }

        TbmKlcFileImport attachFiles = new TbmKlcFileImport
        {
          FileName = file.GetFilename(),
          GuidName = guid.ToString() + ext
        };

        return Ok(new
        {
          success = true,
          message = "",
          data = attachFiles
        });
      }
      catch (System.Exception e)
      {
        return Ok(new
        {
          success = false,
          message = e.Message,
          data = ""
        });
      }
    }

    [HttpGet("DeleteBanner/{filename}")]
    public IActionResult DeleteBanner(string filename)
    {
      try
      {
        if (filename == null)
          return Content("filename not present");

        var path = Path.Combine(
                    Directory.GetCurrentDirectory(), "FileUploaded", "Banner",
                    filename);

        if (System.IO.File.Exists(path))
        {
          System.IO.File.Delete(path);
        }
        return Ok(new
        {
          success = true,
          message = "Delete Success",
          data = ""
        });
      }
      catch (System.Exception e)
      {
        return Ok(new
        {
          success = false,
          message = e.Message,
          data = ""
        });
      }
    }

    [HttpGet("DeleteKlc/{filename}")]
    public IActionResult DeleteKlc(string filename)
    {
      try
      {
        if (filename == null)
          return Content("filename not present");

        var path = Path.Combine(
                    Directory.GetCurrentDirectory(), "FileUploaded", "KLC",
                    filename);

        if (System.IO.File.Exists(path))
        {
          System.IO.File.Delete(path);
        }
        return Ok(new
        {
          success = true,
          message = "Delete Success",
          data = ""
        });
      }
      catch (System.Exception e)
      {
        return Ok(new
        {
          success = false,
          message = e.Message,
          data = ""
        });
      }
    }


  }
}
