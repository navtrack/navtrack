namespace Navtrack.Listener.Protocols.Teltonika;

public static class TeltonikaDataIds
{
    public const int EventId = 0;
    
    // Permanent I/O elements
    public const short DigitalInput1 = 1;
    public const short ICCID1 = 11;
    public const short ICCID2 = 14;
    public const short EcoScore = 15;
    public const short TotalOdometer = 16;
    public const short GsmSignal = 21;
    public const short GNSSSpeed = 24;
    public const short ExternalVoltage = 66;
    public const short BatteryVoltage = 67;
    public const short BatteryCurrent = 68;
    public const short GNSSStatus = 69;
    public const short BatteryLevel = 113;
    public const short GNSSPDOP = 181;
    public const short GNSSHDOP = 182;
    public const short TripOdometer = 199;
    public const short SleepMode = 200;
    public const short GsmCellId = 205;
    public const short GsmAreaCode = 206;
    public const short Ignition = 239;
    public const short Movement = 240;
    public const short ActiveGSMOperator = 241;
    public const short UMTSLTECellID = 636;

    // Event I/O elements
    public const short EventTrip = 250;

    // OBD elements
    public const short ObdDtcCount = 30;
    public const short ObdEngineLoad = 31;
    public const short ObdCoolantTemperature = 32;
    public const short ObdShortTermFuelTrim = 33;
    public const short ObdFuelPressure = 34;
    public const short ObdIntakeManifoldAbsolutPressure = 35;
    public const short ObdEngineRpm = 36;
    public const short ObdSpeed = 37;
    public const short ObdTimingAdvance = 38;
    public const short ObdIntakeAirTemperature = 39;
    public const short ObdMAFAirFlowRate = 40;
    public const short ObdThrottlePosition = 41;
    public const short ObdRuntimeSinceEngineStart = 42;
    public const short ObdDistanceTraveledMILOn = 43;
    public const short ObdRelativeFuelRailPressure = 44;
    public const short ObdDirectFuelRailPressure = 45;
    public const short ObdCommandedEGR = 46;
    public const short ObdEGRError = 47;
    public const short ObdFuelLevel = 48;
    public const short ObdDistanceSinceCodesClear = 49;
    public const short ObdBarometicPressure = 50;
    public const short ObdControlModuleVoltage = 51;
    public const short ObdAbsoluteLoadValue = 52;
    public const short ObdAmbientAirTemperature = 53;
    public const short ObdTimeRunWithMILOn = 54;
    public const short ObdTimeSinceCodesCleared = 55;
    public const short ObdAbsoluteFuelRailPressure = 56;
    public const short ObdHybridBatteryPackLife = 57;
    public const short ObdEngineOilTemperature = 58;
    public const short ObdFuelInjectionTiming = 59;
    public const short ObdFuelRate = 60;
    public const short ObdVIN = 256;
    public const short ObdFaultCodes = 281;
    public const short ObdThrottlePositionGroup = 540;
    public const short ObdCommandedEquivalenceRatio = 541;
    public const short ObdIntakeMAP2Bytes = 542;
    public const short ObdHybridSystemVoltage = 543;
    public const short ObdHybridSystemCurrent = 544;
    public const short ObdFuelType = 759;

    // OBD OEM elements
    public const short ObdOemMileage = 389;
    public const short ObdOemFuelLevel = 390;
    public const short ObdOemDistanceUntilService = 402;
    public const short ObdOemBatteryChargeState = 410;
    public const short ObdOemBatteryLevel = 411;
    public const short ObdOemBatteryPowerConsumption = 412;
    public const short ObdOemRemainingDistance = 755;
    public const short ObdOemBatteryStateOfHealth = 1151;
    public const short ObdOemBatteryTemperature = 1152;

    // CAN adapters
    public const short CanDoorStatus = 90;
    public const short CanProgramNumber = 100;
    public const short CanControlStateFlags = 123;
    public const short CanAgriculturalMachineryFlags = 124;
    public const short CanSecurityStateFlags = 132;
    public const short CanCNGStatus = 232;
    public const short CanSecurityStateFlagsP4 = 517;
}