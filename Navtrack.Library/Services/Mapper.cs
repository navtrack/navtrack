using System;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Library.DI;

namespace Navtrack.Library.Services
{
    [Service(typeof(IMapper))]
    public class Mapper : IMapper
    {
        private readonly IServiceProvider serviceProvider;

        public Mapper(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            TDestination destination = CreateInstance<TDestination>();

            return Map(source, destination);
        }

        public TDestination Map<TSource1, TSource2, TDestination>(TSource1 source1, TSource2 source2)
        {
            TDestination destination = CreateInstance<TDestination>();

            return Map(source1, source2, destination);
        }

        public TDestination Map<TSource1, TSource2, TSource3, TDestination>(TSource1 source1, TSource2 source2,
            TSource3 source3)
        {
            TDestination destination = CreateInstance<TDestination>();

            return Map(source1, source2, source3, destination);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            IMapper<TSource, TDestination>
                mapper = serviceProvider.GetRequiredService<IMapper<TSource, TDestination>>();

            return mapper.Map(source, destination);
        }

        public TDestination Map<TSource1, TSource2, TDestination>(TSource1 source1, TSource2 source2,
            TDestination destination)
        {
            IMapper<TSource1, TSource2, TDestination> mapper =
                serviceProvider.GetRequiredService<IMapper<TSource1, TSource2, TDestination>>();

            return mapper.Map(source1, source2, destination);
        }

        public TDestination Map<TSource1, TSource2, TSource3, TDestination>(TSource1 source1, TSource2 source2,
            TSource3 source3, TDestination destination)
        {
            IMapper<TSource1, TSource2, TSource3, TDestination> mapper =
                serviceProvider.GetRequiredService<IMapper<TSource1, TSource2, TSource3, TDestination>>();

            return mapper.Map(source1, source2, source3, destination);
        }

        private static TDestination CreateInstance<TDestination>()
        {
            Type type = typeof(TDestination);

            return type.IsInterface || type.IsAbstract || type.GetConstructor(Type.EmptyTypes) == null
                ? default
                : Activator.CreateInstance<TDestination>();
        }
    }
}