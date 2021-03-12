using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ema.IjoinsChkInOut.Api.Services;
using Ema.IjoinsChkInOut.Api.Models;
using Ema.IjoinsChkInOut.Api.Helpers;
using Ema.IjoinsChkInOut.Api.EfUserModels;


namespace Ema.IjoinsChkInOut.Api.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class UserRegistrationController : ControllerBase
  {

    private readonly IUserIjoinsService _userIjoinsService;
    public UserRegistrationController(IUserIjoinsService userIjoinsService)
    {
      _userIjoinsService = userIjoinsService;
    }

    [HttpPost]
    public async Task<ActionResult<List<TbUserRegistration>>> GetUserRegistration(TbUserRegistration urIn)
    {
      var userRegistrations = await _userIjoinsService.GetUserRegistration(urIn);

      if (userRegistrations == null)
      {
        return NotFound();
      }

      return userRegistrations;
    }


    [HttpPost("CheckIn")]
    public async Task<ActionResult> CheckIn(TbUserRegistration urIn)
    {
      urIn.CheckInDatetime = DateTime.Now;
      urIn.CheckInDate = Utility.GetStryyyyMMddNow();
      urIn.CheckInTime = int.Parse(Utility.GetStrHHmmNow());
      urIn.IsCheckIn = '1';

      return Ok(await _userIjoinsService.CheckIn(urIn));
    }

    [HttpPost("CheckOut")]
    public async Task<ActionResult> CheckOut(TbUserRegistration urIn)
    {
      urIn.CheckOutDatetime = DateTime.Now;
      urIn.CheckOutDate = Utility.GetStryyyyMMddNow();
      urIn.CheckOutTime = int.Parse(Utility.GetStrHHmmNow());
      urIn.IsCheckOut = '1';

      return Ok(await _userIjoinsService.CheckOut(urIn));
    }



  }
}
