using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Navtrack.Api.Shared;

/// <summary>
/// Discovers controllers from a list of <see cref="ApplicationPart"/> instances.
/// </summary>
public sealed class CustomControllerFeatureProvider : ControllerFeatureProvider
{
    public new void PopulateFeature(
        IEnumerable<ApplicationPart> parts,
        ControllerFeature feature)
    {
        foreach (IApplicationPartTypeProvider part in parts.OfType<IApplicationPartTypeProvider>())
        {
            foreach (TypeInfo type in part.Types)
            {
                if (IsController(type) && !feature.Controllers.Contains(type) && feature.Controllers.All(x => x.Name != type.Name))
                {
                    feature.Controllers.Add(type);
                }
            }
        }
    }
}