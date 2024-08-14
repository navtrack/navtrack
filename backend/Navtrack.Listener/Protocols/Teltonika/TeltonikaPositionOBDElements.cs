namespace Navtrack.Listener.Protocols.Teltonika;

internal class TeltonikaPositionOBDElements
{
    public byte? NumberOfDTC { get; set; }

    /// <summary>
    /// Calculated engine load value (%)
    /// </summary>
    public byte? EngineLoad { get; set; }

    /// <summary>
    /// Engine coolant temperature (°C)
    /// </summary>
    public sbyte? CoolantTemperature { get; set; }

    /// <summary>
    /// Short term fuel trim 1 (%)
    /// </summary>
    public sbyte? ShortFuelTrim { get; set; }

    /// <summary>
    /// Fuel pressure (kPa)
    /// </summary>
    public ulong? FuelPressure { get; set; }

    /// <summary>
    /// Intake manifold absolute pressure (kPa)
    /// </summary>
    public byte? IntakeMAP { get; set; }

    /// <summary>
    /// Engine RPM (rpm)
    /// </summary>
    public ushort? EngineRPM { get; set; }

    public byte? VehicleSpeed { get; set; }
    public sbyte? TimingAdvance { get; set; }
    public sbyte? IntakeAirTemperature { get; set; }
    public ushort? MAF { get; set; }
    public byte? ThrottlePosition { get; set; }
    public ushort? RuntimeSinceEngineStart { get; set; }
    public ushort? DistanceTraveledMILOn { get; set; }
    public ushort? RelativeFuelRailPressure { get; set; }
    public ushort? DirectFuelRailPressure { get; set; }
    public byte? CommandedEGR { get; set; }
    public sbyte? EGRError { get; set; }
    public byte? FuelLevel { get; set; }
    public ushort? DistanceSinceCodesClear { get; set; }
    public byte? BarometicPressure { get; set; }
    public ushort? ControlModuleVoltage { get; set; }
    public ushort? AbsoluteLoadValue { get; set; }
    public sbyte? AmbientAirTemperature { get; set; }
    public ushort? TimeRunWithMILOn { get; set; }
    public ushort? TimeSinceCodesCleared { get; set; }
    public ushort? AbsoluteFuelRailPressure { get; set; }
    public byte? HybridBatteryPackLife { get; set; }
    public byte? EngineOilTemperature { get; set; }
    public short? FuelInjectionTiming { get; set; }
    public ushort? FuelRate { get; set; }
    public string? VIN { get; set; }
    public string? FaultCodes { get; set; }
    public byte? ThrottlePositionGroup { get; set; }
    public byte? CommandedEquivalenceRatio { get; set; }
    public ushort? IntakeMAP2Bytes { get; set; }
    public ushort? HybridSystemVoltage { get; set; }
    public short? HybridSystemCurrent { get; set; }

    /// <summary>
    /// 0 Not available
    /// 1 Gasoline
    /// 2 Methanol
    /// 3 Ethanol
    /// 4 Diesel
    /// 5 LPG
    /// 6 CNG
    /// 7 Propane
    /// 8 Electric
    /// 9 Bifuel running Gasoline
    /// 10 Bifuel running Methanol
    /// 11 Bifuel running Ethanol
    /// 12 Bifuel running LPG
    /// 13 Bifuel running CNG
    /// 14 Bifuel running Propane
    /// 15 Bifuel running Electricity
    /// 16 Bifuel running electric and combustion engine
    /// 17 Hybrid gasoline
    /// 18 Hybrid Ethanol
    /// 19 Hybrid Diesel
    /// 20 Hybrid Electric
    /// 21 Hybrid running electric and combustion engine
    /// 22 Hybrid Regenerative
    /// 23 Bifuel running diesel
    /// </summary>
    public byte? FuelType { get; set; }
}