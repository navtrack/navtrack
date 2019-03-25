using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Navtrack.Library.DI
{
    public class Bootstrapper
    {
        private ServiceProvider serviceProvider;

        public void Initialise()
        {
            ServiceCollection serviceCollection = new ServiceCollection();

            RegisterTypes(serviceCollection);

            serviceProvider = serviceCollection.BuildServiceProvider();
        }

        public T GetService<T>()
        {
            return serviceProvider.GetService<T>();
        }

        private static void RegisterTypes(IServiceCollection serviceCollection)
        {
            IEnumerable<Assembly> assemblies = GetAssemblies();
            
            List<KeyValuePair<Type, ServiceAttribute>> serviceAttributes = assemblies
                .SelectMany(x => x.DefinedTypes)
                .SelectMany(x =>
                    x.GetCustomAttributes<ServiceAttribute>()
                        .Select(y => new KeyValuePair<Type, ServiceAttribute>(x, y)))
                .ToList();

            foreach (KeyValuePair<Type, ServiceAttribute> serviceAttribute in serviceAttributes)
            {
                serviceCollection.AddScoped(serviceAttribute.Value.Type, serviceAttribute.Key);
            }
        }

        private static IEnumerable<Assembly> GetAssemblies()
        {
            List<Assembly> assemblies = new List<Assembly>
            {
                Assembly.GetEntryAssembly()
            };

            assemblies.AddRange(Assembly.GetEntryAssembly().GetReferencedAssemblies().Select(Assembly.Load));

            return assemblies;
        }
    }
}