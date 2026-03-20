using H2_Trainning.Data;
using H2_Trainning.Interfaces;
using H2_Trainning.Models;
using Microsoft.EntityFrameworkCore;

namespace H2_Trainning.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly ApplicationDbContext _context;

        public AssignmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Assignment>> GetByCoachIdAsync(string coachId)
        {
            return await _context.Assignments
                .Include(a => a.Client)
                .Include(a => a.Program)
                .Where(a => a.Program.CoachId == coachId)
                .OrderByDescending(a => a.AssignedDate)
                .ToListAsync();
        }

        public async Task<List<Assignment>> GetRecentByCoachIdAsync(string coachId, int count)
        {
            return await _context.Assignments
                .Include(a => a.Client)
                .Include(a => a.Program)
                .Where(a => a.Program.CoachId == coachId)
                .OrderByDescending(a => a.AssignedDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Assignment>> GetByClientIdAsync(string clientId)
        {
            return await _context.Assignments
                .Include(a => a.Client)
                .Include(a => a.Program)
                .Where(a => a.ClientId == clientId)
                .OrderByDescending(a => a.AssignedDate)
                .ToListAsync();
        }

        public async Task<Assignment?> GetLatestByClientIdAsync(string clientId)
        {
            return await _context.Assignments
                .Include(a => a.Client)
                .Include(a => a.ExerciseLogs)
                .Include(a => a.Program)
                    .ThenInclude(p => p.Days.OrderBy(d => d.DayNumber))
                        .ThenInclude(d => d.Exercises.OrderBy(e => e.SortOrder))
                .Include(a => a.Program)
                    .ThenInclude(p => p.Days.OrderBy(d => d.DayNumber))
                        .ThenInclude(d => d.Meals.OrderBy(m => m.SortOrder))
                .Where(a => a.ClientId == clientId)
                .OrderByDescending(a => a.AssignedDate)
                .AsSplitQuery()
                .FirstOrDefaultAsync();
        }

        public async Task<Assignment?> GetByIdAsync(int id)
        {
            return await _context.Assignments
                .Include(a => a.Client)
                .Include(a => a.Program)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Assignment> CreateAsync(Assignment assignment)
        {
            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();
            return assignment;
        }

        public async Task<Assignment> UpdateAsync(Assignment assignment)
        {
            _context.Assignments.Update(assignment);
            await _context.SaveChangesAsync();
            return assignment;
        }

        public async Task<ExerciseLog?> GetExerciseLogAsync(int assignmentId, int exerciseId)
        {
            return await _context.ExerciseLogs
                .FirstOrDefaultAsync(el => el.AssignmentId == assignmentId && el.ExerciseId == exerciseId);
        }

        public async Task<ExerciseLog> AddExerciseLogAsync(ExerciseLog log)
        {
            _context.ExerciseLogs.Add(log);
            await _context.SaveChangesAsync();
            return log;
        }

        public async Task<ExerciseLog> UpdateExerciseLogAsync(ExerciseLog log)
        {
            _context.ExerciseLogs.Update(log);
            await _context.SaveChangesAsync();
            return log;
        }

        public async Task<List<ExerciseLog>> GetExerciseWeightHistoryAsync(string clientId, int exerciseId)
        {
            return await _context.ExerciseLogs
                .Include(el => el.Assignment)
                .Where(el => el.Assignment.ClientId == clientId && el.ExerciseId == exerciseId && el.Weight.HasValue)
                .OrderBy(el => el.Assignment.AssignedDate)
                .ToListAsync();
        }
    }
}
