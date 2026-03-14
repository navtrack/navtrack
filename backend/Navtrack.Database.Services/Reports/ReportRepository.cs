using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Postgres;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Database.Services.Reports;

[Service(typeof(IReportRepository))]
public class ReportRepository(IPostgresRepository repository) : IReportRepository
{
    public Task<List<DistanceReportItem>> GetDistanceReportItems(Guid assetId, DateTime start, DateTime end)
    {
        DateTime startDate = DateTime.SpecifyKind(start.Date, DateTimeKind.Utc);
        DateTime endDate = DateTime.SpecifyKind(end.Date.AddDays(1), DateTimeKind.Utc);

        Task<List<DistanceReportItem>> result = repository.GetQueryable<DeviceMessageEntity>()
            .Where(m => m.AssetId == assetId && m.Date >= startDate.Date && m.Date <= endDate.Date)
            .GroupBy(m => m.Date.Date)
            .Select(g => new DistanceReportItem
            {
                Date = g.Key,
                AverageSpeed = g.Average(x => x.Speed),
                MaxSpeed = g.Max(x => x.Speed),
                Distance = g.Max(x => x.DeviceOdometer) - g.Min(x => x.DeviceOdometer),
                Duration = g.Max(x => x.VehicleIgnitionDuration) - g.Min(x => x.VehicleIgnitionDuration),
                FuelConsumption = g.Max(x => x.VehicleFuelConsumption) - g.Min(x => x.VehicleFuelConsumption) ?? 0
            })
            .OrderBy(x => x.Date)
            .ToListAsync();

        return result;
    }
}