using System;
using System.Globalization;
using System.Threading;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Server;
using Xunit;

namespace Navtrack.Listener.Tests.Protocols;

public class BaseProtocolTests<TProtocol, TMessageHandler> : IDisposable where TProtocol : IProtocol, new()
    where TMessageHandler : ICustomMessageHandler , new()
{
    protected readonly IProtocolTester ProtocolTester;

    protected BaseProtocolTests()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        ProtocolTester = new ProtocolTester<TProtocol, TMessageHandler>();
    }

    public void Dispose()
    {
        foreach (DeviceMessageDocument position in ProtocolTester.TotalParsedMessages)
        {
            PositionIsValid(position);
        }
    }

    private void PositionIsValid(DeviceMessageDocument deviceMessageDocument)
    {
        Assert.True(GpsUtil.IsValidLatitude(deviceMessageDocument.Position.Latitude));
        Assert.True(GpsUtil.IsValidLongitude(deviceMessageDocument.Position.Longitude));
        Assert.True(deviceMessageDocument.Position.Date >= DateTime.UnixEpoch);
        Assert.True(deviceMessageDocument.Position.Speed is null or >= 0 and <= 1000);
        Assert.True(deviceMessageDocument.Position.Heading is null or >= 0 and <= 360);
        Assert.True(deviceMessageDocument.Position.Satellites is null or >= 0 and <= 50);
        Assert.True(deviceMessageDocument.Position.HDOP is null or >= 0 and <= 100);
        Assert.True(deviceMessageDocument.Position.Odometer is null or >= 0);
        Assert.NotNull(ProtocolTester.ConnectionContext.Device);
        Assert.NotEmpty(ProtocolTester.ConnectionContext.Device.SerialNumber);
    }
}