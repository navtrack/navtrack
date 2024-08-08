using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Navtrack.Api.Model.Account;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Exceptions;
using Navtrack.Api.Services.Extensions;
using Navtrack.Api.Services.Mappers.Accounts;
using Navtrack.Api.Services.Mappers.Users;
using Navtrack.Api.Services.User;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Model.Users.PasswordResets;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;
using Navtrack.Shared.Services.Email;
using Navtrack.Shared.Services.Email.Emails;
using Navtrack.Shared.Services.Passwords;
using Navtrack.Shared.Services.Settings;
using Navtrack.Shared.Services.Settings.Models;

namespace Navtrack.Api.Services.Account;

[Service(typeof(IAccountService))]
public class AccountService(
    IPasswordHasher hasher,
    ICurrentUserAccessor userAccessor,
    IHttpContextAccessor contextAccessor,
    IEmailService service,
    ISettingService settingService,
    IUserRepository repository,
    IPasswordResetRepository resetRepository,
    ICaptchaValidator? captchaValidator = null) : IAccountService
{
    public async Task Register(RegisterAccountModel model)
    {
        ValidationApiException apiException = new();

        if (await repository.EmailIsUsed(model.Email))
        {
            apiException.AddValidationError(nameof(model.Email), ApiErrorCodes.EmailAlreadyUsed);
        }

        if (model.Password != model.ConfirmPassword)
        {
            apiException.AddValidationError(nameof(model.ConfirmPassword), ApiErrorCodes.PasswordsDoNotMatch);
        }

        if (captchaValidator != null && !await captchaValidator.Validate(model.Captcha))
        {
            apiException.AddValidationError(nameof(model.Captcha), ApiErrorCodes.InvalidCaptcha);
        }

        apiException.ThrowIfInvalid();

        (string hash, string salt) = hasher.Hash(model.Password);

        UserDocument userDocument = UserDocumentMapper.Map(model.Email, hash, salt);

        await repository.Add(userDocument);
    }

    public async Task ForgotPassword(ForgotPasswordModel model)
    {
        string? ipAddress = contextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        model.Email = model.Email.Trim().ToLowerInvariant();

        ipAddress.ThrowApiExceptionIfNullOrEmpty(ApiErrorCodes.InvalidIpAddress);

        int passwordResetsIn24H =
            await resetRepository.GetCountOfPasswordResets(ipAddress!, model.Email, DateTime.UtcNow.AddDays(-1));

        if (passwordResetsIn24H >= ApiConstants.MaxPasswordResetIn24Hours)
        {
            throw new ApiException(ApiErrorCodes.MaxPasswordResetsExceeded, HttpStatusCode.TooManyRequests);
        }

        UserDocument? userDocument = await repository.GetByEmail(model.Email);

        if (userDocument == null)
        {
            throw new ValidationApiException()
                .AddValidationError(nameof(ForgotPasswordModel.Email), ApiErrorCodes.EmailDoesNotExist);
        }

        await resetRepository.MarkAsInvalidByUserId(userDocument.Id);

        PasswordResetDocument document =
            PasswordResetMapper.Map(model.Email, userDocument.Id, ipAddress!);

        await resetRepository.Add(document);

        string url = await GetResetPasswordUrl(document);

        await service.Send(document.Email, new ResetPasswordEmail(url, 3));
    }

    public async Task ChangePassword(ChangePasswordModel model)
    {
        UserDocument currentUser = await userAccessor.Get();

        currentUser.ThrowApiExceptionIfNull(HttpStatusCode.Unauthorized);

        ValidationApiException apiException = new();

        if (currentUser.Password != null && !hasher.CheckPassword(model.CurrentPassword, currentUser.Password.Hash,
                currentUser.Password.Salt))
        {
            apiException.AddValidationError(nameof(model.CurrentPassword), ApiErrorCodes.CurrentPasswordInvalid);
        }

        ValidatePasswords(model, apiException, model.CurrentPassword);

        apiException.ThrowIfInvalid();

        (string hash, string salt) = hasher.Hash(model.Password);

        await repository.Update(currentUser.Id, new UpdateUser
        {
            Password = new PasswordElement
            {
                Hash = hash,
                Salt = salt
            }
        });
    }

    public async Task ResetPassword(ResetPasswordModel model)
    {
        ValidationApiException apiException = new();

        ValidatePasswords(model, apiException);

        apiException.ThrowIfInvalid();

        PasswordResetDocument? passwordReset = await resetRepository.GetLatestFromHash(model.Hash);

        if (passwordReset == null || passwordReset.Invalid || passwordReset.Hash != model.Hash)
        {
            throw new ApiException(ApiErrorCodes.PasswordResetInvalid);
        }

        if (passwordReset.CreatedDate < DateTime.UtcNow.AddHours(-ApiConstants.PasswordResetLinkExpirationHours))
        {
            throw new ApiException(ApiErrorCodes.PasswordResetExpired);
        }

        (string hash, string salt) = hasher.Hash(model.Password);

        await repository.Update(passwordReset.CreatedBy, new UpdateUser
        {
            Password = new PasswordElement
            {
                Hash = hash,
                Salt = salt
            }
        });
        await resetRepository.MarkAsInvalid(passwordReset.Id);
    }

    private static void ValidatePasswords(BasePasswordModel model, ValidationApiException apiException,
        string? currentPassword = null)
    {
        if (model.Password != model.ConfirmPassword)
        {
            apiException.AddValidationError(nameof(model.ConfirmPassword), ApiErrorCodes.PasswordsDoNotMatch);
        }
        else if (model.Password == currentPassword)
        {
            apiException.AddValidationError(nameof(model.Password), ApiErrorCodes.PasswordMustBeNew);
        }
    }

    private async Task<string> GetResetPasswordUrl(PasswordResetDocument document)
    {
        AppSettings? appSettings = await settingService.Get<AppSettings>();

        if (!string.IsNullOrEmpty(appSettings?.AppUrl))
        {
            return ApiConstants.ResetPasswordLink(appSettings.AppUrl, document.Hash);
        }

        throw new ArgumentException("App URL not set.");
    }
}