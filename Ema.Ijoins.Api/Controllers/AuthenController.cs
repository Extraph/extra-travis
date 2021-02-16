using Microsoft.AspNetCore.Mvc;
using Ema.Ijoins.Api.Models;
using Ema.Ijoins.Api.Services;

namespace Ema.Ijoins.Api.Controllers
{
  [ApiController]
  [Route("[controller]")]
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
      var response = _userService.Authenticate(model);

      if (response == null)
        return BadRequest(new { message = "Username or password is incorrect" });

      return Ok(response);
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
