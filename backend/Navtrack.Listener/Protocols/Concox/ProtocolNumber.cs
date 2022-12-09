namespace Navtrack.Listener.Protocols.Concox;

public enum ProtocolNumber
{
    LoginInformation = 0x01,
    PositioningData = 0x12,
    PositioningDataNew = 0x22,
    Heartbeat = 0x13,
    OnlineCommandResponse = 0x21,
    AlarmData = 0x16,
    AlarmData2 = 0x26,
    LBS = 0x28,
    OnlineCommand = 0x80,
    TimeCheck = 0x8A,
    InformationTransmission = 0x94
}