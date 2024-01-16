using System;
using Microsoft.Extensions.DependencyInjection;

namespace Navtrack.Shared.Library.DI;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ServiceAttribute(Type type, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) : Attribute
{
    public Type Type { get; } = type;
    public ServiceLifetime ServiceLifetime { get; } = serviceLifetime;
}