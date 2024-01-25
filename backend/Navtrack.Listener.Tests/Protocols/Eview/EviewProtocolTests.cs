using Navtrack.Listener.Protocols.Eview;
using Xunit;

namespace Navtrack.Listener.Tests.Protocols.Eview;

public class EviewProtocolTests : BaseProtocolTests<EviewProtocol, EviewMessageHandler>
{
    public EviewProtocolTests()
    {
        ProtocolTester.SendStringFromDevice("!1,867273023933661,V07S.5701.1621,100#");
    }

    [Fact]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "!D,3/7/13,6:35:30,22.645952,114.040436,0.0,225.8,1f0001,12.11,98,0,0,0;");

        Assert.NotNull(ProtocolTester.LastParsedPosition);
    }

    [Fact]
    public void DeviceSendsLocationV2_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "!D,22/2/14,13:47:51,56.899517,14.811665,0,0,b0001,179.3,97,5,16,0;");

        Assert.NotNull(ProtocolTester.LastParsedPosition);
    }

    [Fact]
    public void DeviceSendsLocationV3_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "!D,02/05/17,19:56:17,47.083542,15.482373,0,0,100001,479.3,100,4,9,0;");

        Assert.NotNull(ProtocolTester.LastParsedPosition);
    }

    [Fact]
    public void DeviceSendsLocationV4_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "!D,15/04/17,13:58:53,51.483067,-0.452548,60,180,140001,28.7,47,4,13,0;");

        Assert.NotNull(ProtocolTester.LastParsedPosition);
    }

    [Fact]
    public void DeviceSendsLocationV5_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "!D,07/04/17,05:42:26,-37.588970,145.121231,0,0,0c0001,185.2,92,7,14,1.2;");

        Assert.NotNull(ProtocolTester.LastParsedPosition);
    }

    [Fact]
    public void DeviceSendsLocationV6_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "!D,28/11/16,00:04:09,42.926067,-85.747589,124,236,140001,179.8,60,11,16,0;");

        Assert.NotNull(ProtocolTester.LastParsedPosition);
    }

    [Fact]
    public void DeviceSendsLocationV7_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "!C,30/1/16,1:1:6,31.259157,30.020910,0,0,100001,25.32,100,0.03,0.01,0;");

        Assert.NotNull(ProtocolTester.LastParsedPosition);
    }

    [Fact]
    public void DeviceSendsLocationV8_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "!A,26/10/12,00:28:41,7.770385,-72.215706,0.0,251.01,0;");

        Assert.NotNull(ProtocolTester.LastParsedPosition);
    }

    [Fact]
    public void DeviceSendsLocationV9_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "!A,01/12/10,13:25:35,22.641724,114.023666,000.1,281.6,0;");

        Assert.NotNull(ProtocolTester.LastParsedPosition);
    }

    [Fact]
    public void DeviceSendsLocationV10_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "!D,08/07/15,04:01:32,40.428257,-3.704808,0,0,170001,701.7,22,5,14,0;");

        Assert.NotNull(ProtocolTester.LastParsedPosition);
    }

    [Fact]
    public void DeviceSendsLocationV11_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "!D,08/07/15,04:55:13,40.428257,-3.704932,0,0,180001,680.0,8,8,13,0;");

        Assert.NotNull(ProtocolTester.LastParsedPosition);
    }

    [Fact]
    public void DeviceSendsLocationV12_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "!D,08/07/15,02:01:32,40.428230,-3.704950,4,170,170001,682.7,43,6,13,0;");

        Assert.NotNull(ProtocolTester.LastParsedPosition);
    }

    [Fact]
    public void DeviceSendsLocationV13_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "!D,22/2/14,13:40:58,56.899601,14.811541,0,0,1,176.0,98,5,16,0;");

        Assert.NotNull(ProtocolTester.LastParsedPosition);
    }
}