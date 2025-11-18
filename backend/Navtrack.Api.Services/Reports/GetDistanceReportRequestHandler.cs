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

[Service(typeof(IRequestHandler<GetDistanceReportRequest, DistanceReportModel>))]
public class GetDistanceReportRequestHandler(
    IAssetRepository assetRepository,
    IDeviceMessageRepository deviceMessageRepository,
    IReportRepository reportRepository)
    : BaseRequestHandler<GetDistanceReportRequest, DistanceReportModel>
{
    private AssetEntity? asset;

    public override async Task Validate(RequestValidationContext<GetDistanceReportRequest> context)
    {
        asset = await assetRepository.GetById(context.Request.AssetId);
        asset.Return404IfNull();
    }

    public override async Task<DistanceReportModel> Handle(GetDistanceReportRequest request)
    {
        List<DistanceReportItem> distanceReportItems =
            await reportRepository.GetDistanceReportItems(asset!.Id, request.Model.StartDate, request.Model.EndDate);

        DistanceReportModel result = new()
        {
            Items = distanceReportItems.Select(x => new DistanceReportItemModel
            {
                Date = x.Date,
                AverageSpeed = x.AverageSpeed ?? 0,
                Distance = x.Distance ?? 0,
                Duration = x.Duration ?? 0,
                MaxSpeed = x.MaxSpeed ?? 0
            })
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