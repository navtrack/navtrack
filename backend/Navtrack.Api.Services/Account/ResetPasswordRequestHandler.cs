using System;
using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Account.Mappers;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.Passwords;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Model.Users.PasswordResets;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Account;

[Service(typeof(IRequestHandler<ResetPasswordRequest>))]
public class ResetPasswordRequestHandler(
    IPasswordResetRepository passwordResetRepository,
    IUserRepository userRepository,
    IPasswordHasher passwordHasher) : BaseRequestHandler<ResetPasswordRequest>
{
    private PasswordResetDocument? passwordReset;

    public override async Task Validate(RequestValidationContext<ResetPasswordRequest> context)
    {
        BasePasswordModelValidator.ValidatePasswords(context.Request.Model, context.ValidationException);
        context.ValidationException.ThrowIfInvalid();

        passwordReset = await passwordResetRepository.GetLatestFromHash(context.Request.Model.Hash);

        if (passwordReset == null || passwordReset.Invalid || passwordReset.Hash != context.Request.Model.Hash)
        {
            throw new ApiException(ApiErrorCodes.User_000006_InvalidPasswordResetHash);
        }

        if (passwordReset.CreatedDate < DateTime.UtcNow.AddHours(-ApiConstants.PasswordResetLinkExpirationHours))
        {
            throw new ApiException(ApiErrorCodes.User_000007_ExpiredPasswordResetHash);
        }
    }

    public override async Task Handle(ResetPasswordRequest request)
    {
        (string hash, string salt) = passwordHasher.Hash(request.Model.Password);

        await userRepository.Update(passwordReset!.CreatedBy, new UpdateUser
        {
            Password = new PasswordElement
            {
                Hash = hash,
                Salt = salt
            }
        });

        await passwordResetRepository.MarkAsInvalid(passwordReset.Id);
    }
}