using System;
using Microsoft.AspNetCore.Authorization;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Model.Teams;

namespace Navtrack.Api.Services.Common.ActionFilters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class NavtrackAuthorizeAttribute : AuthorizeAttribute
{
    public AssetUserRole? AssetUserRole { get; private set; }
    public TeamUserRole? TeamUserRole { get; private set; }
    public OrganizationUserRole? OrganizationUserRole { get; private set; }
    
    public NavtrackAuthorizeAttribute(AssetUserRole userRole)
    {
        AssetUserRole = userRole;
    }
    
    public NavtrackAuthorizeAttribute(TeamUserRole userRole)
    {
        TeamUserRole = userRole;
    }
    
    public NavtrackAuthorizeAttribute(OrganizationUserRole userRole)
    {
        OrganizationUserRole = userRole;
    }
}