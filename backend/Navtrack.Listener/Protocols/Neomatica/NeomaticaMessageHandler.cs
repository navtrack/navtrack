using System;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Neomatica;

[Service(typeof(ICustomMessageHandler<NeomaticaProtocol>))]
public class NeomaticaMessageHandler : BaseMessageHandler<NeomaticaProtocol>
{
    public override Position Parse(MessageInput input)
    {
        input.DataMessage.ByteReader.Get<short>(); // device id

        int size = input.DataMessage.ByteReader.GetOne();
        int type = input.DataMessage.ByteReader.GetOne();

        if (type == 0x03) // imei
        {
            input.ConnectionContext.SetDevice(StringUtil.ConvertByteArrayToString(input.DataMessage.ByteReader.Get(15)));
        }
        else
        {
            return GetLocation(input, type);
        }

        return null;
    }


    private static Position GetLocation(MessageInput input, int type)
    {
        if (input.ConnectionContext.Device == null)
        {
            return null;
        }

        Position position = new()
        {
            Device = input.ConnectionContext.Device
        };

        input.DataMessage.ByteReader.GetOne();
        input.DataMessage.ByteReader.Get<short>();

        short status = input.DataMessage.ByteReader.Get<short>();
        position.PositionStatus = !BitUtil.IsTrue(status, 5);
        position.Latitude = input.DataMessage.ByteReader.Get<float>();
        position.Longitude = input.DataMessage.ByteReader.Get<float>();
        position.Heading = input.DataMessage.ByteReader.Get<short>() * 0.1f;
        position.Speed = input.DataMessage.ByteReader.Get<short>() * 0.1f;
        input.DataMessage.ByteReader.GetOne();
        position.Altitude = input.DataMessage.ByteReader.Get<short>();
        position.HDOP = input.DataMessage.ByteReader.GetOne() * 0.1f;
        position.Satellites = (short?)(input.DataMessage.ByteReader.GetOne() & 0x0f);
        position.Date = DateTime.UnixEpoch.AddSeconds(input.DataMessage.ByteReader.Get<int>());

        MarkAsNull(position);

        return position;
    }

    private static void MarkAsNull(Position position)
    {
        if (!position.PositionStatus.GetValueOrDefault())
        {
            position.Heading = null;
            position.Speed = null;
            position.Altitude = null;
            position.HDOP = null;
            position.Satellites = null;
        }
    }
}