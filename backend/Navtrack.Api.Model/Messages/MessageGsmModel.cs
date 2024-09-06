namespace Navtrack.Api.Model.Messages;

public class MessageGsmModel
{
    public byte? SignalLevel { get; set; }
    public string? MobileCountryCode { get; set; }
    public string? MobileNetworkCode { get; set; }
}