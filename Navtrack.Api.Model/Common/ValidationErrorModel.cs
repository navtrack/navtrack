using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Common;

public class ValidationErrorModel : BaseErrorModel
{
    [Required]
    public string PropertyName { get; set; }
}