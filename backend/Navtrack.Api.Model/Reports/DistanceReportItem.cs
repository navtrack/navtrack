using System;
using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Reports;

public class DistanceReportItem
{
    [Required]
    public DateTime Date { get; set; }

    [Required]
    public double Distance { get; set; }

    [Required]
    public double Duration { get; set; }
    
    [Required]
    public double AverageSpeed { get; set; }
    
    [Required]
    public double MaxSpeed { get; set; }
}