using System;
using System.Threading;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols;

public class BaseProtocolTests<TProtocol, TMessageHandler> where TProtocol : IProtocol, new()
    where TMessageHandler : ICustomMessageHandler, new()
{
    private protected IProtocolTester ProtocolTester;

    [SetUp]
    public void Setup()
    {
        Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
        ProtocolTester = new ProtocolTester<TProtocol, TMessageHandler>();
    }

    [TearDown]
    public void LocationsAreValid()
    {
        foreach (Location location in ProtocolTester.TotalParsedLocations)
        {
            LocationIsValid(location);
        }
    }

    private static void LocationIsValid(Location location)
    {
        Assert.IsTrue(GpsUtil.IsValidLatitude(location.Latitude));
        Assert.IsTrue(GpsUtil.IsValidLongitude(location.Longitude));
        Assert.IsTrue(location.DateTime >= DateTime.UnixEpoch);
        Assert.IsTrue(!location.Speed.HasValue || location.Speed >= 0 && location.Speed <= 1000);
        Assert.IsTrue(!location.Heading.HasValue || location.Heading.Value >= 0 && location.Heading.Value <= 360);
        Assert.IsTrue(!location.Satellites.HasValue || location.Satellites >= 0 && location.Satellites <= 50);
        Assert.IsTrue(!location.HDOP.HasValue || location.HDOP >= 0 && location.HDOP <= 100);
        Assert.IsTrue(!location.Odometer.HasValue || location.Odometer >= 0);
        Assert.IsNotNull(location.Device);
        Assert.IsNotEmpty(location.Device.IMEI);
    }
}