using System;
using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Reports;

public class BaseReportFilterModel
{
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
}