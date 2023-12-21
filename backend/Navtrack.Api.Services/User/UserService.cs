using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Model.User;
using Navtrack.Api.Services.Exceptions;
using Navtrack.Api.Services.Mappers.Users;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;
using Navtrack.Shared.Services.Passwords;

namespace Navtrack.Api.Services.User;

[Service(typeof(IUserService))]
public class UserService(
    IPasswordHasher hasher,
    ICurrentUserAccessor userAccessor,
    IUserRepository repository)
    : IUserService
{
    public async Task<UserModel> GetCurrentUser()
    {
        UserDocument entity = await userAccessor.Get();

        return UserMapper.Map(entity);
    }

    public async Task Update(UpdateUserModel model)
    {
        UserDocument currentUser = await userAccessor.Get();
        UpdateUser updateUser = new();

        if (!string.IsNullOrEmpty(model.Email))
        {
            model.Email = model.Email.ToLower();

            if (currentUser.Email != model.Email)
            {
                if (await repository.EmailIsUsed(model.Email))
                {
                    throw new ValidationApiException().AddValidationError(nameof(UpdateUserModel.Email),
                        ValidationErrorCodes.EmailAlreadyUsed);
                }

                updateUser.Email = model.Email;
            }
        }
        
        if (model.UnitsType.HasValue && currentUser.UnitsType != model.UnitsType)
        {
            updateUser.UnitsType = model.UnitsType;
        }

        await repository.Update(currentUser.Id, updateUser);
    }

    public async Task Register(RegisterAccountModel model)
    {
        ValidationApiException apiException = new();

        if (await repository.EmailIsUsed(model.Email))
        {
            apiException.AddValidationError(nameof(model.Email), ValidationErrorCodes.EmailAlreadyUsed);
        }

        if (model.Password != model.ConfirmPassword)
        {
            apiException.AddValidationError(nameof(model.ConfirmPassword),
                ValidationErrorCodes.PasswordsDoNotMatch);
        }

        apiException.ThrowIfInvalid();

        (string hash, string salt) = hasher.Hash(model.Password);

        UserDocument userDocument = UserDocumentMapper.Map(model.Email, hash, salt);

        await repository.Add(userDocument);
    }
}