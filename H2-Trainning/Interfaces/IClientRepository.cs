using H2_Trainning.Models;

namespace H2_Trainning.Interfaces
{
    public interface IClientRepository
    {
        Task<List<AppUser>> GetClientsByCoachIdAsync(string coachId);
        Task<List<AppUser>> SearchClientsAsync(string coachId, string query);
        Task<AppUser?> GetByIdAsync(string id);
        Task<bool> DeleteAsync(string clientId, string coachId);
    }
}
