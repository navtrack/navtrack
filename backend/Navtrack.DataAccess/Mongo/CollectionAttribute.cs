using System;

namespace Navtrack.DataAccess.Mongo;

[AttributeUsage(AttributeTargets.Class)]
public class CollectionAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}