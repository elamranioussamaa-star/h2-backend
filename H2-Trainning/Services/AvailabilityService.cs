using H2_Trainning.Dtos;
using H2_Trainning.Interfaces;
using H2_Trainning.Models;

namespace H2_Trainning.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly IAvailabilityRepository _repo;

        public AvailabilityService(IAvailabilityRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<SlotDto>> GetByCoachIdAsync(string coachId)
        {
            var slots = await _repo.GetByCoachIdAsync(coachId);
            return slots.Select(MapToDto).ToList();
        }

        public async Task<List<SlotDto>> GetAllAvailableAsync()
        {
            var slots = await _repo.GetAllAvailableAsync();
            return slots.Select(MapToDto).ToList();
        }

        public async Task<SlotDto> CreateAsync(string coachId, CreateSlotDto dto)
        {
            var slot = new AvailabilitySlot
            {
                CoachId = coachId,
                Date = DateOnly.Parse(dto.Date),
                StartTime = TimeOnly.Parse(dto.StartTime),
                EndTime = TimeOnly.Parse(dto.EndTime),
                IsBooked = false
            };

            var created = await _repo.CreateAsync(slot);
            return MapToDto(created);
        }

        public async Task<bool> DeleteAsync(int id, string coachId)
        {
            var slot = await _repo.GetByIdAsync(id);
            if (slot == null || slot.CoachId != coachId) return false;
            return await _repo.DeleteAsync(id);
        }

        private static SlotDto MapToDto(AvailabilitySlot s) => new()
        {
            Id = s.Id,
            CoachId = s.CoachId,
            Date = s.Date.ToString("yyyy-MM-dd"),
            StartTime = s.StartTime.ToString("HH:mm"),
            EndTime = s.EndTime.ToString("HH:mm"),
            IsBooked = s.IsBooked
        };
    }
}
