using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Api.Model.Account;
using Navtrack.Api.Services.Common.Passwords;
using Navtrack.Api.Services.Common.Settings.Models;
using Navtrack.Api.Tests.Helpers;
using Navtrack.Database.Model;
using Navtrack.Database.Model.Users;
using Newtonsoft.Json;

namespace Navtrack.Api.Tests;

public class UserTests(BaseTestFixture fixture) : BaseApiTest(fixture)
{
    [Fact]
    public async Task ResetPassword_EmailNotInDatabase_ReturnsBadRequest()
    {
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiPaths.AccountForgotPassword,
            new ForgotPasswordModel
            {
                Email = "nosuchemail@navtrack"
            });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ResetPassword_EmailIsInDatabase_ReturnsOk()
    {
        const string email = "choco@navtrack.com";

        Repository.GetQueryable<UserEntity>().Add(new UserEntity
        {
            Email = email
        });
        await Repository.GetDbContext().SaveChangesAsync();

        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiPaths.AccountForgotPassword,
            new ForgotPasswordModel
            {
                Email = email
            });

        UserPasswordResetEntity passwordResetDocument = await Repository.GetQueryable<UserPasswordResetEntity>()
            .FirstOrDefaultAsync(x => x.Email == email);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(passwordResetDocument);
    }

    [Fact]
    public async Task ResetPassword_EmailIsInDatabase_EmailIsSent()
    {
        const string email = "choco_is_ok@navtrack.com";

        Repository.GetQueryable<UserEntity>().Add(new UserEntity
        {
            Email = email
        });
        await Repository.GetDbContext().SaveChangesAsync();

        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiPaths.AccountForgotPassword,
            new ForgotPasswordModel
            {
                Email = email
            });

        UserPasswordResetEntity passwordResetDocument = await Repository.GetQueryable<UserPasswordResetEntity>()
            .FirstOrDefaultAsync(x => x.Email == email);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(passwordResetDocument);
    }

    [Fact]
    public async Task ResetPassword_Make11Requests_LastOneReturnsBadRequest()
    {
        const string email = "choco-too-many@navtrack.com";

        Repository.GetQueryable<UserEntity>().Add(new UserEntity
        {
            Email = email
        });
        await Repository.GetDbContext().SaveChangesAsync();

        List<HttpResponseMessage> responseMessages = [];

        for (int i = 0; i < 11; i++)
        {
            responseMessages.Add(await HttpClient.PostAsJsonAsync(ApiPaths.AccountForgotPassword,
                new ForgotPasswordModel
                {
                    Email = email
                }));
        }

        Assert.Equal(HttpStatusCode.BadRequest, responseMessages.Last().StatusCode);
    }

    [Fact]
    public async Task ResetPassword_LinkIsExpired_ReturnsBadRequest()
    {
        const string email = "choco@navtrack.com";

        Repository.GetQueryable<UserEntity>().Add(new UserEntity
        {
            Email = email
        });
        await Repository.GetDbContext().SaveChangesAsync();

        await HttpClient.PostAsJsonAsync(ApiPaths.AccountForgotPassword,
            new ForgotPasswordModel
            {
                Email = email
            });

        await Repository.GetQueryable<UserPasswordResetEntity>()
            .Where(x => x.Email == email)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.CreatedDate, DateTime.UtcNow.AddHours(-12)));

        UserPasswordResetEntity passwordResetDocument = await Repository.GetQueryable<UserPasswordResetEntity>()
            .FirstOrDefaultAsync(x => x.Email == email);

        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiPaths.AccountResetPassword,
            new ResetPasswordModel
            {
                Id = passwordResetDocument.Id.ToString(),
                Password = "new password",
                ConfirmPassword = "new password"
            });

        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task ResetPassword_CompleteFlow_PasswordIsChanged()
    {
        const string email = "password-test@navtrack.com";

        IPasswordHasher? passwordHasher = ServiceProvider.GetService<IPasswordHasher>();

        (string hash, string salt) = passwordHasher!.Hash("old password");

        Repository.GetQueryable<UserEntity>().Add(new UserEntity
        {
            Email = email,
            PasswordHash = hash,
            PasswordSalt = salt
        });
        await Repository.GetDbContext().SaveChangesAsync();

        await HttpClient.PostAsJsonAsync(ApiPaths.AccountForgotPassword,
            new ForgotPasswordModel
            {
                Email = email
            });
        
        UserPasswordResetEntity? passwordResetDocument = await Repository.GetQueryable<UserPasswordResetEntity>()
            .FirstOrDefaultAsync(x => x.Email == email);

        await HttpClient.PostAsJsonAsync(ApiPaths.AccountResetPassword,
            new ResetPasswordModel
            {
                Id = passwordResetDocument.Id.ToString(),
                Password = "new password",
                ConfirmPassword = "new password"
            });

        UserEntity? user = await Repository.GetQueryable<UserEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == passwordResetDocument.CreatedBy);
        passwordResetDocument = await Repository.GetQueryable<UserPasswordResetEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email);

        Assert.True(passwordHasher.CheckPassword("new password", user.PasswordHash, user.PasswordSalt));
        Assert.True(passwordResetDocument?.Invalid);
    }

    [Fact]
    public async Task ResetPassword_Make2RequestAndTryToUseFirstHash_ReturnsBadRequest()
    {
        const string email = "choco@navtrack.com";

        Repository.GetQueryable<UserEntity>().Add(new UserEntity
        {
            Email = email
        });
        await Repository.GetDbContext().SaveChangesAsync();

        await HttpClient.PostAsJsonAsync(ApiPaths.AccountForgotPassword,
            new ForgotPasswordModel
            {
                Email = email
            });
        await HttpClient.PostAsJsonAsync(ApiPaths.AccountForgotPassword,
            new ForgotPasswordModel
            {
                Email = email
            });

        UserPasswordResetEntity passwordResetDocument = await Repository
            .GetQueryable<UserPasswordResetEntity>()
            .Where(x => x.Email == email)
            .OrderBy(x => x.CreatedDate)
            .FirstOrDefaultAsync();

        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiPaths.AccountResetPassword,
            new ResetPasswordModel
            {
                Id = passwordResetDocument.Id.ToString(),
                Password = "new password",
                ConfirmPassword = "new password"
            });

        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task ResetPassword_TryToUseHash2Times_SecondResponseIsBadRequest()
    {
        const string email = "choco_twice@navtrack.com";

        IPasswordHasher? passwordHasher = ServiceProvider.GetService<IPasswordHasher>();

        (string hash, string salt) = passwordHasher!.Hash("old password");

        Repository.GetQueryable<UserEntity>().Add(new UserEntity
        {
            Email = email,
            PasswordHash = hash,
            PasswordSalt = salt
        });
        await Repository.GetDbContext().SaveChangesAsync();

        await HttpClient.PostAsJsonAsync(ApiPaths.AccountForgotPassword,
            new ForgotPasswordModel
            {
                Email = email
            });

        UserPasswordResetEntity passwordResetDocument = await Repository.GetQueryable<UserPasswordResetEntity>()
            .FirstOrDefaultAsync(x => x.Email == email);

        HttpResponseMessage firstResponse = await HttpClient.PostAsJsonAsync(ApiPaths.AccountResetPassword,
            new ResetPasswordModel
            {
                Id = passwordResetDocument.Id.ToString(),
                Password = "new password",
                ConfirmPassword = "new password"
            });

        HttpResponseMessage secondResponse = await HttpClient.PostAsJsonAsync(ApiPaths.AccountResetPassword,
            new ResetPasswordModel
            {
                Id = passwordResetDocument.Id.ToString(),
                Password = "new password",
                ConfirmPassword = "new password"
            });

        Assert.Equal(HttpStatusCode.OK, firstResponse.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, secondResponse.StatusCode);
    }

    protected override void SeedDatabase()
    {
        Repository.GetQueryable<SystemSettingEntity>().Add(new SystemSettingEntity
        {
            Key = "App",
            Value = JsonConvert.SerializeObject(new AppSettings { AppUrl = "http://test.navtrack.com" })
        });
        Repository.GetDbContext().SaveChanges();
    }
}