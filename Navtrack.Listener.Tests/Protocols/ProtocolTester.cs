using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using Moq;
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
        
        public IEnumerable<Location> ParsedLocations { get; private set; }
        public Location ParsedLocation => ParsedLocations.FirstOrDefault();

        public ProtocolTester(IProtocol protocol, ICustomMessageHandler customMessageHandler)
        {
            cancellationTokenSource = new CancellationTokenSource();
            
            Client = new Client
            {
                Protocol = protocol
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

        public string ReceiveInDevice()
        {
            byte[] buffer = new byte[2048];

            int length = receiveStream.Read(buffer, 0, 2048);

            return HexUtil.ConvertHexStringArrayToHexString(HexUtil.ConvertByteArrayToHexStringArray(buffer[..length]));
        }

        private void CallStreamHandler()
        {
            streamHandler.HandleStream(cancellationTokenSource.Token, Client, networkStreamWrapperMock.Object);
        }
        
        private void SetupLocationService()
        {
            locationServiceMock = new Mock<ILocationService>();

            locationServiceMock.Setup(x => x.AddRange(It.IsAny<List<Location>>()))
                .Callback<List<Location>>(x => ParsedLocations = x);
        }

        private void SetupStreamHandler(ICustomMessageHandler customMessageHandler)
        {
            Mock<IServiceProvider> serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.IsAny<Type>()))
                .Returns(customMessageHandler);

            IMessageHandler messageHandlerMock = new MessageHandler(serviceProviderMock.Object,
                locationServiceMock.Object, new Mock<ILogger<MessageHandler>>().Object);

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
        }
    }
}