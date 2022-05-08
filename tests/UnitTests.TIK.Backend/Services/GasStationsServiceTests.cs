using FluentAssertions;
using TIK.Backend.Adapters;
using TIK.Backend.Application.Domain;
using TIK.Backend.Application.Services;
using Xunit;

namespace UnitTests.TIK.Backend.Services;

public class GasStationsServiceTests
{
    private readonly GasStationsService _sut;

    public GasStationsServiceTests()
    {
        _sut = new GasStationsService(
            client: new FileBasedStadtKoelnGasStationsClient());
    }

    [Fact]
    public async Task GetAllGasStations()
    {
        // Act
        var gasStations = await _sut.GetGasStationInfos();
        
        // Assert
        gasStations.Should().NotBeNullOrEmpty();
        gasStations.Should().HaveCount(124);
    }

    [Fact]
    public async Task GetGasStations_FeatureIsTransformedIntoGasStationInfo()
    {
        // Act
        var gasStations = await _sut.GetGasStationInfos();
        
        // Assert
        var expected = new GasStationInfo(
            Id: 98,
            Address: new Address(Street: "Bonner Str. 98",
                ZipCode: "50677",
                City: "Neustadt/Süd",
                Latitude: 50.916095041454554,
                Longitude: 6.960644911005172));
        gasStations.First().Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task WithFilter_GetFilteredGasStations()
    {
        // Act
        var gasStations = await _sut.GetGasStationInfos(filterString: "deutz");
        
        // Assert
        gasStations.Should().NotBeNullOrEmpty();
        gasStations.Should().HaveCount(4);
    }

    [Fact]
    public async Task WithSortingDesc_GetSortedGasStations()
    {
        // Act
        var gasStations = await _sut.GetGasStationInfos(
            sorting: new Sorting(Field: "Street", Ascending: false));
        
        // Assert
        gasStations.Should().BeInDescendingOrder(g => g.Address.Street);
    }
    
    [Fact]
    public async Task WithSortingAsc_GetSortedGasStations()
    {
        // Act
        var gasStations = await _sut.GetGasStationInfos(
            sorting: new Sorting(Field: "Street", Ascending: true));
        
        // Assert
        gasStations.Should().BeInAscendingOrder(g => g.Address.Street);
    }
}