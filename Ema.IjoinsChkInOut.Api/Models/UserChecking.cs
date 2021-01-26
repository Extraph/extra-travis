using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
//using System.ComponentModel.DataAnnotations;
//using Newtonsoft.Json;


namespace Ema.IjoinsChkInOut.Api.Models
{
  public class UserChecking
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string UserId { get; set; }
    public string SessionId { get; set; }
    public string IsCheckIn { get; set; }
    public string IsCheckOut { get; set; }
    public DateTime CheckInDatetime { get; set; }
    public DateTime CheckOutDatetime { get; set; }
    public string CheckInBy { get; set; }
    public DateTime Createdatetime { get; set; }
  }
}
