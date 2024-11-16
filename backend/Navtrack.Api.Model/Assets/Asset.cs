using System.ComponentModel.DataAnnotations;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Model.Messages;

namespace Navtrack.Api.Model.Assets;

public class Asset
{
    [Required]
    public string Id { get; set; }

    [Required]
    public string Name { get; set; }
    
    [Required]
    public string OrganizationId { get; set; }

    [Required]
    public bool Online { get; set; }

    [Required]
    public int MaxSpeed => 400; // TODO update this property

    public Message? LastMessage { get; set; }
    public Message? LastPositionMessage { get; set; }

    public Device? Device { get; set; }
}