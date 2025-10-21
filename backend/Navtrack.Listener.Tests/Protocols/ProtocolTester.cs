using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Devices;
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
    private readonly Mock<IDeviceMessageService> locationServiceMock;
    private readonly Mock<TcpClientAdapter> tcpClientAdapterMock;
    private readonly CancellationTokenSource cancellationTokenSource;

    public ProtocolConnectionContext ConnectionContext { get; }

    public List<DeviceMessageEntity> LastParsedMessages { get; private set; }
    public List<DeviceMessageEntity> TotalParsedMessages { get; }
    public DeviceMessageEntity? LastParsedMessage => LastParsedMessages?.FirstOrDefault();

    public ProtocolTester()
    {
        cancellationTokenSource = new CancellationTokenSource();
        TotalParsedMessages = [];

        locationServiceMock = GetPositionService();
        
        networkStreamWrapperMock = SetupNetworkStreamMock();
        
        ConnectionContext = new ProtocolConnectionContext(networkStreamWrapperMock.Object, new TProtocol(), Guid.NewGuid());
        
        protocolConnectionHandler = GetProtocolClientHandler();
        
        tcpClientAdapterMock = new Mock<TcpClientAdapter>();
        tcpClientAdapterMock.Setup(x => x.GetRemoteEndPoint()).Returns("12.34.56.78");
    }

    public void SendBytesFromDevice(byte[] value)
    {
        sendStream = new MemoryStream(value);

        protocolConnectionHandler.HandleConnection(tcpClientAdapterMock.Object, 
            new Mock<IProtocol>().Object,
            cancellationTokenSource.Token).Wait();
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

    private Mock<IDeviceMessageService> GetPositionService()
    {
        Mock<IDeviceMessageService> mock = new();

        mock.Setup(x => x.Save(It.IsAny<SaveDeviceMessageInput>()))
            .Callback<SaveDeviceMessageInput>(input =>
            {
                LastParsedMessages = input.Messages.ToList();
                TotalParsedMessages.AddRange(LastParsedMessages);
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
            new Mock<IDeviceConnectionRepository>().Object);

        Mock<IProtocolConnectionContextFactory> protocolConnectionContextFactoryMock = new();
        protocolConnectionContextFactoryMock
            .Setup(x => x.GetConnectionContext(It.IsAny<IProtocol>(), It.IsAny<TcpClientAdapter>()))
            .ReturnsAsync(ConnectionContext);

        ProtocolConnectionHandler handler = new(protocolMessageHandler,
            protocolConnectionContextFactoryMock.Object,
            new Mock<ILogger<ProtocolConnectionHandler>>().Object);

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
            .Callback<byte>(x => { receiveStream = new MemoryStream([x]); });
        mock.Setup(x => x.CanRead).Returns(true);
        mock.Setup(x => x.DataAvailable).Returns(true);

        return mock;
    }
}