using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Account;

public class ResetPasswordModel : BasePasswordModel
{
    [Required(ErrorMessage = "id.required")]
    public string Id { get; set; }
}