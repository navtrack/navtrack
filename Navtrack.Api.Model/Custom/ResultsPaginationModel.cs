namespace Navtrack.Api.Model.Custom
{
    public class ResultsPaginationModel<T> : ResultsModel<T>
    {
        public PaginationModel Pagination { get; set; }
    }
}