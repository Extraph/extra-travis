using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
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

    //[BsonElement("SessionId")]
    [Required]
    public string SessionId { get; set; }

    //[BsonElement("UserId")]
    [Required]
    public string UserId { get; set; }

    //[BsonElement("CheckingStatus")]
    [Required]
    public string CheckingStatus { get; set; }

    //[BsonElement("Createby")]
    public string Createby { get; set; } //Case Check In By Admin!!!

    //[BsonElement("Createdatetime")] 
    public DateTime Createdatetime { get; set; }
  }
}
