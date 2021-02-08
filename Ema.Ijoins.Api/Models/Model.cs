using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ema.Ijoins.Api.EfModels;

namespace Ema.Ijoins.Api.Models
{
  public class Model
  {
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
