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
        IEnumerable<TbmKlcFileImport> tbmKlcFileImports = await _context.TbmKlcFileImports.OrderByDescending(o => o.Createdatetime).ToListAsync();

        await transaction.CommitAsync();
        return Ok(new
        {
          success = true,
          message = "",
          dataList = tbmKlcFileImports
        });
      }
      catch (System.Exception e)
      {
        await transaction.RollbackAsync();
        return Ok(new
        {
          success = false,
          message = e.Message
        });
      }
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
