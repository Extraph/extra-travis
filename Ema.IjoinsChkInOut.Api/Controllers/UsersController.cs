using Ema.IjoinsChkInOut.Api.Models;
using Ema.IjoinsChkInOut.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

namespace Ema.IjoinsChkInOut.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    private readonly IjoinsService _userService;

    public UsersController(IjoinsService userService)
    {
      _userService = userService;
    }

    [HttpGet]
    public ActionResult<List<UsersChecking>> Get() => _userService.Get();

    [HttpGet("{id:length(24)}", Name = "GetBook")]
    public ActionResult<UsersChecking> Get(string id)
    {
      var user = _userService.Get(id);

      if (user == null)
      {
        return NotFound();
      }

      return user;
    }

    [HttpPost]
    public ActionResult<UsersChecking> Create(UsersChecking user)
    {
      user.Createdatetime = DateTime.Now;
      _userService.Create(user);

      return CreatedAtRoute("GetBook", new { id = user.Id.ToString() }, user);
    }

    [HttpPut("{id:length(24)}")]
    public IActionResult Update(string id, UsersChecking userIn)
    {
      var user = _userService.Get(id);

      if (user == null)
      {
        return NotFound();
      }

      _userService.Update(id, userIn);

      return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public IActionResult Delete(string id)
    {
      var user = _userService.Get(id);

      if (user == null)
      {
        return NotFound();
      }

      _userService.Remove(user.Id);

      return NoContent();
    }
  }
}
