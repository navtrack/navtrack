using System;
using System.Text.RegularExpressions;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.StarLink;

[Service(typeof(ICustomMessageHandler<ErmProtocol>))]
public class ErmMessageHandler : BaseMessageHandler<ErmProtocol>
{
    private const string DefaultDataFormat =
        "#EDT#,#EID#,#PDT#,#LAT#,#LONG#,#SPD#,#HEAD#,#ODO#,#IN1#,#IN2#,#IN3#,#IN4#,#OUT1#,#OUT2#,#OUT3#,#OUT4#,#LAC#,#CID#,#VIN#,#VBAT#,#DEST#,#IGN#,#ENG#";

    public override Location Parse(MessageInput input)
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
            input.Client.SetDevice(locationMatch.Groups[1].Value);
                
            Location location = new()
            {
                Device = input.Client.Device
            };

            SetData(locationMatch.Groups[4].Value, location);

            return location;
        }

        return null;
    }

    private void SetData(string dataString, Location location)
    {
        string[] data = dataString.Split(",");
        string[] dataKey = DefaultDataFormat.Split(",");

        for (int i = 0; i < Math.Min(data.Length, dataKey.Length); i++)
        {
            if (dataKey[i] == "#LAT#")
            {
                location.Latitude = GetCoordinate(data[i]);
            }

            if (dataKey[i] == "#LONG#")
            {
                location.Longitude = GetCoordinate(data[i]);
            }

            if (dataKey[i] == "#SPD#")
            {
                location.Speed = SpeedUtil.KnotsToKph(Convert.ToSingle(data[i]));
            }

            if (dataKey[i] == "#HEAD#")
            {
                location.Heading = Convert.ToSingle(data[i]);
            }

            if (dataKey[i] == "#ODO#")
            {
                location.Odometer = Convert.ToDouble(data[i]) * 1000;
            }

            if (dataKey[i] == "#EDT#")
            {
                location.DateTime = NewDateTimeUtil.Convert(DateFormat.YYMMDDHHMMSS, data[i]);
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