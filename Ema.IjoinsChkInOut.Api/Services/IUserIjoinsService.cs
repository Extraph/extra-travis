using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ema.IjoinsChkInOut.Api.EfUserModels;
using Ema.IjoinsChkInOut.Api.Helpers;
using Ema.IjoinsChkInOut.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Ema.IjoinsChkInOut.Api.Services
{
  public interface IUserIjoinsService
  {
    Task CreateSessionUser(TbmUserSessionUser suIn);
    Task RemoveSessionUser(TbmUserSessionUser suIn);
    Task UpdateSession(TbmUserSession sIn);
    Task<List<SessionMobile>> GetSessionTodayForMobileByUserId(string userId);
    Task<List<SessionMobile>> GetSessionSevendayForMobileByUserId(string userId);
    Task<List<TbUserRegistration>> GetUserRegistration(TbUserRegistration urIn);
    Task<object> CheckIn(TbUserRegistration urIn);
    Task<object> CheckOut(TbUserRegistration urIn);
  }
  public class UserIjoinsService : IUserIjoinsService
  {
    private readonly userijoin_databaseContext _usercontext;
    public UserIjoinsService(userijoin_databaseContext usercontext)
    {
      _usercontext = usercontext;
    }

    public async Task CreateSessionUser(TbmUserSessionUser suIn)
    {
      if (!TbmUserSessionUsersExists(suIn.SessionId, suIn.UserId))
      {
        //TbmUserSessionUser tbmUserSessionUser = new TbmUserSessionUser
        //{
        //  SessionId = suIn.SessionId,
        //  UserId = suIn.UserId
        //};
        _usercontext.TbmUserSessionUsers.Add(suIn);
        await _usercontext.SaveChangesAsync();
      }
    }
    public async Task RemoveSessionUser(TbmUserSessionUser suIn)
    {
      if (TbmUserSessionUsersExists(suIn.SessionId, suIn.UserId))
      {
        TbmUserSessionUser tbmUserSessionUser = await _usercontext.TbmUserSessionUsers.Where(w => w.SessionId == suIn.SessionId && w.UserId == suIn.UserId).FirstOrDefaultAsync();
        _usercontext.TbmUserSessionUsers.Remove(tbmUserSessionUser);
      }
    }
    public async Task UpdateSession(TbmUserSession sIn)
    {
      if (!TbmUserSessionExists(sIn.SessionId))
      {
        _usercontext.TbmUserSessions.Add(sIn);
        await _usercontext.SaveChangesAsync();
      }
      else
      {
        _usercontext.Entry(sIn).State = EntityState.Modified;
        await _usercontext.SaveChangesAsync();
      }
    }

    public async Task<List<SessionMobile>> GetSessionTodayForMobileByUserId(string userId)
    {
      List<SessionMobile> sessionMobiles = new List<SessionMobile>();
      //var sessionUsers = await _sessionusers.Find<SessionUser>(w => w.UserId == userId).ToListAsync();
      var sessionUsers = await _usercontext.TbmUserSessionUsers.Where(w => w.UserId == userId).ToListAsync();

      foreach (TbmUserSessionUser su in sessionUsers)
      {
        //var session = await _sessions.Find<Session>(w =>
        //       w.IsCancel == '0'
        //    && w.SessionId == su.SessionId
        //    && w.StartDateTime <= Utility.GetEndDay()
        //    && w.EndDateTime >= Utility.GetStartDay()
        //).FirstOrDefaultAsync();

        var session = await _usercontext.TbmUserSessions.Where(w =>
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
    private async Task<List<SessionMobile>> GenSessionsTodayForDisplay(List<SessionMobile> sessionMobiles, TbmUserSessionUser su, TbmUserSession s)
    {
      //var segment = await _segments.Find<Segment>(
      //  w => w.SessionId == s.SessionId &&
      //    w.StartDate == Utility.GetStryyyyMMddNow() &&
      //    w.EndDate == Utility.GetStryyyyMMddNow()
      //  ).FirstOrDefaultAsync();

      var segment = await _usercontext.TbmUserSegments.Where(
            w =>
            w.SessionId == s.SessionId &&
            w.StartDate == Utility.GetStryyyyMMddNow() &&
            w.EndDate == Utility.GetStryyyyMMddNow()
          ).FirstOrDefaultAsync();


      if (segment != null)
      {
        //var userRegistration = await _userregistrations.Find<UserRegistration>(
        //w => w.SessionId == su.SessionId
        //&& w.UserId == su.UserId
        //&& (
        //    w.CheckInDate == Utility.GetStryyyyMMddNow()
        // || w.CheckOutDate == Utility.GetStryyyyMMddNow()
        //   )
        //).FirstOrDefaultAsync();

        var userRegistration = await _usercontext.TbUserRegistrations.Where(
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
             userRegistration.CheckInDatetime.ToString("yyyyMMdd") == Utility.GetStryyyyMMddNow()
          || userRegistration.CheckOutDatetime.ToString("yyyyMMdd") == Utility.GetStryyyyMMddNow()
          )
        )
        {
          sessionMobile.IsCheckIn = userRegistration.IsCheckIn;
          sessionMobile.IsCheckOut = userRegistration.IsCheckOut;
          sessionMobile.CheckInDateTime = userRegistration.CheckInDatetime;
          sessionMobile.CheckOutDateTime = userRegistration.CheckOutDatetime;
          sessionMobile.CheckInBy = userRegistration.CheckInBy;
          sessionMobile.CheckOutBy = userRegistration.CheckOutBy;
        }

        if (
            DateTime.Now >= segment.StartDateTime.AddHours(-2)
            && int.Parse(Utility.GetStrHHmmNow()) >= int.Parse(segment.StartDateTime.AddHours(-2).ToString("HHmm"))
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

    public async Task<List<SessionMobile>> GetSessionSevendayForMobileByUserId(string userId)
    {
      DateTime nextSevenDate = DateTime.Now.AddDays(7);

      List<SessionMobile> sessionMobiles = new List<SessionMobile>();
      //var sessionUsers = await _sessionusers.Find<SessionUser>(w => w.UserId == userId).ToListAsync();
      var sessionUsers = await _usercontext.TbmUserSessionUsers.Where(w => w.UserId == userId).ToListAsync();

      foreach (TbmUserSessionUser su in sessionUsers)
      {
        var session = await _usercontext.TbmUserSessions.Where(w =>
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
    private List<SessionMobile> GenSessionsSevendayForDisplay(List<SessionMobile> sessionMobiles, TbmUserSessionUser su, TbmUserSession s)
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


    public bool TbmUserSessionUsersExists(string SessionId, string UserId)
    {
      return _usercontext.TbmUserSessionUsers.Any(
        e =>
         e.SessionId == SessionId
      && e.UserId == UserId
      );
    }
    public bool TbmUserSessionExists(string SessionId)
    {
      return _usercontext.TbmUserSessions.Any(e => e.SessionId == SessionId);
    }


    public async Task<List<TbUserRegistration>> GetUserRegistration(TbUserRegistration urIn)
    {
      return await _usercontext.TbUserRegistrations.Where
                (
                 w => w.SessionId == urIn.SessionId
                 && w.UserId.Contains(urIn.UserId)
                 && (
                   w.CheckInDate == Utility.GetStryyyyMMddNow()
                || w.CheckOutDate == Utility.GetStryyyyMMddNow()
                    )
                ).ToListAsync();
    }


    public async Task<object> CheckIn(TbUserRegistration urIn)
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

        var segment = await _usercontext.TbmUserSegments.Where(
        w => w.SessionId == urIn.SessionId &&
          w.StartDate == Utility.GetStryyyyMMddNow() &&
          w.EndDate == Utility.GetStryyyyMMddNow()
        ).FirstOrDefaultAsync();
        if (segment != null)
        {
          if (int.Parse(Utility.GetStrHHmmNow()) > int.Parse(segment.StartDateTime.AddMinutes(1).ToString("HHmm")))
          { responseMsg = "Your session has already started!"; isLate = true; }
        }

        var userregistration = await _usercontext.TbUserRegistrations.Where(
          ur =>
             ur.SessionId == urIn.SessionId
          && ur.UserId == urIn.UserId
          && ur.CheckInDate == Utility.GetStryyyyMMddNow()
          ).FirstOrDefaultAsync();
        if (userregistration == null)
        {
          //await _userregistrations.InsertOneAsync(urIn);
          _usercontext.TbUserRegistrations.Add(urIn);
          await _usercontext.SaveChangesAsync();
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
    public async Task<object> CheckOut(TbUserRegistration urIn)
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


        var userregistration = await _usercontext.TbUserRegistrations.Where(
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
          userregistration.CheckOutDatetime = DateTime.Now;
          userregistration.CheckOutDate = Utility.GetStryyyyMMddNow();
          userregistration.CheckOutTime = int.Parse(Utility.GetStrHHmmNow());
          userregistration.IsCheckOut = '1';
          userregistration.CheckOutBy = urIn.CheckOutBy;
          _usercontext.Entry(userregistration).State = EntityState.Modified;
          await _usercontext.SaveChangesAsync();
          //await _userregistrations.ReplaceOneAsync(
          //  ur => ur.SessionId == urIn.SessionId
          //  && ur.UserId == urIn.UserId
          //  && ur.CheckInDate == Utility.GetStryyyyMMddNow()
          //  , userregistration);

          var session = await _usercontext.TbmUserSessions.Where(w => w.SessionId == userregistration.SessionId).FirstOrDefaultAsync();
          if (session != null && session.EndDateTime > Utility.GetEndDay())
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



  }
}
