using System.Text.RegularExpressions;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Alematics;

[Service(typeof(ICustomMessageHandler<AlematicsProtocol>))]
public class AlematicsMessageHandler : BaseMessageHandler<AlematicsProtocol>
{
    public override DeviceMessageDocument Parse(MessageInput input)
    {
        Match locationMatch =
            new Regex("(\\d{15})," + // IMEI
                      "(\\d{4})(\\d{2})(\\d{2})(\\d{2})(\\d{2})(\\d{2})," + // GPS date
                      "(\\d{4})(\\d{2})(\\d{2})(\\d{2})(\\d{2})(\\d{2})," + // Device date
                      "(.*?)," + // Latitude
                      "(.*?)," + // Longitude
                      "(.*?)," + // Speed
                      "(.*?)," + // Heading
                      "(.*?)," + // Altitude
                      "(.*?)," + // HDOP
                      "(.*?)," + // Satellites
                      "(.*?)," + // Input
                      "(.*?)," + // Output
                      "(.*?)," + // ADC
                      "(.*?)," + // Power
                      "(\\d+)") // Odometer
                .Match(input.DataMessage.String);

        if (locationMatch.Success)
        {
            input.ConnectionContext.SetDevice(locationMatch.Groups[1].Value);

            DeviceMessageDocument deviceMessageDocument = new()
            {
                Position = new PositionElement
                {
                    Date = DateTimeUtil.New(locationMatch.Groups[2].Value, locationMatch.Groups[3].Value,
                        locationMatch.Groups[4].Value, locationMatch.Groups[5].Value, locationMatch.Groups[6].Value,
                        locationMatch.Groups[7].Value, add2000Year: false),
                    Latitude = locationMatch.Groups[14].Get<double>(),
                    Longitude = locationMatch.Groups[15].Get<double>(),
                    Speed = locationMatch.Groups[16].Get<float?>(),
                    Heading = locationMatch.Groups[17].Get<float?>(),
                    Altitude = locationMatch.Groups[18].Get<float?>(),
                    HDOP = locationMatch.Groups[19].Get<float?>(),
                    Satellites = locationMatch.Groups[20].Get<short?>(),
                },
                Device = new DeviceElement
                {
                    Odometer = locationMatch.Groups[25].Get<int?>()
                }
            };

            return deviceMessageDocument;
        }

        return null;
    }
}