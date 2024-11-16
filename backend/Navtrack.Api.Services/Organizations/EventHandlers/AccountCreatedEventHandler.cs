using System.Threading.Tasks;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Organizations;
using Navtrack.Api.Services.Account.Events;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;
using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Organizations.EventHandlers;

/// <summary>
/// Creates the default organization for the user.
/// </summary>
[Service(typeof(IEventHandler<AccountCreatedEvent>))]
public class AccountCreatedEventHandler(IUserRepository userRepository, IRequestHandler requestHandler)
    : IEventHandler<AccountCreatedEvent>
{
    public async Task Handle(AccountCreatedEvent payload)
    {
        UserDocument? user = await userRepository.GetById(payload.UserId);

        if (user != null)
        {
            await requestHandler.Handle<CreateOrganizationRequest, Entity>(new CreateOrganizationRequest
            {
                OwnerId = user.Id,
                Model = new CreateOrganization
                {
                    Name = "My Organization",
                }
            });
        }
    }
}