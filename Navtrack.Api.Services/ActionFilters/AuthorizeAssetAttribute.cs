using System;
using Microsoft.AspNetCore.Authorization;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Services.ActionFilters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AuthorizeAssetAttribute : AuthorizeAttribute
{
    public readonly AssetRoleType AssetRoleType;

    public AuthorizeAssetAttribute(AssetRoleType assetRoleType)
    {
        AssetRoleType = assetRoleType;
    }
}