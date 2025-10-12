using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Navtrack.Database.Services.Reports;

public interface IReportRepository
{
    Task<List<DistanceReportItem>> GetDistanceReportItems(Guid assetId, DateTime startDate, DateTime endDate);
    Task<List<FuelConsumptionReportItem>> GetFuelConsumptionReportItems(Guid assetId, DateTime startDate,
        DateTime endDate);
}