using Navtrack.Api.Model.Errors;

namespace Navtrack.Api.Services.Common.Authentication;

public record AuthenticationGrantResult(string? UserId = null, ApiError? Error = null, string? LinkingToken = null);