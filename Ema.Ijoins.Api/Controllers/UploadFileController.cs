using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Ema.Ijoins.Api.Helpers;
using System.Net.Http.Headers;
using Ema.Ijoins.Api.EfModels;

namespace Ema.Ijoins.Api.Controllers
{
  [Route("[controller]")]
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

    //[HttpGet]
    //public IActionResult GetUploadFile()
    //{
    //  var model = new FilesViewModel();
    //  foreach (var item in this.fileProvider.GetDirectoryContents("Knowledge"))
    //  {
    //    model.Files.Add(
    //        new FileDetails { Name = item.Name, Path = item.PhysicalPath });
    //  }
    //  return Ok(new
    //  {
    //    success = true,
    //    message = "",
    //    data = model
    //  });
    //}


    [HttpPost("UploadFile")]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
      try
      {
        if (file == null || file.Length == 0)
          return Content("file not selected");

        var path = Path.Combine(
                    Directory.GetCurrentDirectory(), "FileUploaded", "KLC",
                    file.GetFilename());



        using (var stream = new FileStream(path, FileMode.Create))
        {
          await file.CopyToAsync(stream);
        }

        TbmKlcFileImport attachFiles = new TbmKlcFileImport
        {
          Filename = file.GetFilename()
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


    [HttpPost("UploadFiles")]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> UploadFiles(List<IFormFile> files)
    {
      try
      {
        if (files == null || files.Count == 0)
          return Content("files not selected");

        foreach (var file in files)
        {
          var path = Path.Combine(
                  Directory.GetCurrentDirectory(), "FileUploaded",
                  file.GetFilename());

          using (var stream = new FileStream(path, FileMode.Create))
          {
            await file.CopyToAsync(stream);
          }
        }

        return RedirectToAction("Files");
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



    //[HttpPost("AttachFiles"), DisableRequestSizeLimit]
    //public async Task<IActionResult> AttachFiles(List<IFormFile> files)
    //{
    //  try
    //  {
    //    if (files == null || files.Count == 0)
    //      return Content("files not selected");

    //    List<MerchantEmailAttachFile> attachFiles = new List<MerchantEmailAttachFile>();
    //    foreach (var file in files)
    //    {
    //      Guid guid = Guid.NewGuid();

    //      var path = Path.Combine(
    //              Directory.GetCurrentDirectory(), "FileUploaded",
    //              file.GetFilename());

    //      var ext = Path.GetExtension(path).ToLowerInvariant();

    //      var pathGuid = Path.Combine(
    //              Directory.GetCurrentDirectory(), "FileUploaded",
    //              guid.ToString() + ext);

    //      using (var stream = new FileStream(pathGuid, FileMode.Create))
    //      {
    //        await file.CopyToAsync(stream);
    //      }

    //      attachFiles.Add(new MerchantEmailAttachFile
    //      {
    //        Filename = file.GetFilename(),
    //        Guidname = guid.ToString() + ext
    //      });
    //    }

    //    return Ok(new
    //    {
    //      success = true,
    //      message = "",
    //      data = attachFiles
    //    });
    //  }
    //  catch (System.Exception e)
    //  {
    //    return Ok(new
    //    {
    //      success = false,
    //      message = e.Message,
    //      data = ""
    //    });
    //  }
    //}


    //public IActionResult Files()
    //{
    //  var model = new FilesViewModel();
    //  foreach (var item in this.fileProvider.GetDirectoryContents("Knowledge"))
    //  {
    //    model.Files.Add(
    //        new FileDetails { Name = item.Name, Path = item.PhysicalPath });
    //  }
    //  return Ok(new
    //  {
    //    success = true,
    //    message = "",
    //    data = model
    //  });
    //}

    //[HttpGet("DownloadKnowledge/{filename}")]
    //public async Task<IActionResult> DownloadKnowledge(string filename)
    //{
    //  try
    //  {
    //    if (filename == null)
    //      return Content("filename not present");

    //    var path = Path.Combine(
    //                   Directory.GetCurrentDirectory(),
    //                   "FileUploaded", "Knowledge", filename);

    //    var memory = new MemoryStream();
    //    using (var stream = new FileStream(path, FileMode.Open))
    //    {
    //      await stream.CopyToAsync(memory);
    //    }
    //    memory.Position = 0;
    //    return File(memory, GetContentType(path), Path.GetFileName(path));
    //  }
    //  catch (System.Exception e)
    //  {
    //    return Ok(new
    //    {
    //      success = false,
    //      message = e.Message,
    //      data = ""
    //    });
    //  }
    //}

    //[HttpGet("DownloadFile/{filename}")]
    //public async Task<IActionResult> Download(string filename)
    //{
    //  try
    //  {
    //    if (filename == null)
    //      return Content("filename not present");

    //    var path = Path.Combine(
    //                   Directory.GetCurrentDirectory(),
    //                   "FileUploaded", filename);

    //    var memory = new MemoryStream();
    //    using (var stream = new FileStream(path, FileMode.Open))
    //    {
    //      await stream.CopyToAsync(memory);
    //    }
    //    memory.Position = 0;
    //    return File(memory, GetContentType(path), Path.GetFileName(path));
    //  }
    //  catch (System.Exception e)
    //  {
    //    return Ok(new
    //    {
    //      success = false,
    //      message = e.Message,
    //      data = ""
    //    });
    //  }
    //}

    //[HttpGet("DeleteFileKnowledge/{filename}/{id}")]
    //public async Task<IActionResult> DeleteKnowledge(string filename, int id)
    //{
    //  try
    //  {
    //    if (filename == null)
    //      return Content("filename not present");

    //    var path = Path.Combine(
    //                Directory.GetCurrentDirectory(), "FileUploaded", "Knowledge",
    //                filename);

    //    if (System.IO.File.Exists(path))
    //    {
    //      System.IO.File.Delete(path);
    //    }


    //    var knowledgeAttachFile = await _context.MerchantKnowledgeAttachFile.FindAsync(id);
    //    if (knowledgeAttachFile == null)
    //    {
    //      return NotFound();
    //    }

    //    _context.MerchantKnowledgeAttachFile.Remove(knowledgeAttachFile);
    //    await _context.SaveChangesAsync();

    //    return Ok(new
    //    {
    //      success = true,
    //      message = "Delete Success",
    //      data = ""
    //    });
    //  }
    //  catch (System.Exception e)
    //  {
    //    return Ok(new
    //    {
    //      success = false,
    //      message = e.Message,
    //      data = ""
    //    });
    //  }
    //}

    //[HttpGet("DeleteFile/{filename}")]
    //public IActionResult Delete(string filename)
    //{
    //  try
    //  {
    //    if (filename == null)
    //      return Content("filename not present");

    //    var path = Path.Combine(
    //                Directory.GetCurrentDirectory(), "FileUploaded",
    //                filename);

    //    if (System.IO.File.Exists(path))
    //    {
    //      System.IO.File.Delete(path);
    //    }
    //    return Ok(new
    //    {
    //      success = true,
    //      message = "Delete Success",
    //      data = ""
    //    });
    //  }
    //  catch (System.Exception e)
    //  {
    //    return Ok(new
    //    {
    //      success = false,
    //      message = e.Message,
    //      data = ""
    //    });
    //  }
    //}

    //private string GetContentType(string path)
    //{
    //  var types = GetMimeTypes();
    //  var ext = Path.GetExtension(path).ToLowerInvariant();
    //  return types[ext];
    //}

    //private Dictionary<string, string> GetMimeTypes()
    //{
    //  return new Dictionary<string, string>
    //        {
    //            {".txt", "text/plain"},
    //            {".pdf", "application/pdf"},
    //            {".doc", "application/vnd.ms-word"},
    //            {".docx", "application/vnd.ms-word"},
    //            {".xls", "application/vnd.ms-excel"},
    //            {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
    //            {".png", "image/png"},
    //            {".jpg", "image/jpeg"},
    //            {".jpeg", "image/jpeg"},
    //            {".gif", "image/gif"},
    //            {".csv", "text/csv"}
    //        };
    //}

  }
}
