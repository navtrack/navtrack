using MongoDB.Bson;
using Navtrack.Api.Model.Organizations;

namespace Navtrack.Api.Services.Organizations;

public class CreateOrganizationRequest
{
    public CreateOrganization Model { get; set; }
    public ObjectId OwnerId { get; set; }
}