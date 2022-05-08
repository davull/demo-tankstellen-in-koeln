using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using TIK.Backend.Application.Domain;
using TIK.Backend.Application.Services;

namespace TIK.Backend.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GasStationsController : ControllerBase
{
    private readonly NumberFormatInfo _nfi;
    private const string LatLongFormat = "F5";
    
    private readonly IGasStationsService _gasStationsService;

    public GasStationsController(IGasStationsService gasStationsService)
    {
        _gasStationsService = gasStationsService;

        _nfi = new NumberFormatInfo
        {
            NumberDecimalSeparator = "."
        };
    }

    [HttpGet]
    public async Task<Dtos.GasStationsResponse> Find(
        string? filter = null, 
        string? sortField = null,
        string? sortOrder = null)
    {
        var gasStationInfos = await _gasStationsService.GetGasStationInfos(
            filterString: filter,
            sorting: MapSortingFromQuery(sortField: sortField, sortOrder: sortOrder));
        
        return new Dtos.GasStationsResponse
        {
            GasStations = gasStationInfos
                .Select(MapToGasStationDto)
                .ToList()
        };
    }

    private Dtos.GasStation MapToGasStationDto(GasStationInfo gasStationInfo)
    {
        // Truncate lat/long to prevent rounding
        var latString = (Math.Truncate(gasStationInfo.Address.Latitude * 100_000) / 100_000)
            .ToString(LatLongFormat, _nfi);
        var longString = (Math.Truncate(gasStationInfo.Address.Longitude * 100_000) / 100_000)
            .ToString(LatLongFormat, _nfi);

        return new Dtos.GasStation(
            Id: gasStationInfo.Id,
            Street: gasStationInfo.Address.Street,
            ZipCode: gasStationInfo.Address.ZipCode,
            City: gasStationInfo.Address.City,
            Latitude: latString,
            Longitude: longString);
    }

    private static Sorting MapSortingFromQuery(string? sortField, string? sortOrder)
    {
        // Default sorting order
        if (string.IsNullOrWhiteSpace(sortField))
            return new Sorting(Field: "Street", Ascending: true);

        return new Sorting(
            Field: sortField,
            Ascending: !"desc".Equals(sortOrder, StringComparison.InvariantCultureIgnoreCase));
    }
}