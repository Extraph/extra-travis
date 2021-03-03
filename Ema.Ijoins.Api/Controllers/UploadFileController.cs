using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Ema.Ijoins.Api.Helpers;
using Ema.Ijoins.Api.Models;
using Ema.Ijoins.Api.Services;
using Ema.Ijoins.Api.EfAdminModels;

namespace Ema.Ijoins.Api.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class UploadFileController : BaseController
  {
    private readonly IAdminIjoinsService _ijoinsService;
    public UploadFileController(IAdminIjoinsService ijoinsService)
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

    [HttpPost("UploadCoverPhoto")]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> UploadCoverPhoto(IFormFile file)
    {
      object oRet = await _ijoinsService.UploadCoverPhoto(file);

      return Ok(oRet);
    }

    //[HttpPost("UpdateSessionCoverPhoto")]
    //public async Task<IActionResult> UpdateSessionCoverPhoto(CoverPhotoRequest coverPhotoRequest)
    //{
    //  object oRet = await _ijoinsService.UpdateSessionCoverPhoto(coverPhotoRequest);

    //  return Ok(oRet);
    //}

    [HttpPost("ImportKlcData")]
    public async Task<IActionResult> ImportKlcData(KlcFileImportRequest tbmKlcFileImport)
    {

      object oRet = await _ijoinsService.ImportKlcData(tbmKlcFileImport);

      return Ok(oRet);

    }

    [HttpGet("GetUserCompany")]
    public async Task<ActionResult<List<TbmCompany>>> GetUserCompany()
    {
      AssignUserAuthen();

      return await _ijoinsService.GetUserCompany(UserAuthen.UserId);
    }

  }
}
