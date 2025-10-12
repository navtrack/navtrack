using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Reports;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Devices;
using Navtrack.Database.Services.Reports;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Reports;

[Service(typeof(IRequestHandler<GetFuelConsumptionReportRequest, FuelConsumptionReportModel>))]
public class GetFuelConsumptionReportRequestHandler(
    IAssetRepository assetRepository,
    IDeviceMessageRepository deviceMessageRepository,
    IReportRepository reportRepository)
    : BaseRequestHandler<GetFuelConsumptionReportRequest, FuelConsumptionReportModel>
{
    private AssetEntity? asset;

    public override async Task Validate(RequestValidationContext<GetFuelConsumptionReportRequest> context)
    {
        asset = await assetRepository.GetById(context.Request.AssetId);
        asset.Return404IfNull();
    }

    public override async Task<FuelConsumptionReportModel> Handle(GetFuelConsumptionReportRequest request)
    {
        List<FuelConsumptionReportItem> res = 
            await reportRepository.GetFuelConsumptionReportItems(asset!.Id, request.Model.StartDate,
            request.Model.EndDate);

        FuelConsumptionReportModel result = new()
        {
            Items = res.Select(x => new FuelConsumptionReportItemModel
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