using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Listener.Models;

public class Device
{
    public string IMEI { get; set; }
    public AssetDocument Entity { get; set; }
}