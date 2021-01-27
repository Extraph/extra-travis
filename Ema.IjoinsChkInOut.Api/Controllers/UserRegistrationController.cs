using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ema.IjoinsChkInOut.Api.Services;
using Ema.IjoinsChkInOut.Api.Models;


namespace Ema.IjoinsChkInOut.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UserRegistrationController : ControllerBase
  {

    private readonly UserIjoinsService _userIjoinsService;
    public UserRegistrationController(UserIjoinsService userIjoinsService)
    {
      _userIjoinsService = userIjoinsService;
    }

    [HttpPost("CheckIn")]
    public async Task<ActionResult> CheckIn(UserRegistration urIn)
    {
      urIn.CheckInDateTime = DateTime.Now;
      urIn.IsCheckIn = '1';
      await _userIjoinsService.CheckIn(urIn);
      return Ok();
    }

    [HttpPost("CheckOut")]
    public async Task<ActionResult> CheckOut(UserRegistration urIn)
    {
      urIn.CheckOutDateTime = DateTime.Now;
      urIn.IsCheckOut = '1';
      await _userIjoinsService.CheckOut(urIn);
      return Ok();
    }



  }
}
