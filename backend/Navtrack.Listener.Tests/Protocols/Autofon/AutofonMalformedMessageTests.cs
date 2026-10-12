using System;
using System.IO;
using Moq;
using Navtrack.Listener.Models;
using Navtrack.Listener.Protocols.Autofon;
using Navtrack.Listener.Server;
using Xunit;

namespace Navtrack.Listener.Tests.Protocols.Autofon;

public class AutofonMalformedMessageTests
{
    [Theory]
    [InlineData(0, 10, 26, 12, 0, 0)]
    [InlineData(31, 2, 26, 12, 0, 0)]
    [InlineData(1, 13, 26, 12, 0, 0)]
    [InlineData(1, 10, 26, 24, 0, 0)]
    [InlineData(1, 10, 26, 12, 60, 0)]
    [InlineData(1, 10, 26, 12, 0, 60)]
    public void InvalidDeviceDates_AreRejected(byte day, byte month, byte year, byte hour, byte minute, byte second)
    {
        byte[] packet = new byte[78];
        packet[0] = (byte)MessageType.LocationV1;
        new byte[] { day, month, year, hour, minute, second }.CopyTo(packet, 55);
        INetworkStreamWrapper stream = new Mock<INetworkStreamWrapper>().Object;
        MessageInput input = new()
        {
            ConnectionContext = new ProtocolConnectionContext(stream, new AutofonProtocol(), Guid.NewGuid()),
            NetworkStream = stream,
            DataMessage = new DataMessage(packet, string.Empty)
        };

        Assert.Throws<InvalidDataException>(() => new AutofonMessageHandler().ParseRange(input));
    }
}
