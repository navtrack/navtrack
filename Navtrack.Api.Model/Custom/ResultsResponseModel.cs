using System.Collections.Generic;

namespace Navtrack.Api.Model.Custom
{
    public class ResultsResponseModel<T>
    {
        public IEnumerable<T> Results { get; set; }
        
        public int TotalResults { get; set; }
    }
}