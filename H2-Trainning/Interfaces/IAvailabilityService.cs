using H2_Trainning.Dtos;

namespace H2_Trainning.Interfaces
{
    public interface IAvailabilityService
    {
        Task<List<SlotDto>> GetByCoachIdAsync(string coachId);
        Task<List<SlotDto>> GetAllAvailableAsync();
        Task<SlotDto> CreateAsync(string coachId, CreateSlotDto dto);
        Task<bool> DeleteAsync(int id, string coachId);
    }
}
