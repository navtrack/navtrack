using System;
using System.Collections.Generic;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Galileosky;

[Service(typeof(ICustomMessageHandler<GalileoskyProtocol>))]
public class GalileoskyMessageHandler : BaseMessageHandler<GalileoskyProtocol>
{
    public override IEnumerable<Location> ParseRange(MessageInput input)
    {
        input.DataMessage.ByteReader.Skip(1);
        int length = input.DataMessage.ByteReader.Get<short>() & 0x7fff;

        List<Location> locations = new();
        Location location = new();

        HashSet<byte> tagsFound = new();

        while (input.DataMessage.ByteReader.Index < input.DataMessage.Bytes.Length - 2)
        {
            byte tag = input.DataMessage.ByteReader.GetOne();

            if (tagsFound.Contains(tag))
            {
                location = new Location();
                tagsFound.Clear();
            }

            tagsFound.Add(tag);
            int? tagLength = GalileoskyProtocolTags.GetTagLength(tag);

            switch (tag)
            {
                case 0x03:
                    byte[] bytes = input.DataMessage.ByteReader.Get(tagLength.GetValueOrDefault());
                    input.Client.SetDevice(StringUtil.ConvertByteArrayToString(
                        bytes));
                    break;

                case 0x20:
                    location.DateTime = DateTime.UnixEpoch.AddSeconds(input.DataMessage.ByteReader.Get<int>());
                    break;

                case 0x30:

                    location.PositionStatus = (input.DataMessage.ByteReader.GetOne() & 0xf0) == 0x00;
                    location.Latitude = input.DataMessage.ByteReader.Get<int>() / 1000000.0;
                    location.Longitude = input.DataMessage.ByteReader.Get<int>() / 1000000.0;
                    locations.Add(location);
                    break;

                case 0x33:
                    location.Speed = input.DataMessage.ByteReader.Get<short>() / 10;
                    location.Heading = input.DataMessage.ByteReader.Get<short>() / 10;
                    break;

                case 0x34:
                    location.Altitude = input.DataMessage.ByteReader.Get<short>();
                    break;

                case 0xE1:
                    input.DataMessage.ByteReader.Skip(input.DataMessage.ByteReader.GetOne());
                    break;

                default:
                    input.DataMessage.ByteReader.Skip(tagLength.GetValueOrDefault());
                    break;
            }
        }

        string checksum = input.DataMessage.Hex[^2..].StringJoin();
        input.NetworkStream.Write(HexUtil.ConvertHexStringToByteArray($"02{checksum}"));

        locations.ForEach(x => x.Device = input.Client.Device);

        return locations;
    }
}