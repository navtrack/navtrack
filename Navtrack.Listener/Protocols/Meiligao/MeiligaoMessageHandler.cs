using System;
using System.Text.RegularExpressions;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Meiligao;

[Service(typeof(ICustomMessageHandler<MeiligaoProtocol>))]
public class MeiligaoMessageHandler : BaseMessageHandler<MeiligaoProtocol>
{
    public override Location Parse(MessageInput input)
    {
        MeiligaoInputMessage inputMessage = new(input.DataMessage);
            
        input.Client.SetDevice(inputMessage.DeviceIdTrimmed);

        HandleMessage(input, inputMessage);

        if (inputMessage.MeiligaoDataMessage != null)
        {
            Location location = new()
            {
                Device = input.Client.Device,
                PositionStatus = inputMessage.MeiligaoDataMessage.GPRMCArray[1] == "A",
                Latitude = GpsUtil.ConvertDmmLatToDecimal(inputMessage.MeiligaoDataMessage.GPRMCArray[2],
                    inputMessage.MeiligaoDataMessage.GPRMCArray[3]),
                Longitude = GpsUtil.ConvertDmmLongToDecimal(inputMessage.MeiligaoDataMessage.GPRMCArray[4],
                    inputMessage.MeiligaoDataMessage.GPRMCArray[5]),
                DateTime = GetDateTime(inputMessage.MeiligaoDataMessage.GPRMCArray[0],
                    inputMessage.MeiligaoDataMessage.GPRMCArray[8]),
                Speed = SpeedUtil.KnotsToKph(inputMessage.MeiligaoDataMessage.GPRMCArray.Get<float>(6)),
                Heading = inputMessage.MeiligaoDataMessage.GPRMCArray.Get<float?>(7),
                HDOP = inputMessage.MeiligaoDataMessage.StringSplit.Get<float?>(1),
                Altitude = inputMessage.MeiligaoDataMessage.StringSplit.Get<float?>(2),
                Odometer = inputMessage.MeiligaoDataMessage.StringSplit.Get<uint?>(7)
            };

            return location;
        }

        return null;
    }

    private static void HandleMessage(MessageInput input, MeiligaoInputMessage inputMessage)
    {
        if (inputMessage.Command == MeiligaoCommands.Login)
        {
            MeiligaoOutputMessage reply = new(MeiligaoCommands.LoginConfirmation, inputMessage.DeviceIdHex, "01");

            input.NetworkStream.Write(HexUtil.ConvertHexStringToByteArray(reply.PacketBodyWithChecksum));
        }
    }

    private static DateTime GetDateTime(string timeString, string dateString)
    {
        GroupCollection time = new Regex(@"(\d{2})(\d{2})(\d{2}).(\d{2})").Matches(timeString)[0].Groups;

        GroupCollection date = new Regex(@"(\d{2})(\d{2})(\d{2})").Matches(dateString)[0].Groups;
        return DateTimeUtil.New(date[3].Value, date[2].Value, date[1].Value,
            time[1].Value, time[2].Value, time[3].Value,
            time[4].Value);
    }
}