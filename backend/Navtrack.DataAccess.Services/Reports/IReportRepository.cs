using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Navtrack.DataAccess.Services.Reports;

public interface IReportRepository
{
    Task<List<DistanceReportItem>> GetDistanceReportItems(ObjectId assetId, DateTime startDate, DateTime endDate);
    Task<List<FuelConsumptionReportItem>> GetFuelConsumptionReportItems(ObjectId assetId, DateTime startDate, DateTime endDate);
}