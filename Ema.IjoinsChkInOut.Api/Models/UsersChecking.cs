using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
#region snippet_NewtonsoftJsonImport
using Newtonsoft.Json;
#endregion

namespace Ema.IjoinsChkInOut.Api.Models
{
  public class UsersChecking
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public int SegmentId { get; set; }
    public string UserId { get; set; }
    public string CheckingStatus { get; set; }
    public string Createby { get; set; } //Case Check In By Admin!!!
    public DateTime Createdatetime { get; set; }
  }
}
