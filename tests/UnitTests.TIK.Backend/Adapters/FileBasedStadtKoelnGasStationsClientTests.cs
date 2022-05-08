using FluentAssertions;
using TIK.Backend.Adapters;
using Xunit;

namespace UnitTests.TIK.Backend.Adapters;

public class FileBasedStadtKoelnGasStationsClientTests
{
    [Fact]
    public async Task GetGasStations_ReturnsAllGasStations()
    {
        // Arrange
        var sut = new FileBasedStadtKoelnGasStationsClient();
        
        // Act
        var gasStations = await sut.GetGasStations();
        
        // Assert
        gasStations.Should().NotBeNull();
        gasStations.Features.Should().HaveCount(124);

        gasStations.Features[0].Attributes.Adresse.Should().BeEquivalentTo("Bonner Str. 98 (50677 Neustadt/Süd)");
    }
}