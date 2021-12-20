namespace Navtrack.Listener.Protocols.Meiligao;

public enum MeiligaoCommands
{
    Login = 0x5000,
    LoginConfirmation = 0x4000,
    TrackOnDemand = 0x4101,
    TrackByInterval = 0x4102,
    Authorization = 0x5000,
    SpeedingAlarm = 0x4105,
    MovementAlarm = 0x5000,
    ExtendedSettings = 0x4108,
    Initialization = 0x4108,
    SleepMode = 0x4113,
    OutputControlConditional1 = 0x4114, // applied if speed < 10km/h
    OutputControlConditional2 = 0x5114, // applied if speed < 20km/h
    OutputControlImmediate = 0x4115,
    TriggeredAlarms = 0x4116,
    PowerDown = 0x4126,
    ListenIn = 0x4130,
    LogByInterval = 0x4131,
    TimeZone = 0x4132,
    SetSensitivityOfTrembleSensor = 0x4135,
    HeadingChangeReport = 0x4136,
    SetGPSAntennaCutAlarm = 0x4150, // only for VT400
    SetGPRSParameters = 0x4155,
    SetGeoFenceAlarm = 0x4302,
    TrackByDistance = 0x4303,
    DeleteMileage = 0x4351,
    RebootGPS = 0x4902,
    Heartbeat = 0x5199,
    ClearMessageQueue = 0x5503,
    GetSerialNumberAndIMEI = 0x9001,
    ReadInterval = 0x9002,
    ReadAuthorization = 0x9003,
    ReadLoggedData = 0x9016,
    Alarms = 0x9999
}