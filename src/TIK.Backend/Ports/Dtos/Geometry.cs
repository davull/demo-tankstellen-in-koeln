using System.Text.Json.Serialization;

namespace TIK.Backend.Ports.Dtos;

public class Geometry
{
    [JsonPropertyName("x")]
    public double X { get; set; }

    [JsonPropertyName("y")]
    public double Y { get; set; }

    public override string ToString() => $"{nameof(X)}: {X}, {nameof(Y)}: {Y}";
}