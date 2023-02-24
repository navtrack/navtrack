using System.Collections.Generic;
using Navtrack.Api.Services.IdentityServer.Model;
using Navtrack.Common.Settings.Settings;

namespace Navtrack.Api.Services.Environment;

public static class PublicEnvironmentVariables
{
    public static readonly Dictionary<string, List<string>> Dictionary = new()
    {
        {
            nameof(GoogleAuthenticationSettings), new List<string>
            {
                nameof(GoogleAuthenticationSettings.ClientId)
            }
        },
        {
            nameof(MicrosoftAuthenticationSettings),
            new List<string>
            {
                nameof(MicrosoftAuthenticationSettings.ClientId),
                nameof(MicrosoftAuthenticationSettings.Authority)
            }
        },
        {
            nameof(AppleAuthenticationSettings),
            new List<string>
            {
                nameof(AppleAuthenticationSettings.ClientId),
                nameof(AppleAuthenticationSettings.RedirectUri)
            }
        },
        {
            nameof(MapSettings),
            new List<string>
            {
                nameof(MapSettings.TileUrl)
            }
        },
        {
            nameof(SentrySettings),
            new List<string>
            {
                nameof(SentrySettings.Dsn)
            }
        }
    };
}