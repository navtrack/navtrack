using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Moq;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.DataAccess.Services.Positions;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using Navtrack.Listener.Services;

namespace Navtrack.Listener.Tests.Protocols;

public class ProtocolTester<TProtocol, TMessageHandler> : IProtocolTester
    where TProtocol : IProtocol, new() where TMessageHandler : new()
{
    private MemoryStream sendStream = new();
    private MemoryStream receiveStream = new();

    private readonly ProtocolConnectionHandler protocolConnectionHandler;
    private readonly Mock<INetworkStreamWrapper> networkStreamWrapperMock;
    private readonly Mock<IPositionService> locationServiceMock;
    private readonly CancellationTokenSource cancellationTokenSource;

    public ProtocolConnectionContext ConnectionContext { get; }

    public List<Position>? LastParsedPositions { get; private set; }
    public List<Position> TotalParsedPositions { get; }
    public Position? LastParsedPosition => LastParsedPositions?.FirstOrDefault();

    public ProtocolTester()
    {
        cancellationTokenSource = new CancellationTokenSource();
        TotalParsedPositions = [];

        locationServiceMock = GetPositionService();
        protocolConnectionHandler = GetProtocolClientHandler();
        networkStreamWrapperMock = SetupNetworkStreamMock();
        ConnectionContext = GetProtocolClient();
    }

    public void SendBytesFromDevice(byte[] value)
    {
        sendStream = new MemoryStream(value);

        CallStreamHandler().Wait();
    }

    public void SendStringFromDevice(string value)
    {
        SendBytesFromDevice(StringUtil.ConvertStringToByteArray(value));
    }

    public void SendHexFromDevice(string value)
    {
        SendBytesFromDevice(HexUtil.ConvertHexStringToByteArray(value.Replace(" ", "")));
    }

    public string ReceiveHexInDevice()
    {
        byte[] buffer = new byte[ServerVariables.BufferLength];

        int length = receiveStream.Read(buffer, 0, ServerVariables.BufferLength);

        return HexUtil.ConvertHexStringArrayToHexString(HexUtil.ConvertByteArrayToHexStringArray(buffer[..length]));
    }

    public string ReceiveStringInDevice()
    {
        byte[] buffer = new byte[ServerVariables.BufferLength];

        int length = receiveStream.Read(buffer, 0, ServerVariables.BufferLength);

        return StringUtil.ConvertByteArrayToString(buffer[..length]);
    }

    private async Task CallStreamHandler()
    {
        await protocolConnectionHandler.HandleConnection(ConnectionContext, cancellationTokenSource.Token);
    }

    private Mock<IPositionService> GetPositionService()
    {
        Mock<IPositionService> mock = new();

        mock.Setup(x =>
                x.Save(It.IsAny<Device>(), It.IsAny<DateTime>(), It.IsAny<ObjectId>(), It.IsAny<List<Position>>()))
            .Returns<Device, DateTime, ObjectId, IEnumerable<Position>>((_, _, _, locations) => Task.FromResult(new SavePositionsResult
            {
                Success = true,
                MaxDate = locations.Max(x => x.Date)
            }))
            .Callback<Device, DateTime, ObjectId, IEnumerable<Position>>((_, _, _, locations) =>
            {
                List<Position> locationsList = locations.ToList();
                LastParsedPositions = locationsList;
                TotalParsedPositions.AddRange(locationsList);
            });

        return mock;
    }

    private ProtocolConnectionHandler GetProtocolClientHandler()
    {
        Mock<IServiceProvider> mock = new();

        mock.Setup(x => x.GetService(It.IsAny<Type>()))
            .Returns(() => new TMessageHandler());

        Mock<IServiceScope> serviceScopeMock = new();
        serviceScopeMock.Setup(x => x.ServiceProvider).Returns(mock.Object);

        Mock<IServiceScopeFactory> serviceScopeFactoryMock = new();
        serviceScopeFactoryMock.Setup(x => x.CreateScope())
            .Returns(serviceScopeMock.Object);
        mock.Setup(x => x.GetService(typeof(IServiceScopeFactory)))
            .Returns(serviceScopeFactoryMock.Object);

        IProtocolMessageHandler protocolMessageHandler = new ProtocolMessageHandler(
            new Mock<ILogger<ProtocolMessageHandler>>().Object,
            mock.Object,
            locationServiceMock.Object,
            new Mock<IAssetRepository>().Object,
            new Mock<IConnectionRepository>().Object);

        ProtocolConnectionHandler handler = new(new Mock<ILogger<ProtocolConnectionHandler>>().Object,
            protocolMessageHandler);

        return handler;
    }

    private Mock<INetworkStreamWrapper> SetupNetworkStreamMock()
    {
        Mock<INetworkStreamWrapper> mock = new();

        mock
            .Setup(x => x.Read(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns<byte[], int, int>((x, y, z) => sendStream.Read(x, y, z));
        mock
            .Setup(x => x.Read(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns<byte[], int, int>((x, y, z) => sendStream.Read(x, y, z));
        mock
            .Setup(x => x.Write(It.IsAny<byte[]>()))
            .Callback<byte[]>(x => { receiveStream = new MemoryStream(x); });
        mock
            .Setup(x => x.WriteByte(It.IsAny<byte>()))
            .Callback<byte>(x => { receiveStream = new MemoryStream(new[] { x }); });
        mock.Setup(x => x.CanRead).Returns(true);
        mock.Setup(x => x.DataAvailable).Returns(true);
        mock.Setup(x => x.TcpClient).Returns(new TcpClient());

        return mock;
    }

    private ProtocolConnectionContext GetProtocolClient()
    {
        return new ProtocolConnectionContext(networkStreamWrapperMock.Object, new TProtocol(), ObjectId.GenerateNewId());
    }
}