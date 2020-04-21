using System;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Totem
{
    [Service(typeof(ICustomMessageHandler<TotemProtocol>))]
    public class TotemMessageHandler : BaseMessageHandler<TotemProtocol>
    {
        public override Location Parse(MessageInput input)
        {
            if (input.MessageData.String[2] == '0')
            {
                Location location = new Location
                {
                    Device = new Device
                    {
                        IMEI = input.MessageData.Reader.Skip(8).GetUntil('|')
                    },
                    DateTime = ConvertDate(input.MessageData.Reader.Skip(8).Get(12)),
                    Satellites = Convert.ToInt16(input.MessageData.Reader.Skip(16).Get(2)),
                    Heading = Convert.ToInt32(input.MessageData.Reader.Skip(2).Get(3)),
                    Speed = Convert.ToInt32(input.MessageData.Reader.Get(3)),
                    HDOP = float.Parse(input.MessageData.Reader.Get(4)),
                    Latitude = GpsUtil.ConvertDegreeAngleToDouble(@"(\d{2})(\d{2}).(\d{4})",
                        input.MessageData.Reader.Skip(7).Get(9), input.MessageData.Reader.Get(1)),
                    Longitude = GpsUtil.ConvertDegreeAngleToDouble(@"(\d{3})(\d{2}).(\d{4})",
                        input.MessageData.Reader.Get(10), input.MessageData.Reader.Get(1))
                };
            
                return location;
            }
        
            return null;
        }
        
        private static DateTime ConvertDate(string date) => DateTimeUtil.New(date[..2], date[2..4], date[4..6],
            date[6..8], date[8..10], date[10..12]);
    }
}