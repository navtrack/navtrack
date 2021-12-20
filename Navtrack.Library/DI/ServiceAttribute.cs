using System;
using Microsoft.Extensions.DependencyInjection;

namespace Navtrack.Library.DI;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ServiceAttribute : Attribute
{
    public ServiceAttribute(Type type)
    {
        Type = type;
        ServiceLifetime = ServiceLifetime.Scoped;
    }
        
    public ServiceAttribute(Type type, ServiceLifetime serviceLifetime)
    {
        Type = type;
        ServiceLifetime = serviceLifetime;
    }

    public Type Type { get; }
    public ServiceLifetime ServiceLifetime { get; }
}