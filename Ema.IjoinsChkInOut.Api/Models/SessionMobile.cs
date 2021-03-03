using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ema.IjoinsChkInOut.Api.Models
{
  public class SessionMobile
  {
    public string UserId { get; set; }
    public string SessionId { get; set; }

    public char CanCheckInOut { get; set; }

    public char IsCheckIn { get; set; }
    public char IsCheckOut { get; set; }
    public DateTime CheckInDateTime { get; set; }
    public DateTime CheckOutDateTime { get; set; }
    public string CheckInBy { get; set; }
    public string CheckOutBy { get; set; }


    public string CourseId { get; set; }
    public string CourseName { get; set; }
    public string CourseNameTh { get; set; }
    public string SessionName { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string CourseOwnerEmail { get; set; }
    public string CourseOwnerContactNo { get; set; }
    public string Venue { get; set; }
    public string SegmentName { get; set; }
    public DateTime SegmentStartDateTime { get; set; }
    public DateTime SegmentEndDateTime { get; set; }
    public string Instructor { get; set; }
    public string CourseCreditHoursInit { get; set; }
    public string PassingCriteriaExceptionInit { get; set; }
    public string CourseCreditHours { get; set; }
    public string PassingCriteriaException { get; set; }
    public char IsCancel { get; set; }

    public string CoverPhotoName { get; set; }
    public string CoverPhotoUrl { get; set; }
  }
}
