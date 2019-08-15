namespace Navtrack.DataAccess.Repository
{
    public interface IInterceptor
    {
        void OnAdd<T>(T entity) where T : class;
        void OnUpdate<T>(T entity) where T : class;
        void OnDelete<T>(T entity) where T : class;
    }
}