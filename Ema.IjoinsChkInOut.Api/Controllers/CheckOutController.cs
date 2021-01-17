using Ema.IjoinsChkInOut.Api.Models;
using Ema.IjoinsChkInOut.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

namespace Ema.IjoinsChkInOut.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CheckOutController : ControllerBase
  {
    private readonly IjoinsService _userService;

    public CheckOutController(IjoinsService userService)
    {
      _userService = userService;
    }

    [HttpPost]
    public ActionResult Create(UsersChecking user)
    {
      user.CheckingStatus = "Check-Out";
      user.Createdatetime = DateTime.Now;
      _userService.Create(user);

      return Ok(new
      {
        success = true
      });
    }
  }
}
