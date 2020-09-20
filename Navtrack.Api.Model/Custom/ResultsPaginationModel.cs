namespace Navtrack.Api.Model.Custom
{
    public class ResultsPaginationModel<T> : ResultsModel<T>
    {
        public ResultsPaginationModel()
        {
            Pagination = new PaginationModel();
        }
        
        public PaginationModel Pagination { get; set; }
    }
}