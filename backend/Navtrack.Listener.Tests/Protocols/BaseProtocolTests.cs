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
    protected internal IProtocolTester ProtocolTester;

    public BaseProtocolTests()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        ProtocolTester = new ProtocolTester<TProtocol, TMessageHandler>();
    }

    public void Dispose()
    {
        foreach (Position location in ProtocolTester.TotalParsedPositions)
        {
            PositionIsValid(location);
        }
    }

    private static void PositionIsValid(Position position)
    {
        Assert.True(GpsUtil.IsValidLatitude(position.Latitude));
        Assert.True(GpsUtil.IsValidLongitude(position.Longitude));
        Assert.True(position.Date >= DateTime.UnixEpoch);
        Assert.True(!position.Speed.HasValue || position.Speed >= 0 && position.Speed <= 1000);
        Assert.True(!position.Heading.HasValue || position.Heading.Value >= 0 && position.Heading.Value <= 360);
        Assert.True(!position.Satellites.HasValue || position.Satellites >= 0 && position.Satellites <= 50);
        Assert.True(!position.HDOP.HasValue || position.HDOP >= 0 && position.HDOP <= 100);
        Assert.True(!position.Odometer.HasValue || position.Odometer >= 0);
        Assert.NotNull(position.Device);
        Assert.NotEmpty(position.Device.SerialNumber);
    }
}