using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Protocols.Autofon;
using Navtrack.Listener.Protocols.Galileosky;
using Navtrack.Listener.Protocols.Ruptela;
using Navtrack.Listener.Protocols.Satellite;
using Navtrack.Listener.Protocols.Teltonika;
using Navtrack.Listener.Server;
using Xunit;

namespace Navtrack.Listener.Tests.Server;

public class ProtocolConnectionHandlerTests
{
    [Theory]
    [InlineData("Galileosky", false)]
    [InlineData("Galileosky", true)]
    [InlineData("Ruptela", false)]
    [InlineData("Ruptela", true)]
    [InlineData("Satellite", false)]
    [InlineData("Satellite", true)]
    [InlineData("Teltonika", false)]
    [InlineData("Teltonika", true)]
    [InlineData("AutofonLoginV1", false)]
    [InlineData("AutofonLoginV1", true)]
    [InlineData("AutofonLoginV2", false)]
    [InlineData("AutofonLoginV2", true)]
    [InlineData("AutofonLocationV1", false)]
    [InlineData("AutofonLocationV1", true)]
    [InlineData("AutofonLocationV2", false)]
    [InlineData("AutofonLocationV2", true)]
    [InlineData("AutofonHistory", false)]
    [InlineData("AutofonHistory", true)]
    public async Task FramedPackets_AreReadAcrossGapsAndSeparatedWhenCombined(string name, bool combine)
    {
        (IProtocol protocol, byte[] packet) = GetPacket(name);
        byte[] bytes = combine ? packet.Concat(packet).ToArray() : packet;
        int offset = 0;
        Mock<INetworkStreamWrapper> stream = new();
        stream.SetupGet(x => x.CanRead).Returns(true);
        // DataAvailable is false at every byte, simulating gaps between TCP fragments.
        stream.SetupGet(x => x.DataAvailable).Returns(false);
        stream.Setup(x => x.ReadAsync(It.IsAny<Memory<byte>>(), It.IsAny<CancellationToken>()))
            .Returns<Memory<byte>, CancellationToken>(async (buffer, token) =>
            {
                await Task.Yield();
                token.ThrowIfCancellationRequested();
                if (offset == bytes.Length)
                {
                    return 0;
                }

                buffer.Span[0] = bytes[offset++];
                return 1;
            });

        List<byte[]> received = [];
        Mock<IProtocolMessageHandler> messages = new();
        messages.Setup(x => x.HandleMessage(It.IsAny<ProtocolConnectionContext>(), stream.Object, It.IsAny<byte[]>()))
            .Callback<ProtocolConnectionContext, INetworkStreamWrapper, byte[]>((context, network, data) => received.Add(data))
            .Returns(Task.CompletedTask);

        await CreateHandler(protocol, stream, messages).HandleConnection(new TcpClientAdapter(), protocol, CancellationToken.None);

        Assert.Equal(combine ? 2 : 1, received.Count);
        Assert.All(received, data => Assert.Equal(packet, data));
        stream.Verify(x => x.DisposeAsync(), Times.Once);
    }

    [Fact]
    public async Task ParserFailure_DisposesConnectionContext()
    {
        (IProtocol protocol, byte[] packet) = GetPacket("Teltonika");
        Mock<INetworkStreamWrapper> stream = CreateStream(packet);
        Mock<IProtocolMessageHandler> messages = new();
        messages.Setup(x => x.HandleMessage(It.IsAny<ProtocolConnectionContext>(), stream.Object, It.IsAny<byte[]>()))
            .ThrowsAsync(new InvalidOperationException("Parser failed"));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            CreateHandler(protocol, stream, messages).HandleConnection(new TcpClientAdapter(), protocol, CancellationToken.None));

        stream.Verify(x => x.DisposeAsync(), Times.Once);
    }

    [Fact]
    public async Task TruncatedPacket_IsRejectedBeforeParsingAndConnectionIsDisposed()
    {
        (IProtocol protocol, byte[] packet) = GetPacket("Galileosky");
        Mock<INetworkStreamWrapper> stream = CreateStream(packet[..^1]);
        Mock<IProtocolMessageHandler> messages = new();

        await CreateHandler(protocol, stream, messages).HandleConnection(new TcpClientAdapter(), protocol, CancellationToken.None);

        messages.Verify(x => x.HandleMessage(It.IsAny<ProtocolConnectionContext>(), It.IsAny<INetworkStreamWrapper>(),
            It.IsAny<byte[]>()), Times.Never);
        stream.Verify(x => x.DisposeAsync(), Times.Once);
    }

    [Fact]
    public async Task Cancellation_InterruptsIdleReadAndDisposesConnection()
    {
        IProtocol protocol = new TeltonikaProtocol();
        Mock<INetworkStreamWrapper> stream = new();
        stream.Setup(x => x.ReadAsync(It.IsAny<Memory<byte>>(), It.IsAny<CancellationToken>()))
            .Returns<Memory<byte>, CancellationToken>(async (buffer, token) =>
            {
                await Task.Delay(Timeout.Infinite, token);
                return 0;
            });
        using CancellationTokenSource cancellation = new();
        Task connection = CreateHandler(protocol, stream, new Mock<IProtocolMessageHandler>())
            .HandleConnection(new TcpClientAdapter(), protocol, cancellation.Token);

        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => connection.WaitAsync(TimeSpan.FromSeconds(5)));
        stream.Verify(x => x.DisposeAsync(), Times.Once);
    }

    [Fact]
    public async Task PacketLargerThanOldBuffer_IsReadInFull()
    {
        IProtocol protocol = new GalileoskyProtocol();
        byte[] packet = new byte[4101];
        packet[0] = 1;
        packet[2] = 0x10;
        Mock<INetworkStreamWrapper> stream = CreateStream(packet);
        Mock<IProtocolMessageHandler> messages = new();
        messages.Setup(x => x.HandleMessage(It.IsAny<ProtocolConnectionContext>(), stream.Object, It.IsAny<byte[]>()))
            .Returns(Task.CompletedTask);

        await CreateHandler(protocol, stream, messages).HandleConnection(new TcpClientAdapter(), protocol, CancellationToken.None);

        messages.Verify(x => x.HandleMessage(It.IsAny<ProtocolConnectionContext>(), stream.Object,
            It.Is<byte[]>(data => data.Length == packet.Length)), Times.Once);
    }

    private static Mock<INetworkStreamWrapper> CreateStream(byte[] bytes)
    {
        int offset = 0;
        Mock<INetworkStreamWrapper> stream = new();
        stream.SetupGet(x => x.CanRead).Returns(true);
        stream.Setup(x => x.ReadAsync(It.IsAny<Memory<byte>>(), It.IsAny<CancellationToken>()))
            .Returns<Memory<byte>, CancellationToken>((buffer, token) =>
            {
                if (offset == bytes.Length)
                {
                    return ValueTask.FromResult(0);
                }

                buffer.Span[0] = bytes[offset++];
                return ValueTask.FromResult(1);
            });
        return stream;
    }

    private static ProtocolConnectionHandler CreateHandler(IProtocol protocol, Mock<INetworkStreamWrapper> stream,
        Mock<IProtocolMessageHandler> messages)
    {
        ProtocolConnectionContext context = new(stream.Object, protocol, Guid.NewGuid());
        Mock<IProtocolConnectionContextFactory> factory = new();
        factory.Setup(x => x.GetConnectionContext(protocol, It.IsAny<TcpClientAdapter>())).ReturnsAsync(context);
        return new ProtocolConnectionHandler(messages.Object, factory.Object, new Mock<ILogger<ProtocolConnectionHandler>>().Object);
    }

    private static (IProtocol, byte[]) GetPacket(string name)
    {
        return name switch
        {
            "Galileosky" => (new GalileoskyProtocol(), HexUtil.ConvertHexStringToByteArray("011780011102DF03383638323034303035363437383338043200869C")),
            "Ruptela" => (new RuptelaProtocol(), HexUtil.ConvertHexStringToByteArray("0050000310f5615f419c0100015613d8ed0000fff5b37a035af37801e700000900000d07071b0c020003001c01202cad000500064302a81d33e61e100116317cd3ffff174ad60241000077fa960000f232003c2e")),
            "Satellite" => (new SatelliteProtocol(), HexUtil.ConvertHexStringToByteArray("0000BF4CB2EC16000000")),
            "Teltonika" => (new TeltonikaProtocol(), HexUtil.ConvertHexStringToByteArray("000F383638323034303030353634373833")),
            "AutofonLoginV1" => (new AutofonProtocol(), HexUtil.ConvertHexStringToByteArray("10556103592310314825728F")),
            "AutofonLoginV2" => (new AutofonProtocol(), HexUtil.ConvertHexStringToByteArray("41035151305289931441139602662095148807")),
            "AutofonLocationV1" => (new AutofonProtocol(), CreateAutofonPacket(MessageType.LocationV1, 78)),
            "AutofonLocationV2" => (new AutofonProtocol(), HexUtil.ConvertHexStringToByteArray("023E00001E004D411EFA01772F185285009C48041F1E366C2961380F26B10B00911C")),
            "AutofonHistory" => (new AutofonProtocol(), CreateAutofonPacket(MessageType.HistoryV1, 257)),
            _ => throw new ArgumentException("Unknown protocol", nameof(name))
        };
    }

    private static byte[] CreateAutofonPacket(MessageType type, int length)
    {
        byte[] bytes = new byte[length];
        bytes[0] = (byte)type;
        return bytes;
    }
}
