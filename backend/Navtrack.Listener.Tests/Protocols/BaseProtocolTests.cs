using System;
using System.Globalization;
using System.Threading;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
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
        foreach (Position position in ProtocolTester.TotalParsedPositions)
        {
            PositionIsValid(position);
        }
    }

    private static void PositionIsValid(Position position)
    {
        Assert.True(GpsUtil.IsValidLatitude(position.Latitude));
        Assert.True(GpsUtil.IsValidLongitude(position.Longitude));
        Assert.True(position.Date >= DateTime.UnixEpoch);
        Assert.True(position.Speed is null or >= 0 and <= 1000);
        Assert.True(position.Heading is null or >= 0 and <= 360);
        Assert.True(position.Satellites is null or >= 0 and <= 50);
        Assert.True(position.HDOP is null or >= 0 and <= 100);
        Assert.True(position.Odometer is null or >= 0);
        Assert.NotNull(position.Device);
        Assert.NotEmpty(position.Device.SerialNumber);
    }
}