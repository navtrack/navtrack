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

[Service(typeof(IAccountService))]
public class AccountService(IPasswordHasher hasher, IUserRepository repository)
    : IAccountService
{
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