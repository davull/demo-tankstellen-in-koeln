using System.Text.Json.Serialization;

namespace TIK.Backend.Ports.Dtos;

public class Attributes
{
    [JsonPropertyName("objectid")]
    public int Objectid { get; set; }

    [JsonPropertyName("adresse")]
    public string Adresse { get; set; } = string.Empty;

    public override string ToString() => $"{nameof(Objectid)}: {Objectid}, {nameof(Adresse)}: {Adresse}";
}