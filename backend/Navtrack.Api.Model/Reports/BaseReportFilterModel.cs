using System;

namespace Navtrack.Api.Model.Reports;

public class BaseReportFilterModel
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}