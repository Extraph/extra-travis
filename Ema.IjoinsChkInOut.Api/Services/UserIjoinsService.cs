using Ema.IjoinsChkInOut.Api.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Ema.IjoinsChkInOut.Api.Services
{
  public class UserIjoinsService
  {
    private readonly IMongoCollection<Session> _sessions;
    private readonly IMongoCollection<SessionUser> _sessionusers;
    private readonly IMongoCollection<UserRegistration> _userregistrations;

    public UserIjoinsService(IUserIJoinDatabaseSettings settings)
    {
      var client = new MongoClient(settings.ConnectionString);
      var database = client.GetDatabase(settings.DatabaseName);

      _sessions = database.GetCollection<Session>(settings.SessionCollectionName);
      _sessionusers = database.GetCollection<SessionUser>(settings.SessionUserCollectionName);
      _userregistrations = database.GetCollection<UserRegistration>(settings.UserRegistrationName);

      var indexKeysCom = Builders<UserRegistration>.IndexKeys.Combine(
                                  Builders<UserRegistration>.IndexKeys.Ascending(s => s.SessionId),
                                  Builders<UserRegistration>.IndexKeys.Ascending(s => s.UserId),
                                  Builders<UserRegistration>.IndexKeys.Ascending(s => s.CheckInDateTime)
                              );
      var indexKeysSessionId = Builders<UserRegistration>.IndexKeys.Ascending(s => s.SessionId);
      var indexKeysUserId = Builders<UserRegistration>.IndexKeys.Ascending(s => s.UserId);
      var indexKeysCheckInDateTime = Builders<UserRegistration>.IndexKeys.Ascending(s => s.CheckInDateTime);

      _userregistrations.Indexes.CreateOne(new CreateIndexModel<UserRegistration>(indexKeysCom));
      _userregistrations.Indexes.CreateOne(new CreateIndexModel<UserRegistration>(indexKeysSessionId));
      _userregistrations.Indexes.CreateOne(new CreateIndexModel<UserRegistration>(indexKeysUserId));
      _userregistrations.Indexes.CreateOne(new CreateIndexModel<UserRegistration>(indexKeysCheckInDateTime));

    }
    public async Task CreateSessionUser(SessionUser suIn)
    {
      var sessionuser = await _sessionusers.Find<SessionUser>(su => su.SessionId == suIn.SessionId && su.UserId == suIn.UserId).FirstOrDefaultAsync();
      if (sessionuser == null)
      {
        await _sessionusers.InsertOneAsync(suIn);
      }
      else
      {
        suIn.Id = sessionuser.Id;
        await _sessionusers.ReplaceOneAsync(su => su.SessionId == suIn.SessionId && su.UserId == suIn.UserId, suIn);
      }
    }
    public async Task RemoveSessionUser(SessionUser suIn) => await _sessionusers.DeleteOneAsync(s => s.SessionId == suIn.SessionId && s.UserId == suIn.UserId);

    public async Task<List<UserRegistration>> GetUserRegistration(UserRegistration urIn)
    {
      return await _userregistrations.Find<UserRegistration>
                (
                 w => w.SessionId == urIn.SessionId
                 && w.UserId.Contains(urIn.UserId)
                ).ToListAsync();
    }

    public async Task CheckIn(UserRegistration urIn)
    {
      var userregistration = await _userregistrations.Find<UserRegistration>(ur => ur.SessionId == urIn.SessionId && ur.UserId == urIn.UserId).FirstOrDefaultAsync();
      if (userregistration == null)
      {
        await _userregistrations.InsertOneAsync(urIn);
      }
      else
      {
        userregistration.CheckInDateTime = DateTime.Now;
        userregistration.IsCheckIn = '1';
        userregistration.CheckInBy = urIn.CheckInBy;
        await _userregistrations.ReplaceOneAsync(ur => ur.SessionId == urIn.SessionId && ur.UserId == urIn.UserId, userregistration);
      }
    }
    public async Task CheckOut(UserRegistration urIn)
    {
      var userregistration = await _userregistrations.Find<UserRegistration>(ur => ur.SessionId == urIn.SessionId && ur.UserId == urIn.UserId).FirstOrDefaultAsync();
      if (userregistration == null)
      {
        await _userregistrations.InsertOneAsync(urIn);
      }
      else
      {
        userregistration.CheckOutDateTime = DateTime.Now;
        userregistration.IsCheckOut = '1';
        userregistration.CheckOutBy = urIn.CheckOutBy;
        await _userregistrations.ReplaceOneAsync(ur => ur.SessionId == urIn.SessionId && ur.UserId == urIn.UserId, userregistration);
      }
    }
  }
}

