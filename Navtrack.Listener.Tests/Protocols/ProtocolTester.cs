using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using Moq;
using Navtrack.DataAccess.Model;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using Navtrack.Listener.Services;

namespace Navtrack.Listener.Tests.Protocols
{
    public class ProtocolTester : IProtocolTester
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

        public ProtocolTester(IProtocol protocol, ICustomMessageHandler customMessageHandler)
        {
            cancellationTokenSource = new CancellationTokenSource();
            TotalParsedLocations = new List<Location>();

            Client = new Client
            {
                Protocol = protocol,
                DeviceConnection = new DeviceConnectionEntity()
            };

            SetupLocationService();
            SetupStreamHandler(customMessageHandler);
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

            locationServiceMock.Setup(x => x.AddRange(It.IsAny<List<Location>>(), It.IsAny<int>()))
                .Callback<List<Location>, int>((x, y) =>
                {
                    LastParsedLocations = x;
                    TotalParsedLocations.AddRange(x);
                });
        }

        private void SetupStreamHandler(ICustomMessageHandler customMessageHandler)
        {
            Mock<IServiceProvider> serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.IsAny<Type>()))
                .Returns(customMessageHandler);

            IMessageHandler messageHandlerMock = new MessageHandler(
                serviceProviderMock.Object,
                locationServiceMock.Object,
                new Mock<ILogger<MessageHandler>>().Object,
                new Mock<IConnectionService>().Object);

            streamHandler =
                new StreamHandler(new Mock<ILogger<StreamHandler>>().Object, messageHandlerMock);
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
                .Callback<byte>(x => { receiveStream = new MemoryStream(new[] {x}); });
            networkStreamWrapperMock.Setup(x => x.CanRead).Returns(true);
            networkStreamWrapperMock.Setup(x => x.DataAvailable).Returns(true);
        }
    }
}