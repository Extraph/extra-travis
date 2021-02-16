using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ema.Ijoins.Api.EfModels;
using Ema.Ijoins.Api.Entities;
using System.ComponentModel.DataAnnotations;

namespace Ema.Ijoins.Api.Models
{
  public class Model
  {
  }

  public class AuthenticateRequest
  {
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
  }
  public class AuthenticateResponse
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Token { get; set; }


    public AuthenticateResponse(User user, string token)
    {
      Id = user.Id;
      FirstName = user.FirstName;
      LastName = user.LastName;
      Username = user.Username;
      Token = token;
    }
  }
  public class ModelSessionsQR
  {
    public int FileId { get; set; }
    public int CourseTypeId { get; set; }
    public string CourseId { get; set; }
    public string CourseName { get; set; }
    public string CourseNameTh { get; set; }
    public string SessionId { get; set; }
    public string SessionName { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string CourseOwnerEmail { get; set; }
    public string CourseOwnerContactNo { get; set; }
    public string Venue { get; set; }
    public string Instructor { get; set; }
    public string CourseCreditHoursInit { get; set; }
    public string PassingCriteriaExceptionInit { get; set; }
    public string CourseCreditHours { get; set; }
    public string PassingCriteriaException { get; set; }
    public char IsCancel { get; set; }
    public DateTime Createdatetime { get; set; }
    public string UpdateBy { get; set; }
    public DateTime UpdateDatetime { get; set; }
    public List<VSegmentGenQr> SegmentsQr { get; set; }
  }

  public class ModelReport
  {
    public int No { get; set; }
    public string UserId { get; set; }
    public string UserNameSurname { get; set; }
    public string UserDepartment { get; set; }
    public string UserCompany { get; set; }

    public string Date { get; set; }
    public string CheckInTime { get; set; }
    public string CheckOutTime { get; set; }
    public string TrainingStatus { get; set; }

    public List<ModelSegmentReport> Segments { get; set; }
  }

  public class ModelSegmentReport
  {
    public string StartDateTime { get; set; }
    public string EndDateTime { get; set; }
    public string CheckInDateTime { get; set; }
    public string CheckOutDateTime { get; set; }
  }

  public class ModelNextSixDayDash
  {
    public DateTime DateTime { get; set; }
    public int AddDay { get; set; }
    public string SessionCount { get; set; }
  }

  public class FetchSessions
  {
    public int AddDay { get; set; }
    public DateTime TodayDateTime { get; set; }
    public int FileId { get; set; }
    public int CourseTypeId { get; set; }
    public string CourseId { get; set; }
    public string CourseName { get; set; }
    public string CourseNameTh { get; set; }
    public string SessionId { get; set; }
    public string SessionName { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string CourseOwnerEmail { get; set; }
    public string CourseOwnerContactNo { get; set; }
    public string Venue { get; set; }
    public string Instructor { get; set; }
    public string CourseCreditHoursInit { get; set; }
    public string PassingCriteriaExceptionInit { get; set; }
    public string CourseCreditHours { get; set; }
    public string PassingCriteriaException { get; set; }
    public char IsCancel { get; set; }
    public DateTime Createdatetime { get; set; }
    public string UpdateBy { get; set; }
    public DateTime UpdateDatetime { get; set; }
  }
}
