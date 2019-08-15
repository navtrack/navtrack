namespace Navtrack.DataAccess.Repository
{
    public class BaseInterceptor : IInterceptor
    {
        public virtual void OnAdd<T>(T entity) where T : class
        {
        }

        public virtual void OnUpdate<T>(T entity) where T : class
        {
        }

        public virtual void OnDelete<T>(T entity) where T : class
        {
        }
    }
}