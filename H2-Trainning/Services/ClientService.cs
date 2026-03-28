using H2_Trainning.Dtos;
using H2_Trainning.Enums;
using H2_Trainning.Interfaces;
using H2_Trainning.Models;
using Microsoft.AspNetCore.Identity;

namespace H2_Trainning.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repo;
        private readonly UserManager<AppUser> _userManager;

        public ClientService(IClientRepository repo, UserManager<AppUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        public async Task<List<UserDto>> GetClientsByCoachIdAsync(string coachId)
        {
            var clients = await _repo.GetClientsByCoachIdAsync(coachId);
            return clients.Select(MapToDto).ToList();
        }

        public async Task<List<UserDto>> SearchClientsAsync(string coachId, string query)
        {
            var clients = await _repo.SearchClientsAsync(coachId, query);
            return clients.Select(MapToDto).ToList();
        }

        public async Task<UserDto> CreateClientAsync(string coachId, CreateClientDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new Exception("A user with this email already exists.");

            var client = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                Role = Role.Client,
                Goal = dto.Goal,
                CoachId = coachId,
                IsApproved = true // Coach-created clients are auto-approved
            };

            var result = await _userManager.CreateAsync(client, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create client: {errors}");
            }

            return MapToDto(client);
        }

        public async Task<UserDto?> GetByIdAsync(string id)
        {
            var user = await _repo.GetByIdAsync(id);
            return user == null ? null : MapToDto(user);
        }

        public async Task<bool> DeleteClientAsync(string clientId, string coachId)
        {
            return await _repo.DeleteAsync(clientId, coachId);
        }

        public async Task<List<UserDto>> GetPendingClientsAsync()
        {
            var clients = await _repo.GetPendingClientsAsync();
            return clients.Select(MapToDto).ToList();
        }

        public async Task<UserDto> ApproveClientAsync(string clientId, string coachId)
        {
            var user = await _repo.GetByIdAsync(clientId);
            if (user == null || user.Role != Role.Client || user.IsApproved)
                throw new Exception("Pending client not found.");

            user.IsApproved = true;
            user.CoachId = coachId;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to approve client: {errors}");
            }

            return MapToDto(user);
        }

        public async Task<bool> RejectClientAsync(string clientId)
        {
            return await _repo.DeletePendingAsync(clientId);
        }

        private static UserDto MapToDto(AppUser u) => new()
        {
            Id = u.Id,
            FullName = u.FullName,
            Email = u.Email!,
            Role = u.Role.ToString(),
            Goal = u.Goal,
            IsApproved = u.IsApproved
        };
    }
}
