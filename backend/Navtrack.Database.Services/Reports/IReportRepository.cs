using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Navtrack.Database.Services.Reports;

public interface IReportRepository
{
    Task<List<DistanceReportItem>> GetDistanceReportItems(Guid assetId, DateTime start, DateTime end);
    Task<List<FuelConsumptionReportItem>> GetFuelConsumptionReportItems(Guid assetId, DateTime start,
        DateTime end);
}