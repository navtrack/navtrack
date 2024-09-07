using System;
using Navtrack.Api.Model.Stats;

namespace Navtrack.Api.Services.Stats;

public static class AssetStatsDateRangeMapper
{
    public static DateTime GetMidDate(AssetStatsDateRange dateRange)
    {
        DateTime now = DateTime.UtcNow;

        return dateRange switch
        {
            AssetStatsDateRange.ThisWeek => new DateTime(now.Year, now.Month, now.Day)
                .AddDays(-((int)now.DayOfWeek - 1)),
            AssetStatsDateRange.ThisMonth => new DateTime(now.Year, now.Month, 1),
            AssetStatsDateRange.ThisYear => new DateTime(now.Year, 1, 1),
            _ => new DateTime(now.Year, now.Month, now.Day),
        };
    }

    public static DateTime GetFirstDate(AssetStatsDateRange dateRangeType, DateTime date)
    {
        return dateRangeType switch
        {
            AssetStatsDateRange.ThisWeek => date.AddDays(-7),
            AssetStatsDateRange.ThisMonth => date.AddMonths(-1),
            AssetStatsDateRange.ThisYear => date.AddYears(-1),
            _ => date.AddDays(-1)
        };
    }

    public static DateTime GetEndDate(AssetStatsDateRange dateRange, DateTime startDate)
    {
        return dateRange switch
        {
            AssetStatsDateRange.ThisWeek => startDate.AddDays(7),
            AssetStatsDateRange.ThisMonth => startDate.AddMonths(1),
            AssetStatsDateRange.ThisYear => startDate.AddYears(1),
            _ => startDate.AddDays(1),
        };
    }
}