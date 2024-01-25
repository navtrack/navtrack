using System;
using System.Collections.Generic;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Galileosky;

[Service(typeof(ICustomMessageHandler<GalileoskyProtocol>))]
public class GalileoskyMessageHandler : BaseMessageHandler<GalileoskyProtocol>
{
    public override IEnumerable<Position>? ParseRange(MessageInput input)
    {
        input.DataMessage.ByteReader.Skip(1);
        int length = input.DataMessage.ByteReader.Get<short>() & 0x7fff;

        List<Position> positions = [];
        Position position = new();

        HashSet<byte> tagsFound = [];

        while (input.DataMessage.ByteReader.Index < input.DataMessage.Bytes.Length - 2)
        {
            byte tag = input.DataMessage.ByteReader.GetOne();

            if (tagsFound.Contains(tag))
            {
                position = new Position();
                tagsFound.Clear();
            }

            tagsFound.Add(tag);
            int? tagLength = GalileoskyProtocolTags.GetTagLength(tag);

            switch (tag)
            {
                case 0x03:
                    byte[] bytes = input.DataMessage.ByteReader.Get(tagLength.GetValueOrDefault());
                    input.ConnectionContext.SetDevice(StringUtil.ConvertByteArrayToString(
                        bytes));
                    break;

                case 0x20:
                    position.Date = DateTime.UnixEpoch.AddSeconds(input.DataMessage.ByteReader.Get<int>());
                    break;

                case 0x30:

                    position.PositionStatus = (input.DataMessage.ByteReader.GetOne() & 0xf0) == 0x00;
                    position.Latitude = input.DataMessage.ByteReader.Get<int>() / 1000000.0;
                    position.Longitude = input.DataMessage.ByteReader.Get<int>() / 1000000.0;
                    positions.Add(position);
                    break;

                case 0x33:
                    position.Speed = input.DataMessage.ByteReader.Get<short>() / 10;
                    position.Heading = input.DataMessage.ByteReader.Get<short>() / 10;
                    break;

                case 0x34:
                    position.Altitude = input.DataMessage.ByteReader.Get<short>();
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

        positions.ForEach(x => x.Device = input.ConnectionContext.Device);

        return positions;
    }
}