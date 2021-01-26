using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
//using System.ComponentModel.DataAnnotations;
//using Newtonsoft.Json;

namespace Ema.Ijoins.Api.Models
{
  public class SessionUser
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string SessionId { get; set; }
    public string UserId { get; set; }
  }
}
