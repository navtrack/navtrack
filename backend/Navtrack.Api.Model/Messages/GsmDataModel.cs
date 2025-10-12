namespace Navtrack.Api.Model.Messages;

public class GsmDataModel
{
    public byte? SignalLevel { get; set; }
    public string? MobileCountryCode { get; set; }
    public string? MobileNetworkCode { get; set; }
}