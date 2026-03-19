using H2_Trainning.Models;

namespace H2_Trainning.Interfaces
{
    public interface IAvailabilityRepository
    {
        Task<List<AvailabilitySlot>> GetByCoachIdAsync(string coachId);
        Task<AvailabilitySlot?> GetByIdAsync(int id);
        Task<AvailabilitySlot> CreateAsync(AvailabilitySlot slot);
        Task<bool> DeleteAsync(int id);
        Task<List<AvailabilitySlot>> GetAllAvailableAsync();
    }
}
