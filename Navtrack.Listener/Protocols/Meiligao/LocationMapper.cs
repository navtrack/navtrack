using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Protocols.Meiligao
{
    [Service(typeof(IMapper<MeiligaoMessage, Location>))]
    public class LocationMapper : IMapper<MeiligaoMessage, Location>
    {
        public Location Map(MeiligaoMessage source, Location destination)
        {

            if (source.DataMessage != null)
            {
                destination.Device = new Device
                {
                    IMEI = source.DeviceIdTrimmed
                };

                destination.Latitude = source.DataMessage.Latitude;
                destination.Longitude = source.DataMessage.Longitude;
                destination.DateTime = source.DataMessage.DateTime;
                destination.Speed = (int) source.DataMessage.Speed;
                destination.Heading = source.DataMessage.Heading.HasValue ? (int)source.DataMessage.Heading : -1;
                destination.Altitude = (int) source.DataMessage.Altitude;
                destination.HDOP = source.DataMessage.HDOP;
                destination.ProtocolData = HexUtil.ConvertHexStringArrayToHexString(source.Hex);

                return destination;
            }

            return null;
        }
    }
}