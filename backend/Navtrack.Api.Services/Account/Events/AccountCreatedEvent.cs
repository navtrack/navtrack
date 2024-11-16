using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Account.Events;

public class AccountCreatedEvent : IEvent
{
    public string UserId { get; set; }
}