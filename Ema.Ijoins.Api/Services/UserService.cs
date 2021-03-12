using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Ema.Ijoins.Api.Entities;
using Ema.Ijoins.Api.Helpers;
using Ema.Ijoins.Api.Models;
using Ema.Ijoins.Api.EfAdminModels;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Ema.Ijoins.Api.Services
{
  public interface IUserService
  {
    AuthenticateResponse Authenticate(AuthenticateRequest model);
    IEnumerable<User> GetAll();
    TbmUser GetById(string UserId);
  }

  public class UserService : IUserService
  {
    // users hardcoded for simplicity, store in a db with hashed passwords in production applications
    private List<User> _users = new List<User>
        {
            new User { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test" }
        };

    private readonly adminijoin_databaseContext _context;
    private readonly AppSettings _appSettings;

    public UserService(adminijoin_databaseContext context, IOptions<AppSettings> appSettings)
    {
      _context = context;
      _appSettings = appSettings.Value;
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest model)
    {
      //var user = _users.SingleOrDefault(x => x.Username == model.Username);

      var user = _context.TbmUsers.Where(w => w.UserId == model.Username).FirstOrDefault();

      // return null if user not found
      if (user == null) return null;

      // authentication successful so generate jwt token
      var token = generateJwtToken(user);

      return new AuthenticateResponse(user, token);
    }

    public IEnumerable<User> GetAll()
    {
      return _users;
    }

    public TbmUser GetById(string UserId)
    {
      return _context.TbmUsers.Where(w => w.UserId == UserId).FirstOrDefault();
    }

    // helper methods

    private string generateJwtToken(TbmUser user)
    {
      // generate token that is valid for 7 days
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[] { new Claim("id", user.UserId) }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }
  }
}