using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Settings;
using Navtrack.DataAccess.Services.Settings;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.Environment;

[Service(typeof(IEnvironmentService))]
public class EnvironmentService : IEnvironmentService
{
    private readonly ISettingDataService settingDataService;

    public EnvironmentService(ISettingDataService settingDataService)
    {
        this.settingDataService = settingDataService;
    }

    public async Task<Dictionary<string, string>> Get()
    {
        List<SettingDocument> settings = await settingDataService.GetAll();

        Dictionary<string, string> dictionary = new();

        foreach (SettingDocument document in settings)
        {
            string key = $"{document.Key}Settings";

            if (PublicEnvironmentVariables.Dictionary.TryGetValue(key, out List<string>? value))
            {
                foreach (string publicKey in value)
                {
                    BsonElement el = document.Value.FirstOrDefault(x => x.Name == publicKey);

                    dictionary[$"{document.Key}.{publicKey}"] = $"{el.Value}";
                }
            }
        }

        return dictionary;
    }
}