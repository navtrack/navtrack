using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Protocols;

public class ProtocolModel
{
    [Required]
    public int Port { get; set; }

    [Required]
    public string Name { get; set; }
}