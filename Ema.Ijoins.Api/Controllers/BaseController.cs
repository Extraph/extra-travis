using Microsoft.AspNetCore.Mvc;
using Ema.Ijoins.Api.EfAdminModels;

namespace Ema.Ijoins.Api.Controllers
{
  [Authorize]
  [Route("[controller]")]
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
