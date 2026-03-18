using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Common.RequestContext;

[Service(typeof(INavtrackRequestContextAccessor))]
public class NavtrackRequestContextAccessor : INavtrackRequestContextAccessor
{
    public NavtrackRequestContext? NavtrackContext { get; set; }
}