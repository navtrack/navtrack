using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Organizations;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Interceptors;
using Navtrack.Database.Model.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Organizations.EventHandlers;

/// <summary>
/// Creates the default organization for the user.
/// </summary>
[Service(typeof(IEntityCreatedHandler<UserEntity>))]
public class UserEntityCreatedHandler(IRequestHandler requestHandler) : IEntityCreatedHandler<UserEntity>
{
    public async Task Handle(UserEntity entity, DbContext context, CancellationToken cancellationToken)
    {
        await requestHandler.Handle<CreateOrganizationRequest, Entity>(new CreateOrganizationRequest
        {
            OwnerId = entity.Id,
            Model = new CreateOrganizationModel
            {
                Name = "My Organization"
            }
        });
    }
}