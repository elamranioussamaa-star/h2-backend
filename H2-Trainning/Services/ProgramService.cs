using H2_Trainning.Dtos;
using H2_Trainning.Interfaces;
using H2_Trainning.Models;

namespace H2_Trainning.Services
{
    public class ProgramService : IProgramService
    {
        private readonly IProgramRepository _repo;

        public ProgramService(IProgramRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ProgramDto>> GetByCoachIdAsync(string coachId)
        {
            var programs = await _repo.GetByCoachIdAsync(coachId);
            return programs.Select(MapToDto).ToList();
        }

        public async Task<List<ProgramSummaryDto>> GetSummariesByCoachIdAsync(string coachId)
        {
            var programs = await _repo.GetSummariesByCoachIdAsync(coachId);
            return programs.Select(p => new ProgramSummaryDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                CoachId = p.CoachId
            }).ToList();
        }

        public async Task<ProgramDto?> GetByIdAsync(int id)
        {
            var program = await _repo.GetByIdAsync(id);
            return program == null ? null : MapToDto(program);
        }

        public async Task<ProgramDto> CreateAsync(string coachId, CreateProgramDto dto)
        {
            var program = new H2_Trainning.Models.Program
            {
                Title = dto.Title,
                Description = dto.Description,
                NutritionalBases = dto.NutritionalBases,
                CoachId = coachId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Days = dto.Days.Select(d => new ProgramDay
                {
                    Name = d.Name,
                    DayNumber = d.DayNumber,
                    IsRestDay = d.IsRestDay,
                    Exercises = d.Exercises.Select((e, i) => new Exercise
                    {
                        Name = e.Name,
                        Sets = e.Sets,
                        Reps = e.Reps,
                        Notes = e.Notes,
                        MediaUrl = e.MediaUrl,
                        MediaType = ParseMediaType(e.MediaType),
                        SortOrder = e.SortOrder > 0 ? e.SortOrder : i
                    }).ToList(),
                    Meals = d.Meals.Select((m, i) => new Meal
                    {
                        Name = m.Name,
                        Macros = m.Macros,
                        Time = m.Time,
                        PhotoUrl = m.PhotoUrl,
                        Ingredients = m.Ingredients,
                        SortOrder = m.SortOrder > 0 ? m.SortOrder : i
                    }).ToList()
                }).ToList()
            };

            var created = await _repo.CreateAsync(program);
            return MapToDto(created);
        }

        public async Task<ProgramDto?> UpdateAsync(int id, string coachId, UpdateProgramDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null || existing.CoachId != coachId) return null;

            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.NutritionalBases = dto.NutritionalBases;
            existing.UpdatedAt = DateTime.UtcNow;

            // Simple update: Clear all days and re-add them 
            // In a production scenario, we'd do a more granular merge, but for this simpler structure:
            existing.Days.Clear();
            foreach (var d in dto.Days)
            {
                existing.Days.Add(new ProgramDay
                {
                    Name = d.Name,
                    DayNumber = d.DayNumber,
                    IsRestDay = d.IsRestDay,
                    ProgramId = id,
                    Exercises = d.Exercises.Select((e, i) => new Exercise
                    {
                        Name = e.Name,
                        Sets = e.Sets,
                        Reps = e.Reps,
                        Notes = e.Notes,
                        MediaUrl = e.MediaUrl,
                        MediaType = ParseMediaType(e.MediaType),
                        SortOrder = e.SortOrder > 0 ? e.SortOrder : i
                    }).ToList(),
                    Meals = d.Meals.Select((m, i) => new Meal
                    {
                        Name = m.Name,
                        Macros = m.Macros,
                        Time = m.Time,
                        PhotoUrl = m.PhotoUrl,
                        Ingredients = m.Ingredients,
                        SortOrder = m.SortOrder > 0 ? m.SortOrder : i
                    }).ToList()
                });
            }

            var updated = await _repo.UpdateAsync(existing);
            return MapToDto(updated);
        }

        public async Task<bool> DeleteAsync(int id, string coachId)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null || existing.CoachId != coachId) return false;
            return await _repo.DeleteAsync(id);
        }

        private static Models.MediaType? ParseMediaType(string? type)
        {
            if (string.IsNullOrEmpty(type)) return null;
            return Enum.TryParse<Models.MediaType>(type, true, out var result) ? result : null;
        }

        private static ProgramDto MapToDto(H2_Trainning.Models.Program p) => new()
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            NutritionalBases = p.NutritionalBases,
            CoachId = p.CoachId,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt,
            Days = p.Days?.Select(d => new ProgramDayDto
            {
                Id = d.Id,
                Name = d.Name,
                DayNumber = d.DayNumber,
                IsRestDay = d.IsRestDay,
                Exercises = d.Exercises?.Select(e => new ExerciseDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Sets = e.Sets,
                    Reps = e.Reps,
                    Notes = e.Notes,
                    MediaUrl = e.MediaUrl,
                    MediaType = e.MediaType?.ToString(),
                    SortOrder = e.SortOrder
                }).ToList() ?? new(),
                Meals = d.Meals?.Select(m => new MealDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Macros = m.Macros,
                    Time = m.Time,
                    PhotoUrl = m.PhotoUrl,
                    Ingredients = m.Ingredients,
                    SortOrder = m.SortOrder
                }).ToList() ?? new()
            }).ToList() ?? new()
        };
    }
}
