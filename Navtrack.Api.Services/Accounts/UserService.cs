using System.Threading.Tasks;
using Navtrack.Api.Model.Accounts;
using Navtrack.Api.Services.Exceptions;
using Navtrack.Api.Services.Mappers;
using Navtrack.Api.Services.Users;
using Navtrack.Common.Services;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.Accounts;

[Service(typeof(IAccountService))]
public class AccountService : IAccountService
{
    private readonly ICurrentUserAccessor currentUserAccessor;
    private readonly IPasswordHasher passwordHasher;
    private readonly IUserDataService userDataService;

    public AccountService(ICurrentUserAccessor currentUserAccessor, IPasswordHasher passwordHasher,
        IUserDataService userDataService)
    {
        this.currentUserAccessor = currentUserAccessor;
        this.passwordHasher = passwordHasher;
        this.userDataService = userDataService;
    }

    public async Task ChangePassword(ChangePasswordModel model)
    {
        UserDocument currentUser = await currentUserAccessor.GetCurrentUser();

        ApiException apiException = new();

        if (!passwordHasher.CheckPassword(model.CurrentPassword, currentUser.Password.Hash,
                currentUser.Password.Salt))
        {
            apiException.AddValidationError(nameof(model.CurrentPassword),
                "The current password is not valid.");
        }

        if (model.Password != model.ConfirmPassword)
        {
            apiException.AddValidationError(nameof(model.ConfirmPassword),
                "The passwords do not match.");
        }
        else if (model.Password == model.CurrentPassword)
        {
            apiException.AddValidationError(nameof(model.Password),
                "Your new password must be different than the current one.");
        }

        apiException.ThrowIfInvalid();

        (string hash, string salt) = passwordHasher.Hash(model.Password);
        await userDataService.ChangePassword(currentUser.Id, hash, salt);
    }

    public async Task Register(RegisterAccountModel model)
    {
        ApiException apiException = new();

        if (await userDataService.EmailIsUsed(model.Email))
        {
            apiException.AddValidationError(nameof(model.Email),
                "There is already an account with this email.");
        }

        if (model.Password != model.ConfirmPassword)
        {
            apiException.AddValidationError(nameof(model.ConfirmPassword),
                "The passwords do not match.");
        }

        apiException.ThrowIfInvalid();

        (string hash, string salt) = passwordHasher.Hash(model.Password);

        UserDocument userDocument = UserDocumentMapper.Map(model.Email, hash, salt);

        await userDataService.Add(userDocument);
    }
}