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
    private protected IProtocolTester ProtocolTester;

    public BaseProtocolTests()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        ProtocolTester = new ProtocolTester<TProtocol, TMessageHandler>();
    }

    public void Dispose()
    {
        foreach (Location location in ProtocolTester.TotalParsedLocations)
        {
            LocationIsValid(location);
        }
    }

    private static void LocationIsValid(Location location)
    {
        Assert.True(GpsUtil.IsValidLatitude(location.Latitude));
        Assert.True(GpsUtil.IsValidLongitude(location.Longitude));
        Assert.True(location.DateTime >= DateTime.UnixEpoch);
        Assert.True(!location.Speed.HasValue || location.Speed >= 0 && location.Speed <= 1000);
        Assert.True(!location.Heading.HasValue || location.Heading.Value >= 0 && location.Heading.Value <= 360);
        Assert.True(!location.Satellites.HasValue || location.Satellites >= 0 && location.Satellites <= 50);
        Assert.True(!location.HDOP.HasValue || location.HDOP >= 0 && location.HDOP <= 100);
        Assert.True(!location.Odometer.HasValue || location.Odometer >= 0);
        Assert.NotNull(location.Device);
        Assert.NotEmpty(location.Device.IMEI);
    }
}