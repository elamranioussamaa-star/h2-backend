using H2_Trainning.Dtos;

namespace H2_Trainning.Interfaces
{
    public interface IReservationService
    {
        Task<List<ReservationDto>> GetByClientIdAsync(string clientId);
        Task<List<ReservationDto>> GetByCoachIdAsync(string coachId);
        Task<ReservationDto> BookAsync(string clientId, CreateReservationDto dto);
        Task<bool> CancelAsync(int id, string userId);
        Task<ReservationDto?> ConfirmAsync(int id, string coachId);
        Task<ReservationDto?> RejectAsync(int id, string coachId);
    }
}
