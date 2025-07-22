using System;

namespace Navtrack.Api.Model.Reports;

public class BaseReportFilter
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}