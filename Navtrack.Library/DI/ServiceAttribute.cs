using System;

namespace Navtrack.Library.DI
{
    public class ServiceAttribute : Attribute
    {
        public ServiceAttribute(Type type)
        {
            Type = type;
        }

        public Type Type { get; }
    }
}