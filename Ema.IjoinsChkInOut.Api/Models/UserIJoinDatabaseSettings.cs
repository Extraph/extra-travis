namespace Ema.IjoinsChkInOut.Api.Models
{

  public interface IUserIJoinDatabaseSettings
  {
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
    string SessionCollectionName { get; set; }
    string SessionUserCollectionName { get; set; }    
  }
  public class UserIJoinDatabaseSettings : IUserIJoinDatabaseSettings
  {
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string SessionCollectionName { get; set; }
    public string SessionUserCollectionName { get; set; }
  }

}
