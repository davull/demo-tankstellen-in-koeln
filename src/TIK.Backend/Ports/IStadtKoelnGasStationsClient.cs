using TIK.Backend.Ports.Dtos;

namespace TIK.Backend.Ports;

public interface IStadtKoelnGasStationsClient
{
    Task<Root> GetGasStations();
}