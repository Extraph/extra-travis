using Ema.IjoinsChkInOut.Api.Models;
using Ema.IjoinsChkInOut.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

namespace Ema.IjoinsChkInOut.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CheckInController : ControllerBase
  {
    private readonly IjoinsService _userService;

    public CheckInController(IjoinsService userService)
    {
      _userService = userService;
    }

    [HttpPost]
    public ActionResult Create(UsersChecking user)
    {
      user.CheckingStatus = "Check-In";
      user.Createdatetime = DateTime.Now;
      _userService.Create(user);

      return Ok(new
      {
        success = true
      });
    }

  }
}
