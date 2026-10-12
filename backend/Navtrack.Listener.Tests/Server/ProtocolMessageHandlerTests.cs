using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Devices;
using Navtrack.Listener.Models;
using Navtrack.Listener.Protocols.Teltonika;
using Navtrack.Listener.Server;
using Navtrack.Listener.Services;
using Xunit;

namespace Navtrack.Listener.Tests.Server;

public class ProtocolMessageHandlerTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task MessageScope_IsDisposedOnSuccessAndParserFailure(bool fail)
    {
        Mock<ICustomMessageHandler<TeltonikaProtocol>> parser = new();
        if (fail)
        {
            parser.Setup(x => x.ParseRange(It.IsAny<MessageInput>())).Throws(new InvalidOperationException("Parser failed"));
        }

        Mock<IServiceProvider> scopedProvider = new();
        scopedProvider.Setup(x => x.GetService(typeof(ICustomMessageHandler<TeltonikaProtocol>))).Returns(parser.Object);
        Mock<IServiceScope> scope = new();
        scope.SetupGet(x => x.ServiceProvider).Returns(scopedProvider.Object);
        Mock<IServiceScopeFactory> scopeFactory = new();
        scopeFactory.Setup(x => x.CreateScope()).Returns(scope.Object);
        Mock<IServiceProvider> provider = new();
        provider.Setup(x => x.GetService(typeof(IServiceScopeFactory))).Returns(scopeFactory.Object);
        INetworkStreamWrapper stream = new Mock<INetworkStreamWrapper>().Object;
        ProtocolConnectionContext context = new(stream, new TeltonikaProtocol(), Guid.NewGuid());
        ProtocolMessageHandler handler = new(new Mock<ILogger<ProtocolMessageHandler>>().Object, provider.Object,
            new Mock<IDeviceMessageService>().Object, new Mock<IAssetRepository>().Object,
            new Mock<IDeviceConnectionRepository>().Object);

        if (fail)
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleMessage(context, stream, [1]));
        }
        else
        {
            await handler.HandleMessage(context, stream, [1]);
        }

        scope.Verify(x => x.Dispose(), Times.Once);
    }
}
