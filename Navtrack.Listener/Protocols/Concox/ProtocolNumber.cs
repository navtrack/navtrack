namespace Navtrack.Listener.Protocols.Concox
{
    public enum ProtocolNumber
    {
        LoginInformation = 0x01,
        PositioningData = 0x12,
        StatusInformation = 0x13,
        AlarmData = 0x16,
        DemandInformation = 0x80,
        InformationTransmission = 0x94
    }
}