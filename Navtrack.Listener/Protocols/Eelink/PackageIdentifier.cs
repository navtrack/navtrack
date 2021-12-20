namespace Navtrack.Listener.Protocols.Eelink;

public enum PackageIdentifier
{
    Login = 0x01,
    Heartbeat = 0x03,
    Location = 0x12,
    Warning = 0x14,
    Report = 0x15,
    Message = 0x16,
    OBDData = 0x17,
    OBDBody = 0x18,
    OBDFault = 0x19,
    Pedometer = 0x1A,
    ParamSet = 0x1B,
    Instruction = 0x80,
    Broadcast = 0x81,
    // Protocol V1.8
    GPSOld = 0x02,
    AlarmOld = 0x04,
    TerminalStateOld = 0x05,
    SMSCommandsUploadOld = 0x06,
    OBDDataOld = 0x07,
    OBDFaultOld = 0x09,
    PhotoInformationOld = 0x0E,
    PhotoContentOld = 0x0F
}