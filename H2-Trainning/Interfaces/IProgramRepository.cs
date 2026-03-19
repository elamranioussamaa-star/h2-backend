using H2_Trainning.Models;

namespace H2_Trainning.Interfaces
{
    public interface IProgramRepository
    {
        Task<List<H2_Trainning.Models.Program>> GetByCoachIdAsync(string coachId);
        Task<List<H2_Trainning.Models.Program>> GetSummariesByCoachIdAsync(string coachId);
        Task<H2_Trainning.Models.Program?> GetByIdAsync(int id);
        Task<H2_Trainning.Models.Program> CreateAsync(H2_Trainning.Models.Program program);
        Task<H2_Trainning.Models.Program> UpdateAsync(H2_Trainning.Models.Program program);
        Task<bool> DeleteAsync(int id);
    }
}
