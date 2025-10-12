using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Tests.Helpers;

namespace Navtrack.Api.Tests;

public class AssetsTests(BaseTestFixture fixture) : BaseApiTest(fixture)
{
    [Fact]
    public async Task GetById_AssetAndUserInDatabase_ReturnsOk()
    {
        HttpResponseMessage response =
            await HttpClient.GetAsync(
                GetUrl(ApiPaths.AssetById,
                    new KeyValuePair<string, string>("assetId", DatabaseSeed.Asset.Id.ToString())));

        AssetModel? asset = await response.Content.ReadFromJsonAsync<AssetModel>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(asset);
        Assert.Equal(DatabaseSeed.Asset.Id.ToString(), asset.Id);
    }

    [Fact]
    public async Task GetById_RandomAssetId_ReturnsNotFound()
    {
        HttpResponseMessage response =
            await HttpClient.GetAsync(
                GetUrl(ApiPaths.AssetById, new KeyValuePair<string, string>("assetId", Guid.NewGuid().ToString())));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}