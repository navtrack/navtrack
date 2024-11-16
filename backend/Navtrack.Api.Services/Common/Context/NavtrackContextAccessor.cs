using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Common.Context;

[Service(typeof(INavtrackContextAccessor))]
public class NavtrackContextAccessor : INavtrackContextAccessor
{
    public NavtrackContext? NavtrackContext { get; set; }
}