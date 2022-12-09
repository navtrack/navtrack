using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.VjoyCar;

public class BaseVjoyCarMessageHandler<T> : BaseMessageHandler<T>
{
    public override Location Parse(MessageInput input)
    {
        HandleLoginMessage(input);

        Location location = ParseLocation(input);

        return location;
    }

    private static Location ParseLocation(MessageInput input)
    {
        GroupCollection lgc =
            new Regex(
                    @"(\d{2})(\d{2})(\d{2})(A|V)(\d{4}.\d{4})(N|S)(\d{5}.\d{4})(E|W)(.{5})(\d{2})(\d{2})(\d{2})(.{6})(1|0{8})L(.{0,8})(...)")
                .Matches(input.DataMessage.String)[0].Groups;

        if (lgc.Count == 17)
        {
            Location location = new()
            {
                Device = input.Client.Device,
                DateTime = DateTimeUtil.New(lgc[1].Value, lgc[2].Value, lgc[3].Value, lgc[10].Value, lgc[11].Value,
                    lgc[12].Value),
                PositionStatus = lgc[4].Value == "F",
                Latitude = GpsUtil.ConvertDmmLatToDecimal(lgc[5].Value, lgc[6].Value),
                Longitude = GpsUtil.ConvertDmmLongToDecimal(lgc[7].Value, lgc[8].Value),
                Speed = float.Parse(lgc[9].Value),
                Heading = float.Parse(lgc[13].Value),
                Odometer = long.Parse(lgc[15].Value, NumberStyles.HexNumber)
            };

            return location;
        }

        return null;
    }

    private static void HandleLoginMessage(MessageInput input)
    {
        Dictionary<string, string> loginCommandResponse = new()
        {
            {"BP00", "AP01HSO"},
            {"BP05", "AP05"}
        };

        string command = string.Join(string.Empty, input.DataMessage.String[13..17]);

        if (loginCommandResponse.ContainsKey(command))
        {
            string imei = input.DataMessage.String[17..32];
                
            input.Client.SetDevice(imei);
                
            string reply =
                $"({string.Join(string.Empty, input.DataMessage.String[1..13])}{loginCommandResponse[command]})";

            input.NetworkStream.Write(StringUtil.ConvertStringToByteArray(reply));
        }
    }
}