using Navtrack.Api.Services.Account;
using Navtrack.Api.Shared.Controllers;

namespace Navtrack.Api.Controllers;

public class AccountController(IAccountService accountService) : AccountControllerBase(accountService);