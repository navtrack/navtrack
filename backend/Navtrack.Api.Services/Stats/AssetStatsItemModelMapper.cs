using Navtrack.Api.Model.Stats;
using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Api.Services.Stats;

public static class AssetStatsItemModelMapper
{
    public static AssetStatsItemModel Map(AssetStatsDateRange dateRange,
        (DeviceMessageDocument? first, DeviceMessageDocument? last) currentPositions,
        (DeviceMessageDocument? first, DeviceMessageDocument? last) previousPositions)
    {
        AssetStatsItemModel model = new()
        {
            DateRange = dateRange
        };

        MapCurrentStats(currentPositions, model);
        MapPreviousStats(previousPositions, model);
        MapChanges(model);

        return model;
    }

    private static void MapChanges(AssetStatsItemModel destination)
    {
        destination.DistanceChange = ComputeChange(destination.DistancePrevious, destination.Distance);
        destination.DurationChange = ComputeChange(destination.DurationPrevious, destination.Duration);
        destination.FuelConsumptionChange = ComputeChange(destination.FuelConsumptionPrevious, destination.FuelConsumption);
    }
    
    private static int? ComputeChange(int? previous, int? current)
    {
        if (previous != null && current != null)
        {
            return previous.Value != 0 ? (int)((current.Value - previous.Value) / (double)previous.Value * 100) : 100;
        }

        if (previous == null && current != null)
        {
            return 100;
        }

        if (previous != null && current == null)
        {
            return -100;
        }

        return null;
    }

    private static void MapPreviousStats((DeviceMessageDocument? first, DeviceMessageDocument? last) positions,
        AssetStatsItemModel destination)
    {
        if (positions.first?.Device?.Odometer != null && positions.last?.Device?.Odometer != null &&
            positions.last.Device.Odometer.Value > positions.first.Device.Odometer.Value)
        {
            destination.DistancePrevious = positions.last.Device.Odometer.Value - positions.first.Device.Odometer.Value;
        }

        if (positions.first?.Vehicle?.IgnitionDuration != null &&
            positions.last?.Vehicle?.IgnitionDuration != null &&
            positions.last.Vehicle.IgnitionDuration.Value >
            positions.first.Vehicle.IgnitionDuration.Value)
        {
            destination.DurationPrevious = (positions.last.Vehicle.IgnitionDuration.Value -
                                            positions.first.Vehicle.IgnitionDuration.Value) / 60;
        }
    }

    private static void MapCurrentStats((DeviceMessageDocument? first, DeviceMessageDocument? last) positions,
        AssetStatsItemModel destination)
    {
        if (positions.first?.Device?.Odometer != null && positions.last?.Device?.Odometer != null &&
            positions.last.Device.Odometer.Value > positions.first.Device.Odometer.Value)
        {
            destination.Distance = positions.last.Device.Odometer.Value - positions.first.Device.Odometer.Value;
        }

        if (positions.first?.Vehicle?.IgnitionDuration != null &&
            positions.last?.Vehicle?.IgnitionDuration != null &&
            positions.last.Vehicle.IgnitionDuration.Value >
            positions.first.Vehicle.IgnitionDuration.Value)
        {
            destination.Duration = (positions.last.Vehicle.IgnitionDuration.Value -
                                    positions.first.Vehicle.IgnitionDuration.Value) / 60;
        }
    }
}