using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Navtrack.Listener;
using Navtrack.Listener.Models;
using Navtrack.Listener.Protocols.Teltonika;
using Navtrack.Listener.Server;
using Xunit;

namespace Navtrack.Listener.Tests.Server;

public class ConnectionLifecycleTests
{
    [Fact]
    public async Task RemoteEndpoint_RemainsAvailableAfterClientCloses()
    {
        TcpListener listener = new(IPAddress.Loopback, 0);
        listener.Start();
        try
        {
            using TcpClient client = new();
            Task connect = client.ConnectAsync((IPEndPoint)listener.LocalEndpoint);
            using TcpClient accepted = await listener.AcceptTcpClientAsync();
            await connect;
            using TcpClientAdapter adapter = new(accepted);
            NetworkStreamWrapper stream = new(adapter);
            string? endpoint = stream.RemoteEndPoint;

            stream.Close();

            Assert.NotNull(endpoint);
            Assert.Equal(endpoint, stream.RemoteEndPoint);
            await stream.DisposeAsync();
        }
        finally
        {
            listener.Stop();
        }
    }

    [Fact]
    public void SerialNumberWithNul_IsRejectedBeforeDeviceIsCreated()
    {
        ProtocolConnectionContext context = new(new Mock<INetworkStreamWrapper>().Object, new TeltonikaProtocol(), Guid.NewGuid());
        Assert.Throws<InvalidDataException>(() => context.SetDevice("123\0"));
        Assert.Null(context.Device);
    }

    [Fact]
    public async Task CancelledAccept_StopsNormally()
    {
        Mock<IProtocol> protocol = new();
        Mock<ILogger<ProtocolListener>> logger = new();
        ProtocolListener listener = new(new Mock<IServiceProvider>().Object, logger.Object);
        using CancellationTokenSource cancellation = new();
        Task listening = listener.Start(protocol.Object, cancellation.Token);

        cancellation.Cancel();
        await listening.WaitAsync(TimeSpan.FromSeconds(5));

        Assert.DoesNotContain(logger.Invocations, invocation => invocation.Method.Name == "Log" &&
            invocation.Arguments[0] is LogLevel level && level >= LogLevel.Error);
    }

    [Fact]
    public async Task HostedService_StartReturnsAndStopCancelsListeners()
    {
        Mock<IProtocolListener> listener = new();
        TaskCompletionSource started = new(TaskCreationOptions.RunContinuationsAsynchronously);
        listener.Setup(x => x.Start(It.IsAny<IProtocol>(), It.IsAny<CancellationToken>()))
            .Returns<IProtocol, CancellationToken>(async (protocol, token) =>
            {
                started.SetResult();
                await Task.Delay(Timeout.Infinite, token);
            });
        using ListenerHostedService service = new(listener.Object, [new TeltonikaProtocol()]);

        await service.StartAsync(CancellationToken.None).WaitAsync(TimeSpan.FromSeconds(5));
        await started.Task.WaitAsync(TimeSpan.FromSeconds(5));
        await service.StopAsync(CancellationToken.None).WaitAsync(TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task ListenerFailure_IsObservedAndCancelsOtherListeners()
    {
        Mock<IProtocolListener> listener = new();
        TaskCompletionSource failed = new(TaskCreationOptions.RunContinuationsAsynchronously);
        TaskCompletionSource otherStarted = new(TaskCreationOptions.RunContinuationsAsynchronously);
        TaskCompletionSource otherCancelled = new(TaskCreationOptions.RunContinuationsAsynchronously);
        TeltonikaProtocol first = new();
        Mock<IProtocol> second = new();
        listener.Setup(x => x.Start(first, It.IsAny<CancellationToken>())).Returns(failed.Task);
        listener.Setup(x => x.Start(second.Object, It.IsAny<CancellationToken>()))
            .Returns<IProtocol, CancellationToken>(async (protocol, token) =>
            {
                otherStarted.SetResult();
                try
                {
                    await Task.Delay(Timeout.Infinite, token);
                }
                catch (OperationCanceledException) when (token.IsCancellationRequested)
                {
                    otherCancelled.SetResult();
                }
            });
        using ListenerHostedService service = new(listener.Object, [first, second.Object]);
        await service.StartAsync(CancellationToken.None);
        await otherStarted.Task.WaitAsync(TimeSpan.FromSeconds(5));

        failed.SetException(new InvalidOperationException("Listener failed"));

        Task execution = Assert.IsAssignableFrom<Task>(service.ExecuteTask);
        await Assert.ThrowsAsync<InvalidOperationException>(() => execution.WaitAsync(TimeSpan.FromSeconds(5)));
        await otherCancelled.Task.WaitAsync(TimeSpan.FromSeconds(5));
    }
}
