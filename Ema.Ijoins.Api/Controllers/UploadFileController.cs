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
using Amazon.S3;
using Amazon.S3.Model;

namespace Ema.Ijoins.Api.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class UploadFileController : BaseController
  {
    private readonly IAdminIjoinsService _ijoinsService;

    private static string accessKey = "AKIA2L3MPKZVCWVCUPGO";
    private static string accessSecret = "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";
    private static string bucket = "ema-cms-bucket";

    public UploadFileController(IAdminIjoinsService ijoinsService)
    {
      _ijoinsService = ijoinsService;
    }


    [HttpPost("UploadFileKlc")]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> UploadFileKlc(IFormFile file)
    {
      //try
      //{
      //  var client = new AmazonS3Client(accessKey, accessSecret, Amazon.RegionEndpoint.APSoutheast1);
      //  byte[] fileBytes = new Byte[file.Length];
      //  file.OpenReadStream().Read(fileBytes, 0, Int32.Parse(file.Length.ToString()));
      //  var fileName = Guid.NewGuid() + file.FileName;
      //  PutObjectResponse response = null;
      //  using (var stream = new MemoryStream(fileBytes))
      //  {
      //    var request = new PutObjectRequest
      //    {
      //      BucketName = bucket,
      //      Key = fileName,
      //      InputStream = stream,
      //      ContentType = file.ContentType,
      //      CannedACL = S3CannedACL.PublicRead
      //    };

      //    response = await client.PutObjectAsync(request);
      //  };

      //  var responseObject = client.GetPreSignedURL(new GetPreSignedUrlRequest { BucketName = bucket, Key = fileName, Expires = DateTime.Now.AddMinutes(5) });
      //}
      //catch (Exception ex)
      //{

      //  throw ex;
      //}



      object oRet = await _ijoinsService.UploadFileKlc(file);

      return Ok(oRet);

    }

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
