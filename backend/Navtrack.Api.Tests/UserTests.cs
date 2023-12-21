using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.Api.Model;
using Navtrack.Api.Model.User;
using Navtrack.Api.Tests.Helpers;
using Navtrack.DataAccess.Model.Settings;
using Navtrack.DataAccess.Model.Users;
using Navtrack.Shared.Services.Passwords;
using Navtrack.Shared.Services.Settings.Settings;

namespace Navtrack.Api.Tests;

public class UserTests(BaseTestFixture fixture) : BaseTest(fixture)
{
    [Fact]
    public async Task ResetPassword_EmailNotInDatabase_ReturnsBadRequest()
    {
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiPaths.AccountPasswordForgot,
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

        await Repository.GetCollection<UserDocument>().InsertOneAsync(new UserDocument
        {
            Email = email
        });
        
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiPaths.AccountPasswordForgot,
            new ForgotPasswordModel
            {
                Email = email
            });

        PasswordResetDocument passwordResetDocument = await Repository.GetQueryable<PasswordResetDocument>()
            .FirstOrDefaultAsync(x => x.Email == email);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(passwordResetDocument);
    }

    [Fact]
    public async Task ResetPassword_EmailIsInDatabase_EmailIsSent()
    {
        const string email = "choco_is_ok@navtrack.com";

        await Repository.GetCollection<UserDocument>().InsertOneAsync(new UserDocument
        {
            Email = email
        });
        
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiPaths.AccountPasswordForgot,
            new ForgotPasswordModel
            {
                Email = email
            });

        PasswordResetDocument passwordResetDocument = await Repository.GetQueryable<PasswordResetDocument>()
            .FirstOrDefaultAsync(x => x.Email == email);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(passwordResetDocument);
    }

    [Fact]
    public async Task ResetPassword_Make11Requests_LastOneReturnsBadRequest()
    {
        const string email = "choco-too-many@navtrack.com";

        await Repository.GetCollection<UserDocument>().InsertOneAsync(new UserDocument
        {
            Email = email
        });

        List<HttpResponseMessage> responseMessages = [];

        for (int i = 0; i < 11; i++)
        {
            responseMessages.Add(await HttpClient.PostAsJsonAsync(ApiPaths.AccountPasswordForgot,
                new ForgotPasswordModel
                {
                    Email = email
                }));
        }

        Assert.Equal(HttpStatusCode.TooManyRequests, responseMessages.Last().StatusCode);
    }
    
    [Fact]
    public async Task ResetPassword_LinkIsExpired_ReturnsBadRequest()
    {
        const string email = "choco@navtrack.com";

        await Repository.GetCollection<UserDocument>().InsertOneAsync(new UserDocument
        {
            Email = email
        });

        await HttpClient.PostAsJsonAsync(ApiPaths.AccountPasswordForgot,
            new ForgotPasswordModel
            {
                Email = email
            });
        
        await Repository.GetCollection<PasswordResetDocument>().UpdateOneAsync(x => x.Email == email,
            Builders<PasswordResetDocument>.Update.Set(x => x.Created.Date, DateTime.UtcNow.AddHours(-12)));

        PasswordResetDocument passwordResetDocument = await Repository.GetQueryable<PasswordResetDocument>()
            .FirstOrDefaultAsync(x => x.Email == email);

        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiPaths.AccountPasswordReset,
            new ResetPasswordModel
            {
                Hash = passwordResetDocument.Hash,
                Password = "new password",
                ConfirmPassword = "new password"
            });

        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task ResetPassword_CompleteFlow_PasswordIsChanged()
    {
        const string email = "choco@navtrack.com";

        IPasswordHasher? passwordHasher = ServiceProvider.GetService<IPasswordHasher>();

        (string? hash, string? salt) = passwordHasher!.Hash("old password");

        await Repository.GetCollection<UserDocument>().InsertOneAsync(new UserDocument
        {
            Email = email,
            Password = new PasswordElement
            {
                Hash = hash,
                Salt = salt
            }
        });

        await HttpClient.PostAsJsonAsync(ApiPaths.AccountPasswordForgot,
            new ForgotPasswordModel
            {
                Email = email
            });

        PasswordResetDocument passwordResetDocument = await Repository.GetQueryable<PasswordResetDocument>()
            .FirstOrDefaultAsync(x => x.Email == email);

        await HttpClient.PostAsJsonAsync(ApiPaths.AccountPasswordReset,
            new ResetPasswordModel
            {
                Hash = passwordResetDocument.Hash,
                Password = "new password",
                ConfirmPassword = "new password"
            });

        UserDocument? user = await Repository.GetQueryable<UserDocument>()
            .FirstOrDefaultAsync(x => x.Id == passwordResetDocument.UserId);
        passwordResetDocument = await Repository.GetQueryable<PasswordResetDocument>()
            .FirstOrDefaultAsync(x => x.Email == email);

        Assert.True(passwordHasher.CheckPassword("new password", user.Password.Hash, user.Password.Salt));
        Assert.True(passwordResetDocument.Invalid);
    }
    
    [Fact]
    public async Task ResetPassword_Make2RequestAndTryToUseFirstHash_ReturnsBadRequest()
    {
        const string email = "choco@navtrack.com";

        await Repository.GetCollection<UserDocument>().InsertOneAsync(new UserDocument
        {
            Email = email
        });

       await HttpClient.PostAsJsonAsync(ApiPaths.AccountPasswordForgot,
            new ForgotPasswordModel
            {
                Email = email
            });
       await HttpClient.PostAsJsonAsync(ApiPaths.AccountPasswordForgot,
           new ForgotPasswordModel
           {
               Email = email
           });

        PasswordResetDocument passwordResetDocument = await Repository
            .GetQueryable<PasswordResetDocument>()
            .Where(x => x.Email == email)
            .OrderBy(x => x.Created.Date)
            .FirstOrDefaultAsync();

        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(ApiPaths.AccountPasswordReset,
            new ResetPasswordModel
            {
                Hash = passwordResetDocument.Hash,
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

        (string? hash, string? salt) = passwordHasher!.Hash("old password");

        await Repository.GetCollection<UserDocument>().InsertOneAsync(new UserDocument
        {
            Email = email,
            Password = new PasswordElement
            {
                Hash = hash,
                Salt = salt
            }
        });

        await HttpClient.PostAsJsonAsync(ApiPaths.AccountPasswordForgot,
            new ForgotPasswordModel
            {
                Email = email
            });

        PasswordResetDocument passwordResetDocument = await Repository.GetQueryable<PasswordResetDocument>()
            .FirstOrDefaultAsync(x => x.Email == email);

        HttpResponseMessage firstResponse = await HttpClient.PostAsJsonAsync(ApiPaths.AccountPasswordReset,
            new ResetPasswordModel
            {
                Hash = passwordResetDocument.Hash,
                Password = "new password",
                ConfirmPassword = "new password"
            });

        HttpResponseMessage secondResponse = await HttpClient.PostAsJsonAsync(ApiPaths.AccountPasswordReset,
            new ResetPasswordModel
            {
                Hash = passwordResetDocument.Hash,
                Password = "new password",
                ConfirmPassword = "new password"
            });

        Assert.Equal(HttpStatusCode.OK, firstResponse.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, secondResponse.StatusCode);
    }

    protected override void SeedDatabase()
    {
        Repository.GetCollection<SettingDocument>().InsertOne(new SettingDocument
        {
            Key = "App",
            Value = new AppSettings { AppUrl = "http://test.navtrack.com" }.ToBsonDocument()
        });
    }
}