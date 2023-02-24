using System.Collections.Generic;

namespace Navtrack.Api.Model.Common;

public class ErrorModel : BaseErrorModel
{
    public List<ValidationErrorModel>? ValidationErrors { get; set; }
}