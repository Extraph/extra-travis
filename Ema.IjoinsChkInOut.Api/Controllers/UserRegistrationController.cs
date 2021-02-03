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

    [HttpPost]
    public async Task<ActionResult<List<UserRegistration>>> GetUserRegistration(UserRegistration urIn)
    {
      var userRegistrations = await _userIjoinsService.GetUserRegistration(urIn);

      if (userRegistrations == null)
      {
        return NotFound();
      }

      return userRegistrations;
    }


    [HttpPost("CheckIn")]
    public async Task<ActionResult> CheckIn(UserRegistration urIn)
    {
      urIn.CheckInDateTime = DateTime.UtcNow;
      urIn.CheckInDate = DateTime.UtcNow.ToString("yyyyMMdd");
      urIn.CheckInTime = int.Parse(DateTime.UtcNow.ToString("HHmm"));
      urIn.IsCheckIn = '1';
      
      return Ok(await _userIjoinsService.CheckIn(urIn));
    }

    [HttpPost("CheckOut")]
    public async Task<ActionResult> CheckOut(UserRegistration urIn)
    {
      urIn.CheckOutDateTime = DateTime.UtcNow;
      urIn.CheckOutDate = DateTime.UtcNow.ToString("yyyyMMdd");
      urIn.CheckOutTime = int.Parse(DateTime.UtcNow.ToString("HHmm"));
      urIn.IsCheckOut = '1';
      
      return Ok(await _userIjoinsService.CheckOut(urIn));
    }



  }
}
