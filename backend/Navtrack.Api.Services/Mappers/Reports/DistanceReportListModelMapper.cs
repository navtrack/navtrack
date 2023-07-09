using System;
using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Reports;
using Navtrack.Core.Model.Trips;
using Navtrack.DataAccess.Model.Common;

namespace Navtrack.Api.Services.Mappers.Reports;

public static class DistanceReportListModelMapper
{
    public static DistanceReportListModel Map(IEnumerable<Trip> source, UnitsType unitsType)
    {
        DistanceReportListModel model = new DistanceReportListModel
        {
            Items = source.GroupBy(x => new
                    { x.StartLocation.DateTime.Day, x.StartLocation.DateTime.Month, x.StartLocation.DateTime.Year })
                .Select(x => new DistanceReportItemModel
                {
                    Day = new DateTime(x.Key.Year, x.Key.Month, x.Key.Day),
                    Trips = x.Count(),
                    Distance = x.Sum(y => y.Distance),
                    Duration = x.Sum(y => y.Duration)
                })
        };

        return model;
    }
}