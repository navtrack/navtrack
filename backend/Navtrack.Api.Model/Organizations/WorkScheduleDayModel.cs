using System;
using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Organizations;

public class WorkScheduleDayModel
{
    [Required]
    public DayOfWeek DayOfWeek { get; set; }
    
    public TimeOnly StartTime { get; set; }
    
    public TimeOnly EndTime { get; set; }
}