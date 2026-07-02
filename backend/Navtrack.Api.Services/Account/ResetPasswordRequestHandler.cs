using System;
using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Account.Mappers;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.Passwords;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Account;

[Service(typeof(IRequestHandler<ResetPasswordRequest>))]
public class ResetPasswordRequestHandler(
    IPasswordResetRepository passwordResetRepository,
    IUserRepository userRepository,
    IPasswordHasher passwordHasher) : BaseRequestHandler<ResetPasswordRequest>
{
    public override async Task Handle(ResetPasswordRequest request)
    {
        ValidationApiException validationException = new();
        BasePasswordModelValidator.ValidatePasswords(request.Model, validationException);
        validationException.ThrowIfInvalid();

        UserPasswordResetEntity? passwordReset = await passwordResetRepository.GetLatestFromId(request.Model.Id);

        if (passwordReset == null || passwordReset.Invalid || passwordReset.Id != Guid.Parse(request.Model.Id))
        {
            throw new ApiException(ApiErrorCodes.User_InvalidPasswordResetHash);
        }

        if (passwordReset.CreatedDate < DateTime.UtcNow.AddHours(-ApiConstants.PasswordResetLinkExpirationHours))
        {
            throw new ApiException(ApiErrorCodes.User_ExpiredPasswordResetHash);
        }

        (string hash, string salt) = passwordHasher.Hash(request.Model.Password);

        await userRepository.Update(passwordReset.CreatedBy, new UpdateUser
        {
            Password = new PasswordElement
            {
                Hash = hash,
                Salt = salt
            }
        });

        await passwordResetRepository.MarkAsInvalidByUserId(passwordReset.CreatedBy);
    }
}
