using System;
using Navtrack.Api.Model.Stats;
using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Api.Services.Stats;

public static class AssetStatsItemModelMapper
{
    public static AssetStatsItemModel Map(AssetStatsDateRange dateRange,
        DeviceMessageDocument? initial,
        (DeviceMessageDocument? first, DeviceMessageDocument? last) current,
        (DeviceMessageDocument? first, DeviceMessageDocument? last) previous)
    {
        AssetStatsItemModel model = new()
        {
            DateRange = dateRange
        };

        model.Distance = (int?)ComputeDifference(current.first?.Device?.Odometer,
            current.last?.Device?.Odometer, initial?.Device?.Odometer);
        model.DistancePrevious = (int?)ComputeDifference(previous.first?.Device?.Odometer,
            previous.last?.Device?.Odometer, initial?.Device?.Odometer);
        model.DistanceChange = (int?)ComputeChange(model.DistancePrevious, model.Distance);

        model.Duration = (int?)(ComputeDifference(current.first?.Vehicle?.IgnitionDuration,
            current.last?.Vehicle?.IgnitionDuration) / 60);
        model.DurationPrevious = (int?)(ComputeDifference(previous.first?.Vehicle?.IgnitionDuration,
            previous.last?.Vehicle?.IgnitionDuration) / 60);
        model.DurationChange = (int?)ComputeChange(model.DurationPrevious, model.Duration);
        
        model.FuelConsumption = ComputeDifference(current.first?.Vehicle?.FuelConsumed,
            current.last?.Vehicle?.FuelConsumed);
        model.FuelConsumptionPrevious = ComputeDifference(previous.first?.Vehicle?.FuelConsumed,
            previous.last?.Vehicle?.FuelConsumed);
        model.FuelConsumptionChange = (int?)ComputeChange(model.FuelConsumptionPrevious, model.FuelConsumption);

        return model;
    }

    private static double? ComputeChange(double? previous, double? current)
    {
        if (previous != null && current != null)
        {
            return previous.Value != 0
                ? (int)((current.Value - previous.Value) / previous.Value * 100)
                : 100;
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

    private static double? ComputeDifference(double? from, double? to, int? initial = null)
    {
        if (to != null)
        {
            double diff = to.Value - (from ?? 0);
            
            if (initial.HasValue && !from.HasValue && to.Value > initial)
            {
                diff -= initial.Value;
            }

            return Math.Max(0, diff);
        }

        return null;
    }
}