using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.Api.Tests.Helpers;

public class BaseTest : IClassFixture<BaseTestFixture>
{
    private readonly BaseTestFixture fixture;
    protected bool DisableAuthentication { get; set; }

    protected BaseTest(BaseTestFixture fixture)
    {
        this.fixture = fixture;
        fixture.Initialize(new BaseTestFixtureOptions
        {
            AuthenticatedUserId = !DisableAuthentication ? AuthenticatedUser.Id.ToString() : null
        });

        InitializeSeedDatabase();
    }
    
    
    private HttpClient? httpClient;
    protected HttpClient HttpClient => httpClient ??= fixture.Factory.CreateClient();

    private IServiceProvider? serviceProvider;
    protected IServiceProvider ServiceProvider =>
        serviceProvider ??= fixture.Factory.Services.CreateScope().ServiceProvider;

    protected IRepository Repository => ServiceProvider.GetService<IRepository>()!;

    private void InitializeSeedDatabase()
    {
        if (!fixture.DatabaseSeeded)
        {
            if (!DisableAuthentication)
            {
                Repository.GetCollection<UserDocument>().InsertOne(AuthenticatedUser);
            }

            Repository.GetCollection<AssetDocument>().InsertOne(Asset);
            SeedDatabase();

            fixture.DatabaseSeeded = true;
        }
    }

    protected virtual void SeedDatabase()
    {
    }

    private static readonly ObjectId AuthenticatedUserId = ObjectId.GenerateNewId();
    private static readonly ObjectId AssetId = ObjectId.GenerateNewId();

    protected readonly UserDocument AuthenticatedUser = new()
    {
        Id = AuthenticatedUserId,
        Email = "choco@navtrack.com",
        AssetRoles = new[]
        {
            new UserAssetRoleElement
            {
                Role = AssetRoleType.Owner,
                AssetId = AssetId
            }
        }
    };

    protected readonly AssetDocument Asset = new()
    {
        Id = AssetId,
        Name = "Choco's car",
        Device = new AssetDeviceElement
        {
            DeviceTypeId = "1",
            SerialNumber = "123456789"
        },
        UserRoles = new List<AssetUserRoleElement>
        {
            new()
            {
                Role = AssetRoleType.Owner,
                UserId = AuthenticatedUserId
            }
        }
    };

    protected static string GetUrl(string path, params KeyValuePair<string, string>[] values)
    {
        return values.Aggregate(path,
            (current, pair) =>
                current.Replace($"{{{pair.Key}}}", pair.Value, StringComparison.InvariantCultureIgnoreCase));
    }
}