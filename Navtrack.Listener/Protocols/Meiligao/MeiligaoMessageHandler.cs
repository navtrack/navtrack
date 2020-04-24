using System;
using System.Text.RegularExpressions;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Meiligao
{
    [Service(typeof(ICustomMessageHandler<MeiligaoProtocol>))]
    public class MeiligaoMessageHandler : BaseMessageHandler<MeiligaoProtocol>
    {
        public override Location Parse(MessageInput input)
        {
            MeiligaoInputMessage inputMessage = new MeiligaoInputMessage(input.MessageData);

            HandleMessage(input, inputMessage);

            if (inputMessage.MeiligaoDataMessage != null)
            {
                Location location = new Location
                {
                    Device = new Device
                    {
                        IMEI = inputMessage.DeviceIdTrimmed
                    }
                };
                
                location.PositionStatus = inputMessage.MeiligaoDataMessage.GPRMCArray[1] == "A";
                location.Latitude = GpsUtil.ConvertDegreeAngleToDouble(@"(\d{2})(\d{2}).(\d{4})",
                    inputMessage.MeiligaoDataMessage.GPRMCArray[2], inputMessage.MeiligaoDataMessage.GPRMCArray[3]);
                location.Longitude = GpsUtil.ConvertDegreeAngleToDouble(@"(\d{3})(\d{2}).(\d{4})",
                    inputMessage.MeiligaoDataMessage.GPRMCArray[4], inputMessage.MeiligaoDataMessage.GPRMCArray[5]);
                location.DateTime = GetDateTime(inputMessage.MeiligaoDataMessage.GPRMCArray[0],
                    inputMessage.MeiligaoDataMessage.GPRMCArray[8]);
                location.Speed = inputMessage.MeiligaoDataMessage.GPRMCArray.Get<double>(6) * 1.852;
                location.Heading = inputMessage.MeiligaoDataMessage.GPRMCArray.Get<float?>(7);
                location.HDOP = inputMessage.MeiligaoDataMessage.StringSplit.Get<float>(1);
                location.Altitude = inputMessage.MeiligaoDataMessage.StringSplit.Get<double>(2);
                location.Odometer = inputMessage.MeiligaoDataMessage.StringSplit.Get<uint?>(7);
                
                return location;
            }

        return null;
    }

    private static void HandleMessage(MessageInput input, MeiligaoInputMessage inputMessage)
    {
    if (inputMessage.Command == MeiligaoCommands.Login)
    {
        MeiligaoOutputMessage reply =
            new MeiligaoOutputMessage(MeiligaoCommands.LoginConfirmation, inputMessage.DeviceIdHex, "01");

        input.NetworkStream.Write(HexUtil.ConvertHexStringToByteArray(reply.PacketBodyWithChecksum));
    }
    }

    public static DateTime GetDateTime( string timeString, string dateString)
    {
    GroupCollection time = new Regex(@"(\d{2})(\d{2})(\d{2}).(\d{2})").Matches(timeString)[0].Groups;

    GroupCollection date = new Regex(@"(\d{2})(\d{2})(\d{2})").Matches(dateString)[0].Groups;
        return DateTimeUtil.New(date[3].Value, date[2].Value, date[1].Value,
    time[1].Value, time[2].Value, time[3].Value,
    time[4].Value);
    }
}

}