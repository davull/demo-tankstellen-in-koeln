using System.Reflection;
using TIK.Backend.Ports;
using TIK.Backend.Ports.Dtos;

namespace TIK.Backend.Adapters;

public class FileBasedStadtKoelnGasStationsClient : IStadtKoelnGasStationsClient
{
    public async Task<Root> GetGasStations()
    {
        var json = await ReadGasStationsFromResource();
        return System.Text.Json.JsonSerializer.Deserialize<Root>(json)!;
    }

    private static async Task<string> ReadGasStationsFromResource()
    {
        const string resourceName = "TIK.Backend.Assets.gasstations-source.json";
        var assembly = Assembly.GetExecutingAssembly();
        
        await using var stream = assembly.GetManifestResourceStream(name: resourceName);
        using var reader = new StreamReader(stream!);

        return await reader.ReadToEndAsync();
    }
}