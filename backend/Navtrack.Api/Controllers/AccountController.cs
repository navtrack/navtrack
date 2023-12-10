using Navtrack.Api.Services.User;
using Navtrack.Api.Shared.Controllers;

namespace Navtrack.Api.Controllers;

public class AccountController(IAccountService accountService, IUserAccessService userAccessService)
    : AccountControllerBase(accountService, userAccessService);