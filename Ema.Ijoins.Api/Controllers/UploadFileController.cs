using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Ema.Ijoins.Api.Helpers;
using Ema.Ijoins.Api.EfModels;
using Ema.Ijoins.Api.Services;

namespace Ema.Ijoins.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UploadFileController : ControllerBase
  {
    private readonly IIjoinsService _ijoinsService;

    public UploadFileController(IIjoinsService ijoinsService)
    {
      _ijoinsService = ijoinsService;
    }


    [HttpPost("UploadFileKlc")]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> UploadFileKlc(IFormFile file)
    {

      object oRet = await _ijoinsService.UploadFileKlc(file);

      return Ok(oRet);

    }

    [HttpPost("ImportKlcData")]
    public async Task<IActionResult> ImportKlcData(TbmKlcFileImport tbmKlcFileImport)
    {

      object oRet = await _ijoinsService.ImportKlcData(tbmKlcFileImport);

      return Ok(oRet);

    }

  }
}
