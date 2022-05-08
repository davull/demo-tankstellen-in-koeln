namespace TIK.Backend.Application.Services;

public interface IGasStationsService
{
    Task<ICollection<Domain.GasStationInfo>> GetGasStationInfos(
        string? filterString = null, 
        Sorting? sorting = null, 
        Paging? paging = null);
}