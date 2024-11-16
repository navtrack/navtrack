namespace Navtrack.Api.Services.Common.Context;

public interface INavtrackContextAccessor
{
    NavtrackContext? NavtrackContext { get; set; }
}