using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Model.User;
using Navtrack.Api.Services.Exceptions;
using Navtrack.Api.Services.Mappers;
using Navtrack.Common.Passwords;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.User;

[Service(typeof(IUserService))]
public class UserService : IUserService
{
    private readonly IPasswordHasher passwordHasher;
    private readonly ICurrentUserAccessor currentUserAccessor;
    private readonly IUserDataService userDataService;

    public UserService(IPasswordHasher passwordHasher, ICurrentUserAccessor currentUserAccessor,
        IUserDataService userDataService)
    {
        this.passwordHasher = passwordHasher;
        this.currentUserAccessor = currentUserAccessor;
        this.userDataService = userDataService;
    }

    public async Task<CurrentUserModel> GetCurrentUser()
    {
        UserDocument entity = await currentUserAccessor.GetCurrentUser();

        return CurrentUserMapper.Map(entity);
    }

    public async Task UpdateUser(UpdateUserRequest model)
    {
        UserDocument currentUser = await currentUserAccessor.GetCurrentUser();

        await userDataService.UpdateUser(currentUser, model.Email, model.UnitsType);
    }

    public async Task Register(RegisterAccountRequest model)
    {
        ApiException apiException = new();

        if (await userDataService.EmailExists(model.Email))
        {
            apiException.AddValidationError(nameof(model.Email), ValidationErrorCodes.EmailAlreadyExists);
        }

        if (model.Password != model.ConfirmPassword)
        {
            apiException.AddValidationError(nameof(model.ConfirmPassword), ValidationErrorCodes.PasswordsDoNotMatch);
        }

        apiException.ThrowIfInvalid();

        (string hash, string salt) = passwordHasher.Hash(model.Password);

        UserDocument userDocument = UserDocumentMapper.Map(model.Email, hash, salt);

        await userDataService.Add(userDocument);
    }
}