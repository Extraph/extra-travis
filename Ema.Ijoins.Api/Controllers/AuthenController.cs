using Microsoft.AspNetCore.Mvc;
using Ema.Ijoins.Api.Models;
using Ema.Ijoins.Api.Services;

namespace Ema.Ijoins.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthenController : ControllerBase
  {
    private IUserService _userService;

    public AuthenController(IUserService userService)
    {
      _userService = userService;
    }

    [HttpPost("authenticate")]
    public IActionResult Authenticate(AuthenticateRequest model)
    {
      try
      {
        var response = _userService.Authenticate(model);

        if (response == null)
          return Ok(new { isauthen = false, message = "Username or password is incorrect.", response });

        return Ok(new { isauthen = true, message = "Success", response });
      }
      catch (System.Exception ex)
      {
        return Ok(new { isauthen = false, message = string.Format("Service Issue : {0}", ex.Message) });
      }

    }

    [Authorize]
    [HttpGet]
    public IActionResult GetAll()
    {
      var users = _userService.GetAll();
      return Ok(users);
    }
  }
}
