using System.Globalization;
using System.Text.RegularExpressions;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.KingSword;

[Service(typeof(ICustomMessageHandler<KingSwordProtocol>))]
public class KingSwordMessageHandler : BaseMessageHandler<KingSwordProtocol>
{
    public override DeviceMessageDocument Parse(MessageInput input)
    {
        Match locationMatch =
            new Regex("(\\d{15})," + // imei
                      "(..)," + // command
                      "(A|V)," + // position status
                      "(.{2})(.{2})(.{2})," + // yy mm dd
                      "(.{2})(.{2})(.{2})," + // hh mm ss
                      "(.)(.{7})," + // latitude
                      "(.)(.{7})," + // longitude
                      "(.{4})," + // speed
                      "(.{4})," + // heading
                      "(.{8})," + // status 
                      "(..)," + // gsm signal
                      "(\\d+)," + // power
                      "(.*?)," + // oil
                      "(.*?)(,|#)" + // odometer
                      "(\\d*)") // altitude
                .Match(input.DataMessage.String);

        if (locationMatch.Success)
        {
            input.ConnectionContext.SetDevice(locationMatch.Groups[1].Value);

            DeviceMessageDocument deviceMessageDocument = new()
            {
                Position = new PositionElement(),
                Gsm = new GsmElement()
            };


            deviceMessageDocument.Position.Date = DateTimeUtil.NewFromHex(
                locationMatch.Groups[4].Value,
                locationMatch.Groups[5].Value,
                locationMatch.Groups[6].Value,
                locationMatch.Groups[4].Value,
                locationMatch.Groups[5].Value,
                locationMatch.Groups[6].Value,
                locationMatch.Groups[7].Value);
            deviceMessageDocument.Position.Latitude =
                GetCoordinate(locationMatch.Groups[10].Value, locationMatch.Groups[11].Value);
            deviceMessageDocument.Position.Longitude =
                GetCoordinate(locationMatch.Groups[12].Value, locationMatch.Groups[13].Value);
            deviceMessageDocument.Position.Speed =
                int.Parse(locationMatch.Groups[14].Value, NumberStyles.HexNumber) / 100;
            deviceMessageDocument.Position.Heading =
                locationMatch.Groups[15].Get<float?>();
            deviceMessageDocument.Gsm.SignalStrength =
                short.Parse(locationMatch.Groups[17].Value, NumberStyles.HexNumber);
            deviceMessageDocument.Device ??= new DeviceElement();
            deviceMessageDocument.Device.Odometer =
                int.Parse(locationMatch.Groups[20].Value, NumberStyles.HexNumber);
            deviceMessageDocument.Position.Altitude = locationMatch.Groups[22].Get<float?>();

            return deviceMessageDocument;
        }

        return null;
    }

    private static double GetCoordinate(string minus, string p1)
    {
        double coordinate = int.Parse(p1, NumberStyles.HexNumber) / (double)600000;

        return minus == "8" ? -coordinate : coordinate;
    }
}