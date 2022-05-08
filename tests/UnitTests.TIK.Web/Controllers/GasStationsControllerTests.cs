using FluentAssertions;
using NSubstitute;
using TIK.Backend.Application.Domain;
using TIK.Backend.Application.Services;
using TIK.Backend.Web.Controllers;
using Xunit;

namespace UnitTests.TIK.Web.Controllers;

public class GasStationsControllerTests
{
    private readonly IGasStationsService _gasStationsServiceMock;
    private readonly GasStationsController _sut;

    public GasStationsControllerTests()
    {
        _gasStationsServiceMock = Substitute.For<IGasStationsService>();
        
        _sut = new GasStationsController(
            gasStationsService: _gasStationsServiceMock);
    }

    [Fact]
    public async Task Find_returnsAllGasStations()
    {
        // Arrange
        var mockData = new List<GasStationInfo>
        {
            new(1, new Address(
                "Musterstrasse",
                "12345",
                "Musterstadt",
                49.12345789d,
                9.12345678d))
        };
        _gasStationsServiceMock
            .GetGasStationInfos()
            .ReturnsForAnyArgs(mockData);
        
        // Act
        var result = await _sut.Find();
        
        // Assert
        result.GasStations.Should().NotBeNullOrEmpty();
    }
    
    [Fact]
    public async Task Find_formatLatLong()
    {
        // Arrange
        var mockData = new List<GasStationInfo>
        {
            new(1, new Address(
                "Musterstrasse",
                "12345",
                "Musterstadt",
                49.12345789d,
                9.12345678d))
        };
        _gasStationsServiceMock
            .GetGasStationInfos()
            .ReturnsForAnyArgs(mockData);
        
        // Act
        var result = await _sut.Find();
        
        // Assert
        var gasStation = result.GasStations.First();
        gasStation.Latitude.Should().BeEquivalentTo("49.12345");
        gasStation.Longitude.Should().BeEquivalentTo("9.12345");
    }
}