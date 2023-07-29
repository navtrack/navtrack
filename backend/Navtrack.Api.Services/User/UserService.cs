using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Model.User;
using Navtrack.Api.Services.Exceptions;
using Navtrack.Api.Services.Mappers.Users;
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

    public async Task<UserModel> GetCurrentUser()
    {
        UserDocument entity = await currentUserAccessor.Get();

        return UserModelMapper.Map(entity);
    }

    public async Task Update(UpdateUserRequest model)
    {
        UserDocument currentUser = await currentUserAccessor.Get();
        UpdateUser updateUser = new();

        if (!string.IsNullOrEmpty(model.Email))
        {
            model.Email = model.Email.ToLower();

            if (currentUser.Email != model.Email)
            {
                if (await userDataService.EmailIsUsed(model.Email))
                {
                    throw new ValidationException().AddValidationError(nameof(UpdateUserRequest.Email),
                        ValidationErrorCodes.EmailAlreadyUsed);
                }

                updateUser.Email = model.Email;
            }
        }
        
        if (model.UnitsType.HasValue && currentUser.UnitsType != model.UnitsType)
        {
            updateUser.UnitsType = model.UnitsType;
        }

        await userDataService.Update(currentUser.Id, updateUser);
    }

    public async Task Register(RegisterAccountRequest model)
    {
        ApiException apiException = new();

        if (await userDataService.EmailIsUsed(model.Email))
        {
            apiException.AddValidationError(nameof(model.Email), ValidationErrorCodes.EmailAlreadyUsed);
        }

        if (model.Password != model.ConfirmPassword)
        {
            apiException.AddValidationError(nameof(model.ConfirmPassword),
                ValidationErrorCodes.PasswordsDoNotMatch);
        }

        apiException.ThrowIfInvalid();

        (string hash, string salt) = passwordHasher.Hash(model.Password);

        UserDocument userDocument = UserDocumentMapper.Map(model.Email, hash, salt);

        await userDataService.Add(userDocument);
    }
}