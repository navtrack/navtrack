using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Api.Model.Reports;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.DataAccess.Services.Devices;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Reports;

[Service(typeof(IRequestHandler<GetDistanceReportRequest, DistanceReport>))]
public class GetDistanceReportRequestHandler(
    IAssetRepository assetRepository,
    IDeviceMessageRepository deviceMessageRepository)
    : BaseRequestHandler<GetDistanceReportRequest, DistanceReport>
{
    private AssetDocument? asset;

    public override async Task Validate(RequestValidationContext<GetDistanceReportRequest> context)
    {
        asset = await assetRepository.GetById(context.Request.AssetId);
        asset.Return404IfNull();
    }

    public override async Task<DistanceReport> Handle(GetDistanceReportRequest request)
    {
        List<DistanceReportItem> list = [];
        List<DateTime> dates = GetDatesInRange(request.Model.StartDate, request.Model.EndDate);

        foreach (DateTime date in dates)
        {
            GetFirstAndLastPositionResult firstAndLast =
                await deviceMessageRepository.GetFirstAndLast(asset!.Id, date);
            
            Console.WriteLine($"First {firstAndLast.FirstOdometer?.Id == firstAndLast.FirstFuelConsumption?.Id} Last {firstAndLast.LastOdometer?.Id == firstAndLast.LastFuelConsumption?.Id} ");

            DistanceReportItem item = new()
            {
                Date = date,
                Distance = ComputeDifference(firstAndLast.FirstOdometer?.Device?.Odometer,
                    firstAndLast.LastOdometer?.Device?.Odometer),
                Duration = ComputeDifference(firstAndLast.FirstOdometer?.Vehicle?.IgnitionDuration,
                    firstAndLast.LastOdometer?.Vehicle?.IgnitionDuration),
                FuelConsumption = ComputeDifference(firstAndLast.FirstFuelConsumption?.Vehicle?.FuelConsumption,
                    firstAndLast.LastFuelConsumption?.Vehicle?.FuelConsumption)
            };
            
            if (item.Distance is < 1000)
            {
                item.Distance = null;
                item.Duration = null;
                item.FuelConsumption = null;
            }

            list.Add(item);
        }

        DistanceReport result = new()
        {
            Items = list
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