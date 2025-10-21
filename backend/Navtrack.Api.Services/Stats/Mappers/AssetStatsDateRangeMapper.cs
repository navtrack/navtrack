using System;
using Navtrack.Api.Model.Stats;
using Navtrack.Shared.Library.Utils;

namespace Navtrack.Api.Services.Stats.Mappers;

public static class AssetStatsDateRangeMapper
{
    public static DateTime? GetMidDate(AssetStatsPeriod period)
    {
        DateTime now = DateTime.UtcNow;

        DateTime? result = period switch
        {
            AssetStatsPeriod.Day => new DateTime(now.Year, now.Month, now.Day),
            AssetStatsPeriod.Week => new DateTime(now.Year, now.Month, now.Day).AddDays(
                -now.DayOfWeek(DayOfWeek.Monday)),
            AssetStatsPeriod.Month => new DateTime(now.Year, now.Month, 1),
            AssetStatsPeriod.Year => new DateTime(now.Year, 1, 1),
            _ => null
        };

        return result;
    }

    public static DateTime? GetFirstDate(AssetStatsPeriod period, DateTime? date)
    {
        return period switch
        {
            AssetStatsPeriod.Day => date?.AddDays(-1),
            AssetStatsPeriod.Week => date?.AddDays(-7),
            AssetStatsPeriod.Month => date?.AddMonths(-1),
            AssetStatsPeriod.Year => date?.AddYears(-1),
            _ => null
        };
    }

    public static DateTime? GetEndDate(AssetStatsPeriod period, DateTime? date)
    {
        return period switch
        {
            AssetStatsPeriod.Day => date?.AddDays(1),
            AssetStatsPeriod.Week => date?.AddDays(7),
            AssetStatsPeriod.Month => date?.AddMonths(1),
            AssetStatsPeriod.Year => date?.AddYears(1),
            _ => null
        };
    }
}