namespace Navtrack.Api.Model.Custom
{
    public class ResultsPaginationResponseModel<T> : ResultsResponseModel<T>
    {
        public ResultsPaginationResponseModel()
        {
            Pagination = new PaginationResponseModel();
        }
        
        public PaginationResponseModel Pagination { get; set; }
    }
}