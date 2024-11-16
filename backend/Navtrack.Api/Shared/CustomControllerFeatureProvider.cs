using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Navtrack.Api.Shared;

/// <summary>
/// Discovers controllers from a list of <see cref="ApplicationPart"/> instances
/// and filters out controllers with the same name.
/// </summary>
public class CustomControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    private const string ControllerTypeNameSuffix = "Controller";

    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        foreach (IApplicationPartTypeProvider part in parts.OfType<IApplicationPartTypeProvider>())
        {
            foreach (TypeInfo type in part.Types)
            {
                if (IsController(type) && !feature.Controllers.Contains(type) &&
                    feature.Controllers.All(x => x.Name != type.Name))
                {
                    feature.Controllers.Add(type);
                }
            }
        }
    }

    private static bool IsController(TypeInfo typeInfo)
    {
        if (!typeInfo.IsClass)
        {
            return false;
        }

        if (typeInfo.IsAbstract)
        {
            return false;
        }

        // We only consider public top-level classes as controllers. IsPublic returns false for nested
        // classes, regardless of visibility modifiers
        if (!typeInfo.IsPublic)
        {
            return false;
        }

        if (typeInfo.ContainsGenericParameters)
        {
            return false;
        }

        if (typeInfo.IsDefined(typeof(NonControllerAttribute)))
        {
            return false;
        }

        if (!typeInfo.Name.EndsWith(ControllerTypeNameSuffix, StringComparison.OrdinalIgnoreCase) &&
            !typeInfo.IsDefined(typeof(ControllerAttribute)))
        {
            return false;
        }

        return true;
    }
}