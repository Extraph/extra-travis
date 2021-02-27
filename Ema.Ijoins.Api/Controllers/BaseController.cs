﻿using Microsoft.AspNetCore.Mvc;
using Ema.Ijoins.Api.EfModels;

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
