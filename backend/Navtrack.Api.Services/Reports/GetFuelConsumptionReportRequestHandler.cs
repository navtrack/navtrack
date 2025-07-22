using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Reports;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.DataAccess.Services.Devices;
using Navtrack.DataAccess.Services.Reports;
using Navtrack.Shared.Library.DI;
using FuelConsumptionReportItem = Navtrack.Api.Model.Reports.FuelConsumptionReportItem;

namespace Navtrack.Api.Services.Reports;

[Service(typeof(IRequestHandler<GetFuelConsumptionReportRequest, FuelConsumptionReport>))]
public class GetFuelConsumptionReportRequestHandler(
    IAssetRepository assetRepository,
    IDeviceMessageRepository deviceMessageRepository,
    IReportRepository reportRepository)
    : BaseRequestHandler<GetFuelConsumptionReportRequest, FuelConsumptionReport>
{
    private AssetDocument? asset;

    public override async Task Validate(RequestValidationContext<GetFuelConsumptionReportRequest> context)
    {
        asset = await assetRepository.GetById(context.Request.AssetId);
        asset.Return404IfNull();
    }

    public override async Task<FuelConsumptionReport> Handle(GetFuelConsumptionReportRequest request)
    {
        List<DataAccess.Services.Reports.FuelConsumptionReportItem> res = 
            await reportRepository.GetFuelConsumptionReportItems(asset!.Id, request.Model.StartDate,
            request.Model.EndDate);

        FuelConsumptionReport result = new()
        {
            Items = res.Select(x => new FuelConsumptionReportItem
            {
                Date = x.Date,
                Distance = x.Distance,
                Duration = x.Duration,
                FuelConsumption = x.FuelConsumption,
                AverageSpeed = x.AverageSpeed
            }).ToList()
        };

        return result;
    }


    private static int? ComputeDifference(int? start, int? end)
    {
        if (start.HasValue && end.HasValue)
        {
            return ComputeDifference(start.Value, end.Value);
        }

        return null;
    }

    private static int ComputeDifference(int start, int end)
    {
        return Math.Max(end - start, 0);
    }

    private static double? ComputeDifference(double? start, double? end)
    {
        if (start.HasValue && end.HasValue)
        {
            return ComputeDifference(start.Value, end.Value);
        }

        return null;
    }

    private static double ComputeDifference(double start, double end)
    {
        return Math.Max(end - start, 0);
    }

    private static List<DateTime> GetDatesInRange(DateTime start, DateTime end)
    {
        List<DateTime> dates = [];

        for (DateTime date = start.Date; date <= end.Date; date = date.AddDays(1))
        {
            dates.Add(date);
        }

        return dates;
    }
}