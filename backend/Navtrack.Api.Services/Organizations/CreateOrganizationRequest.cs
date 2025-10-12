using System;
using Navtrack.Api.Model.Organizations;

namespace Navtrack.Api.Services.Organizations;

public class CreateOrganizationRequest
{
    public CreateOrganizationModel Model { get; set; }
    public Guid OwnerId { get; set; }
}