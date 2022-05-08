using System.Text.RegularExpressions;
using TIK.Backend.Application.Domain;
using TIK.Backend.Ports;
using TIK.Backend.Ports.Dtos;

namespace TIK.Backend.Application.Services;

public class GasStationsService : IGasStationsService
{
    private readonly IStadtKoelnGasStationsClient _client;

    private const string AddressPattern = @"^(.+) \((.+) (.+)\)$";
    private static readonly Regex AddressRegex = new(AddressPattern);

    public GasStationsService(IStadtKoelnGasStationsClient client)
    {
        _client = client;
    }

    public async Task<ICollection<GasStationInfo>> GetGasStationInfos(
        string? filterString = null,
        Sorting? sorting = null,
        Paging? paging = null)
    {
        var gasStations = (await _client.GetGasStations()).Features
            .Select(TransformFeature);

        // Filter
        if (!string.IsNullOrEmpty(filterString))
            gasStations = ApplyFilter(gasStations, filterString);

        // Sorting by Id, Street, ZipCode or City
        if (sorting is not null)
            gasStations = ApplySorting(gasStations, sorting);

        return gasStations.ToList();
    }

    private static GasStationInfo TransformFeature(Feature feature)
    {
        var id = feature.Attributes.Objectid;

        var (street, zipCode, city) = ParseAddress(feature.Attributes.Adresse);
        var address = new Address(
            Street: street,
            ZipCode: zipCode,
            City: city,
            Latitude: feature.Geometry.Y,
            Longitude: feature.Geometry.X);

        return new GasStationInfo(Id: id, Address: address);
    }

    private static (string, string, string) ParseAddress(string address)
    {
        // address is something like "Bonner Str. 98 (50677 Neustadt/Süd)"
        var match = AddressRegex.Match(address);

        var street = match.Groups[1].Value;
        var zipCode = match.Groups[2].Value;
        var city = match.Groups[3].Value;

        return (street, zipCode, city);
    }

    private static IEnumerable<GasStationInfo> ApplyFilter(
        IEnumerable<GasStationInfo> gasStationInfos, string filterString)
    {
        var where = new Func<GasStationInfo, bool>(g =>
            g.Address.Street.Contains(filterString, StringComparison.InvariantCultureIgnoreCase) ||
            g.Address.City.Contains(filterString, StringComparison.InvariantCultureIgnoreCase) ||
            g.Address.ZipCode.Contains(filterString, StringComparison.InvariantCultureIgnoreCase));

        return gasStationInfos.Where(where);
    }

    private static IEnumerable<GasStationInfo> ApplySorting(
        IEnumerable<GasStationInfo> gasStationInfos, Sorting sorting)
    {
        return sorting.Field switch
        {
            "Id" when sorting.Ascending => gasStationInfos.OrderBy(g => g.Id),
            "Id" => gasStationInfos.OrderByDescending(g => g.Id),
            
            "Street" when sorting.Ascending => gasStationInfos.OrderBy(g => g.Address.Street),
            "Street" => gasStationInfos.OrderByDescending(g => g.Address.Street),
            
            "City" when sorting.Ascending => gasStationInfos.OrderBy(g => g.Address.City),
            "City" => gasStationInfos.OrderByDescending(g => g.Address.City),
            
            "ZipCode" when sorting.Ascending => gasStationInfos.OrderBy(g => g.Address.ZipCode),
            "ZipCode" => gasStationInfos.OrderByDescending(g => g.Address.ZipCode),
            
            _ => gasStationInfos
        };
    }
}