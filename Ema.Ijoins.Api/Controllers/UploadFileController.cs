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
using System.Net.Http.Headers;
using Ema.Ijoins.Api.EfModels;

namespace Ema.Ijoins.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UploadFileController : ControllerBase
  {
    private readonly IFileProvider fileProvider;
    private readonly ema_databaseContext _context;

    public UploadFileController(IFileProvider fileProvider, ema_databaseContext context)
    {
      this.fileProvider = fileProvider;
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

        List<TbKlcDataMasterHi> klcDataMasterHi = Utility.MoveDataToHis(_context.TbKlcDataMasters.ToList());
        _context.TbKlcDataMasterHis.AddRange(klcDataMasterHi);
        await _context.SaveChangesAsync();

        _context.TbKlcDataMasters.RemoveRange(_context.TbKlcDataMasters.ToList());
        await _context.SaveChangesAsync();


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
        foreach (TbKlcDataMaster klcDataMaster in tbKlcDataMasters)
        {
          int CourseTypeId;
          if (!TbmCourseTypeExists(klcDataMaster.CourseType))
          {
            TbmCourseType tbmCourseType = new TbmCourseType { CourseType = klcDataMaster.CourseType };
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
            TbmCourse tbmCourse = new TbmCourse { CourseId = int.Parse(klcDataMaster.CourseId), CourseName = klcDataMaster.CourseName, CourseNameTh = klcDataMaster.CourseNameTh };
            _context.TbmCourses.Add(tbmCourse);
            await _context.SaveChangesAsync();
          }

          if (!TbmSessionExists(klcDataMaster.SessionId))
          {
            TbmSession tbmSession = new TbmSession { SessionId = int.Parse(klcDataMaster.SessionId), SessionName = klcDataMaster.SessionName };
            _context.TbmSessions.Add(tbmSession);
            await _context.SaveChangesAsync();
          }

          if (!TbmSegmentExists(klcDataMaster.StartDateTime, klcDataMaster.EndDateTime, klcDataMaster.SessionId, klcDataMaster.CourseId))
          {
            TbmSegment tbmSegment = new TbmSegment
            {
              StartDateTime = klcDataMaster.StartDateTime,
              EndDateTime = klcDataMaster.EndDateTime,
              SessionId = int.Parse(klcDataMaster.SessionId),
              SessionName = klcDataMaster.SessionName,
              CourseId = int.Parse(klcDataMaster.CourseId),
              CourseName = klcDataMaster.CourseName,
              CourseNameTh = klcDataMaster.CourseNameTh,
              CourseOwnerEmail = klcDataMaster.CourseOwnerEmail,
              CourseOwnerContactNo = klcDataMaster.CourseOwnerContactNo,
              Venue = klcDataMaster.Venue,
              Instructor = klcDataMaster.Instructor,
              CourseCreditHours = klcDataMaster.CourseCreditHours,
              PassingCriteriaException = klcDataMaster.PassingCriteriaException
            };
            _context.TbmSegments.Add(tbmSegment);
            await _context.SaveChangesAsync();
          }

          if (!TbmRegistrationStatusExists(klcDataMaster.RegistrationStatus))
          {
            TbmRegistrationStatus tbmRegistrationStatus = new TbmRegistrationStatus { RegistrationStatus = klcDataMaster.RegistrationStatus };
            _context.TbmRegistrationStatuses.Add(tbmRegistrationStatus);
            await _context.SaveChangesAsync();
          }

          if (!TbtIjoinScanQrExists(klcDataMaster.StartDateTime, klcDataMaster.EndDateTime, klcDataMaster.SessionId, klcDataMaster.CourseId, klcDataMaster.UserId))
          {
            TbtIjoinScanQr tbtIjoinScanQr = new TbtIjoinScanQr
            {
              Id = klcDataMaster.Id,
              FileId = klcDataMaster.FileId,
              CourseTypeId = CourseTypeId,
              CourseId = int.Parse(klcDataMaster.CourseId),
              SessionId = int.Parse(klcDataMaster.SessionId),
              StartDateTime = klcDataMaster.StartDateTime,
              EndDateTime = klcDataMaster.EndDateTime,
              UserId = klcDataMaster.UserId,
              RegistrationStatus = klcDataMaster.RegistrationStatus
            };
            _context.TbtIjoinScanQrs.Add(tbtIjoinScanQr);
            await _context.SaveChangesAsync();
          }
          else
          {
            TbtIjoinScanQr tbtIjoinScanQrOld = await _context.TbtIjoinScanQrs.Where(
              w =>
              w.StartDateTime == klcDataMaster.StartDateTime
              && w.EndDateTime == klcDataMaster.EndDateTime
              && w.SessionId == int.Parse(klcDataMaster.SessionId)
              && w.CourseId == int.Parse(klcDataMaster.CourseId)
              && w.UserId == klcDataMaster.UserId
            ).FirstOrDefaultAsync();
            tbtIjoinScanQrOld.Id = klcDataMaster.Id;
            tbtIjoinScanQrOld.FileId = klcDataMaster.FileId;
            tbtIjoinScanQrOld.RegistrationStatus = klcDataMaster.RegistrationStatus;
            tbtIjoinScanQrOld.Updatedatetime = DateTime.Now;
            _context.Entry(tbtIjoinScanQrOld).State = EntityState.Modified;
            await _context.SaveChangesAsync();
          }
        }

        var klcFileImport = await _context.TbmKlcFileImports.FindAsync(tbmKlcFileImport.Id);
        klcFileImport.ImportTotalrecords = tbKlcDataMasters.Count().ToString();
        klcFileImport.Status = "import success";
        _context.Entry(klcFileImport).State = EntityState.Modified;
        await _context.SaveChangesAsync();


        List<TbKlcDataMasterHi> klcDataMasterHi = Utility.MoveDataToHis(tbKlcDataMasters.ToList());
        _context.TbKlcDataMasterHis.AddRange(klcDataMasterHi);
        await _context.SaveChangesAsync();
        _context.TbKlcDataMasters.RemoveRange(tbKlcDataMasters.ToList());
        await _context.SaveChangesAsync();



        //IEnumerable<TbtIjoinScanQr> tbtIjoinScanQrs = await _context.TbtIjoinScanQrs.Where(w => w.EndDateTime < DateTime.Now.AddDays(-7)).ToListAsync();
        //_context.TbtIjoinScanQrHis.AddRange(Utility.MoveDataIJoinToHis(tbtIjoinScanQrs.ToList()));
        //await _context.SaveChangesAsync();
        //_context.TbtIjoinScanQrs.RemoveRange(tbtIjoinScanQrs.ToList());
        //await _context.SaveChangesAsync();


        await transaction.CommitAsync();

        var tbmKlcFileImports = await _context.TbmKlcFileImports.OrderByDescending(o => o.Createdatetime).ToListAsync();
        tbmKlcFileImports.ForEach(w => { w.TbKlcDataMasters = null; w.TbKlcDataMasterHis = null; });

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
      return _context.TbmCourseTypes.Any(e => e.CourseType == CourseType);
    }
    private bool TbmCourseExists(string CourseId)
    {
      return _context.TbmCourses.Any(e => e.CourseId == int.Parse(CourseId));
    }
    private bool TbmSessionExists(string SessionId)
    {
      return _context.TbmSessions.Any(e => e.SessionId == int.Parse(SessionId));
    }
    private bool TbmSegmentExists(DateTime StartDateTime, DateTime EndDateTime, string SessionId, string CourseId)
    {
      return _context.TbmSegments.Any(
        e => e.StartDateTime == StartDateTime
      && e.EndDateTime == EndDateTime
      && e.SessionId == int.Parse(SessionId)
      && e.CourseId == int.Parse(CourseId)
      );
    }
    private bool TbmRegistrationStatusExists(String RegistrationStatus)
    {
      return _context.TbmRegistrationStatuses.Any(e => e.RegistrationStatus == RegistrationStatus);
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


    [HttpGet("GetKlc")]
    public IActionResult GetKlc()
    {
      var model = new FilesViewModel();
      foreach (var item in this.fileProvider.GetDirectoryContents("KLC"))
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
      foreach (var item in this.fileProvider.GetDirectoryContents("Banner"))
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
