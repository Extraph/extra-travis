using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
//using System.ComponentModel.DataAnnotations;
//using Newtonsoft.Json;

namespace Ema.IjoinsChkInOut.Api.Models
{
  public class Session
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
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
  }
}
