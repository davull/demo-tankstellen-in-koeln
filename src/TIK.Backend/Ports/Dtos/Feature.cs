using System.Text.Json.Serialization;

namespace TIK.Backend.Ports.Dtos;

public class Feature
{
    [JsonPropertyName("attributes")]
    public Attributes Attributes { get; set; } = null!;

    [JsonPropertyName("geometry")]
    public Geometry Geometry { get; set; } = null!;
}