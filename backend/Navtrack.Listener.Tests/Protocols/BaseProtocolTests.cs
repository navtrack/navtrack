using System;
using System.Globalization;
using System.Threading;
using Navtrack.Database.Model.Devices;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Xunit;

namespace Navtrack.Listener.Tests.Protocols;

public class BaseProtocolTests<TProtocol, TMessageHandler> : IDisposable where TProtocol : IProtocol, new()
    where TMessageHandler : ICustomMessageHandler, new()
{
    protected readonly IProtocolTester ProtocolTester;

    protected BaseProtocolTests()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        ProtocolTester = new ProtocolTester<TProtocol, TMessageHandler>();
    }

    public void Dispose()
    {
        foreach (DeviceMessageEntity position in ProtocolTester.TotalParsedMessages)
        {
            PositionIsValid(position);
        }
    }

    private void PositionIsValid(DeviceMessageEntity deviceMessageDocument)
    {
        Assert.True(GpsUtil.IsValidLatitude(deviceMessageDocument.Latitude));
        Assert.True(GpsUtil.IsValidLongitude(deviceMessageDocument.Longitude));
        Assert.True(deviceMessageDocument.Speed is null or >= 0 and <= 1000);
        Assert.True(deviceMessageDocument.Heading is null or >= 0 and <= 360);
        Assert.True(deviceMessageDocument.Satellites is null or >= 0);
        Assert.True(deviceMessageDocument.HDOP is null or >= 0 and <= 100);
        Assert.NotNull(ProtocolTester.ConnectionContext.Device);
        Assert.NotEmpty(ProtocolTester.ConnectionContext.Device.SerialNumber);
    }
}