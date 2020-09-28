namespace Navtrack.Api.Model.Custom
{
    public class BaseCommand
    {
        public int UserId { get; set; }
    }
    
    public class BaseCommand<T> : BaseCommand
    {
        public T Model { get; set; }
    }
}