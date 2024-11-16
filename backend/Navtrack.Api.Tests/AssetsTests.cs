using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Tests.Helpers;

namespace Navtrack.Api.Tests;

public class AssetsTests(BaseTestFixture fixture) : BaseTest(fixture)
{
    [Fact]
    public async Task GetById_AssetAndUserInDatabase_ReturnsOk()
    {
        HttpResponseMessage response =
            await HttpClient.GetAsync(
                GetUrl(ApiPaths.AssetById, new KeyValuePair<string, string>("assetId", Asset.Id.ToString())));

        Asset? asset = await response.Content.ReadFromJsonAsync<Asset>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(asset);
        Assert.Equal(Asset.Id.ToString(), asset.Id);
    }
    
    [Fact]
    public async Task GetById_RandomAssetId_ReturnsNotFound()
    {
        HttpResponseMessage response =
            await HttpClient.GetAsync(
                GetUrl(ApiPaths.AssetById, new KeyValuePair<string, string>("assetId", ObjectId.GenerateNewId().ToString())));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}