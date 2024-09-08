using System;
using Navtrack.Api.Model.Stats;
using Navtrack.Shared.Library.Utils;

namespace Navtrack.Api.Services.Stats;

public static class AssetStatsDateRangeMapper
{
    public static DateTime? GetMidDate(AssetStatsDateRange dateRange)
    {
        DateTime now = DateTime.UtcNow;

        DateTime? result = dateRange switch
        {
            AssetStatsDateRange.Today => new DateTime(now.Year, now.Month, now.Day),
            AssetStatsDateRange.ThisWeek => new DateTime(now.Year, now.Month, now.Day).AddDays(
                -now.DayOfWeek(DayOfWeek.Monday)),
            AssetStatsDateRange.ThisMonth => new DateTime(now.Year, now.Month, 1),
            AssetStatsDateRange.ThisYear => new DateTime(now.Year, 1, 1),
            _ => null
        };

        return result;
    }

    public static DateTime? GetFirstDate(AssetStatsDateRange dateRangeType, DateTime? date)
    {
        return dateRangeType switch
        {
            AssetStatsDateRange.Today => date?.AddDays(-1),
            AssetStatsDateRange.ThisWeek => date?.AddDays(-7),
            AssetStatsDateRange.ThisMonth => date?.AddMonths(-1),
            AssetStatsDateRange.ThisYear => date?.AddYears(-1),
            _ => null
        };
    }

    public static DateTime? GetEndDate(AssetStatsDateRange dateRange, DateTime? date)
    {
        return dateRange switch
        {
            AssetStatsDateRange.Today => date?.AddDays(1),
            AssetStatsDateRange.ThisWeek => date?.AddDays(7),
            AssetStatsDateRange.ThisMonth => date?.AddMonths(1),
            AssetStatsDateRange.ThisYear => date?.AddYears(1),
            _ => null
        };
    }
}