using Navtrack.Api.Model.Account;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Common.Exceptions;

namespace Navtrack.Api.Services.Account.Mappers;

public static class BasePasswordModelValidator
{
    public static void ValidatePasswords(BasePasswordModel model, ValidationApiException apiException,
        string? currentPassword = null)
    {
        if (model.Password != model.ConfirmPassword)
        {
            apiException.AddValidationError(nameof(model.ConfirmPassword), ApiErrorCodes.User_000003_PasswordsNotEqual);
        }
        else if (model.Password == currentPassword)
        {
            apiException.AddValidationError(nameof(model.Password), ApiErrorCodes.User_000009_PasswordMustBeDifferent);
        }
    }
}