using System;
using Navtrack.Api.Model.Stats;
using Navtrack.Database.Services.Devices;

namespace Navtrack.Api.Services.Stats.Mappers;

public static class AssetStatItemMapper
{
    public static AssetStatsModel Map(int? initialOdometer, GetFirstAndLastPositionResult current,
        GetFirstAndLastPositionResult previous)
    {
        AssetStatsModel model = new();

        model.Distance = (int?)ComputeDifference(current.FirstOdometer, current.LastOdometer, initialOdometer);
        model.DistancePrevious =
            (int?)ComputeDifference(previous.FirstOdometer, previous.LastOdometer, initialOdometer);
        model.DistanceChange = ComputeChange(model.DistancePrevious, model.Distance);

        model.Duration = (int?)ComputeDifference(current.FirstIgnitionDuration, current.LastIgnitionDuration);
        model.DurationPrevious = (int?)ComputeDifference(previous.FirstIgnitionDuration, previous.LastIgnitionDuration);
        model.DurationChange = ComputeChange(model.DurationPrevious, model.Duration);

        model.FuelConsumption = ComputeDifference(current.FirstFuelConsumption, current.LastFuelConsumption);
        model.FuelConsumptionPrevious = ComputeDifference(previous.FirstFuelConsumption, previous.LastFuelConsumption);
        model.FuelConsumptionChange = ComputeChange(model.FuelConsumptionPrevious, model.FuelConsumption);

        // Fuel consumption average in l/100km 
        // 100 km * 1000 m * total fuel consumption / distance in meters
        model.FuelConsumptionAverage = 100 * 1000 * model.FuelConsumption / model.Distance;
        model.FuelConsumptionAveragePrevious = 100 * 1000 * model.FuelConsumptionPrevious / model.DistancePrevious;
        model.FuelConsumptionAverageChange =
            ComputeChange(model.FuelConsumptionAveragePrevious, model.FuelConsumptionAverage);

        return model;
    }

    private static int? ComputeChange(double? previous, double? current)
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