namespace Navtrack.Library.Services
{
    public interface IMapper
    {
        TDestination Map<TSource, TDestination>(TSource source);
        TDestination Map<TSource1, TSource2, TDestination>(TSource1 source1, TSource2 source2);
        TDestination Map<TSource1, TSource2, TSource3, TDestination>(TSource1 source1, TSource2 source2,
            TSource3 source3);
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
        TDestination Map<TSource1, TSource2, TDestination>(TSource1 source1, TSource2 source2,
            TDestination destination);
        TDestination Map<TSource1, TSource2, TSource3, TDestination>(TSource1 source1, TSource2 source2,
            TSource3 source3, TDestination destination);
    }

    public interface IMapper<in TSource, TDestination>
    {
        TDestination Map(TSource source, TDestination destination);
    }

    public interface IMapper<in TSource1, in TSource2, TDestination>
    {
        TDestination Map(TSource1 source1, TSource2 source2, TDestination destination);
    }

    public interface IMapper<in TSource1, in TSource2, in TSource3, TDestination>
    {
        TDestination Map(TSource1 source1, TSource2 source2, TSource3 source3, TDestination destination);
    }
}