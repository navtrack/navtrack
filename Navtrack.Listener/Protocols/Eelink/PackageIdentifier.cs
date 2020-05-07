namespace Navtrack.Listener.Protocols.Eelink
{
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
        Broadcast = 0x81
    }
}