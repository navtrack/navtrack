using System;
using System.Reflection;

namespace Navtrack.DataAccess.Mongo;

public static class MongoUtils
{
    public static string GetCollectionName<T>() where T : class
    {
        return typeof(T).GetCustomAttribute(typeof(CollectionAttribute)) is CollectionAttribute collectionAttribute
            ? collectionAttribute.Name
            : throw new ArgumentException($"{typeof(T).Name} does not have a Collection set.");
    }
}