using System;
using Navtrack.Api.Model.Stats;
using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Api.Services.Stats;

public static class AssetStatsItemModelMapper
{
    public static AssetStatsItemModel Map(AssetStatsDateRange dateRange,
        (DeviceMessageDocument? first, DeviceMessageDocument? last) current,
        (DeviceMessageDocument? first, DeviceMessageDocument? last) previous)
    {
        AssetStatsItemModel model = new()
        {
            DateRange = dateRange
        };

        model.Distance = ComputeDifference(current.first?.Device?.Odometer,
            current.last?.Device?.Odometer);
        model.DistancePrevious = ComputeDifference(previous.first?.Device?.Odometer,
            previous.last?.Device?.Odometer);
        model.DistanceChange = ComputeChange(model.DistancePrevious, model.Distance);
        
        model.Duration = ComputeDifference(current.first?.Vehicle?.IgnitionDuration,
            current.last?.Vehicle?.IgnitionDuration) / 60;
        model.DurationPrevious = ComputeDifference(previous.first?.Vehicle?.IgnitionDuration,
            previous.last?.Vehicle?.IgnitionDuration) / 60;
        model.DurationChange = ComputeChange(model.DurationPrevious, model.Duration);

        model.FuelConsumptionChange = ComputeChange(model.FuelConsumptionPrevious, model.FuelConsumption);

        return model;
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

    private static int? ComputeDifference(int? first, int? last)
    {
        return last != null ? Math.Max(0, last.Value - (first ?? 0)) : null;
    }
}