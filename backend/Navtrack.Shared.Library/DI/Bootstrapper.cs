using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using static System.String;

namespace Navtrack.Library.DI;

public static class Bootstrapper
{
    public static void ConfigureServices<TEntry>(IServiceCollection serviceCollection)
    {
        IEnumerable<Assembly> assemblies = GetAssemblies<TEntry>();

        List<KeyValuePair<Type, ServiceAttribute>> serviceAttributes = assemblies
            .SelectMany(x => x.DefinedTypes)
            .SelectMany(x =>
                x.GetCustomAttributes<ServiceAttribute>(false)
                    .Select(y => new KeyValuePair<Type, ServiceAttribute>(x, y)))
            .ToList();

        foreach ((Type type, ServiceAttribute serviceAttribute) in serviceAttributes)
        {
            serviceCollection.Add(new ServiceDescriptor(serviceAttribute.Type, type,
                serviceAttribute.ServiceLifetime));
        }
    }

    private static IEnumerable<Assembly> GetAssemblies<T>()
    {
        List<Assembly> assemblies = GetAssemblies(typeof(T).Assembly)
            .Distinct()
            .ToList();

        return assemblies;
    }

    private static IEnumerable<Assembly> GetAssemblies(Assembly assembly)
    {
        string namespacePrefix = typeof(Bootstrapper).Namespace?.Split(".")[0];

        foreach (Assembly referencedAssembly in assembly.GetReferencedAssemblies()
                     .Where(x => x.Name != null && x.Name.StartsWith(namespacePrefix ?? Empty)).Select(Assembly.Load)
                     .SelectMany(GetAssemblies))
        {
            yield return referencedAssembly;
        }

        yield return assembly;
    }
}