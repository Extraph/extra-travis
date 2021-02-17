using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ema.Ijoins.Api.EfModels;
using System.Security.Claims;

namespace Ema.Ijoins.Api.Controllers
{
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class BaseController : ControllerBase
  {
    public TbmUser UserAuthen { get; set; }
    protected void AssignUserAuthen()
    {
      var user = (TbmUser)HttpContext.Items["User"];
      UserAuthen = user;
    }
  }
}
