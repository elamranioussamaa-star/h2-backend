using H2_Trainning.Dtos;
using H2_Trainning.Interfaces;
using H2_Trainning.Models;

namespace H2_Trainning.Services
{
    public class CheckInService : ICheckInService
    {
        private readonly ICheckInRepository _repo;

        public CheckInService(ICheckInRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<CheckInDto>> GetByClientIdAsync(string clientId)
        {
            var checkIns = await _repo.GetByClientIdAsync(clientId);
            return checkIns.Select(MapToDto).ToList();
        }

        public async Task<CheckInDto> CreateAsync(string clientId, CreateCheckInDto dto)
        {
            var checkIn = new CheckIn
            {
                ClientId = clientId,
                Weight = dto.Weight,
                SleepQuality = dto.SleepQuality,
                EnergyLevel = dto.EnergyLevel,
                Mood = dto.Mood,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repo.CreateAsync(checkIn);
            return MapToDto(created);
        }

        private static CheckInDto MapToDto(CheckIn c) => new()
        {
            Id = c.Id,
            ClientId = c.ClientId,
            ClientName = c.Client?.FullName ?? "",
            Weight = c.Weight,
            SleepQuality = c.SleepQuality,
            EnergyLevel = c.EnergyLevel,
            Mood = c.Mood,
            Notes = c.Notes,
            CreatedAt = c.CreatedAt
        };
    }
}
