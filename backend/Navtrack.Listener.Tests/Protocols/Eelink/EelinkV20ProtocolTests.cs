using Navtrack.Listener.Protocols.Eelink;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Eelink;

public class EelinkV20ProtocolTests : BaseProtocolTests<EelinkProtocol, EelinkMessageHandler>
{
    [Test]
    public void DeviceSendsLoginPackage_ServerAcknowledges()
    {
        ProtocolTester.SendHexFromDevice("67670100180005035254407167747100200205020500010432000088BD");

        string hex = ProtocolTester.ReceiveHexInDevice();
            
        Assert.IsTrue(hex.StartsWith("67670100090005"));
        Assert.IsTrue(hex.EndsWith("000103"));
    }
        
    [Test]
    public void DeviceSendsHeartbeatPackage_ServerAcknowledges()
    {
        ProtocolTester.SendHexFromDevice("676703000400070188");

        Assert.AreEqual("67670300020007", ProtocolTester.ReceiveHexInDevice());
    }

    [Test]
    public void DeviceSendsLocationPackage_ServerAcknowledgesAndLocationIsParsed()
    {
        // Login
        ProtocolTester.SendHexFromDevice("67670100180005035254407167747100200205020500010432000088BD");

        // Location
        ProtocolTester.SendHexFromDevice("67671200410022590BD94203026B940D0C3952AD0021000000000001CC0001A53F0170F0AB1301890F08000000000000C2D0001C001600000000000000000000000000000000");

        Assert.AreEqual("67671200020022", ProtocolTester.ReceiveHexInDevice());
        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsWarningPackage_ServerAcknowledges()
    {
        // Login
        ProtocolTester.SendHexFromDevice("67670100180005035254407167747100200205020500010432000088BD");

        // Warning
        ProtocolTester.SendHexFromDevice("6767140024000A590BD54903026B940D0C3952AD0021000400000501CC0001A53F0170F0AB19020789");

        Assert.AreEqual("6767140002000A", ProtocolTester.ReceiveHexInDevice());
        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsReportPackage_ServerAcknowledges()
    {
        // Login
        ProtocolTester.SendHexFromDevice("67670100180005035254407167747100200205020500010432000088BD");

        // Report
        ProtocolTester.SendHexFromDevice("6767150024000B590BD57103026B940D0C3952AD0021000000000501CC0001A53F0170F0AB18020789");

        Assert.AreEqual("6767150002000B", ProtocolTester.ReceiveHexInDevice());
    }

    [Test]
    public void DeviceSendsMessagePackage_ServerAcknowledges()
    {
        // Login
        ProtocolTester.SendHexFromDevice("67670100180005035254407167747100200205020500010432000088BD");

        // Message
        ProtocolTester.SendHexFromDevice("6767160039000D590BD5AF03026B940D0C3952AD0021000000000501CC0001A53F0170F0AB17323031383536363232313235300000000000000000313233");

        Assert.AreEqual("6767160017000D323031383536363232313235300000000000000000", ProtocolTester.ReceiveHexInDevice());
    }

    [Test]
    public void DeviceSendsParamSetPackage_ServerAcknowledges()
    {
        ProtocolTester.SendHexFromDevice("67671B009E000500010432009266DF000008053FC0A20341303EFE8110D414404C0680185610CEF3A23C8C18154005AB64300BD0AAA845755C0CE331CF0C1B036478B843D0EA288988320B42D068956405053C11A4588FA38803FD599EC6EF4B7383D0FC3FB7333919EA637F3D8EFB1D79F9D27B8D7782191146AE344DC0766F01599EE898BBE5ED3217444DBECA0AB4BADA4B08224A48F235D59759EDEB2A24EE9C20");

        Assert.AreEqual("67671B0003000500", ProtocolTester.ReceiveHexInDevice());
    }
}