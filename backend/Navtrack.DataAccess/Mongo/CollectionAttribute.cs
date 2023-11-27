using System;

namespace Navtrack.DataAccess.Mongo;

[AttributeUsage(AttributeTargets.Class)]
public class CollectionAttribute : Attribute
{
    public string Name { get; }

    public CollectionAttribute(string name)
    {
        Name = name;
    }
}