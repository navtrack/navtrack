using System;
using Microsoft.AspNetCore.Authorization;
using Navtrack.DataAccess.Model.Teams;

namespace Navtrack.Api.Services.Common.ActionFilters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AuthorizeTeamAttribute(TeamUserRole userRole) : AuthorizeAttribute
{
    public readonly TeamUserRole UserRole = userRole;
}