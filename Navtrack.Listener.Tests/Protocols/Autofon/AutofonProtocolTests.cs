using Navtrack.Listener.Protocols.Autofon;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Autofon;

public class AutofonProtocolTests : BaseProtocolTests<AutofonProtocol, AutofonMessageHandler>
{
    [Test]
    public void DeviceSendsLoginV1_DeviceSendsReply()
    {
        ProtocolTester.SendHexFromDevice("10556103592310314825728F");

        Assert.AreEqual("726573705F6372633D8F", ProtocolTester.ReceiveHexInDevice());
    }

    [Test]
    public void DeviceSendsLocationV1_LocationIsParsed_1()
    {
        // Send login
        ProtocolTester.SendHexFromDevice("10556103592310314825728F");
            
        // Send location
        ProtocolTester.SendHexFromDevice("02080000251848470AFA010262DAA690013AA4046DA83745F8812560DF010001126A");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV1_LocationIsParsed_2()
    {
        // Send login
        ProtocolTester.SendHexFromDevice("10556103592310314825728F");

        // Send location
        ProtocolTester.SendHexFromDevice(
            "111E00000000000000000100007101010B0C020302010B0C0005A053FFFFFFFF02010B0C00276047FFFFFFFF1F5600FA000176F218C7850C0B0B0C203A033DBD46035783EF009E00320014FFFF45");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLoginV2_DeviceSendsReply()
    {
        ProtocolTester.SendHexFromDevice("41035151305289931441139602662095148807");

        Assert.AreEqual("726573705F6372633D07", ProtocolTester.ReceiveHexInDevice());
    }

    [Test]
    public void DeviceSendsLocationV2_LocationIsParsed()
    {
        // Send login
        ProtocolTester.SendHexFromDevice("41035151305289931441139602662095148807");

        // Send location
        ProtocolTester.SendHexFromDevice("023E00001E004D411EFA01772F185285009C48041F1E366C2961380F26B10B00911C");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}