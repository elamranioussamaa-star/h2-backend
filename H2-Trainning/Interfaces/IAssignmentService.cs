using H2_Trainning.Dtos;

namespace H2_Trainning.Interfaces
{
    public interface IAssignmentService
    {
        Task<List<AssignmentDto>> GetByCoachIdAsync(string coachId);
        Task<List<AssignmentDto>> GetByClientIdAsync(string clientId);
        Task<AssignmentDto> CreateAsync(string coachId, CreateAssignmentDto dto);
        Task<AssignmentDto?> UpdateStatusAsync(int id, UpdateAssignmentStatusDto dto);
        Task<bool> UpdateExerciseLogAsync(int assignmentId, int exerciseId, UpdateExerciseLogDto dto);
        Task<object> GetExerciseWeightHistoryAsync(string clientId, int exerciseId);
    }
}
