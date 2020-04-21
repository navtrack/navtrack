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
            MeiligaoMessage message = new MeiligaoMessage(input.MessageData);
            
            if (message.Command == MeiligaoCommands.Login)
            {
                MeiligaoCommand reply =
                    new MeiligaoCommand(MeiligaoCommands.LoginConfirmation, message.DeviceIdHex, "01");
                
                input.NetworkStream.Write(HexUtil.ConvertHexStringToByteArray(reply.PacketBodyWithChecksum));
            }
            
            if (message.MeiligaoDataMessage != null)
            {
                Location location = new Location
                {
                    Device = new Device
                    {
                        IMEI = message.DeviceIdTrimmed
                    },
                    Latitude = message.MeiligaoDataMessage.Latitude,
                    Longitude = message.MeiligaoDataMessage.Longitude,
                    DateTime = message.MeiligaoDataMessage.DateTime,
                    Speed = (int) message.MeiligaoDataMessage.Speed,
                    Heading = message.MeiligaoDataMessage.Heading.HasValue
                        ? (int) message.MeiligaoDataMessage.Heading
                        : -1,
                    Altitude = (int) message.MeiligaoDataMessage.Altitude,
                    HDOP = message.MeiligaoDataMessage.HDOP,
                };
                
                return location;
            }

            return null;
        }
    }
}