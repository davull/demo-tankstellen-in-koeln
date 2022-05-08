using System.Text.Json.Serialization;

namespace TIK.Backend.Ports.Dtos;

public class Root
{
    [JsonPropertyName("features")]
    public List<Feature> Features { get; set; } = new();
}