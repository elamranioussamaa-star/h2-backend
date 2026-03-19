using H2_Trainning.Models;

namespace H2_Trainning.Interfaces
{
    public interface IAssignmentRepository
    {
        Task<List<Assignment>> GetByCoachIdAsync(string coachId);
        Task<List<Assignment>> GetRecentByCoachIdAsync(string coachId, int count);
        Task<List<Assignment>> GetByClientIdAsync(string clientId);
        Task<Assignment?> GetLatestByClientIdAsync(string clientId);
        Task<Assignment?> GetByIdAsync(int id);
        Task<Assignment> CreateAsync(Assignment assignment);
        Task<Assignment> UpdateAsync(Assignment assignment);

        Task<ExerciseLog?> GetExerciseLogAsync(int assignmentId, int exerciseId);
        Task<ExerciseLog> AddExerciseLogAsync(ExerciseLog log);
        Task<ExerciseLog> UpdateExerciseLogAsync(ExerciseLog log);
        Task<List<ExerciseLog>> GetExerciseWeightHistoryAsync(string clientId, int exerciseId);
    }
}
