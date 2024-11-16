using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Common;

public class List<T>
{
    [Required]
    public IEnumerable<T> Items { get; set; }
}