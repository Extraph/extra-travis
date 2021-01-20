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
  public class ModelSegmentsQR
  {
    public int Id { get; set; }
    public int FileId { get; set; }
    public int CourseTypeId { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string SessionId { get; set; }
    public string SessionName { get; set; }
    public string CourseId { get; set; }
    public string CourseName { get; set; }
    public string CourseNameTh { get; set; }
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
    public List<TbmSegment> SegmentsQr { get; set; }

  }

}
