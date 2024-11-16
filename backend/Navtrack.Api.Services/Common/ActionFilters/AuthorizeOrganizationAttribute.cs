using System;
using Microsoft.AspNetCore.Authorization;
using Navtrack.DataAccess.Model.Organizations;

namespace Navtrack.Api.Services.Common.ActionFilters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AuthorizeOrganizationAttribute(OrganizationUserRole userRole) : AuthorizeAttribute
{
    public readonly OrganizationUserRole UserRole = userRole;
}