using System;
using Microsoft.Extensions.DependencyInjection;

namespace Navtrack.Shared.Library.DI;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ServiceAttribute(Type type, ServiceLifetime serviceLifetime) : Attribute
{
    public ServiceAttribute(Type type) : this(type, ServiceLifetime.Scoped)
    {
    }

    public Type Type { get; } = type;
    public ServiceLifetime ServiceLifetime { get; } = serviceLifetime;
}