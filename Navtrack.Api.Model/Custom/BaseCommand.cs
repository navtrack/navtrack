namespace Navtrack.Api.Model.Custom
{
    public class BaseCommand
    {
        public int UserId { get; set; }
    }
    
    public class BaseCommand<TRequestModel> : BaseCommand
    {
        public TRequestModel Model { get; set; }
    }
}