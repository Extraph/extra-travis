using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
//using System.ComponentModel.DataAnnotations;
//using Newtonsoft.Json;

namespace Ema.Ijoins.Api.Models
{
  public class UserRegistration
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string SessionId { get; set; }
    public string UserId { get; set; }
    public string StartDateQr { get; set; }
    public char IsCheckIn { get; set; }
    public char IsCheckOut { get; set; }
    public DateTime CheckInDateTime { get; set; }
    public string CheckInDate { get; set; }
    public int CheckInTime { get; set; }
    public DateTime CheckOutDateTime { get; set; }
    public string CheckOutDate { get; set; }
    public int CheckOutTime { get; set; }
    public string CheckInBy { get; set; }
    public string CheckOutBy { get; set; }
  }
}

