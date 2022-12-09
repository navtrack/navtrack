using Navtrack.Listener.Protocols.VSun;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.VSun;

public class VSunProtocolTests : BaseProtocolTests<VSunProtocol, VSunMessageHandler>
{
    [Test]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice("#860425040088567#MT600+#0000#0#1#129#40#0#AUTOLOW#1\r\n" +
                                            "#000321901$GPRMC,172030.00,A,4845.2906,N,01910.2742,E,0.01,,041219,,,A*43\r\n");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV2_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice("#869260042149724#MP90_4G#0000#AUTOLOW#1\r\n" +
                                            "#02201be0000$GPRMC,001645.00,A,5333.2920,N,11334.3857,W,0.03,,250419,,,A*5E\r\n");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV3_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice("#867962040161955#MT600#0000#0#0#137#41#0#AUTO#1\r\n" +
                                            "#00019023402$GPRMC,084702.00,A,3228.6772,S,11545.9684,E,,159.80,251018,,,A*56\r\n");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV4_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice("#868323028789359#MT600#0000#AUTOLOW#1\r\n" +
                                            "#07d8cd5198$GPRMC,164934.00,A,1814.4854,N,09926.0566,E,0.03,,240417,,,A*4A\r\n");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV5_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice("#863835026938048#MT500#0000#AUTO#1\r\n" +
                                            "#67904917c0e$GPRMC,173926.00,A,4247.8476,N,08342.6996,W,0.03,,160417,,,A*59\r\n");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV6_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice("#357671030108689##0000#AUTO#1\r\n" +
                                            "#13AE2F8F$GPRMC,211452.000,A,0017.378794,S,03603.441981,E,0.000,0,060216,,,A*68\r\n");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV7_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice("#357671030946351#V500#0000#AUTO#1\r\n" +
                                            "#$GPRMC,223835.000,A,0615.3545,S,10708.5779,E,14.62,97.41,070313,,,D*70\r\n");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV8_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice("\r\n#357671030946351#V500#0000#AUTO#1\r\n" +
                                            "#$GPRMC,223835.000,A,0615.3545,S,10708.5779,E,14.62,97.41,070313,,,D*70\r\n");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV9_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice("#357671030938911#V500#0000#AUTOSTOP#1\r\n" +
                                            "#00b34d3c$GPRMC,140026.000,A,2623.6452,S,02828.8990,E,0.00,65.44,130213,,,A*4B\r\n");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV10_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice("#357671031289215#V600#0000#AUTOLOW#1\r\n" +
                                            "#00735e1c$GPRMC,115647.000,A,5553.6524,N,02632.3128,E,0.00,0.0,130614,0.0,W,A*28");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSends2Locations_2LocationAreParsed()
    {
        ProtocolTester.SendStringFromDevice("#353686009063310#353686009063310#0000#AUTO#2\r\n" +
                                            "#239757a9$GPRMC,150252.001,A,2326.6856,S,4631.8154,W,,,260513,,,A*52\r\n" +
                                            "#239757a9$GPRMC,150322.001,A,2326.6854,S,4631.8157,W,,,260513,,,A*55");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSends3Locations_3LocationAreParsed()
    {
        ProtocolTester.SendStringFromDevice("#123456789000001#V3338#0000#SMS#3\r\n" +
                                            "#25ee0dff$GPRMC,083945.180,A,2233.4249,N,11406.0046,E,0.00,315.00,251207,,,A*6E\r\n" +
                                            "#25ee0dff$GPRMC,083950.180,A,2233.4249,N,11406.0046,E,0.00,315.00,251207,,,A*6E\r\n" +
                                            "#25ee0dff$GPRMC,083955.180,A,2233.4249,N,11406.0046,E,0.00,315.00,251207,,,A*6E");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}