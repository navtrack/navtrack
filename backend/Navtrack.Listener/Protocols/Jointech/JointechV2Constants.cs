namespace Navtrack.Listener.Protocols.Jointech;

public static class JointechV2Constants
{
    public const int GeneralResponseMessageId = 0x8001;
    public const int LocationInformationReportMessageId = 0x0200;
    
    public const byte ResponseSucceed = 0x0;
    public const byte ResponseFail = 0x1;
    public const byte ResponseWrongMessage = 0x2;
    public const byte ResponseUnsupported = 0x3;
    public const byte ResponseAlarmConfirmation = 0x4;
}