using Ema.IjoinsChkInOut.Api.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Globalization;
using Ema.IjoinsChkInOut.Api.Helpers;

namespace Ema.IjoinsChkInOut.Api.Services
{
  public class UserIjoinsServiceOld
  {
    private readonly IMongoCollection<Session> _sessions;
    private readonly IMongoCollection<Segment> _segments;
    private readonly IMongoCollection<SessionUser> _sessionusers;
    private readonly IMongoCollection<UserRegistration> _userregistrations;

    public UserIjoinsServiceOld(IUserIJoinDatabaseSettings settings)
    {
      var client = new MongoClient(settings.ConnectionString);
      var database = client.GetDatabase(settings.DatabaseName);

      _sessions = database.GetCollection<Session>(settings.SessionCollectionName);
      _segments = database.GetCollection<Segment>(settings.SegmentCollectionName);
      _sessionusers = database.GetCollection<SessionUser>(settings.SessionUserCollectionName);
      _userregistrations = database.GetCollection<UserRegistration>(settings.UserRegistrationName);

      var indexKeysCom = Builders<UserRegistration>.IndexKeys.Combine(
                                  Builders<UserRegistration>.IndexKeys.Ascending(s => s.SessionId),
                                  Builders<UserRegistration>.IndexKeys.Ascending(s => s.UserId),
                                  Builders<UserRegistration>.IndexKeys.Ascending(s => s.CheckInDate),
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
                 && (
                   w.CheckInDate == Utility.GetStryyyyMMddNow()
                || w.CheckOutDate == Utility.GetStryyyyMMddNow()
                    )
                ).ToListAsync();
    }

    public async Task<object> CheckIn(UserRegistration urIn)
    {
      try
      {
        string responseMsg = "";
        bool isLate = false;
        bool isAlert = false;

        if (urIn.StartDateQr != Utility.GetStryyyyMMddNow())
        {
          isAlert = true;
          responseMsg = "Wrong date of QR Code!";
          return new { isScanSuccess = !isAlert, isAlert, isLate, scanResponseMsg = responseMsg };
        }

        var segment = await _segments.Find<Segment>(
        w => w.SessionId == urIn.SessionId &&
          w.StartDate == Utility.GetStryyyyMMddNow() &&
          w.EndDate == Utility.GetStryyyyMMddNow()
        ).FirstOrDefaultAsync();
        if (segment != null)
        {
          if (int.Parse(Utility.GetStrHHmmNow()) > int.Parse(segment.StartDateTime.ToLocalTime().AddMinutes(1).ToString("HHmm")))
          { responseMsg = "Your session has already started!"; isLate = true; }
        }

        var userregistration = await _userregistrations.Find<UserRegistration>(
          ur =>
             ur.SessionId == urIn.SessionId
          && ur.UserId == urIn.UserId
          && ur.CheckInDate == Utility.GetStryyyyMMddNow()
          ).FirstOrDefaultAsync();
        if (userregistration == null)
        {
          await _userregistrations.InsertOneAsync(urIn);
        }
        else
        {
          isAlert = true;
          responseMsg = "You already checked in.";
        }

        return new { isScanSuccess = !isAlert, isAlert, isLate, scanResponseMsg = responseMsg };
      }
      catch (Exception ex)
      {
        return new { isScanSuccess = false, scanResponseMsg = ex.Message };
      }
    }
    public async Task<object> CheckOut(UserRegistration urIn)
    {
      try
      {
        string responseMsg = "";
        bool isAlert = false;

        if (urIn.StartDateQr != Utility.GetStryyyyMMddNow())
        {
          isAlert = true;
          responseMsg = "Wrong date of QR Code!";
          return new { isScanSuccess = !isAlert, isAlert, scanResponseMsg = responseMsg };
        }


        var userregistration = await _userregistrations.Find<UserRegistration>(
          ur => ur.SessionId == urIn.SessionId
          && ur.UserId == urIn.UserId
          && ur.CheckInDate == Utility.GetStryyyyMMddNow()
          ).FirstOrDefaultAsync();
        if (userregistration == null)
        {
          isAlert = true;
          responseMsg = "Please checked in before check out!";
        }
        else
        {
          userregistration.CheckOutDateTime = DateTime.Now;
          userregistration.CheckOutDate = Utility.GetStryyyyMMddNow();
          userregistration.CheckOutTime = int.Parse(Utility.GetStrHHmmNow());
          userregistration.IsCheckOut = '1';
          userregistration.CheckOutBy = urIn.CheckOutBy;
          await _userregistrations.ReplaceOneAsync(
            ur => ur.SessionId == urIn.SessionId
            && ur.UserId == urIn.UserId
            && ur.CheckInDate == Utility.GetStryyyyMMddNow()
            , userregistration);

          var session = await _sessions.Find<Session>(w => w.SessionId == userregistration.SessionId).FirstOrDefaultAsync();
          if (session != null && session.EndDateTime.ToLocalTime() > Utility.GetEndDay())
          {
            responseMsg = "Don't forget to join the remaining days.";
          }
        }

        return new { isScanSuccess = !isAlert, isAlert, scanResponseMsg = responseMsg };
      }
      catch (Exception ex)
      {
        return new { isScanSuccess = false, scanResponseMsg = ex.Message };
      }

    }

    public async Task<List<SessionMobile>> GetSessionTodayForMobileByUserId(string userId)
    {
      List<SessionMobile> sessionMobiles = new List<SessionMobile>();
      var sessionUsers = await _sessionusers.Find<SessionUser>(w => w.UserId == userId).ToListAsync();

      foreach (SessionUser su in sessionUsers)
      {
        var session = await _sessions.Find<Session>(w =>
               w.IsCancel == '0'
            && w.SessionId == su.SessionId
            && w.StartDateTime <= Utility.GetEndDay()
            && w.EndDateTime >= Utility.GetStartDay()
        ).FirstOrDefaultAsync();

        if (session != null)
        {
          await GenSessionsTodayForDisplay(sessionMobiles, su, session);
        }
      }

      return sessionMobiles.OrderBy(o => o.StartDateTime).ThenBy(o => o.SessionId).ToList();
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
            && (w.StartDateTime > Utility.GetEndDay() && w.StartDateTime <= nextSevenDate)
        ).FirstOrDefaultAsync();

        if (session != null)
        {
          GenSessionsSevendayForDisplay(sessionMobiles, su, session);
        }
      }

      return sessionMobiles.OrderBy(o => o.StartDateTime).ThenBy(o => o.SessionId).ToList();
    }

    private async Task<List<SessionMobile>> GenSessionsTodayForDisplay(List<SessionMobile> sessionMobiles, SessionUser su, Session s)
    {
      var segment = await _segments.Find<Segment>(
        w => w.SessionId == s.SessionId &&
          w.StartDate == Utility.GetStryyyyMMddNow() &&
          w.EndDate == Utility.GetStryyyyMMddNow()
        ).FirstOrDefaultAsync();


      if (segment != null)
      {
        var userRegistration = await _userregistrations.Find<UserRegistration>(
        w => w.SessionId == su.SessionId
        && w.UserId == su.UserId
        && (
            w.CheckInDate == Utility.GetStryyyyMMddNow()
         || w.CheckOutDate == Utility.GetStryyyyMMddNow()
           )
        ).FirstOrDefaultAsync();

        SessionMobile sessionMobile = new SessionMobile();
        sessionMobile.UserId = su.UserId;
        sessionMobile.SessionId = su.SessionId;
        if (userRegistration != null
          &&
          (
             userRegistration.CheckInDateTime.ToLocalTime().ToString("yyyyMMdd") == Utility.GetStryyyyMMddNow()
          || userRegistration.CheckOutDateTime.ToLocalTime().ToString("yyyyMMdd") == Utility.GetStryyyyMMddNow()
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

        if (
            DateTime.Now >= segment.StartDateTime.ToLocalTime().AddHours(-2)
            && int.Parse(Utility.GetStrHHmmNow()) >= int.Parse(segment.StartDateTime.ToLocalTime().AddHours(-2).ToString("HHmm"))
          )
        {
          sessionMobile.CanCheckInOut = '1';
        }
        else
        {
          sessionMobile.CanCheckInOut = '0';
        }


        sessionMobile.CourseId = s.CourseId;
        sessionMobile.CourseName = s.CourseName;
        sessionMobile.CourseNameTh = s.CourseNameTh;
        sessionMobile.SessionName = s.SessionName;

        sessionMobile.StartDateTime = s.StartDateTime;
        sessionMobile.EndDateTime = s.EndDateTime;

        if (segment != null)
        {
          sessionMobile.SegmentStartDateTime = segment.StartDateTime;
          sessionMobile.SegmentEndDateTime = segment.EndDateTime;
        }

        sessionMobile.CourseOwnerEmail = s.CourseOwnerEmail;
        sessionMobile.CourseOwnerContactNo = s.CourseOwnerContactNo;


        if (segment != null)
        {
          sessionMobile.Venue = segment.Venue;
          sessionMobile.SegmentName = segment.SegmentName;
        }
        else
        {
          sessionMobile.Venue = s.Venue;
        }


        sessionMobile.Instructor = s.Instructor;
        sessionMobile.CourseCreditHoursInit = s.CourseCreditHoursInit;
        sessionMobile.PassingCriteriaExceptionInit = s.PassingCriteriaExceptionInit;
        sessionMobile.CourseCreditHours = s.CourseCreditHours;
        sessionMobile.PassingCriteriaException = s.PassingCriteriaException;
        sessionMobile.IsCancel = s.IsCancel;

        sessionMobiles.Add(sessionMobile);
      }
      else
      {
        // รอถามคุณก้อย ว่า กรณี Script วันทำอย่างไร

        //SessionMobile sessionMobile = new SessionMobile();
        //sessionMobile.UserId = su.UserId;
        //sessionMobile.SessionId = su.SessionId;
        //sessionMobile.CanCheckInOut = '0';
        //sessionMobile.CourseId = s.CourseId;
        //sessionMobile.CourseName = s.CourseName;
        //sessionMobile.CourseNameTh = s.CourseNameTh;
        //sessionMobile.SessionName = s.SessionName;

        //sessionMobile.StartDateTime = s.StartDateTime;
        //sessionMobile.EndDateTime = s.EndDateTime;

        //sessionMobile.CourseOwnerEmail = s.CourseOwnerEmail;
        //sessionMobile.CourseOwnerContactNo = s.CourseOwnerContactNo;

        //sessionMobile.Venue = s.Venue;

        //sessionMobile.Instructor = s.Instructor;
        //sessionMobile.CourseCreditHoursInit = s.CourseCreditHoursInit;
        //sessionMobile.PassingCriteriaExceptionInit = s.PassingCriteriaExceptionInit;
        //sessionMobile.CourseCreditHours = s.CourseCreditHours;
        //sessionMobile.PassingCriteriaException = s.PassingCriteriaException;
        //sessionMobile.IsCancel = s.IsCancel;

        //sessionMobiles.Add(sessionMobile);
      }


      return sessionMobiles;
    }

    private List<SessionMobile> GenSessionsSevendayForDisplay(List<SessionMobile> sessionMobiles, SessionUser su, Session s)
    {

      SessionMobile sessionMobile = new SessionMobile();
      sessionMobile.UserId = su.UserId;
      sessionMobile.SessionId = su.SessionId;

      sessionMobile.CanCheckInOut = '0';

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

