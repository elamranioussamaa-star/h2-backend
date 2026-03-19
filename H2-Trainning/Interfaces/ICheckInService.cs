using H2_Trainning.Dtos;

namespace H2_Trainning.Interfaces
{
    public interface ICheckInService
    {
        Task<List<CheckInDto>> GetByClientIdAsync(string clientId);
        Task<CheckInDto> CreateAsync(string clientId, CreateCheckInDto dto);
    }
}
