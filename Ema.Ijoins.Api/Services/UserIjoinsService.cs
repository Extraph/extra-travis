using Ema.Ijoins.Api.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System;

namespace Ema.Ijoins.Api.Services
{
  public class UserIjoinsService
  {
    private readonly IMongoCollection<Session> _sessions;
    private readonly IMongoCollection<Segment> _segments;
    private readonly IMongoCollection<SessionUser> _sessionusers;
    private readonly IMongoCollection<UserRegistration> _userregistrations;

    public UserIjoinsService(IUserIJoinDatabaseSettings settings)
    {
      var client = new MongoClient(settings.ConnectionString);
      var database = client.GetDatabase(settings.DatabaseName);

      _sessions = database.GetCollection<Session>(settings.SessionCollectionName);
      _segments = database.GetCollection<Segment>(settings.SegmentCollectionName);
      _sessionusers = database.GetCollection<SessionUser>(settings.SessionUserCollectionName);
      _userregistrations = database.GetCollection<UserRegistration>(settings.UserRegistrationName);

      var indexKeysSessionCom = Builders<Session>.IndexKeys.Combine(
                                  Builders<Session>.IndexKeys.Ascending(s => s.SessionId),
                                  Builders<Session>.IndexKeys.Ascending(s => s.StartDateTime),
                                  Builders<Session>.IndexKeys.Ascending(s => s.EndDateTime)
                              );
      var indexKeysSessionId = Builders<Session>.IndexKeys.Ascending(s => s.SessionId);
      var indexKeysStartDateTime = Builders<Session>.IndexKeys.Ascending(s => s.StartDateTime);
      var indexKeysEndDateTime = Builders<Session>.IndexKeys.Ascending(s => s.EndDateTime);

      _sessions.Indexes.CreateOne(new CreateIndexModel<Session>(indexKeysSessionCom));
      _sessions.Indexes.CreateOne(new CreateIndexModel<Session>(indexKeysSessionId));
      _sessions.Indexes.CreateOne(new CreateIndexModel<Session>(indexKeysStartDateTime));
      _sessions.Indexes.CreateOne(new CreateIndexModel<Session>(indexKeysEndDateTime));



      var indexKeysSegmentCom = Builders<Segment>.IndexKeys.Combine(
                                  Builders<Segment>.IndexKeys.Ascending(s => s.SessionId),
                                  Builders<Segment>.IndexKeys.Ascending(s => s.StartDate),
                                  Builders<Segment>.IndexKeys.Ascending(s => s.EndDate)
                              );
      var indexKeysSegmentSessionId = Builders<Segment>.IndexKeys.Ascending(s => s.SessionId);
      var indexKeysSegmentStartDate = Builders<Segment>.IndexKeys.Ascending(s => s.StartDate);
      var indexKeysSegmentEndDate = Builders<Segment>.IndexKeys.Ascending(s => s.EndDate);

      _segments.Indexes.CreateOne(new CreateIndexModel<Segment>(indexKeysSegmentCom));
      _segments.Indexes.CreateOne(new CreateIndexModel<Segment>(indexKeysSegmentSessionId));
      _segments.Indexes.CreateOne(new CreateIndexModel<Segment>(indexKeysSegmentStartDate));
      _segments.Indexes.CreateOne(new CreateIndexModel<Segment>(indexKeysSegmentEndDate));




      var indexKeysSessionUserCom = Builders<SessionUser>.IndexKeys.Combine(
                                  Builders<SessionUser>.IndexKeys.Ascending(s => s.SessionId),
                                  Builders<SessionUser>.IndexKeys.Ascending(s => s.UserId)
                              );
      var indexKeysUserSessionId = Builders<SessionUser>.IndexKeys.Ascending(s => s.SessionId);
      var indexKeysUserUserId = Builders<SessionUser>.IndexKeys.Ascending(s => s.UserId);

      _sessionusers.Indexes.CreateOne(new CreateIndexModel<SessionUser>(indexKeysSessionUserCom));
      _sessionusers.Indexes.CreateOne(new CreateIndexModel<SessionUser>(indexKeysUserSessionId));
      _sessionusers.Indexes.CreateOne(new CreateIndexModel<SessionUser>(indexKeysUserUserId));

    }


    public async Task<List<Session>> GetSession() => await _sessions.Find(s => true).ToListAsync();
    public async Task<Session> GetSession(string id) => await _sessions.Find<Session>(s => s.Id == id).FirstOrDefaultAsync();
    public async void CreateSession(Session sIn)
    {
      var session = await _sessions.Find<Session>(s => s.SessionId == sIn.SessionId).FirstOrDefaultAsync();
      if (session == null)
      {
        await _sessions.InsertOneAsync(sIn);
      }
      else
      {
        sIn.Id = session.Id;
        await _sessions.ReplaceOneAsync(s => s.SessionId == sIn.SessionId, sIn);
      }
    }
    public async void CreateSessionUser(SessionUser suIn)
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
    public async void RemoveSessionUser(SessionUser sIn) => await _sessionusers.DeleteManyAsync(s => s.SessionId == sIn.SessionId);
    public async void CreateSegment(Segment seIn)
    {
      var segment = await _segments.Find<Segment>(su =>
            su.SessionId == seIn.SessionId &&
            su.StartDateTime == seIn.StartDateTime &&
            su.EndDateTime == seIn.EndDateTime
            ).FirstOrDefaultAsync();
      if (segment == null)
      {
        await _segments.InsertOneAsync(seIn);
      }
      else
      {
        seIn.Id = segment.Id;
        await _segments.ReplaceOneAsync(su =>
            su.SessionId == seIn.SessionId &&
            su.StartDateTime == seIn.StartDateTime &&
            su.EndDateTime == seIn.EndDateTime
            , seIn);
      }
    }
    public async void RemoveSegmentUser(Segment seIn) => await _segments.DeleteManyAsync(s => s.SessionId == seIn.SessionId);
    public async Task<List<UserRegistration>> GetUserRegistration(UserRegistration urIn)
    {
      return await _userregistrations.Find<UserRegistration>
                (
                 w => w.SessionId == urIn.SessionId
                 && w.UserId == urIn.UserId
                ).ToListAsync();
    }
  }
}

