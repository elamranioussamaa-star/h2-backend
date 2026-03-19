using H2_Trainning.Data;
using H2_Trainning.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace H2_Trainning.Repositories
{
    public class ProgramRepository : IProgramRepository
    {
        private readonly ApplicationDbContext _context;

        public ProgramRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<H2_Trainning.Models.Program>> GetByCoachIdAsync(string coachId)
        {
            return await _context.Programs
                .Include(p => p.Exercises.OrderBy(e => e.SortOrder))
                .Include(p => p.Meals.OrderBy(m => m.SortOrder))
                .Where(p => p.CoachId == coachId)
                .OrderByDescending(p => p.UpdatedAt)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<List<H2_Trainning.Models.Program>> GetSummariesByCoachIdAsync(string coachId)
        {
            return await _context.Programs
                .Where(p => p.CoachId == coachId)
                .OrderByDescending(p => p.UpdatedAt)
                .ToListAsync();
        }

        public async Task<H2_Trainning.Models.Program?> GetByIdAsync(int id)
        {
            return await _context.Programs
                .Include(p => p.Exercises.OrderBy(e => e.SortOrder))
                .Include(p => p.Meals.OrderBy(m => m.SortOrder))
                .AsSplitQuery()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<H2_Trainning.Models.Program> CreateAsync(H2_Trainning.Models.Program program)
        {
            _context.Programs.Add(program);
            await _context.SaveChangesAsync();
            return program;
        }

        public async Task<H2_Trainning.Models.Program> UpdateAsync(H2_Trainning.Models.Program program)
        {
            _context.Programs.Update(program);
            await _context.SaveChangesAsync();
            return program;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var program = await _context.Programs
                .Include(p => p.Exercises)
                .Include(p => p.Meals)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (program == null) return false;

            _context.Exercises.RemoveRange(program.Exercises);
            _context.Meals.RemoveRange(program.Meals);
            _context.Programs.Remove(program);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
