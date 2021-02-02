﻿using Ema.IjoinsChkInOut.Api.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Globalization;

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

    public async Task UpdateSession(Session sIn)
    {
      var session = await _sessions.Find<Session>(s => s.SessionId == sIn.SessionId).FirstOrDefaultAsync();
      if (session == null)
      {
        await _sessions.InsertOneAsync(sIn);
      }
      else
      {
        sIn.Id = session.Id;
        await _sessions.ReplaceOneAsync(su => su.SessionId == sIn.SessionId, sIn);
      }
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
        // Return You Already Check In
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
        // Return You Already Check Out
      }
    }

    public async Task<List<SessionMobile>> GetSessionTodayForMobileByUserId(string userId)
    {
      CultureInfo enUS = new CultureInfo("en-US");
      DateTime.TryParseExact(DateTime.Now.ToString("yyyyMMdd") + " " + "01AM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime StartDay);
      DateTime.TryParseExact(DateTime.Now.ToString("yyyyMMdd") + " " + "11PM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime EndDay);

      List<SessionMobile> sessionMobiles = new List<SessionMobile>();
      var sessionUsers = await _sessionusers.Find<SessionUser>(w => w.UserId == userId).ToListAsync();

      foreach (SessionUser su in sessionUsers)
      {
        var session = await _sessions.Find<Session>(w =>
               w.IsCancel == '0'
            && w.SessionId == su.SessionId
            && w.StartDateTime <= EndDay
            && w.EndDateTime >= StartDay
        ).FirstOrDefaultAsync();

        if (session != null)
        {
          await GenSessionsForDisplay(sessionMobiles, su, session);
        }
      }

      return sessionMobiles.OrderBy(o => o.StartDateTime).ToList();
    }

    public async Task<List<SessionMobile>> GetSessionSevendayForMobileByUserId(string userId)
    {
      DateTime nextSevenDate = DateTime.Now.AddDays(7);

      List<SessionMobile> sessionMobiles = new List<SessionMobile>();
      var sessionUsers = await _sessionusers.Find<SessionUser>(w => w.UserId == userId).ToListAsync();

      foreach (SessionUser su in sessionUsers)
      {
        var session = await _sessions.Find<Session>(w =>
               w.IsCancel == '0'
            && w.SessionId == su.SessionId
            && (w.EndDateTime >= DateTime.Now && w.StartDateTime <= nextSevenDate)
        ).FirstOrDefaultAsync();

        if (session != null)
        {
          await GenSessionsForDisplay(sessionMobiles, su, session);
        }
      }

      return sessionMobiles.OrderBy(o => o.StartDateTime).ToList();
    }

    private async Task<List<SessionMobile>> GenSessionsForDisplay(List<SessionMobile> sessionMobiles, SessionUser su, Session s)
    {
      var userRegistration = await _userregistrations.Find<UserRegistration>(w => w.SessionId == su.SessionId && w.UserId == su.UserId).FirstOrDefaultAsync();



      SessionMobile sessionMobile = new SessionMobile();
      sessionMobile.UserId = su.UserId;
      sessionMobile.SessionId = su.SessionId;
      if (userRegistration != null
        && (
           userRegistration.CheckInDateTime.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd")
        || userRegistration.CheckOutDateTime.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd")
        )
      )
      {
        sessionMobile.IsCheckIn = userRegistration.IsCheckIn;
        sessionMobile.IsCheckOut = userRegistration.IsCheckOut;
        sessionMobile.CheckInDateTime = userRegistration.CheckInDateTime;
        sessionMobile.CheckOutDateTime = userRegistration.CheckOutDateTime;
        sessionMobile.CheckInBy = userRegistration.CheckInBy;
        sessionMobile.CheckOutBy = userRegistration.CheckOutBy;
      }
      sessionMobile.CourseId = s.CourseId;
      sessionMobile.CourseName = s.CourseName;
      sessionMobile.CourseNameTh = s.CourseNameTh;
      sessionMobile.SessionName = s.SessionName;
      sessionMobile.StartDateTime = s.StartDateTime;
      sessionMobile.EndDateTime = s.EndDateTime;
      sessionMobile.CourseOwnerEmail = s.CourseOwnerEmail;
      sessionMobile.CourseOwnerContactNo = s.CourseOwnerContactNo;
      sessionMobile.Venue = s.Venue;
      sessionMobile.Instructor = s.Instructor;
      sessionMobile.CourseCreditHoursInit = s.CourseCreditHoursInit;
      sessionMobile.PassingCriteriaExceptionInit = s.PassingCriteriaExceptionInit;
      sessionMobile.CourseCreditHours = s.CourseCreditHours;
      sessionMobile.PassingCriteriaException = s.PassingCriteriaException;
      sessionMobile.IsCancel = s.IsCancel;

      sessionMobiles.Add(sessionMobile);

      return sessionMobiles;
    }

  }
}

