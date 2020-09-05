using System.Collections.Generic;

namespace Navtrack.Api.Model
{
    public class TableResponse<T>
    {
        public IEnumerable<T> Results { get; set; }
        public int TotalResults { get; set; }
        public int Page { get; set; }
        public int MaxPage { get; set; }
        public int PerPage { get; set; }
    }
}