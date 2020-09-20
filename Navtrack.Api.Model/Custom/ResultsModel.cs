using System.Collections.Generic;

namespace Navtrack.Api.Model.Custom
{
    public class ResultsModel<T>
    {
        public IEnumerable<T> Results { get; set; }
    }
}