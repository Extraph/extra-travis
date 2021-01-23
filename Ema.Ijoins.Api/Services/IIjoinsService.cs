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


namespace Ema.Ijoins.Api.Services
{
  public interface IIjoinsService
  {
    List<TbKlcDataMaster> UploadFileKlc(IFormFile file);
  }

  public class IjoinsService : IIjoinsService
  {

    private readonly IFileProvider _fileProvider;
    private readonly ema_databaseContext _context;

    public IjoinsService(IFileProvider fileProvider, ema_databaseContext context)
    {
      _fileProvider = fileProvider;
      _context = context;
    }

    public List<TbKlcDataMaster> UploadFileKlc(IFormFile file)
    {



      return new List<TbKlcDataMaster>();
    }
  }
}
