using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Common;

public class ListModel<T>
{
    [Required]
    public IEnumerable<T> Items { get; set; }
}