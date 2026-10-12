using System.Collections.Generic;

namespace Navtrack.Api.Model.Organizations;

public class UpdateOrganizationModel
{
    public string? Name { get; set; }
    public List<WorkScheduleDayModel>? WorkSchedules { get; set; }
}