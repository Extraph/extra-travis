using Ema.IjoinsChkInOut.Api.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Ema.IjoinsChkInOut.Api.Services
{
  public class IjoinsService
  {
    private readonly IMongoCollection<UsersChecking> _users;


    public IjoinsService(IIjoinsDatabaseSettings settings)
    {
      var client = new MongoClient(settings.ConnectionString);
      var database = client.GetDatabase(settings.DatabaseName);

      _users = database.GetCollection<UsersChecking>(settings.IjoinsCollectionName);
    }


    public List<UsersChecking> Get() => _users.Find(user => true).ToList();

    public UsersChecking Get(string id) =>
        _users.Find<UsersChecking>(user => user.Id == id).FirstOrDefault();

    public UsersChecking Create(UsersChecking user)
    {
      _users.InsertOne(user);
      return user;
    }

    public void Update(string id, UsersChecking userIn) =>
        _users.ReplaceOne(user => user.Id == id, userIn);

    public void Remove(UsersChecking userIn) =>
        _users.DeleteOne(user => user.Id == userIn.Id);

    public void Remove(string id) =>
        _users.DeleteOne(user => user.Id == id);
  }
}

