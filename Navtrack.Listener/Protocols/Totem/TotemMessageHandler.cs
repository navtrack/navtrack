using System;
using System.Collections.Generic;
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
            List<Func<MessageInput, Location>> funcs = new List<Func<MessageInput, Location>>
            {
                ParseLocation_AT07, ParseLocation_AT09
            };

            foreach (Func<MessageInput,Location> func in funcs)
            {
                try
                {
                    input.MessageData.Reader.Reset();
                    Location location = func(input);

                    if (location != null)
                    {
                        return location;
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return null;
        }


        private static Location ParseLocation_AT07(MessageInput input)
        {
            return new Location
            {
                Device = new Device
                {
                    IMEI = input.MessageData.Reader.Skip(8).GetUntil('|')
                },
                DateTime = ConvertDate(input.MessageData.Reader.Skip(8).Get(12)),
                Satellites = Convert.ToInt16(input.MessageData.Reader.Skip(16).Get(2)),
                GsmSignal = Convert.ToInt16(input.MessageData.Reader.Get(2)),
                Heading = Convert.ToInt32(input.MessageData.Reader.Get(3)),
                Speed = Convert.ToInt32(input.MessageData.Reader.Get(3)),
                HDOP = float.Parse(input.MessageData.Reader.Get(4)),
                Odometer = uint.Parse(input.MessageData.Reader.Get(7)),
                Latitude = GpsUtil.ConvertDegreeAngleToDouble(@"(\d{2})(\d{2}).(\d{4})",
                    input.MessageData.Reader.Get(9), input.MessageData.Reader.Get(1)),
                Longitude = GpsUtil.ConvertDegreeAngleToDouble(@"(\d{3})(\d{2}).(\d{4})",
                    input.MessageData.Reader.Get(10), input.MessageData.Reader.Get(1))
            };
        }
        
        private static Location ParseLocation_AT09(MessageInput input)
        {
            return new Location
            {
                Device = new Device
                {
                    IMEI = input.MessageData.Reader.Skip(8).GetUntil('|')
                },
                DateTime = ConvertDate(input.MessageData.Reader.Skip(8).Get(12)),
                Satellites = Convert.ToInt16(input.MessageData.Reader.Skip(36).Get(2)),
                GsmSignal = Convert.ToInt16(input.MessageData.Reader.Get(2)),
                Heading = Convert.ToInt32(input.MessageData.Reader.Get(3)),
                Speed = Convert.ToInt32(input.MessageData.Reader.Get(3)),
                HDOP = float.Parse(input.MessageData.Reader.Get(4)),
                Odometer = uint.Parse(input.MessageData.Reader.Get(7)),
                Latitude = GpsUtil.ConvertDegreeAngleToDouble(@"(\d{2})(\d{2}).(\d{4})",
                    input.MessageData.Reader.Get(9), input.MessageData.Reader.Get(1)),
                Longitude = GpsUtil.ConvertDegreeAngleToDouble(@"(\d{3})(\d{2}).(\d{4})",
                    input.MessageData.Reader.Get(10), input.MessageData.Reader.Get(1))
            };
        }

        private static DateTime ConvertDate(string date) => DateTimeUtil.New(date[..2], date[2..4], date[4..6],
            date[6..8], date[8..10], date[10..12]);
    }
}