using System;
using Microsoft.AspNetCore.Authorization;
using Navtrack.Database.Model.Assets;

namespace Navtrack.Api.Services.Common.ActionFilters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AuthorizeAssetAttribute(AssetUserRole userRole) : AuthorizeAttribute
{
    public readonly AssetUserRole UserRole = userRole;
}