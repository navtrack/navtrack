using System;
using System.Text.RegularExpressions;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.StarLink;

[Service(typeof(ICustomMessageHandler<ErmProtocol>))]
public class ErmMessageHandler : BaseMessageHandler<ErmProtocol>
{
    private const string DefaultDataFormat =
        "#EDT#,#EID#,#PDT#,#LAT#,#LONG#,#SPD#,#HEAD#,#ODO#,#IN1#,#IN2#,#IN3#,#IN4#,#OUT1#,#OUT2#,#OUT3#,#OUT4#,#LAC#,#CID#,#VIN#,#VBAT#,#DEST#,#IGN#,#ENG#";

    public override DeviceMessageDocument Parse(MessageInput input)
    {
        Match locationMatch =
            new Regex("\\$SLU" +
                      "(.*?)," + // device id
                      "(\\d+)," + // type
                      "(\\d+)," + //index 
                      "(.*?)" + // data
                      "\\*(..)") // checksum
                .Match(input.DataMessage.String);

        if (locationMatch.Success)
        {
            input.ConnectionContext.SetDevice(locationMatch.Groups[1].Value);
                
            DeviceMessageDocument deviceMessageDocument = new()
            {
                // Device = input.ConnectionContext.Device
                Position = new PositionElement()
            };

            SetData(locationMatch.Groups[4].Value, deviceMessageDocument);

            return deviceMessageDocument;
        }

        return null;
    }

    private void SetData(string dataString, DeviceMessageDocument deviceMessageDocument)
    {
        string[] data = dataString.Split(",");
        string[] dataKey = DefaultDataFormat.Split(",");

        for (int i = 0; i < Math.Min(data.Length, dataKey.Length); i++)
        {
            if (dataKey[i] == "#LAT#")
            {
                deviceMessageDocument.Position.Latitude = GetCoordinate(data[i]);
            }

            if (dataKey[i] == "#LONG#")
            {
                deviceMessageDocument.Position.Longitude = GetCoordinate(data[i]);
            }

            if (dataKey[i] == "#SPD#")
            {
                deviceMessageDocument.Position.Speed = SpeedUtil.KnotsToKph(Convert.ToSingle(data[i]));
            }

            if (dataKey[i] == "#HEAD#")
            {
                deviceMessageDocument.Position.Heading = Convert.ToSingle(data[i]);
            }

            if (dataKey[i] == "#ODO#")
            {
                deviceMessageDocument.Device ??= new DeviceElement();
                deviceMessageDocument.Device.Odometer = Convert.ToInt32(data[i]) * 1000;
            }

            if (dataKey[i] == "#EDT#")
            {
                deviceMessageDocument.Position.Date = DateTimeUtil.Convert(DateFormat.YYMMDDHHMMSS, data[i]);
            }
        }
    }

    private double GetCoordinate(string value)
    {
        Match coordinateMatch = new Regex("(\\+?)(\\d{2})(\\d+.\\d+)").Match(value);

        double coordinate = coordinateMatch.Groups[2].Get<double>();
        coordinate += coordinateMatch.Groups[3].Get<double>() / 60;

        return coordinateMatch.Groups[1].Value == "+" ? coordinate : -coordinate;
    }
}