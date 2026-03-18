namespace Navtrack.Api.Services.Common.RequestContext;

public interface INavtrackRequestContextAccessor
{
    NavtrackRequestContext? NavtrackContext { get; set; }
}