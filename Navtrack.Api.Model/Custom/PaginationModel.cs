namespace Navtrack.Api.Model.Custom
{
    public class PaginationModel
    {
        public int TotalResults { get; set; }
        public int Page { get; set; }
        public int MaxPage { get; set; }
        public int PerPage { get; set; }
    }
}