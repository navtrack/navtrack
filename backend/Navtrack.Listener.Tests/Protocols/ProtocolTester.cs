using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Moq;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using Navtrack.Listener.Services;

namespace Navtrack.Listener.Tests.Protocols;

public class ProtocolTester<TProtocol, TMessageHandler> : IProtocolTester
    where TProtocol : IProtocol, new() where TMessageHandler : new()
{
    private MemoryStream sendStream;
    private MemoryStream receiveStream;
    private IStreamHandler streamHandler;
    private readonly CancellationTokenSource cancellationTokenSource;
    private Mock<INetworkStreamWrapper> networkStreamWrapperMock;
    private Mock<ILocationService> locationServiceMock;

    public Client Client { get; }

    public List<Location> LastParsedLocations { get; private set; }
    public List<Location> TotalParsedLocations { get; }
    public Location LastParsedLocation => LastParsedLocations?.FirstOrDefault();

    public ProtocolTester()
    {
        cancellationTokenSource = new CancellationTokenSource();
        TotalParsedLocations = new List<Location>();

        Client = new Client
        {
            Protocol = new TProtocol(),
            DeviceConnection = new DeviceConnectionDocument()
        };

        SetupLocationService();
        SetupStreamHandler();
        SetupNetworkStreamMock();
    }

    public void SendBytesFromDevice(byte[] value)
    {
        sendStream = new MemoryStream(value);

        CallStreamHandler();
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

    private void CallStreamHandler()
    {
        streamHandler.HandleStream(cancellationTokenSource.Token, Client, networkStreamWrapperMock.Object);
    }

    private void SetupLocationService()
    {
        locationServiceMock = new Mock<ILocationService>();

        locationServiceMock.Setup(x => x.AddRange(It.IsAny<List<Location>>(), It.IsAny<ObjectId>()))
            .Callback<List<Location>, ObjectId>((x, _) =>
            {
                LastParsedLocations = x;
                TotalParsedLocations.AddRange(x);
            });
    }

    private void SetupStreamHandler()
    {
        Mock<IServiceProvider> serviceProviderMock = new();
        serviceProviderMock.Setup(x => x.GetService(It.IsAny<Type>()))
            .Returns(() => new TMessageHandler());
        Mock<IServiceScope> serviceScopeMock = new();
        serviceScopeMock.Setup(x => x.ServiceProvider).Returns(serviceProviderMock.Object);
        Mock<IServiceScopeFactory> serviceScopeFactoryMock = new();
        serviceScopeFactoryMock.Setup(x => x.CreateScope())
            .Returns(serviceScopeMock.Object);
        serviceProviderMock.Setup(x => x.GetService(typeof(IServiceScopeFactory)))
            .Returns(serviceScopeFactoryMock.Object);

        IMessageHandler messageHandler = new MessageHandler(
            serviceProviderMock.Object,
            locationServiceMock.Object,
            new Mock<ILogger<MessageHandler>>().Object,
            new Mock<IConnectionService>().Object);

        streamHandler =
            new StreamHandler(new Mock<ILogger<StreamHandler>>().Object, messageHandler);
    }

    private void SetupNetworkStreamMock()
    {
        networkStreamWrapperMock = new Mock<INetworkStreamWrapper>();

        networkStreamWrapperMock
            .Setup(x => x.Read(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns<byte[], int, int>((x, y, z) => sendStream.Read(x, y, z));
        networkStreamWrapperMock
            .Setup(x => x.Read(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns<byte[], int, int>((x, y, z) => sendStream.Read(x, y, z));
        networkStreamWrapperMock
            .Setup(x => x.Write(It.IsAny<byte[]>()))
            .Callback<byte[]>(x => { receiveStream = new MemoryStream(x); });
        networkStreamWrapperMock
            .Setup(x => x.WriteByte(It.IsAny<byte>()))
            .Callback<byte>(x => { receiveStream = new MemoryStream(new[] { x }); });
        networkStreamWrapperMock.Setup(x => x.CanRead).Returns(true);
        networkStreamWrapperMock.Setup(x => x.DataAvailable).Returns(true);
    }
}