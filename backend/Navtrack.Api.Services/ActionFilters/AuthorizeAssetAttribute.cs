using System;
using Microsoft.AspNetCore.Authorization;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Services.ActionFilters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AuthorizeAssetAttribute(AssetRoleType assetRoleType) : AuthorizeAttribute
{
    public readonly AssetRoleType AssetRoleType = assetRoleType;
}