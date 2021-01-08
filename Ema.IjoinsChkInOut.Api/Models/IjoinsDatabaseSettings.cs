namespace Ema.IjoinsChkInOut.Api.Models
{
  public class IjoinsDatabaseSettings : IIjoinsDatabaseSettings
  {
    public string IjoinsCollectionName { get; set; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
  }

  public interface IIjoinsDatabaseSettings
  {
    string IjoinsCollectionName { get; set; }
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
  }
}
